using System.Collections.Generic;
using System.IO;
using System.Text;
using SaberTest.Models;

using (FileStream fs = File.Create("test.txt"))
{
    ListNode node1 = new ListNode() { Data = "Hi" };
    ListNode node2 = new ListNode() { Data = "Hello" };
    ListNode node3 = new ListNode() { Data = "Hola" };
    ListNode node4 = new ListNode() { Data = "Bonjur" };
    node1.Next = node2;
    node2.Next = node3;
    node3.Next = node4;
    node2.Previous = node1;
    node3.Previous = node2;
    node4.Previous = node3;
    node1.Random = node4;
    node3.Random = node2;
    node2.Random = node4;
    node4.Random = node1;
    ListRandom list = new ListRandom() { Head = node1, Tail = node4, Count = 4 };
    list.Serialize(fs);
}



using (FileStream s = File.OpenRead("test.txt"))
    {
    ListRandom list = new ListRandom();
        list.Deserialize(s);
        Console.WriteLine(list.Count);
    }
