using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace Common
{

    public class LeiLog
    {
        private  static  log4net.ILog log;


        //这里提供了一个供外部访问本class的静态方法，可以直接访问　　
        public static ILog Log
        {
            get
            {
                if (log == null)
                {
                    log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                }
                return log;
            }
        }
 
        private LeiLog()
        {
        }
    }
}
