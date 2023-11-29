Create Database SuaChuaXeMay
Go

Use SuaChuaXeMay
Go
----------------------------------------------------------------------------
CREATE TABLE NhaCungCap (
    IDNhaCungCap VARCHAR(10) PRIMARY KEY,
    TenNhaCungCap NVARCHAR(100),
    ThongTinLienLac NVARCHAR(200)
);

CREATE TABLE PhuTung (
    IDPhuTung VARCHAR(10) PRIMARY KEY,
    TenPhuTung NVARCHAR(100),
    SoLuong INT,
    GiaNhap DECIMAL(18, 2),
    GiaBan DECIMAL(18, 2),
    LoaiPhuTung NVARCHAR(50)
);

CREATE TABLE NhanVien (
    IDNhanVien VARCHAR(10) PRIMARY KEY,
    TenNhanVien NVARCHAR(100),
    NamSinh DATE,
    TrinhDo NVARCHAR(50)
);

CREATE TABLE HoaDonNhapHang (
    IDHoaDon VARCHAR(10) PRIMARY KEY,
    IDNhaCungCap VARCHAR(10) FOREIGN KEY REFERENCES NhaCungCap(IDNhaCungCap),
    TongTien DECIMAL(18, 2),
    NgayNhap DATE
);

CREATE TABLE ChiTietHoaDonNhapHang (
    IDHoaDon VARCHAR(10) FOREIGN KEY REFERENCES HoaDonNhapHang(IDHoaDon),
    IDPhuTung VARCHAR(10) FOREIGN KEY REFERENCES PhuTung(IDPhuTung),
    SoLuong INT,
    GiaNhap DECIMAL(18, 2),
    PRIMARY KEY (IDHoaDon, IDPhuTung)
);

CREATE TABLE YeuCauSuaChua (
    IDYeuCau VARCHAR(10) PRIMARY KEY,
    IDNhanVien VARCHAR(10) FOREIGN KEY REFERENCES NhanVien(IDNhanVien),
    TongTien DECIMAL(18, 2),
    NgaySuaChua DATE
);

CREATE TABLE ChiTietYeuCauSuaChua (
    IDYeuCau VARCHAR(10) FOREIGN KEY REFERENCES YeuCauSuaChua(IDYeuCau),
    IDPhuTung VARCHAR(10) FOREIGN KEY REFERENCES PhuTung(IDPhuTung),
    SoLuong INT,
    GiaBan DECIMAL(18, 2),
    PRIMARY KEY (IDYeuCau, IDPhuTung)
);

CREATE TABLE KhachHang (
    IDKhachHang VARCHAR(10) PRIMARY KEY,
    TenKhachHang NVARCHAR(100),
    ThongTinLienLac NVARCHAR(200)
);

CREATE TABLE HoaDonBanHang (
    IDHoaDon VARCHAR(10) PRIMARY KEY,
    IDKhachHang VARCHAR(10) FOREIGN KEY REFERENCES KhachHang(IDKhachHang),
    TongTien DECIMAL(18, 2),
    NgayBan DATE
);

CREATE TABLE ChiTietHoaDonBanHang (
    IDHoaDon VARCHAR(10) FOREIGN KEY REFERENCES HoaDonBanHang(IDHoaDon),
    IDPhuTung VARCHAR(10) FOREIGN KEY REFERENCES PhuTung(IDPhuTung),
    SoLuong INT,
    GiaBan DECIMAL(18, 2),
    PRIMARY KEY (IDHoaDon, IDPhuTung)
);

CREATE TABLE TaiKhoan (
    IDTaiKhoan VARCHAR(10) PRIMARY KEY,
    TenDangNhap NVARCHAR(50),
    MatKhau NVARCHAR(50),
    IDNhanVien VARCHAR(10) FOREIGN KEY REFERENCES NhanVien(IDNhanVien)
);

----------------------------------------------------------------------------

-- Bảng NhaCungCap
INSERT INTO NhaCungCap (IDNhaCungCap, TenNhaCungCap, ThongTinLienLac)
VALUES 
('NCC01', N'Công ty TNHH Phụ tùng xe máy Hà Nội', N'Hà Nội'),
('NCC02', N'Công ty TNHH Phụ tùng xe máy Sài Gòn', N'TP.HCM'),
('NCC03', N'Công ty TNHH Phụ tùng xe máy Đà Nẵng', N'Đà Nẵng'),
('NCC04', N'Công ty TNHH Phụ tùng xe máy Cần Thơ', N'Cần Thơ'),
('NCC05', N'Công ty TNHH Phụ tùng xe máy Hải Phòng', N'Hải Phòng');

-- Bảng PhuTung
INSERT INTO PhuTung (IDPhuTung, TenPhuTung, SoLuong, GiaNhap, GiaBan, LoaiPhuTung)
VALUES 
('PT01', N'Bình xăng con', 100, 50000, 55000, N'Loại 1'),
('PT02', N'Bu-gi', 200, 60000, 66000, N'Loại 2'),
('PT03', N'Dây curoa', 300, 70000, 77000, N'Loại 3'),
('PT04', N'Đèn pha', 400, 80000, 88000, N'Loại 4'),
('PT05', N'Ổ bi', 500, 90000, 99000, N'Loại 5');

INSERT INTO PhuTung (IDPhuTung, TenPhuTung, SoLuong, GiaNhap, GiaBan, LoaiPhuTung)
VALUES 
('PT06', N'Phanh đĩa', 150, 30000, 33000, N'Loại 1'),
('PT07', N'Giảm xóc', 250, 80000, 88000, N'Loại 2'),
('PT08', N'Ổ bi', 350, 90000, 99000, N'Loại 3'),
('PT09', N'Nhông xích', 450, 100000, 110000, N'Loại 4'),
('PT10', N'Bánh xe', 550, 120000, 132000, N'Loại 5');

select * from PhuTung

-- Bảng NhanVien
INSERT INTO NhanVien (IDNhanVien, TenNhanVien, NamSinh, TrinhDo)
VALUES 
('NV01', N'Nguyễn Văn A', '1990-01-01', N'Đại học'),
('NV02', N'Trần Văn B', '1991-01-01', N'Cao đẳng'),
('NV03', N'Lê Văn C', '1992-01-01', N'Trung cấp'),
('NV04', N'Phạm Văn D', '1993-01-01', N'Đại học'),
('NV05', N'Vũ Văn E', '1994-01-01', N'Cao đẳng');

Select * from NhanVien

-- Bảng HoaDonNhapHang
INSERT INTO HoaDonNhapHang (IDHoaDon, IDNhaCungCap, TongTien, NgayNhap)
VALUES 
('HDNH01', 'NCC01', 5000000, '2023-11-29'),
('HDNH02', 'NCC02', 6000000, '2023-11-29'),
('HDNH03', 'NCC03', 7000000, '2023-11-29'),
('HDNH04', 'NCC04', 8000000, '2023-11-29'),
('HDNH05', 'NCC05', 9000000, '2023-11-29');

-- Bảng ChiTietHoaDonNhapHang
INSERT INTO ChiTietHoaDonNhapHang (IDHoaDon, IDPhuTung, SoLuong, GiaNhap)
VALUES 
('HDNH01', 'PT01', 100, 50000),
('HDNH02', 'PT02', 200, 60000),
('HDNH03', 'PT03', 300, 70000),
('HDNH04', 'PT04', 400, 80000),
('HDNH05', 'PT05', 500, 90000);

-- Bảng YeuCauSuaChua
INSERT INTO YeuCauSuaChua (IDYeuCau, IDNhanVien, TongTien, NgaySuaChua)
VALUES 
('YCSC01', 'NV01', 5500000, '2023-11-29'),
('YCSC02', 'NV02', 6600000, '2023-11-29'),
('YCSC03', 'NV03', 7700000, '2023-11-29'),
('YCSC04', 'NV04', 8800000, '2023-11-29'),
('YCSC05', 'NV05', 9900000, '2023-11-29');

-- Bảng ChiTietYeuCauSuaChua
INSERT INTO ChiTietYeuCauSuaChua (IDYeuCau, IDPhuTung, SoLuong, GiaBan)
VALUES 
('YCSC01', 'PT01', 10, 55000),
('YCSC02', 'PT02', 20, 66000),
('YCSC03', 'PT03', 30, 77000),
('YCSC04', 'PT04', 40, 88000),
('YCSC05', 'PT05', 50, 99000);

-- Bảng KhachHang
INSERT INTO KhachHang (IDKhachHang, TenKhachHang, ThongTinLienLac)
VALUES 
('KH01', N'Nguyễn Thị A', N'Hà Nội'),
('KH02', N'Trần Thị B', N'TP.HCM'),
('KH03', N'Lê Thị C', N'Đà Nẵng'),
('KH04', N'Phạm Thị D', N'Cần Thơ'),
('KH05', N'Vũ Thị E', N'Hải Phòng');

-- Bảng HoaDonBanHang
INSERT INTO HoaDonBanHang (IDHoaDon, IDKhachHang, TongTien, NgayBan)
VALUES 
('HDBH01', 'KH01', 550000, '2023-11-29'),
('HDBH02', 'KH02', 660000, '2023-11-29'),
('HDBH03', 'KH03', 770000, '2023-11-29'),
('HDBH04', 'KH04', 880000, '2023-11-29'),
('HDBH05', 'KH05', 990000, '2023-11-29');

-- Bảng ChiTietHoaDonBanHang
INSERT INTO ChiTietHoaDonBanHang (IDHoaDon, IDPhuTung, SoLuong, GiaBan)
VALUES 
('HDBH01', 'PT01', 10, 55000),
('HDBH02', 'PT02', 20, 66000),
('HDBH03', 'PT03', 30, 77000),
('HDBH04', 'PT04', 40, 88000),
('HDBH05', 'PT05', 50, 99000);

-- Bảng TaiKhoan
INSERT INTO TaiKhoan (IDTaiKhoan, TenDangNhap, MatKhau, IDNhanVien)
VALUES 
('TK01', 'admin', 'admin123', 'NV01'),
('TK02', 'manager', 'manager123', 'NV02'),
('TK03', 'employee', 'employee123', 'NV03'),
('TK04', 'user', 'user123', 'NV04'),
('TK05', 'guest', 'guest123', 'NV05');
