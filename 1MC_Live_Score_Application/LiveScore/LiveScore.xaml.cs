using _1MC_Live_Score_Application.Models;
using _1MC_Live_Score_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _1MC_Live_Score_Application.LiveScore
{
    /// <summary>
    /// Interaction logic for LiveScore.xaml
    /// </summary>
    public partial class LiveScore : Window
    {
        public DataViewModel DataVM { get; set; }

        public LiveScore()
        {
            DataVM = DataViewModel.GetInstance();

            InitializeComponent();

            this.DataContext = DataVM;
        }

    }
}
