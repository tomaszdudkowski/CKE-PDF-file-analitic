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
using System.Windows.Navigation;
using System.Windows.Shapes;
using iText.Kernel.Utils;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace AnaliticsCKE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public string filename;

        private void Set_File_Button_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".PDF"; // Default file extension
            dlg.Filter = "PDF Portable Document Format|*.PDF"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string safefilename = dlg.SafeFileName;
                filename = dlg.FileName;
                PathOfPDF.Text = safefilename;
                Analyze.IsEnabled = true;
            }
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Wybrano: " + filename,
                                          "Informacja",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                MessageBoxResult resultWithSomeData = MessageBox.Show("Data from PDF/A",
                                                            "Some data",
                                                            MessageBoxButton.OKCancel,
                                                            MessageBoxImage.Warning);
                if(resultWithSomeData == MessageBoxResult.OK)
                {
                    string pageContent = "";
                    PdfReader pdfReader = new PdfReader(filename);
                    PdfDocument pdfDocument = new PdfDocument(pdfReader);
                    for (int page = 1; page <= pdfDocument.GetNumberOfPages(); page++)
                    {
                        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                        pageContent = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(page), strategy);
                    }

                    StringBuilder stringBuilder = new StringBuilder();
                    for(int i=0; i<pageContent.Length; i++)
                    {
                        if (pageContent[i] == ' ')
                        {
                            stringBuilder.Append('^');
                            continue;
                        }
                        stringBuilder.Append(pageContent[i]);
                        
                    }

                    MessageBoxResult resultWithPageContent = MessageBox.Show(stringBuilder.ToString(),
                                                            "Page Content From PDF/A",
                                                            MessageBoxButton.OK,
                                                            MessageBoxImage.Information);
                    pdfDocument.Close();
                    pdfReader.Close();
                    
                }
            }
        }
    }
}
