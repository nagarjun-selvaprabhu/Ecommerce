using System;
using System.ComponentModel.DataAnnotations;

namespace OrderEndpoint.Model
{
    public class Product
    {
        [Key]

        public int Id { get; set; }
        [Required]

        public string ProductName { get; set; }

        [Required]
        public int ProductAmout { get; set; }

        public int ProductQuantity { get; set; }

        public DateTime Created { get; set; }
    }
}
