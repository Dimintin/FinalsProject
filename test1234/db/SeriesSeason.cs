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
    
    public partial class SeriesSeason
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SeriesSeason()
        {
            this.SeriesEpisode = new HashSet<SeriesEpisode>();
        }
    
        public int Id { get; set; }
        public Nullable<int> FilmId { get; set; }
        public int SeasonNumber { get; set; }
        public string Title { get; set; }
        public string EnTitle { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> PremiereDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SeriesEpisode> SeriesEpisode { get; set; }
    }
}