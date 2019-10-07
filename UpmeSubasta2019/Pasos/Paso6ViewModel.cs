using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpmeSubasta2019
{
    using UpmeSubasta2019.WizardView;

    class Paso6ViewModel : IWizardItem
    {
        public string GetHeader()
        {
            return "Mecanismo" + Environment.NewLine + "Complementario";
        }

        public bool CanDisplay()
        {
            return true;
        }

        public void OnWizardItemNavigatedTo(ref bool autoAcknoledgeNext)
        {
            autoAcknoledgeNext = false;
        }

        public void OnWizardItemNavigatedFrom(ref bool canNavigateAway)
        {
            canNavigateAway = true;
        }
    }
}
