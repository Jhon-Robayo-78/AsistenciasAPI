using System;
using System.Collections.Generic;

namespace AsistenciasApi.Models;

public partial class Matricula
{
    public int id_matricula { get; set; }

    public string codigoEstudiante { get; set; } = null!;

    public int? id_Horario { get; set; }

    public DateTime? inscrito { get; set; }

    public string periodo {  get; set; } 

    public string nrc { get; set;}

    /*public virtual Useruniversity CodigoEstudianteNavigation { get; set; } = null!;

    public virtual Horario? IdHorarioNavigation { get; set; }*/
}
