﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: PBMailInfo.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"PBMailInfo")]
    public partial class PBMailInfo
    {
        public PBMailInfo() { }

        private int m_Id;
        [ProtoMember(1, Name = @"Id", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        private string m_Sender;
        [ProtoMember(2, Name = @"Sender", IsRequired = true, DataFormat = DataFormat.Default)]
        public string Sender
        {
            get { return m_Sender; }
            set { m_Sender = value; }
        }

        private string m_Title;
        [ProtoMember(3, Name = @"Title", IsRequired = true, DataFormat = DataFormat.Default)]
        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }

        private string m_Message;
        [ProtoMember(4, Name = @"Message", IsRequired = true, DataFormat = DataFormat.Default)]
        public string Message
        {
            get { return m_Message; }
            set { m_Message = value; }
        }

        private int m_Priority;
        [ProtoMember(5, Name = @"Priority", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int Priority
        {
            get { return m_Priority; }
            set { m_Priority = value; }
        }

        private bool m_IsRead;
        [ProtoMember(6, Name = @"IsRead", IsRequired = true, DataFormat = DataFormat.Default)]
        public bool IsRead
        {
            get { return m_IsRead; }
            set { m_IsRead = value; }
        }

        private PBInt64 m_GenerateTime;
        [ProtoMember(7, Name = @"GenerateTime", IsRequired = true, DataFormat = DataFormat.Default)]
        public PBInt64 GenerateTime
        {
            get { return m_GenerateTime; }
            set { m_GenerateTime = value; }
        }

        private readonly List<PBItemInfo> m_ItemInfo = new List<PBItemInfo>();
        [ProtoMember(8, Name = @"ItemInfo", DataFormat = DataFormat.Default)]
        public List<PBItemInfo> ItemInfo
        {
            get { return m_ItemInfo; }
        }
    }
}
