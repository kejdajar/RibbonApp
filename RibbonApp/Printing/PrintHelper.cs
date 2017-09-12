using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RibbonApp.Database;
using RibbonApp.Model;
using RibbonApp.ViewModel;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Xsl;

namespace RibbonApp.Printing
{
    class PrintHelper
    {

        public PrintHelper()
        {
          
        }        

        public static string ExportDataName;

        private static object _dataToExport;   
        public static object DataToExport
        {
            get { return _dataToExport; }
            set { _dataToExport = value;
                if(value == null)
                {
                    Configuration.MainWindow.exportGroup.IsEnabled = false;
                }
                else
                {
                    Configuration.MainWindow.exportGroup.IsEnabled = true;
                }
            }
        }


        public Func<Object, XDocument> SetGenerateXmlFile;
        public XDocument GenerateXmlFile()
        {
           return SetGenerateXmlFile(_dataToExport);  
        }
       
        public XDocument TransformXslt(XDocument xmlTree, string xsltString)
        {
            XDocument newTree = new XDocument();
            using (XmlWriter writer = newTree.CreateWriter())
            {
                // Load the style sheet.  
                XslCompiledTransform xslt = new XslCompiledTransform();
                xslt.Load(XmlReader.Create(new StringReader(xsltString)));

                // Execute the transform and output the results to a writer.  
                xslt.Transform(xmlTree.CreateReader(), writer);
            }
            return newTree;
        }

        public string XDocumentToFullString(XDocument document)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                return System.Text.Encoding.UTF8.GetString(ms.ToArray());
            }
           
        }
    }
}
