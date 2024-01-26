using System;
using System.Collections.Generic;

namespace AsistenciasApi.Models;

public partial class Periodo
{
    public string P_Academico { get; set; } = null!;

    //public virtual ICollection<Clase> Clases { get; set; } = new List<Clase>();

    //public virtual ICollection<Semana> Semanas { get; set; } = new List<Semana>();
}
