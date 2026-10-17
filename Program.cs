using System;
using System.Collections.Generic;
using System.Text;
using QuanLyThietBi;

namespace QuanLyThietBi
{
    // Tạo một lớp cụ thể kế thừa từ Device để khởi tạo các thiết bị thực tế trong chương trình
    public class ConcreteDevice : Device
    {
        public ConcreteDevice(string deviceId, string deviceName, int startYears, decimal costPrice, DeviceStatus status, int maxBorrow)
            : base(deviceId, deviceName, startYears, costPrice, status, maxBorrow) { }

        public override decimal CalculateAnnualMaintenanceCost()
        {
            return CostPrice * 0.05m;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("==================================================");
            Console.WriteLine("   CHƯƠNG TRÌNH QUẢN LÝ VÀ MƯỢN TRẢ THIẾT BỊ");
            Console.WriteLine("==================================================\n");

            // ==================================================
            // 11.1. LUỒNG THÀNH CÔNG
            // ==================================================
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("11.1. LUỒNG THÀNH CÔNG");
            Console.WriteLine("--------------------------------------------------");

            try
            {
                // 1. Tạo một giảng viên
                Lecturer lecturer = new Lecturer("GV01", "Nguyễn Văn A");
                Console.WriteLine($"1. Đã tạo giảng viên: {lecturer.FullName} (Mã: {lecturer.BorrowerId})");

                // 2. Tạo một điều khiển máy chiếu, một điều khiển điều hòa và một micro không dây
                Device projectorRemote = new ConcreteDevice("D01", "Điều khiển máy chiếu", 2023, 500000m, DeviceStatus.Available, 5);
                Device acRemote = new ConcreteDevice("D02", "Điều khiển điều hòa", 2023, 400000m, DeviceStatus.Available, 5);
                Device wirelessMic = new ConcreteDevice("D03", "Micro không dây", 2022, 1200000m, DeviceStatus.Available, 3);
                Console.WriteLine("2. Đã tạo các thiết bị:");
                Console.WriteLine($"   - {projectorRemote.DeviceName} ({projectorRemote.DeviceId})");
                Console.WriteLine($"   - {acRemote.DeviceName} ({acRemote.DeviceId})");
                Console.WriteLine($"   - {wirelessMic.DeviceName} ({wirelessMic.DeviceId})");

                // 3. Tạo phiếu mượn cho một phòng dạy
                ClassRoom teachingRoom = new ClassRoom("P101", "Phòng 101", "50");
                DateTime borrowDate = new DateTime(2026, 10, 10);
                BorrowingSlip slip = new BorrowingSlip("PM001", lecturer, teachingRoom, borrowDate);
                Console.WriteLine($"3. Đã tạo phiếu mượn {slip.SlipId} cho phòng {teachingRoom.RoomName} vào ngày {borrowDate:dd/MM/yyyy}.");

                // 4. Thêm điều khiển máy chiếu và micro vào phiếu
                Console.WriteLine("4. Thêm thiết bị vào phiếu mượn:");
                slip.AddDevice(projectorRemote, borrowDays: 2, fromPeriod: 1, toPeriod: 5); // Hạn: 12/10/2026
                slip.AddDevice(wirelessMic, borrowDays: 3, fromPeriod: 6, toPeriod: 10);     // Hạn: 13/10/2026

                // 5. Trả điều khiển máy chiếu đúng hạn
                DateTime projectorReturnDate = new DateTime(2026, 10, 12);
                slip.ReturnDevice("D01", projectorReturnDate);
                Console.WriteLine($"5. Đã trả {projectorRemote.DeviceName} đúng hạn ngày {projectorReturnDate:dd/MM/yyyy}.");

                // 6. Trả micro muộn 2 ngày (Hạn 13/10, trả ngày 15/10)
                DateTime micReturnDate = new DateTime(2026, 10, 15);
                slip.ReturnDevice("D03", micReturnDate);
                Console.WriteLine($"6. Đã trả {wirelessMic.DeviceName} ngày {micReturnDate:dd/MM/yyyy} (Muộn 2 ngày).");

                // 7. Hiển thị các phiếu trả muộn, chưa trả & tổng tiền phạt
                Console.WriteLine("\n7. Trạng thái phiếu mượn & Phí phạt:");
                Console.WriteLine($"   - Tổng phí phạt trả muộn: {slip.CalculateTotalLateFee():N0} VNĐ");
                
                foreach (var item in slip.Items)
                {
                    string status = item.IsReturned 
                        ? (item.ReturnedDate.Value.Date > item.DueDate.Date ? $"Trả muộn (Trả: {item.ReturnedDate:dd/MM/yyyy})" : "Đã trả đúng hạn") 
                        : "Chưa trả";

                    Console.WriteLine($"   + Thiết bị: {item.Device.DeviceName} | Hạn: {item.DueDate:dd/MM/yyyy} | Trạng thái: {status}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi trong luồng thành công: {ex.Message}");
            }


            // ==================================================
            // 11.2. LUỒNG PHÁT SINH NGOẠI LỆ
            // ==================================================
            Console.WriteLine("\n--------------------------------------------------");
            Console.WriteLine("11.2. LUỒNG PHÁT SINH NGOẠI LỆ");
            Console.WriteLine("--------------------------------------------------\n");

            Lecturer defaultLecturer = new Lecturer("GV02", "Trần Văn B");
            ClassRoom defaultRoom = new ClassRoom("P102", "Phòng 102", "40");

            // Tình huống 1: Mượn điều khiển điều hòa đang UnderMaintenance
            Console.WriteLine("[Tình huống 1]: Mượn điều khiển điều hòa đang UnderMaintenance");
            try
            {
                BorrowingSlip slipEx1 = new BorrowingSlip("PM_EX1", defaultLecturer, defaultRoom, DateTime.Now);
                Device acMaintenance = new ConcreteDevice("D02_MAINT", "Điều khiển điều hòa", 2023, 400000m, DeviceStatus.UnderMaintenance, 5);
                
                slipEx1.AddDevice(acMaintenance, 1);
            }
            catch (DeviceUnavailableException ex)
            {
                Console.WriteLine($" -> Bắt ngoại lệ thành công: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" -> Ngoại lệ khác: {ex.Message}");
            }

            // Tình huống 2: Trợ giảng mượn thiết bị thứ ba (Giới hạn trợ giảng là 2)
            Console.WriteLine("\n[Tình huống 2]: Trợ giảng mượn thiết bị thứ ba");
            try
            {
                TeachingAssistant ta = new TeachingAssistant("TA01", "Lê Văn C");
                BorrowingSlip slipEx2 = new BorrowingSlip("PM_EX2", ta, defaultRoom, DateTime.Now);

                Device dev1 = new ConcreteDevice("D10", "Thiết bị 1", 2023, 100000m, DeviceStatus.Available, 5);
                Device dev2 = new ConcreteDevice("D11", "Thiết bị 2", 2023, 100000m, DeviceStatus.Available, 5);
                Device dev3 = new ConcreteDevice("D12", "Thiết bị 3", 2023, 100000m, DeviceStatus.Available, 5);

                slipEx2.AddDevice(dev1, 1);
                slipEx2.AddDevice(dev2, 1);
                
                // Cố gắng thêm thiết bị thứ 3
                slipEx2.AddDevice(dev3, 1);
            }
            catch (BorrowLimitExceededException ex)
            {
                Console.WriteLine($" -> Bắt ngoại lệ thành công: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" -> Ngoại lệ khác: {ex.Message}");
            }

            // Tình huống 3: Thêm cùng một micro hai lần
            Console.WriteLine("\n[Tình huống 3]: Thêm cùng một micro hai lần");
            try
            {
                BorrowingSlip slipEx3 = new BorrowingSlip("PM_EX3", defaultLecturer, defaultRoom, DateTime.Now);
                Device micDuplicate = new ConcreteDevice("D03_DUP", "Micro không dây", 2022, 1200000m, DeviceStatus.Available, 3);

                slipEx3.AddDevice(micDuplicate, 1);
                
                // Thêm lại chính thiết bị đó lần 2
                slipEx3.AddDevice(micDuplicate, 1);
            }
            catch (DuplicateDeviceException ex)
            {
                Console.WriteLine($" -> Bắt ngoại lệ thành công: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" -> Ngoại lệ khác: {ex.Message}");
            }

            // Tình huống 4: Mượn micro với số ngày không hợp lệ (ví dụ <= 0 ngày)
            Console.WriteLine("\n[Tình huống 4]: Mượn micro với số ngày mượn không hợp lệ (<= 0)");
            try
            {
                BorrowingSlip slipEx4 = new BorrowingSlip("PM_EX4", defaultLecturer, defaultRoom, DateTime.Now);
                Device micInvalidDay = new ConcreteDevice("D03_INV", "Micro không dây", 2022, 1200000m, DeviceStatus.Available, 3);

                // Mượn 0 ngày
                slipEx4.AddDevice(micInvalidDay, borrowDays: 0);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($" -> Bắt ngoại lệ thành công: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" -> Ngoại lệ khác: {ex.Message}");
            }

            // Tình huống 5: Trả một điều khiển máy chiếu không thuộc phiếu
            Console.WriteLine("\n[Tình huống 5]: Trả một điều khiển máy chiếu không thuộc phiếu");
            try
            {
                BorrowingSlip slipEx5 = new BorrowingSlip("PM_EX5", defaultLecturer, defaultRoom, DateTime.Now);

                // Trả mã thiết bị chưa từng được thêm vào phiếu
                slipEx5.ReturnDevice("D_UNKNOWN", DateTime.Now);
            }
            catch (DeviceNotInSlipException ex)
            {
                Console.WriteLine($" -> Bắt ngoại lệ thành công: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" -> Ngoại lệ khác: {ex.Message}");
            }

            // Tình huống 6: Trả cùng một thiết bị hai lần
            Console.WriteLine("\n[Tình huống 6]: Trả cùng một thiết bị hai lần");
            try
            {
                BorrowingSlip slipEx6 = new BorrowingSlip("PM_EX6", defaultLecturer, defaultRoom, DateTime.Now);
                Device devToReturnTwice = new ConcreteDevice("D20", "Thiết bị test trả 2 lần", 2023, 200000m, DeviceStatus.Available, 5);

                slipEx6.AddDevice(devToReturnTwice, 1);
                
                // Trả lần 1
                slipEx6.ReturnDevice("D20", DateTime.Now);
                
                // Trả lần 2
                slipEx6.ReturnDevice("D20", DateTime.Now);
            }
            catch (DeviceAlreadyReturnedException ex)
            {
                Console.WriteLine($" -> Bắt ngoại lệ thành công: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" -> Ngoại lệ khác: {ex.Message}");
            }

            // Tình huống 7: Trả thiết bị với ngày trả trước ngày mượn
            Console.WriteLine("\n[Tình huống 7]: Trả thiết bị với ngày trả trước ngày mượn");
            try
            {
                DateTime borrowDateEx7 = new DateTime(2026, 10, 10);
                BorrowingSlip slipEx7 = new BorrowingSlip("PM_EX7", defaultLecturer, defaultRoom, borrowDateEx7);
                Device devWrongDate = new ConcreteDevice("D21", "Thiết bị test ngày trả", 2023, 200000m, DeviceStatus.Available, 5);

                slipEx7.AddDevice(devWrongDate, 2);

                // Ngày trả (05/10) trước ngày mượn (10/10)
                DateTime invalidReturnDate = new DateTime(2026, 10, 5);
                slipEx7.ReturnDevice("D21", invalidReturnDate);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($" -> Bắt ngoại lệ thành công: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" -> Ngoại lệ khác: {ex.Message}");
            }

            Console.WriteLine("\n==================================================");
            Console.WriteLine("        HOÀN THÀNH CHẠY CHƯƠNG TRÌNH TEST");
            Console.WriteLine("==================================================");
        }
    }
}