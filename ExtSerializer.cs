using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;
.........
namespace Utils
{

    public enum SerializeType
    {
        XML,
        JSON,
        Binary,
        SOAP
    }
    public static class ExtSerializer
    {
        public static string Serialize(this object obj, SerializeType ST)
        {
            switch (ST)
            {
                case SerializeType.JSON:
                    {
                        return ToJson(obj);
                    }
                case SerializeType.SOAP:
                    {
                        return ToSOAP(obj);
                    }
                case SerializeType.Binary:
                    {
                        return ToBinary(obj);
                    }
                case SerializeType.XML:
                    {
                        return ToXml(obj);
                    }
                default:
                    {
                        return ToXml(obj);
                    }
            }

        }


        public static string ToBinary<T>(T obj)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    new BinaryFormatter().Serialize(stream, obj);
                    return Convert.ToBase64String(stream.ToArray());
                }
            }
            catch (Exception er) { return er.Message; }
        }

        public static string ToJson<T>(T obj)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(obj.GetType());
                ser.WriteObject(ms, obj);
                return Encoding.Default.GetString(ms.ToArray());
            }
            catch (Exception er) { return er.Message; }
        }
        public static string ToSOAP<T>(T obj)
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
        public static string ToXml<T>(T obj)
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

        public static T Deserialize<T>(this T obj, string text) where T : class
        {
            var v = FromXml<T>(ref obj, text);
            if (v != default(T))
                return v;
            else
            {
                v = FromBinary<T>(ref obj, text);
                if (v != default(T))
                    return v;
                else
                {
                    v = FromJson<T>(ref obj, text);
                    if (v != default(T))
                        return v;
                    else
                    {
                        v = FromSOAP<T>(ref obj, text);
                        if (v != default(T))
                            return v;
                        else
                        {

                            return default;
                        }
                    }

                }
            }
        }


        public static T FromXml<T>(ref T obj, string text)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));

                using (TextReader reader = new StringReader(text))
                {
                    return (T)serializer.Deserialize(reader);
                }
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static T FromBinary<T>(ref T obj, string text)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(text);

                MemoryStream stream = new MemoryStream(bytes);

                BinaryFormatter bformatter = new BinaryFormatter();


                obj = (T)bformatter.Deserialize(stream);
                return obj;
            }
            catch (Exception)
            {
                return default;
            }
        }
        public static T FromJson<T>(ref T obj, string text)
        {
            try
            {
                MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(text));

                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                ms.Position = 0;
                obj = (T)ser.ReadObject(ms);
                return obj;
            }
            catch (Exception)
            {
                return default;

            }
        }

        public static T FromSOAP<T>(ref T obj, string text)
        {
            try
            {
                MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(text));

                SoapFormatter formatter = new SoapFormatter();

                obj = (T)formatter.Deserialize(ms);
                return obj;
            }
            catch (Exception)
            {
                return default;

            }
        }
    }
}
