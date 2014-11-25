using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace RxSandbox
{
    public static class XmlSerializationHelper
    {
        public static T FromXml<T>(string xml)
        {
            return (T)FromXml(typeof(T), xml);
        }

        public static object FromXml(Type type, XmlReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            return serializer.Deserialize(reader);
        }

        public static object FromXml(Type type, string xml)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            using (StringReader reader = new StringReader(xml))
            {
                return serializer.Deserialize(reader);
            }
        }

        public static string ToXml<T>(T serializationObject)
        {
            return ToXml(serializationObject, Encoding.Default, true);
        }

        public static string ToXml<T>(T serializationObject, Encoding encoding)
        {
            return ToXml(serializationObject, encoding, false);
        }

        public static string ToXml<T>(T serializationObject, Encoding encoding, bool omitXmlDeclaration)
        {
            return ToXml(typeof(T), serializationObject, encoding, omitXmlDeclaration);
        }

        public static string ToXml(Type type, object serializationObject, Encoding encoding, bool omitXmlDeclaration)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            string xml;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                writerSettings.OmitXmlDeclaration = omitXmlDeclaration;
                writerSettings.Encoding = encoding;
                writerSettings.Indent = false;

                using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, writerSettings))
                {
                    serializer.Serialize(xmlWriter, serializationObject);

                    // Get rid of encoding preamble.
                    int offset = encoding.GetPreamble().Length;
                    int count = ((int)memoryStream.Length) - offset;
                    xml = encoding.GetString(memoryStream.ToArray(), offset, count);
                }
            }

            return xml;
        }
    }
}

