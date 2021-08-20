using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Model
{
    public class Products
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
