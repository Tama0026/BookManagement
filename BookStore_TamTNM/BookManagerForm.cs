using Repositories.Entities;
using Services;

namespace BookStore_MinhDLH
{
    public partial class BookManagerForm : Form
    {
        public BookManagementMember Account { get; set; }

        private BookService _bookService = new BookService();
        private BookCategoryService _categoryService = new BookCategoryService();

        //trên mạng, theo chuẩn, Service cx làm Interface luôn
        //để giúp UI linh hoạt dùng gói Service khác nhau - Dependency Injection

        public BookManagerForm()
        {
            InitializeComponent();
            dgvBookList.ReadOnly = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BookManagerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void BookManagerForm_Load(object sender, EventArgs e)
        {
            //load cái danh sách Sách vào grid
            var result = _bookService.GetAllBooks();
            dgvBookList.DataSource = null;    //vip, xoá lưới, lấy danh sách mới
            dgvBookList.DataSource = result;
            //giấu cột CategoryId, nó đang chứa data 1 dòng của table Category\
            //dgvBookList.Columns["BookCategory"].Visible = false;

            //đổ toàn bộ Category vào ComboBox hoặc DropDown
            cboCategory.DataSource = _categoryService.GetAllCategories();
            cboCateSearch.DataSource = _categoryService.GetAllCategories();
            //giấu cột ko cần thiết trong 3 cột của table Category,
            //chỉ show cái cột Name của Category -  BookGenreType
            //nhưng khi chọn 1 dòng xổ ra, thì ngầm hiểu value là cột Id
            cboCategory.DisplayMember = "BookGenreType";
            cboCategory.ValueMember = "BookCategoryId"; //chọn 1 dòng xổ ra

            cboCateSearch.DisplayMember = "BookGenreType";
            cboCateSearch.ValueMember = "BookCategoryId";
            //nghĩa là lấy CategoryId

            lblHello.Text = "Hello: " + Account.FullName;
            //DISABLE NÚT BẤM TÙY ROLE

            if (Account.MemberRole == 2)
            {
                txtId.Enabled = false;
                btnDelete.Enabled = false;
                btnModify.Enabled = false;
            }

            if (Account.MemberRole == 3)
            {
                btnAdd.Enabled = false;
                btnUpdate.Enabled = false;
                txtId.Enabled = false;
                btnDelete.Enabled = false;
                btnModify.Enabled = false;
            }

        }

        private void dgvBookList_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvBookList.SelectedRows.Count > 0)
            {
                var selectedBook = (Book)dgvBookList.SelectedRows[0].DataBoundItem;
                //có trong tay 1 object/1 cuốn sách đang được lựa chọn rồi
                //chấm từng field của nó đổ vào form
                txtId.Text = selectedBook.BookId.ToString();
                txtName.Text = selectedBook.BookName;
                txtDescription.Text = selectedBook.Description;
                dtpReleasedDate.Value = selectedBook.ReleaseDate;
                txtQuantity.Text = selectedBook.Quantity.ToString();
                txtPrice.Text = selectedBook.Price.ToString();
                cboCategory.SelectedValue = selectedBook.BookCategoryId;
                txtAuthor.Text = selectedBook.Author;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKeyword.Text))
            {
                MessageBox.Show("The search keyword is required!!!",
                    "Search keyword required",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = _bookService.SearchBooks(txtKeyword.Text.Trim());

            //TODO: THÔNG BÁO NẾU KẾT QUẢ SEARCH KO CÓ
            dgvBookList.DataSource = null;    //vip, xoá lưới, lấy danh sách mới
            dgvBookList.DataSource = result;

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //mã sách là con số int
            //Convert.ToInt32(chữ về số)  Integer.Parse()
            //int.Parse(chữ về số)
            //Int32.Parse(chữ về số)
            //ném ra ngoại lệ nếu convert ko đc, đưa Ahihi sao convert         
            int id;

            if (string.IsNullOrWhiteSpace(txtId.Text) || !int.TryParse(txtId.Text, out id))
            {
                MessageBox.Show("The BookModel ID is invalid. Please input a number!!!",
                    "Invalid BookModel ID",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            _bookService.DeleteABook(id);

            //load cái danh sách Sách vào grid
            var result = _bookService.GetAllBooks();
            dgvBookList.DataSource = null;    //vip, xoá lưới, lấy danh sách mới
            dgvBookList.DataSource = result;

            //TODO: XOÁ THÀNH CÔNG THÌ XOÁ TRẮNG CÁC Ô NHẬP
            //TODO: ĐÃ XOÁ RỒI, MÀ XOÁ LẠI, HOẶC GÕ ID ĐÃ XOÁ, CHỬI KO TỒN TẠI
            //HINT: Get(id) của class Repo

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id;
            if (string.IsNullOrWhiteSpace(txtId.Text) || !int.TryParse(txtId.Text, out id))
            {
                MessageBox.Show("The BookModel ID is invalid. Please select a row in the grid to edit or input a number!!!",
                    "Invalid BookModel ID",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //TODO: PHẢI CHECK NẾU CUỐN SÁCH TỒN TẠI, GIỐNG NHƯ XOÁ SÁCH
            //HINT: GETABOOK()

            //đã có id rồi, chuyển sang form đơn 
            BookForm bookForm = new BookForm();
            bookForm.BookId = int.Parse(txtId.Text);
            bookForm.ShowDialog();
            //refesh lại grid
            //load cái danh sách Sách vào grid
            var result = _bookService.GetAllBooks();
            dgvBookList.DataSource = null;    //vip, xoá lưới, lấy danh sách mới
            dgvBookList.DataSource = result;

            //xoá ô text
            //txtId.Text = "";

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            BookForm bookForm = new BookForm();
            bookForm.BookId = null;
            bookForm.ShowDialog();
            //NEW THÌ SHOW FORM TRỐNG, DO NOTHING
            //xong, đóng form thì show lại lưới/grid
            //load cái danh sách Sách vào grid
            var result = _bookService.GetAllBooks();
            dgvBookList.DataSource = null;    //vip, xoá lưới, lấy danh sách mới
            dgvBookList.DataSource = result;
        }

        private void btnCateSearch_Click(object sender, EventArgs e)
        {

            var result = _bookService.SearchBooksByCate(cboCateSearch.SelectedValue.ToString());

            dgvBookList.DataSource = null;
            dgvBookList.DataSource = result;
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            CategoryForm categoryForm = new CategoryForm();
            categoryForm.ShowDialog();
            cboCategory.DataSource = _categoryService.GetAllCategories();
            cboCateSearch.DataSource = _categoryService.GetAllCategories();
            //giấu cột ko cần thiết trong 3 cột của table Category,
            //chỉ show cái cột Name của Category -  BookGenreType
            //nhưng khi chọn 1 dòng xổ ra, thì ngầm hiểu value là cột Id
            cboCategory.DisplayMember = "BookGenreType";
            cboCategory.ValueMember = "BookCategoryId";

        }
    }
}
