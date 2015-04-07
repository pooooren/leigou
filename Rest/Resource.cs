using biz;
using biz.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace Rest
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Stock" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Stock.svc or Stock.svc.cs at the Solution Explorer and start debugging.
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class Resource
    {
        [WebGet(UriTemplate = "test", ResponseFormat = WebMessageFormat.Json)]
        public string version()
        {
            return "1.0";
        }

        [WebGet(UriTemplate = "query/address/{address}", ResponseFormat = WebMessageFormat.Json)]
        public IList<MapData> QueryData(string address)
        {
            return BizApi.QueryById(int.Parse(address));
        }

        [WebGet(UriTemplate = "query1/address/{address}?lat={lat}&lng={lng}&limit={limit}", ResponseFormat = WebMessageFormat.Json)]
        public IList<MapData> QueryData1(string address,string lat,string lng,string limit)
        {
            return BizApi.QueryById(int.Parse(address),int.Parse(limit),decimal.Parse(lng),decimal.Parse(lat));
        }


        [WebGet(UriTemplate = "query2/address/{address}?laty={laty}&lngy={lngy}&lng={lng}&lat={lat}", ResponseFormat = WebMessageFormat.Json)]
        public MapData QueryDataXY(string address, string laty, string lngy, string lng,string lat)
        {
            return BizApi.QueryFromOrig(int.Parse(address), decimal.Parse(lngy), decimal.Parse(laty), decimal.Parse(lng), decimal.Parse(lat));
        }

        [WebGet(UriTemplate = "query3/address/{address}?laty={laty}&lngy={lngy}&lng1={lng1}&lat1={lat1}&lng2={lng2}&lat2={lat2}", ResponseFormat = WebMessageFormat.Json)]
        public List<MapData> QueryDataByEdge(string address, string laty, string lngy, string lng1, string lat1,string lng2,string lat2)
        {
            List<MapData> list= BizApi.QueryByXY(int.Parse(address), decimal.Parse(lngy), decimal.Parse(laty), decimal.Parse(lng1), decimal.Parse(lat1), decimal.Parse(lng2), decimal.Parse(lat2));

            return list;
        }



        [WebInvoke(Method = "GET", UriTemplate = "insert/shangjia1/?location={location}&name={name}&lat_orig={lat_orig}&lng_orig={lng_orig}", ResponseFormat = WebMessageFormat.Json)]
        public void InsertShangjiaData1(string name, string location,string lng_orig,string lat_orig)
        {
            BizApi.InsertShangjiaMap(name,location,decimal.Parse(lng_orig),decimal.Parse(lat_orig));
        }

        [WebInvoke(Method = "GET", UriTemplate = "query/shangjiaAll", ResponseFormat = WebMessageFormat.Json)]
        public List<ShangjiaData> QueryShangjiaDataAll()
        {
            return BizApi.QueryShangjiaAll();
        }

        /// <summary>
        /// 0-原始点，1-查询区域
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [WebInvoke(Method = "GET", UriTemplate = "query/shangjia/name/{name}?type={type}", ResponseFormat = WebMessageFormat.Json)]
        public List<ShangjiaData> QueryShangjiaByName(string name,string type)
        {
            if (!string.IsNullOrEmpty(type))
            return BizApi.QueryShangjiaByName(name,int.Parse(type));
            else
                return BizApi.QueryShangjiaByName(name, 1);
        }

        [WebInvoke(Method = "GET", UriTemplate = "query/shangjia1?lat_orig={lat_orig}&lng_orig={lng_orig}&lng={lng}&lat={lat}", ResponseFormat = WebMessageFormat.Json)]
        public List<ShangjiaData> QueryShangjiaByXY(string lng_orig, string lat_orig, string lng, string lat)
        {
            return BizApi.QueryShangjia(decimal.Parse(lng_orig), decimal.Parse(lat_orig), decimal.Parse(lng), decimal.Parse(lat));
        }

    }
}
