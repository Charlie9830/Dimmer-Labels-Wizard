using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public class UndoRedoManager
    {
        public UndoRedoManager()
        {
            // Connect to DataModel.
            foreach (var element in Globals.DimmerDistroUnits)
            {
                element.NotifyModification += Incoming_Modification;
            }
        }

        #region Fields.
        // Stacks
        protected Stack<ModificationBase> _UndoStack = new Stack<ModificationBase>();
        protected Stack<ModificationBase> _RedoStack = new Stack<ModificationBase>();

        // Tracking.
        protected bool _IgnoreIncomingModifications = false;
        #endregion

        #region Properties.
        public bool CanUndo
        {
            get
            {
                return _UndoStack.Count > 0;
            }
        }

        public bool CanRedo
        {
            get
            {
                return _RedoStack.Count > 0;
            }
        }
        #endregion

        #region Public Methods.
        public void Undo()
        {
            if (_UndoStack.Count == 0)
            {
                // Nothing to Undo. Bail out.
                return;
            }

            // Pop the Stack.
            ModificationBase modification = _UndoStack.Pop();

            // Determine Modification Type.
            if (modification.GetType() == typeof(DataModification))
            {
                // Data Modification.
                var dataModification = (DataModification)modification;

                // Collect current Value and Push to Redo Stack.
                string currentData = dataModification.Target.GetData(dataModification.Property);

                _RedoStack.Push(new DataModification(dataModification.Target, dataModification.Property, currentData));

                // Execute Data Modification.
                _IgnoreIncomingModifications = true;

                dataModification.Target.SetData(dataModification.Value, dataModification.Property);

                _IgnoreIncomingModifications = false;
            }
        }

        public void Redo()
        {
            if (_RedoStack.Count == 0)
            {
                // Nothing to Redo. Bail out.
                return;
            }

            // Pop the Stack.
            ModificationBase modification = _RedoStack.Pop();

            // Determine Modification Type.
            if (modification.GetType() == typeof(DataModification))
            {
                // Data Modification.
                var dataModification = (DataModification)modification;

                // Collect current Value and Push to Undo Stack.
                string currentData = dataModification.Target.GetData(dataModification.Property);

                _UndoStack.Push(new DataModification(dataModification.Target, dataModification.Property, currentData));

                // Execute Data Modification.
                _IgnoreIncomingModifications = true;

                dataModification.Target.SetData(dataModification.Value, dataModification.Property);

                _IgnoreIncomingModifications = false;
            }
        }
        #endregion

        #region Event Handlers.
        private void Incoming_Modification(object sender, NotifyModificationEventArgs e)
        {
            if (_IgnoreIncomingModifications)
            {
                return;
            }

            if (sender.GetType() == typeof(DimmerDistroUnit))
            {
                // Modification sent from DimmerDistro Unit. Push Modification to Undo Stack.
                _UndoStack.Push(new DataModification((DimmerDistroUnit)e.Target, (string)e.Property, (string)e.OldValue));
            }
        }
        #endregion
    }
}
