using System;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using System.Collections.Generic;

namespace FamilyFinance2
{
    partial class FFDBDataSet
    {
        partial class AccountCatagoryDataTable
        {
            ///////////////////////////////////////////////////////////////////////
            //   Local Variables
            ///////////////////////////////////////////////////////////////////////

            ///////////////////////////////////////////////////////////////////////
            //   Overriden Functions 
            ///////////////////////////////////////////////////////////////////////
            public override void EndInit()
            {
                base.EndInit();
            }


            ///////////////////////////////////////////////////////////////////////
            //   Internal Events
            ///////////////////////////////////////////////////////////////////////


            ///////////////////////////////////////////////////////////////////////
            //   Functions Public
            ///////////////////////////////////////////////////////////////////////
            public void myFill()
            {
                string query = "SELECT * FROM AccountCatagory;";
                object[] newRow = new object[2];

                this.Rows.Clear();

                SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
                connection.Open();
                SqlCeCommand command = new SqlCeCommand(query, connection);
                SqlCeDataReader reader = command.ExecuteReader();

                // Iterate through the results
                while (reader.Read())
                {
                    reader.GetValues(newRow);
                    this.Rows.Add(newRow);
                }

                // Always call Close the reader and connection when done reading
                reader.Close();
                connection.Close();
                this.AcceptChanges();
            }




        }// END partial class AccountTypeDataTable
    }// END partial class FamilyFinanceDBDataSet
} // END namespace FamilyFinance
