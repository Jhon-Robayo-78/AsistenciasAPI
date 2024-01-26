using System;
using System.Collections.Generic;

namespace AsistenciasApi.Models;

public partial class Useruniversity
{
    public string Id { get; set; } = null!;

    public string Tipo_doc { get; set; } = null!;

    public string No_doc { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public string EmailAcademico { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Rol { get; set; }

    public string StatusUser { get; set; } = null!;

    //public virtual ICollection<Asistencium> Asistencia { get; set; } = new List<Asistencium>();

    //public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();

    //public virtual ICollection<ProfessorWithMaterium> ProfessorWithMateria { get; set; } = new List<ProfessorWithMaterium>();
}
