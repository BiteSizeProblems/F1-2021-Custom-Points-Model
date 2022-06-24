using _1MC_Live_Score_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _1MC_Live_Score_Application.MultiViews
{
    /// <summary>
    /// Interaction logic for TeamScoreBoxView.xaml
    /// </summary>
    public partial class TeamScoreBoxView : UserControl
    {
        public DataViewModel DataVM { get; set; }

        public TeamScoreBoxView()
        {
            DataVM = DataViewModel.GetInstance();

            InitializeComponent();

            this.DataContext = DataVM;
        }
    }
}
