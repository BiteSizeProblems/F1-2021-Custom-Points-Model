using _1MC_Live_Score_Application.ViewModels;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace _1MC_Live_Score_Application.LiveScore
{
    /// <summary>
    /// Interaction logic for LiveScore.xaml
    /// </summary>
    public partial class LiveScore : Window
    {
        private Timer slowTimer;

        public DataViewModel DataVM { get; set; }

        public LiveScore()
        {
            DataVM = DataViewModel.GetInstance();

            InitializeComponent();

            this.DataContext = DataVM;

            TeamScoreBox1.DataContext = DataVM.Team1;
            TeamScoreBox2.DataContext = DataVM.Team2;
            TeamScoreBox3.DataContext = DataVM.Team3;
            TeamScoreBox4.DataContext = DataVM.Team4;

            slowTimer = new Timer(AdjustTeamsView, null, 0, 10000);
        }

        private void AdjustTeamsView(object? state)
        {
            if (DataVM.SettingsModel.NumTeams == 2)
            {
                this.Dispatcher.Invoke(() =>
                {
                    AllTeamsBorder.Width = 400;
                    TeamScoreBox3.Visibility = Visibility.Collapsed;
                    TeamScoreBox4.Visibility = Visibility.Collapsed;
                });
            }
            else if (DataVM.SettingsModel.NumTeams == 3)
            {
                this.Dispatcher.Invoke(() =>
                {
                    AllTeamsBorder.Width = 650;
                    TeamScoreBox3.Visibility = Visibility.Visible;
                    TeamScoreBox4.Visibility = Visibility.Collapsed;
                });
            }
            else if (DataVM.SettingsModel.NumTeams == 4)
            {
                this.Dispatcher.Invoke(() =>
                {
                    TeamScoreBox3.Visibility = Visibility.Visible;
                    TeamScoreBox4.Visibility = Visibility.Visible;
                });
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
