using SaberTest.Exceptions;

namespace SaberTest.Models
{
    public class ListRandomDeserialize
    {
        public static void DeserializeListRandom(Stream stream, ListRandom listRandom)
        {
            List<Tuple<ListNode, int>> tuplesListNodeAndRandomNodeIndex;
            using (var reader = new BinaryReader(stream))
            {
                CheckArgumentsDeserialize(stream, reader);
                tuplesListNodeAndRandomNodeIndex = ReadListRandomFromStream(reader, listRandom);
            }
            ConnectAllNodes(tuplesListNodeAndRandomNodeIndex, listRandom);
        }

        /// <summary>
        /// Check input data for deserialization
        /// </summary>
        private static void CheckArgumentsDeserialize(Stream stream, BinaryReader reader)
        {
            if (!stream.CanRead)
            {
                throw new ArgumentException("Ureadable stream!");
            }

            if (reader.PeekChar() <= 0)
            {
                throw new ArgumentException("Empty stream!");
            }
        }

        /// <summary>
        /// Updating a ListRandom from a stream 
        /// </summary>
        private static List<Tuple<ListNode, int>> ReadListRandomFromStream(BinaryReader reader, ListRandom listRandom)
        {
            int count = reader.ReadInt32();
            listRandom.Count = count;
            List<Tuple<ListNode,int>> tuplesListNodeAndRandomNodeIndex = new List<Tuple<ListNode, int>>(count);

            for (int nodeIndex = 0; nodeIndex < count ; nodeIndex++)
            {
                ReadNodeFromStream(reader, nodeIndex, count, tuplesListNodeAndRandomNodeIndex);
            }

            if(reader.PeekChar() != -1)
            {
                throw new IncorrectCountOfRandomListInStreamException("Wrong Count of ListRandom in stream");
            }
            return tuplesListNodeAndRandomNodeIndex;
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
        }

        /// <summary>
        /// Create Dependency between a node and its random one
        /// </summary>
        private static void ConnectNodeWithRandomNode(List<Tuple<ListNode, int>> tuplesListNodeAndRandomNodeIndex, int nodeIndex)
        {
            int randomNodeNumber = tuplesListNodeAndRandomNodeIndex[nodeIndex].Item2;

            if (randomNodeNumber == -2)
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
            ListNode listNode = new ListNode() { Data = null};
            int randomNodeIndex = -2;

            for (int stepNumber = 0; stepNumber < 2; stepNumber++)
            {
                int nextChar = reader.PeekChar();

                if (nextChar is -1)
                {
                    throw new EmptyStreamWhileReadingNodeException("Empty stream while reading Data");
                }
                else if (nextChar is -2)
                {
                    reader.ReadInt32();
                }
                else
                {
                    if (stepNumber == 0)
                    {
                        listNode.Data = reader.ReadString();
                    }
                    else
                    {
                        randomNodeIndex = reader.ReadInt32();

                        if (randomNodeIndex >= count)
                        {
                            throw new IndexOutOfRangeException($"Index = {randomNodeIndex} of RandomNode for ListNode with index {nodeIndex} more than ListRandom Count = {count}");
                        }
                    }
                }
            }

            tuplesListNodeAndRandomNodeIndex.Add(new Tuple<ListNode,int>(listNode, randomNodeIndex));
            return listNode;
        }
    }
}

