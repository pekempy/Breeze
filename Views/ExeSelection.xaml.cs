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
using GameLauncher.Models;
using GameLauncher.ViewModels;
using System.Globalization;
using Microsoft.Win32;
using System.IO;
using IWshRuntimeLibrary;
using GameLauncher.Properties;
using MaterialDesignThemes.Wpf;
using System.Diagnostics;

namespace GameLauncher.Views
{
    public partial class ExeSelection : Page
    {
        public AutoImage ai = new AutoImage();
        public static ExeSelection es;
        public static ExeSearch exs = new ExeSearch();
        public List<string> ExeList = new List<string>();
        private MainWindow MainWindow = ((MainWindow)Application.Current.MainWindow);
        private bool matchFound = false;
        private bool matchExe = false;
        public string title;
        public string selectedExe;

        public ExeSelection()
        {
            es = this;
            InitializeComponent();
            if (Settings.Default.theme.ToString() == "Dark")
            {
                ThemeAssist.SetTheme(this, BaseTheme.Dark);
            }
            else if (Settings.Default.theme.ToString() == "Light")
            {
                ThemeAssist.SetTheme(this, BaseTheme.Light);
            }
        }

        public void CloseExeSelection(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow)?.CloseExeSearchDialog();
        }
        public static void ChangeWindowSize(double height, double width)
        {
            es.ExeGrid.Height = ((MainWindow)Application.Current.MainWindow).ActualHeight * 0.9;
            es.ExeGrid.Width = ((MainWindow)Application.Current.MainWindow).ActualWidth * 0.9;
        }
        protected void UILoaded(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Resized();
        }
        private void RadioButton_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                selectedExe = ((RadioButton)sender).Tag.ToString();
                title = ((RadioButton)sender).CommandParameter.ToString();
            }
            catch {
                Trace.WriteLine(DateTime.Now + ": Couldn't Select?");
            }

            for (int i = 0; i < ExeList.Count; i++)
            {
                if (ExeList[i].Contains(title + ";"))
                {
                    matchFound = true;
                    if (!ExeList[i].Contains(";" + selectedExe))
                    {
                        ExeList[i] = title + ";" + selectedExe;
                    }
                    else { Trace.WriteLine(DateTime.Now + ": RadioButtonSelected-Dupe"); }
                    matchFound = false;
                }
            }
            if (matchFound == false)
            {
                for (int i = 0; i < ExeList.Count; i++)
                {
                    if (!ExeList[i].Contains(title + ";"))
                        {
                        if (!ExeList[i].Contains(";" + selectedExe))
                        {
                            matchExe = false;
                        }
                        else if (ExeList.Contains(";" + selectedExe)) { matchExe = true; }
                    }
                    else if (ExeList[i].Contains(title + ";")) { matchExe = true; }
                    if (ExeList[i].Contains(";" + selectedExe)) { matchExe = true; }
                }
                if (ExeList.Count == 0 || matchExe == false)
                {
                    ExeList.Add(title + ";" + selectedExe);
                }
            }
        }
        private void ManualLauncher(object sender, RoutedEventArgs e)
        {
            string title = ((Button)sender).Tag.ToString();
            string exe;
            string newgame;
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.RestoreDirectory = true;
            fileDialog.Filter = "Executable Files (*.exe) | *.exe;*.lnk;*.url";
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult == true)
            {
                exe = fileDialog.FileName;
                newgame = title + ";" + exe;
                for (int i = 0; i<ExeList.Count; i++)
                {
                    if (ExeList[i].Contains(title + ";")){
                        matchFound = true;
                        if (!ExeList[i].Contains(";" + exe))
                        {
                            ExeList[i] = newgame;
                        }
                        else { Trace.WriteLine(DateTime.Now + ": ManualLauncher - Duplicate"); }
                        matchFound = false;
                    }
                }
                if (matchFound == false)
                {
                    ExeList.Add(newgame);
                }
                ((MainWindow)Application.Current.MainWindow).UpdateObsCol(title, exe);
            }
        }

        private void CardLoaded(object sender, RoutedEventArgs e)
        {
            string title;
            for (int i = 0; i < ExeListings.Items.Count; i++)
            {
                ContentPresenter c = (ContentPresenter)ExeListings.ItemContainerGenerator.ContainerFromItem(ExeListings.Items[i]);
                TextBlock tb = c.ContentTemplate.FindName("Title", c) as TextBlock;
                RadioButton rb = c.ContentTemplate.FindName("R1", c) as RadioButton;
                title = tb.Text.ToString();
                bool result = ((MainWindow)Application.Current.MainWindow).CheckBinding(title);
                if (result == true)
                {
                    rb.IsChecked = true;
                }
            }
        }

        private void UpdateObsCol(string title, string exe)
        {
            exs.UpdateObsCol(title, exe);
            
        }
        private void AcceptExeSelection_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var item in ExeList)
            {
                TextWriter tw = new StreamWriter(@"./Resources/GamesList.txt", true);
                Guid gameGuid = Guid.NewGuid();
                string[] gameitems = item.Split(';');
                string title = gameitems[0];
                string exe = gameitems[1];
                string installPath = AppDomain.CurrentDomain.BaseDirectory;
                string fileNameIcon = ai.AutoDownloadImages(title, "icon");
                string fileNamePoster = ai.AutoDownloadImages(title, "poster");
                string fileNameBanner = ai.AutoDownloadImages(title, "banner");
                string icon = fileNameIcon;
                string poster = fileNamePoster;
                string banner = fileNameBanner;
                string shortcut = createShortcut(title, exe);
                string game = title + "||" + shortcut + "||" + icon + "|" + poster + "|" + banner + "|" + gameGuid;
                tw.WriteLine(game);
                tw.Close();
            }
            ((MainWindow)Application.Current.MainWindow).CloseExeSearchDialog();
        }
        private string createShortcut(string title, string exe)
        {
            string installPath = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(installPath + "\\Resources\\shortcuts"))
            {
                System.IO.Directory.CreateDirectory(installPath + "\\Resources\\shortcuts");
            }
            WshShell wsh = new WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut = wsh.CreateShortcut(
            installPath + "\\Resources\\shortcuts" + "\\" + title + ".lnk") as IWshRuntimeLibrary.IWshShortcut;
            shortcut.Arguments = "";
            shortcut.TargetPath = exe;
            shortcut.WindowStyle = 1;
            shortcut.Description = "Shortcut to " + title;
            shortcut.WorkingDirectory = "C:\\App";
            shortcut.IconLocation = exe;
            shortcut.Save();
            return installPath + "\\Resources\\shortcuts" + "\\" + title + ".lnk";
        }
    }
    
    public sealed class Null2Visibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public sealed class Null2VisibilityInvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
