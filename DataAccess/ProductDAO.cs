using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ProductDAO
    {
        private readonly PRN221_Warehouse _context;

        public ProductDAO(PRN221_Warehouse context)
        {
            _context = context;
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public Product GetProductByID(int productId)
        {
            return _context.Products.Find(productId);
        }

        public void UpdateProduct(Product product)
        {
            var existingProduct = _context.Products.Find(product.ProductId);
            if (existingProduct == null)
            {
                throw new Exception($"Product with ID {product.ProductId} not found.");
            }

            existingProduct.CategoryId = product.CategoryId;
            existingProduct.AreaId = product.AreaId;
            existingProduct.ProductCode = product.ProductCode;
            existingProduct.Name = product.Name;
            existingProduct.Quantity = product.Quantity;
            existingProduct.Status = product.Status;

            _context.SaveChanges();
        }

        public List<Product> GetProductsByCategoryId(int categoryId)
        {
            return _context.Products.Where(p => p.CategoryId == categoryId).ToList();
        }


        public void UpdateProductQuantity(int productId, int quantityToAdd)
        {
            var product = _context.Products.Find(productId);
            if (product == null)
            {
                throw new Exception($"Product with ID {productId} not found.");
            }

            if (product.Quantity + quantityToAdd < 0)
            {
                throw new Exception("Resulting quantity cannot be negative.");
            }

            product.Quantity += quantityToAdd;
            _context.SaveChanges();
        }


        public bool AddProduct(Product newProduct)
        {
            try
            {
                _context.Products.Add(newProduct);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Log exception (e.g., using a logging framework)
                Console.WriteLine($"Error adding product: {ex.Message}");
                return false;
            }
        }


        public Product GetProductById(int productId)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Area)
                .FirstOrDefault(p => p.ProductId == productId);
        }

    }
}
