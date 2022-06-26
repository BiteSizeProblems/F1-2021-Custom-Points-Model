using _1MC_Live_Score_Application.Models;
using _1MC_Live_Score_Application.ViewModels;
using System;
using Microsoft.Win32;
using OfficeOpenXml;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace _1MC_Live_Score_Application.Core
{
    public class Export
    {
        public DataViewModel DataVM { get; set; }

        public Export()
        {
            DataVM = DataViewModel.GetInstance();
        }

        public static async Task ModelToExcel()
        {
            var DataVM = DataViewModel.GetInstance();
            var driver = DataVM.Driver;
            var team = DataVM.Team;
            var settings = DataVM.SettingsModel;

            var round = settings.Round;
            var series = settings.Series;

            string folderPath = DataVM.SettingsModel.FilePath;
            string eventName = series + round;
            string filePath = $"{folderPath}\\{eventName}.xlsx";

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            var file = new FileInfo(@$"{filePath}");

            await SaveExcelFile(team, driver, file);
        }

        private static async Task SaveExcelFile(ObservableCollection<TeamDataModel> team, ObservableCollection<DriverDataModel> driver, FileInfo file)
        {
            DeleteIfExists(file);

            using var package = new ExcelPackage(file);

            // Create new worksheets
            var ws = package.Workbook.Worksheets.Add("Drivers_Report");
            var ws_teams = package.Workbook.Worksheets.Add("Teams_Report");

            // Format Column Data Types
            //ws.Column(0).Style.Numberformat = 

            // Select & Filter
            var range = ws.Cells["A1"].LoadFromCollection(driver, true);
            var range_teams = ws_teams.Cells["A1"].LoadFromCollection(team, true);

            range.AutoFilter = true;
            range_teams.AutoFilter = true;

            var colPosition = ws.AutoFilter.Columns.AddValueFilterColumn(0);
            var colPositionTeams = ws_teams.AutoFilter.Columns.AddValueFilterColumn(0);

            //ws.Columns.AutoFit();

            // Style & Format all cells
            ws.Cells["A2:CF23"].Sort(x => x.SortBy.Column(0));
            ws_teams.Cells["A2:CF23"].Sort(x => x.SortBy.Column(1));

            ws.Cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws_teams.Cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            ws.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws_teams.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

            ws.Columns.BestFit = true;
            ws_teams.Columns.BestFit = true;

            ws.Cells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            ws.Cells.Style.Fill.BackgroundColor.SetColor(Color.White);

            ws_teams.Cells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            ws_teams.Cells.Style.Fill.BackgroundColor.SetColor(Color.White);

            // Style Header
            ws.Row(1).Style.Font.Bold = true;
            ws.Row(1).Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

            ws_teams.Row(1).Style.Font.Bold = true;
            ws_teams.Row(1).Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

            // Format Session Sheet

            await package.SaveAsync();
        }

        private static void DeleteIfExists(FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
            }
        }

        private void ModelToExcel(object sender, RoutedEventArgs e)
        {
            ModelToExcel();
        }
    }
}
