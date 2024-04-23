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
using System.Windows.Media.Animation;

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
            listview_newItem.ItemsSource = filmList.Where(i => i.WorldPremiereDate > DateTime.Now.AddYears(-4));
            listview_favItem.ItemsSource = filmList.Where(i => i.WorldPremiereDate > DateTime.Now.AddYears(-4));
            listview_burgerMenu.ItemsSource = EF.Context.Genre.ToList();
            textblock_login.Text = ActiveUser.activeUser.UserName;
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
                TotalResultWindow totalResultWindow = new TotalResultWindow(textbox_searchBar.Text);
                totalResultWindow.Show();
                this.Close();
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
            TotalResultWindow totalResultWindow = new TotalResultWindow(1);
            totalResultWindow.Show();
            this.Close();
        }

        private void textblock_serieslink_Click(object sender, RoutedEventArgs e)
        {
            TotalResultWindow totalResultWindow = new TotalResultWindow(2);
            totalResultWindow.Show();
            this.Close();
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var button = sender as TextBlock;
            if (button == null)
            {
                return;
            }
            var objectGenre = button.DataContext as Genre;
            TotalResultWindow totalResultWindow = new TotalResultWindow(objectGenre);
            totalResultWindow.Show();
            this.Close();
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
        }

        private void textbox_searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textbox_searchBar.Text.Length != 0)
            {
                button_searchDiscardValue.Visibility = Visibility.Visible;
            }
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            ScrollViewer scrollViewer = GetChildOfType<ScrollViewer>(listview_newItem);
            scrollViewer.LineRight();
        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            ScrollViewer scrollViewer = GetChildOfType<ScrollViewer>(listview_newItem);
            scrollViewer.LineLeft();
        }

        private T GetChildOfType<T>(Visual listview_item) where T : Visual
        {
            T child = default;
            int numVisuals = VisualTreeHelper.GetChildrenCount(listview_item);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(listview_item, i);
                child = v as T;
                if (child == null)
                {
                    child = GetChildOfType<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        private void textblock_mainpagelink_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Show();
        }



        // ЭЛЕМЕНТЫ ДЛЯ КАРУСЕЛИ LISTVIEW


    }
}
