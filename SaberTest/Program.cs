using SaberTest.Models;

namespace SaberTest
{
    internal class Program
    {
        public static void Main(string[] args)
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
        }
    }
}
