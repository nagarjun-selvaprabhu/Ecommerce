using Ecommerce.Data;
using Ecommerce.Model;
using Ecommerce.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Repository
{
    // Service layer consists the logic for Products endpoint
    public class ProductRepo : IProductRepo
    {
        private readonly ApplicationDbContext _db;

        public ProductRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateProduct(Products product)
        {

            _db.products.Add(product);
            return save();
        }

        public bool deleteProduct(Products product)
        {
            _db.products.Remove(product);
            return save();
        }

        public ICollection<Products> GetAllProducts()
        {
            return _db.products.OrderBy(a => a.ProductName).ToList();
        }

        public Products GetProduct(int ProductId)
        {
            return _db.products.FirstOrDefault(a => a.Id == ProductId);
        }

        public bool ProductExists(string name)
        {
            bool value = _db.products.Any(a => a.ProductName.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool ProductExists(int ProductId)
        {
            return _db.products.Any(a => a.Id == ProductId);
        }

        public bool save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateProduct(Products product)
        {
            _db.products.Update(product);
            return save();
        }
    }
}
