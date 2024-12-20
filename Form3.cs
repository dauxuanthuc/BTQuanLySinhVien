using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BTQuanLySinhVien.EntitiesDB;
namespace BTQuanLySinhVien
{
    public partial class Form3 : Form
    {
        QuanLySVDB db = new QuanLySVDB();
        public Form3()
        {
            InitializeComponent();
            List<Khoa> listFalcultys = db.Khoas.ToList(); //lấy các khoa
            FillFalcultyCombobox(listFalcultys);
        }
        private void FillFalcultyCombobox(List<Khoa> listFalcultys)
        {
            this.cbbKhoa.DataSource = listFalcultys;
            this.cbbKhoa.DisplayMember = "TenKhoa";
            this.cbbKhoa.ValueMember = "MaKhoa";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy mã khoa được chọn từ ComboBox
                int selectedKhoaId = (int)cbbKhoa.SelectedValue;

                // Truy vấn danh sách sinh viên thuộc mã khoa
                List<SinhVien> students = db.SinhViens
                    .Where(sv => sv.MaKhoa == selectedKhoaId)
                    .ToList();

                // Hiển thị danh sách sinh viên lên DataGridView
                DisplayStudentsOnDataGridView(students);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DisplayStudentsOnDataGridView(List<SinhVien> students)
        {
            // Xóa dữ liệu cũ trên DataGridView
            dvgTimKiem.Rows.Clear();

            // Duyệt qua danh sách sinh viên và thêm vào DataGridView
            foreach (var student in students)
            {
                int index = dvgTimKiem.Rows.Add();
                dvgTimKiem.Rows[index].Cells[0].Value = student.MaSV;
                dvgTimKiem.Rows[index].Cells[1].Value = student.HoTen;
                dvgTimKiem.Rows[index].Cells[2].Value = student.Khoa.TenKhoa; // Lấy tên khoa từ quan hệ
                dvgTimKiem.Rows[index].Cells[3].Value = student.DiemTB;
            }
            capnhatsoluongtimkiem();
        }
        private void btnVe_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }
        private void capnhatsoluongtimkiem()
        {
            //viết hàm tính tổng số sinh viên tìm kiếm được
            int tong = dvgTimKiem.Rows.Count - 1;
            txtTongTK.Text = tong.ToString();
        }
        private void txtTongTK_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
