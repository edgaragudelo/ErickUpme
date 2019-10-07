using System.Windows;
//using log4net;

namespace UpmeSubasta2019
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            WindowState = WindowState.Maximized;
            DataContext = new MainWindowViewModel();
            InitializeComponent();
        }
    }
}
