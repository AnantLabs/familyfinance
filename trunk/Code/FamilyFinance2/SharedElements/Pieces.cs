using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;


namespace FamilyFinance2.SharedElements
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

    public class SpclLineType
    {
        public const int NULL = -1;
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
        public const byte ENVELOPE = 4;
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

    public class SpclEnvelopeGroup
    {
        public const int NULL = -1;
    }



    /////////////////////////////////
    // Custom List items, Passing variables and sorting classes
    public enum DBTables { Account, AccountType, AEBalance, Envelope, LineItem, LineType, SubLineItem }
    public class Changes
    {
        public List<DBTables> Tables;

        public Changes()
        {
            Tables = new List<DBTables>();
        }

        public void AddTable(DBTables table)
        {
            bool found = false;

            foreach (DBTables tab in this.Tables)
            {
                if (tab == table)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
                this.Tables.Add(table);
        }

        public void Copy(Changes otherChanges)
        {
            foreach (DBTables tab in otherChanges.Tables)
	        {
                this.AddTable(tab);
	        }

        }
    }



    // Data Source = "|DataDirectory|\FamilyFinanceDB.sdf"; File Mode = 'Read Write'; Max Database Size = 1024; Max Buffer Size = 640; Password = 's7upahu2umEcrabr!c#?u66v*FRad4gum2swe#22'; 

    ///////////////////////////////////////
    //  Special Celltypes
    public class MyCellStyleNormal : DataGridViewCellStyle
    {
        public MyCellStyleNormal() : base()
        {
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        }
    }
    
    public class MyCellStyleMoney : DataGridViewCellStyle
    {
        public MyCellStyleMoney() : base()
        {
            this.Alignment = DataGridViewContentAlignment.TopRight;
            this.Format = "C2";
        }
    }
    
    public class MyCellStyleAlternatingRow : DataGridViewCellStyle
    {
        public MyCellStyleAlternatingRow() : base()
        {
            //ButtonFace / Control is a nise soft greay color.
            //GradientInactiveCaption is a baby blue color
            //InactiveBorder nice very soft blue color.
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
        }
    }
    
    public class MyCellStyleError : DataGridViewCellStyle
    {
        public MyCellStyleError() : base()
        {
            this.BackColor = System.Drawing.Color.Red;
        }
    }
    
    public class MyCellStyleFuture : DataGridViewCellStyle
    {
        public MyCellStyleFuture() : base()
        {
            this.BackColor = System.Drawing.Color.LightGray;
        }
    }



    ///////////////////////////////////////
    //  Special DataTypes
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
        public int ID;

        public MyTreeNode(string text, int id)
            : base(text)
        {
            this.ID = id;
        }
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
