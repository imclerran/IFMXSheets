using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using CptS321;

namespace SpreadsheetEngine
{
    /// <summary>  
    /// class which handles the spreadsheet logic and manages cells 
    /// </summary>
    public class Spreadsheet
    {
        #region nested classes

        /// <summary>
        /// implementation of Cell class, allows only the spreadsheet to set the cell value
        /// </summary>
        private class SpreadsheetCell : Cell
        {
            public readonly List<SpreadsheetCell> Subscribers;
            public readonly List<SpreadsheetCell> Subscriptions;
            
            public SpreadsheetCell(int rowIndex, int columnIndex) : base(rowIndex, columnIndex) {
                Subscribers = new List<SpreadsheetCell>();
                Subscriptions = new List<SpreadsheetCell>();
            }

            public void setValue(string value) {
                if (_value != value) {
                    _value = value;
                    this.OnPropertyChanged(nameof(Value));
                }
            }
        }

        /// <summary>
        /// implementation of Cell class, provides a public class with the 
        /// necessary getters/setters to enable automatic XML serialization
        /// </summary>
        public class SerializableCell : Cell
        {
            public SerializableCell() : base() {}

            public SerializableCell(int rowIndex, int columnIndex, string text) {
                RowIndex = rowIndex;
                ColumnIndex = columnIndex;
                Text = text;
            }

            public new int RowIndex { get; set; }
            public new int ColumnIndex { get; set; }
        }

        #endregion

        #region enums

        private enum ColumnNames { A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z };

        #endregion
        
        #region fields

        private readonly SpreadsheetCell[,] _cells;

        #endregion

        #region properties

        public int RowCount => _cells.GetUpperBound(0) + 1;
        public int ColumnCount => _cells.GetUpperBound(1) + 1;

        #endregion

        #region constructor

        /// <summary>
        /// initializes a spreadsheet with specified rows/columns
        /// </summary>
        /// <param name="rows">number of rows contained by the Spreadsheet</param>
        /// <param name="columns">number of columns contained by the Spreadsheet</param>
        public Spreadsheet(int rows, int columns) {
            _cells = new SpreadsheetCell[rows, columns];

            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < columns; j++) {
                    // create cells and subscribe to each cell
                    _cells[i, j] = new SpreadsheetCell(i, j);
                    _cells[i, j].PropertyChanged += HandlePropertyChangedEvent;
                }
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// calculate and set the value of a cell based on cell text, and any equations/variable in the text
        /// </summary>
        /// <param name="cell">a cell to be updated</param>
        private void UpdateCellValue(Cell cell) {
            var cellToInterpret = (SpreadsheetCell)cell;

            if (String.IsNullOrEmpty(cellToInterpret.Text)) {
                cellToInterpret.setValue("");
                Unsubscribe(cellToInterpret);
            }
            else if (cellToInterpret.Text.StartsWith("=")) {
                string expression = cellToInterpret.Text.Substring(1);
                cellToInterpret.setValue(InterpretExpression(expression, ref cellToInterpret));
            }
            else { // cell does not contain an equation. use text as value, and unsubscribe from any cells
                cellToInterpret.setValue(cellToInterpret.Text);
                Unsubscribe(cellToInterpret);
            }
        }

        /// <summary>
        /// Interpret the expression contained by a cell, and return its value
        /// </summary>
        /// <param name="expression">the expression to interpret</param>
        /// <param name="cellToInterpret">the cell containing the expression</param>
        /// <returns>the interpreted value</returns>
        private string InterpretExpression(string expression, ref SpreadsheetCell cellToInterpret) {
            var expTree = new ExpTree(expression);
            string value = "";

            var singleCellExpression = (SpreadsheetCell)GetCellFromStr(expression);
            if (null != singleCellExpression && singleCellExpression  != cellToInterpret) {
                if (VerifyNoCycles(cellToInterpret, singleCellExpression)) {
                    value = String.IsNullOrEmpty(singleCellExpression.Value) ? "0" : singleCellExpression.Value;
                    Subscribe(cellToInterpret, singleCellExpression);
                }
                else {
                    value = "#REF!";
                }
            }
            else if (expTree.IsValidExpression()) {
                if (AssignExpTreeVars(ref cellToInterpret, ref expTree))
                    value = expTree.Eval().ToString();
                else {
                    value = "#REF!";
                }
            }
            else {
                value = "#VALUE!";
            }
            return value;
        }

        /// <summary>
        /// assign values to all variables found in the expression tree from the corresponding cells
        /// </summary>
        /// <param name="cellToInterpret">the cell who's value is being updated</param>
        /// <param name="expTree">the expression tree for the cell being updated</param>
        /// <returns>true if all variables assigned, otherwise false</returns>
        private bool AssignExpTreeVars(ref SpreadsheetCell cellToInterpret, ref ExpTree expTree) {
            // create copy of variables found in expression tree
            var tempVars = new Dictionary<string, double>(expTree.Variables);
            foreach (var variable in tempVars) // iterate through variables - find matching cells
            {
                var cellToGetValueFrom = (SpreadsheetCell)GetCellFromStr(variable.Key);

                if (cellToGetValueFrom != null && cellToGetValueFrom != cellToInterpret) {
                    
                    if (!VerifyNoCycles(cellToInterpret, cellToInterpret)) {
                        return false;
                    }
                    Subscribe(cellToInterpret, cellToGetValueFrom);
                    if (double.TryParse(cellToGetValueFrom.Value, out var varVal)) {
                        expTree.Variables[variable.Key] = varVal; // store cell value in expression tree
                    }
                    else if (String.IsNullOrEmpty(cellToGetValueFrom.Value)) {
                        expTree.Variables[variable.Key] = 0.0f; // cell has no value -- default to zero
                    }
                    else {
                        return false; // could not interpret cell value
                    }
                }
                else { // cell not found, or variable cell points to self
                    return false;
                }
            }
            return true;
        }
        
        /// <summary>
        /// verify that cell subscriptions contain no loops
        /// </summary>
        /// <param name="originCell">the originating cell to look for references to</param>
        /// <param name="cellToCheck">the cell whos subscribers should be checked</param>
        /// <returns>returns true if no loops, otherwise false</returns>
        private bool VerifyNoCycles(SpreadsheetCell originCell, SpreadsheetCell cellToCheck) {
            foreach (var subscription in cellToCheck.Subscriptions) {
                if (originCell == subscription) {
                    return false;
                }
                else {
                    var noLoops = VerifyNoCycles(originCell, subscription);
                    if (!noLoops) {
                        return false;
                    }
                }
            }
            return true;
        }
        
        /// <summary>
        /// set a cell to subscribe to updates to another cell
        /// </summary>
        /// <param name="subscriber">a cell which depends on the value of another cell</param>
        /// <param name="subscription">the cell which the subscriber depends on</param>
        private void Subscribe(SpreadsheetCell subscriber, SpreadsheetCell subscription) {
            if (!subscriber.Subscriptions.Contains(subscription)) {
                subscriber.Subscriptions.Add(subscription);
            }
            if (!subscription.Subscribers.Contains(subscriber)) {
                subscription.Subscribers.Add(subscriber);
            }
        }

        /// <summary>
        /// unsubscribe a cell to all its subscriptions
        /// </summary>
        /// <param name="subscriber">a cell which no longer depends on any other cells</param>
        private void Unsubscribe(SpreadsheetCell subscriber) {
            for (int i = 0; i < subscriber.Subscriptions.Count; i++) {
                var subscription = subscriber.Subscriptions[i];
                subscription.Subscribers.Remove(subscriber);
            }
            subscriber.Subscriptions.Clear();
        }

        /// <summary>
        /// get a cell from its cell name
        /// </summary>
        /// <param name="str">the string name of a cell</param>
        /// <returns>the cell described, or null if not found</returns>
        private Cell GetCellFromStr(string str) {
            Match m = Regex.Match(str.Substring(0, 1), "[a-zA-Z]"); // first char is a letter
            if (m.Success) {
                string col = m.Value.ToUpper(); // column name
                int colNum = (int)Enum.Parse(typeof(ColumnNames), col); // column number

                if (0 <= colNum && colNum < 26) { // column number is within expected range
                    if (Int32.TryParse(str.Substring(1), out int row1)) { // try to get 1-indexed row number
                        if (1 <= row1 && row1 < 51) { // check if row # is in expected range
                            int row0 = row1 - 1; // convert to 0 index row number
                            return _cells[row0, colNum];
                        }
                        else {
                            return null; // row number not in expected range
                        }
                    }
                    else {
                        return null; // could not parse row number
                    }
                }
            }
            return null; // first char of column name is not a letter
        }

        /// <summary>
        /// get a cell from its 0-indexed row/column numbers
        /// </summary>
        /// <param name="row">the row index of the cell to get</param>
        /// <param name="col">the column index of the cell to get</param>
        /// <returns>the cell requested</returns>
        public Cell GetCell(int row, int col) {
            return _cells[row, col];
        }

        /// <summary>
        /// clear the text in all cells
        /// </summary>
        public void ClearSpreadsheet() {
            foreach (var cell in _cells) {
                cell.Text = "";
            }
        }

        /// <summary>
        /// serialize the spreadsheet to a stream as XML
        /// </summary>
        /// <param name="stream"></param>
        public void Serialize(Stream stream) {
            var cellsToSave = new List<SerializableCell>();
            foreach (var cell in _cells) {
                if (!String.IsNullOrEmpty(cell.Text)) {
                    cellsToSave.Add(new SerializableCell(cell.RowIndex, cell.ColumnIndex, cell.Text));
                }
            }
            var serializer = new XmlSerializer(typeof(List<SerializableCell>));
            serializer.Serialize(stream, cellsToSave);
        }

        /// <summary>
        /// deserialize a spreadsheet from a stream containing XML
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>returns true if successfully deserialized</returns>
        public bool Deserialize(Stream stream) {
            var serializer = new XmlSerializer(typeof(List<SerializableCell>));
            try {
                var cells = (List<SerializableCell>) serializer.Deserialize(stream);
                ClearSpreadsheet();
                foreach (var cell in cells) {
                    GetCell(cell.RowIndex, cell.ColumnIndex).Text = cell.Text;
                }

                return true;
            }
            catch (Exception e) {
                return false;
            }
        }

        #endregion

        #region events & handlers

        /// <summary>
        /// when text of a cell changes update its value,
        /// or when the value of a cell changes, update its subsribers
        /// </summary>
        /// <param name="sender">the cell which triggered the event</param>
        /// <param name="e">event args containing the name of the property that changed</param>
        void HandlePropertyChangedEvent(object sender, PropertyChangedEventArgs e) {
            var cell = (SpreadsheetCell)sender;

            if (e.PropertyName == nameof(Cell.Text)) { // text changed -- update value
                UpdateCellValue(cell);
            }
            else if (e.PropertyName == nameof(Cell.Value)) { // value changed -- update subscribers
                foreach (var subscriber in cell.Subscribers) {
                    UpdateCellValue(subscriber);
                }
            }
            OnCellPropertyChanged(cell.RowIndex, cell.ColumnIndex); // when cell changes, trigger event
        }

        /// <summary>
        /// event to provide single point of subscription through spreadsheet class for all cell events
        /// </summary>
        /// <param name="cellRow">the row index of the updated cell</param>
        /// <param name="cellColumn">the column index of the updated cell</param>
        protected virtual void OnCellPropertyChanged(int cellRow, int cellColumn) {
            EventHandler handler = CellPropertyChanged;
            handler?.Invoke(this, new CellChangedEventArgs(cellRow, cellColumn));
        }

        public event EventHandler CellPropertyChanged;

        #endregion
    }
}
