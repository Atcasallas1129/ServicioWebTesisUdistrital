//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1
{
    using System;
    using System.Collections.Generic;
    
    public partial class procesoDocumentacion
    {
        public long id { get; set; }
        public System.Guid uniqueIdentifier { get; set; }
        public string rad { get; set; }
        public string proceso { get; set; }
        public long hechoVictimizante { get; set; }
        public long daneOcurrenciaHecho { get; set; }
        public System.DateTime fechaOcurrenciaHecho { get; set; }
        public long parentesco { get; set; }
        public string porcentaje { get; set; }
        public Nullable<long> usuarioModificacion { get; set; }
        public Nullable<System.DateTime> fechaModificacion { get; set; }
        public long idPersonaVictima { get; set; }
        public long idPersonaDestinatario { get; set; }
        public Nullable<bool> regModificado { get; set; }
    
        public virtual dane dane { get; set; }
        public virtual hechoVictimizante hechoVictimizante1 { get; set; }
        public virtual parentesco parentesco1 { get; set; }
        public virtual persona persona { get; set; }
        public virtual persona persona1 { get; set; }
        public virtual usuario usuario { get; set; }
    }
}
