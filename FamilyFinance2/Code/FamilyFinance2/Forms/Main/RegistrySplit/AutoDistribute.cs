using System;
using System.Collections.Generic;
using System.Text;

using FamilyFinance2.Forms.Import;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.Main.RegistrySplit
{
    static class AutoDistribute
    {
        private class EnvBal
        {
            public int envelopeID;
            public decimal balance = 0.0m;

            public EnvBal(int eID, decimal bal)
            {
                this.envelopeID = eID;
                this.balance = bal;
            }
        }

        private class LineSum
        {
            public FFDataSet.LineItemRow Line;
            public decimal RemainingAmount = 0.0m;

            public LineSum(FFDataSet.LineItemRow line, decimal remainingAmount)
            {
                this.Line = line;
                this.RemainingAmount = remainingAmount;
            }
        }

        static public void Distribute(int accountID)
        {
            List<EnvBal> envelopes = new List<EnvBal>();
            Queue<LineSum> holdLines = new Queue<LineSum>();
            decimal skipAmount = 0.0m;

            FFDataSet dataSet = new FFDataSet();
            dataSet.myInit();
            dataSet.myFillForAutoDistribute();
            
            for (int index = 0; index < dataSet.LineItem.Count; index++)
            {
                FFDataSet.LineItemRow line = dataSet.LineItem[index];

                if (line.accountID != accountID)
                    continue;

                FFDataSet.EnvelopeLineRow[] envLines = line.GetEnvelopeLineRows();
                decimal envSum = envLineSum(envLines);

                // Decide what to do with this line
                if (envSum == line.amount)
                    updateEnvelopes(line.creditDebit, envLines, envelopes);

                else if (copyFromOppLines(dataSet, line, envSum))
                {
                    envLines = line.GetEnvelopeLineRows();
                    updateEnvelopes(line.creditDebit, envLines, envelopes);

                    if (envLines.Length > 1)
                        line.envelopeID = SpclEnvelope.SPLIT;

                    else if (envLines.Length == 1)
                        line.envelopeID = envLines[0].envelopeID;

                }
                else if (line.creditDebit == LineCD.CREDIT && envLines.Length > 0)
                {
                    updateEnvelopes(line.creditDebit, envLines, envelopes);
                    skipAmount += line.amount - envSum;
                }
                else if (line.creditDebit == LineCD.CREDIT && envLines.Length == 0)
                {
                    skipAmount += line.amount;
                }
                else if (line.creditDebit == LineCD.DEBIT)
                {
                    if (envLines.Length > 0)
                        updateEnvelopes(line.creditDebit, envLines, envelopes);

                    holdLines.Enqueue(new LineSum(line, line.amount - envSum));
                }

                // See if there are enough negative envelopes and skip amounts to disribute the 
                // next line being held.
                while (holdLines.Count > 0 && holdLines.Peek().RemainingAmount <= skipAmount)
                {
                    LineSum lineSum = holdLines.Dequeue();
                    skipAmount -= lineSum.RemainingAmount;
                }

                while(holdLines.Count > 0 && holdLines.Peek().RemainingAmount <= skipAmount + getNegativeSum(envelopes))
                {
                    LineSum lineSum = holdLines.Dequeue();
                    fillInFromEnvelopeLines(dataSet, lineSum, envelopes, skipAmount);
                    skipAmount = 0.0m;

                    envLines = lineSum.Line.GetEnvelopeLineRows();

                    if (envLines.Length > 1)
                        lineSum.Line.envelopeID = SpclEnvelope.SPLIT;

                    else if (envLines.Length == 1)
                        lineSum.Line.envelopeID = envLines[0].envelopeID;
                }
            }

            dataSet.mySaveData();
            dataSet.Clear();
            envelopes.Clear();
            holdLines.Clear();
        }

        private static void fillInFromEnvelopeLines(FFDataSet dataSet, LineSum lineSum, List<EnvBal> envelopes, decimal skipAmount)
        {
            decimal remainingAmount = lineSum.RemainingAmount - skipAmount;

            while (remainingAmount > 0.0m)
            {
                FFDataSet.EnvelopeLineRow envLine = null;
                EnvBal envelope = getNegativeEnvelope(envelopes);

                if (envelope == null)
                    break;

                // Make a new envelope line for this envelope
                envLine = dataSet.EnvelopeLine.NewEnvelopeLineRow();

                envLine.lineItemID = lineSum.Line.id;
                envLine.envelopeID = envelope.envelopeID;
                envLine.description = "";
                envLine.amount = 0.0m;

                dataSet.EnvelopeLine.AddEnvelopeLineRow(envLine);

                // decide how much to change the envelopeLine.amount
                if (remainingAmount >= + Math.Abs(envelope.balance))
                {
                    envLine.amount = Math.Abs(envelope.balance);
                    remainingAmount -= Math.Abs(envelope.balance);
                    envelope.balance = 0.0m;
                }
                else
                {
                    envLine.amount = remainingAmount;
                    envelope.balance += remainingAmount;
                    remainingAmount = 0.0m;
                }
            }
        }

        private static decimal envLineSum(FFDataSet.EnvelopeLineRow[] envLines)
        {
            decimal sum = 0.0m;

            foreach (FFDataSet.EnvelopeLineRow row in envLines)
                sum += row.amount;

            return sum;
        }

        private static void updateEnvelopes(bool lineCD, FFDataSet.EnvelopeLineRow[] envLines, List<EnvBal> envelopes)
        {
            bool found;
            decimal amount;

            foreach (FFDataSet.EnvelopeLineRow eLine in envLines)
            {
                found = false;

                if(lineCD == LineCD.CREDIT)
                    amount = Decimal.Negate(eLine.amount);
                else
                    amount = eLine.amount;

                foreach (EnvBal eSum in envelopes)
                {
                    if (eSum.envelopeID == eLine.envelopeID)
                    {
                        found = true;
                        eSum.balance += amount;
                    }
                }

                if (!found)
                    envelopes.Add(new EnvBal(eLine.envelopeID, amount));
            }
        }

        private static bool copyFromOppLines(FFDataSet dataSet, FFDataSet.LineItemRow line, decimal envSum)
        {
            bool copied = false;

            List<FFDataSet.EnvelopeLineRow> oppELines = new List<FFDataSet.EnvelopeLineRow>();
            decimal sum = 0.0m;

            // get opp Envelope lines
            foreach(FFDataSet.EnvelopeLineRow oppELine in dataSet.EnvelopeLine)
            {
                if (oppELine.LineItemRow.transactionID == line.transactionID
                    && oppELine.LineItemRow.creditDebit != line.creditDebit)
                {
                    oppELines.Add(oppELine);
                    sum += oppELine.amount;
                }
            }

            if (sum == line.amount - envSum)
            {
                foreach (FFDataSet.EnvelopeLineRow eLine in oppELines)
                {
                    FFDataSet.EnvelopeLineRow newEline = dataSet.EnvelopeLine.NewEnvelopeLineRow();

                    newEline.lineItemID = line.id;
                    newEline.envelopeID = eLine.envelopeID;
                    newEline.description = eLine.description;
                    newEline.amount = eLine.amount;

                    dataSet.EnvelopeLine.AddEnvelopeLineRow(newEline);

                    if (oppELines.Count == 1)
                        newEline.LineItemRow.envelopeID = eLine.envelopeID;

                    else if (oppELines.Count > 1)
                        newEline.LineItemRow.envelopeID = SpclEnvelope.SPLIT;
                }

                copied = true;
            }

            return copied;
        }

        private static decimal getNegativeSum(List<EnvBal> envelopes)
        {
            decimal negativeSum = 0.0m;

            foreach (EnvBal eSum in envelopes)
            {
                if (eSum.balance < 0.0m)
                    negativeSum += eSum.balance;
            }

            return Decimal.Negate(negativeSum);
        }

        private static EnvBal getNegativeEnvelope(List<EnvBal> envelopes)
        {
            decimal smallestNegative = Decimal.Negate(Decimal.MaxValue);
            EnvBal smallest = null;

            foreach(EnvBal eSum in envelopes)
                if (eSum.balance < 0.0m && eSum.balance > smallestNegative)
                {
                    smallestNegative = eSum.balance;
                    smallest = eSum;
                }

            return smallest;
        }

    }
}
