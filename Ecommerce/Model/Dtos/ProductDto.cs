using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Model.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int ProductAmout { get; set; }
        public int ProductQuantity { get; set; }
        public DateTime Created { get; set; }
    }
}
