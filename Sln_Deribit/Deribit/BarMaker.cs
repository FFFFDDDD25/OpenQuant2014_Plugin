using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SmartQuant;

namespace SmartQuant.DB
{
    public delegate void EmitData_delegate(DataObject data, bool queued = true);

    public class BarMaker
    {
        public event EmitData_delegate EmitData_event;
        BarType barType;
        int interval;
        int instrumentId;
        int count;
        //--------------------------
        DateTime openDateTime;
        DateTime closeDateTime;
        double last = 0;
        double open = 0;
        double high = 0;
        double low = 0;
        //double close = 0;  close is last last
        long volume = 0;
        //long openInt = 0;  not yet use

        public BarMaker(int intervel, BarType barType,  EmitData_delegate func, int instrumentId)
        {
            this.interval = intervel; //intervel = ticks or seconds, depends on barType
            this.instrumentId = instrumentId;
            this.barType = barType;
            this.EmitData_event += func;

            new Thread(() =>
            {
                while (true)
                {
                    CheckBarFinished();
                    Thread.Sleep(1000);
                }
            }).Start();
        }


        public void CheckBarFinished()
        {
            lock (lock_)
            {
                if (this.open == 0)//no market fill received
                    return;

                bool isEnd = false;

                int size = 0;

                if (barType == BarType.Tick)
                {
                    size = interval;
                    if (count > interval)
                        isEnd = true;
                }
                else
                {
                    size = 0;
                    if (DateTime.Now > closeDateTime)
                        isEnd = true;
                }


                if (isEnd)
                {
                    if (this.EmitData_event != null)
                    {
                        EmitData_event(
                            new Bar(
                            openDateTime,
                            DateTime.Now,
                            this.instrumentId,
                            this.barType,
                            size,//long size
                            this.open,
                            this.high,
                            this.low,
                            this.last,
                            this.volume,
                            0
                            )
                            );
                    }


                    count = 0;
                    openDateTime = DateTime.Now;
                    closeDateTime = DateTime.Now.Add(new TimeSpan(0, 0, interval));
                    open = last;
                    high = last;
                    low = last;
                    volume = 0;
                }
            }
        }


        object lock_ = new object();
        bool first = true;
        public void Input(double price, int volume)
        {
            CheckBarFinished();//careful deadlock

            lock (lock_)
            {
                if (first)
                {
                    first = false;
                    this.open = price;
                    this.high = price;
                    this.low = price;
                    openDateTime = DateTime.Now;
                }


                this.count++;
                this.volume += volume;
                this.high = (price > this.high ? price : this.high);
                this.low = (price < this.low ? price : this.low);
                this.last = price;
            }
        }
    }
}
