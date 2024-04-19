using System;
using System.Collections.Generic;
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
        public TotalResultWindow()
        {
            InitializeComponent();
            listview_burgerMenu.ItemsSource = EF.Context.FilmGenre.ToList();
        }
        public TotalResultWindow(Genre burgerGenre)
        {
            List<FilmLibrary> genreLibrary = new List<FilmLibrary>();

            InitializeComponent();
            listview_burgerMenu.ItemsSource = EF.Context.FilmGenre.ToList();
            foreach (var item in EF.Context.FilmGenre.Where(i => i.GenreId == burgerGenre.Id))
            {
                genreLibrary.AddRange(EF.Context.FilmLibrary.Where(i => i.Id == item.Id));
            }
            listview_newItem.ItemsSource = genreLibrary;
        }
        public TotalResultWindow(string searchPrompt)
        {
            InitializeComponent();
            listview_burgerMenu.ItemsSource = EF.Context.FilmGenre.ToList();
            if (true)
            {
                listview_newItem.ItemsSource = EF.Context.FilmLibrary.ToList().FindAll(i => i.Title.ToLower().Contains(searchPrompt.ToLower()));
            }
        }
        public TotalResultWindow(int burgerVariant)
        {
            InitializeComponent();
            listview_burgerMenu.ItemsSource = EF.Context.FilmGenre.ToList();
            switch (burgerVariant)
            {
                case 1:
                    listview_newItem.ItemsSource = EF.Context.FilmLibrary.Where(i => i.IsSeries == false).ToList();
                    break;
                case 2:
                    listview_newItem.ItemsSource = EF.Context.FilmLibrary.Where(i => i.IsSeries == true).ToList();
                    break;
                case 3:
                    break;
                default:
                    break;
            }
        }

        internal void SetListValues(List<FilmLibrary> collection)
        {
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
            SetListValues(EF.Context.FilmLibrary.ToList().FindAll(i => i.Title.ToLower().Contains(textbox_searchBar.Text)));
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

            listview_burgerMenu.ItemsSource = EF.Context.FilmGenre.ToList();
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
    }
}
