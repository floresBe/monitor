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
    
    public partial class PiezasTomadas
    {
        public int PiezasId { get; set; }
        public string ModeloId { get; set; }
        public Nullable<int> PID { get; set; }
        public Nullable<int> Ingenieria { get; set; }
        public Nullable<int> Calidad { get; set; }
        public Nullable<int> Produccion { get; set; }
        public Nullable<System.DateTime> FechaHora { get; set; }
    
        public virtual Modelo Modelo { get; set; }
    }
}
