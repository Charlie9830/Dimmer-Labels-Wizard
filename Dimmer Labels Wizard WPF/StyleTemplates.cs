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

        public void SetSetterValue(DependencyProperty property, object value)
        {
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
        // StripWidth
        public double StripWidth
        {
            get
            {
                return (double)GetSetterValue(LabelStrip.StripWidthProperty);
            }
            set
            {
                SetSetterValue(LabelStrip.StripWidthProperty, value);
            }
        }

        // Upper Cells Template
        public IEnumerable<LabelCellTemplate> UpperCellTemplates
        {
            get
            {
                return (IEnumerable<LabelCellTemplate>)GetSetterValue(LabelStrip.UpperCellTemplatesProperty);
            }
            set
            {
                SetSetterValue(LabelStrip.UpperCellTemplatesProperty, value);
            }
        }

        public IEnumerable<LabelCellTemplate> LowerCellTemplates
        {
            get
            {
                return (IEnumerable<LabelCellTemplate>)GetSetterValue(LabelStrip.LowerCellTemplatesProperty);
            }
            set
            {
                SetSetterValue(LabelStrip.LowerCellTemplatesProperty, value);
            }
        }

        // Strip Height
        public double StripHeight
        {
            get
            {
                return (double)GetSetterValue(LabelStrip.StripHeightProperty);
            }
            set
            {
                SetSetterValue(LabelStrip.StripHeightProperty, value);
            }
        }

        // Strip Mode.
        public LabelStripMode StripMode
        {
            get
            {
                return (LabelStripMode)GetSetterValue(LabelStrip.StripModeProperty);
            }
            set
            {
                SetSetterValue(LabelStrip.StripModeProperty, value);
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

        public LabelCellTemplate(LabelCellTemplate basedOn)
        {
            TargetType = typeof(LabelCell);
            BasedOn = basedOn;
        }

        // Cell Row Templates
        public IEnumerable<CellRowTemplate> CellRowTemplates
        {
            get
            {
                return (IEnumerable<CellRowTemplate>)GetSetterValue(LabelCell.RowTemplatesProperty);
            }
            set
            {
                SetSetterValue(LabelCell.RowTemplatesProperty, value);
            }
        }
        
        // Row Height Mode.
        public CellRowHeightMode RowHeightMode
        {
            get
            {
                return (CellRowHeightMode)GetSetterValue(LabelCell.RowHeightModeProperty);
            }
            set
            {
                SetSetterValue(LabelCell.RowHeightModeProperty, value);
            }
        }

        // Row Count.
        public int RowCount
        {
            get
            {
                return (int)GetSetterValue(LabelCell.RowCountProperty);
            }
            set
            {
                SetSetterValue(LabelCell.RowCountProperty, value);
            }
        }

        // SingleField Font
        public Typeface SingleFieldFont
        {
            get
            {
                return (Typeface)GetSetterValue(LabelCell.SingleFieldFontProperty);
            }
            set
            {
                SetSetterValue(LabelCell.SingleFieldFontProperty, value);
            }
        }

        // SingleFieldDesiredFontSize
        public double SingleFieldDesiredFontSize
        {
            get
            {
                return (double)GetSetterValue(LabelCell.SingleFieldDesiredFontSizeProperty);
            }
            set
            {
                SetSetterValue(LabelCell.SingleFieldDesiredFontSizeProperty, value);
            }
        }

        // SingleFieldDataField
        public LabelField SingleFieldDataField
        {
            get
            {
                return (LabelField)GetSetterValue(LabelCell.SingleFieldDataFieldProperty);
            }
            set
            {
                SetSetterValue(LabelCell.SingleFieldDataFieldProperty, value);
            }
        }

        // Cell Data Mode.
        public CellDataMode CellDataMode
        {
            get
            {
                return (CellDataMode)GetSetterValue(LabelCell.CellDataModeProperty);
            }
            set
            {
                SetSetterValue(LabelCell.CellDataModeProperty, value);
            }
        }

        // Left Weight
        public double LeftWeight
        {
            get
            {
                return (double)GetSetterValue(LabelCell.LeftWeightProperty);
            }
            set
            {
                SetSetterValue(LabelCell.LeftWeightProperty, value);
            }
        }

        // Top Weight
        public double TopWeight
        {
            get
            {
                return (double)GetSetterValue(LabelCell.TopWeightProperty);
            }
            set
            {
                SetSetterValue(LabelCell.TopWeightProperty, value);
            }
        }

        // Right Weight
        public double RightWeight
        {
            get
            {
                return (double)GetSetterValue(LabelCell.RightWeightProperty);
            }
            set
            {
                SetSetterValue(LabelCell.RightWeightProperty, value);
            }
        }

        // Bottom Weight
        public double BottomWeight
        {
            get
            {
                return (double)GetSetterValue(LabelCell.BottomWeightProperty);
            }
            set
            {
                SetSetterValue(LabelCell.BottomWeightProperty, value);
            }
        }
    }

    public class CellRowTemplate : LabelStripTemplateBase
    {
        public CellRowTemplate()
        {
            TargetType = typeof(CellRow);
        }

        public CellRowTemplate(CellRowTemplate basedOn)
        {
            TargetType = typeof(CellRow);
            BasedOn = basedOn;
        }

        // DataField.
        public LabelField DataField
        {
            get
            {
                return (LabelField)GetSetterValue(CellRow.DataFieldProperty);
            }
            set
            {
                SetSetterValue(CellRow.DataFieldProperty, value);
            }
        }

        // Font.
        public Typeface Font
        {
            get
            {
                return (Typeface)GetSetterValue(CellRow.FontProperty);
            }
            set
            {
                SetSetterValue(CellRow.FontProperty, value);
            }
        }

        // Desired Font Size.
        public double DesiredFontSize
        {
            get
            {
                return (double)GetSetterValue(CellRow.DesiredFontSizeProperty);
            }
            set
            {
                SetSetterValue(CellRow.DesiredFontSizeProperty, value);
            }
        }
    }
}
