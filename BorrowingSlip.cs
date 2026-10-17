//Họ và Tên: Trần Thùy Linh
//MSSV: 202418935
using System;

namespace QuanLyThietBi
{

    //Phiếu mượn đồ
    public class BorrowingSlip
    {
        public string SlipId { get; } //ID
        public IBorrower Borrower { get; } //Người mượn
        public ClassRoom TeachingRoom { get; } //Phòng
        public DateTime BorrowedDate { get; } //Ngày mượn

        public readonly List<BorrowItem> Items;
        public IReadOnlyList<BorrowItem> items => Items;

        public BorrowingSlip(
            string slipId,
            IBorrower borrower,
            ClassRoom teachingRoom,
            DateTime borrowedDate
        )
        {
            if (string.IsNullOrWhiteSpace(slipId))
            {
                throw new ArgumentException(
                    "Mã phiếu không được để trống.");
            }

            //Người mượn chưa đạt giới hạn thiết bị
            if (items.Count >= Borrower.GetMaximumDevices())
            {
                throw new BorrowLimitExceededException(
                    Borrower.BorrowerId,
                    Borrower.GetMaximumDevices());
            }

            if (borrower == null)
            {
                throw new ArgumentNullException(
                    nameof(borrower), "Người mượn không được để trống");
            }

            if (teachingRoom == null)
            {
                throw new ArgumentNullException(
                    nameof(teachingRoom), "Phòng học không được để trống");
            }
            
            SlipId = slipId;
            Borrower = borrower;
            TeachingRoom = teachingRoom;
            BorrowedDate = borrowedDate.Date;
            Items = new List<BorrowItem>();
        }

        //Nếu không nhập số tiết thì gán tự động số tiết từ 1-10
        public void AddDevice(Device device, int borrowDays, int fromPeriod = 1, int toPeriod = 10)
        {
            if(device == null)
            {
                throw new ArgumentException(nameof(device), "Thiết bị không được để trống.");
            }

            if(borrowDays <=0)
            {
                throw new ArgumentException(nameof(borrowDays), "Số ngày mượn phải lớn hơn 0.");
            }

            DateTime dueDate = BorrowedDate.AddDays(borrowDays);

            bool isExisted = Items.Any(item => item.Device.DeviceId == device.DeviceId);

            // Kiểm tra xem thiết bị có tồn tại trong phiếu không.
            if (!isExisted)
            {
                throw new ArgumentException("Thiết bị chưa tồn tại trong phiếu!");
            }

            //Kiểm tra xem người mượn đã đạt đến giới hạn chưa 
            if(device.Quantity );

            BorrowItem newItem = new BorrowItem(device, BorrowedDate, dueDate, fromPeriod, toPeriod);

            Items.Add(newItem);
            Console.WriteLine($"Thêm thiết bị {device.DeviceName} ({device.DeviceId}) vào phiếu mượn thành công.");
        }

        public decimal ReturnDevice(string deviceId, DateTime returnedDate)
        {
            if(string.IsNullOrWhiteSpace(deviceId))
            {
                throw new ArgumentException(nameof(deviceId), "Id thiết bị không được để trống.");
            }

            bool isExisted = Items.Any(item => item.Device.DeviceId == deviceId);

            if (!isExisted)
            {
                throw new ArgumentException("Không tồn tại deviceID này!");
            }

            TimeSpan returnDevice = returnedDate - BorrowedDate;
            decimal soNgay = (decimal)returnDevice.TotalDays;
            return soNgay;
        }

        public decimal CalculateTotalLateFee()
        {
            DateTime date;

            decimal total = 0;
            decimal finePerDay = 10000m;

            foreach( BorrowItem borrowItem in Items)
            {
                if (borrowItem.IsReturned)
                {
                    date = borrowItem.ReturnedDate.Value.Date;
                }
                else
                {
                    date = DateTime.Now;
                }

                if(date > borrowItem.DueDate.Date)
                {
                    int lateDays = (date -borrowItem.DueDate.Date).Days;

                    total +=lateDays*finePerDay;
                }
            }

            return total;
        }
    }
}