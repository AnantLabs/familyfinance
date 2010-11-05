using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyFinance.Registry
{

    public delegate void AccountEnvelopeChangedEventHandler(Object sender, AccountEnvelopeChangedEventArgs e);

    public class AccountEnvelopeChangedEventArgs : EventArgs
    {
        public int AccountID;
        public int EnvelopeID;

        public AccountEnvelopeChangedEventArgs(int accountID, int envelopeID)
        {
            this.AccountID = accountID;
            this.EnvelopeID = envelopeID;
        }
    }
}
