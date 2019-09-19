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

namespace UpmeSubasta2019
{
    /// <summary>
    /// Interaction logic for Paso1.xaml
    /// </summary>
    /// //Hola 3
    /// //Hola 2
    public partial class Paso8 : UserControl
    {
        public Paso8()
        {
            DataContext = new Paso8ViewModel();
            InitializeComponent();
        }
    }
}
