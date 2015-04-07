using biz.entity;
using Bresenham;
using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz
{
    public class BizApi
    {

        public static string MAP_DATA = "mapdata";
        public static string SHANGJIA_DATA = "shangjia";
        public static decimal SCALE = 0.002M;//200m

        #region 插入更新


        

        //插入分析好的数据
        public static void InsertMapData(MapData md)
        {
            string sql = String.Format(
                "INSERT INTO {0}(code,address,lat,lng,X,Y,lngH,lngL,latH,latL)VALUES({1},{2},{3},{4},{5},{6},{7},{8},{9},{10})",
                        MAP_DATA, md.code, md.address, md.lat, md.lng, md.X, md.Y, md.lngH, md.lngL, md.latH, md.latL);
            MySqlHelper.ExecuteNonQuery(sql);
        }


        public static List<MapData> QueryById(int address)
        {
            return BuildDataList(address);
        }

        public static List<MapData> QueryById(int address, int limit, decimal lng, decimal lat)
        {
            string sql = string.Format("select * from {0} where address={1} and lat>{2} and lat<{3}  and lng>{4} and lng<{5}", MAP_DATA, address, lat - limit * SCALE, lat + limit * SCALE, lng - limit * SCALE, lng + limit * SCALE);
            DataSet ds = MySqlHelper.GetDataSet(sql);
            DataTable dt = ds.Tables[0];
            List<MapData> list = new List<MapData>();
            foreach (DataRow dr in dt.Rows)
            {
                MapData bd = new MapData()
                {
                    code = (int)dr["code"],
                    address = (int)dr["address"],
                    lat = (Decimal)dr["lat"],
                    lng = (Decimal)dr["lng"],
                    X = (int)dr["X"],
                    Y = (int)dr["Y"],
                    latH = (Decimal)dr["latH"],
                    lngH = (Decimal)dr["lngH"],
                    latL = (Decimal)dr["latL"],
                    lngL = (Decimal)dr["lngL"],

                };

                list.Add(bd);
            }

            return list;
        }

        /// <summary>
        /// 查询空间区域
        /// </summary>
        /// <param name="lngH"></param>
        /// <param name="lngL"></param>
        /// <param name="latH"></param>
        /// <param name="latL"></param>
        /// <returns></returns>
        public static List<MapData> QueryMapList(decimal lngH, decimal lngL, decimal latH, decimal latL)
        {
            string sql = string.Format("select * from {0} where lat>{1} and lat<{2}  and lng>{3} and lng<{4}", MAP_DATA, latL - SCALE/2, latH + SCALE/2, lngL - SCALE/2, lngH + SCALE/2);
            DataSet ds = MySqlHelper.GetDataSet(sql);
            DataTable dt = ds.Tables[0];
            List<MapData> list = new List<MapData>();
            foreach (DataRow dr in dt.Rows)
            {
                MapData bd = new MapData()
                {
                    code = (int)dr["code"],
                    address = (int)dr["address"],
                    lat = (Decimal)dr["lat"],
                    lng = (Decimal)dr["lng"],
                    X = (int)dr["X"],
                    Y = (int)dr["Y"],
                    latH = (Decimal)dr["latH"],
                    lngH = (Decimal)dr["lngH"],
                    latL = (Decimal)dr["latL"],
                    lngL = (Decimal)dr["lngL"],
                };

                list.Add(bd);
            }

            return list;
        }

        public static List<ShangjiaData> QueryShangjiaAll()
        {
            string sql = string.Format("select distinct name from {0}", SHANGJIA_DATA);
            DataSet ds = MySqlHelper.GetDataSet(sql);
            DataTable dt = ds.Tables[0];
            List<ShangjiaData> list = new List<ShangjiaData>();
            foreach (DataRow dr in dt.Rows)
            {
                ShangjiaData bd = new ShangjiaData()
                {
                    name = dr["name"].ToString()
                };

                list.Add(bd);
            }

            return list;
        }

        public static List<ShangjiaData> QueryShangjia(decimal lng_orig,decimal lat_orig,decimal lng_s,decimal lat_s)
        {
            MapData md = QueryFromOrig(1, lng_orig, lat_orig, lng_s, lat_s);

            string sql = string.Format("select * from {0} where code={1}", SHANGJIA_DATA, md.code);
            DataSet ds = MySqlHelper.GetDataSet(sql);
            DataTable dt = ds.Tables[0];
            List<ShangjiaData> list = new List<ShangjiaData>();
            foreach (DataRow dr in dt.Rows)
            {
                ShangjiaData bd = new ShangjiaData()
                {
                    code = (int)dr["code"],
                    name = dr["name"].ToString(),
                    id=(int)dr["id"],
                    lat = (Decimal)dr["lat"],
                    lng = (Decimal)dr["lng"]

                };

                list.Add(bd);
            }

            return list;
        }


        public static List<ShangjiaData> QueryShangjiaByName(string name,int type)
        {

            string sql = string.Format("select * from {0} where name='{1}' and type={2} order by id desc", SHANGJIA_DATA, name,type);
            DataSet ds = MySqlHelper.GetDataSet(sql);
            DataTable dt = ds.Tables[0];
            List<ShangjiaData> list = new List<ShangjiaData>();
            foreach (DataRow dr in dt.Rows)
            {
                ShangjiaData bd = new ShangjiaData()
                {
                    code = (int)dr["code"],
                    name = dr["name"].ToString(),
                    id = (int)dr["id"],
                    lat = (Decimal)dr["lat"],
                    lng = (Decimal)dr["lng"]

                };

                list.Add(bd);
            }

            return list;
        }
        public static string InsertShangjiaMap(string name, string location_list, decimal lng_orig, decimal lat_orig)
        {

            string sql1 = String.Format("delete from {0} where name='{1}'", SHANGJIA_DATA, name);
            MySqlHelper.ExecuteNonQuery(sql1);

            string[] list = location_list.Split('|');
            string sql = "";

            List<MapData> sourceList = new List<MapData>();
            foreach (string aa in list)
            {
                string[] bb = aa.Split(',');
                sourceList.Add(new MapData()
                {
                    lng = decimal.Parse(bb[0]),
                    lat = decimal.Parse(bb[1])
                });

                
                ////MapData md = QueryFromOrig(1, lng_orig, lat_orig, decimal.Parse(bb[0]), decimal.Parse(bb[1]));
                //sql = String.Format("INSERT INTO {0}(name,lat,lng,code)VALUES('{1}',{2},{3},{4})",
                //        SHANGJIA_DATA, name, bb[0], bb[1], md.code);
                //MySqlHelper.ExecuteNonQuery(sql);

            }

            //原始点
            foreach (MapData md1 in sourceList)
            {
                InsertShangjiaMap(md1, name, 0);
            }

            List<MapData> area = CompueteGrid(sourceList);

            foreach (MapData md in area)
            {
                InsertShangjiaMap(md,name,1);
            }
            ////string sql = String.Format(
            ////    "INSERT INTO {0}(name,lat,lng)VALUES('{1}',{2},{3})",
            ////            SHANGJIA_DATA, sd.name,sd.lat,sd.lng);
            //MySqlHelper.ExecuteNonQuery(sql);

            return name;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="md"></param>
        /// <param name="name"></param>
        /// <param name="type">0-原始点，1-包含的点</param>
        public static void InsertShangjiaMap(MapData md,string name,int type)
        {
            string sql = String.Format("INSERT INTO {0}(name,lng,lat,code,type)VALUES('{1}',{2},{3},{4},{5})",
            SHANGJIA_DATA, name,md.lng, md.lat, md.code,type);
            MySqlHelper.ExecuteNonQuery(sql);
        }

        //保存网格的点
        public static List<MapData> CompueteGrid(List<MapData> sourcelist)
        {
            decimal lngH = -99999M, lngL = 99999M, latH = -99999M, latL = 99999M;
            foreach (MapData md in sourcelist)
            {
                if (md.lat < latL) latL = md.lat;
                if (md.lat > latH) latH = md.lat;

                if (md.lng < lngL) lngL = md.lng;
                if (md.lng > lngH) lngH = md.lng;
            }

            //缩小范围，取最大最小值
            List<MapData> target_list = QueryMapList(lngH, lngL, latH, latL);
            List<MapData> list = new List<MapData>();
            foreach (MapData md in target_list)
            {
                if (Compute(md, sourcelist)){
                    Console.WriteLine(md.code);
                    list.Add(md);
                }
            }
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p">点</param>
        /// <param name="all">多边形</param>
        /// <returns></returns>
        //public static bool Compute(MapData p, List<MapData> all)
        //{

        //    int i, j;
        //    var inside = false;
        //    var count1 = 0;
        //    var count2 = 0;
        //    var N = all.Count;
        //    for (i = 0, j = N - 1; i < N; j = i++)
        //    {
        //        var value = (p.lng - all[j].lng) * (all[i].lat - all[j].lat) - (p.lat - all[j].lat) * (all[i].lng - all[j].lng);
        //        if (value > 0)
        //        {
        //            ++count1;
        //        }
        //        else if (value < 0)
        //        {
        //            ++count2;
        //        }
        //    }

        //    if (0 == count1 || 0 == count2)
        //    {
        //        inside = true;
        //    }
        //    return inside;
        //}
        public static bool Compute(MapData p, List<MapData> all)
        {
            return (Compute1(p.lngL, p.latL, all) || Compute1(p.lngH, p.latH, all) || Compute1(p.lngH, p.latL, all) || Compute1(p.lngL, p.latH, all));
        }

        public static bool Compute1(decimal lng,decimal lat, List<MapData> all)
        {

            int i, j;
            var inside = false;
            var count1 = 0;
            var count2 = 0;
            var N = all.Count;
            for (i = 0, j = N - 1; i < N; j = i++)
            {
                var value = (lng - all[j].lng) * (all[i].lat - all[j].lat) - (lat - all[j].lat) * (all[i].lng - all[j].lng);
                if (value > 0)
                {
                    ++count1;
                }
                else if (value < 0)
                {
                    ++count2;
                }
            }

            if (0 == count1 || 0 == count2)
            {
                inside = true;
            }
            return inside;
        }

        public static List<MapData> QueryByXY(int address, decimal lng_orig, decimal lat_orig, decimal lng_1, decimal lat_1, decimal lng_2, decimal lat_2)
        {

            List<MapData> list = new List<MapData>();
            MapData point1 = QueryFromOrig(address, lng_orig, lat_orig, lng_1, lat_1);
            MapData point2 = QueryFromOrig(address, lng_orig, lat_orig, lng_2, lat_2);
            IEnumerable<Point> p_list = BresLine.RenderLine(new Point(point1.X, point1.Y), new Point(point2.X, point2.Y));

            string code_list = "";
            foreach (Point p in p_list)
            {
                code_list += Util.FormatCode(p.X, p.Y) + ",";
            }

            //包括起点和终点
            code_list = Util.FormatCode(point1.X, point1.Y) + "," + code_list + Util.FormatCode(point2.X, point2.Y);

            //if (string.IsNullOrEmpty(code_list)) code_list = code_list.Substring(0, code_list.Length - 1);
            string sql = string.Format("select * from {0} where address={1} and code in ({2})", MAP_DATA, address, code_list);
            DataSet ds = MySqlHelper.GetDataSet(sql);
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                MapData bd = new MapData()
                {
                    code = (int)dr["code"],
                    address = (int)dr["address"],
                    lat = (Decimal)dr["lat"],
                    lng = (Decimal)dr["lng"],
                    X = (int)dr["X"],
                    Y = (int)dr["Y"],
                    latH = (Decimal)dr["latH"],
                    lngH = (Decimal)dr["lngH"],
                    latL = (Decimal)dr["latL"],
                    lngL = (Decimal)dr["lngL"],

                };

                list.Add(bd);
            }
            //Console.WriteLine(code_list);
            return list;
            // 
        }

        //public static Point GeneratePoint(int code)
        //{
        //    return new System.Drawing.Point((code / 1000) % 100, (code % 1000) / 100);
        //}
        /// <summary>
        /// 查询某一点所处的格子
        /// LH,HH,HL,LL
        /// </summary>
        /// <param name="address"></param>
        /// <param name="lng_orig">原点</param>
        /// <param name="lat_orig">原点</param>
        /// <param name="lng">目标点</param>
        /// <param name="lat">目标点</param>
        /// <returns></returns>
        public static MapData QueryFromOrig(int address, decimal lng_orig, decimal lat_orig, decimal lng, decimal lat)
        {
            decimal s = 0.001M;
            int lng_c = 0, lat_c = 0;

            decimal lngd = lng - lng_orig;
            decimal latd = lat - lat_orig;

            if (Math.Abs(lngd) <= s) { lng_c = 0; }
            else
            {
                if (lngd != 0)
                {
                    lngd = lngd > 0 ? (lngd - s) : (lngd + s);
                }

                lng_c = lngd > 0 ? (int)(lngd * 10000) / 20 + 1 : (int)(lngd * 10000) / 20 - 1;
            }


            if (Math.Abs(latd) <= s) { lat_c = 0; }
            else
            {
                if (latd != 0)
                {
                    latd = latd > 0 ? (latd - s) : (latd + s);
                }
                lat_c = latd > 0 ? (int)(latd * 10000) / 20 + 1 : (int)(latd * 10000) / 20 - 1;
            }


            int code = Util.FormatCode(lng_c, lat_c);

            string sql = string.Format("select * from {0} where address={1} and code={2}", MAP_DATA, address, code);
            DataSet ds = MySqlHelper.GetDataSet(sql);
            DataTable dt = ds.Tables[0];
            List<MapData> list = new List<MapData>();
            foreach (DataRow dr in dt.Rows)
            {
                MapData bd = new MapData()
                {
                    code = (int)dr["code"],
                    address = (int)dr["address"],
                    lat = (Decimal)dr["lat"],
                    lng = (Decimal)dr["lng"],
                    X = (int)dr["X"],
                    Y = (int)dr["Y"],
                    latH = (Decimal)dr["latH"],
                    lngH = (Decimal)dr["lngH"],
                    latL = (Decimal)dr["latL"],
                    lngL = (Decimal)dr["lngL"],

                };

                list.Add(bd);
            }

            return list[0];
        }

        //public static MapData QueryFromOrig(int address,decimal lng, decimal lat)
        //{

        //    string sql = string.Format("select code,address,lat,lng from {0} where address={1} and lng>={2} and lng<={3} and lat>={4} and lat<={5}", MAP_DATA, address, code);
        //    DataSet ds = MySqlHelper.GetDataSet(sql);
        //    DataTable dt = ds.Tables[0];
        //    List<MapData> list = new List<MapData>();
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        MapData bd = new MapData()
        //        {
        //            code = (int)dr["code"],
        //            address = (int)dr["address"],
        //            lat = (Decimal)dr["lat"],
        //            lng = (Decimal)dr["lng"]

        //        };

        //        list.Add(bd);
        //    }

        //    return list[0];
        //}

        private static List<MapData> BuildDataList(int address)
        {

            string sql = string.Format("select code,address,lat,lng from {0} where address={1}", MAP_DATA, address);
            DataSet ds = MySqlHelper.GetDataSet(sql);
            DataTable dt = ds.Tables[0];
            List<MapData> list = new List<MapData>();
            foreach (DataRow dr in dt.Rows)
            {
                MapData bd = new MapData()
                {
                    code = (int)dr["code"],
                    address = (int)dr["address"],
                    lat = (Decimal)dr["lat"],
                    lng = (Decimal)dr["lng"]

                };

                list.Add(bd);
            }

            return list;
        }

        #endregion

    }

    public class MyPoint
    {
        public decimal lng, lat;
        public MyPoint(decimal lng, decimal lat)
        {
            this.lng = lng;
            this.lat = lat;
        }
    }

}
