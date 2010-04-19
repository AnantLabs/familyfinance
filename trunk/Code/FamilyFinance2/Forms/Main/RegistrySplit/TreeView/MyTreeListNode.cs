using System;
using System.Collections.Generic;
using System.Text;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.Main.RegistrySplit.TreeView
{

    public enum NodeImage { None = -1, Bank = 0, Envelope = 1, Money = 2, ErrorFlag = 3 };
    public enum MyNodes { Root, AccountType, EnvelopeGroup, Account, Envelope, AENode }

    public class BaseNode : TreeList.Node
    {
        public readonly MyNodes NodeType;
        private int ErrorCount;
        private int origionalImageID;

        protected BaseNode (MyNodes nodeType, String name)
            : base(name)
        {
            this.NodeType = nodeType;
        }

        public void SetError()
        {
            ErrorCount++;
            setImage();
        }

        public void RemoveError()
        {
            ErrorCount--;
            setImage();
        }

        private void setImage()
        {
            if (ErrorCount < 0)
                ErrorCount = 0;

            if (ErrorCount > 0)
                this.ImageId = (int)NodeImage.ErrorFlag;

            else
                this.ImageId = this.origionalImageID; ;
        }
    }

    public class RootNode : BaseNode
    {
        public readonly byte  Catagory;

        public RootNode(byte catagory, string name)
            : base(MyNodes.Root, name) 
        {
            this.Catagory = catagory;
        }
    }

    public class TypeNode : BaseNode
    {
        public readonly byte Catagory;
        public readonly int TypeID;

        public TypeNode(int typeID, string name, byte catagory)
            : base(MyNodes.AccountType, name)
        {
            this.TypeID = typeID;
            this.Catagory = catagory;
        }
    }

    public class GroupNode : BaseNode
    {
        public readonly int GroupID;

        public GroupNode(int groupID, string name)
            : base(MyNodes.EnvelopeGroup, name)
        {
            this.GroupID = groupID;
        }
    }

    public class AccountNode : BaseNode
    {
        public readonly byte Catagory;
        public readonly int AccountID;
        public readonly bool Envelopes;

        public AccountNode(int accountID, string name, bool envelopes, decimal balance)
            : base(MyNodes.Account, name)
        {
            this.Catagory = SpclAccountCat.ACCOUNT;
            this.AccountID = accountID;
            this.Envelopes = envelopes;
            this[0] = name;
            this[1] = balance.ToString("C2");
        }

        public AccountNode(byte catagory, int accountID, string name)
            : base(MyNodes.Account, name)
        {
            this.Catagory = catagory;
            this.AccountID = accountID;
            this.Envelopes = false;
        }
    }

    public class EnvelopeNode : BaseNode
    {
        public readonly int EnvelopeID;

        public EnvelopeNode(int envelopeID, string name, decimal balance)
            : base(MyNodes.Envelope, name)
        {
            this.EnvelopeID = envelopeID;
            this[0] = name;
            this[1] = balance.ToString("C2");
        }
    }

    public class AENode : BaseNode
    {
        public readonly int EnvelopeID;
        public readonly int AccountID;

        public AENode(int accountID, int envelopeID, string name, decimal balance)
            : base(MyNodes.AENode, name)
        {
            this.AccountID = accountID;
            this.EnvelopeID = envelopeID;
            this[0] = name;
            this[1] = balance.ToString("C2");
        }
    }
}
