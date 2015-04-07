using biz.entity;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static decimal x = 121.563500M;
        static decimal y = 31.210255M;
        static decimal scale = Constant.SCALE;
        static void Main(string[] args)
        {

            List<MapData> list=Show(x, y,1);
            foreach (MapData md in list)
                biz.BizApi.InsertMapData(md);
            Console.WriteLine();
        }

        public static List<MapData> Show(decimal x, decimal y,int address)
        {
            List<MapData> list = new List<MapData>();

            for (int i = -100; i < 100; i++)
                for (int j = -100; j < 100; j++)
                {
                    list.Add(
                        new MapData()
                        {
                            lng = (x + i * scale),
                            lat = (y + j * scale),
                            address = address,
                            code = Util.FormatCode(i, j),
                            X = i,
                            Y = j,
                            lngH = (x + i * scale) + (scale / 2),
                            lngL = (x + i * scale) - (scale / 2),
                            latH = (y + j * scale) + (scale / 2),
                            latL = (y + j * scale) - (scale / 2)
                        });
                }
            return list;
        }
    }
}
