//Họ và Tên: Trần Thùy Linh
//MSSV: 202418935
using System;

namespace QuanLyThietBi
{
    //Thiết bị không ở trạng thái Available
    public class DeviceUnavailableException : Exception
    {
        public string DeviceId { get; }
        public DeviceStatus CurrentStatus { get; }

        public DeviceUnavailableException(string deviceId, DeviceStatus currentStatus)
        :base($"Thiết bị {deviceId} không còn có sẵn")
        {
            DeviceId = deviceId;
            CurrentStatus = currentStatus;
        }
    }

    //Người mượn đạt số thiết bị tối đa
    public class BorrowLimitExceededException : Exception
    {
        public string BorrowerId { get; }
        public int MaximumDevices { get; }

        public BorrowLimitExceededException(string borrowerId, int maximumDevices)
        :base($"Người mượn {borrowerId} đã đạt giới hạn {maximumDevices}")
        {
            BorrowerId =borrowerId;
            MaximumDevices = maximumDevices;
        }
    }

    //Số ngày mượn không dương
    public class InvalidBorrowPeriodException : Exception
    {
        public string DeviceId {get;}
        public int RequestedPeriods { get; }
        public int MaximumPeriods { get; }

        public InvalidBorrowPeriodException(
            string deviceId, int requestedPeriods, int maximumPeriods)
            : base($"Thiết bị {deviceId}: số tiết yêu cầu không hợp lệ. " +
                $"Yêu cầu: {requestedPeriods}, tối đa: {maximumPeriods}.")
        {
            DeviceId = deviceId;
            RequestedPeriods = requestedPeriods;
            MaximumPeriods = maximumPeriods;
        }
    }

    //Phiếu mượn đã có thiết bị
    public class DuplicateDeviceException : Exception
    {
        public DuplicateDeviceException(string deviceId)
            : base($"Thiết bị {deviceId} đã có trong phiếu mượn.")
        {
        }
    }

    //Trả một thiết bị ko thuộc phiếu
    public class DeviceNotInSlipException : Exception
    {
        public DeviceNotInSlipException(string deviceId)
            : base($"Thiết bị {deviceId} không có trong phiếu mượn.")
        {
        }
    }

    //Thiết bị trong phiếu đã được trả trước đó.
    public class DeviceAlreadyReturnedException : Exception
    {
        public DeviceAlreadyReturnedException(string deviceId)
            : base($"Thiết bị {deviceId} đã được trả trước đó.")
        {
        }
    }

}