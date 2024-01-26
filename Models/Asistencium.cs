using System;
using System.Collections.Generic;

namespace AsistenciasApi.Models;

public partial class Asistencium
{
    public string? id_User { get; set; }

    public int? id_Horario { get; set; }

    public int Id { get; set; }

    public int? No_Semana { get; set; }

    public string Dato { get; set; } = null!;

    public int nrc { get; set; }

    /*public virtual Horario? IdHorarioNavigation { get; set; }

    public virtual Useruniversity? IdUserNavigation { get; set; }

    public virtual Semana? NoSemanaNavigation { get; set; }*/
}
