using System;
using System.Collections.Generic;

namespace DBFirst.Models.db​
{
    public partial class Publish
    {
        public int PublishId { get; set; }

        public string? PublishName { get; set; }

        public string? ContactName { get; set; }

        public string? Address { get; set; }

        public string? Telephone { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}


