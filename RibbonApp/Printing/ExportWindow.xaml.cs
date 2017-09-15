using RibbonApp.Model;
using RibbonApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.IO;

namespace RibbonApp.Printing
{
    /// <summary>
    /// Interaction logic for ExportWindow.xaml
    /// </summary>
    public partial class ExportWindow
    {
        PrintHelper printHelper = new PrintHelper();
        string xsltString = string.Empty;

        public ExportWindow()
        {           
            InitializeComponent();
            InitializePrintHelper();
        }

        private void InitializePrintHelper()
        {          
            if (PrintHelper.ExportDataName == "CustomersDataGrid")
            {
                printHelper.SetGenerateXmlFile += (object collection) => {

                    ObservableCollection<Customer> customers = (ObservableCollection<Customer>)collection;
                    XDeclaration declaration = new XDeclaration("1.0", "utf-8", null);
                    XDocument mainDocument = new XDocument(declaration, new XElement("Customers",
                        customers.Select(c => new XElement("Customer", new XAttribute("id", c.Id),
                        new XElement("Name", c.Name),
                        new XElement("Surname", c.Surname)
                        ))));
                    return mainDocument;
                };

                xsltString = @"<?xml version='1.0'?>  
               <xsl:stylesheet xmlns:xsl='http://www.w3.org/1999/XSL/Transform' version='1.0'>  
               <xsl:template match='/'>  
               <html>  
               <head> <title> Zákazníci </title>  </head>
               <body>
               <xsl:for-each select='Customers/Customer'>
               <xsl:value-of select='@id'/>  <br />
               <xsl:value-of select='Name'/>  <br />
               <xsl:value-of select='Surname'/>  <br />
               <hr />
               </xsl:for-each> 
               </body>
               </html>  
               </xsl:template>  
               </xsl:stylesheet>";
            }
            else if (PrintHelper.ExportDataName == "CustomerOrdersDataGrid")
            {
                printHelper.SetGenerateXmlFile += (object collection) => {

                    ObservableCollection<Order> customers = (ObservableCollection<Order>)collection;
                    XDeclaration declaration = new XDeclaration("1.0", "utf-8", null);
                    XDocument mainDocument = new XDocument(declaration, new XElement("Orders",
                        customers.Select(o => new XElement("Order", new XAttribute("id", o.Id),
                        new XElement("Comment", o.Comment)                       
                        ))));
                    return mainDocument;
                };

                xsltString = @"<?xml version='1.0'?>  
               <xsl:stylesheet xmlns:xsl='http://www.w3.org/1999/XSL/Transform' version='1.0'>  
               <xsl:template match='/'>  
               <html>  
               <head> <title> Objednávky </title>  </head>
               <body>
               <xsl:for-each select='Orders/Order'>
               <xsl:value-of select='@id'/>  <br />
               <xsl:value-of select='Comment'/>  <br />               
               <hr />
               </xsl:for-each> 
               </body>
               </html>  
               </xsl:template>  
               </xsl:stylesheet>";
            }
        }

        public ExportType ExportType { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            switch (ExportType)
            {
                case ExportType.XML: toggleBtnXml.IsChecked = true; break;
                case ExportType.HTML: toggleBtnHtml.IsChecked = true; break;
            }

            switch (ExportType)
            {
                case ExportType.XML: Xml(); break;
                case ExportType.HTML: Html(); break;
                case ExportType.PDF: Pdf(); break;
                default: Xml(); break;
            }

            // Události připojit až když budou toggle buttony nasteveny, jinak se spustí událost
            // checked již při jejich nastavování v code-behid
            toggleBtnHtml.Checked += toggleBtnHtml_Checked;
            toggleBtnXml.Checked += toggleBtnXml_Checked;
        }

        private void Pdf()
        {
           // TODO: PDF export
        }

        private void Html()
        {
            XDocument ms = printHelper.GenerateXmlFile();
            webBrowser1.NavigateToString(printHelper.XDocumentToFullString(printHelper.TransformXslt(ms,xsltString)));
        }
        
        private void Xml()
        {     
            string xml = printHelper.XDocumentToFullString(printHelper.GenerateXmlFile());
            webBrowser1.NavigateToString(xml);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void toggleBtnHtml_Checked(object sender, RoutedEventArgs e)
        {
        
            ExportType = ExportType.HTML;
            Html();
        }

        private void toggleBtnXml_Checked(object sender, RoutedEventArgs e)
        {
          
            ExportType = ExportType.XML;
            Xml();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            mshtml.IHTMLDocument2 doc = webBrowser1.Document as mshtml.IHTMLDocument2;
            doc.execCommand("Print", true, null);

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            switch(ExportType)
            {
                case ExportType.XML: SaveXml();break;
                case ExportType.HTML: SaveHtml(); break;

            }
        }
        private void SaveXml()
        {
            string xml = printHelper.XDocumentToFullString(printHelper.GenerateXmlFile());

            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.FileName = PrintHelper.ExportDataName;
            sfd.Filter = "xml |*.xml";
            if (sfd.ShowDialog() == true)
                File.WriteAllText(sfd.FileName, xml);
        }

        private void SaveHtml()
        {
            XDocument ms = printHelper.GenerateXmlFile();
            string html = printHelper.XDocumentToFullString(printHelper.TransformXslt(ms, xsltString));

            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.FileName = PrintHelper.ExportDataName;
            sfd.Filter = "html |*.html";
            if (sfd.ShowDialog() == true)
                File.WriteAllText(sfd.FileName, html);

        }
    }

    public enum ExportType
    {
       HTML,XML, PDF
    }
}
