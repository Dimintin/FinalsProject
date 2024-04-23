using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
using System.Windows.Shapes;
using test1234.classes;
using test1234.db;

namespace test1234.windows
{
    /// <summary>
    /// Логика взаимодействия для TotalResultWindow.xaml
    /// </summary>
    public partial class TotalResultWindow : Window
    {
        ObservableCollection<FilmLibrary> _pageFilms = new ObservableCollection<FilmLibrary>();
        ObservableCollection<FilmLibrary> _tempTable = new ObservableCollection<FilmLibrary>(EF.Context.FilmLibrary);
        public static int currentPageIndex = 1;
        public static int itemPerPage = 24;
        public static int totalPage;

        public TotalResultWindow()
        {
            InitializeComponent();
        }
        public TotalResultWindow(Genre burgerGenre)
        {
            List<FilmLibrary> genreLibrary = new List<FilmLibrary>();

            InitializeComponent();

            foreach (var item in EF.Context.FilmGenre.Where(i => i.GenreId == burgerGenre.Id))
            {
                genreLibrary.AddRange(EF.Context.FilmLibrary.Where(i => i.Id == item.Id));
            }
            listview_newItem.ItemsSource = genreLibrary;

            listview_burgerMenu.ItemsSource = EF.Context.Genre.ToList();
        }
        public TotalResultWindow(string searchPrompt)
        {
            InitializeComponent();

            if (EF.Context.FilmLibrary.ToList().FindAll(i => i.Title.ToLower().Contains(searchPrompt.ToLower())).Count() == 0)
            {
                textblock_noValues.Visibility = Visibility.Visible;
            }
            listview_newItem.ItemsSource = EF.Context.FilmLibrary.ToList().FindAll(i => i.Title.ToLower().Contains(searchPrompt.ToLower()));

            listview_burgerMenu.ItemsSource = EF.Context.Genre.ToList();
        }
        public TotalResultWindow(int burgerVariant)
        {
            InitializeComponent();

            switch (burgerVariant)
            {
                case 1:
                    listview_newItem.ItemsSource = EF.Context.FilmLibrary.Where(i => i.IsSeries == false && i.Id < 30).ToList();
                    break;
                case 2:
                    listview_newItem.ItemsSource = EF.Context.FilmLibrary.Where(i => i.IsSeries == true).ToList();
                    break;
                case 3:
                    break;
                default:
                    break;
            }

            listview_burgerMenu.ItemsSource = EF.Context.Genre.ToList();
        }

        internal void SetListValues(List<FilmLibrary> collection)
        {
            textblock_noValues.Visibility = Visibility.Collapsed;
            listview_newItem.ItemsSource = collection;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            borderBurger.IsEnabled = false;
            borderBurger.Visibility = Visibility.Hidden;
            grid_main.IsEnabled = true;
        }

        private void btn_burgeropen_сlick(object sender, RoutedEventArgs e)
        {
            borderBurger.Visibility = Visibility.Visible;
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            objBlur.Radius = 4;
            grid_main.Effect = objBlur;
            grid_main.IsHitTestVisible = false;
        }

        private void btn_header_search_сlick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(textbox_searchBar.Text))
            {
                SetListValues(EF.Context.FilmLibrary.ToList().FindAll(i => i.Title.ToLower().Contains(textbox_searchBar.Text)));
            }
        }

        private void button_closeBurgerMenu_Click(object sender, RoutedEventArgs e)
        {
            grid_main.Effect = null;
            grid_main.IsHitTestVisible = true;

            borderBurger.Visibility = Visibility.Collapsed;
        }

        private void textblock_filmlink_Click(object sender, RoutedEventArgs e)
        {
            SetListValues(EF.Context.FilmLibrary.Where(i => i.IsSeries == false).ToList());
        }

        private void textblock_serieslink_Click(object sender, RoutedEventArgs e)
        {
            SetListValues(EF.Context.FilmLibrary.Where(i => i.IsSeries == true).ToList());
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var button = sender as TextBlock;
            if (button == null)
            {
                return;
            }
            var objectGenre = button.DataContext as Genre;


            List<FilmLibrary> genreLibrary = new List<FilmLibrary>();

            listview_burgerMenu.ItemsSource = EF.Context.Genre.ToList();
            foreach (var item in EF.Context.FilmGenre.Where(i => i.GenreId == objectGenre.Id))
            {
                genreLibrary.AddRange(EF.Context.FilmLibrary.Where(i => i.Id == item.Id));
            }

            SetListValues(genreLibrary);
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var element = sender as Grid;
            if (element == null)
            {
                return;
            }
            var filmObject = element.DataContext as FilmLibrary;
            FilmWindow filmWindow = new FilmWindow(filmObject);
            filmWindow.ShowDialog();
        }

        private void button_searchDiscardValue_Click(object sender, RoutedEventArgs e)
        {
            textbox_searchBar.Text = "";
            button_searchDiscardValue.Visibility = Visibility.Collapsed;
            SetListValues(EF.Context.FilmLibrary.ToList());
        }

        private void textbox_searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textbox_searchBar.Text.Length != 0)
            {
                button_searchDiscardValue.Visibility = Visibility.Visible;
            }
        }

        private void textblock_mainpagelink_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }





        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //int totalPage = EF.Context.FilmLibrary.Count() / itemPerPage;
            //if (EF.Context.FilmLibrary.Count() % itemPerPage != 0)
            //{
            //    totalPage += 1;
            //}
            //view_Filter();
        }

        void view_Filter()
        {
            textblock_TotalPage.Text = currentPageIndex.ToString();
            for (int i = 0; i < currentPageIndex * itemPerPage && i > currentPageIndex * itemPerPage; i++)
            {
                _pageFilms.Add(_tempTable[i]);
            }
            listview_newItem.ItemsSource = _pageFilms;
        }

        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            // Display the first page
            //if (currentPageIndex != 1)
            //{
            //    currentPageIndex = 1;
            //}
            //view_Filter();
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            // Display previous page
            //if (currentPageIndex > 1)
            //{
            //    currentPageIndex--;
            //}
            //view_Filter();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            // Display next page
            //currentPageIndex++;
            //view_Filter();
        }

        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            // Display the last page
            //if (currentPageIndex != totalPage)
            //{
            //    currentPageIndex = totalPage;
            //}
            //view_Filter();
        }
    }
}
