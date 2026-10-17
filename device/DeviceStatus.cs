//Họ và Tên: Trần Thùy Linh
//MSSV: 202418935
using System;

//Giống với Device của lab w05
namespace QuanLyThietBi
{
    public enum DeviceStatus
    {
        Active, //đang hoạt động
        UnderMaintenance, //đang bảo trì
        Retired, // đã ngừng hoạt động
        Available, //có sẵn
        Borrowed
    }
}