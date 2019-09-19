using System.Collections.Generic;

namespace UpmeSubasta2019
{
    using System.Windows;
    using System.Windows.Input;

    class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            WizardItems = new List<object> {
                new Paso1(),
                new Paso2(),
                new Paso3(),
                new Paso4(),
                new Paso5(),
                new Paso6(),
                new Paso7(),
                new Paso8(),
                new Paso9(),
                new Paso10()

                
            };
        }

        public IList<object> WizardItems { get; set; }

        public ICommand CancelCommand
        {
            get
            {
                //return new RelayCommand((o) => MessageBox.Show("La aplicación se cerrará."));
                return new RelayCommand((o) => Application.Current.Shutdown());
            }
        }

        public ICommand OkCommand
        {
            get
            {
                //return new RelayCommand((o) => Application.Current.Shutdown());
                return new RelayCommand((o) => MessageBox.Show("No hay más pasos"));
            }
        }
    }
}
