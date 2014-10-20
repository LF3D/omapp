using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Collections.Specialized;
using System.Globalization;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

namespace OperatingManagement.Framework.Helper
{
    public class Serializer
    {
        /// <summary>
        /// Whether binary seriablizable.
        /// </summary>
        public static readonly bool CanBinarySerialize;

        #region -Constructor-
        /// <summary>
        /// Create a new instance of <see cref="OperatingManagement.Framework.Helper.Serializer"/>
        /// </summary>
        private Serializer() { }

        static Serializer()
        {
            SecurityPermission permission = new SecurityPermission(SecurityPermissionFlag.SerializationFormatter);
            try
            {
                permission.Demand();
                CanBinarySerialize = true;
            }
            catch
            {
                CanBinarySerialize = false;
            }
        }
        #endregion

        #region -Save-
        /// <summary>
        /// Save the object as binary. 
        /// </summary>
        /// <param name="objectToSave">Object</param>
        /// <param name="path">FilePath</param>
        /// <returns></returns>
        public static bool SaveAsBinary(object objectToSave, string path)
        {
            if ((objectToSave != null) && CanBinarySerialize)
            {
                byte[] buffer = ConvertToBytes(objectToSave);
                if (buffer != null)
                {
                    using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        using (BinaryWriter writer = new BinaryWriter(stream))
                        {
                            writer.Write(buffer);
                            return true;
                        }
                    }
                }
            }
            return false;

        }
        /// <summary>
        /// Save the object as xml.
        /// </summary>
        /// <param name="objectToConvert">Object</param>
        /// <param name="path">FilePath</param>
        public static void SaveAsXML(object objectToConvert, string path)
        {
            if (objectToConvert != null)
            {
                XmlSerializer serializer = new XmlSerializer(objectToConvert.GetType());
                using (StreamWriter writer = new StreamWriter(path))
                {
                    serializer.Serialize((TextWriter)writer, objectToConvert);
                    writer.Close();
                }
            }

        }
        #endregion

        #region -Convert-
        /// <summary>
        /// Convert the object to byte array.
        /// </summary>
        /// <param name="objectToConvert">Object</param>
        /// <returns></returns>
        public static byte[] ConvertToBytes(object objectToConvert)
        {
            byte[] buffer = null;
            if (CanBinarySerialize)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (MemoryStream stream = new MemoryStream())
                {
                    formatter.Serialize(stream, objectToConvert);
                    stream.Position = 0L;
                    buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    stream.Close();
                }
            }
            return buffer;
        }
        /// <summary>
        /// Convert the string to byte object.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] ConvertToBytes(string s)
        {
            return Convert.FromBase64String(s);
        }

        /// <summary>
        /// Serialize as xml
        /// </summary>
        /// <param name="objectToConvert">Object</param>
        /// <returns></returns>
        public static string ConvertToString(object objectToConvert)
        {
            string str = null;
            if (objectToConvert != null)
            {
                XmlSerializer serializer = new XmlSerializer(objectToConvert.GetType());
                using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
                {
                    serializer.Serialize((TextWriter)writer, objectToConvert);
                    str = writer.ToString();
                    writer.Close();
                }
            }
            return str;

        }
        /// <summary>
        /// Serialize as xml
        /// </summary>
        /// <param name="arr">Byte Array</param>
        /// <returns></returns>
        public static string ConvertToString(byte[] arr)
        {
            return Convert.ToBase64String(arr);
        }

        /// <summary>
        /// Convert the byte array to object
        /// </summary>
        /// <param name="byteArray">Byte array</param>
        /// <returns></returns>
        public static object ConvertToObject(byte[] byteArray)
        {
            object obj2 = null;
            if ((CanBinarySerialize && (byteArray != null)) && (byteArray.Length > 0))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (MemoryStream stream = new MemoryStream())
                {
                    stream.Write(byteArray, 0, byteArray.Length);
                    stream.Position = 0L;
                    if (byteArray.Length > 4)
                    {
                        obj2 = formatter.Deserialize(stream);
                    }
                    stream.Close();
                }
            }
            return obj2;

        }

        /// <summary>
        /// Convert the file to object.
        /// </summary>
        /// <param name="path">XML file path</param>
        /// <param name="objectType">Object type</param>
        /// <returns></returns>
        public static object ConvertFileToObject(string path, Type objectType)
        {
            object obj2 = null;
            if ((path != null) && (path.Length > 0))
            {
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    obj2 = new XmlSerializer(objectType).Deserialize(stream);
                    stream.Close();
                }
            }
            return obj2;

        }

        /// <summary>
        /// Convert the xml to object.
        /// </summary>
        /// <param name="xml">XML string</param>
        /// <param name="objectType">Object type</param>
        /// <returns></returns>
        public static object ConvertToObject(string xml, Type objectType)
        {
            object obj2 = null;
            if (!string.IsNullOrEmpty(xml))
            {
                using (StringReader reader = new StringReader(xml))
                {
                    XmlSerializer serializer = new XmlSerializer(objectType);
                    try
                    {
                        obj2 = serializer.Deserialize(reader);
                    }
                    catch (InvalidOperationException)
                    {
                    }
                    reader.Close();
                }
            }
            return obj2;

        }

        /// <summary>
        /// Convert the xml to object
        /// </summary>
        /// <param name="xml">XML Node</param>
        /// <param name="objectType">Object type</param>
        /// <returns></returns>
        public static object ConvertToObject(XmlNode node, Type objectType)
        {
            object obj2 = null;
            if (node != null)
            {
                using (StringReader reader = new StringReader(node.OuterXml))
                {
                    XmlSerializer serializer = new XmlSerializer(objectType);
                    try
                    {
                        obj2 = serializer.Deserialize(reader);
                    }
                    catch (InvalidOperationException)
                    {
                    }
                    reader.Close();
                }
            }
            return obj2;

        }
        /// <summary>
        /// Load data from file
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns></returns>
        public static object LoadBinaryFile(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                BinaryReader reader = new BinaryReader(stream);
                byte[] buffer = new byte[stream.Length];
                reader.Read(buffer, 0, (int)stream.Length);
                return ConvertToObject(buffer);
            }

        }
        #endregion

        #region -NameValueCollection-
        /// <summary>
        /// Deserialize NameValueCollection
        /// </summary>
        /// <param name="keys">Keys</param>
        /// <param name="values">Values</param>
        /// <returns>NVC</returns>
        /// <example>
        /// string keys = "key1:S:0:3:key2:S:3:2:";
        /// string values = "12345";
        /// it will returns NVC[keys: (Key1,Key2) and values: (123,45)]
        /// </example>
        public static NameValueCollection ConvertToNameValueCollection(string keys, string values)
        {
            NameValueCollection values2 = new NameValueCollection();
            if (((keys != null) && (values != null)) && ((keys.Length > 0) && (values.Length > 0)))
            {
                char[] separator = new char[] { ':' };
                string[] strArray = keys.Split(separator);
                for (int i = 0; i < (strArray.Length / 4); i++)
                {
                    int startIndex = int.Parse(strArray[(i * 4) + 2], CultureInfo.InvariantCulture);
                    int length = int.Parse(strArray[(i * 4) + 3], CultureInfo.InvariantCulture);
                    string str = strArray[i * 4];
                    if (((strArray[(i * 4) + 1] == "S") && (startIndex >= 0)) && (values.Length >= (startIndex + length)))
                    {
                        values2[str] = values.Substring(startIndex, length);
                    }
                }
            }
            return values2;
        }

        /// <summary>
        /// Convert NVC to Keys&&Values pair
        /// </summary>
        /// <param name="nvc">NVC</param>
        /// <param name="keys">Keys</param>
        /// <param name="values">Values</param>
        public static void ConvertFromNameValueCollection(NameValueCollection nvc, ref string keys, ref string values)
        {
            ConvertFromNameValueCollection(nvc, ref keys, ref values, false);

        }
        /// <summary>
        /// Serialize keys&&values pair as NVC.
        /// </summary>
        /// <param name="nvc">NVC</param>
        /// <param name="keys">Keys</param>
        /// <param name="values">Values</param>
        /// <param name="allowEmptyStrings">Whether allow empty strings.</param>
        public static void ConvertFromNameValueCollection(NameValueCollection nvc, ref string keys, ref string values, bool allowEmptyStrings)
        {
            if ((nvc != null) && (nvc.Count != 0))
            {
                StringBuilder builder = new StringBuilder();
                StringBuilder builder2 = new StringBuilder();
                int num = 0;
                foreach (string str in nvc.AllKeys)
                {
                    if (str.IndexOf(':') != -1)
                    {
                        throw new ArgumentException("ExtendedAttributes keys does not contains \":\".");
                    }
                    string text = nvc[str];
                    if (allowEmptyStrings && !string.IsNullOrEmpty(text))
                    {
                        builder.AppendFormat("{0}:S:{1}:{2}:", str, num, text.Length);
                        builder2.Append(text);
                        num += text.Length;
                    }
                }
                keys = builder.ToString();
                values = builder2.ToString();
            }

        }
        #endregion
    }
}
