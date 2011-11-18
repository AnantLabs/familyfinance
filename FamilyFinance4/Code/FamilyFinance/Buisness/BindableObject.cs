using System.ComponentModel;


namespace FamilyFinance.Buisness
{
    public abstract class BindableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void raisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void reportAllPropertiesChanged()
        {
            raisePropertyChanged("");
        }

        protected void reportPropertyChangedWithName(string propertyName)
        {
            if (isValidPropertyName(propertyName))
                raisePropertyChanged(propertyName);
            else
                showInvalidPropertyNamePopUp(propertyName);
        }

        private bool isValidPropertyName(string propertyName)
        {
            if(TypeDescriptor.GetProperties(this)[propertyName] == null)
                return false;
            else
                return true;
        }

        private void showInvalidPropertyNamePopUp(string propertyName)
        {
            System.Windows.MessageBox.Show("Invalid property name: " + propertyName, 
                "Invalid Property", 
                System.Windows.MessageBoxButton.OK, 
                System.Windows.MessageBoxImage.Stop);
        }

    }
}
