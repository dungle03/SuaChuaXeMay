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
    public partial class FormQuanLyNhanVien : Form
    {
        ProcessDatabase pd = new ProcessDatabase();
        public FormQuanLyNhanVien()
        {
            InitializeComponent();
        }

        private void FormQuanLyNhanVien_Load(object sender, EventArgs e)
        {
            // tắt nút sửa và xóa
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

            // đổ dữ liệu lên datagridview
            dgvDanhSachNV.DataSource = pd.DocBang("select * from NhanVien");
            dgvDanhSachNV.Columns[0].HeaderText = "ID";
            dgvDanhSachNV.Columns[0].Width = 70;

            dgvDanhSachNV.Columns[1].HeaderText = "Tên NV";
            dgvDanhSachNV.Columns[1].Width = 150;

            dgvDanhSachNV.Columns[2].HeaderText = "Ngày Sinh";
            dgvDanhSachNV.Columns[3].HeaderText = "Trình Độ";

            // trỏ vào ô điền mã nhân viên
            txtID.Focus();
        }

        private void dgvDanhSachNV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // bật nút sửa và xóa 
            btnSua.Enabled = true;
            btnXoa.Enabled = true;

            // tắt nút thêm
            btnThem.Enabled = false;
            txtID.Enabled = false;
            try
            {
                txtID.Text = dgvDanhSachNV.SelectedRows[0].Cells[0].Value.ToString();
                txtHoTen.Text = dgvDanhSachNV.SelectedRows[0].Cells[1].Value.ToString();
                //txtNgaysinh.Text = dgvDanhSachNV.SelectedRows[0].Cells[2].Value.ToString();
                dtpNgaySinh.Value = Convert.ToDateTime(dgvDanhSachNV.SelectedRows[0].Cells[2].Value.ToString());
                //txtMatrinhdo.Text = dgvDanhSachNV.SelectedRows[0].Cells[3].Value.ToString();
                //txtTentrinhdo.Text = dgvDanhSachNV.SelectedRows[0].Cells[4].Value.ToString();
                txtTrinhDo.Text = dgvDanhSachNV.SelectedRows[0].Cells[3].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi: " + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            // làm trắng các ô nhập
            txtID.Text = "";
            txtHoTen.Text = "";
            dtpNgaySinh.Value = DateTime.Now;
            txtTrinhDo.Text = "";

            btnThem.Enabled = true;
            btnTimKiem.Enabled = true;
            btnXoa.Enabled = false;
            btnSua.Enabled = false;

            dgvDanhSachNV.DataSource = pd.DocBang("select * from NhanVien");
            txtID.Enabled = true;
            txtID.Focus();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateEmptyField(txtID.Text, "Mã nhân viên", txtID)) return;
            if (!ValidateEmptyField(txtHoTen.Text, "Tên nhân viên", txtHoTen)) return;
            if (!ValidateNgaySinh(dtpNgaySinh.Value, "Ngày sinh", dtpNgaySinh)) return;
            if (!ValidateEmptyField(txtTrinhDo.Text, "Trình độ", txtTrinhDo)) return;

            if (!ValidateLength(txtID.Text, "Mã nhân viên", 10, txtID)) return;
            if (!ValidateRegex(txtHoTen.Text, @"^[a-zA-Z\s\u00C0-\u1EF9]+$", "Tên nhân viên không được chứa số hoặc ký tự đặc biệt", txtHoTen)) return;
            if (!ValidateRegex(txtTrinhDo.Text, @"^[a-zA-Z\s\u00C0-\u1EF9]+$", "Trình độ không được chứa số hoặc ký tự đặc biệt", txtTrinhDo)) return;

            // kiểm tra mã nhân viên đã tồn tại hay chưa
            DataTable dt = pd.DocBang($"select * from NhanVien where IDNhanVien = '{txtID.Text}'");
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Mã nhân viên đã tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtID.Focus();
                return;
            }

            // thêm nhân viên
            string sql = "insert into NhanVien values('" + txtID.Text + "',N'" + txtHoTen.Text + "','" + dtpNgaySinh.Value.ToString("yyyy-MM-dd") + "',N'" + txtTrinhDo.Text + "')";

            pd.CapNhat(sql);
            MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnLamMoi_Click(sender, e);
            dgvDanhSachNV.DataSource = pd.DocBang("select * from NhanVien");
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

        private bool ValidateNgaySinh(DateTime input, string fieldName, Control control)
        {
            // kiểm tra ngày sinh hợp lệ hoặc năm sinh phải lớn hơn 13 tuổi
            if (input > DateTime.Now || DateTime.Now.Year - input.Year < 13)
            {
                MessageBox.Show($"Ngày sinh không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            // thông báo có muốn xóa không
            DialogResult result = MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // xóa dữ liệu trong database
                string sql = "delete from NhanVien where IDNhanVien = '" + txtID.Text + "'";
                pd.CapNhat(sql);

                MessageBox.Show("Xóa nhân viên thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // đổ lại dữ liệu lên datagridview
                dgvDanhSachNV.DataSource = pd.DocBang("select * from NhanVien");
                btnLamMoi_Click(sender, e);
                txtID.Focus();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!ValidateEmptyField(txtHoTen.Text, "Tên nhân viên", txtHoTen)) return;
            if (!ValidateNgaySinh(dtpNgaySinh.Value, "Ngày sinh", dtpNgaySinh)) return;
            if (!ValidateEmptyField(txtTrinhDo.Text, "Trình độ", txtTrinhDo)) return;

            if (!ValidateRegex(txtHoTen.Text, @"^[a-zA-Z\s\u00C0-\u1EF9]+$", "Tên nhân viên không được chứa số hoặc ký tự đặc biệt", txtHoTen)) return;
            if (!ValidateRegex(txtTrinhDo.Text, @"^[a-zA-Z\s\u00C0-\u1EF9]+$", "Trình độ không được chứa số hoặc ký tự đặc biệt", txtTrinhDo)) return;

            DialogResult result = MessageBox.Show("Bạn có chắc muốn sửa nhân viên này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }

            // sửa dữ liệu trong database
            string sql = "update NhanVien set TenNhanVien = N'" + txtHoTen.Text + "', NamSinh = '" + dtpNgaySinh.Value.ToString("yyyy-MM-dd") + "', TrinhDo = N'" + txtTrinhDo.Text + "' where IDNhanVien = '" + txtID.Text + "'";
            pd.CapNhat(sql);

            MessageBox.Show("Sửa nhân viên thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // đổ lại dữ liệu lên datagridview
            dgvDanhSachNV.DataSource = pd.DocBang("select * from NhanVien");
            btnLamMoi_Click(sender, e);
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            // tìm kiếm theo ID
            if (!ValidateEmptyField(txtID.Text, "Mã nhân viên", txtID)) return;
            if (!ValidateLength(txtID.Text, "Mã nhân viên", 10, txtID)) return;

            DataTable dt = pd.DocBang($"select * from NhanVien where IDNhanVien = '{txtID.Text}'");
            if (dt.Rows.Count > 0)
            {
                dgvDanhSachNV.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Không tìm thấy nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
