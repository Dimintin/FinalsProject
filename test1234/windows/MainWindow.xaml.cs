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
using System.Collections.ObjectModel;
using test1234.classes;
using test1234.db;
using System.Globalization;
using test1234.windows;

namespace test1234
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<FilmLibrary> filmList { set; get; } = new List<FilmLibrary>(EF.Context.FilmLibrary);

        public MainWindow()
        {
            InitializeComponent();
            listview_item.ItemsSource = filmList.ToList();
            listview_burgerMenu.ItemsSource = EF.Context.Genre.ToList();
        }

        internal void SetListValues(List<FilmLibrary> collection)
        {
            textblock_emplyListWarning.Visibility = Visibility.Collapsed;
            filmList = collection;
            listview_item.ItemsSource = filmList;
            if (filmList.Count == 0)
            {
                textblock_emplyListWarning.Visibility = Visibility.Visible;
            }
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
            SetListValues(filmList.FindAll(i => i.Title.ToLower().Contains(textbox_searchBar.Text)));
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
    }
}
