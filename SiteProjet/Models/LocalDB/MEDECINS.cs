//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SiteProjet.Models.LocalDB
{
    using System;
    using System.Collections.Generic;
    
    public partial class MEDECINS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MEDECINS()
        {
            this.CRENEAUX = new HashSet<CRENEAUX>();
        }
    
        public long ID { get; set; }
        public string TITRE { get; set; }
        public string NOM { get; set; }
        public string PRENOM { get; set; }
        public string EMAIL { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CRENEAUX> CRENEAUX { get; set; }
    }
}
