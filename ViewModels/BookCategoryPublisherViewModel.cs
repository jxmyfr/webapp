using System.ComponentModel.DataAnnotations;

namespace DBFirst.ViewModels
{
    public class BookCategoryPublisherViewModel
    {
        [Key]
        public string BookId { get; set; }
        public string BookName { get; set; }
        public string CategoryName { get; set; }
        public string PublishName { get; set; }
        public string Isbn { get; set; }
        public double? BookCost { get; set; }
        public double? BookPrice { get; set; }
    }
}
