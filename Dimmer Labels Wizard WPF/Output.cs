using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media;

namespace Dimmer_Labels_Wizard_WPF
{
    public static class Output
    {
        public static void ExportToRackLabel()
        {
            int outputIndex = 0;

            // Clear the List if it has been previously Populated.
            if (Globals.LabelStrips.Count > 0)
            {
                Globals.LabelStrips.Clear();
            }

            // Process DimmerDistroUnits into LabelStrip objects and Add to Global List of LabelStrip Objects
            for (int inputIndex = 0; inputIndex < Globals.DimmerDistroUnits.Count;)
            {
                int rackSize = NoOfCells(inputIndex);
                Globals.LabelStrips.Insert(outputIndex, new LabelStrip());

                // Set Lineweight.
                Globals.LabelStrips[outputIndex].LineWeight = 1d;

                for (int j = 0; j < rackSize; j++)
                {
                    // Create the Header Object.
                    Globals.LabelStrips[outputIndex].Headers.Insert(j, new HeaderCell());

                    // Assign the rackUnitType and Rack Numbers
                    Globals.LabelStrips[outputIndex].RackUnitType = Globals.DimmerDistroUnits[inputIndex + j].RackUnitType;
                    Globals.LabelStrips[outputIndex].RackNumber = Globals.DimmerDistroUnits[inputIndex + j].RackNumber;

                    // Assign Label Width.
                    if (Globals.LabelStrips[outputIndex].RackUnitType == RackType.Dimmer)
                    {
                        Globals.LabelStrips[outputIndex].LabelWidthInMM = UserParameters.DimmerLabelWidthInMM;
                        Globals.LabelStrips[outputIndex].LabelHeightInMM = UserParameters.DimmerLabelHeightInMM;
                    }

                    if (Globals.LabelStrips[outputIndex].RackUnitType == RackType.Distro)
                    {
                        Globals.LabelStrips[outputIndex].LabelWidthInMM = UserParameters.DistroLabelWidthInMM;
                        Globals.LabelStrips[outputIndex].LabelHeightInMM = UserParameters.DistroLabelHeightInMM;
                    }

                    // Assign the Header Data.
                    #region Header Data Switch
                    switch (UserParameters.HeaderField)
                    {
                        case LabelField.ChannelNumber:
                            Globals.LabelStrips[outputIndex].Headers[j].Data =
                                Globals.DimmerDistroUnits[inputIndex + j].ChannelNumber;
                            break;
                        case LabelField.InstrumentName:
                            Globals.LabelStrips[outputIndex].Headers[j].Data =
                                Globals.DimmerDistroUnits[inputIndex + j].InstrumentName;
                            break;
                        case LabelField.MulticoreName:
                            Globals.LabelStrips[outputIndex].Headers[j].Data = 
                                Globals.DimmerDistroUnits[inputIndex + j].MulticoreName;
                            break;
                        case LabelField.Position:
                            Globals.LabelStrips[outputIndex].Headers[j].Data =
                                Globals.DimmerDistroUnits[inputIndex + j].Position;
                            break;
                        case LabelField.UserField1:
                            Globals.LabelStrips[outputIndex].Headers[j].Data =
                                Globals.DimmerDistroUnits[inputIndex + j].UserField1;
                            break;
                        case LabelField.UserField2:
                            Globals.LabelStrips[outputIndex].Headers[j].Data =
                                Globals.DimmerDistroUnits[inputIndex + j].UserField2;
                            break;
                        case LabelField.UserField3:
                            Globals.LabelStrips[outputIndex].Headers[j].Data =
                                Globals.DimmerDistroUnits[inputIndex + j].UserField3;
                            break;
                        case LabelField.UserField4:
                            Globals.LabelStrips[outputIndex].Headers[j].Data =
                                Globals.DimmerDistroUnits[inputIndex + j].UserField4;
                            break;
                        case LabelField.NoAssignment:
                            Globals.LabelStrips[outputIndex].Headers[j].Data = null;
                            break;
                        default:
                            Globals.LabelStrips[outputIndex].Headers[j].Data = "Default Switch Case";
                            break;
                    }
                    #endregion Header Data Switch

                    // Assign Default Fonts.
                    Globals.LabelStrips[outputIndex].Headers[j].Font = new Typeface("Arial");
                    Globals.LabelStrips[outputIndex].Headers[j].FontSize = 16d;

                    // Assign a Reference to DimmerDistroUnit
                    Globals.LabelStrips[outputIndex].Headers[j].PreviousReference = Globals.DimmerDistroUnits[inputIndex + j];

                    // Set Background Color.
                    Globals.LabelStrips[outputIndex].Headers[j].BackgroundBrush =
                        Globals.GetLabelColor(Globals.LabelStrips[outputIndex].Headers[j].PreviousReference);

                    // Create the Footer Cell Object.
                    Globals.LabelStrips[outputIndex].Footers.Insert(j, new FooterCell());

                    // Assign the Footer Data.
                    #region Footer Top Data Switch.
                    switch (UserParameters.FooterTopField)
                    {
                        case LabelField.ChannelNumber:
                            Globals.LabelStrips[outputIndex].Footers[j].TopData =
                               Globals.DimmerDistroUnits[inputIndex + j].ChannelNumber;
                            break;
                        case LabelField.InstrumentName:
                            Globals.LabelStrips[outputIndex].Footers[j].TopData =
                               Globals.DimmerDistroUnits[inputIndex + j].InstrumentName;
                            break;
                        case LabelField.MulticoreName:
                            Globals.LabelStrips[outputIndex].Footers[j].TopData =
                               Globals.DimmerDistroUnits[inputIndex + j].MulticoreName;
                            break;
                        case LabelField.Position:
                            Globals.LabelStrips[outputIndex].Footers[j].TopData = 
                               Globals.DimmerDistroUnits[inputIndex + j].Position;
                            break;
                        case LabelField.UserField1:
                            Globals.LabelStrips[outputIndex].Footers[j].TopData =
                               Globals.DimmerDistroUnits[inputIndex + j].UserField1;
                            break;
                        case LabelField.UserField2:
                            Globals.LabelStrips[outputIndex].Footers[j].TopData =
                               Globals.DimmerDistroUnits[inputIndex + j].UserField2;
                            break;
                        case LabelField.UserField3:
                            Globals.LabelStrips[outputIndex].Footers[j].TopData =
                               Globals.DimmerDistroUnits[inputIndex + j].UserField3;
                            break;
                        case LabelField.UserField4:
                            Globals.LabelStrips[outputIndex].Footers[j].TopData =
                               Globals.DimmerDistroUnits[inputIndex + j].UserField4;
                            break;
                        case LabelField.NoAssignment:
                            Globals.LabelStrips[outputIndex].Footers[j].TopData = null;
                            break;
                        default:
                            Globals.LabelStrips[outputIndex].Footers[j].TopData = "Defualt Switch Case";
                            break;
                    }
                    #endregion

                    #region Footer Middle Data Switch
                    switch (UserParameters.FooterMiddleField)
                    {
                        case LabelField.ChannelNumber:
                            Globals.LabelStrips[outputIndex].Footers[j].MiddleData =
                                Globals.DimmerDistroUnits[inputIndex + j].ChannelNumber;
                            break;
                        case LabelField.InstrumentName:
                            Globals.LabelStrips[outputIndex].Footers[j].MiddleData =
                                Globals.DimmerDistroUnits[inputIndex + j].InstrumentName;
                            break;
                        case LabelField.MulticoreName:
                            Globals.LabelStrips[outputIndex].Footers[j].MiddleData =
                                Globals.DimmerDistroUnits[inputIndex + j].MulticoreName;
                            break;
                        case LabelField.Position:
                            Globals.LabelStrips[outputIndex].Footers[j].MiddleData =
                                Globals.DimmerDistroUnits[inputIndex + j].Position;
                            break;
                        case LabelField.UserField1:
                            Globals.LabelStrips[outputIndex].Footers[j].MiddleData =
                                Globals.DimmerDistroUnits[inputIndex + j].UserField1;
                            break;
                        case LabelField.UserField2:
                            Globals.LabelStrips[outputIndex].Footers[j].MiddleData =
                                Globals.DimmerDistroUnits[inputIndex + j].UserField2;
                            break;
                        case LabelField.UserField3:
                            Globals.LabelStrips[outputIndex].Footers[j].MiddleData =
                                Globals.DimmerDistroUnits[inputIndex + j].UserField3;
                            break;
                        case LabelField.UserField4:
                            Globals.LabelStrips[outputIndex].Footers[j].MiddleData =
                                Globals.DimmerDistroUnits[inputIndex + j].UserField4;
                            break;
                        case LabelField.NoAssignment:
                            Globals.LabelStrips[outputIndex].Footers[j].MiddleData = null;
                            break;
                        default:
                            Globals.LabelStrips[outputIndex].Footers[j].MiddleData = "Defualt Switch Case";
                            break;
                    }
                    #endregion

                    #region Footer Bottom Data Switch
                    switch (UserParameters.FooterBottomField)
                    {
                        case LabelField.ChannelNumber:
                            Globals.LabelStrips[outputIndex].Footers[j].BottomData =
                                Globals.DimmerDistroUnits[inputIndex + j].ChannelNumber;
                            break;
                        case LabelField.InstrumentName:
                            Globals.LabelStrips[outputIndex].Footers[j].BottomData = 
                                Globals.DimmerDistroUnits[inputIndex + j].InstrumentName;
                            break;
                        case LabelField.MulticoreName:
                            Globals.LabelStrips[outputIndex].Footers[j].BottomData =
                                Globals.DimmerDistroUnits[inputIndex + j].MulticoreName;
                            break;
                        case LabelField.Position:
                            Globals.LabelStrips[outputIndex].Footers[j].BottomData =
                                Globals.DimmerDistroUnits[inputIndex + j].Position;
                            break;
                        case LabelField.UserField1:
                            Globals.LabelStrips[outputIndex].Footers[j].BottomData =
                                Globals.DimmerDistroUnits[inputIndex + j].UserField1;
                            break;
                        case LabelField.UserField2:
                            Globals.LabelStrips[outputIndex].Footers[j].BottomData =
                                Globals.DimmerDistroUnits[inputIndex + j].UserField2;
                            break;
                        case LabelField.UserField3:
                            Globals.LabelStrips[outputIndex].Footers[j].BottomData =
                                Globals.DimmerDistroUnits[inputIndex + j].UserField3;
                            break;
                        case LabelField.UserField4:
                            Globals.LabelStrips[outputIndex].Footers[j].BottomData =
                                Globals.DimmerDistroUnits[inputIndex + j].UserField4;
                            break;
                        case LabelField.NoAssignment:
                            Globals.LabelStrips[outputIndex].Footers[j].BottomData = null;
                            break;
                        default:
                            Globals.LabelStrips[outputIndex].Footers[j].BottomData = "Default Switch Case";
                            break;
                    }
                    #endregion

                    // Set Default Fonts.
                    Globals.LabelStrips[outputIndex].Footers[j].TopFont = new Typeface("Arial");
                    Globals.LabelStrips[outputIndex].Footers[j].TopFontSize = 10d;

                    Globals.LabelStrips[outputIndex].Footers[j].MiddleFont = new Typeface("Arial");
                    Globals.LabelStrips[outputIndex].Footers[j].MiddleFontSize = 12d;

                    Globals.LabelStrips[outputIndex].Footers[j].BottomFont = new Typeface("Arial");
                    Globals.LabelStrips[outputIndex].Footers[j].BottomFontSize = 10d;

                    // Assign a Reference to DimmerDistroUnit
                    Globals.LabelStrips[outputIndex].Footers[j].PreviousReference = Globals.DimmerDistroUnits[inputIndex + j];

                    // Set Background Color
                    Globals.LabelStrips[outputIndex].Footers[j].BackgroundBrush =
                        Globals.GetLabelColor(Globals.LabelStrips[outputIndex].Footers[j].PreviousReference);
                }

                // Itterate Input_index to the beginning of the next Rack.
                inputIndex += rackSize;
                outputIndex++;
            }
        }

        // Returns the Number of DimDistroUnits belonging to a Rack from a Starting_index.
        private static int NoOfCells(int startingIndex)
        {
            DimmerDistroUnit referenceUnit = Globals.DimmerDistroUnits[startingIndex];

            // Searches DimmerDistroUnits for all objects with Rack, Universe and rackUnitType matching the Reference
            // Unit, then return the amount of units found.
            int cellCount = Globals.DimmerDistroUnits.FindAll(item => item.RackNumber == referenceUnit.RackNumber
                && item.UniverseNumber == referenceUnit.UniverseNumber &&
                item.RackUnitType == referenceUnit.RackUnitType).Count;

            return cellCount;
        }
    }
}
