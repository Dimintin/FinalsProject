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
    /// Логика взаимодействия для FilmWindow.xaml
    /// </summary>
    public partial class FilmWindow : Window
    {
        List<FilmLibrary> filmList { set; get; } = new List<FilmLibrary>(EF.Context.FilmLibrary);

        public FilmWindow()
        {
            InitializeComponent();
        }
        public FilmWindow(FilmLibrary filmObject)
        {
            InitializeComponent();

            image_filmPoster.Source = new BitmapImage(new Uri(filmObject.PromoPosterLink));
            textblock_filmTitle.Text = filmObject.Title;
            textblock_filmEnglishTitle.Text = filmObject.EnTitle;
            textblock_filmDescription.Text = filmObject.ShortDecription;
            textblock_ageRestriction.Text = filmObject.AgeRestriction.HasValue ? filmObject.AgeRestriction.ToString() + '+' : "-";
            textblock_russiaPremiere.Text = filmObject.RussiaPremiereDate.HasValue ? filmObject.RussiaPremiereDate.Value.ToLongDateString() : "-";
            textblock_worldPremiere.Text = filmObject.WorldPremiereDate.HasValue ? filmObject.WorldPremiereDate.Value.ToLongDateString() : "-";
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

            }

            listview_burgerMenu.ItemsSource = EF.Context.Genre.ToList();

            listview_trailer.ItemsSource = EF.Context.PromoTrailer.Where(i => i.FilmId == filmObject.Id).ToList();


            List<TextReview> textReviews = new List<TextReview>();
            foreach (var item in EF.Context.UserView.Where(i => i.FilmId == filmObject.Id /*|| i.EpisodeId == (EF.Context.SeriesEpisode.Where(k => k.EpisodeId == (EF.Context.SeriesSeason.Where(j => j.FilmId == filmObject.Id) as SeriesSeason).Id) as SeriesEpisode).Id*/))
            {
                textReviews.Add(EF.Context.TextReview.Where(k => k.UserViewId == item.Id).FirstOrDefault() as TextReview);
            }
            listview_textReview.ItemsSource = textReviews;
        }


        internal void SetListValues(List<FilmLibrary> collection)
        {
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

        private void StackPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void textblock_mainpagelink_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
