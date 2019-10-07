﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpmeSubasta2019
{
    using UpmeSubasta2019.WizardView;

    class Paso7ViewModel : IWizardItem
    {
        public string GetHeader()
        {
            return "Resultados" + Environment.NewLine + "Mecanismo";
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
