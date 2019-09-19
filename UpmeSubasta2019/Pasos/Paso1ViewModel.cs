using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpmeSubasta2019
{
    using UpmeSubasta2019.Wizard;

    class Paso1ViewModel : IWizardItem
    {
        public string GetHeader()
        {
            return "Inicio";
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
