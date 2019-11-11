using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DeriSock_4_7_2
{
    class JToken_Try_Parse_Function
    {

        //origin recieve:
        //       string json = "{\"Name\" : \"Jack\", \"Age\" : 34, \"Colleagues\" : [{\"Name\" : \"Tom\" , \"Age\":44},{\"Name\" : \"Abel\",\"Age\":29}
        //       JObject jObj = JObject.Parse(json);
        //1st way to parse:
        //       JToken ageToken =  jObj["Age"];
        //       string sAge = ageToken.ToString();
        //       int nAge = int.Parse(sAge);
        //2nd way to parse:
        //       if(TryParse(jObj["Age"], out int nAge)) {.... nAge ....}  else {/*error*/}


        static public bool TryParse(JToken j, out string s)
        {
            s = "";
            if (j != null)
            {
                s = j.ToString();
                return true;
            }
            return false;
        }

        static bool TryParse(JToken j, out double d)
        {
            d = 0;
            if (j != null)
            {
                if (Double.TryParse(j.ToString(), out double d_))
                {
                    d = d_;
                    return true;
                }
            }
            return false;
        }

        static bool TryParse(JToken j, out int n)
        {
            n = 0;
            if (j != null)
            {
                if (Int32.TryParse(j.ToString(), out int n_))
                {
                    n = n_;
                    return true;
                }
            }
            return false;
        }

        static bool TryParse(JToken j, out long l)
        {
            l = 0;
            if (j != null)
            {
                if (Int64.TryParse(j.ToString(), out long l_))
                {
                    l = l_;
                    return true;
                }
            }
            return false;
        }

        static bool TryParse(JToken j, out DateTime dt)
        {
            dt = DateTime.Now;
            if (j != null)
            {
                if (Int64.TryParse(j.ToString(), out long l_))
                {
                    dt = DateTimeOffset.FromUnixTimeMilliseconds(l_).DateTime.ToLocalTime();
                    return true;
                }
            }
            return false;
        }
    }
}
