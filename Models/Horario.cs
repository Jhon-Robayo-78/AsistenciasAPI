using System;
using System.Collections.Generic;

namespace AsistenciasApi.Models;

public partial class Horario
{
    public int Id { get; set; }

    public string Edf { get; set; } = null!;

    public string Salon { get; set; } = null!;

    public int? Nrc { get; set; }

    public TimeSpan? hora_inicio { get; set; }

    public TimeSpan? hora_fin { get; set; }

    //public virtual ICollection<Asistencium> Asistencia { get; set; } = new List<Asistencium>();

    //public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();

    //public virtual Clase? NrcNavigation { get; set; }
}
