﻿using SaberTest.Models;

namespace SaberTest
{
    internal class Program
    {
        private static void ShowListRandom(ListRandom listRandom)
        {
            ListNode listNode = listRandom.Head;

            for (int listNodeIndex = 0; listNodeIndex < listRandom.Count; listNodeIndex++)
            {
                string listNodeData = listNode.Data is not null ? listNode.Data : "Nullable";
                string listNodeRandomInfo = "has random node Data: ";

                if (listNode.Random is not null)
                {
                    listNodeRandomInfo += listNode.Random.Data is not null ? listNode.Random.Data : "Nullable";
                }
                else
                {
                    listNodeRandomInfo = "hasn't random node";
                }

                Console.WriteLine($"Node {listNodeIndex}, has Data: {listNodeData}, {listNodeRandomInfo}");
                listNode = listNode.Next;
            }
        }

        public static void Main(string[] args)
        {
            ListNode node1 = new ListNode() { Data = "Hi" };
            ListNode node2 = new ListNode() { Data = "Hello" };
            ListNode node3 = new ListNode() { Data = null };
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
            node3.Random = null;
            node4.Random = node1;
            node5.Random = node3;
            ListRandom listRandom = new ListRandom() { Head = node1, Tail = node5, Count = 5 };

            Console.WriteLine("ListRandom before serialization");
            ShowListRandom(listRandom);

            using (FileStream stream = File.Create("test.txt"))
            {
                listRandom.Serialize(stream);
            }

            using (FileStream stream = File.OpenRead("test.txt"))
            {
                listRandom.Deserialize(stream);
            }

            File.Delete("test.txt");
            Console.WriteLine("ListRandom after deserialization");
            ShowListRandom(listRandom);
            Console.Read();
        }
    }
}
