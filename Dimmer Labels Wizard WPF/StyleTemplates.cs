using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

    public class LabelStripTemplate : LabelStripTemplateBase, INotifyPropertyChanged
    {
        public LabelStripTemplate()
        {
            TargetType = typeof(LabelStrip);
            BasedOn = Globals.BaseLabelStripTemplate;
            
        }

        public LabelStripTemplate(LabelStripTemplate basedOn)
        {
            TargetType = typeof(LabelStrip);
            BasedOn = basedOn;
        }

        #region Fields
        public List<Strip> AssignedToStrips = new List<Strip>();
        public bool IsBuiltIn = false;
        #endregion

        #region Binding Sources.

        protected string _Name = "No Name";

        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;

                    // Notify.
                    OnPropertyChanged(nameof(Name));
                }
            }
        }


        protected bool _EditorUpdatesPending = false;

        public bool EditorUpdatesPending
        {
            get { return _EditorUpdatesPending; }
            set
            {
                if (_EditorUpdatesPending != value)
                {
                    _EditorUpdatesPending = value;

                    // Notify.
                    OnPropertyChanged(nameof(EditorUpdatesPending));
                }
            }
        }

        #endregion
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
        public LabelCellTemplate UpperCellTemplate
        {
            get
            {
                return (LabelCellTemplate)GetSetterValue(LabelStrip.UpperCellTemplateProperty);
            }
            set
            {
                SetSetterValue(LabelStrip.UpperCellTemplateProperty, value);
                

            }
        }

        public LabelCellTemplate LowerCellTemplate
        {
            get
            {
                return (LabelCellTemplate)GetSetterValue(LabelStrip.LowerCellTemplateProperty);
            }
            set
            {
                SetSetterValue(LabelStrip.LowerCellTemplateProperty, value);
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

        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    public class LabelCellTemplate : LabelStripTemplateBase
    {
        public LabelCellTemplate()
        {
            TargetType = typeof(LabelCell);

            // Check if we are at Runtime or Designtime before setting the BasedOn Property.
            // Designer will crash because it is not initalizing the Globals.BaseLabelCellTemplate
            // early enough.
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                BasedOn = Globals.BaseLabelCellTemplate;
            }
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

            // Check if we are at Runtime or Designtime before setting the BasedOn Property.
            // Designer will crash because it is not initalizing the Globals.BaseCellRowTemplate
            // early enough.
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                BasedOn = Globals.BaseCellRowTemplate;
            }
            
        }

        public CellRowTemplate(CellRowTemplate basedOn)
        {
            TargetType = typeof(CellRow);
            BasedOn = basedOn;
        }

        // ManualRowHeight.
        public double ManualRowHeight
        {
            get
            {
                return (double)GetSetterValue(CellRow.RowHeightProperty);
            }
            set
            {
                SetSetterValue(CellRow.RowHeightProperty, value);
            }
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

        #region Overrides
        public override string ToString()
        {
            return DataField.ToString();
        }
        #endregion
    }
}
