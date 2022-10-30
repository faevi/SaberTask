using System.IO;
using SaberTest.Exceptions;

namespace SaberTest.Models
{
    public class ListRandomDeserialize
    {
        public static void DeserializeListRandom(Stream stream, ListRandom listRandom)
        {
            List<Tuple<ListNode, int>> tuplesListNodeAndRandomNodeIndex;
            CheckArgumentsDeserialize(stream);
            tuplesListNodeAndRandomNodeIndex = ReadListRandomFromStream(stream, listRandom);
            ConnectAllNodes(tuplesListNodeAndRandomNodeIndex, listRandom);
        }

        /// <summary>
        /// Check input data for deserialization
        /// </summary>
        private static void CheckArgumentsDeserialize(Stream stream)
        {
            if (!stream.CanRead)
            {
                throw new ArgumentException("Ureadable stream!");
            }

            if (stream.Length == 0)
            {
                throw new ArgumentException("Empty stream!");
            }
        }

        /// <summary>
        /// Updating a ListRandom from a stream 
        /// </summary>
        private static List<Tuple<ListNode, int>> ReadListRandomFromStream(Stream stream, ListRandom listRandom)
        {
            using (var reader = new BinaryReader(stream))
            {
                int count = reader.ReadInt32();
                listRandom.Count = count;
                List<Tuple<ListNode, int>> tuplesListNodeAndRandomNodeIndex = new List<Tuple<ListNode, int>>(count);

                for (int nodeIndex = 0; nodeIndex < count; nodeIndex++)
                {
                    ReadNodeFromStream(reader, nodeIndex, count, tuplesListNodeAndRandomNodeIndex);
                }

                if (reader.PeekChar() != -1)
                {
                    throw new IncorrectCountOfRandomListInStreamException("Wrong Count of ListRandom in stream");
                }

                return tuplesListNodeAndRandomNodeIndex;
            }
        }


        /// <summary>
        /// Create dependencies between Nodes in ListRandom
        /// </summary>
        private static void ConnectAllNodes(List<Tuple<ListNode, int>> tuplesListNodeAndRandomNodeIndex, ListRandom listRandom)
        {
            listRandom.Head = tuplesListNodeAndRandomNodeIndex.First().Item1;
            listRandom.Tail = tuplesListNodeAndRandomNodeIndex.Last().Item1;

            for (int tupleIndex = 0; tupleIndex < tuplesListNodeAndRandomNodeIndex.Count - 1; tupleIndex++)
            {
                ConnectTwoNodes(tuplesListNodeAndRandomNodeIndex[tupleIndex].Item1, tuplesListNodeAndRandomNodeIndex[tupleIndex + 1].Item1);
                ConnectNodeWithRandomNode(tuplesListNodeAndRandomNodeIndex, tupleIndex);
            }

            ConnectNodeWithRandomNode(tuplesListNodeAndRandomNodeIndex,listRandom.Count - 1);
        }

        /// <summary>
        /// Create Dependency between a node and its random one
        /// </summary>
        private static void ConnectNodeWithRandomNode(List<Tuple<ListNode, int>> tuplesListNodeAndRandomNodeIndex, int nodeIndex)
        {
            int randomNodeNumber = tuplesListNodeAndRandomNodeIndex[nodeIndex].Item2;

            if (randomNodeNumber == -1)
            {
                return;
            }

            tuplesListNodeAndRandomNodeIndex[nodeIndex].Item1.Random = tuplesListNodeAndRandomNodeIndex[randomNodeNumber].Item1;
        }

        /// <summary>
        /// Create Dependency between a node and next node
        /// </summary>
        private static void ConnectTwoNodes(ListNode firstNode, ListNode secondNode)
        {
            firstNode.Next = secondNode;
            secondNode.Previous = firstNode;
        }

        /// <summary>
        /// Read node from stream
        /// </summary>
        private static ListNode ReadNodeFromStream(BinaryReader reader, int nodeIndex, int count, List<Tuple<ListNode, int>> tuplesListNodeAndRandomNodeIndex)
        {
            string data = reader.ReadString();
            int randomNodeIndex = reader.ReadInt32();

            if (randomNodeIndex >= count)
            {
                throw new IndexOutOfRangeException($"Index = {randomNodeIndex} of RandomNode for ListNode with index {nodeIndex} more than ListRandom Count = {count}");
            }
           
            ListNode listNode = new ListNode() { Data = data != "Nullable" ? data : null};
            tuplesListNodeAndRandomNodeIndex.Add(new Tuple<ListNode,int>(listNode, randomNodeIndex));
            return listNode;
        }
    }
}

