using System;
using System.Collections.Generic;

namespace PRM_Project.Models;

public partial class StoreLocation
{
    public int LocationId { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public string Address { get; set; } = null!;

    public string Name { get; set; }

    public string Phone { get; set; }

    public string OpeningHours { get; set; }
}

public partial class StoreLocationDTO
{

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public string Address { get; set; } = null!;

    public string Name { get; set; }

    public string Phone { get; set; }

    public string OpeningHours { get; set; }
}