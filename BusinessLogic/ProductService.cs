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
        public void UpdateProductQuantity(int productId, int quantityToAdd)
        {
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


        public Product GetProductById(int productId)
        {
            return _productDAO.GetProductById(productId);
        }
        public void UpdateProduct(Product product)
        {
            // Thực hiện các kiểm tra bổ sung ở đây nếu cần
            _productDAO.UpdateProduct(product);
        }
    }
}

