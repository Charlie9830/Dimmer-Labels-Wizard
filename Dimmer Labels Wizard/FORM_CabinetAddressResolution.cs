using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dimmer_Labels_Wizard
{
    public partial class FORM_CabinetAddressResolution : Form
    {
        public FORM_CabinetAddressResolution()
        {
            InitializeComponent();
        }

        private void FORM_CabinetAddressResolution_Load(object sender, EventArgs e)
        {
            PopulateResolvedDataGrid();
            PopulateUnresolvedDataGrid();

        }

        private void ResolvedDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            
        }

        private void PopulateResolvedDataGrid()
        {
            foreach (var element in Globals.ResolvedCabinetRackNumbers)
            {
                if (element.RackUnitType == RackType.Distro)
                {
                    ResolvedDataGrid.Rows.Add(element.ChannelNumber, "N" + element.DimmerNumber, element.CabinetNumber, element.RackNumber);
                }
                
                else if (element.RackUnitType == RackType.Dimmer)
                {
                    ResolvedDataGrid.Rows.Add(element.ChannelNumber, element.UniverseNumber + "/" + element.DimmerNumber, element.CabinetNumber, element.RackNumber);
                }

                else
                {
                    ResolvedDataGrid.Rows.Add(element.ChannelNumber, "?" + element.DimmerNumber, element.CabinetNumber, element.RackNumber);
                }
            }
            ResolvedCabinetsCount.Text = Globals.ResolvedCabinetRackNumbers.Count.ToString();
        }

        private void PopulateUnresolvedDataGrid()
        {
            foreach (var element in Globals.UnresolvedCabinetRackNumbers)
            {
                if (element.RackUnitType == RackType.Distro)
                {
                    UnresolvedDataGrid.Rows.Add(element.ChannelNumber, "N" + element.DimmerNumber, element.CabinetNumber, element.RackNumber);
                }

                else if (element.RackUnitType == RackType.Dimmer)
                {
                    UnresolvedDataGrid.Rows.Add(element.ChannelNumber, element.UniverseNumber + "/" + element.DimmerNumber, element.CabinetNumber, element.RackNumber);
                }

                else
                {
                    UnresolvedDataGrid.Rows.Add(element.ChannelNumber, "?" + element.DimmerNumber, element.CabinetNumber, element.RackNumber);
                }
            }
            UnresolvedCabinetsCount.Text = Globals.UnresolvedCabinetRackNumbers.Count.ToString();
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {          
        }
    }
}
