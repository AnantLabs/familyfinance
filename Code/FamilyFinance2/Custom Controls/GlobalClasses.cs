using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using TreeList;

namespace FamilyFinance2
{
    /////////////////////////////////
    // Constants
    public class LineCD
    {
        public const bool CREDIT = false;
        public const bool DEBIT = true;
    }

    public class LineState
    {
        public const string CLEARED = "C";
        public const string RECONSILED = "R";
        public const string PENDING = " ";
    }

    public class SpclAccount
    {
        public const int MULTIPLE = -2;
        public const int NULL = -1;
    }

    public class SpclAccountCat
    {
        public const byte NULL = 0;
        public const byte INCOME = 1;
        public const byte ACCOUNT = 2;
        public const byte EXPENSE = 3;
    }

    public class SpclAccountType
    {
        public const short NULL = -1;
    }

    public class SpclEnvelope
    {
        public const short SPLIT = -2;
        public const short NULL = -1;
        public const short NOENVELOPE = 0;
    }

    public class SpclLineType
    {
        public const short NULL = -1;
    }



    /////////////////////////////////
    // Custom List items, Passing variables and sorting classes


    //public class DetailedErrorParam
    //{
    //    public int ErrorID;
    //    public string Type;
    //    public string Description;
    //    public int RefID;

    //    public DetailedErrorParam(int errorID, string type, int refID, string description)
    //    {
    //        ErrorID = errorID;
    //        Type = type;
    //        RefID = refID;
    //        Description = description;
    //    }
    //}

    //public class ErrorParam
    //{
    //    public int ErrorID;
    //    public string Type;
    //    public int RefID;

    //    public ErrorParam(int errorID, string type, int refID)
    //    {
    //        ErrorID = errorID;
    //        Type = type;
    //        RefID = refID;
    //    }
    //}

    //public class ErrorParamComparer : IComparer<ErrorParam>
    //{
    //    public int Compare(ErrorParam x, ErrorParam y)
    //    {
    //        return x.Type.CompareTo(y.Type);
    //    }
    //}

    //public class EnvelopesIDandName
    //{
    //    public int ID;
    //    public string Name;

    //    public EnvelopesIDandName(int id, string name)
    //    {
    //        ID = id;
    //        Name = name;
    //    }
    //}

    //public class EnvelopesIDandNameComparer : IComparer<EnvelopesIDandName>
    //{
    //    public int Compare(EnvelopesIDandName x, EnvelopesIDandName y)
    //    {
    //        return x.Name.CompareTo(y.Name);
    //    }
    //}

    //public class MyConnectionString
    //{
    //    public enum Modes {ReadWrite, ReadOnly, Exclusive, SharedRead };

    //    static public string Build(string filePath, string mode)
    //    {
    //        string connectionString;

    //        connectionString = "Persist Security Info = False; ";
    //        connectionString += "Data Source = '" + filePath + "'; ";
    //        //connectionString += "File Mode = '" + mode + "'; ";
    //        connectionString += "File Mode = 'Read Write'; ";
    //        connectionString += "Max Database Size = 1024; "; // 1 Gig
    //        connectionString += "Max Buffer Size = 640; ";
    //        connectionString += "Password = 's7upahu2umEcrabr!c#?u66v*FRad4gum2swe#22'; ";

    //        return connectionString;
    //    }
    //}
    // Data Source = "|DataDirectory|\FamilyFinanceDB.sdf"; File Mode = 'Read Write'; Max Database Size = 1024; Max Buffer Size = 640; Password = 's7upahu2umEcrabr!c#?u66v*FRad4gum2swe#22'; 

    //public class SubLineEntry
    //{
    //    public int EnvelopeID;
    //    public string Description;
    //    public decimal Amount;

    //    public SubLineEntry()
    //    {
    //        EnvelopeID = SpclEnvelope.NULL;
    //        Description = "";
    //        Amount = 0;
    //    }
    //}

    //public class OppLineEntry
    //{
    //    public int AccountID;
    //    public string Description;
    //    public decimal Amount;

    //    public OppLineEntry()
    //    {
    //        AccountID = SpclAccount.NULL;
    //        Description = "";
    //        Amount = 0;
    //    }
    //}

    //public class TransactionEntry
    //{
    //    public DateTime Date;
    //    public string Type;
    //    public int AccountID;
    //    public string Description;
    //    public string ConfermationNumber;
    //    public string Complete;
    //    public decimal Amount;
    //    public bool CreditDebit;
    //    public List<OppLineEntry> OppLineList;
    //    public List<SubLineEntry> SubLineList;

    //    public TransactionEntry()
    //    {
    //        Date = new DateTime(1, 1, 1);
    //        Type = "";
    //        AccountID = SpclAccount.NULL;
    //        Description = "";
    //        ConfermationNumber = "";
    //        Complete = LineState.PENDING;
    //        Amount = 0;
    //        CreditDebit = LineCD.DEBIT;

    //        OppLineList = new List<OppLineEntry>();
    //        SubLineList = new List<SubLineEntry>();
    //    }
    //}

    //public class AccountEntry
    //{
    //    public string name;
    //    public string type;
    //    public string catagory;
    //    public bool creditDebit;
    //    public bool envelopes;

    //    public AccountEntry()
    //    {
    //        name = "";
    //        type = "";
    //        catagory = "";
    //        creditDebit = true;
    //        envelopes = true;
    //    }
    //}


    ///////////////////////////////////////
    // Special DataTypes
    public class MyComboBoxItem
    {
        public int ID;
        public string Name;

        public MyComboBoxItem(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name.ToString();
        }
    }

    public class MyTreeNode : TreeNode
    {
        public short ID;
    }

    public class MyTreeListNode : TreeList.Node
    {
        public short EnvelopeID;
        public short AccountID;
    }



    ///////////////////////////////////////
    // Delagates and Event Arguments
    public delegate void BalanceChangedEventHandler(Object sender, BalanceChangedEventArgs e);
    public class BalanceChangedEventArgs : EventArgs
    {
        public int AccountID;
        public int EnvelopeID;
        public decimal NewAmount;

        public BalanceChangedEventArgs(int accountID, int envelopeID, decimal newAmount)
        {
            AccountID = accountID;
            EnvelopeID = envelopeID;
            NewAmount = newAmount;
        }
    }

    public delegate void SelectedAccountEnvelopeChangedEventHandler(Object sender, SelectedAccountEnvelopeChangedEventArgs e);
    public class SelectedAccountEnvelopeChangedEventArgs : EventArgs
    {
        public int AccountID;
        public int EnvelopeID;

        public SelectedAccountEnvelopeChangedEventArgs(int accountID, int envelopeID)
        {
            AccountID = accountID;
            EnvelopeID = envelopeID;
        }
    }


}// END nameSpace FamilyFinance
