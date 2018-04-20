using System.ComponentModel;
using System.Runtime.CompilerServices;
using SpreadsheetEngine.Annotations;

namespace CptS321
{
    /// <summary>
    /// Abstract base class from which SpreadsheetCell and SerializableCell are derived
    /// </summary>
    public abstract class Cell : INotifyPropertyChanged
    {
        #region fields

        protected string _text;
        protected string _value;

        #endregion

        #region properties

        public int RowIndex { get; }
        public int ColumnIndex { get; }

        public string Text
        {
            get => _text;
            set
            {
                if (_text?.CompareTo(value) != 0)
                {
                    _text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }

        public string Value => _value;

        #endregion

        #region constructor

        protected Cell()
        {
        }

        protected Cell(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Text = "";
        }

        #endregion

        #region events & handlers

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}