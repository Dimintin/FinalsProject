using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Runtime.InteropServices.ComTypes;

namespace test1234.windows
{
    /// <summary>
    /// Логика взаимодействия для FilmWindow.xaml
    /// </summary>
    public partial class FilmWindow : Window
    {
        FilmLibrary pageFilm;
        public FilmWindow()
        {
            InitializeComponent();
        }

        public FilmWindow(FilmLibrary filmObject)
        {
            InitializeComponent();

            pageFilm = filmObject;

            image_filmPoster.Source = new BitmapImage(new Uri(filmObject.PromoPosterLink));
            textblock_filmTitle.Text = filmObject.Title;
            textblock_filmEnglishTitle.Text = filmObject.EnTitle;
            textblock_filmDescription.Text = filmObject.ShortDecription;
            textblock_ageRestriction.Text = filmObject.AgeRestriction.HasValue ? filmObject.AgeRestriction.ToString() + '+' : "-";
            textblock_russiaPremiere.Text = filmObject.RussiaPremiereDate.HasValue ? filmObject.RussiaPremiereDate.Value.ToLongDateString() : "-";
            textblock_worldPremiere.Text = filmObject.WorldPremiereDate.HasValue ? filmObject.WorldPremiereDate.Value.ToLongDateString() : "-";
            textblock_budget.Text = filmObject.BudgetNum.HasValue ? filmObject.BudgetNum.ToString() + EF.Context.Currency.Where(i => i.Id == filmObject.CurrencyId).FirstOrDefault().CurrencySymb : "-";

            foreach (var item in EF.Context.FilmGenre.Where(i => i.FilmId == filmObject.Id))
            {
                textblock_genrese.Text += (EF.Context.Genre.Where(g => g.Id == item.GenreId).FirstOrDefault()).Title + ", ";
            }
            textblock_genrese.Text = textblock_genrese.Text.TrimEnd(',', ' ');

            foreach (var item in EF.Context.FilmCountry.Where(i => i.FilmId == filmObject.Id))
            {
                textblock_country.Text += (EF.Context.Country.Where(g => g.Id == item.CountryId).FirstOrDefault()).Title + ", ";
            }
            textblock_country.Text = textblock_country.Text.TrimEnd(',', ' ');

            textblock_FullDescription.Text = filmObject.FullDescription;

            if (filmObject.IsSeries == true)
            {
                stackPanel_filmSeries.Visibility = Visibility.Visible;
                listView_filmSeriesSeason.ItemsSource = EF.Context.SeriesSeason.Include(i => i.SeriesEpisode).Where(i => i.FilmId == filmObject.Id).ToList();
            }

            listview_burgerMenu.ItemsSource = EF.Context.Genre.ToList();

            listview_review.ItemsSource = EF.Context.View_UserReviews.Where(i => i.FilmId == filmObject.Id).ToList();

            textblock_login.Text = ActiveUser.activeUser.UserName;

            if (!string.IsNullOrEmpty(ActiveUser.activeUser.ProfilePhotoLink)) 
            {
                image_profile.ImageSource = new BitmapImage(new Uri(ActiveUser.activeUser.ProfilePhotoLink));
            }

            if (EF.Context.UserView.Where(i => i.FilmId == filmObject.Id && i.UserId == ActiveUser.activeUser.Id).ToList().Count != 0)
            {
                button_createReview.IsEnabled = true;
            }

            if (EF.Context.View_UserReviews.Where(i => i.UserName == ActiveUser.activeUser.UserName && i.FilmId == filmObject.Id).ToList().Count != 0)
            {
                grid_review.Visibility = Visibility.Visible;
                textbox_reviewTitle.Text = (EF.Context.View_UserReviews.Where(i => i.UserName == ActiveUser.activeUser.UserName && i.FilmId == filmObject.Id).FirstOrDefault().ReviewTitle);
                textbox_reviewText.Text = (EF.Context.View_UserReviews.Where(i => i.UserName == ActiveUser.activeUser.UserName && i.FilmId == filmObject.Id).FirstOrDefault().ReviewText);
                textbox_reviewText.IsReadOnly = true;
                textbox_reviewTitle.IsReadOnly = true;
                button_discardReview.Visibility = Visibility.Visible;
                button_editReview.Visibility = Visibility.Visible;
                stackPanel_saveOrDiscardReview.Visibility = Visibility.Visible;
            }

            if (EF.Context.View_FilmProduction.Where(i => i.FilmId == filmObject.Id).ToList().Count != 0)
            {
                stackpanel_production.Visibility = Visibility.Visible;
                listView_production.ItemsSource = EF.Context.View_FilmProduction.Where(f => f.FilmId == pageFilm.Id).ToList();
            }
        }

        //BURGER MENU CODE
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
        private void button_closeBurgerMenu_Click(object sender, RoutedEventArgs e)
        {
            grid_main.Effect = null;
            grid_main.IsHitTestVisible = true;

            borderBurger.Visibility = Visibility.Collapsed;
        }
        private void textblock_filmlink_Click(object sender, RoutedEventArgs e)
        {
            TotalResultWindow totalResultWindow = new TotalResultWindow(2);
            totalResultWindow.Show();
            this.Close();
        }
        private void textblock_serieslink_Click(object sender, RoutedEventArgs e)
        {
            TotalResultWindow totalResultWindow = new TotalResultWindow(1);
            totalResultWindow.Show();
            this.Close();
        }
        private void textblock_mainpagelink_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        //SEARCH BOX
        private void btn_header_search_сlick(object sender, RoutedEventArgs e)
        {

            TotalResultWindow totalResultWindow = new TotalResultWindow(textbox_searchBar.Text);
            totalResultWindow.Show();
            this.Close();
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

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            TotalResultWindow totalResultWindow = new TotalResultWindow();
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

        private void StackPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void button_filmToFav_Click(object sender, RoutedEventArgs e)
        {
            if (EF.Context.UserFavorite.Where(i => i.UserId == ActiveUser.activeUser.Id && i.FilmId == pageFilm.Id).ToList().Count == 0)
            {
                UserFavorite userFavorite = new UserFavorite
                {
                    Id = EF.Context.UserFavorite.Max(i => i.Id) + 1,
                    UserId = ActiveUser.activeUser.Id,
                    FilmId = pageFilm.Id
                };

                EF.Context.UserFavorite.Add(userFavorite);
                EF.Context.SaveChanges();

                image_favIcon.Source = new BitmapImage(new Uri(new Uri(Directory.GetCurrentDirectory(), UriKind.Absolute), new Uri(@"..\img\icons8-heart-filled-48.png", UriKind.Relative)));
            }
            else
            {
                UserFavorite userFavoriteRemove = new UserFavorite();
                userFavoriteRemove = EF.Context.UserFavorite.Where(i => i.UserId == ActiveUser.activeUser.Id && i.FilmId == pageFilm.Id).FirstOrDefault();

                EF.Context.UserFavorite.Remove(userFavoriteRemove);
                EF.Context.SaveChanges();

                image_favIcon.Source = new BitmapImage(new Uri(new Uri(Directory.GetCurrentDirectory(), UriKind.Absolute), new Uri(@"..\img\icons8-heart-hollow-48.png", UriKind.Relative)));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (EF.Context.UserFavorite.Where(i => i.UserId == ActiveUser.activeUser.Id && i.FilmId == pageFilm.Id).ToList().Count != 0)
            {
                image_favIcon.Source = new BitmapImage(new Uri(new Uri(Directory.GetCurrentDirectory(), UriKind.Absolute), new Uri(@"..\img\icons8-heart-filled-48.png", UriKind.Relative)));
            }
            else
            {
                image_favIcon.Source = new BitmapImage(new Uri(new Uri(Directory.GetCurrentDirectory(), UriKind.Absolute), new Uri(@"..\img\icons8-heart-hollow-48.png", UriKind.Relative)));
            }
        }

        private void textblock_favlink_Click(object sender, MouseButtonEventArgs e)
        {
            TotalResultWindow totalResultWindow = new TotalResultWindow(3);
            totalResultWindow.ShowDialog();
            this.Close();
        }

        private void button_user_Click(object sender, MouseButtonEventArgs e)
        {
            UserDataWindow userDataWindow = new UserDataWindow();
            userDataWindow.ShowDialog();
        }

        private void button_watchButton_Click(object sender, RoutedEventArgs e)
        {
            UserView view = new UserView();

            view.Id = EF.Context.UserView.Max(i => i.Id) + 1;
            view.UserId = ActiveUser.activeUser.Id;
            view.FilmId = pageFilm.Id;
            view.ViewTime = DateTime.Now;

            EF.Context.UserView.Add(view);
            EF.Context.SaveChanges();

            button_createReview.IsEnabled = true;
        }

        private void button_createReview_Click(object sender, RoutedEventArgs e)
        {
            grid_review.Visibility = Visibility.Visible;
            stackPanel_saveOrDiscardReview.Visibility = Visibility.Visible;
        }

        private void button_postReview_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(textbox_reviewTitle.Text) && !string.IsNullOrEmpty(textbox_reviewText.Text))
            {
                TextReview text1 = EF.Context.TextReview.Include(i => i.UserView).Where(i => i.UserView.UserId == ActiveUser.activeUser.Id).FirstOrDefault();
                TextReview text = EF.Context.TextReview.Where(i => i.Id == EF.Context.View_UserReviews.Where(j => j.Id == ActiveUser.activeUser.Id).FirstOrDefault().ReviewId).FirstOrDefault();

                if (!EF.Context.TextReview.Include(i => i.UserView).Any(j => j.UserView.UserId == ActiveUser.activeUser.Id && j.UserView.FilmId == pageFilm.Id))
                {
                    TextReview textReview = new TextReview();

                    textReview.Id = EF.Context.TextReview.Max(i => i.Id) + 1;
                    textReview.UserViewId = EF.Context.UserView.Where(i => i.UserId == ActiveUser.activeUser.Id && i.FilmId == pageFilm.Id).Max(i => i.Id);
                    textReview.ReviewTitle = textbox_reviewTitle.Text;
                    textReview.ReviewText = textbox_reviewText.Text;
                    textReview.ReviewTime = DateTime.Now;

                    EF.Context.TextReview.Add(textReview);
                }
                else
                {
                    TextReview textReview = EF.Context.TextReview.Include(i => i.UserView).Where(j => j.UserView.UserId == ActiveUser.activeUser.Id && j.UserView.FilmId == pageFilm.Id).FirstOrDefault();
                    textReview.ReviewTitle = textbox_reviewTitle.Text;
                    textReview.ReviewText = textbox_reviewText.Text;
                    textReview.ReviewTime = DateTime.Now;
                }

                EF.Context.SaveChanges();
                MessageBox.Show("Отзыв успешно сохранен", "Отзыв сохранен", MessageBoxButton.OK, MessageBoxImage.Information);

                textbox_reviewTitle.IsReadOnly = true;
                textbox_reviewText.IsReadOnly = true;
                button_createReview.IsEnabled = false;
                button_editReview.Visibility = Visibility.Visible;
            }
        }

        private void button_discardReview_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(string.Format("Вы уверены, что хотите удалить отзыв на \"{0}\"?", pageFilm.Title), "Удаление отзыва", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                textbox_reviewText.Text = "";
                textbox_reviewTitle.Text = "";

                if (EF.Context.TextReview.Include(i => i.UserView).Any(j => j.UserView.UserId == ActiveUser.activeUser.Id && j.UserView.FilmId == pageFilm.Id))
                {
                    TextReview textReview = EF.Context.TextReview.Include(i => i.UserView).Where(i => i.UserView.FilmId == pageFilm.Id && i.UserView.UserId == ActiveUser.activeUser.Id).FirstOrDefault();

                    EF.Context.TextReview.Remove(textReview);
                    EF.Context.SaveChanges();

                    listview_review.ItemsSource = EF.Context.View_UserReviews.Where(i => i.FilmId == pageFilm.Id).ToList();

                    button_createReview.IsEnabled = true;
                    textbox_reviewTitle.IsReadOnly = false;
                    textbox_reviewText.IsReadOnly = false;
                    button_editReview.Visibility = Visibility.Collapsed;
                }

            }
        }
        private void button_editReview_Click(object sender, RoutedEventArgs e)
        {
            textbox_reviewTitle.IsReadOnly = false;
            textbox_reviewText.IsReadOnly = false;
        }
    }
}
