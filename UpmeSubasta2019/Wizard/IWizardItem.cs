namespace UpmeSubasta2019.WizardView
{
    public interface IWizardItem
    {
        /// <summary>
        /// This method should return the header for wizard item to display
        /// </summary>
        /// <returns> A string value.</returns>
        string GetHeader();

        /// <summary>
        /// This method will be invoked to check whether this item can be displayed or not.
        /// </summary>
        /// <returns>A boolean value indicating true or false status</returns>
        bool CanDisplay();

        /// <summary>
        /// This method will get invoked when the wizard item becomes the active item.
        /// </summary>
        /// <param name="autoAcknoledgeNext">
        /// The auto Acknoledge Next. Setting this to true will take you to the next page automatically.
        /// </param>
        void OnWizardItemNavigatedTo(ref bool autoAcknoledgeNext);

        /// <summary>
        /// This method will get invoked on the current wizard item when the control is moved to next wizard item.
        /// </summary>
        /// <param name="canNavigateAway">
        /// The can Navigate Away. This value when set to false, will not allow navigation from this view.
        /// </param>
        void OnWizardItemNavigatedFrom(ref bool canNavigateAway);
    }
}
