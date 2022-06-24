using _1MC_Live_Score_Application.Core;
using _1MC_Live_Score_Application.Core.Converters;
using _1MC_Live_Score_Application.Core.Utils;
using _1MC_Live_Score_Application.ViewModels;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace _1MC_Live_Score_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DataViewModel DataVM { get; set; }

        public MainWindow()
        {
            DataVM = DataViewModel.GetInstance();

            InitializeComponent();

            this.DataContext = DataVM;

            ConnectionStatusTB.DataContext = DataVM.SettingsModel;
            TeamsAndScoringModelSelectors.DataContext = DataVM.SettingsModel;

            TeamSummaryButton.Visibility = Visibility.Hidden;
            DriverListBox.Visibility = Visibility.Hidden;

            ExportButton.Visibility = Visibility.Hidden;

            ConfirmAndDisplayButton.Visibility = Visibility.Hidden;
            SimulateScoreButton.Visibility = Visibility.Hidden;

            this.DriverList.Items.IsLiveSorting = true;
            this.DriverList.Items.SortDescriptions.Add(new SortDescription("CurrentPosition", ListSortDirection.Ascending));

        }

        private void NumberOfTeams_Checked(object sender, RoutedEventArgs e)
        {
            if (TwoTeams.IsChecked == true)
            {
                DataVM.SettingsModel.NumTeams = 2;
            }
            else if (ThreeTeams.IsChecked == true)
            {
                DataVM.SettingsModel.NumTeams = 3;
            }
            else if (FourTeams.IsChecked == true)
            {
                DataVM.SettingsModel.NumTeams = 4;
            }
        }

        private void SettingsConfirmed_Checked(object sender, RoutedEventArgs e)
        {
            // Set number of available teams.
            DataVM.SettingsModel.AvailableTeams = new int[DataVM.SettingsModel.NumTeams];

            for (int i = 0; i < DataVM.SettingsModel.NumTeams; i++)
            {
                DataVM.SettingsModel.AvailableTeams[i] = i + 1;
            }

            DataVM.SettingsModel.PointsModel = PositionToPointsConverter.GetPointsModel(DataVM.SettingsModel.NumTeams);

            DataVM.CreateTeams();

            // Set series and round tags.
            DataVM.SettingsModel.Series = SeriesNameTextBox.Text;
            DataVM.SettingsModel.Round = RoundNumberTextBox.Text;

            // Set data context for team drop-down menu.
            TeamsComboBox.ItemsSource = DataVM.SettingsModel.AvailableTeams;

            // Hide driver list until settings have been confirmed.
            DriverListBox.Visibility = Visibility.Visible;

            ConfirmAndDisplayButton.Visibility = Visibility.Visible;
            SimulateScoreButton.Visibility = Visibility.Visible;
        }

        private void ConfirmAndDisplay_Checked(object sender, RoutedEventArgs e)
        {
            TeamSummaryButton.Visibility = Visibility.Visible; // Display button for team summary
            ExportButton.Visibility = Visibility.Visible;

            DataVM.GetDriversPerTeam(); // Create teams

            if ( SimulateScoreButton.IsChecked == true)
            {
                DataVM.SimulateTeamPointsNew();
            }

            LiveScore.LiveScore secondWindow = new LiveScore.LiveScore(); // Create new window to display scores.
            secondWindow.Show();
        }

        private void Simulate_Checked(object sender, RoutedEventArgs e)
        {
            DataVM.SimulateDrivers();
        }

        private void TeamsButton_Clicked(object sender, RoutedEventArgs e)
        {
            // Create new window to display Team Summaries.
            TeamsSummaryWindow.TeamsSummaryWindow summaryWindow = new TeamsSummaryWindow.TeamsSummaryWindow();
            summaryWindow.Show();
        }

        private void ExportButtonClicked(object sender, RoutedEventArgs e)
        {
            Export.ModelToExcel();
        }

        private void Team_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (Team1ColorSelector.SelectedColor.HasValue)
            {
                DataVM.SettingsModel.Team1Color = Team1ColorSelector.SelectedColor.Value;
            }

            if (Team2ColorSelector.SelectedColor.HasValue)
            {
                DataVM.SettingsModel.Team2Color = Team2ColorSelector.SelectedColor.Value;
            }

            if (Team3ColorSelector.SelectedColor.HasValue)
            {
                DataVM.SettingsModel.Team3Color = Team3ColorSelector.SelectedColor.Value;
            }

            if (Team4ColorSelector.SelectedColor.HasValue)
            {
                DataVM.SettingsModel.Team4Color = Team4ColorSelector.SelectedColor.Value;
            }
        }
    }
}
