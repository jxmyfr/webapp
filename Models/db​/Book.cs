using System;
using System.Collections.Generic;

namespace DBFirst.Models.db​;

public partial class Book
{
    public string BookId { get; set; } = null!;

    public string? BookName { get; set; }

    public int? CategoryId { get; set; }

    public int? PublishId { get; set; }

    public string? Isbn { get; set; }

    public double? BookCost { get; set; }

    public double? BookPrice { get; set; }

    public virtual Category? Category { get; set; } 

    public virtual Publish? Publish { get; set; }
}
