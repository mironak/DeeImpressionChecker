using System;
using System.Collections.ObjectModel;
using System.Linq;

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
    }
}
