using DeeImpressionChecker.Classes;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DeeImpressionChecker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// "MainWindow" is loaded
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load properties.
            VenueUrlTextBox.Text = Properties.Settings.Default.VenueUrl;
            ImpressionIdTextBox.Text = Properties.Settings.Default.ImpressionID;

            // Impression ID length is cheked.
            if (ImpressionIdTextBox.Text.Length == 0)
            {
                SetStatusListNone();
                return;
            }

            // Impression ID length is cheked.
            if (!SQLAccess.ExistsTableFile(ImpressionIdTextBox.Text, VenueUrlTextBox.Text))
            {
                SetStatusListNone();
                return;
            }

            // If database has been created, load database.
            SongDataTable.Table = SQLAccess.GetTable(ImpressionIdTextBox.Text, VenueUrlTextBox.Text);
            TableListView.ItemsSource = SongDataTable.Table;
            SetStatusWait();
        }

        /// <summary>
        /// "MainWindow" is closed
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Window_Closed(object sender, EventArgs e)
        {
            // Save properties.
            Properties.Settings.Default.VenueUrl = VenueUrlTextBox.Text;
            Properties.Settings.Default.ImpressionID = ImpressionIdTextBox.Text;
            Properties.Settings.Default.Save();

            // Save table
            if (SQLAccess.ExistsTableFile(ImpressionIdTextBox.Text, VenueUrlTextBox.Text))
            {
                SQLAccess.SetImpressionState(ImpressionIdTextBox.Text, VenueUrlTextBox.Text, SongDataTable.Table.ToList());
            }
        }

        /// <summary>
        /// "Impression ID" textBox is changed.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void ImpressionIdTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            SetStatusListNone();
        }

        /// <summary>
        /// "Hide impressioned songs" is checked.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void HideImpressionedSongsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            TableListView.ItemsSource = SongDataTable.AvoidFinishedTable;
        }

        /// <summary>
        /// "Hide impressioned songs" is unchecked.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void HideImpressionedSongsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // All song view.
            TableListView.ItemsSource = SongDataTable.Table;
        }

        /// <summary>
        /// "Avoid" checkBox is clicked.
        /// </summary>
        /// <param name="sender"sender></param>
        /// <param name="e">e</param>
        private void ImpressionAvoidCheckBox_Click(object sender, RoutedEventArgs e)
        {
            EntryBmsLabel.Content = SongDataTable.EntryNumber;
            ResidualLabel.Content = SongDataTable.ResidualNumber;
        }

        /// <summary>
        /// "Get list" button is clicked.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private async void GetListButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ImpressionIdTextBox.Text.Length == 0)
                {
                    MessageBox.Show($"Please input Impression ID");
                    return;
                }

                // Get table
                SongDataTable.Table = await HtmlGetter.GetSongTable(VenueUrlTextBox.Text);
                TableListView.ItemsSource = SongDataTable.Table;

                // Save
                SQLAccess.CreateTable(ImpressionIdTextBox.Text, VenueUrlTextBox.Text);
                SQLAccess.SaveSongDataTable(ImpressionIdTextBox.Text, VenueUrlTextBox.Text, SongDataTable.Table.ToList());

                SetStatusWait();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                SetStatusError();
            }
        }

        /// <summary>
        /// "Start" button is clicked.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ImpressionIdTextBox.Text.Length == 0)
                {
                    MessageBox.Show($"Please input Impression ID");
                    return;
                }

                SetStatusProcessing();

                await CheckImpressioned();

                SetStatusCompleted();

                // Save
                SQLAccess.SetImpressionState(ImpressionIdTextBox.Text, VenueUrlTextBox.Text, SongDataTable.Table.ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                SetStatusError();
            }
        }

        /// <summary>
        /// Song title in the list is clicked.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void SongTitle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(SongDataTable.Table[TableListView.SelectedIndex].Url);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                SetStatusError();
            }
        }

        /// <summary>
        /// Check impressioned.
        /// </summary>
        /// <returns></returns>
        private async Task CheckImpressioned()
        {
            for (int i = 0; i < SongDataTable.Table.Count; i++)
            {
                if (SongDataTable.Table[i].IsImpressioned || SongDataTable.Table[i].IsAvoided)
                {
                    continue;
                }

                StatusProgressBar.Value = 100 * i / SongDataTable.Table.Count;
                SongDataTable.Table[i].IsImpressioned = await HtmlGetter.IsImpressioned(SongDataTable.Table[i].Url, ImpressionIdTextBox.Text);
                SetStatusProcessing();
                await Task.Delay(1000);
            }
        }

        /// <summary>
        /// Set list none status.
        /// </summary>
        private void SetStatusListNone()
        {
            StatusLabel.Content = "Input Venue URL and Impression ID, press [Get list] button";
            StatusProgressBar.Value = 0;
            StartButton.IsEnabled = false;
        }

        /// <summary>
        /// Set completed status.
        /// </summary>
        private void SetStatusProcessing()
        {
            EntryBmsLabel.Content = SongDataTable.EntryNumber;
            ImpressionedLabel.Content = SongDataTable.ImpressionedNumber;
            ResidualLabel.Content = SongDataTable.ResidualNumber;
            
            StatusLabel.Content = "Processing...";
            GetListButton.IsEnabled = false;
        }

        /// <summary>
        /// Set completed status.
        /// </summary>
        private void SetStatusCompleted()
        {
            ImpressionedLabel.Content = SongDataTable.ImpressionedNumber;
            ResidualLabel.Content = SongDataTable.ResidualNumber;

            StatusLabel.Content = "Completed.";
            StatusProgressBar.Value = 100;
            GetListButton.IsEnabled = true;
        }

        /// <summary>
        /// Set wait status.
        /// </summary>
        private void SetStatusWait()
        {
            EntryBmsLabel.Content = SongDataTable.EntryNumber;
            ImpressionedLabel.Content = SongDataTable.ImpressionedNumber;
            ResidualLabel.Content = SongDataTable.ResidualNumber;

            StatusLabel.Content = "Input Impression ID, press [Start] button";
            StatusProgressBar.Value = 0;
            GetListButton.IsEnabled = true;
            StartButton.IsEnabled = true;
        }

        /// <summary>
        /// Set completed status.
        /// </summary>
        private void SetStatusError()
        {
            StatusLabel.Content = "Error";
            StatusProgressBar.Value = 0;
            GetListButton.IsEnabled = true;
            StartButton.IsEnabled = true;
        }
    }
}
