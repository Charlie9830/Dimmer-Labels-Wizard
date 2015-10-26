using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Dimmer_Labels_Wizard_WPF
{
    public class DrawingTestViewModel : ViewModelBase
    {
        public DrawingTestViewModel(LabelCell labelCell)
        {
            LabelCell = labelCell;

            var betterTypefaces = new List<BetterTypeface>();
            foreach (var element in Fonts.SystemTypefaces)
            {
                betterTypefaces.Add(new BetterTypeface(element));
            }

            _SystemTypefaces = betterTypefaces.ToArray();
        }

        protected BetterTypeface[] _SystemTypefaces;
        public BetterTypeface[] SystemTypefaces
        {
            get
            {
                return _SystemTypefaces;
            }
        }

        protected bool _SingleFieldModeActive = false;
        public bool SingleFieldModeActive
        {
            get
            {
                return _SingleFieldModeActive;
            }
            set
            {
                if (value == true)
                {
                    LabelCell.CellDataMode = CellDataMode.SingleField;
                }
                else
                {
                    LabelCell.CellDataMode = CellDataMode.MixedField;
                }

                _SingleFieldModeActive = value;
                OnPropertyChanged("SingleFieldModeActive");
            }
        }

        public BetterTypeface SelectedTypeface
        {
            get
            {
                if (_SelectedRow != null)
                {
                    return new BetterTypeface(_SelectedRow.Font);
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (_SelectedRow != null)
                {
                    SelectedRow.Font = value;
                    OnPropertyChanged("SelectedTypeface");
                }
            }
        }



        protected LabelCell _LabelCell;
        public LabelCell LabelCell
        {
            get { return _LabelCell; }
            set { _LabelCell = value; }
        }

        protected CellRow _SelectedRow = null;
        public CellRow SelectedRow
        {
            get
            {
                return _SelectedRow;
            }
            set
            {
                _SelectedRow = value;
                OnPropertyChanged("SelectedRow");
                OnPropertyChanged("CellData");
                OnPropertyChanged("SelectedRowDataField");
                OnPropertyChanged("FontSize");
                OnPropertyChanged("SelectedRowHeightMode");
                OnPropertyChanged("RowHeight");
            }
        }

        public LabelField SelectedRowDataField
        {
            get
            {
                if (_SelectedRow != null)
                {
                    return _SelectedRow.DataField;
                }
                else
                {
                    return LabelField.NoAssignment;
                }
            }
            set
            {
                if (_SelectedRow != null)
                {
                    if (value != _SelectedRow.DataField)
                    {
                        _SelectedRow.DataField = value;
                        OnPropertyChanged("CellData");
                    }
                }
            }
        }

        public string CellData
        {
            get
            {
                if (LabelCell.CellDataMode == CellDataMode.MixedField)
                {
                    if (_SelectedRow != null)
                    {
                        return _SelectedRow.Data;
                    }
                    else
                    {
                        return "***";
                    }
                }
                else
                {
                    return LabelCell.SFcellData;
                }
            }
            set
            {
                if (LabelCell.CellDataMode == CellDataMode.MixedField)
                {
                    if (_SelectedRow != null)
                    {
                        if (value != _SelectedRow.Data)
                        {
                            _SelectedRow.Data = value;
                            OnPropertyChanged("CellData");
                            OnPropertyChanged("FontSize");
                        }
                    }
                }
                else
                {
                    if (value != LabelCell.SFcellData)
                    {
                        LabelCell.SFcellData = value;
                        OnPropertyChanged("CellData");
                        OnPropertyChanged("FontSize");
                    }
                }
            }
        }

        public double FontSize
        {
            get
            {
                if (LabelCell.CellDataMode == CellDataMode.MixedField)
                {
                    if (_SelectedRow != null)
                    {
                        return _SelectedRow.FontSize;
                    }
                    else
                    {
                        return 0d;
                    }
                }

                else
                {
                    return LabelCell.SFfontSize;
                }
            }
            set
            {
                if (LabelCell.CellDataMode == CellDataMode.MixedField)
                {
                    if (_SelectedRow != null)
                    {
                        if (value != _SelectedRow.FontSize)
                        {
                            _SelectedRow.FontSize = value;
                            OnPropertyChanged("FontSize");
                        }
                    }
                }

                else
                {
                    if (value != LabelCell.SFfontSize)
                    LabelCell.SFfontSize = value;
                    OnPropertyChanged("FontSize");
                }
            }
        }

        public CellRowHeightMode SelectedRowHeightMode
        {
            get
            {
                return LabelCell.RowHeightMode;
            }

            set
            {

                if (value != LabelCell.RowHeightMode)
                {
                    LabelCell.RowHeightMode = value;
                    OnPropertyChanged("CellData");
                    OnPropertyChanged("SelectedRowHeightMode");
                }
                
            }
        }

        public double RowHeight
        {
            get
            {
                if (_SelectedRow != null)
                {
                    return _SelectedRow.RowHeight;
                }
                else
                {
                    return 69d;
                }
            }
            set
            {
                if (_SelectedRow != null)
                {
                    if (value != _SelectedRow.RowHeight)
                    {
                        _SelectedRow.RowHeight = value;
                        OnPropertyChanged("RowHeight");
                    }
                }
            }
        }
    }

    public class BetterTypeface : Typeface
    {
        public BetterTypeface(Typeface crapTypeface)
            : base(crapTypeface.FontFamily, crapTypeface.Style, crapTypeface.Weight, crapTypeface.Stretch)
        {
            
        }

        public override string ToString()
        {
            return FontFamily.ToString();
        }
    }
}
