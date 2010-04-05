using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyFinance2.Forms.Main.RegistrySplit.TreeView
{
    public class CatagoryNode : TreeList.Node
    {
        public byte Catagory;

        public CatagoryNode(string text, byte catagory)
            : base(text)
        {
            this.Catagory = catagory;
        }
    }

    public class TypeNode : TreeList.Node
    {
        public byte Catagory;
        public int TypeID;

        public TypeNode(string text, byte catagory, int typeID)
            : base(text)
        {
            this.Catagory = catagory;
            this.TypeID = typeID;
        }
    }

    public class AccountNode : TreeList.Node
    {
        public int AccountID;
        public bool Envelopes;

        public AccountNode(string text, int accountID, bool envelopes)
            : base(text)
        {
            this.AccountID = accountID;
            this.Envelopes = envelopes;
        }
    }

    public class EnvelopeNode : TreeList.Node
    {
        public int EnvelopeID;

        public EnvelopeNode(string text, byte catagory, int envelopeID)
            : base(text)
        {
            this.EnvelopeID = envelopeID;
        }
    }

    public class AENode : TreeList.Node
    {
        public int EnvelopeID;
        public int AccountID;

        public AENode(string text, int envelopeID, int accountID)
            : base(text)
        {
            this.EnvelopeID = envelopeID;
            this.AccountID = accountID;
        }
    }
}
