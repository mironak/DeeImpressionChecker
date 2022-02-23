using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace DeeImpressionChecker.Classes
{
    /// <summary>
    /// HTML operation
    /// </summary>
    public static class HtmlGetter
    {
        /// <summary>
        /// Get songs table.
        /// </summary>
        /// <param name="url">Top page URL</param>
        /// <returns></returns>
        public static async Task<ObservableCollection<SongDetail>> GetSongTable(string url)
        {
            var doc = await GetDocument(url);

            foreach (var f in SongListGetter.Func)
            {
                var table = f(doc, url);
                if (table != null)
                {
                    return new ObservableCollection<SongDetail>(table);
                }
            }

            return null;
        }

        /// <summary>
        /// Return true, if user of "Impression ID" has already impressioned.
        /// </summary>
        /// <param name="url">Song page URL</param>
        /// <param name="id">Impression ID</param>
        /// <returns></returns>
        public static async Task<bool> IsImpressioned(string url, string id)
        {
            var doc = await GetDocument(url);

            foreach (var f in ImpressionGetter.Func)
            {
                var success = f(doc, id);
                if (success)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get HTML document.
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns></returns>
        private static async Task<IHtmlDocument> GetDocument(string url)
        {
            using (var client = new HttpClient())
            using (var stream = await client.GetStreamAsync(new Uri(url)))
            {
                var parser = new HtmlParser();
                return parser.ParseDocument(stream);
            }
        }
    }
}
