using System;
using System.Collections.Generic;

namespace PRM_Project.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

public partial class CategoryDTO
{
    public string CategoryName { get; set; } = null!;
}
