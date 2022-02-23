using AngleSharp.Html.Dom;
using System;
using System.Collections.Generic;

namespace DeeImpressionChecker.Classes
{
    public static class SongListGetter
    {
        /// <summary>
        /// Get song list from top page.
        /// </summary>
        /// <param name="doc">HTML document</param>
        /// <param name="url">Top page URL</param>
        /// <returns></returns>
        public delegate List<SongDetail> GetSongList(IHtmlDocument doc, string url);

        /// <summary>
        /// Song list getters.
        /// </summary>
        public static GetSongList[] Func =
        {
            new GetSongList(GetSongListFromList),
            new GetSongList(GetSongListFromTeam),
        };

        /// <summary>
        /// Get song information from team style page.
        /// </summary>
        /// <param name="doc">HTML document</param>
        /// <param name="url">Top page URL</param>
        /// <returns></returns>
        private static List<SongDetail> GetSongListFromTeam(IHtmlDocument doc, string url)
        {
            var musicList = doc.QuerySelector("section#musiclist");
            if (musicList == null)
            {
                return null ;
            }

            var tableData = musicList.QuerySelectorAll(".pricing-title > div > h4 > strong");
            if (tableData.Length == 0)
            {
                return null;
            }
            var list = new List<SongDetail>();
            for (int i = 0; i < tableData.Length; i++)
            {
                var aTag = tableData[i].GetElementsByTagName("a")[0];
                var link = aTag.GetAttribute("href");
                int num = Convert.ToUInt16(link.Split('&')[1].Split('=')[1]);
                var pageUrl = GetSongUrl(url, link);

                var table = new SongDetail(pageUrl, aTag.InnerHtml, num);
                list.Add(table);
            }
            return list;
        }

        /// <summary>
        /// Get song information from list style page.
        /// </summary>
        /// <param name="doc">HTML document</param>
        /// <param name="url">Top page URL</param>
        /// <returns></returns>
        private static List<SongDetail> GetSongListFromList(IHtmlDocument doc, string url)
        {
            var musicList = doc.QuerySelector("section#musiclist");
            if (musicList == null)
            {
                return null;
            }

            var tableHead = musicList.QuerySelectorAll("thead > tr > th");
            if (tableHead.Length == 0)
            {
                return null;
            }

            int titleIndex = -1;
            for (int i = 0; i < tableHead.Length; i++)
            {
                if (tableHead[i].InnerHtml == "TITLE")
                {
                    titleIndex = i;
                }
            }
            if (titleIndex == -1)
            {
                return null;
            }

            var tableData = musicList.QuerySelectorAll("tbody > tr");
            if (tableData.Length == 0)
            {
                return null;
            }

            var list = new List<SongDetail>();
            for (int i = 0; i < tableData.Length; i++)
            {
                var aTag = tableData[i].GetElementsByTagName("td")[titleIndex].GetElementsByTagName("a")[0];
                var link = aTag.GetAttribute("href");
                int num = Convert.ToUInt16(link.Split('&')[1].Split('=')[1]);
                var pageUrl = GetSongUrl(url, link);

                var table = new SongDetail(pageUrl, aTag.InnerHtml, num);
                list.Add(table);
            }
            return list;
        }

        /// <summary>
        /// Create song detail page URL.
        /// </summary>
        /// <param name="url">Top page URL</param>
        /// <param name="songPage">Song detail page URL</param>
        /// <returns></returns>
        private static string GetSongUrl(string url, string songPage)
        {
            var linkUrl = url.Split('/');
            string pageLink = "https://";
            for (int i = 2; i < (linkUrl.Length - 1); i++)  // Index of "http://" is 2.
            {
                pageLink += linkUrl[i];
                pageLink += "/";
            }
            return pageLink + songPage;
        }
    }
}
