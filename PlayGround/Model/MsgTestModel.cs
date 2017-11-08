using System;
using System.Collections.Generic;
using MsgPack.Serialization;
using System.Runtime.Serialization;

namespace PlayGround.Model
{
    [DataContract]
    public class MsgTestModel
    {
        [DataMember]
        int X { get; set; }
        [DataMember]
        int Y { get; }
        [DataMember]
        public int Xpub { get; set; }
        [DataMember]
        public int Ypub { get; }
        [DataMember]
        public MsgPackPublicModel PublicModel { get; set; }
        [DataMember]
        protected List<MsgPackPublicModel> List { get; set; }

        public void SetList(List<MsgPackPublicModel> list)
        {
            List = list;
        }
        //public MsgTestModel(int y)
        //{
        //    Y = y;
        //    Ypub = y*10;
        //    X = y + 1;
        //}
    }

    [DataContractAttribute]
    public class MsgPackPublicModel
    {
        //[MessagePackMember(0)]
        [DataMemberAttribute]
        public int Mpublic { get; set; }
        //[MessagePackMember(1)]
        [DataMemberAttribute]
        protected int Mprotected { get; set; }
        //[MessagePackMember(2)]
        [DataMemberAttribute]
        private int Mprivate { get; set; }

        public void SetMprotected(int input)
        {
            Mprotected = input;
        }
        public void SetMprivate(int input)
        {
            Mprivate = input;
        }
        //public MsgPackPublicModel(int m)
        //{
        //    Mprivate = m;
        //}
    }

    class MsgPackProtectedModel
    {

    }
}
