using System.ComponentModel;


namespace FamilyFinance.Buisness
{
    public abstract class BindableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Helper function to raise the property changed event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (propertyName != "" && TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                System.Windows.MessageBox.Show("Invalid property name: " + propertyName, "Invalid Property", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
            }

            else if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
