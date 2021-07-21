        static void Main()
        {
            Dat data = new Dat() { Name = "Ivanov Ivan Ivanovich", Age = 32 };
            Console.WriteLine("Serialized to: BIN");
            string bin = data.Serialize(SerializeType.Binary);
            Console.WriteLine(bin);
            Console.WriteLine("Serialized to: JSON");
            string json = data.Serialize(SerializeType.JSON);
            Console.WriteLine(json);
            Console.WriteLine("Serialized to: SOAP");
            string soap = data.Serialize(SerializeType.SOAP);
            Console.WriteLine(soap);
            Console.WriteLine("Serialized to: XML");
            string xml = data.Serialize(SerializeType.XML);
            Console.WriteLine(xml);

            Console.ReadLine();


            Dat a = new Dat().Deserialize(bin);
            Dat b = new Dat().Deserialize(json);
            Dat c = new Dat().Deserialize(soap);
            Dat d = new Dat().Deserialize(xml);


            Console.WriteLine("NAME: " + a.Name);
            Console.WriteLine("NAME: " + b.Name);
            Console.WriteLine("NAME: " + c.Name);
            Console.WriteLine("NAME: " + d.Name);
            Console.ReadLine();
            List<Dat> l = new List<Dat>();
            l.Add(new Dat() { Age = 56, Name = "Name 1 " });
            l.Add(new Dat() { Age = 16, Name = "Name 2 " });
            l.Add(new Dat() { Age = 27, Name = "Name 3 " });
            l.Add(new Dat() { Age = 32, Name = "Name 4 " });
            l.Add(new Dat() { Age = 11, Name = "Name 5 " });
            string sl = l.Serialize(SerializeType.XML);

            Console.WriteLine(sl);
            Console.ReadLine();
          }

    [Serializable]
    public class Dat
    {
        public string Name { set; get; }
        public int Age { set; get; }


    }


}
