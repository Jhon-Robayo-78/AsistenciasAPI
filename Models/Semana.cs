using System;
using System.Collections.Generic;

namespace AsistenciasApi.Models;

public partial class Semana
{
    public int Id { get; set; }

    public int NoSemana { get; set; }

    public DateTime? Fecha_Inicio { get; set; }

    public DateTime? Fecha_Fin { get; set; }

    public string Periodo { get; set; } = null!;

    //public virtual ICollection<Asistencium> Asistencia { get; set; } = new List<Asistencium>();

   // public virtual Periodo PeriodoNavigation { get; set; } = null!;
}
