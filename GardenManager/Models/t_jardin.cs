//------------------------------------------------------------------------------
// <auto-generated>
//    Ce code a été généré à partir d'un modèle.
//
//    Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//    Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GardenManager.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class t_jardin
    {
        public t_jardin()
        {
            this.t_tile = new HashSet<t_tile>();
        }
    
        public long jarId { get; set; }
        public string jarName { get; set; }
        public string jarDisposition { get; set; }
    
        public virtual ICollection<t_tile> t_tile { get; set; }
    }
}
