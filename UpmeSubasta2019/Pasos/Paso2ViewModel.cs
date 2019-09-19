using System;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;

namespace UpmeSubasta2019
{
    using LiveCharts;
    using LiveCharts.Wpf;
    using System.Windows.Controls;
    using UpmeSubasta2019.Wizard;

    class Paso2ViewModel : IWizardItem
    {
        public string GetHeader()
        {
            return "Carga de Ofertas" + Environment.NewLine +" (Sobre No 1)";
        }

        public bool CanDisplay()
        {
            return true; ;
        }

        public void OnWizardItemNavigatedTo()
        {
        }

        public void OnWizardItemNavigatedFrom()
        {
        }

     
    }
}
