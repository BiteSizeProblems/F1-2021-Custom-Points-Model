using _1MC_Live_Score_Application.Core.Utils;
using _1MC_Live_Score_Application.Models;
using _1MC_Live_Score_Application.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Data;

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
            StartButton.Visibility = Visibility.Hidden;
            SimulateButton.Visibility = Visibility.Hidden;

            this.DriverList.Items.IsLiveSorting = true;
            this.DriverList.Items.SortDescriptions.Add(new SortDescription("CurrentPosition", ListSortDirection.Ascending));

        }

        private void ConfirmSettings_Click(object sender, RoutedEventArgs e)
        {
            // Set number of available teams.
            DataVM.SettingsModel.AvailableTeams = new int[DataVM.SettingsModel.NumTeams];

            for (int i = 0; i < DataVM.SettingsModel.NumTeams; i++)
            {
                DataVM.SettingsModel.AvailableTeams[i] = i + 1;
            }

            DataVM.SettingsModel.PointsModel = PositionToPointsConverter.GetPointsModel(DataVM.SettingsModel.NumTeams);

            // Set series and round tags.
            DataVM.SettingsModel.Series = SeriesNameTextBox.Text;
            DataVM.SettingsModel.Round = RoundNumberTextBox.Text;

            // Set data context for team drop-down menu.
            TeamsComboBox.ItemsSource = DataVM.SettingsModel.AvailableTeams;

            // Hide driver list until settings have been confirmed.
            DriverListBox.Visibility = Visibility.Visible;
            TeamSummaryButton.Visibility = Visibility.Visible;
            StartButton.Visibility = Visibility.Visible;
            SimulateButton.Visibility = Visibility.Visible;

        }

        private void SimulateButton_Clicked(object sender, RoutedEventArgs e)
        {
            DataVM.ResetTeams();

            if (DataVM.SettingsModel._settingsEntered == false)
            {
                // Error/Debug Terminal.
                DataVM.SubmitSettings_Simulate();

                // Create new window to display scores.
                LiveScore.LiveScore secondWindow = new LiveScore.LiveScore();
                secondWindow.Show();

                DataVM.SettingsModel._settingsEntered = true;
            }
            else
            {

                ClearExistingData();

                // Error/Debug Terminal.
                DataVM.SubmitSettings_Simulate();

                // Create new window to display scores.
                LiveScore.LiveScore secondWindow = new LiveScore.LiveScore();
                secondWindow.Show();

                DataVM.SettingsModel._settingsEntered = true;
            }
        }

        private void StartButton_Clicked(object sender, RoutedEventArgs e)
        {
            DataVM.ResetTeams();

            if (DataVM.SettingsModel._settingsEntered == false)
            {
                // Error/Debug Terminal.
                DataVM.SubmitSettings();

                // Create new window to display scores.
                LiveScore.LiveScore secondWindow = new LiveScore.LiveScore();
                secondWindow.Show();

                DataVM.SettingsModel._settingsEntered = true;

            }
            else
            {
                ClearExistingData();

                // Error/Debug Terminal.
                DataVM.SubmitSettings();

                // Create new window to display scores.
                LiveScore.LiveScore secondWindow = new LiveScore.LiveScore();
                secondWindow.Show();

                DataVM.SettingsModel._settingsEntered = true;
            }
        }

        private void ClearExistingData()
        {
            DataVM.Team.Clear();
            //DataVM.Reset();
        }

        private void TeamsButton_Clicked(object sender, RoutedEventArgs e)
        {
            // Create new window to display Team Summaries.
            TeamsSummaryWindow.TeamsSummaryWindow summaryWindow = new TeamsSummaryWindow.TeamsSummaryWindow();
            summaryWindow.Show();
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
    }
}
