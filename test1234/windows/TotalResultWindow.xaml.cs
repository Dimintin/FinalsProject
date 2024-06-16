using Google.Apis.Util;
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
        private List<FilmLibrary> _filmCollection = new List<FilmLibrary>(EF.Context.FilmLibrary);
        private static int currentPageIndex = 0;
        private static int pageBurgerVariant = 0;
        private static int itemPerPage = 24;
        private static int totalPage;

        public TotalResultWindow()
        {
            InitializeComponent();
        }
        public TotalResultWindow(Genre burgerGenre)
        {
            InitializeComponent();

            List<FilmLibrary> genreLibrary = new List<FilmLibrary>();
            var genretable = EF.Context.FilmGenre.Where(i => i.GenreId == (EF.Context.Genre.Where(j => j.Title == burgerGenre.Title).FirstOrDefault() as Genre).Id);

            foreach (var item in genretable)
            {
                genreLibrary.Add(EF.Context.FilmLibrary.Where(i => i.Id == item.FilmId).FirstOrDefault());
            }
            _filmCollection = genreLibrary;
            currentPageIndex = 0;

            listview_burgerMenu.ItemsSource = EF.Context.Genre.ToList();
            textblock_login.Text = ActiveUser.activeUser.UserName;
            if (!string.IsNullOrEmpty(ActiveUser.activeUser.ProfilePhotoLink))
            {
                image_profile.ImageSource = new BitmapImage(new Uri(ActiveUser.activeUser.ProfilePhotoLink));
            }

            if (_filmCollection.Count == 0)
            {
                textblock_noValues.Visibility = Visibility.Visible;
            }
            else
            {
                textblock_noValues.Visibility = Visibility.Collapsed;
            }
            textblock_resultTitle.Text = burgerGenre.Title.Substring(0, 1).ToUpper() + burgerGenre.Title.Substring(1);
        }
        public TotalResultWindow(string searchPrompt)
        {
            InitializeComponent();

            listview_burgerMenu.ItemsSource = EF.Context.Genre.ToList();

            _filmCollection = _filmCollection.FindAll(i => i.Title.ToLower().Contains(searchPrompt.ToLower()) || !string.IsNullOrEmpty(i.EnTitle) && i.EnTitle.ToLower().Contains(searchPrompt.ToLower()));
            if (_filmCollection.Count == 0)
            {
                textblock_noValues.Visibility = Visibility.Visible;
            }
            else
            {
                textblock_noValues.Visibility = Visibility.Collapsed;
            }
            currentPageIndex = 0;
            textblock_login.Text = ActiveUser.activeUser.UserName;
            if (!string.IsNullOrEmpty(ActiveUser.activeUser.ProfilePhotoLink))
            {
                image_profile.ImageSource = new BitmapImage(new Uri(ActiveUser.activeUser.ProfilePhotoLink));
            }
            textblock_resultTitle.Text = "Результаты поиска...";
        }
        public TotalResultWindow(int burgerVariant)
        {
            InitializeComponent();

            pageBurgerVariant = burgerVariant;

            switch (burgerVariant)
            {
                case 1:
                    _filmCollection = EF.Context.FilmLibrary.Where(i => i.IsSeries == false).ToList();
                    textblock_resultTitle.Text = "Фильмы";
                    break;
                case 2:
                    _filmCollection = EF.Context.FilmLibrary.Where(i => i.IsSeries == true).ToList();
                    textblock_resultTitle.Text = "Сериалы";
                    break;
                case 3:
                    _filmCollection.Clear();
                    foreach (var item in EF.Context.UserFavorite.Where(i => i.UserId == ActiveUser.activeUser.Id).ToList())
                    {
                        _filmCollection.Add(EF.Context.FilmLibrary.Where(i => i.Id == item.FilmId).FirstOrDefault());
                    }
                    textblock_resultTitle.Text = "Избранное";
                    break;
                default:
                    break;
            }

            if (_filmCollection.Count == 0)
            {
                textblock_noValues.Visibility = Visibility.Visible;
            }
            else
            {
                textblock_noValues.Visibility = Visibility.Collapsed;
            }
            currentPageIndex = 0;

            listview_burgerMenu.ItemsSource = EF.Context.Genre.ToList();
            textblock_login.Text = ActiveUser.activeUser.UserName;
            if (!string.IsNullOrEmpty(ActiveUser.activeUser.ProfilePhotoLink))
            {
                image_profile.ImageSource = new BitmapImage(new Uri(ActiveUser.activeUser.ProfilePhotoLink));
            }
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
                _filmCollection = EF.Context.FilmLibrary.ToList().FindAll(i => i.Title.ToLower().Contains(textbox_searchBar.Text.ToLower()) || !string.IsNullOrEmpty(i.EnTitle) && i.EnTitle.ToLower().Contains(textbox_searchBar.Text.ToLower()));
                textblock_resultTitle.Text = "Результаты поиска...";
            }

            if (_filmCollection.Count == 0)
            {
                textblock_noValues.Visibility = Visibility.Visible;
            }
            else
            {
                textblock_noValues.Visibility = Visibility.Collapsed;
            }
            currentPageIndex = 0;
            view_Filter();
        }

        public void closeBurgerMenu()
        {
            grid_main.Effect = null;
            grid_main.IsHitTestVisible = true;

            borderBurger.Visibility = Visibility.Collapsed;
        }

        private void button_closeBurgerMenu_Click(object sender, RoutedEventArgs e)
        {
            closeBurgerMenu();
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
            var genretable = EF.Context.FilmGenre.Where(i => i.GenreId == (EF.Context.Genre.Where(j => j.Title == objectGenre.Title).FirstOrDefault() as Genre).Id);

            foreach (var item in genretable)
            {
                genreLibrary.Add(EF.Context.FilmLibrary.Where(i => i.Id == item.FilmId).FirstOrDefault());
            }
            _filmCollection = genreLibrary;
            textblock_resultTitle.Text = objectGenre.Title.Substring(0,1).ToUpper() + objectGenre.Title.Substring(1);


            if (_filmCollection.Count == 0)
            {
                textblock_noValues.Visibility = Visibility.Visible;
            }
            else
            {
                textblock_noValues.Visibility = Visibility.Collapsed;
            }

            currentPageIndex = 0;
            _filmCollection = genreLibrary;
            view_Filter();

            closeBurgerMenu();
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
            if (pageBurgerVariant == 3)
            {
                _filmCollection.Clear();
                foreach (var item in EF.Context.UserFavorite.Where(i => i.UserId == ActiveUser.activeUser.Id).ToList())
                {
                    _filmCollection.Add(EF.Context.FilmLibrary.Where(i => i.Id == item.FilmId).FirstOrDefault());
                }
            }
            closeBurgerMenu();
        }

        private void button_searchDiscardValue_Click(object sender, RoutedEventArgs e)
        {
            textbox_searchBar.Text = "";
            button_searchDiscardValue.Visibility = Visibility.Collapsed;
            if (pageBurgerVariant != 0)
            {
                switch (pageBurgerVariant)
                {
                    case 1:
                        _filmCollection = EF.Context.FilmLibrary.Where(i => i.IsSeries == false).ToList();
                        textblock_resultTitle.Text = "Фильмы";
                        break;
                    case 2:
                        _filmCollection = EF.Context.FilmLibrary.Where(i => i.IsSeries == true).ToList();
                        textblock_resultTitle.Text = "Сериалы";
                        break;
                    case 3:
                        _filmCollection.Clear();
                        foreach (var item in EF.Context.UserFavorite.Where(i => i.UserId == ActiveUser.activeUser.Id).ToList())
                        {
                            _filmCollection.Add(EF.Context.FilmLibrary.Where(i => i.Id == item.FilmId).FirstOrDefault());
                        }
                        textblock_resultTitle.Text = "Избранное";
                        break;
                    default:
                        break;
                }
            }

            if (_filmCollection.Count == 0)
            {
                textblock_noValues.Visibility = Visibility.Visible;
            }
            else
            {
                textblock_noValues.Visibility = Visibility.Collapsed;
            }

            currentPageIndex = 0;
            view_Filter();
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

        // ПАГИНАЦИЯ

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            view_Filter();
        }

        void view_Filter()
        {
            List<FilmLibrary> _pageFilms = new List<FilmLibrary>(); // лист для содержимого открытой страницы

            totalPage = (_filmCollection.Count / itemPerPage); // подсчет количества необходимых страниц

            if (_filmCollection .Count % itemPerPage != 0 || _filmCollection.Count == 0) // если список не идеальный,
            {                                                                            // то добавляется страница для отсатка
                totalPage += 1;
            }

            textblock_TotalPage.Text = totalPage.ToString();
            textblock_Page.Text = (currentPageIndex + 1).ToString();

            // Далее из результирующего списка получают необходимые элементы.
            // Если страница последнняя, то лишь отсаток от полного деления каталога на количество страниц

            for (int i = currentPageIndex * itemPerPage; i < (currentPageIndex * itemPerPage) +                  
                ((currentPageIndex + 1) == totalPage ? (_filmCollection.Count % itemPerPage) : itemPerPage); i++)
            {                                                                                                    
                _pageFilms.Add(_filmCollection[i]);                                                              
            }
            listview_newItem.ItemsSource = _pageFilms; // количество необходимых элементов на вывод
        }

        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageIndex != 0)
            {
                currentPageIndex = 0;
                view_Filter();
            }
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageIndex > 0)
            {
                currentPageIndex--;
                view_Filter();
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if ((currentPageIndex + 1) < totalPage)
            {
                currentPageIndex++;
                view_Filter();
            }
        }

        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageIndex + 1 != totalPage)
            {
                currentPageIndex = totalPage - 1;
                view_Filter();
            }
        }

        private void textblock_filmlink_Click(object sender, MouseButtonEventArgs e)
        {
            _filmCollection = EF.Context.FilmLibrary.Where(i => i.IsSeries == false).ToList();

            if (_filmCollection.Count == 0)
            {
                textblock_noValues.Visibility = Visibility.Visible;
            }
            else
            {
                textblock_noValues.Visibility = Visibility.Collapsed;
            }
            textblock_resultTitle.Text = "Фильмы";

            currentPageIndex = 0;
            view_Filter();
            closeBurgerMenu();
        }

        private void textblock_serieslink_Click(object sender, MouseButtonEventArgs e)
        {
            _filmCollection = EF.Context.FilmLibrary.Where(i => i.IsSeries == true).ToList();

            if (_filmCollection.Count == 0)
            {
                textblock_noValues.Visibility = Visibility.Visible;
            }
            else
            {
                textblock_noValues.Visibility = Visibility.Collapsed;
            }
            textblock_resultTitle.Text = "Сериалы";

            currentPageIndex = 0;
            view_Filter();
            closeBurgerMenu();
        }

        private void textblock_favlink_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _filmCollection.Clear();
            foreach (var item in EF.Context.UserFavorite.Where(i => i.UserId == ActiveUser.activeUser.Id).ToList())
            {
                _filmCollection.Add(EF.Context.FilmLibrary.Where(i => i.Id == item.FilmId).FirstOrDefault());
            }
            textblock_resultTitle.Text = "Избранное";
            currentPageIndex = 0;
            view_Filter();
            closeBurgerMenu();
        }

        private void button_user_Click(object sender, MouseButtonEventArgs e)
        {
            UserDataWindow userDataWindow = new UserDataWindow();
            userDataWindow.ShowDialog();
            textblock_login.Text = ActiveUser.activeUser.UserName;
            image_profile.ImageSource = new BitmapImage(new Uri(ActiveUser.activeUser.ProfilePhotoLink));
        }
    }
}
