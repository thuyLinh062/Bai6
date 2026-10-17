//Họ và Tên: Trần Thùy Linh
//MSSV: 202418935
using System;

//Giống với Device của lab w05
namespace QuanLyThietBi
{   
    public abstract class Device
    {
        // Các thuộc tính chung

        //Mã thiết bị
        public string DeviceId { get; init; }

        //Tên thiết bị
        public string DeviceName { get; set; }

        //Năm đưa vào sử dụng
        public int StartYears { get; set; }

        //Giá mua
        public decimal CostPrice { get; set; }

        //Trạng thái hoạt động 
        public DeviceStatus Status {get; set;}

        //Số tiết được mượn tối đa
        public int MaxBorrow {get; }
        
        protected Device(
            string deviceId, 
            string deviceName, 
            int startYears, 
            decimal costPrice,
            DeviceStatus status, 
            int MaxBorrow)
        {
            if(string.IsNullOrWhiteSpace(deviceId))
            {
                throw new ArgumentException("ID không được để trống");
            }

            if (costPrice <= 0)
            {
                throw new ArgumentException("Giá mua phải lớn hơn 0.");
            }

            //Năm đưa vào sử dụng không được lớn hơn năm hiện tại.
            int currentYear = DateTime.Now.Year;

            if (startYears > currentYear)
            {
                throw new ArgumentException("Năm đưa vào sử dụng không được lớn hơn năm hiện tại.");
            }

            this.DeviceId = deviceId;
            this.DeviceName = deviceName;
            this.StartYears = startYears;
            this.CostPrice =costPrice;
            this.Status = status;
            this.MaxBorrow = MaxBorrow;
        }

        //Cung cấp phương thức trừu tượng tính chi phí bảo trì dự kiến trong một năm.
        public abstract decimal CalculateAnnualMaintenanceCost();

        //Ghi đè phương thức biểu diễn thông tin của thiết bị dưới dạng chuỗi
        public override string ToString()
        {
            return $"[Mã: {DeviceId}] - Tên: {DeviceName} | Năm SX: {StartYears} | Giá: {CostPrice:N0} VNĐ | Trạng thái: {Status}";
        }
    }
}