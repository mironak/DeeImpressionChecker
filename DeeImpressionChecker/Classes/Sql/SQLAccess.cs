using DeeImpressionChecker.Classes.Sql;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace DeeImpressionChecker.Classes
{
    /// <summary>
    /// SQL operation.
    /// </summary>
    static class SQLAccess
    {
        static ISqlAccessor _sqlAccessor;

        /// <summary>
        /// Constructor
        /// </summary>
        static SQLAccess()
        {
            _sqlAccessor = new SQLiteAccess();
        }

        /// <summary>
        /// If SQL table has already been created, return true.
        /// </summary>
        /// <param name="id">Impression ID</param>
        /// <param name="url">Song detail page URL</param>
        public static bool ExistsTableFile(string id, string url)
        {
            return File.Exists(GetDataSourceName(id, url));
        }

        /// <summary>
        /// Create SQL table if it doesn't exist.
        /// </summary>
        /// <param name="id">Impression ID</param>
        /// <param name="url">Song detail page URL</param>
        public static void CreateTable(string id, string url)
        {
            if (ExistsTableFile(id, url))
            {
                return;
            }

            _sqlAccessor.CreateTable(GetDataSourceName(id, url));
        }

        /// <summary>
        /// Get song list.
        /// </summary>
        /// <param name="id">Impression ID</param>
        /// <param name="url">Song detail page URL</param>
        public static ObservableCollection<SongDetail> GetTable(string id, string url)
        {
            if (!ExistsTableFile(id, url))
            {
                _sqlAccessor.CreateTable(GetDataSourceName(id, url));
            }

            return _sqlAccessor.GetTable(GetDataSourceName(id, url));
        }

        /// <summary>
        /// Save table data.
        /// </summary>
        /// <param name="id">Impression ID</param>
        /// <param name="url">Song detail page URL</param>
        /// <param name="table">Song data table</param>
        public static void SaveSongDataTable(string id, string url, List<SongDetail> table)
        {
            _sqlAccessor.SaveSongDataTable(GetDataSourceName(id, url), table);
        }

        /// <summary>
        /// Save table data.
        /// </summary>
        /// <param name="id">Impression ID</param>
        /// <param name="url">Song detail page URL</param>
        /// <param name="table">Song data table</param>
        public static void SetImpressionState(string id, string url, List<SongDetail> table)
        {
            if (!ExistsTableFile(id, url))
            {
                return;
            }

            _sqlAccessor.SetImpressionState(GetDataSourceName(id, url), table);
        }

        /// <summary>
        /// Get SQL file name.
        /// </summary>
        /// <param name="id">Impression ID</param>
        /// <param name="url">Song detail page URL</param>
        /// <returns></returns>
        private static string GetDataSourceName(string id, string url)
        {
            //return (id + url.Split('=').Last()).GetHashCode().ToString() + ".sqlite";
            var dir = $"{System.IO.Directory.GetCurrentDirectory()}\\Database\\";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return $"{dir}{(id + url.Split('=').Last())}.sqlite";
        }
    }
}
