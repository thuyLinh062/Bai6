//Họ và Tên: Trần Thùy Linh
//MSSV: 202418935
using System;

namespace QuanLyThietBi
{
    public class ProjectorRemote : Device
    {
        public ProjectorRemote(
            string deviceId, 
            string deviceName, 
            int startYears, 
            decimal costPrice, 
            DeviceStatus status, 
            int MaxBorrow) 
            : base (deviceId, deviceName, startYears, costPrice, status, MaxBorrow)
        {}

        //Cung cấp phương thức trừu tượng tính chi phí bảo trì dự kiến trong một năm.
        public override decimal CalculateAnnualMaintenanceCost()
        {
            return CostPrice * 0.05m;
        }
    }
}