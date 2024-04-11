using System;
using System.Collections.Generic;

namespace Andyy.Models;

public partial class WorkSheet
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreateDateTime { get; set; }

    public DateTime UpDateTime { get; set; }
}