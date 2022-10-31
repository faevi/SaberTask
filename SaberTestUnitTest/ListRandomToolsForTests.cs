using SaberTest.Models;

namespace SaberTestUnitTests
{
    public static class ListRandomToolsForTests
    {
        /// <summary>
        /// Copy Count, Head and Tail to new ListRandom
        /// </summary>
        public static ListRandom CreateCopyOfListRandom(ListRandom listRandom)
        {
            ListNode newHead = new ListNode();
            ListNode newTail = new ListNode();
            CopyNodeDataToOther(newHead, listRandom.Head);
            CopyNodeDataToOther(newTail, listRandom.Tail);
            return new ListRandom { Head = newHead, Tail = newTail, Count = listRandom.Count };

        }

        /// <summary>
        /// Copy node to other
        /// </summary>
        public static void CopyNodeDataToOther(ListNode node, ListNode nodeToCopy)
        {
            node.Data = nodeToCopy.Data;
            node.Next = nodeToCopy.Next;
            node.Previous = nodeToCopy.Previous;
            node.Random = nodeToCopy.Random;
        }

        /// <summary>
        /// Compare two nodes data
        /// </summary>
        public static bool AreTwoNodesDataEqual(ListNode firstNode, ListNode secondNode)
        {
            if (firstNode.Data == secondNode.Data)
            {
                if (firstNode.Random is not null && firstNode.Random.Data != secondNode.Random.Data)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Copare two ListRandom Data
        /// </summary>
        public static bool AreTwoListRandomEqual(ListRandom firstListRandom, ListRandom secondListRandom)
        {
            if (firstListRandom.Count != secondListRandom.Count)
            {
                return false;
            }

            HashSet<ListNode> alreadyVisitedNodes = new HashSet<ListNode>();

            ListNode firstTempNode = firstListRandom.Head;
            ListNode secondTempNode = secondListRandom.Head;

            while (!secondTempNode.Next.Equals(secondListRandom.Tail))
            {
                firstTempNode = firstTempNode.Next;
                secondTempNode = secondTempNode.Next;

                if (secondTempNode is null || alreadyVisitedNodes.Contains(secondTempNode))
                {
                    firstTempNode =   firstListRandom.Tail;
                    secondTempNode = secondListRandom.Tail;

                    while (secondTempNode.Previous != null && !alreadyVisitedNodes.Contains(secondTempNode.Previous))
                    {
                        firstTempNode = firstTempNode.Previous;
                        secondTempNode = secondTempNode.Previous;
                        AreTwoNodesDataEqual(firstTempNode, secondTempNode);
                        alreadyVisitedNodes.Add(secondTempNode);
                    }

                    break;
                }

                AreTwoNodesDataEqual(firstTempNode, secondTempNode);
                alreadyVisitedNodes.Add(secondTempNode);
            }

            return true;
        }

        /// <summary>
        /// Crete basic listRandom with Count 5
        /// </summary>
        public static ListRandom CreateBasicListRandomCount5()
        {
            ListNode node1 = new ListNode() { Data = "Hi" };
            ListNode node2 = new ListNode() { Data = "Hello" };
            ListNode node3 = new ListNode() { Data = "Hola" };
            ListNode node4 = new ListNode() { Data = "Bonjur" };
            ListNode node5 = new ListNode() { Data = "Nihao" };
            node1.Next = node2;
            node2.Next = node3;
            node3.Next = node4;
            node4.Next = node5;
            node2.Previous = node1;
            node3.Previous = node2;
            node4.Previous = node3;
            node5.Previous = node4;
            node1.Random = node4;
            node2.Random = node4;
            node3.Random = node5;
            node4.Random = node1;
            node5.Random = node3;
            ListRandom listRandom = new ListRandom() { Head = node1, Tail = node5, Count = 5 };
            return listRandom;
        }

        /// <summary>
        /// Generates ListRandom with random Data
        /// </summary>
        public static ListRandom GenerateListRandom(int count)
        {
            var rand = new Random();
            ListRandom listRandom = new ListRandom() { Count = count };
            List<ListNode> listNodes = new List<ListNode>(count);

            for (int listNodeIndex = 0; listNodeIndex < count; listNodeIndex++)
            {
                ListNode listNode = new ListNode() { Data = rand.Next(count * 1000).ToString() };
                listNodes.Add(listNode);
            }

            listRandom.Head = listNodes.First();
            listRandom.Tail = listNodes.Last();
            listRandom.Tail.Random = listNodes[rand.Next(count)];

            for (int listNodeIndex = 0; listNodeIndex < count - 1; listNodeIndex++)
            {
                listNodes[listNodeIndex].Next = listNodes[listNodeIndex + 1];
                listNodes[listNodeIndex + 1].Previous = listNodes[listNodeIndex];
                listNodes[listNodeIndex].Random = listNodes[rand.Next(count)];
            }

            return listRandom;
        }
    }
}