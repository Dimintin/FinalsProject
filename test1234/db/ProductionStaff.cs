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
    
    public partial class ProductionStaff
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductionStaff()
        {
            this.FilmProductionStaff = new HashSet<FilmProductionStaff>();
        }
    
        public int Id { get; set; }
        public string FullName { get; set; }
        public System.DateTime BirthDate { get; set; }
        public Nullable<System.DateTime> DeathDate { get; set; }
        public Nullable<int> BirthCountryId { get; set; }
        public Nullable<int> GenderId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FilmProductionStaff> FilmProductionStaff { get; set; }
        public virtual Gender Gender { get; set; }
    }
}
