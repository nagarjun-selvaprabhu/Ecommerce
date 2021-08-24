using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
