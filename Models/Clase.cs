using System;
using System.Collections.Generic;

namespace AsistenciasApi.Models;

public partial class Clase
{
    public int Nrc { get; set; }

    public int No_Students { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime? Inicio { get; set; }

    public DateTime? Fin { get; set; }

    public string Tipo { get; set; } = null!;

    public string? Materia { get; set; }

    public string? P_Academico { get; set; }

    //public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();

    /*public virtual Materium? MateriaNavigation { get; set; }

    public virtual Periodo? PAcademicoNavigation { get; set; }*/

    //public virtual ICollection<ProfessorWithMaterium> ProfessorWithMateria { get; set; } = new List<ProfessorWithMaterium>();
}
