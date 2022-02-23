using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;

namespace DeeImpressionChecker.Classes
{
    /// <summary>
    /// Song data table
    /// </summary>
    public static class SongDataTable
    {
        static ObservableCollection<SongDetail> _table;

        /// <summary>
        /// Song table
        /// </summary>
        public static ObservableCollection<SongDetail> Table { get { return _table; } set { _table = value; } }

        /// <summary>
        /// Song table(Hide impressioned songs = true)
        /// </summary>
        public static IEnumerable AvoidFinishedTable
        {
            get
            {
                var itemSourceList = new CollectionViewSource() { Source = _table };
                itemSourceList.Filter += new FilterEventHandler(SongListFilterByIsFinished);
                return itemSourceList.View;
            }
        }

        /// <summary>
        /// Entry number.
        /// </summary>
        public static int EntryNumber
        {
            get
            {
                return _table.Count - _table.Sum(v => Convert.ToInt16(v.IsAvoided == true));
            }
        }

        /// <summary>
        /// Finisshed number.
        /// </summary>
        public static int ImpressionedNumber
        {
            get
            {
                return _table.Sum(v => Convert.ToInt16(v.IsImpressioned == true));
            }
        }

        /// <summary>
        /// Residual number.
        /// </summary>
        public static int ResidualNumber
        {
            get
            {
                return EntryNumber - ImpressionedNumber;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        static SongDataTable()
        {
            _table = new ObservableCollection<SongDetail>();
        }

        /// <summary>
        /// If IsFinished is true, hide the row.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        static private void SongListFilterByIsFinished(object sender, FilterEventArgs e)
        {
            var obj = e.Item as SongDetail;
            if (obj != null)
            {
                e.Accepted = !obj.IsImpressioned;
            }
        }
    }
}
