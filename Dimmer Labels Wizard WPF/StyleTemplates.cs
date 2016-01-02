using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Dimmer_Labels_Wizard_WPF
{
    public abstract class LabelStripTemplateBase : Style
    {
        #region Methods
        protected object GetSetterValue(DependencyProperty property)
        {
            var query = GetSetter(property, Setters);

            if (query == null)
            {
                return FindAncestorsSetter(property).Value;
            }

            else
            {
                return query.Value;
            }
        }

        protected Setter FindAncestorsSetter(DependencyProperty property)
        {
            if (BasedOn != null)
            {
                bool searchComplete = false;
                Style searchTarget = BasedOn;
                Setter searchResult = null;

                while (searchComplete == false)
                {
                    if ((searchResult = GetSetter(property, searchTarget.Setters)) == null)
                    {
                        // No matching setter found in this node.
                        if (searchTarget.BasedOn != null)
                        {
                            searchTarget = searchTarget.BasedOn;
                        }

                        else
                        {
                            // Search Complete. No Setter found.
                            searchComplete = true;
                            return null;
                        }
                    }

                    else
                    {
                        // Setter Found.
                        searchComplete = true;
                        return searchResult;
                    }
                }

                return null;
            }

            else
            {
                return null;
            }
        } 

        protected Setter GetSetter(DependencyProperty property, SetterBaseCollection setterCollection)
        {
            // Cast to Queryable List.
            List<Setter> setters = new List<Setter>(setterCollection.AsEnumerable().Cast<Setter>());

            var query = setters.Find(item => item.Property == property);

            return query;
        }

        public void SetSetterValue(DependencyProperty property, object value, Type expectedType)
        {
            if (value.GetType() != property.PropertyType)
            {
                // Incorrect Type.
                throw new InvalidCastException("Type Error");
            }

            if (GetSetter(property, Setters) == null)
            {
                // Add new Setter
                Setters.Add(new Setter(property, value));
            }

            else
            {
                // Modify Existing Setter
                GetSetter(property, Setters).Value = value;
            }
        }
        #endregion
    }

    public class LabelStripTemplate : LabelStripTemplateBase
    {
        public LabelStripTemplate()
        {
            TargetType = typeof(LabelStrip);
        }

        public LabelStripTemplate(LabelStripTemplate basedOn)
        {
            TargetType = typeof(LabelStrip);
            BasedOn = basedOn;
        }

        public string Name = "No Name";
        public List<Strip> AssignedToStrips = new List<Strip>();


        #region Styling Values.
        // Upper Cells Template
        public object UpperCellsTemplate
        {
            get
            {
                return GetSetterValue(LabelStrip.UpperCellsTemplateProperty);
            }
            set
            {
                SetSetterValue(LabelStrip.UpperCellsTemplateProperty, value, typeof(LabelCellTemplate));
            }
        }

        public object LowerCellsTemplate
        {
            get
            {
                return GetSetterValue(LabelStrip.LowerCellsTemplateProperty);
            }
            set
            {
                SetSetterValue(LabelStrip.LowerCellsTemplateProperty, value, typeof(LabelCellTemplate));
            }
        }

        // Strip Height
        public object StripHeight
        {
            get
            {
                return GetSetterValue(LabelStrip.StripHeightProperty);
            }
            set
            {
                SetSetterValue(LabelStrip.StripHeightProperty, value, typeof(double));
            }
        }

        // Strip Mode.
        public object StripMode
        {
            get
            {
                return GetSetterValue(LabelStrip.StripModeProperty);
            }
            set
            {
                SetSetterValue(LabelStrip.StripModeProperty, value, typeof(LabelStripMode));
            }
        }
        #endregion

        #region Overrides.
        public override string ToString()
        {
            return Name;
        }
        #endregion
    }

    public class LabelCellTemplate : LabelStripTemplateBase
    {
        public LabelCellTemplate()
        {
            TargetType = typeof(LabelCell);
        }

        // Cell Row Templates
        public object CellRowTemplates
        {
            get
            {
                return GetSetterValue(LabelCell.RowTemplatesProperty);
            }
            set
            {
                SetSetterValue(LabelCell.RowTemplatesProperty, value, typeof(IEnumerable<CellRowTemplate>));
            }
        }
        
        // Row Height Mode.
        public object RowHeightMode
        {
            get
            {
                return GetSetterValue(LabelCell.RowHeightModeProperty);
            }
            set
            {
                SetSetterValue(LabelCell.RowHeightModeProperty, value, typeof(CellRowHeightMode));
            }
        }

        // Row Count.
        public object RowCount
        {
            get
            {
                return GetSetterValue(LabelCell.RowCountProperty);
            }
            set
            {
                SetSetterValue(LabelCell.RowCountProperty, value, typeof(int));
            }
        }

        // Width.
        public object Width
        {
            get
            {
                return GetSetterValue(LabelCell.WidthProperty);
            }
            set
            {
                SetSetterValue(LabelCell.WidthProperty, value, typeof(double));
            }
        }

        // Cell Data Mode.
        public object CellDataMode
        {
            get
            {
                return GetSetterValue(LabelCell.CellDataModeProperty);
            }
            set
            {
                SetSetterValue(LabelCell.CellDataModeProperty, value, typeof(CellDataMode));
            }
        }


    }

    public class CellRowTemplate : LabelStripTemplateBase
    {
        public CellRowTemplate()
        {
            TargetType = typeof(CellRow);
        }

        // DataField.
        public object DataField
        {
            get
            {
                return GetSetterValue(CellRow.DataFieldProperty);
            }
            set
            {
                SetSetterValue(CellRow.DataFieldProperty, value, typeof(LabelField));
            }
        }

        // Font.
        public object Font
        {
            get
            {
                return GetSetterValue(CellRow.FontProperty);
            }
            set
            {
                SetSetterValue(CellRow.FontProperty, value, typeof(Typeface));
            }
        }

        // Desired Font Size.
        public object DesiredFontSize
        {
            get
            {
                return GetSetterValue(CellRow.DesiredFontSizeProperty);
            }
            set
            {
                SetSetterValue(CellRow.DesiredFontSizeProperty, value, typeof(double));
            }
        }
    }
}
