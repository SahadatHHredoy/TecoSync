using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class Log
    {
        public int enrollid { get; set; }
        public string time { get; set; }
        public int mode { get; set; }
        public int inout { get; set; }
        public int  @event { get;set; }
    }
    public class Logs
    {
        public string ret { get; set; }
        public bool result { get; set; }
        public int count { get; set; }
        public int from { get; set; }
        public int to { get; set; }
        public List<Log> record { get; set; }
    }
   
    public class MachineReg
    {
       public string cmd { get; set; }
        public string sn { get; set; }
        public DeviceInfo devinfo { get; set; }
    }
    public class DeviceInfo
    {
        public string modelname { get; set; }
        public int usersize { get; set; }
        public int fpsize { get; set; }
        public int cardsize { get; set; }
        public int pwdsize { get; set; }
        public int logsize { get; set; }
        public int useduser { get; set; }
        public int usedfp { get; set; }
        public int usedcard { get; set; }
        public int usedpwd { get; set; }
        public int usedlog { get; set; }
        public int usednewlog { get; set; }
        public string fpalgo { get; set; }
        public string firmware { get; set; }
        public string time { get; set; }
    }

    public static class Common
    {
        public static string GetTimeFormat(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string GetJsonFormat(this object value)
        {
           return JsonConvert.SerializeObject(value, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }
    }
}
