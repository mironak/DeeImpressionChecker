using DeeImpressionChecker.Classes.Sql;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DeeImpressionChecker.Classes
{
    public sealed class SQLiteAccess : ISqlAccessor
    {
        /// <summary>
        /// Create SQLite table.
        /// </summary>
        /// <param name="source">SQL database file name</param>
        public void CreateTable(string source)
        {
            // Create SQL table.
            using (var connection = new SqliteConnection("Data Source=" + source))
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText =
                    @"create table songList(num int primary key, title varchar(100), url varchar(100), isFinished int, avoid int)";
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Get SQLite table.
        /// </summary>
        /// <param name="source">SQL database file name</param>
        /// <returns></returns>
        public ObservableCollection<SongDetail> GetTable(string source)
        {
            var table = new ObservableCollection<SongDetail>();

            using (var connection = new SqliteConnection("Data Source=" + source))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = @"select * from songList";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object[] data = Enumerable.Range(0, reader.FieldCount).Select(i => reader[i]).ToArray();
                        table.Add(ParseTableData(data));
                    }
                }
                command.ExecuteNonQuery();
            }
            return table;
        }

        /// <summary>
        /// Save song data table.
        /// </summary>
        /// <param name="source">SQL database file name</param>
        /// <param name="table">Song data table</param>
        public void SaveSongDataTable(string source, List<SongDetail> table)
        {
            using (var connection = new SqliteConnection("Data Source=" + source))
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                var insertCommand = "";
                foreach (var t in table)
                {
                    insertCommand +=
                        (@"insert or ignore into songList values("
                        + t.Num + ", "
                        + "'" + t.SongTitle.Replace("'", "''") + "', "
                        + "'" + t.Url + "', "
                        + Convert.ToInt32(t.IsImpressioned) + ", "
                        + Convert.ToInt32(t.IsAvoided) + ");");
                }

                command.CommandText = insertCommand;
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Set impression and avoid state.
        /// </summary>
        /// <param name="source">SQL database file name</param>
        /// <param name="table">Song data table</param>
        public void SetImpressionState(string source, List<SongDetail> table)
        {
            using (var connection = new SqliteConnection("Data Source=" + source))
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                var insertCommand = "";
                foreach (var t in table)
                {
                    insertCommand +=
                        (@"update songList set "
                        + "isFinished = " + Convert.ToInt32(t.IsImpressioned) + ", "
                        + "avoid = " + Convert.ToInt32(t.IsAvoided) + " "
                        + "where num = " + t.Num + ";");
                    command.CommandText = insertCommand;
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Get song table class from sql result.
        /// </summary>
        /// <param name="sqlResult">Result of select</param>
        /// <returns></returns>
        private static SongDetail ParseTableData(object[] sqlResult)
        {
            var table = new SongDetail(sqlResult[2].ToString(), sqlResult[1].ToString(), Convert.ToInt32(sqlResult[0]));
            table.IsImpressioned = (Convert.ToInt32(sqlResult[3]) == 1);
            table.IsAvoided = (Convert.ToInt32(sqlResult[4]) == 1);
            return table;
        }
    }
}
