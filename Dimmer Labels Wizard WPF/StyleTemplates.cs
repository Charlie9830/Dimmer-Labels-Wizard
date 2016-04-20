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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dimmer_Labels_Wizard_WPF
{
    public abstract class LabelStripTemplateBase
    {
        public virtual Style Style { get; }

        protected abstract Style GetStyle();
    }

    public class LabelStripTemplate : LabelStripTemplateBase, INotifyPropertyChanged, ICloneable
    {
        #region Fields
        public List<Strip> AssignedToStrips = new List<Strip>();

        #endregion

        #region Properties.

        // Database.
        public int ID { get; set; }

        public bool IsBuiltIn { get; set; } = false;

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
        protected double _StripWidth = 70d * 12;
        public double StripWidth
        {
            get
            {
                return _StripWidth;
            }
            set
            {
                if (_StripWidth != value)
                {
                    _StripWidth = value;
                }
            }
        }

        // Upper Cells Template
        protected LabelCellTemplate _UpperCellTemplate = new LabelCellTemplate();
        public LabelCellTemplate UpperCellTemplate
        {
            get
            {
                return _UpperCellTemplate;
            }
            set
            {
                if (_UpperCellTemplate != value)
                {
                    _UpperCellTemplate = value;
                }
            }
        }

        protected LabelCellTemplate _LowerCellTemplate = new LabelCellTemplate();
        public LabelCellTemplate LowerCellTemplate
        {
            get
            {
                return _LowerCellTemplate;
            }
            set
            {
                if (_LowerCellTemplate != value)
                {
                    _LowerCellTemplate = value;
                }
            }
        }

        // Strip Height
        protected double _StripHeight = 70d;
        public double StripHeight
        {
            get
            {
                return _StripHeight;
            }
            set
            {
                if (_StripHeight != value)
                {
                    _StripHeight = value;
                }
            }
        }

        // Strip Mode.
        protected LabelStripMode _StripMode = LabelStripMode.Dual;
        public LabelStripMode StripMode
        {
            get
            {
                return _StripMode;
            }
            set
            {
                if (_StripMode != value)
                {
                    _StripMode = value;
                }
            }
        }
        #endregion

        #region Overrides.
        [NotMapped]
        public override Style Style
        {
            get
            {
                return GetStyle();
            }
        }

        public override string ToString()
        {
            return Name;
        }

        protected override Style GetStyle()
        {
            var style = new Style(typeof(LabelStrip));
            var setters = style.Setters;

            setters.Add(new Setter(LabelStrip.StripWidthProperty, StripWidth));
            setters.Add(new Setter(LabelStrip.UpperCellTemplateProperty, UpperCellTemplate.Clone()));
            setters.Add(new Setter(LabelStrip.LowerCellTemplateProperty, LowerCellTemplate.Clone()));
            setters.Add(new Setter(LabelStrip.StripHeightProperty, StripHeight));
            setters.Add(new Setter(LabelStrip.StripModeProperty, StripMode));

            return style;

            
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

        public object Clone()
        {
            return new LabelStripTemplate()
            {
                StripHeight = StripHeight,
                StripWidth = StripWidth,
                UpperCellTemplate = UpperCellTemplate,
                LowerCellTemplate = LowerCellTemplate,
                StripMode = StripMode,
                Name = Name,
            };
        }

        #endregion
    }

    public class LabelCellTemplate : LabelStripTemplateBase, ICloneable
    {
       
        #region Fields

        public bool IsUniqueTemplate = false;
        public int UniqueCellIndex = -1;

        #endregion

        public int ID { get; set; }
        public virtual LabelStripTemplate LabelStripTemplate { get; set; }
        public virtual Strip Strip { get; set; }

        // Cell Row Templates
        protected ICollection<CellRowTemplate> _CellRowTemplates = new List<CellRowTemplate>();
        public ICollection<CellRowTemplate> CellRowTemplates
        {
            get
            {
                return _CellRowTemplates;
            }
            set
            {
                if (_CellRowTemplates != value)
                {
                    _CellRowTemplates = value;
                }
            }
        }

        // Row Height Mode.
        protected CellRowHeightMode _RowHeightMode = CellRowHeightMode.Static;
        public CellRowHeightMode RowHeightMode
        {
            get
            {
                return _RowHeightMode;
            }
            set
            {
                if (_RowHeightMode != value)
                {
                    _RowHeightMode = value;
                }
            }
        }

        // SingleField Font
        public SerializableFont SingleFieldSerializableFont { get; set; } = new SerializableFont("Arial");
        
        [NotMapped]
        public Typeface SingleFieldFont
        {
            get
            {
                return SingleFieldSerializableFont.Typeface;
            }
            set
            {
                if (SingleFieldSerializableFont.Typeface != value)
                {
                    SingleFieldSerializableFont.Typeface = value;
                }
            }
        }

        // SingleFieldDesiredFontSize
        protected double _SingleFieldDesiredFontSize = 12d;
        public double SingleFieldDesiredFontSize
        {
            get
            {
                return _SingleFieldDesiredFontSize;
            }
            set
            {
                if (_SingleFieldDesiredFontSize != value)
                {
                    _SingleFieldDesiredFontSize = value;
                }
            }
        }

        // SingleFieldDataField
        protected LabelField _SingleFieldDataField = LabelField.NoAssignment;
        public LabelField SingleFieldDataField
        {
            get
            {
                return _SingleFieldDataField;
            }
            set
            {
                if (_SingleFieldDataField != value)
                {
                    _SingleFieldDataField = value;
                }
            }
        }

        // Cell Data Mode.
        protected CellDataMode _CellDataMode = CellDataMode.MultiField;
        public CellDataMode CellDataMode
        {
            get
            {
                return _CellDataMode;
            }
            set
            {
                if (_CellDataMode != value)
                {
                    _CellDataMode = value;
                }
            }
        }

        // Left Weight
        protected double _LeftWeight = 1d;
        public double LeftWeight
        {
            get
            {
                return _LeftWeight;
            }
            set
            {
                if (_LeftWeight != value)
                {
                    _LeftWeight = value;
                }
            }
        }

        // Top Weight
        protected double _TopWeight = 1d;
        public double TopWeight
        {
            get
            {
                return _TopWeight;
            }
            set
            {
                if (_TopWeight != value)
                {
                    _TopWeight = value;
                }
            }
        }

        // Right Weight
        protected double _RightWeight = 1d;
        public double RightWeight
        {
            get
            {
                return _RightWeight;
            }
            set
            {
                if (_RightWeight != value)
                {
                    _RightWeight = value;
                }
            }
        }

        // Bottom Weight
        protected double _BottomWeight = 1d;
        public double BottomWeight
        {
            get
            {
                return _BottomWeight;
            }
            set
            {
                if (_BottomWeight != value)
                {
                    _BottomWeight = value;
                }
            }
        }

        #region Overrides.
        [NotMapped]
        public override Style Style
        {
            get
            {
                return GetStyle();
            }
        }

        protected override Style GetStyle()
        {
            var style = new Style(typeof(LabelCell));
            var setters = style.Setters;

            setters.Add(new Setter(LabelCell.RowTemplatesProperty, CellRowTemplates));
            setters.Add(new Setter(LabelCell.RowHeightModeProperty, RowHeightMode));
            setters.Add(new Setter(LabelCell.SingleFieldFontProperty, SingleFieldFont));
            setters.Add(new Setter(LabelCell.SingleFieldDesiredFontSizeProperty, SingleFieldDesiredFontSize));
            setters.Add(new Setter(LabelCell.SingleFieldDataFieldProperty, SingleFieldDataField));
            setters.Add(new Setter(LabelCell.CellDataModeProperty, CellDataMode));
            setters.Add(new Setter(LabelCell.LeftWeightProperty, LeftWeight));
            setters.Add(new Setter(LabelCell.TopWeightProperty, TopWeight));
            setters.Add(new Setter(LabelCell.RightWeightProperty, RightWeight));
            setters.Add(new Setter(LabelCell.BottomWeightProperty, BottomWeight));

            return style;
        }

        public object Clone()
        {
            return new LabelCellTemplate()
            {
                LeftWeight = LeftWeight,
                TopWeight = TopWeight,
                RightWeight = RightWeight,
                BottomWeight = BottomWeight,
                SingleFieldDataField = SingleFieldDataField,
                SingleFieldDesiredFontSize = SingleFieldDesiredFontSize,
                SingleFieldFont = SingleFieldFont,
                CellDataMode = CellDataMode,
                RowHeightMode = RowHeightMode,
                CellRowTemplates = new List<CellRowTemplate>(CellRowTemplates),
            };
        }
        #endregion
    }

    public class CellRowTemplate : LabelStripTemplateBase, ICloneable
    {
        public CellRowTemplate()
        {

        }

        public CellRowTemplate(CellRowTemplate cellRowTemplate)
        {
            ManualRowHeight = cellRowTemplate.ManualRowHeight;
            DataField = cellRowTemplate.DataField;
            
        }

        public int ID { get; set; }
        public virtual LabelCellTemplate LabelCellTemplate { get; set; }

        // ManualRowHeight.
        protected double _ManualRowHeight = 1d;
        public double ManualRowHeight
        {
            get
            {
                return _ManualRowHeight;
            }
            set
            {
                if (_ManualRowHeight != value)
                {
                    _ManualRowHeight = value;
                }
            }
        }

        // DataField.
        protected LabelField _DataField = LabelField.NoAssignment;
        public LabelField DataField
        {
            get
            {
                return _DataField;
            }
            set
            {
                if (_DataField != value)
                {
                    _DataField = value;
                }
            }
        }

        // Font.
        public SerializableFont SerializableFont { get; set; } = new SerializableFont("Arial");
        [NotMapped]
        public Typeface Font
        {
            get
            {
                return SerializableFont.Typeface;
            }
            set
            {
                if (SerializableFont.Typeface != value)
                {
                    SerializableFont.Typeface = value;
                }
            }
        }

        // Desired Font Size.
        protected double _DesiredFontSize = 12d;
        public double DesiredFontSize
        {
            get
            {
                return _DesiredFontSize;
            }
            set
            {
                if (_DesiredFontSize != value)
                {
                    _DesiredFontSize = value;
                }
            }
        }

        #region Overrides
        [NotMapped]
        public override Style Style
        {
            get
            {
                return GetStyle();
            }
        }

        public override string ToString()
        {
            return DataField.ToString();
        }

        protected override Style GetStyle()
        {
            var style = new Style(typeof(CellRow));
            var setters = style.Setters;


            // Can't find a Suitable DP for ManualRowHeight Property.
            
            setters.Add(new Setter(CellRow.DataFieldProperty, DataField));
            setters.Add(new Setter(CellRow.FontProperty, Font));
            setters.Add(new Setter(CellRow.DesiredFontSizeProperty, DesiredFontSize));

            return style;

        }

        public object Clone()
        {
            return new CellRowTemplate()
            {
                DataField = DataField,
                DesiredFontSize = DesiredFontSize,
                ManualRowHeight = ManualRowHeight,
                Font = Font
            };
        }
        #endregion
    }
}
