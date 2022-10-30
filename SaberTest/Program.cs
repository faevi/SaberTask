using System.Collections.Generic;
using System.IO;
using System.Text;
using SaberTest.Models;



using (FileStream fs = File.Create("test2.txt"))
{
    using (var writer = new BinaryWriter(fs))
    {
        writer.Write("haaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
    }
}



using (FileStream s = File.OpenRead("test2.txt"))
{
    using (var reader = new BinaryReader(s))
    {
        Console.WriteLine(reader.PeekChar());
        Console.WriteLine(reader.ReadString());
        Console.WriteLine(reader.PeekChar());
    }
}
