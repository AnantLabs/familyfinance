namespace FamilyFinance.Model
{
    class IdNameCat
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public string Catagory { get; private set; }

        public IdNameCat(int id, string name, string catagory)
        {
            this.ID = id;
            this.Name = name;
            this.Catagory = catagory;
        }


    }

    class IdNameCatComparer : System.Collections.Generic.IComparer<IdNameCat>
    {
        public int Compare(IdNameCat x, IdNameCat y)
        {
            int cName = string.Compare(x.Name, y.Name);
            int cCat = string.Compare(x.Catagory, y.Catagory);
            int cID = x.ID - y.ID;

            if (x.ID > 0 && y.ID > 0)
            {
                if (cCat == 0)
                    return cName;
                else
                    return cCat;
            }
            else
            {
                return cID;
            }
        }
    }
}
