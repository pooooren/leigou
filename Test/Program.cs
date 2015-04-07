using biz;
using biz.entity;
using Bresenham;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        //900900
        static decimal x = 121.563500M;
        static decimal y = 31.210255M;

        static decimal x1 = 121.565600M;
        static decimal y1 = 31.210565M;

        static decimal x2 = 121.595600M;
        static decimal y2= 31.220565M;

        static decimal scale = 0.002M;
        static void Main(string[] args)
        {

            //902802
            decimal x3 = 121.557445M;
            decimal y3 = 31.213961M;

           // MapData list = biz.BizApi.QueryFromOrig(1, x, y, x3, y3);
            //IEnumerable<Point> p_list = BresLine.RenderLine(new Point(-2, 2), new Point(-1, 3));
            //List<MapData> l = biz.BizApi.QueryByXY(1, x, y, x1, y1, x2, y2);

            //biz.BizApi.InsertShangjiaMap("shangjia1", "121.497500,31.316255|121.497500,31.302255|121.497500,31.18025", x, y);

            List<ShangjiaData> tt = biz.BizApi.QueryShangjiaByName("test", 0);


            List<MapData> sourceList = new List<MapData>();

            foreach (ShangjiaData aa in tt)
            {
                sourceList.Add(new MapData()
                {
                    lng = aa.lng,
                    lat = aa.lat
                });
            }

            List<MapData> area = BizApi.CompueteGrid(sourceList);



            foreach (MapData md in area)
            {
                BizApi.InsertShangjiaMap(md, "test", 1);
            }

            Console.WriteLine();


        }

    }
}
