using System;
using System.Collections.Generic;

namespace StudentInfoManagement.Models;

public partial class StudentInfo
{
    public int Id { get; set; }

    public string? StudentCode { get; set; }

    public string? StudentName { get; set; }

    public DateTime? Birthday { get; set; }

    public string? Gender { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }
}
