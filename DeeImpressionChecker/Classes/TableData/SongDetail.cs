using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DeeImpressionChecker.Classes
{
    /// <summary>
    /// Song table datas.
    /// </summary>
    public class SongDetail : INotifyPropertyChanged
    {
        /// <summary>
        /// Song page URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Song title
        /// </summary>
        public string SongTitle { get; set; }

        /// <summary>
        /// Registration number of song
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// The song is Impressioned
        /// </summary>
        bool _isImpressioned;
        public bool IsImpressioned
        {
            get
            {
                return _isImpressioned;
            }
            set
            {
                _isImpressioned = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Impression is avoided
        /// </summary>
        bool _isAvoided;
        public bool IsAvoided
        {
            get
            {
                return _isAvoided;
            }
            set
            {
                _isAvoided = value && !IsImpressioned;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="url">Song page URL</param>
        /// <param name="songTitle">Song title</param>
        /// <param name="num">Registration number of song</param>
        public SongDetail(string url, string songTitle, int num)
        {
            Url = url;
            SongTitle = songTitle;
            Num = num;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notify property change.
        /// </summary>
        /// <param name="name"></param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
