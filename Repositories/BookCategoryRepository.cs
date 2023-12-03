using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    /// <summary>
    /// Class này dùng để CRUD table Category - Thể loại Sách
    /// Thường thể loại sách hiếm khi thay đổi, chủ yếu là lấy ra
    /// danh sách đưa vào dropdown/combobox
    /// </summary>
    public class BookCategoryRepository
    {
        private BookManagement2023DbContext _context = new();
        public List<BookCategory> GetAll()
        {
            //_context = new BookManagement2023DbContext();
            //return _context.BookCategories.ToList();
            return new BookManagement2023DbContext().BookCategories.ToList();
        }
        public void DeleteCate(int CateId)
        {
            var cate =_context.BookCategories.FirstOrDefault(c => c.BookCategoryId == CateId);
            if(cate != null)
            {
                _context.Remove(cate);
                _context.SaveChanges();
            }
        }
        public void AddCate(BookCategory cate)
        {
            _context.Add(cate);
            _context.SaveChanges();

        }
        public void UpdateCate(BookCategory cate)
        {
            _context.Update(cate);
            _context.SaveChanges();
        }
    }
}
