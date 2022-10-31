using System.Reflection.Metadata.Ecma335;
using SaberTest.Models;

namespace SaberTest
{
    internal class Program
    {
        /// <summary>
        /// This Sample shows serialize and desirialize List Random with Loop
        /// </summary>
        public static void Main(string[] args)
        {
            ListRandom listRandom = ListRandomConsoleDisplayer.CreateConsoleTestListRandom();
            Console.WriteLine("ListRandom before serialization");
            ListRandomConsoleDisplayer.ShowListRandom(listRandom);

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
            ListRandomConsoleDisplayer.ShowListRandom(listRandom);
            Console.Read();
        }
    }
}