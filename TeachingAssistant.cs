//Họ và Tên: Trần Thùy Linh
//MSSV: 202418935
using System;

namespace QuanLyThietBi
{
    public class TeachingAssistant : IBorrower
    {
        public string BorrowerId { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public TeachingAssistant(string borrowerId, string fullName)
        {
            if(string.IsNullOrWhiteSpace(BorrowerId))
            {
                throw new ArgumentException("ID không được để trống");
            }

             if(string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException("Tên không được để trống");
            }
            
            BorrowerId = borrowerId;
            FullName = fullName;
        }

        public int GetMaximumDevices()
        {
            return 2;
        }
    }
}