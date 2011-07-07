using System;

namespace FamilyFinance.Buisness
{
    public static class InputValidator
    {
        public static void CheckNotNull(object objectToValidate, string objectName)
        {
            if (objectToValidate == null)
                allertUserToError(objectName, "Null Object");
        }

        private static void allertUserToError(string objectName, string errorType)
        {
            string messageToUser = "Unexpected " + errorType + "\n" + objectName;
                        
            System.Windows.MessageBox.Show(messageToUser, 
                errorType, 
                System.Windows.MessageBoxButton.OK, 
                System.Windows.MessageBoxImage.Stop);
        }

    }
}
