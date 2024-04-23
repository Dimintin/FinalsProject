using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test1234.db;

namespace test1234.classes
{
    public class FilmCollection
    {
        public static ObservableCollection<FilmLibrary> filmLibraries = new ObservableCollection<FilmLibrary>(EF.Context.FilmLibrary);
    }
}
