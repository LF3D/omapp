using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.IO;

namespace OperatingManagement.ServicesKernel.File
{
    public class XMLFileHandle
    {

        #region -Properties-
        private XmlDocument XmlDoc = null;
        private static object configLocker = new object();
        private static XMLFileHandle _config = null;
        private string _path = null;
        #endregion

        public XMLFileHandle(XmlDocument doc)
        {
            XmlDoc = doc;
            LoadValuesFromConfigurationXml();
        }

        private void LoadValuesFromConfigurationXml()
        {
            XmlNode node = GetConfigSection("privacy/core");
            XmlAttributeCollection attributeCollection = node.Attributes;
            for (int i = 0; i < attributeCollection.Count; i++)
            {
                attr.Add(attributeCollection[i].Name, attributeCollection[i].Value);
            }
        }

        #region -Indexer-
        private Dictionary<string, object> attr = new Dictionary<string, object>();
        /// <summary>
        /// Gets the value from congiguration settings by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get
            {
                if (attr.ContainsKey(name))
                    return attr[name];
                else
                    return string.Empty;
            }
        }
        #endregion

        public XMLFileHandle Config
        {
            get
            {
                if (_config == null)
                {
                    lock (configLocker)
                    {
                        if (_config == null)
                        {
                            string path = _path;
                            XmlDocument doc = new XmlDocument();
                            doc.Load(path);
                            _config = new XMLFileHandle(doc);
                        }
                    }
                }
                return _config;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodePath">Path</param>
        /// <returns></returns>
        public XmlNode GetConfigSection(string nodePath)
        {
            return XmlDoc.SelectSingleNode(nodePath);
        }

        public static string PhysicalPath(string path)
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            char dirSep = Path.DirectorySeparatorChar;
            rootPath = rootPath.Replace("/", dirSep.ToString());
            return string.Concat(rootPath.TrimEnd(dirSep), dirSep, path.TrimStart(dirSep));
        }

        #region -path-
        public string FilePath
        {
            get
            {
                return PhysicalPath(_path.Replace("/", Path.DirectorySeparatorChar.ToString()).Replace("~", ""));
            }
            set
            {
                _path = value;
            }
        }
        #endregion


        #region --== Validate XML Formate ==--

        static int numErrors = 0;
        static string msgError = "";

        private static void ErrorHandler(object sender, ValidationEventArgs args)
        {
            msgError = msgError + "\r\n" + args.Message;
            numErrors++;
        }
        private static bool Validate(string xmlFilename, string xsdFilename)
        {
            return Validate(GetFileStream(xmlFilename), GetFileStream(xsdFilename));
        }

        private static void ClearErrorMessage()
        {
            msgError = "";
            numErrors = 0;
        }   // returns a stream of the contents of the given filename     

        private static Stream GetFileStream(string filename)
        {
            try
            {
                return new FileStream(filename, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                return null;
            }
        }

        private static bool Validate(Stream xml, Stream xsd)
        {
            ClearErrorMessage();
            try
            {
                XmlTextReader tr = new XmlTextReader(xsd);
                XmlSchemaSet schema = new XmlSchemaSet();
                schema.Add(null, tr);
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add(schema);
                settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
                settings.ValidationEventHandler += new ValidationEventHandler(ErrorHandler);

                XmlReader reader = XmlReader.Create(xml, settings);  // Validate XML data   
                while (reader.Read()) ;
                reader.Close();                // exception if validation failed                   
                if (numErrors > 0)
                    throw new Exception(msgError);
                tr.Close();
                return true;
            }
            catch
            {
                msgError = "Validation failed\r\n" + msgError;
                return false;
            }
        }

        #endregion
    }
}
