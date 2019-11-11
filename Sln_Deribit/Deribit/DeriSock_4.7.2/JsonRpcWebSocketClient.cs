namespace DeriSock
{
    using Converter;
    using Events;
    using Exceptions;
    using Model;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Utils;
    using ClearLog;

    public class JsonRpcWebSocketClient
    {
        private volatile ClientWebSocket _webSocket;
        private readonly CancellationTokenSource _receiveLoopCancellationTokenSource = new CancellationTokenSource();
        private volatile bool _receiveLoopRunning;
        private volatile int _requestId;
        private readonly ConcurrentDictionary<int, TaskInfo> _tasks = new ConcurrentDictionary<int, TaskInfo>();
        private readonly Dictionary<string, SubscriptionEntry> _eventsMap = new Dictionary<string, SubscriptionEntry>();
        private Logger log;
        public Uri EndpointUri { get; }
        public bool ClosedByError { get; private set; }
        public bool ClosedByClient { get; private set; }
        public bool ClosedByHost { get; private set; }
        public bool IsConnected
        {
            get => _webSocket != null && !(ClosedByHost || ClosedByClient || ClosedByError);
        }

        public JsonRpcWebSocketClient(string endpointUri, Logger log)
        {
            this.log = log;
            EndpointUri = new Uri(endpointUri);
        }

        public async Task ConnectAsync()
        {
            if (_webSocket != null)
            {
                throw new WebSocketAlreadyConnectedException();
            }

            log.Info("Connecting to " + EndpointUri);
            _webSocket = new ClientWebSocket();
            log.Info("test1 57");


            //if not using open source: System.Net.WebSockets.Client.Managed, set to Tls12
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;



            log.Info("test2");

            try
            {
                await _webSocket.ConnectAsync(EndpointUri, CancellationToken.None);
                log.Info("test3");
            }
            catch (Exception ex)
            {
                log.Error("Exception during ConnectAsync:" + ex);
                _webSocket.Dispose();
                _webSocket = null;
                throw;
            }

            log.Info("test4");
            _ = Task.Factory.StartNew(ReceiveLoopAsync, _receiveLoopCancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        public async Task DisconnectAsync()
        {
            if (_webSocket == null || _webSocket.State != WebSocketState.Open)
            {
                throw new WebSocketNotConnectedException();
            }

            log.Info("Disconnecting from " + EndpointUri);

            //Shutdown the Receive Thread
            _receiveLoopCancellationTokenSource.Cancel();

            //Wait for the Thread to shutdown gracefully
            while (_receiveLoopRunning)
            {
                Thread.Sleep(1);
            }

            ClosedByClient = true;

            //Close the connection
            if (_webSocket.State == WebSocketState.Open)
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing Connection", CancellationToken.None);
            }

            _webSocket.Dispose();
            _webSocket = null;
        }

        public Task<T> SendAsync<T>(string method, object @params, Converter.IJsonConverter<T> converter)
        {
            Interlocked.CompareExchange(ref _requestId, 0, int.MaxValue);
            var reqId = Interlocked.Increment(ref _requestId);

            var request = new JsonRpcRequest() { jsonrpc = "2.0", id = reqId, method = method, @params = @params };
            var tcs = new TaskCompletionSource<T>();
            var taskInfo = new TypedTaskInfo<T> { Tcs = tcs, id = request.id, Converter = converter };

            log.Info("SendAsync task: " + request);

            _tasks[request.id] = taskInfo;
            var message = JsonConvert.SerializeObject(request);

            log.Info(">>>>> Request >>>>>" + "        " + message);

            var buffer = Encoding.UTF8.GetBytes(message);
            var msgInfo = new MessageInfo { task = taskInfo, message = buffer };

            are.WaitOne();

            _ = _webSocket.SendAsync(new ArraySegment<byte>(msgInfo.message), WebSocketMessageType.Text, true, CancellationToken.None);

            new Thread(() =>
            {
                Thread.Sleep(10);//避免兩筆request間隔太近 會直接被Deribit ban掉
                are.Set();
            }).Start();

            return tcs.Task;
        }

        static AutoResetEvent are = new AutoResetEvent(true);//dave added

        private async Task ReceiveLoopAsync()
        {
            _receiveLoopRunning = true;
            var buffer = new byte[4096];
            var resultMessage = "";
            try
            {
                while (!_receiveLoopCancellationTokenSource.IsCancellationRequested)
                {
                    if (_webSocket == null || _webSocket.State != WebSocketState.Open) continue;

                    try
                    {
                        var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _receiveLoopCancellationTokenSource.Token);
                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            ClosedByHost = true;
                            ClosedByClient = false;
                            log.Info("The host closed the connection");
                            break;
                        }

                        resultMessage = string.Concat(resultMessage, Encoding.UTF8.GetString(buffer, 0, result.Count));
                        if (!result.EndOfMessage)
                        {
                            continue;
                        }

                        _ = Task.Factory.StartNew(
                                                  (msg) =>
                                                  {
                                                      var message = (string)msg;
                                                      try
                                                      {
                                                          log.Info("<<<<< Respond <<<<<" + "        " + message);

                                                          var jObject = (JObject)JsonConvert.DeserializeObject(message);
                                                          if (jObject == null) return;

                                                          if (jObject.ContainsKey("params"))
                                                          {
                                                              var eventRes = jObject.ToObject<EventResponse>();
                                                              eventRes.@params.timestamp = DateTime.Now;
                                                              EventResponseReceived(new EventResponseReceivedEventArgs(message, eventRes));
                                                          }
                                                          else
                                                          {
                                                              var parsedResult = jObject.ToObject<JsonRpcResponse>();
                                                              if (_tasks.TryRemove(parsedResult.id, out var task))
                                                              {
                                                                  MessageResponseReceived(new MessageResponseReceivedEventArgs(message, parsedResult, task));
                                                              }
                                                              else
                                                              {
                                                                  log.Info("ConsumeResponsesLoop cannot resolve task:" + parsedResult);
                                                              }
                                                          }
                                                      }
                                                      catch (Exception ex)
                                                      {
                                                          log.Info("ReceiveLoopAsync Error during parsing task: " + ex);
                                                      }
                                                  }, resultMessage, _receiveLoopCancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);

                        resultMessage = "";
                    }
                    catch (OperationCanceledException)
                    {
                        if (_receiveLoopCancellationTokenSource.IsCancellationRequested)
                        {
                            //ignore - valid cancellation
                            ClosedByClient = true;
                            log.Info("Valid manual Cancellation");
                            return;
                        }

                        throw;
                    }
                    catch (Exception e)
                    {
                        log.Info("error: " + e);
                        ClosedByError = true;
                    }
                }
            }
            finally
            {
                _receiveLoopRunning = false;
            }
            log.Info("Leaving ReceiveLoop");
        }

        protected virtual void MessageResponseReceived(MessageResponseReceivedEventArgs e)
        {
            try
            {
                if (e.Response.error != null)
                {
                    if (e.Response.error.Type == JTokenType.Object)
                    {
                        var error = e.Response.error.ToObject<JsonRpcError>();
                        Task.Factory.StartNew(
                                              () =>
                                              {
                                                  e.TaskInfo.Reject(new InvalidResponseException(e.Response, error, $"(Object) Invalid response for {e.Response.id}, code: {error.code}, message: {error.message}"));
                                                  if (error.data != null && error.data.ToString() != "")
                                                  {
                                                      Task.Factory.StartNew(() => { System.Windows.Forms.MessageBox.Show(e.Response.id + ":" + error.data); });
                                                  }
                                                  else
                                                  {
                                                      Task.Factory.StartNew(() => { System.Windows.Forms.MessageBox.Show(e.Response.id + ":" + error.message); });
                                                  }
                                              }, TaskCreationOptions.LongRunning);
                    }
                    else
                    {
                        Task.Factory.StartNew(
                                              () =>
                                              {
                                                  e.TaskInfo.Reject(new InvalidResponseException(e.Response, null, $"(Non-Object) Invalid response for {e.Response.id}, code: {e.Response.error}"));
                                              }, TaskCreationOptions.LongRunning);
                    }
                }
                else
                {
                    var convertedResult = e.TaskInfo.Convert(e.Response.result);

                    log.Info("MessageResponseReceived task resolve Id: " + e.Response.id);
                    Task.Factory.StartNew(
                                          () =>
                                          {
                                              e.TaskInfo.Resolve(convertedResult);
                                          }, TaskCreationOptions.LongRunning);
                }
            }
            catch (Exception ex)
            {
                log.Error("ConsumeResponsesLoop Error during parsing task:" + ex);
                Task.Factory.StartNew(
                                      () =>
                                      {
                                          e.TaskInfo.Reject(ex);
                                      }, TaskCreationOptions.LongRunning);
            }
        }

        protected virtual void EventResponseReceived(EventResponseReceivedEventArgs e)
        {
            lock (_eventsMap)
            {
                if (e.EventData.method == "heartbeat")
                {
                    log.Info("Hearbeat received: " + e.EventData);
                    if (e.EventData.@params.type == "test_request")
                    {
                        HeartbeatTestRequestReceived();
                    }
                    return;
                }
                if (!_eventsMap.TryGetValue(e.EventData.@params.channel, out var entry))
                {
                    log.Info("Could not find event for: " + e.EventData);
                    return;
                }

                foreach (var callback in entry.Callbacks)
                {
                    try
                    {
                        callback(e.EventData);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex + "    ReceiveMessageQueue Error during calling event callback: " + e.EventData);
                    }
                }
            }
        }

        protected virtual void HeartbeatTestRequestReceived()
        {
            TestAsync("ok");
        }

        public Task<TestResponse> TestAsync(string expectedResult)
        {
            return SendAsync("public/test", new { expected_result = expectedResult }, new ObjectJsonConverter<TestResponse>());
        }

        public Task<string> PingAsync()
        {
            return SendAsync("public/ping", new { }, new ObjectJsonConverter<string>());
        }

        public Task<List<string>> SubscribePublicAsync(string[] channels)
        {
            log.Info("*******SubscribePublicAsync");

            return SendAsync("public/subscribe", new { channels }, new ListJsonConverter<string>());
        }

        public Task<List<string>> UnsubscribePublicAsync(string[] channels)
        {
            return SendAsync("public/unsubscribe", new { channels }, new ListJsonConverter<string>());
        }

        public Task<List<string>> SubscribePrivateAsync(string[] channels, string accessToken)
        {
            log.Info("*******SubscribePrivateAsync");
            return SendAsync("private/subscribe", new { channels, access_token = accessToken }, new ListJsonConverter<string>());
        }

        public Task<List<string>> UnsubscribePrivateAsync(string[] channels, string accessToken)
        {
            return SendAsync("private/unsubscribe", new { channels, access_token = accessToken }, new ListJsonConverter<string>());
        }

        public async Task<bool> ManagedSubscribeAsync(string channel, bool @private, string accessToken, Action<EventResponse> callback)
        {
            SubscriptionEntry entry;
            TaskCompletionSource<bool> defer = null;
            lock (_eventsMap)
            {
                if (_eventsMap.ContainsKey(channel))
                {
                    entry = _eventsMap[channel];
                    switch (entry.State)
                    {
                        case SubscriptionState.Subscribed:
                            {
                                log.Info($"Already subsribed added to callbacks {channel}");
                                if (callback != null)
                                {
                                    entry.Callbacks.Add(callback);
                                }
                                return true;
                            }

                        case SubscriptionState.Unsubscribing:
                            log.Info($"Unsubscribing return false {channel}");
                            return false;

                        case SubscriptionState.Unsubscribed:

                            log.Info($"Unsubscribed resubscribing {channel}");
                            entry.State = SubscriptionState.Subscribing;
                            defer = new TaskCompletionSource<bool>();
                            entry.CurrentAction = defer.Task;
                            break;
                    }
                }
                else
                {
                    log.Info($"Not exists subscribing {channel}");
                    defer = new TaskCompletionSource<bool>();
                    entry = new SubscriptionEntry()
                    {
                        State = SubscriptionState.Subscribing,
                        Callbacks = new List<Action<EventResponse>>(),
                        CurrentAction = defer.Task
                    };
                    _eventsMap[channel] = entry;
                }
            }
            if (defer == null)
            {
                log.Info($"Empty defer wait for already subscribing {channel}");
                var currentAction = entry.CurrentAction;
                var result = currentAction != null && await currentAction;
                log.Info($"Empty defer wait for already subscribing res {result} {channel}");
                lock (_eventsMap)
                {
                    if (!result || entry.State != SubscriptionState.Subscribed)
                    {
                        return false;
                    }

                    log.Info($"Empty defer adding callback {channel}");
                    if (callback != null)
                    {
                        entry.Callbacks.Add(callback);
                    }
                    return true;
                }
            }
            try
            {
                log.Info($"Subscribing {channel}");
                var response = !@private ? await SubscribePublicAsync(new[] { channel }) : await SubscribePrivateAsync(new[] { channel }, accessToken);
                log.Info("test 0124");
                if (response.Count != 1 || response[0] != channel)
                {
                    log.Info($"Invalid subscribe result {response} {channel}");
                    defer.SetResult(false);
                }
                else
                {
                    lock (_eventsMap)
                    {
                        log.Info($"Successfully subscribed adding callback {channel}");
                        entry.State = SubscriptionState.Subscribed;
                        if (callback != null)
                        {
                            entry.Callbacks.Add(callback);
                        }
                        entry.CurrentAction = null;
                    }
                    defer.SetResult(true);
                }
            }
            catch (Exception e)
            {
                defer.SetException(e);
            }
            return await defer.Task;
        }

        public async Task<bool> ManagedUnsubscribeAsync(string channel, bool @private, string accessToken, Action<EventResponse> callback)
        {
            SubscriptionEntry entry;
            TaskCompletionSource<bool> defer;
            lock (_eventsMap)
            {
                if (!_eventsMap.ContainsKey(channel))
                {
                    return false;
                }
                entry = _eventsMap[channel];
                if (!entry.Callbacks.Contains(callback))
                {
                    return false;
                }
                switch (entry.State)
                {
                    case SubscriptionState.Subscribing:
                        return false;
                    case SubscriptionState.Unsubscribed:
                    case SubscriptionState.Unsubscribing:
                        entry.Callbacks.Remove(callback);
                        return true;
                    case SubscriptionState.Subscribed:
                        if (entry.Callbacks.Count > 1)
                        {
                            entry.Callbacks.Remove(callback);
                            return true;
                        }
                        entry.State = SubscriptionState.Unsubscribing;
                        defer = new TaskCompletionSource<bool>();
                        entry.CurrentAction = defer.Task;
                        break;
                    default: return false;
                }
            }
            try
            {
                var response = !@private ? await UnsubscribePublicAsync(new[] { channel }) : await UnsubscribePrivateAsync(new[] { channel }, accessToken);
                if (response.Count != 1 || response[0] != channel)
                {
                    defer.SetResult(false);
                }
                else
                {
                    lock (_eventsMap)
                    {
                        entry.State = SubscriptionState.Unsubscribed;
                        entry.Callbacks.Remove(callback);
                        entry.CurrentAction = null;
                    }
                    defer.SetResult(true);
                }
            }
            catch (Exception e)
            {
                defer.SetException(e);
            }
            return await defer.Task;
        }
    }
}
