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
    
    public partial class PromoTrailer
    {
        public int Id { get; set; }
        public Nullable<int> FilmId { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
    
        public virtual FilmLibrary FilmLibrary { get; set; }
    }
}
