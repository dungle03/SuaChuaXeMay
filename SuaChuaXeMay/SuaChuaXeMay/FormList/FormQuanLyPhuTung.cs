using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DevExpress.Utils.Filtering.ExcelFilterOptions;

namespace SuaChuaXeMay.FormList
{
    public partial class FormQuanLyPhuTung : Form
    {
        ProcessDatabase pd = new ProcessDatabase();
        public FormQuanLyPhuTung()
        {
            InitializeComponent();
        }

        private void FormQuanLyPhuTung_Load(object sender, EventArgs e)
        {
            // tắt nút sửa và xóa
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

            // đổ dữ liệu lên datagridview
            dgvDanhSachPT.DataSource = pd.DocBang("select * from PhuTung");
            dgvDanhSachPT.Columns[0].HeaderText = "ID";
            //dgvDanhSachPT.Columns[0].Width = 100;

            dgvDanhSachPT.Columns[1].HeaderText = "Tên Phụ Tùng";
            dgvDanhSachPT.Columns[1].Width = 120;

            dgvDanhSachPT.Columns[2].HeaderText = "Số Lượng";
            dgvDanhSachPT.Columns[3].HeaderText = "Giá Nhập";
            dgvDanhSachPT.Columns[4].HeaderText = "Giá Bán";
            dgvDanhSachPT.Columns[5].HeaderText = "Loại Phụ Tùng";
            dgvDanhSachPT.Columns[5].Width = 120;

            // trỏ vào ô điền mã nhân viên
            txtIDPhuTung.Focus();

            txtGiaBan.Enabled = false;
        }

        private void dgvDanhSachPT_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // bật nút sửa và xóa 
            btnSua.Enabled = true;
            btnXoa.Enabled = true;

            // tắt nút thêm
            btnThem.Enabled = false;
            btnTimKiem.Enabled = false;
            txtIDPhuTung.Enabled = false;
            try
            {
                txtIDPhuTung.Text = dgvDanhSachPT.SelectedRows[0].Cells[0].Value.ToString();
                txtTenPhuTung.Text = dgvDanhSachPT.SelectedRows[0].Cells[1].Value.ToString();
                txtSoLuong.Text = dgvDanhSachPT.SelectedRows[0].Cells[2].Value.ToString();
                txtGiaNhap.Text = dgvDanhSachPT.SelectedRows[0].Cells[3].Value.ToString();
                txtGiaBan.Text = dgvDanhSachPT.SelectedRows[0].Cells[4].Value.ToString();
                cbbLoaiPT.Text = dgvDanhSachPT.SelectedRows[0].Cells[5].Value.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi: " + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            // làm trắng các ô nhập
            txtIDPhuTung.Text = "";
            txtTenPhuTung.Text = "";
            txtSoLuong.Text = "";
            txtGiaNhap.Text = "";
            txtGiaBan.Text = "";
            cbbLoaiPT.Text = "";

            btnThem.Enabled = true;
            btnTimKiem.Enabled = true;
            btnXoa.Enabled = false;
            btnSua.Enabled = false;

            dgvDanhSachPT.DataSource = pd.DocBang("select * from PhuTung");
            txtIDPhuTung.Enabled = true;
            txtIDPhuTung.Focus();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateEmptyField(txtIDPhuTung.Text, "Mã Phụ Tùng", txtIDPhuTung)) return;
            if (!ValidateEmptyField(txtTenPhuTung.Text, "Tên Phụ Tùng", txtTenPhuTung)) return;
            if (!ValidateEmptyField(txtSoLuong.Text, "Số Lượng", txtSoLuong)) return;
            if (!ValidateEmptyField(txtGiaNhap.Text, "Giá Nhập", txtGiaNhap)) return;
            //if (!ValidateEmptyField(txtGiaBan.Text, "Số Lượng", txtSoLuong)) return;
            if (!ValidateEmptyField(cbbLoaiPT.Text, "Loại Phụ Tùng", cbbLoaiPT)) return;

            if (!ValidateLength(txtIDPhuTung.Text, "Mã Phụ Tùng", 10, txtIDPhuTung)) return;

            if (!ValidateRegex(txtTenPhuTung.Text, @"^[a-zA-Z\s\u00C0-\u1EF9\d]+$", "Tên Phụ Tùng không được chứa ký tự đặc biệt", txtTenPhuTung)) return;

            if (!ValidateRegex(txtSoLuong.Text, @"^[1-9][0-9]*$", "Số Lượng phải là số và > 0", txtSoLuong)) return;
            if (!ValidateRegex(txtGiaNhap.Text, @"^[1-9][0-9]*$", "Giá Nhập phải là số và > 0", txtGiaNhap)) return;
           

            //if (!ValidateNumber(txtSoLuong.Text, "Số Lượng", txtSoLuong)) return;
            //if (!ValidateNumber(txtGiaNhap.Text, "Giá Nhập", txtGiaNhap)) return;
            //if (!ValidateNumber(txtGiaBan.Text, "Giá Bán", txtGiaBan)) return;



            // kiểm tra mã nhân viên đã tồn tại hay chưa
            DataTable dt = pd.DocBang($"select * from PhuTung where IDPhuTung = '{txtIDPhuTung.Text}'");
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Mã phụ tùng đã tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIDPhuTung.Focus();
                return;
            }

            
            // thêm phụ tùng, khi nhập giá nhập thì giá bán sẽ tự động cập nhật bằng 110% giá nhập
            string sql = "insert into PhuTung values('" + txtIDPhuTung.Text + "',N'" + txtTenPhuTung.Text + "','" + txtSoLuong.Text + "','" + txtGiaNhap.Text + "','" + (Convert.ToInt32(txtGiaNhap.Text) * 1.1) + "',N'" + cbbLoaiPT.Text + "')";

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

        

        private void btnSua_Click(object sender, EventArgs e)
        {
            //if (!ValidateEmptyField(txtIDPhuTung.Text, "Mã Phụ Tùng", txtIDPhuTung)) return;
            if (!ValidateEmptyField(txtTenPhuTung.Text, "Tên Phụ Tùng", txtTenPhuTung)) return;
            if (!ValidateEmptyField(txtSoLuong.Text, "Số Lượng", txtSoLuong)) return;
            if (!ValidateEmptyField(txtGiaNhap.Text, "Giá Nhập", txtGiaNhap)) return;
            //if (!ValidateEmptyField(txtGiaBan.Text, "Số Lượng", txtSoLuong)) return;
            if (!ValidateEmptyField(cbbLoaiPT.Text, "Loại Phụ Tùng", cbbLoaiPT)) return;

            //if (!ValidateLength(txtIDPhuTung.Text, "Mã Phụ Tùng", 10, txtIDPhuTung)) return;

            if (!ValidateRegex(txtTenPhuTung.Text, @"^[a-zA-Z\s\u00C0-\u1EF9\d]+$", "Tên Phụ Tùng không được chứa ký tự đặc biệt", txtTenPhuTung)) return;


            /*if (!ValidateNumber(txtSoLuong.Text, "Số Lượng", txtSoLuong)) return;
            if (!ValidateNumber(txtGiaNhap.Text, "Giá Nhập", txtGiaNhap)) return;*/
            //if (!ValidateNumber(txtGiaBan.Text, "Giá Bán", txtGiaBan)) return;
            if (!ValidateRegex(txtSoLuong.Text, @"^[1-9][0-9]*$", "Số Lượng phải là số và > 0", txtSoLuong)) return;
            if (!ValidateRegex(txtGiaNhap.Text, @"^[1-9][0-9]*$", "Giá Nhập phải là số và > 0", txtGiaNhap)) return;

            // thông báo cho người dùng có chắc chắn muốn sửa phụ tùng này không
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn sửa phụ tùng này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                // sửa phụ tùng
                string sql = "update PhuTung set TenPhuTung = N'" + txtTenPhuTung.Text + "', SoLuong = '" + txtSoLuong.Text + "', GiaNhap = '" + txtGiaNhap.Text + "', GiaBan = '" + (Convert.ToInt32(txtGiaNhap.Text) * 1.1) + "', LoaiPhuTung = N'" + cbbLoaiPT.Text + "' where IDPhuTung = '" + txtIDPhuTung.Text + "'";
                pd.CapNhat(sql);
                MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                //dgvDanhSachPT.DataSource = pd.DocBang("select * from PhuTung");
                btnLamMoi_Click(sender, e);
            }

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Thông báo cho người dùng có chắc chắn muốn xóa phụ tùng này không
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn xóa phụ tùng này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                // xóa phụ tùng
                string sql = "delete from PhuTung where IDPhuTung = '" + txtIDPhuTung.Text + "'";
                pd.CapNhat(sql);
                MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                //dgvDanhSachPT.DataSource = pd.DocBang("select * from PhuTung");
                btnLamMoi_Click(sender, e);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTenPhuTung.Text) && string.IsNullOrEmpty(txtSoLuong.Text) && string.IsNullOrEmpty(cbbLoaiPT.Text))
            {
                MessageBox.Show("Bạn chưa nhập thông tin tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string sql = "select * from PhuTung where 1=1";

            if (!string.IsNullOrEmpty(txtTenPhuTung.Text))
            {
                sql += " AND TenPhuTung like N'%" + txtTenPhuTung.Text + "%'";
            }

            if (!string.IsNullOrEmpty(txtSoLuong.Text))
            {
                sql += " AND SoLuong like '%" + txtSoLuong.Text + "%'";
            }

            if (!string.IsNullOrEmpty(cbbLoaiPT.Text))
            {
                sql += " AND LoaiPhuTung like N'%" + cbbLoaiPT.Text + "%'";
            }

            DataTable dt = pd.DocBang(sql);

            if (dt.Rows.Count > 0)
            {
                dgvDanhSachPT.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Không tìm thấy phụ tùng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
