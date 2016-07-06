using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
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

namespace View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var vm = new ViewModel.MainViewModel();
            var catalog = new AssemblyCatalog(typeof(MainWindow).Assembly);
            var cc = new CompositionContainer(catalog);
            cc.ComposeParts(vm);

            DataContext = vm;

            InitializeComponent();

            MBPrompt.element_image = image;
        }

        [Export(typeof(ViewModel.IPromptService))]
        public class MBPrompt : ViewModel.IPromptService
        {
            public static Image element_image;

            public bool Confirm(string text)
            {
                return MessageBox.Show(text, "Question", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
            }

            public string GetOpenFileName()
            {
                var open_dialog = new OpenFileDialog();
                if (open_dialog.ShowDialog() == true) return open_dialog.FileName;
                else return null;
            }

            public string GetSaveFileName()
            {
                var save_dialog = new SaveFileDialog();
                if (save_dialog.ShowDialog() == true) return save_dialog.FileName;
                else return null;
            }

            public Point GetMousePosition()
            {
                return Mouse.GetPosition(element_image);
            }
        }
    }
}
