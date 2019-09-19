// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WizardItemHeader.cs" company="Vestas Technology R&D">
//   WizardItemHeader
// </copyright>
// <summary>
//   Defines the WizardItemHeader type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UpmeSubasta2019.Wizard
{
    internal class WizardItemHeader
    {
        public int ItemNumber { get; set; }

        public string ItemHeader { get; set; }

        public bool Visited { get; set; }
    }
}
