using DataAccess.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class ProductService
    {




        private readonly ProductDAO _productDAO;

        public ProductService(PRN221_Warehouse context)
        {
            _productDAO = new ProductDAO(context);
        }

        public List<Product> GetAllProducts()
        {
            return _productDAO.GetAllProducts();
        }

        public Product GetProductByID(int productId)
        {
            return _productDAO.GetProductByID(productId);
        }
        public void UpdateProduct(Product product)
        {
            ValidateProduct(product);
            _productDAO.UpdateProduct(product);
        }

        public void UpdateProductQuantity(int productId, int quantityToAdd)
        {
            if (quantityToAdd == 0)
            {
                throw new Exception("Quantity change cannot be zero.");
            }
            _productDAO.UpdateProductQuantity(productId, quantityToAdd);
        }
        public bool AddProduct(Product newProduct)
        {
            // Additional business logic if needed (e.g., validation)
            if (string.IsNullOrWhiteSpace(newProduct.ProductCode) || !System.Text.RegularExpressions.Regex.IsMatch(newProduct.ProductCode, @"^PROD\d{3,}$"))
            {
                throw new ArgumentException("ProductCode must start with 'PROD' followed by at least 3 digits.");
            }

            return _productDAO.AddProduct(newProduct);
        }
        public int GetTotalProducts()
        {
            return _productDAO.GetAllProducts().Count; // Adjust this method
        }

        public Product GetProductById(int productId)
        {
            return _productDAO.GetProductById(productId);
        }
        public List<Product> GetProductsByCategoryId(int categoryId)
        {
            return _productDAO.GetProductsByCategoryId(categoryId);
        }


        private void ValidateProduct(Product product)
        {
            // Validate ProductCode format
            if (string.IsNullOrWhiteSpace(product.ProductCode) ||
                !System.Text.RegularExpressions.Regex.IsMatch(product.ProductCode, @"^PROD\d{3,}$"))
            {
                throw new Exception("ProductCode must start with 'PROD' followed by at least 3 digits.");
            }

            // Validate Name
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new Exception("Product name is required.");
            }

            // Validate Category and Area
            if (product.CategoryId <= 0)
            {
                throw new Exception("Valid category must be selected.");
            }

            if (product.AreaId <= 0)
            {
                throw new Exception("Valid storage area must be selected.");
            }

            // Validate Quantity
            if (product.Quantity < 0)
            {
                throw new Exception("Quantity cannot be negative.");
            }

            // Validate Status
            if (product.Status != 0 && product.Status != 1)
            {
                throw new Exception("Status must be either 0 (Inactive) or 1 (Active).");
            }
        }

    }
}

