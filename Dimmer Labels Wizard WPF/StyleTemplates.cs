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
using System.Runtime.Serialization;

namespace Dimmer_Labels_Wizard_WPF
{
    [DataContract]
    public class LabelStripTemplate : INotifyPropertyChanged
    {
        #region Constructors

        #endregion

        #region Properties.
        // Database and Navigation Properties.
        public  int ID { get; set; }
        public virtual ICollection<Strip> Strip { get; set; }

        [DataMember]
        public bool IsBuiltIn { get; set; } = false;

        protected string _Name = "No Name";
        [DataMember]
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
        // StripSpacers.
        protected List<StripSpacer> _StripSpacers = new List<StripSpacer>();
        [DataMember]
        public virtual List<StripSpacer> StripSpacers
        {
            get
            {
                return _StripSpacers;
            }
            set
            {
                if (StripSpacers != value)
                {
                    _StripSpacers = value;
                }
            }
        }

        // StripWidth
        protected double _StripWidth = 70d * 12;
        [DataMember]
        public  double StripWidth
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
        [DataMember]
        public virtual LabelCellTemplate UpperCellTemplate
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
        [DataMember]
        public virtual LabelCellTemplate LowerCellTemplate
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
        [DataMember]
        public  double StripHeight
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
        [DataMember]
        public  LabelStripMode StripMode
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

        [NotMapped]
        public Style Style
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

        protected Style GetStyle()
        {
            var style = new Style(typeof(LabelStrip));
            var setters = style.Setters;

            setters.Add(new Setter(LabelStrip.StripWidthProperty, StripWidth));
            setters.Add(new Setter(LabelStrip.UpperCellTemplateProperty, UpperCellTemplate.Clone()));
            setters.Add(new Setter(LabelStrip.LowerCellTemplateProperty, LowerCellTemplate.Clone()));
            setters.Add(new Setter(LabelStrip.StripHeightProperty, StripHeight));
            setters.Add(new Setter(LabelStrip.StripModeProperty, StripMode));
            setters.Add(new Setter(LabelStrip.StripSpacersProperty, StripSpacers));

            return style;
        }

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

    [DataContract]
    public class LabelCellTemplate : ViewModelBase, ICloneable
    {
        // Database and Navigation Properties.
        public int ID { get; set; }
        public virtual LabelStripTemplate LabelStripTemplate { get; set; }
        public virtual ICollection<Strip> Strip { get; set; }
        
        // Test.
        public string EFTest { get; set; }


        // Unique Cell Template Properties.
        [DataMember]
        public virtual bool IsUniqueTemplate { get; set; } = false;

        protected string _UniqueCellName = string.Empty;
        [DataMember]
        public virtual string UniqueCellName
        {
            get { return _UniqueCellName; }
            set
            {
                if (_UniqueCellName != value)
                {
                    _UniqueCellName = value;

                    // Notify.
                    OnPropertyChanged(nameof(UniqueCellName));
                }
            }
        }

        [DataMember]
        public virtual List<StripAddress> StripAddresses { get; set; } = new List<StripAddress>();
        [NotMapped]
        public virtual bool IsSpecialSelectionHandlingTemplate { get; set; } = false;
        

        // Cell Row Templates
        protected List<CellRowTemplate> _CellRowTemplates = new List<CellRowTemplate>();
        [DataMember]
        public virtual List<CellRowTemplate> CellRowTemplates
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
        public  double SingleFieldDesiredFontSize
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
        [DataMember]
        public  LabelField SingleFieldDataField
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
        [DataMember]
        public  CellDataMode CellDataMode
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
        [DataMember]
        public  double LeftWeight
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
        [DataMember]
        public  double TopWeight
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
        [DataMember]
        public  double RightWeight
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
        [DataMember]
        public  double BottomWeight
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

        [NotMapped]
        public Style Style
        {
            get
            {
                return GetStyle();
            }
        }

        protected Style GetStyle()
        {
            var style = new Style(typeof(LabelCell));
            var setters = style.Setters;

            setters.Add(new Setter(LabelCell.CellDataModeProperty, CellDataMode));
            setters.Add(new Setter(LabelCell.RowTemplatesProperty, CellRowTemplates));
            setters.Add(new Setter(LabelCell.RowHeightModeProperty, RowHeightMode));
            setters.Add(new Setter(LabelCell.SingleFieldFontProperty, SingleFieldFont));
            setters.Add(new Setter(LabelCell.SingleFieldDesiredFontSizeProperty, SingleFieldDesiredFontSize));
            setters.Add(new Setter(LabelCell.SingleFieldDataFieldProperty, SingleFieldDataField));
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
                StripAddresses = new List<StripAddress>(StripAddresses),
                UniqueCellName = UniqueCellName,
                IsUniqueTemplate = IsUniqueTemplate,
            };
        }
    }

    public class LabelCellTemplateWrapper
    {
        public LabelCellTemplate CellTemplate { get; set; }
        public List<StripAddress> FilteredStripAddresses { get; set; } = new List<StripAddress>();
    }

    [DataContract]
    public class CellRowTemplate
    {
        public CellRowTemplate()
        {

        }

        // Database and Navigation Properties.
        public int ID { get; set; }
        public virtual LabelCellTemplate LabelCellTemplate { get; set; }



        // ManualRowHeight.
        protected double _ManualRowHeight = 1d;
        [DataMember]
        public  double ManualRowHeight
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
        [DataMember]
        public  LabelField DataField
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
        [DataMember]
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
        [DataMember]
        public  double DesiredFontSize
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

        [NotMapped]
        public Style Style
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

        protected Style GetStyle()
        {
            var style = new Style(typeof(CellRow));
            var setters = style.Setters;


            // Can't find a Suitable DP for ManualRowHeight Property.
            
            setters.Add(new Setter(CellRow.DataFieldProperty, DataField));
            setters.Add(new Setter(CellRow.FontProperty, Font));
            setters.Add(new Setter(CellRow.DesiredFontSizeProperty, DesiredFontSize));

            return style;

        }
    }
}
