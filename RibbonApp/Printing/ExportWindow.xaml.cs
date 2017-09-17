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
using RibbonApp.Database;
using Microsoft.Win32;

namespace RibbonApp.Printing
{
    public enum ExportType
    {
        HTML, XML, PDF
    }   

    /// <summary>
    /// Okno pro export.
    /// </summary>
    public partial class ExportWindow
    {   
        // Instance pomocné třídy pro export 
        PrintHelper printHelper = new PrintHelper();
        string xsltString = string.Empty;
        public ExportType ExportType { get; set; }

        public ExportWindow()
        {           
            InitializeComponent();  // Autogenerovaný kód visual studia
            ResizeThisWindow();     // Toto okno bude vždy v určitém poměru k oknu hlavnímu
            InitializePrintHelper();  // Nastevení třídy pro export
            PrintHelperGenerateAllFormats(); // Všechny exporty se vygenerují předem při otevření okna, nikoliv až při vyžádání
        }

        private void ResizeThisWindow()
        {
            this.Width = Configuration.MainWindow.ActualWidth / 1.5;
            this.Height = Configuration.MainWindow.ActualHeight / 1.5;
        }

        private void InitializePrintHelper()
        {
            switch (PrintHelper.ExportDataName)
            {
                case ExportDataName.CustomersDataGrid: Init_CustomersDataGrid();break;
                case ExportDataName.CustomerOrdersDataGrid: Init_CustomerOrdersDataGrid();break;
            }           
        }

        private void PrintHelperGenerateAllFormats()
        {
            // xml
            _xmlTree = printHelper.GenerateXmlFile();
            _displayXmlTree = printHelper.XDocumentToFullString(_xmlTree);
            // html
            _xsltTree = printHelper.TransformXslt(_xmlTree, xsltString);
            _displayXsltTree = printHelper.XDocumentToFullString(_xsltTree);
            // PDF
            _pdfFile = GeneratePdfFile(); // uloženo v %APPDATA%
        }

        // format-specific data: XML
        XDocument _xmlTree;
        string _displayXmlTree;
        // format-specific data: HTML
        XDocument _xsltTree;
        string _displayXsltTree;
        // format-specific data: PDF
        public static readonly string _fileName = "data.pdf";
        string _pdfFileUri = System.IO.Path.Combine(Configuration.AppDataPath, _fileName);
        Byte[] _pdfFile;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            switch (ExportType)
            {
                case ExportType.XML: toggleBtnXml.IsChecked = true; Xml(); break;
                case ExportType.HTML: toggleBtnHtml.IsChecked = true; Html(); break;
                case ExportType.PDF: toggleBtnPdf.IsChecked = true; Pdf(); break;
            }            

            // Události připojit až když budou toggle buttony nasteveny, jinak se spustí událost
            // checked již při jejich nastavování v code-behid
            toggleBtnHtml.Checked += toggleBtnHtml_Checked;
            toggleBtnXml.Checked += toggleBtnXml_Checked;
            toggleBtnPdf.Checked += toggleBtnPdf_Checked;
            toggleBtnPdf.Unchecked += toggleBtnPdf_UnChecked;


        }

        private Byte[] GeneratePdfFile()
        {
            string path = _pdfFileUri;
            using (MemoryStream ms = new MemoryStream()) 
            {
                Byte[] res = null;
                var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(_displayXsltTree, PdfSharp.PageSize.A4);
                pdf.Save(ms);
                res = ms.ToArray();
                File.WriteAllBytes(path, res);
                return res;      
            }    
           
        }       

        private void Pdf()
        {           
            webBrowser1.Navigate(_pdfFileUri);
        }

        private void Html()
        {           
            webBrowser1.NavigateToString(_displayXsltTree);
        }
        
        private void Xml()
        {                 
            webBrowser1.NavigateToString(_displayXmlTree);
        }       

        

        private void SavePdf()
        {
            Byte[] pdf = _pdfFile;

            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.FileName = PrintHelper.ExportDataName.ToString();
            sfd.Filter = "pdf |*.pdf";
            if (sfd.ShowDialog() == true)
                File.WriteAllBytes(sfd.FileName, pdf);
        }

        private void SaveXml()
        {
            string xml = _displayXmlTree;

            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.FileName = PrintHelper.ExportDataName.ToString();
            sfd.Filter = "xml |*.xml";
            if (sfd.ShowDialog() == true)
                File.WriteAllText(sfd.FileName, xml);
        }

        private void SaveHtml()
        {

            string html = _displayXsltTree;

            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.FileName = PrintHelper.ExportDataName.ToString();
            sfd.Filter = "html |*.html";
            if (sfd.ShowDialog() == true)
                File.WriteAllText(sfd.FileName, html);

        }

        /* ------------------------------Inicializační metody pro specifická data-------------------------- */
        private void Init_CustomersDataGrid()
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

        private void  Init_CustomerOrdersDataGrid()
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

        /* -----------------------------------------Events------------------------------------ */
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

        private void toggleBtnPdf_Checked(object sender, RoutedEventArgs e)
        {
            groupBoxPrintSave.Visibility = Visibility.Hidden;
            ExportType = ExportType.PDF;
            Pdf();
        }

        private void toggleBtnPdf_UnChecked(object sender, RoutedEventArgs e)
        {
            groupBoxPrintSave.Visibility = Visibility.Visible;            
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {            
             // Získání objektu z prohlížeče k tisku
             mshtml.IHTMLDocument2 doc = webBrowser1.Document as mshtml.IHTMLDocument2;

            if(ExportType == ExportType.HTML)
            {
                webBrowser1.LoadCompleted += InvokePrint; // Tisknout můžeme, až když je okno prohlížeče načtené, jinak se vytiskne prázdná stránka
            }
            
            //  Soubor .xml se vždy vytiskne jako prázdná stránka, proto je potřeba převést i XML na HTML pro tisk
            if(ExportType == ExportType.XML)
            {
                // je nutné nahradit speciální znaky ze xml: (< &lt),(> &gt;);
                string alteredXml = _displayXmlTree;
                alteredXml= alteredXml.Replace("<","&lt;");
                alteredXml = alteredXml.Replace(">", "&gt;");
                string html = $@"
                                <html>  
                                  <head>  
                                    <meta charset=""utf-8"" />
                                    <title>Xml</title>  
                                  </head>
                                  <body>
                                     <pre>
                                     {alteredXml}
                                     </pre>
                                  </body >
                                </html>";                             

                // nejdříve načteme xml ve fromě html do prohlížeče a po skončení tisku zobrazíme čisté xml zpět
                webBrowser1.NavigateToString(html);
                webBrowser1.LoadCompleted += InvokePrintReturnToXmlAfterFinished;                
               
            }

        }

        private void InvokePrint(object o, object e)
        {
            Print(webBrowser1.Document as mshtml.IHTMLDocument2);
            webBrowser1.LoadCompleted -= InvokePrint; // po tisku událost odebereme
        }

        private void InvokePrintReturnToXmlAfterFinished(object o, object e)
        {
            Print(webBrowser1.Document as mshtml.IHTMLDocument2);
            webBrowser1.LoadCompleted -= InvokePrintReturnToXmlAfterFinished;
            webBrowser1.NavigateToString(_displayXmlTree);
        }

        private void Print(mshtml.IHTMLDocument2 doc)
        {
            if (doc != null)
            {
                // Aby se stránka vytiskla bez hlavičky a patičky, tak je potřeba to upravit v nastevení IE v registrech
                // Po skončení tisku jsou registry obnoveny do původní podoby
                try
                {
                    const string keyName = @"Software\Microsoft\Internet Explorer\PageSetup";
                    using (var key = Registry.CurrentUser.OpenSubKey(keyName, true))
                    {
                        if (key != null)
                        {
                            var oldFooter = key.GetValue("footer");
                            var oldHeader = key.GetValue("header");
                            key.SetValue("footer", "");
                            key.SetValue("header", "");
                            doc.execCommand("Print", true, null);
                            key.SetValue("footer", oldFooter);
                            key.SetValue("header", oldHeader);
                            return;
                        }
                    }
                }
                catch
                {

                }

                // při chybě úpravy registrů bude mít export nežádoucí hlavičku - alespoň se ale tisk podaří 
                doc.execCommand("Print", true, null);                 
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            switch (ExportType)
            {
                case ExportType.XML: SaveXml(); break;
                case ExportType.HTML: SaveHtml(); break;
                case ExportType.PDF: SavePdf(); break;
            }
        }

        private void RibbonWindow_Closed(object sender, EventArgs e)
        {
            FileIsUsedByAnotherProcessFix(); // při dalším exportu nelze soubor bez tohoto fixu přepsat ani smazat, protože ho používá jiný proces (browser)
        }

       private void FileIsUsedByAnotherProcessFix()
        {           
            webBrowser1.Dispose(); // jinak chyba, že je soubor používán jiným procesem    
            
            // není nutné - při dalším exportu se soubor přepíše, ale je lepší exportovaný soubor ve složce zbytečně neponechávat
            // pokud bude aplikace ukončena natvrdo, soubor se nesmaže, ale to v podstatě nevadí
            DeletePdfFileFromAppDafta(); 
        }

        public static void DeletePdfFileFromAppDafta()
        {
            try // pokud nastane chyba při mazaní, chyba bude ignorována a soubor se nesmaže
            {            
            string path = System.IO.Path.Combine(Configuration.AppDataPath, _fileName);
            if (File.Exists(path))
            {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                File.Delete(path);
            }
            }
            catch {
            }
        }
    }

   
}
