using System.Windows.Controls;
using System.Windows;

namespace FamilyFinance.Custom
{
    class ExtendedTextColumn : DataGridTextColumn
    {

        public HorizontalAlignment HorizontalAlignment
        {
            get;
            set;

        }

        public VerticalAlignment VerticalAlignment
        {
            get;
            set;
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            FrameworkElement element = base.GenerateElement(cell, dataItem);

            element.HorizontalAlignment = HorizontalAlignment;
            element.VerticalAlignment = VerticalAlignment;

            return element;
        }

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            TextBox textBox = (TextBox)base.GenerateEditingElement(cell, dataItem);

            switch (HorizontalAlignment)
            {
                case System.Windows.HorizontalAlignment.Center:
                    textBox.TextAlignment = TextAlignment.Center;
                    break;

                case System.Windows.HorizontalAlignment.Left:
                    textBox.TextAlignment = TextAlignment.Left;
                    break;

                case System.Windows.HorizontalAlignment.Right:
                    textBox.TextAlignment = TextAlignment.Right;
                    break;

                case System.Windows.HorizontalAlignment.Stretch:
                    textBox.TextAlignment = TextAlignment.Justify;
                    break;
            }

            textBox.VerticalContentAlignment = VerticalAlignment;

            return textBox;
        }

    }
}
