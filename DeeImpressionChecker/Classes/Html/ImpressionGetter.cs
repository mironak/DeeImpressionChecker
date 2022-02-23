using AngleSharp.Html.Dom;

namespace DeeImpressionChecker.Classes
{
    /// <summary>
    /// Get impressioned.
    /// </summary>
    public static class ImpressionGetter
    {
        /// <summary>
        /// Search impression id from song detail page.
        /// </summary>
        /// <param name="doc">HTML document</param>
        /// <param name="id">Impression ID</param>
        /// <returns></returns>
        public delegate bool GetImpression(IHtmlDocument doc, string id);

        /// <summary>
        /// Impression getters.
        /// </summary>
        public static GetImpression[] Func =
        {
            new GetImpression(ImpressionGetterFromImpression),
            new GetImpression(ImpressionGetterFromPoint),
        };

        /// <summary>
        /// Search impression id from impression.
        /// </summary>
        /// <param name="doc">HTML document</param>
        /// <param name="id">Impression ID</param>
        /// <returns></returns>
        private static bool ImpressionGetterFromImpression(IHtmlDocument doc, string id)
        {
            var impreDataList = doc.GetElementsByClassName("spost");
            for (int i = 0; i < impreDataList.Length; i++)
            {
                // One line impression
                if (impreDataList[i].GetElementsByClassName("points_oneline").Length != 0)
                {
                    if (impreDataList[i].InnerHtml.Contains(id))
                    {
                        return true;
                    }
                }
                
                // Long impression
                if (impreDataList[i].GetElementsByClassName("points_normal").Length != 0)
                {
                    if (impreDataList[i].InnerHtml.Contains(id))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Search impression id from vote.
        /// </summary>
        /// <param name="doc">HTML document</param>
        /// <param name="id">Impression ID</param>
        /// <returns></returns>
        private static bool ImpressionGetterFromPoint(IHtmlDocument doc, string id)
        {
            var voteList = doc.GetElementsByClassName("col_one_third");
            for (int i = 0; i < voteList.Length; i++)
            {
                if (voteList[i].InnerHtml.Contains(id))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
