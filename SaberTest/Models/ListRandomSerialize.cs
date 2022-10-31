using SaberTest.Exceptions;

namespace SaberTest.Models
{
    public static class ListRandomSerialize
    {
        /// <summary>
        /// Serialize ListRandom in binary format
        /// </summary>
        public static void SerializeListRandom (Stream stream, ListRandom listRandom)
        {
            CheckSerializeArguments(listRandom.Head, listRandom.Tail, stream);
            Dictionary<ListNode, int> listNodeToNodeIndex = GetListNodeToNodeIndex(listRandom);
            using (var writer = new BinaryWriter(stream))
            {
                AddBinaryListRandomToStream(listNodeToNodeIndex, writer, listRandom.Count);
            }
            stream.Close();
        }

        /// <summary>
        /// Getting Dictionary with key ListNode and value Index ListNode in ListRandom
        /// </summary>
        private static Dictionary<ListNode,int> GetListNodeToNodeIndex (ListRandom listRandom)
        {
            Dictionary<ListNode, int> listNodeToNodeIndex = new Dictionary<ListNode, int>();
            ListNode tempNode = listRandom.Head;
            int nodeIndex = 0;

            while (tempNode is not null)
            {
                listNodeToNodeIndex.Add(tempNode, nodeIndex++);
                tempNode = tempNode.Next;

                if (tempNode is null || listNodeToNodeIndex.ContainsKey(tempNode))
                {
                    AddNodesFromTail(listNodeToNodeIndex, listRandom);
                    break;
                }
            } 

            if (listNodeToNodeIndex.Count != listRandom.Count)
            {
                throw new ArgumentException($"Incorrect Count: {listRandom.Count}, number of nodes in list {nodeIndex}");
            }

            return listNodeToNodeIndex;
        }

        /// <summary>
        /// Add Nodes from tail if loop detected
        /// </summary>
        private static void AddNodesFromTail(Dictionary<ListNode, int> listNodeToNodeIndex, ListRandom listRandom)
        {
            ListNode tempNode = listRandom.Tail;
            int nodeIndex = listRandom.Count - 1;

            while (tempNode != null && !listNodeToNodeIndex.ContainsKey(tempNode))
            {
                listNodeToNodeIndex.Add(tempNode, nodeIndex--);
                tempNode = tempNode.Previous;
            }
        }

        /// <summary>
        /// This method ad ListRandom to stream in binary format
        /// </summary>
        private static void AddBinaryListRandomToStream(Dictionary<ListNode, int> listNodeToNodeIndex, BinaryWriter writer, int Count)
        {
            writer.Write(Count);

            foreach (KeyValuePair<ListNode, int> listNodeIndexPair in listNodeToNodeIndex)
            {
                WriteNodeToStream(writer, listNodeIndexPair.Key, listNodeToNodeIndex);
            }
        }

        /// <summary>
        /// This method writes ListNode to stream in binary format
        /// </summary>
        private static void WriteNodeToStream(BinaryWriter writer, ListNode listNode, Dictionary<ListNode, int> listNodeToNodeIndex)
        {
            int randomNodeIndex = listNode.Random is not null ? listNodeToNodeIndex[listNode.Random] : -1;
            int nextNodeIndex = listNode.Next is not null ? listNodeToNodeIndex[listNode.Next] : -1;
            int prevNodeIndex = listNode.Previous is not null ? listNodeToNodeIndex[listNode.Previous] : -1;
            int nodeIndex = listNodeToNodeIndex[listNode];
            string data = listNode.Data is not null ? listNode.Data : "Nullable";
            writer.Write(data);
            writer.Write(nodeIndex);
            writer.Write(randomNodeIndex);
            writer.Write(nextNodeIndex);
            writer.Write(prevNodeIndex);
        }   

        /// <summary>
        /// Сheck the input data for serialization
        /// </summary>
        private static void CheckSerializeArguments(ListNode head, ListNode tail, Stream stream)
        {
            if (head is null)
            {
                throw new ArgumentException("Empty ListRandom!");
            }

            if (tail is null)
            {
                throw new NullReferenceException("Tail is null!");
            }

            if (!stream.CanWrite)
            {
                throw new ArgumentException("Unwritable stream!");
            }
        }
    }
}