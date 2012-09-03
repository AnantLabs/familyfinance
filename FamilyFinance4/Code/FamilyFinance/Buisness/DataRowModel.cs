using System;
using System.Data;


using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public class DataRowModel : BindableObject
    {
        protected virtual bool isDataRowNull(DataRow dataRow)
        {

            if (dataRow == null)
                return true;

            if (dataRow.RowState == System.Data.DataRowState.Deleted)
                return true;

            if (dataRow.RowState == System.Data.DataRowState.Detached)
                return true;

            else
                return false;
        }
    }
}
