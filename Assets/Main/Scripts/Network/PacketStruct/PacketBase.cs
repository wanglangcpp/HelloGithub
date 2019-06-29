using GameFramework.Network;
using ProtoBuf;

namespace Genesis.GameClient
{
    public abstract class PacketBase : Packet, IExtensible
    {
        private IExtension m_ExtensionObject;

        public PacketBase()
        {
            m_ExtensionObject = null;
        }

        public abstract PacketType PacketType
        {
            get;
        }

        public abstract int PacketActionId
        {
            get;
        }

        public override int Id
        {
            get
            {
                return NetworkHelper.GetOpCode(PacketType, PacketActionId);
            }
        }

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref m_ExtensionObject, createIfMissing);
        }
    }
}
