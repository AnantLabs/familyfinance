using System;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using System.Collections.Generic;

namespace FamilyFinance2
{
    partial class FFDBDataSet
    {
        partial class EnvelopeDataTable
        {
            ///////////////////////////////////////////////////////////////////////
            //   Local Variables
            ///////////////////////////////////////////////////////////////////////
            private FFDBDataSetTableAdapters.EnvelopeTableAdapter thisTableAdapter;

            private bool autoChange;


            ///////////////////////////////////////////////////////////////////////
            //   Properties
            ///////////////////////////////////////////////////////////////////////


            ////////////////////////////////////////////////////////////////////////////////////////////
            //   Functions Override
            ////////////////////////////////////////////////////////////////////////////////////////////
            public override void EndInit()
            {
                base.EndInit();

                this.thisTableAdapter = new FFDBDataSetTableAdapters.EnvelopeTableAdapter();
                this.thisTableAdapter.ClearBeforeFill = true;

                this.TableNewRow += new System.Data.DataTableNewRowEventHandler(EnvelopeDataTable_TableNewRow);
                this.ColumnChanged += new DataColumnChangeEventHandler(EnvelopeDataTable_ColumnChanged);

                autoChange = true;
            }


            ////////////////////////////////////////////////////////////////////////////////////////////
            //   Internal Events
            ////////////////////////////////////////////////////////////////////////////////////////////
            private void EnvelopeDataTable_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
            {
                autoChange = false;

                EnvelopeRow envelopeRow = e.Row as EnvelopeRow;
                    
                envelopeRow.id = Convert.ToInt16(FFDBDataSet.myDBGetNewID("id", "Envelope"));
                envelopeRow.name = "";
                envelopeRow.fullName = "";
                envelopeRow.parentEnvelope = SpclEnvelope.NULL;
                envelopeRow.closed = false;
                envelopeRow.endingBalance = 0.0m;
                envelopeRow.currentBalance = 0.0m;

                autoChange = true;
            }

            private void EnvelopeDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                EnvelopeRow row;
                string tmp;
                int maxLen;

                if (autoChange == false)
                    return;

                autoChange = false;

                row = e.Row as EnvelopeRow;


                switch (e.Column.ColumnName)
                {
                    case "name":
                        {
                            tmp = e.ProposedValue as string;
                            maxLen = this.nameColumn.MaxLength;

                            if (tmp.Length > maxLen)
                                row.name = tmp.Substring(0, maxLen);

                            mySetFullName(ref row);
                            break;
                        }

                    case "parentEnvelope":
                        {
                            mySetFullName(ref row);
                            break;
                        }
                }

                autoChange = true;
            }



            ////////////////////////////////////////////////////////////////////////////////////////////
            //   External Events
            ////////////////////////////////////////////////////////////////////////////////////////////



            ////////////////////////////////////////////////////////////////////////////////////////////
            //   Functions Private
            ////////////////////////////////////////////////////////////////////////////////////////////
            private void mySetFullName(ref EnvelopeRow thisEnvelope)
            {
                string fullName;
                List<short> childIDList;
                int maxLen = this.fullNameColumn.MaxLength;

                if (thisEnvelope == null)
                    return;

                // Get this envelope Full Name
                fullName = this.myGetFullName(ref thisEnvelope);

                // Truncate if too long
                if (fullName.Length > maxLen)
                    fullName = fullName.Substring(0, maxLen);

                // Set the Full name
                thisEnvelope.fullName = fullName;

                // Find all the child envelopes and update their full names too.
                childIDList = this.myGetChildEnvelopeIDList(thisEnvelope.id);

                foreach (short id in childIDList)
                {
                    EnvelopeRow row = this.FindByid(id);
                    mySetFullName(ref row);
                }
            }

            private string myGetFullName(ref EnvelopeRow thisEnvelope)
            {
                if (thisEnvelope.parentEnvelope == SpclEnvelope.NULL)
                    return thisEnvelope.name;

                else
                {
                    EnvelopeRow parent = this.FindByid(thisEnvelope.parentEnvelope);
                    return this.myGetFullName(ref parent) + ":" + thisEnvelope.name;
                }
            }


            ////////////////////////////////////////////////////////////////////////////////////////////
            //   Functions Public
            ////////////////////////////////////////////////////////////////////////////////////////////
            public void myUpdateTA()
            {
                this.thisTableAdapter.Update(this);
            }

            public void myFillTA()
            {
                this.thisTableAdapter.Fill(this);
            }



            public void myUpdateEnvelopeEBUndo(short oldEnvelopeID, bool oldCD, decimal oldAmount)
            {
                EnvelopeRow row = this.FindByid(oldEnvelopeID);

                // Undo the old Amount
                if (oldCD == LineCD.CREDIT)
                    row.endingBalance += oldAmount;

                else
                    row.endingBalance -= oldAmount;

                //OnEnvelopeEndingBalanceChanged(new BalanceChangedEventArgs(SpclAccount.NULL, row.id, row.endingBalance));

            }

            public void myUpdateEnvelopeEBDo(short newEnvelopeID, bool newCD, decimal newAmount)
            {
                EnvelopeRow row = FindByid(newEnvelopeID);

                //  Update to the new amount
                if (newCD == LineCD.CREDIT)
                    row.endingBalance -= newAmount;

                else
                    row.endingBalance += newAmount;

                //OnEnvelopeEndingBalanceChanged(new BalanceChangedEventArgs(SpclAccount.NULL, row.id, row.endingBalance));
            }

            public void myUpdateEnvelopeEBUndoDo(short oldEnvelopeID, bool oldCD, decimal oldAmount, short newEnvelopeID, bool newCD, decimal newAmount)
            {
                EnvelopeRow oldEnvelopeRow = FindByid(oldEnvelopeID);
                EnvelopeRow newEnvelopeRow = FindByid(newEnvelopeID);

                // Undo the old Amount
                if (oldCD == LineCD.CREDIT)
                    oldEnvelopeRow.endingBalance += oldAmount;

                else
                    oldEnvelopeRow.endingBalance -= oldAmount;


                //  Update to the new amount
                if (newCD == LineCD.CREDIT)
                    newEnvelopeRow.endingBalance -= newAmount;

                else
                    newEnvelopeRow.endingBalance += newAmount;


                //if (oldEnvelopeID == newEnvelopeID)
                //{
                //    OnEnvelopeEndingBalanceChanged(new BalanceChangedEventArgs(SpclAccount.NULL, newEnvelopeID, newEnvelopeRow.endingBalance));
                //}
                //else
                //{
                //    OnEnvelopeEndingBalanceChanged(new BalanceChangedEventArgs(SpclAccount.NULL, newEnvelopeID, newEnvelopeRow.endingBalance));
                //    OnEnvelopeEndingBalanceChanged(new BalanceChangedEventArgs(SpclAccount.NULL, oldEnvelopeID, oldEnvelopeRow.endingBalance));
                //}

            }



            public List<short> myGetChildEnvelopeIDList(short envelopeID)
            {
                List<short> idList = new List<short>();

                foreach (EnvelopeRow row in this)
                {
                    if (row.id > 0 && row.parentEnvelope == envelopeID)
                        idList.Add(row.id);
                }

                idList.Sort();
                return idList;
            }

            public List<short> myGetAllChildEnvelopeIDList(short envelopeID)
            {
                List<short> idList = myGetChildEnvelopeIDList(envelopeID);
                List<short> temp;

                for (int i = 0; i < idList.Count; i++ )
                {
                    temp = myGetChildEnvelopeIDList(idList[i]);

                    foreach (short id in temp)
                        idList.Add(id);
                }

                return idList;
            }


        }// END Envelope partial class
    }// END partial class FamilyFinanceDBDataSet
} //END namespace FamilyFinance
