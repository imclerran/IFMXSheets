using System;
using System.IO;
using System.Windows.Forms;
using SpreadsheetEngine;

namespace SpreadsheetApp
{
    public partial class Form1 : Form
    {
        #region fields

        private readonly string[] _alphabet = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
            "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};

        private struct Cell
        {
            internal int RowIndex;
            internal int ColumnIndex;
        }

        private Cell _currentCell;
        private readonly SpreadsheetEngine.Spreadsheet _spreadsheet;

        #endregion

        #region initialization
        
        /// <summary>
        /// initialize Form, populate grid with rows/columns,
        ///  create a new spreadsheet, and subscribe to spreadsheet event
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            CreateRowsColumns();
            _spreadsheet = new SpreadsheetEngine.Spreadsheet(50, 26);
            _spreadsheet.CellPropertyChanged += HandleCellChangedEvent;
        }
        
        /// <summary>
        /// add rows/columns to DataGridView
        /// </summary>
        private void CreateRowsColumns()
        {
            foreach (var letter in _alphabet)
            {
                DataGridViewColumn col = new DataGridViewTextBoxColumn();
                col.HeaderText = letter;
                dataGridView1.Columns.Add(col);
            }

            for (int i = 0; i < 50; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.HeaderCell.Value = (i + 1).ToString();
                dataGridView1.Rows.Add(row);
            }
        }

        #endregion

        #region methods
        
        /// <summary>
        /// HW5 demo: set text for cells with strings/equations
        /// </summary>
        private void RunDemo()
        {
            var Rand = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < 50; i++)
            {
                int columnIndex = Rand.Next(0, 25);
                int rowIndex = Rand.Next(0, 49);
                var cell = _spreadsheet.GetCell(rowIndex, columnIndex);
                cell.Text = "Hello, world!";
            }

            for (int i = 0; i < _spreadsheet.RowCount; i++)
            {
                var cell = _spreadsheet.GetCell(i, 1);
                cell.Text = "This is cell B" + (i + 1);

                cell = _spreadsheet.GetCell(i, 0);
                cell.Text = "=B" + (i + 1);
            }
        }

        /// <summary>
        /// save the spreadsheet by writing its XML serialization to a file
        /// </summary>
        /// <param name="filepath"></param>
        private void SaveSpreadsheet(string filepath) {
            Stream writer = new FileStream(filepath, FileMode.Create);
            _spreadsheet.Serialize(writer);
        }

        /// <summary>
        /// load the spreadsheet by deserializing an XML file
        /// </summary>
        /// <param name="filepath"></param>
        private void LoadSpreadsheet(string filepath) {
            Stream reader = new FileStream(filepath, FileMode.Open);
        }

        #endregion

        #region events & handlers
        
        /// <summary>
        /// when a cell in the SpreadsheetEngine changes, update the cell in the DataGridView (GUI)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleCellChangedEvent(object sender, EventArgs e)
        {
            var a = (CellChangedEventArgs)e;
            dataGridView1.Rows[a.RowIndex].Cells[a.ColumnIndex].Value =
                _spreadsheet.GetCell(a.RowIndex, a.ColumnIndex).Value;

            if (dataGridView1.SelectedCells.Count == 1
                && dataGridView1.SelectedCells[0].RowIndex == a.RowIndex
                && dataGridView1.SelectedCells[0].ColumnIndex == a.ColumnIndex) {
                txtFormulaEditor.Text = _spreadsheet.GetCell(a.RowIndex, a.ColumnIndex).Text;
            }
        }
        
        /// <summary>
        /// when editing a cell, show text instead of value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value =
                _spreadsheet.GetCell(e.RowIndex, e.ColumnIndex).Text;
        }
        
        /// <summary>
        /// when finished editing cell, replace text with value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            _spreadsheet.GetCell(e.RowIndex, e.ColumnIndex).Text =
                (string)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value =
                _spreadsheet.GetCell(e.RowIndex, e.ColumnIndex).Value;
        }
        
        /// <summary>
        /// run the HW5 demo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runDemoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RunDemo();
        }
        
        /// <summary>
        /// save the focused cell coords in currentCell, and set textbox text from cell text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            _currentCell.RowIndex = e.RowIndex;
            _currentCell.ColumnIndex = e.ColumnIndex;
            txtFormulaEditor.Text = _spreadsheet.GetCell(e.RowIndex, e.ColumnIndex).Text;
        }
        
        /// <summary>
        /// set text of selected cell to textbox text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFormulaEditor_Leave(object sender, EventArgs e)
        {
            _spreadsheet.GetCell(_currentCell.RowIndex, _currentCell.ColumnIndex).Text = txtFormulaEditor.Text;
        }
        
        /// <summary>
        /// when enter pressed, set text of selected cell to  textbox text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFormulaEditor_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                _spreadsheet.GetCell(_currentCell.RowIndex, _currentCell.ColumnIndex).Text = txtFormulaEditor.Text;
            }
        }
        
        /// <summary>
        /// when delete key pressed, if not currently editing a cell, then clear the contents of the selected cell.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (!dataGridView1.IsCurrentCellInEditMode)
            {
                if (e.KeyData == Keys.Delete)
                {
                    _spreadsheet.GetCell(_currentCell.RowIndex, _currentCell.ColumnIndex).Text = "";
                    txtFormulaEditor.Text = "";
                }
            }
        }

        /// <summary>
        /// when "open" selected from "file" menu, load the spreadsheet using the deserializer
        /// </summary>
        /// <param name="sender">the object that triggered the event</param>
        /// <param name="e">the event args</param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) {
                var filepath = openFileDialog1.FileName;
                Stream reader = new FileStream(filepath, FileMode.Open);
                _spreadsheet.Deserialize(reader);
            }
        }

        /// <summary>
        /// when "save" selected from "file" menu, save the spreadsheet using the serializer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            saveFileDialog1.FileName = "spreadsheet1.ifmx";
            var result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK) {
                var filepath = saveFileDialog1.FileName;
                Stream writer = new FileStream(filepath, FileMode.Create);
                _spreadsheet.Serialize(writer);
            }
        }

        /// <summary>
        /// when "about" selected from "help" menu, show the about box window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            AboutBox1 aboutBox = new AboutBox1();
            aboutBox.Show();
        }

        /// <summary>
        /// Clear the spreadsheet to start fresh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            _spreadsheet.ClearSpreadsheet();
        }

        #endregion
    }
}