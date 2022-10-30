using System.IO;
using SaberTest.Exceptions;

namespace SaberTest.Models
{
    public class ListRandomDeserialize
    {
        private struct NodeConnectionInfo
        {
            public NodeConnectionInfo(int NodeIndex, int RandomNodeIndex, int NextNodeIndex, int PrevNodeIndex)
            {
                nodeIndex = NodeIndex;
                nextNodeIndex = NextNodeIndex;
                prevNodeIndex = PrevNodeIndex;
                randomNodeIndex = RandomNodeIndex;
            }

            public int nodeIndex;
            public int nextNodeIndex;
            public int prevNodeIndex;
            public int randomNodeIndex;
        }

        public static void DeserializeListRandom(Stream stream, ListRandom listRandom)
        {
            Dictionary<ListNode, NodeConnectionInfo> listNodeToNodeConnectioInfo;
            CheckArgumentsDeserialize(stream);
            listNodeToNodeConnectioInfo = ReadListRandomFromStream(stream, listRandom);
            ConnectAllNodes(listNodeToNodeConnectioInfo, listRandom);
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
        private static Dictionary<ListNode, NodeConnectionInfo> ReadListRandomFromStream(Stream stream, ListRandom listRandom)
        {
            using (var reader = new BinaryReader(stream))
            {
                int count = reader.ReadInt32();
                listRandom.Count = count;
                Dictionary<ListNode, NodeConnectionInfo> listNodeToNodeConnectioInfo = new Dictionary<ListNode, NodeConnectionInfo>();

                for (int nodeIndex = 0; nodeIndex < count; nodeIndex++)
                {
                    ReadNodeFromStream(reader, listNodeToNodeConnectioInfo);
                }

                if (reader.PeekChar() != -1)
                {
                    throw new IncorrectCountListRanodmException("Wrong Count of ListRandom in stream");
                }

                return listNodeToNodeConnectioInfo;
            }
        }

        /// <summary>
        /// Create dictionary index nodes to nodes to optimize searching nodes while connection
        /// </summary>
        private static Dictionary<int, ListNode> CreateDictionaruIndexNodesToListNodes(Dictionary<ListNode, NodeConnectionInfo> listNodeToNodeConnectioInfo)
        {
            Dictionary<int, ListNode> indexNodesToListNodes = new Dictionary<int, ListNode>();

            foreach (KeyValuePair<ListNode, NodeConnectionInfo> pair in listNodeToNodeConnectioInfo)
            {
                indexNodesToListNodes.Add(pair.Value.nodeIndex, pair.Key);
            }

            return indexNodesToListNodes;
        }

        /// <summary>
        /// Create dependencies between Nodes in ListRandom
        /// </summary>
        private static void ConnectAllNodes(Dictionary<ListNode, NodeConnectionInfo> listNodeToNodeConnectioInfo, ListRandom listRandom)
        {
            Dictionary<int, ListNode> indexNodesToListNodes = CreateDictionaruIndexNodesToListNodes(listNodeToNodeConnectioInfo);
            listRandom.Head = indexNodesToListNodes[0];
            listRandom.Tail = indexNodesToListNodes[listNodeToNodeConnectioInfo.Count - 1];
            ListNode tempNode;
            NodeConnectionInfo tempNodeConnectionInfo;

            foreach (KeyValuePair<ListNode, NodeConnectionInfo> listNodeToNodeConnectionInfo in listNodeToNodeConnectioInfo)
            {
                ConnectNode(listNodeToNodeConnectionInfo, indexNodesToListNodes);
            }
        }

        /// <summary>
        /// Create dependencies in Node with others
        /// </summary>
        private static void ConnectNode(KeyValuePair<ListNode, NodeConnectionInfo> listNodeToNodeConnectionInfo, Dictionary<int, ListNode> indexNodesToListNodes)
        {
            ListNode listNode = listNodeToNodeConnectionInfo.Key;
            NodeConnectionInfo nodeConnectionInfo = listNodeToNodeConnectionInfo.Value;

            listNode.Next = nodeConnectionInfo.nextNodeIndex != -1 ? indexNodesToListNodes[nodeConnectionInfo.nextNodeIndex] : null;
            listNode.Previous = nodeConnectionInfo.prevNodeIndex != -1 ? indexNodesToListNodes[nodeConnectionInfo.prevNodeIndex] : null;
            listNode.Random = nodeConnectionInfo.randomNodeIndex != -1 ? indexNodesToListNodes[nodeConnectionInfo.randomNodeIndex] : null;
        }

        /// <summary>
        /// Read node from stream
        /// </summary>
        private static ListNode ReadNodeFromStream(BinaryReader reader, Dictionary<ListNode, NodeConnectionInfo> listNodeToNodeConnectioInfo)
        {
            string data = reader.ReadString();
            int nodeIndex = reader.ReadInt32();
            int randomNodeIndex = reader.ReadInt32();
            int nextNodeIndex = reader.ReadInt32();
            int prevNodeIndex = reader.ReadInt32();
            NodeConnectionInfo nodeConnectionInfo = new NodeConnectionInfo(nodeIndex, randomNodeIndex, nextNodeIndex, prevNodeIndex);
            ListNode listNode = new ListNode() { Data = data != "Nullable" ? data : null};
            listNodeToNodeConnectioInfo.Add(listNode, nodeConnectionInfo);
            return listNode;
        }
    }
}

