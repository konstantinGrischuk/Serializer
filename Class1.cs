using System;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Windows.Forms;

namespace Utils
{
    public class Serialize
    {


        //      public enum To { Binary, Xml }
        public class To
        {
            public static string Xml<T>(T obj)
            {

                try
                {
                    var serializer = new XmlSerializer(obj.GetType());
                    StringBuilder sb = new StringBuilder();
                    StringWriter w = new StringWriter(sb, System.Globalization.CultureInfo.InvariantCulture);
                    serializer.Serialize(w, obj);
                    w.Close();
                    return sb.ToString();
                }
                catch (Exception er) { return er.Message; }
            }

            public static string Binary<T>(T obj)
            {
                try
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        new BinaryFormatter().Serialize(stream, obj);
                        // return Encoding.Default.GetString(stream.ToArray());
                        return Convert.ToBase64String(stream.ToArray());
                    }
                }
                catch (Exception er) { return er.Message; }
            }

            public static string Json<T>(T obj)
            {
                try
                {
                    MemoryStream ms = new MemoryStream();
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                    ser.WriteObject(ms, obj);
                    return Encoding.Default.GetString(ms.ToArray());
                }
                catch (Exception er) { return er.Message; }
            }
            public static string SOAP<T>(T obj)
            {
                MemoryStream ms = new MemoryStream();
                try
                {
                    SoapFormatter soap = new SoapFormatter();

                    soap.Serialize(ms, obj);
                    return Encoding.Default.GetString(ms.ToArray());
                }
                catch (Exception er) { return er.Message; }
            }
        }

        public class From
        {

            public static void Xml<T>(ref T obj, string text)
            {
                try
                {
                    var serializer = new XmlSerializer(typeof(T));

                    using (TextReader reader = new StringReader(text))
                    {                            
                        obj= (T)serializer.Deserialize(reader); 
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show("ERR " + er.Message);
                }
            }
          
            public static void   Binary<T>(ref T obj, string text)
            {
                try
                {
                    byte[] bytes = Convert.FromBase64String(text);

                    MemoryStream stream = new MemoryStream(bytes);

                    BinaryFormatter bformatter = new BinaryFormatter();


                    obj= (T)bformatter.Deserialize(stream);
                }
                catch (Exception er)
                {
                    MessageBox.Show("ERR " + er.Message);
                }
            }
            public static void Json<T>(ref T obj, string text)
            {
                try
                {
                    MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(text));

                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                    ms.Position = 0;
                    obj= (T)ser.ReadObject(ms);

                }
                catch (Exception er) { MessageBox.Show("ERR " + er.Message); }
            }

            public static void SOAP<T>(ref T obj, string text)
            {
                try
                {
                    MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(text));

                    SoapFormatter formatter = new SoapFormatter();                 

                    obj=   (T) formatter.Deserialize(ms);

                }
                catch (Exception er) { MessageBox.Show("ERR " + er.Message);  }
            }
        }
    }
}




