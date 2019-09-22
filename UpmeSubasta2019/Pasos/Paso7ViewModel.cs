using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpmeSubasta2019
{
    using UpmeSubasta2019.Wizard;

    class Paso7ViewModel : IWizardItem
    {
        public string GetHeader()
        {
            return "Ejecutar" + Environment.NewLine + "Mecanismo";
        }

        public bool CanDisplay()
        {
            return true;
        }

        public void OnWizardItemNavigatedTo()
        {
        }

        public void OnWizardItemNavigatedFrom()
        {
        }
    }
}
