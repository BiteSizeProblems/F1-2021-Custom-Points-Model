using _1MC_Live_Score_Application.ViewModels;
using System.Windows;

namespace _1MC_Live_Score_Application.TeamsSummaryWindow
{
    /// <summary>
    /// Interaction logic for TeamsSummaryWindow.xaml
    /// </summary>
    public partial class TeamsSummaryWindow : Window
    {
        public DataViewModel DataVM { get; set; }

        public TeamsSummaryWindow()
        {
            DataVM = DataViewModel.GetInstance();

            InitializeComponent();

            this.DataContext = DataVM;
        }
    }
}
