using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BTQuanLySinhVien.EntitiesDB;
using System.Data.Entity;
namespace BTQuanLySinhVien
{
    public partial class Form1 : Form
    {
        //tạo đối tượng db
        QuanLySVDB db = new QuanLySVDB();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                List<Khoa> listFalcultys = db.Khoas.ToList(); //lấy các khoa
                List<SinhVien> listStudent = db.SinhViens.Include(s => s.Khoa).ToList();
                FillFalcultyCombobox(listFalcultys);
                BindGrid(listStudent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
       
        private void FillFalcultyCombobox(List<Khoa> listFalcultys)
        {
            this.cbbKhoa.DataSource = listFalcultys;
            this.cbbKhoa.DisplayMember = "TenKhoa";
            this.cbbKhoa.ValueMember = "MaKhoa";
        }
        private void BindGrid(List<SinhVien> listStudent)
        {
            dvgQLSinhVien.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dvgQLSinhVien.Rows.Add();
                dvgQLSinhVien.Rows[index].Cells[0].Value = item.MaSV;
                dvgQLSinhVien.Rows[index].Cells[1].Value = item.HoTen;
                dvgQLSinhVien.Rows[index].Cells[2].Value = item.Khoa != null ? item.Khoa.TenKhoa : "N/A";
                dvgQLSinhVien.Rows[index].Cells[3].Value = item.DiemTB;
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            //ràng buộc mã SV chỉ chứa 10 kí tự

            try
            {
                // Kiểm tra mã sinh viên có đủ 10 ký tự
                if (txtMa.Text.Length != 10)
                {
                    MessageBox.Show("Mã sinh viên phải đúng 10 ký tự", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra dữ liệu nhập đầy đủ
                if (string.IsNullOrWhiteSpace(txtMa.Text) ||
                    string.IsNullOrWhiteSpace(txtTen.Text) ||
                    string.IsNullOrWhiteSpace(txtDiem.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra điểm trung bình là số hợp lệ
                if (!double.TryParse(txtDiem.Text, out double diemTB) || diemTB < 0 || diemTB > 10)
                {
                    MessageBox.Show("Điểm trung bình phải là số từ 0 đến 10", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra mã khoa hợp lệ
                if (cbbKhoa.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn một khoa hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra mã sinh viên đã tồn tại hay chưa
                if (db.SinhViens.Any(s => s.MaSV == txtMa.Text))
                {
                    MessageBox.Show("Mã sinh viên đã tồn tại. Vui lòng nhập mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo sinh viên mới
                var newStudent = new SinhVien()
                {
                    MaSV = txtMa.Text,
                    HoTen = txtTen.Text,
                    MaKhoa = int.Parse(cbbKhoa.SelectedValue.ToString()),
                    DiemTB = diemTB
                };

                // Thêm sinh viên vào db
                db.SinhViens.Add(newStudent);
                db.SaveChanges();

                // Hiển thị lại danh sách sinh viên
                BindGrid(db.SinhViens.ToList());
                MessageBox.Show("Thêm sinh viên thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm dữ liệu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy mã sinh viên từ textbox
                string maSV = txtMa.Text;

                if (string.IsNullOrEmpty(maSV))
                {
                    MessageBox.Show("Vui lòng nhập mã sinh viên để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tìm kiếm sinh viên trong cơ sở dữ liệu
                var student = db.SinhViens.FirstOrDefault(s => s.MaSV == maSV);

                if (student != null)
                {
                    // Xóa sinh viên khỏi db
                    db.SinhViens.Remove(student);

                    // Lưu thay đổi vào cơ sở dữ liệu
                    db.SaveChanges();

                    // Hiển thị lại danh sách sinh viên
                    BindGrid(db.SinhViens.ToList());

                    MessageBox.Show("Xóa sinh viên thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên cần xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa dữ liệu: {ex.Message}", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void dvgQLSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
                // Kiểm tra nếu hàng được chọn hợp lệ
                if (e.RowIndex >= 0)
                {
                    // Lấy hàng được chọn
                    DataGridViewRow selectedRow = dvgQLSinhVien.Rows[e.RowIndex];

                // Gán giá trị từ hàng được chọn vào các TextBox
                    txtMa.Text = selectedRow.Cells[0].Value.ToString();// Mã sinh viên
                    txtTen.Text = selectedRow.Cells[1].Value.ToString(); // Họ tên
                    cbbKhoa.SelectedValue = selectedRow.Cells[2].Value.ToString(); // Khoa (phải đảm bảo combobox đã bind dữ liệu đúng)
                    txtDiem.Text = selectedRow.Cells[3].Value.ToString(); // Điểm trung bình
                }
            
            
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                List<SinhVien> listStudent = db.SinhViens.ToList();
                var student = listStudent.FirstOrDefault(s => s.MaSV == txtMa.Text);
                if (student != null)
                {
                    student.HoTen = txtTen.Text;
                    student.MaKhoa = int.Parse(cbbKhoa.SelectedValue.ToString());
                    student.DiemTB = double.Parse(txtDiem.Text);
                    db.SaveChanges();
                    BindGrid(db.SinhViens.ToList());
                    MessageBox.Show("Cập nhật sinh viên thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên cần cập nhật", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }    
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm dữ liệu: {ex.Message}", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chứcNăngToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void quảnLýKhoaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Show();
            Hide();
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Show();
            Hide();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3();
            f.Show();
            Hide();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
