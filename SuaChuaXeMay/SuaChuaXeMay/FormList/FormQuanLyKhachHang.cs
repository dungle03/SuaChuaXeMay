using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuaChuaXeMay.FormList
{
    public partial class FormQuanLyKhachHang : Form
    {
        ProcessDatabase pd = new ProcessDatabase();
        public FormQuanLyKhachHang()
        {
            InitializeComponent();
        }

        private void FormQuanLyKhachHang_Load(object sender, EventArgs e)
        {
            // tắt nút sửa và xóa
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

            // đổ dữ liệu lên datagridview
            dgvDanhSachKH.DataSource = pd.DocBang("select * from KhachHang");
            dgvDanhSachKH.Columns[0].HeaderText = "ID";
            //dgvDanhSachKH.Columns[0].Width = 70;

            dgvDanhSachKH.Columns[1].HeaderText = "Tên Khách Hàng";
            dgvDanhSachKH.Columns[1].Width = 150;

            dgvDanhSachKH.Columns[2].HeaderText = "Thông tin liên hệ";
            dgvDanhSachKH.Columns[2].Width = 200;

            // trỏ vào ô điền mã nhân viên
            txtIDKhachHang.Focus();
        }

        private void dgvDanhSachKH_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // bật nút sửa và xóa 
            btnSua.Enabled = true;
            btnXoa.Enabled = true;

            // tắt nút thêm
            btnThem.Enabled = false;
            btnTimKiem.Enabled = false;
            txtIDKhachHang.Enabled = false;
            try
            {
                txtIDKhachHang.Text = dgvDanhSachKH.SelectedRows[0].Cells[0].Value.ToString();
                txtHoTenKH.Text = dgvDanhSachKH.SelectedRows[0].Cells[1].Value.ToString();
                txtThongTinLH.Text = dgvDanhSachKH.SelectedRows[0].Cells[2].Value.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi: " + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            // làm trắng các ô nhập
            txtIDKhachHang.Text = "";
            txtHoTenKH.Text = "";
            txtThongTinLH.Text = "";
            

            btnThem.Enabled = true;
            btnTimKiem.Enabled = true;
            btnXoa.Enabled = false;
            btnSua.Enabled = false;

            dgvDanhSachKH.DataSource = pd.DocBang("select * from KhachHang");
            txtIDKhachHang.Enabled = true;
            txtIDKhachHang.Focus();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateEmptyField(txtIDKhachHang.Text, "Mã Khách Hàng", txtIDKhachHang)) return;
            if (!ValidateEmptyField(txtHoTenKH.Text, "Ho Tên Khách Hàng", txtHoTenKH)) return;
            if (!ValidateEmptyField(txtThongTinLH.Text, "Thông tin liên hệ", txtThongTinLH)) return;
            

            if (!ValidateLength(txtIDKhachHang.Text, "Mã Khách Hàng", 10, txtIDKhachHang)) return;
            
            // IDKhachHang chỉ được chứa chữ và số
            if (!ValidateRegex(txtIDKhachHang.Text, @"^[a-zA-Z0-9]+$", "Mã Khách Hàng chỉ được chứa chữ và số", txtIDKhachHang)) return;

            // kiểm tra mã nhân viên đã tồn tại hay chưa
            DataTable dt = pd.DocBang($"select * from KhachHang where IDKhachHang = '{txtIDKhachHang.Text}'");
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Mã khách hàng đã tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIDKhachHang.Focus();
                return;
            }


            // thêm khách hàng vào database
            string sql = $"insert into KhachHang values('{txtIDKhachHang.Text}', N'{txtHoTenKH.Text}', N'{txtThongTinLH.Text}')";

            pd.CapNhat(sql);
            MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);


            //dgvDanhSachPT.DataSource = pd.DocBang("select * from PhuTung");
            btnLamMoi_Click(sender, e);

        }

        private bool ValidateEmptyField(string input, string fieldName, Control control)
        {
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show($"Bạn chưa nhập {fieldName}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                control.Focus();
                return false;
            }

            return true;
        }

        private bool ValidateLength(string input, string fieldName, int maxLength, TextBox textBox)
        {
            if (input.Length > maxLength)
            {
                MessageBox.Show($"{fieldName} không được vượt quá {maxLength} ký tự", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox.Focus();
                return false;
            }

            return true;
        }

        private bool ValidateRegex(string input, string pattern, string errorMessage, TextBox textBox)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(input, pattern))
            {
                MessageBox.Show(errorMessage, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox.Focus();
                return false;
            }

            return true;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Thông báo cho người dùng có chắc chắn muốn xóa khách hàng này không
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                // xóa phụ tùng
                string sql = "delete from KhachHang where IDKhachHang = '" + txtIDKhachHang.Text + "'";
                pd.CapNhat(sql);
                MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //dgvDanhSachPT.DataSource = pd.DocBang("select * from PhuTung");
                btnLamMoi_Click(sender, e);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!ValidateEmptyField(txtIDKhachHang.Text, "Mã Khách Hàng", txtIDKhachHang)) return;
            if (!ValidateEmptyField(txtHoTenKH.Text, "Ho Tên Khách Hàng", txtHoTenKH)) return;
            if (!ValidateEmptyField(txtThongTinLH.Text, "Thông tin liên hệ", txtThongTinLH)) return;


            if (!ValidateLength(txtIDKhachHang.Text, "Mã Khách Hàng", 10, txtIDKhachHang)) return;

            // IDKhachHang chỉ được chứa chữ và số
            if (!ValidateRegex(txtIDKhachHang.Text, @"^[a-zA-Z0-9]+$", "Mã Khách Hàng chỉ được chứa chữ và số", txtIDKhachHang)) return;

            // thông báo cho người dùng có chắc chắn muốn sửa khách hàng này không
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn sửa khách hàng này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                // sửa khách hàng
                string sql = $"update KhachHang set TenKhachHang = N'{txtHoTenKH.Text}', ThongTinLienHe = N'{txtThongTinLH.Text}' where IDKhachHang = '{txtIDKhachHang.Text}'";
                pd.CapNhat(sql);
                MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //dgvDanhSachPT.DataSource = pd.DocBang("select * from PhuTung");
                btnLamMoi_Click(sender, e);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            // tìm kiếm theo tên khách hàng
            if (string.IsNullOrEmpty(txtHoTenKH.Text))
            {
                MessageBox.Show("Bạn chưa nhập tên khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTenKH.Focus();
                return;
            }

            string sql = "select * from KhachHang where 1=1";

            if (!string.IsNullOrEmpty(txtHoTenKH.Text))
            {
                sql += " AND TenKhachHang LIKE N'%" + txtHoTenKH.Text + "%'";
            }

            DataTable dt = pd.DocBang(sql);

            if (dt.Rows.Count > 0)
            {
                dgvDanhSachKH.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Không tìm thấy Khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
