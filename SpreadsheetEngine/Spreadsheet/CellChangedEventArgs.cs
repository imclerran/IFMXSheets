using System;

namespace SpreadsheetEngine
{
    //=========================================================================
    // class:   CellChangedEventArgs
    // purpose: EventArgs to provide event subscribers the coordinates of
    //          the cell that changed
    //=========================================================================
    public class CellChangedEventArgs : EventArgs
    {
        #region properties

        public int RowIndex { get; }
        public int ColumnIndex { get; }

        #endregion

        #region constructor

        public CellChangedEventArgs(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
        }

        #endregion
    }
}
