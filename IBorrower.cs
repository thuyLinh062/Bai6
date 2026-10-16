//Họ và Tên: Trần Thùy Linh
//MSSV: 202418935
using System;

namespace QuanLyThietBi
{
    public interface IBorrower
    {
        string BorrowerId { get; }
        string FullName { get; }
        int GetMaximumDevices();
    }
}