using System;
using System.Collections.Generic;

namespace AsistenciasApi.Models;

public partial class ProfessorWithMaterium
{
    public int Id { get; set; }

    public string Fk_Id { get; set; } = null!;

    public int Nrc { get; set; }

    public string P_Academico { get; set; }

    //public virtual Useruniversity Fk { get; set; } = null!;

    //public virtual Clase NrcNavigation { get; set; } = null!;
}
