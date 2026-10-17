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

            //Người mượn chưa đạt giới hạn thiết bị
            if (items.Count >= borrower.GetMaximumDevices())
            {
                throw new BorrowLimitExceededException(
                    borrower.BorrowerId,
                    borrower.GetMaximumDevices());
            }
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

            // Kiểm tra xem thiết bị có tồn tại trong phiếu không.
            bool isExisted = Items.Any(item => item.Device.DeviceId == device.DeviceId);
            if (isExisted)
            {
                throw new DuplicateDeviceException(device.DeviceId);
            }

            //Kiểm tra xem người mượn đã đạt đến giới hạn chưa 
            if(Items.Count >= Borrower.GetMaximumDevices())
            {
                throw new BorrowLimitExceededException(Borrower.BorrowerId, Borrower.GetMaximumDevices());
            }

            //Kiểm tra xem thiết bị có sẵn không 
            bool isAvailable = device.Status == DeviceStatus.Available;
            if(!isAvailable)
            {
                throw new DeviceUnavailableException(device.DeviceId, device.Status);
            }

            DateTime dueDate = BorrowedDate.AddDays(borrowDays);
            BorrowItem newItem = new BorrowItem(device, BorrowedDate, dueDate,  fromPeriod, toPeriod);

            Items.Add(newItem);
            device.Status = DeviceStatus.Borrowed; //Chuyển đổi status của device thành borrowed

            Console.WriteLine($"Thêm thiết bị {device.DeviceName} ({device.DeviceId}) vào phiếu mượn thành công.");
        }

        public void ReturnDevice(string deviceId, DateTime returnedDate)
        {
            if(string.IsNullOrWhiteSpace(deviceId))
            {
                throw new ArgumentException(nameof(deviceId), "Id thiết bị không được để trống.");
            }

            BorrowItem? item = items.FirstOrDefault(
                i => string.Equals(
                    i.Device.DeviceId,
                    deviceId,
                    StringComparison.OrdinalIgnoreCase));

            if (item == null)
                throw new DeviceNotInSlipException(deviceId);

            item.Return(returnedDate);
            item.Device.Status = DeviceStatus.Available;
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