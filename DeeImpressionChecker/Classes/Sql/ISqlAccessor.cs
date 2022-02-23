using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DeeImpressionChecker.Classes.Sql
{
    /// <summary>
    /// SQL infrastructure interface.
    /// </summary>
    interface ISqlAccessor
    {
        /// <summary>
        /// Create SQL table.
        /// </summary>
        /// <param name="source">SQL file name</param>
        void CreateTable(string source);

        /// <summary>
        /// Get SQL table.
        /// </summary>
        /// <param name="source">SQL database file name</param>
        /// <returns></returns>
        ObservableCollection<SongDetail> GetTable(string source);

        /// <summary>
        /// Save song data table.
        /// </summary>
        /// <param name="source">SQL database file name</param>
        /// <param name="table">Song data table</param>
        void SaveSongDataTable(string source, List<SongDetail> table);

        /// <summary>
        /// Set impression and avoid state.
        /// </summary>
        /// <param name="source">SQL database file name</param>
        /// <param name="table">Song data table</param>
        void SetImpressionState(string source, List<SongDetail> table);
    }
}
