namespace SaberTest.Models
{
    public static class ListRandomConsoleDisplayer
    {
        /// <summary>
        /// Creates test List Random Count 5 wirh Loops in second node, null data in third node, null Random node in third node
        /// </summary>
        public static ListRandom CreateConsoleTestListRandom()
        {
            ListNode node1 = new ListNode() { Data = "Hi" };
            ListNode node2 = new ListNode() { Data = "Hello" };
            ListNode node3 = new ListNode() { Data = null };
            ListNode node4 = new ListNode() { Data = "Bonjur" };
            ListNode node5 = new ListNode() { Data = "Nihao" };
            node1.Next = node2;
            node2.Next = node1;
            node3.Next = node4;
            node4.Next = node5;
            node2.Previous = node1;
            node3.Previous = node2;
            node4.Previous = node3;
            node5.Previous = node4;
            node1.Random = node4;
            node2.Random = node4;
            node3.Random = null;
            node4.Random = node1;
            node5.Random = node3;
            return new ListRandom() { Head = node1, Tail = node5, Count = 5 };
        }

        /// <summary>
        /// Display ListRandom in Console 
        /// </summary>
        public static void ShowListRandom(ListRandom listRandom)
        {
            ListNode listNode = listRandom.Head;
            string listNodeData;
            string listNodeRandomInfo;
            string listNodeNextInfo;
            string listNodePrevInfo;
            int listNodeIndex = 0;
            HashSet<ListNode> alreadyVistedNodes = new HashSet<ListNode>();

            while (listNode is not null)
            {
                Console.WriteLine(GetNodeInfo(listNode, listNodeIndex++));
                alreadyVistedNodes.Add(listNode);
                listNode = listNode.Next;

                if (listNode is null || alreadyVistedNodes.Contains(listNode))
                {
                    listNode = listRandom.Tail;
                    listNodeIndex = listRandom.Count - 1;

                    while (listNode != null && !alreadyVistedNodes.Contains(listNode))
                    {
                        alreadyVistedNodes.Add(listNode);
                        Console.WriteLine(GetNodeInfo(listNode, listNodeIndex--));
                        listNode = listNode.Previous;
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Retrun ListNodeInfo
        /// </summary>
        private static string GetNodeInfo(ListNode listNode, int listNodeIndex)
        {
            string listNodeData = listNode.Data is not null ? listNode.Data : "Nullable";
            return $"Node {listNodeIndex}, has Data: {listNodeData}, {GetRandomNodeInfo(listNode)}, {GetNextNodeInfo(listNode)}, {GetPrevNodeInfo(listNode)}"; 
        }

        /// <summary>
        /// Return info about Previous Node
        /// </summary>
        private static string GetPrevNodeInfo(ListNode listNode)
        {
            string listNodePrevInfo = "has previous node with Data: ";

            if (listNode.Previous is not null)
            {
                listNodePrevInfo += listNode.Previous.Data is not null ? listNode.Previous.Data : "Nullable";
            }
            else
            {
                listNodePrevInfo = "hasn't previous node";
            }

            return listNodePrevInfo;
        }

        /// <summary>
        /// Reurn info about Next Node
        /// </summary>
        private static string GetNextNodeInfo(ListNode listNode)
        {
            string listNodeNextInfo = "has next node with Data: ";

            if (listNode.Next is not null)
            {
                listNodeNextInfo += listNode.Next.Data is not null ? listNode.Next.Data : "Nullable";
            }
            else
            {
                listNodeNextInfo = "hasn't next node";
            }

            return listNodeNextInfo;
        }

        /// <summary>
        /// Reurn info about Random Node
        /// </summary>
        private static string GetRandomNodeInfo(ListNode listNode)
        {
            string listNodeRandomInfo = "has random node with Data: ";

            if (listNode.Random is not null)
            {
                listNodeRandomInfo += listNode.Random.Data is not null ? listNode.Random.Data : "Nullable";
            }
            else
            {
                listNodeRandomInfo = "hasn't random node";
            }

            return listNodeRandomInfo;
        }
    }
}