namespace FamilyFinance.Model
{
    class IdName
    {
        public int ID { get; private set; }
        public string Name { get; private set; }

        public IdName(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

    }

    class IdNameComparer : System.Collections.Generic.IComparer<IdName>
    {
        public int Compare(IdName x, IdName y)
        {
            int cName = string.Compare(x.Name, y.Name);
            int cID = x.ID - y.ID;

            if (x.ID > 0 && y.ID > 0)
                return cName;
            else
                return cID;
        }
    }
}
