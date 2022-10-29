using SaberTest.Exceptions;
using System.Threading;

namespace SaberTest.Models
{
    public class ListRandom
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;
        public delegate Task<bool> AsyncMethodCaller(Stream s);

        public void Serialize(Stream s)
        {
            ListRandomSerialize.SerializeListRandom(s, this);
        }

        public void Deserialize(Stream s)
        {
            ListRandomDeserialize.DeserializeListRandom(s, this);
        }
    }
}
