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
    
    public partial class ResultadoSoldadora
    {
        public int ResultadoId { get; set; }
        public string ModeloId { get; set; }
        public Nullable<int> EstacionId { get; set; }
        public Nullable<System.DateTime> FechaHora { get; set; }
        public Nullable<int> Cycle { get; set; }
        public Nullable<double> PkPwr { get; set; }
        public Nullable<double> TotalAbs { get; set; }
        public Nullable<double> Energy { get; set; }
        public Nullable<double> WeldForce { get; set; }
    
        public virtual Estacion Estacion { get; set; }
        public virtual Modelo Modelo { get; set; }
    }
}
