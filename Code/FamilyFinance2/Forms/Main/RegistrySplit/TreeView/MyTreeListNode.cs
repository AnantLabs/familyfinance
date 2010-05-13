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
            this.HasChildren = true;
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
            this.HasChildren = true;
        }
    }

    public class GroupNode : BaseNode
    {
        public readonly int GroupID;

        public GroupNode(int groupID, string name)
            : base(MyNodes.EnvelopeGroup, name)
        {
            this.GroupID = groupID;
            this.HasChildren = true;
        }
    }

    public class AccountNode : BaseNode
    {
        public readonly byte Catagory;
        public readonly int AccountID;

        public AccountNode(byte catagory, int accountID, string name, bool envelopes)
            : base(MyNodes.Account, name)
        {
            this.Catagory = catagory;
            this.AccountID = accountID;
            this[0] = name;

            if(catagory == SpclAccountCat.ACCOUNT)
            {
                this.HasChildren = envelopes;
                this.ImageId = (int)NodeImage.Bank;
                this.setBalance(0.0m);
            }
            else
            {
                this.HasChildren = false;
                this.ImageId = (int)NodeImage.None;
            }
        }

        public void setBalance(decimal balance)
        {
            this[1] = balance.ToString("C2");
        }
    }

    public class EnvelopeNode : BaseNode
    {
        public readonly int EnvelopeID;

        public EnvelopeNode(int envelopeID, string name)
            : base(MyNodes.Envelope, name)
        {
            this.EnvelopeID = envelopeID;
            this.HasChildren = true;
            this.ImageId = (int)NodeImage.Envelope;
            this[0] = name;
            this.setBalance(0.0m);
        }

        public void setBalance(decimal balance)
        {
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
            this.setBalance(balance);
            this.HasChildren = false;
        }

        public void setBalance(decimal balance)
        {
            this[1] = balance.ToString("C2");
        }
    }
}
