//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace test1234.db
{
    using System;
    using System.Collections.Generic;
    
    public partial class FilmGenre
    {
        public int Id { get; set; }
        public int FilmId { get; set; }
        public int GenreId { get; set; }
    
        public virtual FilmLibrary FilmLibrary { get; set; }
        public virtual Genre Genre { get; set; }
    }
}