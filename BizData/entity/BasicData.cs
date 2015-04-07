using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace biz.entity
{
    [DataContract]
    public class MapData
    {

        [DataMember]
        public int code;
        [DataMember]
        public int address;
        [DataMember]
        public decimal lat;
        [DataMember]
        public decimal lng;
        [DataMember]
        public int X;
        [DataMember]
        public int Y;
        [DataMember]
        public decimal latL;
        [DataMember]
        public decimal latH;
        [DataMember]
        public decimal lngH;
        [DataMember]
        public decimal lngL;

    }

    [DataContract]
    public class ShangjiaData
    {
        [DataMember]
        public string name;
        [DataMember]
        public int code;
        [DataMember]
        public int id;
        [DataMember]
        public decimal lng;
        [DataMember]
        public decimal lat;
    }
}
