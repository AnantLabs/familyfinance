using System;

namespace FamilyFinance.Buisness
{
    public static class InputValidator
    {
        public static void CheckNotNull(object objectToValidate)
        {
            if (objectToValidate == null)
                allertUserToError(objectToValidate, "Null Object");
        }

        private static void allertUserToError(object problemObject, string errorType)
        {
            string nameOfObjectType = problemObject.GetType().FullName;
            string messageToUser = "Unexpected " + errorType;
                        
            System.Windows.MessageBox.Show(messageToUser, 
                errorType, 
                System.Windows.MessageBoxButton.OK, 
                System.Windows.MessageBoxImage.Stop);
        }

    }
}
