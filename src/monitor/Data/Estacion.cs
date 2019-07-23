//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace monitor.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Estacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Estacion()
        {
            this.Pieza = new HashSet<Pieza>();
            this.ResultadoSoldadora = new HashSet<ResultadoSoldadora>();
        }
    
        public int EstacionId { get; set; }
        public string Nombre { get; set; }
        public string Monitor { get; set; }
        public string IPPLC { get; set; }
        public Nullable<byte> Soldador { get; set; }
        public string IPSoldador { get; set; }
        public Nullable<int> Estatus { get; set; }
        public Nullable<System.DateTime> FechaHora { get; set; }
        public Nullable<int> SegundosAyudaVisual { get; set; }
        public string Modelo { get; set; }
        public string PID { get; set; }
        public bool isRunning { get; set; }
        public string Mensaje { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pieza> Pieza { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ResultadoSoldadora> ResultadoSoldadora { get; set; }
    }
}
