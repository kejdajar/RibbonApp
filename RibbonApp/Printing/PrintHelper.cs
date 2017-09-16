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
        
        // Každá tabulka má jiné sloupce a proto je potřeba napsat custom kód pro export každé tabulky zvlášť.
        // ExportDataName se používá poté v konstrukci SWITCH, kde se tento custom kód píše. ExportDataName
        // musí být unikátní pro každou další tabulku.
        public static string ExportDataName;

        // Pokud někde v kódu naplním proměnnou DataToExport, tak se odemkne panel pro export.
        // Pokud naplím hodnotou null, tak se panel zamkne.
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

        // Funkce, která bere jako parametr data k exportu a vrací XDocument a je
        // namíru napsaná danému datovému zdroji
        public Func<Object, XDocument> SetGenerateXmlFile;

        // Zavolání funkce SetGenerateXmlFile(object kolekceDat)
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
