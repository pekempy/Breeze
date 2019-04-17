using GameLauncher.Models;
using GameLauncher.Properties;
using GameLauncher.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace GameLauncher.Views
{
    public partial class ListView : UserControl
    {
        DateTime dt;
        DispatcherTimer t;
        public DoubleAnimation doubleAnimation = new DoubleAnimation();
        public static string FilterGenreName;
        public string installPath = AppDomain.CurrentDomain.BaseDirectory;
        private MainWindow MainWindow = ((MainWindow)Application.Current.MainWindow);
        public CollectionViewSource GameListCVS;

        public ListView()
        {
            InitializeComponent();
            t = new DispatcherTimer();
            t.Tick += new EventHandler(t_Tick);
        }

        private void GameLink_OnClick(object sender, RoutedEventArgs e)
        {
            object link = ((Button)sender).Tag;
            string linkstring = link.ToString().Trim();

            if (linkstring != "")
            {
                Process.Start(new ProcessStartInfo(linkstring));
            }
        }
        public void UpdateColours(object sender, RoutedEventArgs e)
        {
            var converter = new BrushConverter();
            if (gameListView.Items.Count != 0)
            {
                for (int i = 0; i < gameListView.Items.Count; i++)
                {
                    ContentPresenter c = (ContentPresenter)gameListView.ItemContainerGenerator.ContainerFromItem(gameListView.Items[i]);
                    try
                    {
                        Label tb = c.ContentTemplate.FindName("GameTitle", c) as Label;
                        tb.Foreground = (Brush)converter.ConvertFromString(Settings.Default.listtitles);
                    }
                    catch (Exception br) { Trace.WriteLine("Break: " + br); break; }
                }
            }
        }
        private void LaunchButton_OnClick(object sender, RoutedEventArgs e)
        {
            object link = ((Button)sender).Tag;
            string linkString = link.ToString().Trim();
            if (linkString != "")
            {
                Process.Start(new ProcessStartInfo(installPath + "Resources/shortcuts/" + linkString));
            }
        }

        private void EditGame_OnClick(object sender, RoutedEventArgs e)
        {
            ModifyFile.EditGameInfile(((Button)sender).Tag);
        }

        private void DeleteGame_OnClick(object sender, RoutedEventArgs e)
        {
            ModifyFile.RemoveGameFromFile(((Button)sender).Tag);
            try
            {
                ModifyFile.DeleteGameImages(((Button)sender).CommandParameter.ToString());
            }
            catch (Exception exc) { Trace.WriteLine("Failed to delete images for game: " + exc); }
            string removeguid = ((Button)sender).Tag.ToString();
            foreach (var item in ListViewModel.ListViewOC.ToList())
            {
                if (removeguid == item.Guid)
                {
                    Trace.WriteLine(DateTime.Now + ": Removed Game: " + item.Title);
                    ListViewModel.ListViewOC.Remove(item);
                }
            }
        }

        public void Marquee_Start(object sender, RoutedEventArgs e)
        {
            dt = DateTime.Now;
            t.Interval = new TimeSpan(0, 0, 1);
            t.IsEnabled = true;
        }

        void t_Tick(object sender, EventArgs e)
        {
            if ((DateTime.Now - dt).Seconds >= 2)
            {
                if (gameListView.Items.Count != 0)
                {
                    for (int i = 0; i < gameListView.Items.Count; i++)
                    {
                        ContentPresenter c = (ContentPresenter)gameListView.ItemContainerGenerator.ContainerFromItem(gameListView.Items[i]);
                        Label title = c.ContentTemplate.FindName("GameTitle", c) as Label;
                        Canvas canvas = c.ContentTemplate.FindName("canvasTitle", c) as Canvas;
                        MaterialDesignThemes.Wpf.Card card = c.ContentTemplate.FindName("gameCard", c) as MaterialDesignThemes.Wpf.Card;
                        if (card.IsMouseOver == true)
                        {
                            if (title.Content.ToString().Length > 20)
                            {
                                doubleAnimation.From = 0;
                                doubleAnimation.To = canvas.ActualWidth;
                                doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
                                doubleAnimation.Duration = new Duration(TimeSpan.Parse("0:0:5"));
                                title.BeginAnimation(Canvas.RightProperty, doubleAnimation);
                            }
                        }
                    }
                }
            }
        }
        public void Marquee_Stop(object sender, RoutedEventArgs e)
        {
            if (gameListView.Items.Count != 0)
            {
                for (int i = 0; i < gameListView.Items.Count; i++)
                {
                    ContentPresenter c = (ContentPresenter)gameListView.ItemContainerGenerator.ContainerFromItem(gameListView.Items[i]);
                    Label title = c.ContentTemplate.FindName("GameTitle", c) as Label;
                    Canvas canvas = c.ContentTemplate.FindName("canvasTitle", c) as Canvas;
                    MaterialDesignThemes.Wpf.Card card = c.ContentTemplate.FindName("gameCard", c) as MaterialDesignThemes.Wpf.Card;
                    if (card.IsMouseOver != true)
                    {
                        if (title.Content.ToString().Length > 20)
                        {
                            title.BeginAnimation(Canvas.RightProperty, null);
                        }
                    }
                }
            }
        }

        //When text is changed in searchbar, apply filter
        private void SearchString_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshList();
        }

        //Hacky method to set cvs in mainwindow on a hidden button
        private void EnableFilteringCheat(object sender, RoutedEventArgs e)
        {
            GameListCVS = ((CollectionViewSource)(FindResource("GameListCVS")));
            MainWindow.cvs = GameListCVS;
            MainWindow.MenuToggleButton.IsChecked = true;
        }

        //FILTERS GAMES BASED ON THE TITLE SEARCHED
        private void GameSearch(object sender, FilterEventArgs e)
        {
            GameList gl = e.Item as GameList;
            e.Accepted &= gl.Title.ToUpper().Contains(GameSearchBar.Text.ToUpper());
        }

        //FILTERS GAMES BASED ON THE GENRE SELECTED
        public void GenreFilter(object sender, FilterEventArgs e)
        {
            GameList gl = e.Item as GameList;
            e.Accepted &= gl.Genre.ToUpper().Contains(FilterGenreName.ToUpper());
        }

        //PULLS GENRENAME FROM MAINWINDOW
        public void GenreToFilter(string filtergenrename)
        {
            //Set public variable for use in GenreFilter
            FilterGenreName = filtergenrename;
        }

        //REFRESHES LIST AFTER SEARCH TEXT
        public void RefreshList()
        {
            GameListCVS = ((CollectionViewSource)(FindResource("GameListCVS")));
            MainWindow.cvs = GameListCVS;
            if (FilterGenreName != null)
            {
                GameListCVS.Filter += new FilterEventHandler(GenreFilter);
            }
            if (GameSearchBar.Text != null)
            {
                GameListCVS.Filter += new FilterEventHandler(GameSearch);
            }
            if (GameListCVS.View != null)
                GameListCVS.View.Refresh();
        }

        //REFRESHES LIST AFTER GENRE SELECTED
        public void RefreshList2(CollectionViewSource cvscvs)
        {
            if (cvscvs != null)
            {
                GameListCVS = cvscvs;
                if (FilterGenreName != null)
                {
                    GameListCVS.Filter += new FilterEventHandler(GenreFilter);
                }
                if (GameSearchBar.Text != null)
                {
                    GameListCVS.Filter += new FilterEventHandler(GameSearch);
                }
                if (GameListCVS.View != null)
                    GameListCVS.View.Refresh();
            }
        }
    }
}