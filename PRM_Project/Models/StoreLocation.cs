using System;
using System.Collections.Generic;

namespace PRM_Project.Models;

public partial class StoreLocation
{
    public int LocationId { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public string Address { get; set; } = null!;
}

public partial class AddStoreLocationDTO
{
    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public string Address { get; set; } = null!;
}

public partial class UpdateStoreLocationDTO
{
    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public string Address { get; set; } = null!;
}