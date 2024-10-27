﻿using DataAccess.Models;
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
        public void UpdateProductQuantity(int productId, int quantityToAdd)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                throw new InvalidOperationException($"Product with ID {productId} does not exist.");
            }

            product.Quantity += quantityToAdd;
            _context.SaveChanges();
        }
        public void UpdateProduct(Product product)
        {
            // Tìm sản phẩm theo ID
            var existingProduct = _context.Products.Find(product.ProductId);
            if (existingProduct != null)
            {
                // Cập nhật thông tin sản phẩm
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.AreaId = product.AreaId;
                existingProduct.ProductCode = product.ProductCode;
                existingProduct.Name = product.Name;
                existingProduct.Quantity = product.Quantity;
                existingProduct.Status = product.Status;

                // Lưu thay đổi vào cơ sở dữ liệu
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Product not found");
            }
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