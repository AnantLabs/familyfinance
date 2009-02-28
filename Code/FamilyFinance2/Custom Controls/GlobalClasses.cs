using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace FamilyFinance2
{
    /////////////////////////////////
    // Constants
    public class AccountCatagory
    {
        public const string INCOME = "I";
        public const string ACCOUNT = "A";
        public const string EXPENSE = "E";
    }

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

    public class SpclAccountType
    {
        public const int NULL = -1;
    }

    public class SpclEnvelope
    {
        public const int SPLIT = -2;
        public const int NULL = -1;
        public const int NOENVELOPE = 0;
    }

    public class SpclLineType
    {
        public const int NULL = -1;
    }





    /////////////////////////////////
    // Custom List items, Passing variables and sorting classes
    public class AccEnvDetails
    {
        public int accountID;
        public int envelopeID;
        public bool error;
        public string name;
        public string type;
        public string bank;
        public string catagory;
        public decimal balance;

        public AccEnvDetails()
        {
            accountID = SpclAccount.NULL;
            envelopeID = SpclEnvelope.NULL;
            error = false;
            name = "";
            type = "";
            bank = "";
            catagory = "";
            balance = 0.0m; 
        }
    }

    public class AccEnvDetailsNameComparer : IComparer<AccEnvDetails>
    {
        public int Compare(AccEnvDetails x, AccEnvDetails y)
        {
            return x.name.CompareTo(y.name);         
        }   
    }

    public class AccEnvDetailsTypeNameComparer : IComparer<AccEnvDetails>
    {
        public int Compare(AccEnvDetails x, AccEnvDetails y)
        {
            int temp = x.type.CompareTo(y.type);
            if (temp == 0)
                return x.name.CompareTo(y.name);
            else
                return temp;
        }
    }

    public class AccEnvDetailsBankNameComparer : IComparer<AccEnvDetails>
    {
        public int Compare(AccEnvDetails x, AccEnvDetails y)
        {
            int temp = x.bank.CompareTo(y.bank);
            if (temp == 0)
                return x.name.CompareTo(y.name);
            else
                return temp;
        }
    }
    
    public class DetailedErrorParam
    {
        public int ErrorID;
        public string Type;
        public string Description;
        public int RefID;

        public DetailedErrorParam(int errorID, string type, int refID, string description)
        {
            ErrorID = errorID;
            Type = type;
            RefID = refID;
            Description = description;
        }
    }

    public class ErrorParam
    {
        public int ErrorID;
        public string Type;
        public int RefID;

        public ErrorParam(int errorID, string type, int refID)
        {
            ErrorID = errorID;
            Type = type;
            RefID = refID;
        }
    }

    public class ErrorParamComparer : IComparer<ErrorParam>
    {
        public int Compare(ErrorParam x, ErrorParam y)
        {
            return x.Type.CompareTo(y.Type);
        }
    }

    public class EnvelopesIDandName
    {
        public int ID;
        public string Name;

        public EnvelopesIDandName(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }

    public class EnvelopesIDandNameComparer : IComparer<EnvelopesIDandName>
    {
        public int Compare(EnvelopesIDandName x, EnvelopesIDandName y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }

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

    public class SubLineEntry
    {
        public int EnvelopeID;
        public string Description;
        public decimal Amount;

        public SubLineEntry()
        {
            EnvelopeID = SpclEnvelope.NULL;
            Description = "";
            Amount = 0;
        }
    }

    public class OppLineEntry
    {
        public int AccountID;
        public string Description;
        public decimal Amount;

        public OppLineEntry()
        {
            AccountID = SpclAccount.NULL;
            Description = "";
            Amount = 0;
        }
    }

    public class TransactionEntry
    {
        public DateTime Date;
        public string Type;
        public int AccountID;
        public string Description;
        public string ConfermationNumber;
        public string Complete;
        public decimal Amount;
        public bool CreditDebit;
        public List<OppLineEntry> OppLineList;
        public List<SubLineEntry> SubLineList;

        public TransactionEntry()
        {
            Date = new DateTime(1, 1, 1);
            Type = "";
            AccountID = SpclAccount.NULL;
            Description = "";
            ConfermationNumber = "";
            Complete = LineState.PENDING;
            Amount = 0;
            CreditDebit = LineCD.DEBIT;

            OppLineList = new List<OppLineEntry>();
            SubLineList = new List<SubLineEntry>();
        }
    }

    public class AccountEntry
    {
        public string name;
        public string type;
        public string catagory;
        public bool creditDebit;
        public bool envelopes;

        public AccountEntry()
        {
            name = "";
            type = "";
            catagory = "";
            creditDebit = true;
            envelopes = true;
        }
    }

    public class AEBalanceEntry
    {
        public int AccountID;
        public int EnvelopeID;

        public AEBalanceEntry(int accountID, int envelopeID)
        {
            AccountID = accountID;
            EnvelopeID = envelopeID;
        }
    }

    public class AEBalanceEntryComparer : IComparer<AEBalanceEntry>
    {
        public int Compare(AEBalanceEntry x, AEBalanceEntry y)
        {
            int temp = x.AccountID.CompareTo(y.AccountID);
            if (temp == 0)
                return x.EnvelopeID.CompareTo(y.EnvelopeID);
            else
                return temp;
        }
    }


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
        public int EnvelopeID;
    }


    ///////////////////////////////////////
    // Cell Styles and Colors
    public class CellStyles
    {
        public readonly DataGridViewCellStyle Normal;
        public readonly DataGridViewCellStyle Money;
        public readonly DataGridViewCellStyle AlternatingRow;
        public readonly DataGridViewCellStyle Future;
        public readonly DataGridViewCellStyle Error;

        public CellStyles()
        {
            Normal = new DataGridViewCellStyle();

            Money = new DataGridViewCellStyle();
            Money.Alignment = DataGridViewContentAlignment.TopRight;
            Money.Format = "C2";
            Money.NullValue = null;

            AlternatingRow = new DataGridViewCellStyle();
            AlternatingRow.BackColor = System.Drawing.SystemColors.InactiveCaptionText;

            Future = new DataGridViewCellStyle();
            Future.BackColor = System.Drawing.Color.LightGray;

            Error = new DataGridViewCellStyle();
            Error.BackColor = System.Drawing.Color.Red;

        }
    }

    public class MyColors
    {
        public Color DGVSelectedBack;
        public Color DGVSelectedFore;

        public Color DGVBack;
        public Color DGVFore;

        public Color DGVAlternateBack;
        public Color DGVAlternateFore;

        public Color DGVFutureBack;
        public Color DGVFutureFore;

        //public Color DGVFutureAlternateBack;
        //public Color DGVFutureAlternateFore;

        public Color DGVErrorBack;
        public Color DGVErrorFore;

        public Color DGVNegativeBalanceFore;

        public Color DGVDisabledBack;

        
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

    public delegate void LabelChangedEventHandler(Object sender, LabelChangedEventArgs e);
    public class LabelChangedEventArgs : EventArgs
    {
        public string Text;

        public LabelChangedEventArgs(string text)
        {
            Text = text;
        }
    }

    public delegate void AccountAddedEventHandler(Object sender, AccountAddedEventArgs e);
    public class AccountAddedEventArgs : EventArgs
    {
        public int AccountID;
        public string Catagory;
        public string Type;
        public string BankName;

        public AccountAddedEventArgs(int accountID, string catagory, string type, string bankName)
        {
            AccountID = accountID;
            Catagory = catagory;
            Type = type;
            BankName = bankName;
        }
    }

    public delegate void EnvelopeAddedEventHandler(Object sender, EnvelopeAddedEventArgs e);
    public class EnvelopeAddedEventArgs : EventArgs
    {
        public int EnvelopeID;

        public EnvelopeAddedEventArgs(int envelopeID)
        {
            EnvelopeID = envelopeID;
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
