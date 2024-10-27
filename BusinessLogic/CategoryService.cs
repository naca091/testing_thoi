using DataAccess.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class CategoryService
    {
        private readonly CategoryDAO _categoryDAO;

        public CategoryService(CategoryDAO categoryDAO)
        {
            _categoryDAO = categoryDAO;
        }

        public List<Category> GetAllCategories()
        {
            return _categoryDAO.GetAllCategories();
        }

        public void CreateCategory(string categoryCode, string name, int status)
        {
            if (string.IsNullOrWhiteSpace(categoryCode)) throw new ArgumentException("Category code is required.");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Category name is required.");

            var category = new Category
            {
                CategoryCode = categoryCode,
                Name = name,
                Status = status
            };

            _categoryDAO.AddCategory(category);
        }

        public void UpdateCategoryName(int categoryId, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName)) throw new ArgumentException("Category name is required.");
            _categoryDAO.UpdateCategoryName(categoryId, newName);
        }
        public void UpdateCategory(Category category)
        {
            _categoryDAO.UpdateCategory(category);
        }
        public bool CanDeleteCategory(int categoryId)
        {
            var category = _categoryDAO.GetById(categoryId);
            return category != null && !category.Products.Any();
        }

        public void DeleteCategory(int categoryId)
        {
            if (!CanDeleteCategory(categoryId))
            {
                throw new InvalidOperationException("Cannot delete category with existing products.");
            }
            _categoryDAO.Delete(categoryId);
        }
    }
}
