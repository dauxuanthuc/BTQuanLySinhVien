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

namespace BTQuanLySinhVien
{
    public partial class Form2 : Form
    {
        QuanLySVDB db = new QuanLySVDB();
        public Form2()
        {
            InitializeComponent();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                QuanLySVDB context = new QuanLySVDB();
                List<Khoa> listFalcultys = context.Khoas.ToList();
                loadKhoa();
         
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void loadKhoa()
        {
            dvgQuanLyKhoa.Rows.Clear();
            QuanLySVDB context = new QuanLySVDB();
            List<Khoa> listFalcultys = context.Khoas.ToList();
            foreach (var item in listFalcultys)
            {
                int index = dvgQuanLyKhoa.Rows.Add();
                dvgQuanLyKhoa.Rows[index].Cells[0].Value = item.MaKhoa;
                dvgQuanLyKhoa.Rows[index].Cells[1].Value = item.TenKhoa;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            int maKhoa = int.Parse(txtMaKhoa.Text);
            string tenKhoa = txtTenKhoa.Text;
            Khoa khoa = db.Khoas.Find(maKhoa);
            if(khoa != null)
            {
                MessageBox.Show("Mã khoa đã tồn tại");
                return;
            }
            else
            {
                khoa = new Khoa();
                khoa.MaKhoa = maKhoa;
                khoa.TenKhoa = tenKhoa;
                db.Khoas.Add(khoa);
                db.SaveChanges();
                loadKhoa();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                List<Khoa> listKhoa = db.Khoas.ToList();
                //kiểm tra mã khoa có tồn tại không
                Khoa khoa = db.Khoas.Find(int.Parse(txtMaKhoa.Text));
                if (khoa != null)
                {
                    khoa.TenKhoa = txtTenKhoa.Text;
                    db.SaveChanges();
                    loadKhoa();
                }
                else
                {
                    MessageBox.Show("Mã khoa không tồn tại");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm dữ liệu: {ex.Message}", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                List<Khoa> listKhoa = db.Khoas.ToList();
                //tìm kiếm khoa có tồn tại trong db
                Khoa khoa = listKhoa.FirstOrDefault(s => s.MaKhoa == int.Parse(txtMaKhoa.Text));
                if (khoa != null)
                {
                    //xóa sinh viên
                    db.Khoas.Remove(khoa);
                    db.SaveChanges();
                    loadKhoa();
                    MessageBox.Show("Xóa Khoa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy Khoa cần xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa dữ liệu: {ex.Message}", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dvgQuanLyKhoa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu hàng được chọn hợp lệ
            if (e.RowIndex >= 0)
            {
                // Lấy hàng được chọn
                DataGridViewRow selectedRow = dvgQuanLyKhoa.Rows[e.RowIndex];

                // Gán giá trị từ hàng được chọn vào các TextBox
                txtMaKhoa.Text = selectedRow.Cells[0].Value.ToString();// Mã sinh viên
                txtTenKhoa.Text = selectedRow.Cells[1].Value.ToString(); // Họ tên
            }
        }
    }
}
