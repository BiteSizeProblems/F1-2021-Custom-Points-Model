using _1MC_Live_Score_Application.Core;
using _1MC_Live_Score_Application.ViewModels;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;

namespace _1MC_Live_Score_Application.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : Window
    {
        public DataViewModel DataVM { get; set; }

        public SettingsView()
        {
            DataVM = DataViewModel.GetInstance();

            InitializeComponent();
        }

        private void OpenNewDialog()
        {
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "My Title";
            dlg.IsFolderPicker = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var folder = dlg.FileName;
                DataVM.SettingsModel.FilePath = folder;
                Debug.WriteLine(folder);
            }
        }

        private void ExportButtonClicked(object sender, RoutedEventArgs e)
        {
            DataVM.SettingsModel.Series = SeriesNameTextBox.Text;
            DataVM.SettingsModel.Round = RoundNumberTextBox.Text;

            OpenNewDialog();

            Export.ModelToExcel();
        }
    }
}
