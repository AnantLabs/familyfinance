using System;
using System.Data;


using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public abstract class DataRowModel : BindableObject
    {
        /// <summary>
        /// Truncates a string if it is longer than the given length.
        /// </summary>
        /// <param name="value">The string to be trunked if it is too long.</param>
        /// <param name="length">The maximum length the string should be.</param>
        /// <returns></returns>
        protected string truncateIfNeeded(string value, int length)
        {
            string validString;

            if (value.Length > length)
            {
                // TODO: Show a warning dialog saying the max this length should be.
                System.Media.SystemSounds.Hand.Play();
                validString = value.Substring(0, length);
            }
            else
                validString = value;

            return validString;
        }
        

    }
}
