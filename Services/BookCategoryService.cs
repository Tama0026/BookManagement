using Repositories;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{

    /// <summary>
    /// Class này cung cấp dữ liệu từ table Category cho UI, và ngược lại
    /// Class này sẽ xài CategoryRepo vừa tạo, Repo thì lại gọi đến DbContex
    /// </summary>
    public class BookCategoryService
    {

        private BookCategoryRepository _repo;
        public List<BookCategory> GetAllCategories()
        {
            _repo = new BookCategoryRepository();
            return _repo.GetAll();
        }
        public BookCategory GetCategoryById(int id)
        {
            _repo = new BookCategoryRepository();
            var cate = GetAllCategories().FirstOrDefault(c => c.BookCategoryId == id);
            return cate;
        }
        public BookCategory GetCategoryByGenre(string Genre)
        {
            _repo = new BookCategoryRepository();
            var cate = GetAllCategories().FirstOrDefault(c => c.BookGenreType.ToLower() == Genre.ToLower());
            return cate;
        }
        public void DeleteCate(int id)
        {
            _repo = new BookCategoryRepository();
            _repo.DeleteCate(id);
        }
        public void UpdateCate (BookCategory cate)
        {
            _repo = new BookCategoryRepository();
            _repo.UpdateCate(cate);
        }
        public void AddCate(BookCategory cate)
        {
            _repo = new BookCategoryRepository();
            _repo.AddCate(cate);
        }
        public List<BookCategory> SearchCates(string keyword)
        {
            _repo = new BookCategoryRepository();
            return _repo.GetAll().Where(c => c.BookGenreType.ToLower().Contains(keyword.ToLower()) || c.Description.ToLower().Contains(keyword.ToLower())).ToList();        
        }

    }
}
