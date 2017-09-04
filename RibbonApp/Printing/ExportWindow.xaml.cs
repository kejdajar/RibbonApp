using RibbonApp.ViewModel;
using System;
using System.Collections.Generic;
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

namespace RibbonApp.Printing
{
    /// <summary>
    /// Interaction logic for ExportWindow.xaml
    /// </summary>
    public partial class ExportWindow
    {
        public ExportWindow()
        {           
            InitializeComponent();
        }

        public List<EntityNotify> Data { get; set; } = null;
        public ExportType ExportType { get; set; } = ExportType.XML; // Defaultně xml

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            switch(ExportType)
            {
                case ExportType.XML: Xml();break;
                case ExportType.HTML: Html(); break;
                case ExportType.PDF: Pdf(); break;
                default: Xml(); break;
            }
            
        }

        private void Pdf()
        {
           
        }

        private void Html()
        {            
            PrintHelper printHelper = new PrintHelper(Data);
            XDocument ms = printHelper.GenerateXmlFile();
           webBrowser1.NavigateToString(printHelper.XDocumentToFullString(printHelper.DemoXslt(ms)));
        }

        private void Xml()
        {
            PrintHelper printHelper = new PrintHelper(Data);
            string xml = printHelper.XDocumentToFullString(printHelper.GenerateXmlFile());
            webBrowser1.NavigateToString(xml);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public enum ExportType
    {
       XML, HTML, PDF
    }
}
