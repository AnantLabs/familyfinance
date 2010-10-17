using FamilyFinance.Database;

namespace FamilyFinance.Model
{
    class EnvelopeGoalModel : EnvelopeModel
    {
        private FFDataSet.GoalRow goalRow;

        public int EnvelopeID
        {
            get
            {
                if (this.goalRow == null)
                    return SpclAccount.NULL;
                else
                    return this.goalRow.envelopeID;
            }
        }

        public int Priority
        {
            get
            {
                if (this.goalRow == null)
                    return int.MaxValue;
                else
                    return this.goalRow.priority;
            }

            set
            {
                if (this.goalRow != null)
                {
                    this.goalRow.priority = value;

                    this.saveRow();
                    this.RaisePropertyChanged("Priority");
                }
            }
        }

        public string Notes
        {
            get
            {
                if (this.goalRow == null)
                    return "";
                else
                    return this.goalRow.notes;
            }
        }

        public decimal Step
        {
            get
            {
                if (this.goalRow == null)
                    return 0.0m;
                else
                    return this.goalRow.step;
            }

            set
            {
                if (this.goalRow != null)
                {
                    this.goalRow.step = value;
                    
                    this.saveRow();
                    this.RaisePropertyChanged("Step");
                }
            }
        }

        public decimal Goal
        {
            get
            {
                if (this.goalRow == null)
                    return 0.0m;
                else
                    return this.goalRow.goal;
            }

            set
            {
                if (this.goalRow != null)
                {
                    this.goalRow.goal = value;

                    this.saveRow();
                    this.RaisePropertyChanged("Goal");
                }
            }
        }

        public System.DateTime DueDate
        {
            get
            {
                if (this.goalRow == null)
                    return System.DateTime.MinValue;
                else
                    return this.goalRow.dueDate;
            }

            set
            {
                if (this.goalRow != null)
                {
                    this.goalRow.dueDate = value;

                    this.saveRow();
                    this.RaisePropertyChanged("DueDate");
                }
            }
        }


        public EnvelopeGoalModel(FFDataSet.EnvelopeRow eRow) : base(eRow)
        {
            this.goalRow = MyData.getInstance().Goal.FindByenvelopeID(eRow.id);
        }

        public EnvelopeGoalModel() : base()
        {
            this.goalRow = null;
        }

        public void addGoal()
        {
            if (this.goalRow == null)
            {
                this.goalRow = MyData.getInstance().Goal.NewGoalRow();
                MyData.getInstance().Goal.AddGoalRow(this.goalRow);
                this.saveRow();
            }
        }

        private void saveRow()
        {
            MyData.getInstance().saveGoalRow(this.goalRow);
        }
    }
}
