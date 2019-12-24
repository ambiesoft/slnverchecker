using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace slnverchecker
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var productName = Assembly.GetEntryAssembly()
                .GetCustomAttributes(typeof(AssemblyProductAttribute))
                .OfType<AssemblyProductAttribute>()
                .FirstOrDefault().Product;

            this.Title = productName;
        }

        SolutionVersionInfo svi_;

        private void btnBrowseSolution_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog() {
                Filter = "Solution Files (*.sln)|*.sln"
            };
            var result = ofd.ShowDialog();
            if (result == false)
                return;
            txtSolutionPath.Text = ofd.FileName;

            List<string> lines = new List<string>();
            try
            {
                using (StreamReader sr = new StreamReader(txtSolutionPath.Text))
                {
                    string line;
                    for (; (line = sr.ReadLine()) != null;)
                    {
                        lines.Add(line);
                    }

                    svi_ = new SolutionVersionInfo(lines.ToArray());
                    tbSolutionVersion.Text = svi_.ToString();
                }
            }
            catch (FileNotFoundException ex)
            {
                tbSolutionVersion.Text = ex.Message;
                return;
            }
        }
    }
}
