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
using System.Windows.Shapes;
using test1234.classes;

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
        }
        public TotalResultWindow(db.Genre searchValue)
        {
            InitializeComponent();

            listview_item.ItemsSource = EF.Context.FilmLibrary.
                Where(i => i.Title == (EF.Context.View_FilmGenreList.Where(j => j.Genre == searchValue.Title) as db.View_FilmGenreList).Title);


            //Where(i => i.Id ==
            //    (EF.Context.FilmGenre.Where(x => x.GenreId ==
            //        (EF.Context.Genre.Where(y => y.Title == searchValue.Title)
            //        as db.Genre).Id)
            //    as db.FilmGenre).FilmId).ToList();
        }

        private void btn_burgeropen_сlick(object sender, RoutedEventArgs e)
        {

        }

        private void btn_header_search_сlick(object sender, RoutedEventArgs e)
        {

        }
    }
}
