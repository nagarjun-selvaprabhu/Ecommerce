using Ecommerce.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Repository.Interface
{
    public interface IProductRepo
    {
        ICollection<Products> GetAllProducts();
        Products GetProduct(int ProductId);
        bool ProductExists(string name);
        bool ProductExists(int ProductId);
        bool CreateProduct(Products product);
        bool UpdateProduct(Products product);
        bool deleteProduct(Products product);
        bool save();

    }
}
