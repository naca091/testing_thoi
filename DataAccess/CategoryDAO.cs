using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class CategoryDAO
    {
        private readonly PRN221_Warehouse _context;

        public CategoryDAO(PRN221_Warehouse context)
        {
            _context = context;
        }

        public List<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public void CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }
        public void AddCategory(Category category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));

            _context.Categories.Add(category);
            _context.SaveChanges();
        }
        public void UpdateCategoryName(int categoryId, string newName)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == categoryId);
            if (category == null) throw new ArgumentException("Category not found.");

            category.Name = newName;
            _context.SaveChanges();
        }
        public void UpdateCategory(Category category)
        {
            var existingCategory = _context.Categories.Find(category.CategoryId);
            if (existingCategory != null)
            {
                existingCategory.CategoryCode = category.CategoryCode;
                existingCategory.Name = category.Name;
                existingCategory.Status = category.Status;
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Category not found.");
            }
        }
        public Category? GetById(int categoryId)
        {
            return _context.Categories.Include(c => c.Products).FirstOrDefault(c => c.CategoryId == categoryId);
        }

        public void Delete(int categoryId)
        {
            var category = GetById(categoryId);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }
    }
}
