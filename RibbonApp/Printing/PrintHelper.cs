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
        public PrintHelper(List<EntityNotify> entites)
        {
            this._entites = entites;
        }

        private List<EntityNotify> _entites;

        public XDocument GenerateXmlFile()
        {
            XDeclaration declaration = new XDeclaration("1.0", "utf-8", null);

            XDocument mainDocument = new XDocument(declaration, new XElement("Entities",
                _entites.Select(e => new XElement("Entity", new XAttribute("id", e.Id),
                new XElement("Name", e.Name),
                new XElement("Date", e.Date),
                new XElement("Check", e.Check)
                ))));
            return mainDocument;

         

        }

        public XDocument DemoXslt(XDocument xmlTree)
        {
            string xsltString = @"<?xml version='1.0'?>  
<xsl:stylesheet xmlns:xsl='http://www.w3.org/1999/XSL/Transform' version='1.0'>  
    <xsl:template match='/'>  
        <html>  
       <head> <title> Xslt demo </title>  </head>
        <body>
        
  <ul>
 <xsl:for-each select='Entities/Entity'>
<li>  <xsl:value-of select='Name'/>  </li>
</xsl:for-each>
  </ul>
           
        </body>
        </html>  
    </xsl:template>  
</xsl:stylesheet>";

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
