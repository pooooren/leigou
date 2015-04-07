using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.IO;

namespace Common
{
    public  class Constant
    {

        private static Dictionary<string, string> map = new Dictionary<string, string>();
        private static Constant t = new Constant();
        private  Constant()
        {
            XmlDocument doc = new XmlDocument();
            //Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            doc.Load(AppDomain.CurrentDomain.BaseDirectory+"app.xml");

            XmlNode node = doc.SelectSingleNode("/App");
      
            foreach (XmlNode n in node.ChildNodes)
            {
                map.Add(n.Name, n.InnerText);
            }
        }
        public static string MYSQL_STRING = map["mysqlstring"];

        public static  decimal SCALE = 0.002M;//200m
    
    }

    public class Util
    {
        public static int FormatCode(int x, int y)
        {
            return Format(x) * 10000 + Format(y);
        }
        public static int Format(int x)
        {
            if (x < 0)
            {
                return 8000 + (-x);
            }
            else
            {
                return 9000 + x;
            }
        }
    }
}
