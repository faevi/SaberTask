using SaberTest.Models;

namespace SaberTestUnitTests
{
    [TestClass]
    public class ListRandomSerializeTests
    {
        /// <summary>
        /// Generates ListRandom with random Data
        /// </summary>
        private ListRandom GenerateListRandom(int count)
        {
            var rand = new Random();
            ListRandom listRandom = new ListRandom() { Count = count };
            List<ListNode> listNodes = new List<ListNode>(count);

            for (int listNodeIndex = 0; listNodeIndex < count; listNodeIndex++)
            {
                ListNode listNode = new ListNode() { Data = rand.Next(count).ToString() };
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

        [DataTestMethod]
        [DataRow(21)]
        [DataRow(31)]
        [DataRow(56)]
        [DataRow(1)]
        [DataRow(2)]
        public void SimpleSerializeDesirializeTests(int count)
        {
            ListRandom listRandom = GenerateListRandom(count);

            using (FileStream fs = File.Create("test.txt"))
            {
                listRandom.Serialize(fs);
            }

            using (FileStream s = File.OpenRead("test.txt"))
            {
                ListRandom list = new ListRandom();
                list.Deserialize(s);
                Assert.AreEqual(count, list.Count);
            }
        }
    }
}


