//Họ và Tên: Trần Thùy Linh
//MSSV: 202418935
using System;

namespace QuanLyThietBi
{
    public class BorrowItem
    {
        public Device Device { get; }
        public DateTime BorrowedDate { get; }
        public DateTime DueDate {get;}
        public int FromPeriod { get; set; }
        public int ToPeriod { get; set; }
        public DateTime? ReturnedDate { get; private set; }

        public bool IsReturned
        {
            get
            {
                return ReturnedDate.HasValue;
            }
        }

        public BorrowItem(
            Device device, 
            DateTime borrowedDate,
            DateTime dueDate,
            int fromPeriod,
            int toPeriod )
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device), "Device không được để trống (NULL)");
            }

            if (dueDate.Date < borrowedDate.Date)
            {
                throw new ArgumentException(
                    "Ngày đến hạn không được trước ngày mượn.");
            }

            if (fromPeriod <= 0 || toPeriod < fromPeriod)
            {
                throw new ArgumentException(
                    "Khoảng tiết mượn không hợp lệ.");
            }

            Device = device;
            BorrowedDate = borrowedDate.Date;
            DueDate = dueDate.Date;
            FromPeriod = fromPeriod;
            ToPeriod = toPeriod;

            ReturnedDate = null;
        }

        
        public void Return(DateTime returnedDate)
        {
            // Không cho phép trả hai lần
            if (IsReturned)
            {
                throw new InvalidOperationException(
                    $"Thiết bị {Device.DeviceId} đã được trả trước đó.");
            }

            //Ngày trả không được trước ngày mượn
            if (returnedDate.Date < BorrowedDate.Date)
            {
                throw new ArgumentException(
                    "Ngày trả không được trước ngày mượn.");
            }

            // Chỉ cập nhật sau khi tất cả điều kiện hợp lệ
            ReturnedDate = returnedDate.Date;
        }

        /*public override string ToString()
        {
            string returnInfo = IsReturned
                ? ReturnedDate!.Value.ToString("dd/MM/yyyy")
                : "Chưa trả";

            return $"Mã thiết bị: {Device.DeviceId}\n" +
                $"Tên thiết bị: {Device.DeviceName}\n" +
                $"Ngày mượn: {BorrowedDate:dd/MM/yyyy}\n" +
                $"Ngày đến hạn: {DueDate:dd/MM/yyyy}\n" +
                $"Tiết mượn: {FromPeriod} - {ToPeriod}\n" +
                $"Ngày trả: {returnInfo}";
        }*/
        //đoạn này xem xét
    }
}