using FamilyFinance.Database;

namespace FamilyFinance.Model
{
    /// <summary>
    /// Keeps track of the static settings that are stored in the database.
    /// The values can be changed and saved. The defaults are kept in this class.
    /// </summary>
    class SettingsModel : ModelBase
    {

        /// <summary>
        /// The current databases version.
        /// </summary>
        public static SettingsModel DBVersion = new SettingsModel("DBVersion", "2");

        /// <summary>
        /// The id value of the setting. This is also the hash code of the name of the setting to create
        /// a unique id value.
        /// </summary>
        private readonly int id;

        /// <summary>
        /// The default value for the setting.
        /// </summary>
        private readonly string defaultValue;

        /// <summary>
        /// Gets or sets the value of the setting.
        /// </summary>
        public string Value
        {
            get 
            {
                FFDataSet.SettingsRow row = MyData.getInstance().Settings.FindByid(this.id);

                if (row == null)
                {
                    row = MyData.getInstance().Settings.NewSettingsRow();
                    
                    row.id = this.id;
                    row.value = this.defaultValue;

                    MyData.getInstance().Settings.AddSettingsRow(row);
                    MyData.getInstance().saveRow(row);
                }

                return row.value; 
            }

            set
            {
                FFDataSet.SettingsRow row = MyData.getInstance().Settings.FindByid(this.id);

                if (row == null)
                {
                    row = MyData.getInstance().Settings.NewSettingsRow();
                    row.id = this.id;
                }

                row.value = value;
                MyData.getInstance().saveRow(row);

                this.RaisePropertyChanged("Value");
            }
        }

        /// <summary>
        /// Gets or sets the value if it is an integer. This will throw exceptions if trying to get
        /// a settings value that can not be parsed to an integer.
        /// </summary>
        public int ValueInt
        {
            get { return int.Parse(this.Value); }
            set
            {
                this.Value = value.ToString();
                this.RaisePropertyChanged("ValueInt");
            }
        }

        /// <summary>
        /// Sets the default value for this setting.
        /// </summary>
        public void setDefault()
        {
            this.Value = this.defaultValue;
        }

        /// <summary>
        /// Prevents outside instantiation of this class. This is esentially an Enum like the kind
        /// available in Java.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        /// <param name="dValue">The default value for the setting.</param>
        private SettingsModel(string name, string dValue)
        {
            this.id = name.GetHashCode();
            this.defaultValue = dValue;
        }

    }
}
