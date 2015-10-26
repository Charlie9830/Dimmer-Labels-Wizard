using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dimmer_Labels_Wizard_WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF.Tests
{
    [TestClass()]
    public class LabelCellTests
    {
        [TestMethod()]
        public void GetCascadedRowsTest()
        {
            // Arrange
            LabelCell testCell = new LabelCell();

            List<CellRow> rows = new List<CellRow>();
            rows.Add(new CellRow(testCell, LabelField.ChannelNumber) {Index = 0 });
            rows.Add(new CellRow(testCell, LabelField.ChannelNumber) { Index = 1 });
            rows.Add(new CellRow(testCell, LabelField.InstrumentName) { Index = 2 });
            rows.Add(new CellRow(testCell, LabelField.InstrumentName) { Index = 3 });
            var testResult = testCell.GetCascadedRows(rows);


            Assert.Fail();
        }
    }
}