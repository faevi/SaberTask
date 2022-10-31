using SaberTest.Models;
using SaberTest.Exceptions;

namespace SaberTestUnitTests
{
    [TestClass]
    public class ListRandomTests
    {
        [DataTestMethod]
        [DataRow(21)]
        [DataRow(1000)]
        [DataRow(56)]
        [DataRow(5)]
        [DataRow(2)]
        public void SimpleSerializeDesirializeListRandomTests(int count)
        {
            ListRandom listRandom = ListRandomToolsForTests.GenerateListRandom(count);
            ListRandom oldListRandom = ListRandomToolsForTests.CreateCopyOfListRandom(listRandom);
            CheckSerializeDesirializeListRandom(listRandom, oldListRandom);
        }

        [TestMethod]
        public void SerializeDesirializeListRandomTest_Count5_3NodeRandomNull_2NodeDataNull()
        {
            ListRandom listRandom = ListRandomToolsForTests.CreateBasicListRandomCount5();
            listRandom.Head.Next.Next.Random = null;
            listRandom.Head.Next.Data = null;
            ListRandom oldListRandom = ListRandomToolsForTests.CreateCopyOfListRandom(listRandom);
            CheckSerializeDesirializeListRandom(listRandom, oldListRandom);
        }

        [TestMethod]
        public void SerializeDesirializeListRandomTest_Count5_TailNull()
        {
            ListRandom listRandom = ListRandomToolsForTests.CreateBasicListRandomCount5();
            listRandom.Tail = null;
            ListRandom oldListRandom = ListRandomToolsForTests.CreateBasicListRandomCount5();

            Assert.ThrowsException<NullReferenceException>(() => CheckSerializeDesirializeListRandom(listRandom, oldListRandom));
        }

        [TestMethod]
        public void SerializeDesirializeListRandomTest_Count5_WithLoop()
        {
            ListRandom listRandom = ListRandomToolsForTests.CreateBasicListRandomCount5();
            listRandom.Head.Next.Next = listRandom.Head;
            ListRandom oldListRandom = ListRandomToolsForTests.CreateCopyOfListRandom(listRandom);

            CheckSerializeDesirializeListRandom(listRandom, oldListRandom);
        }

        [TestMethod]
        public void SerializeDesirializeListRandomTest_Count1()
        {
            ListNode node = new ListNode { Data = "Hi" };
            node.Next = node;
            node.Previous = node;
            node.Random = node;

            ListRandom listRandom = new ListRandom { Head = node, Count = 1, Tail = node };
            ListRandom oldListRandom = ListRandomToolsForTests.CreateCopyOfListRandom(listRandom);

            CheckSerializeDesirializeListRandom(listRandom, oldListRandom);
        }

        [TestMethod]
        public void SerializeDesirializeListRandomTest_EmptyStream()
        {
            ListRandom listRandom = ListRandomToolsForTests.CreateBasicListRandomCount5();
            ListRandom oldListRandom = ListRandomToolsForTests.CreateBasicListRandomCount5();

            using (FileStream stream = File.Create("test.txt"))
            {

            }

            using (FileStream stream = File.OpenRead("test.txt"))
            {
                Assert.ThrowsException<ArgumentException>(() => listRandom.Deserialize(stream));
            }

            File.Delete("test.txt");
        }

        /// <summary>
        /// Assert if one list data equal to other
        /// </summary>
        private void CheckSerializeDesirializeListRandom(ListRandom listRandom , ListRandom oldListRandom)
        {
            using (FileStream stream = File.Create("test.txt"))
            {
                listRandom.Serialize(stream);
            }

            using (FileStream stream = File.OpenRead("test.txt"))
            {
                listRandom.Deserialize(stream);
                Assert.IsTrue(ListRandomToolsForTests.AreTwoListRandomEqual(oldListRandom, listRandom));
            }

            File.Delete("test.txt");
        }
    }
}


