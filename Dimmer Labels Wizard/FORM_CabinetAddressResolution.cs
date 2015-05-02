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
            foreach (var element in Globals.ResolvedCabinetRacks)
            {
                if (element.rack_unit_type == RackType.Distro)
                {
                    ResolvedDataGrid.Rows.Add(element.channel_number, "N" + element.dimmer_number, element.cabinet_number, element.rack_number);
                }
                
                else if (element.rack_unit_type == RackType.Dimmer)
                {
                    ResolvedDataGrid.Rows.Add(element.channel_number, element.universe_number + "/" + element.dimmer_number, element.cabinet_number, element.rack_number);
                }

                else
                {
                    ResolvedDataGrid.Rows.Add(element.channel_number, "?" + element.dimmer_number, element.cabinet_number, element.rack_number);
                }
            }
            ResolvedCabinetsCount.Text = Globals.ResolvedCabinetRacks.Count.ToString();
        }

        private void PopulateUnresolvedDataGrid()
        {
            foreach (var element in Globals.UnresolvedCabinetRacks)
            {
                if (element.rack_unit_type == RackType.Distro)
                {
                    UnresolvedDataGrid.Rows.Add(element.channel_number, "N" + element.dimmer_number, element.cabinet_number, element.rack_number);
                }

                else if (element.rack_unit_type == RackType.Dimmer)
                {
                    UnresolvedDataGrid.Rows.Add(element.channel_number, element.universe_number + "/" + element.dimmer_number, element.cabinet_number, element.rack_number);
                }

                else
                {
                    UnresolvedDataGrid.Rows.Add(element.channel_number, "?" + element.dimmer_number, element.cabinet_number, element.rack_number);
                }
            }
            UnresolvedCabinetsCount.Text = Globals.UnresolvedCabinetRacks.Count.ToString();
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {          
        }
    }
}
