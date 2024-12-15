using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace QuanLyTaiSan_UserManagement.Helper
{
    public class eStatus
    {
        public enum DeviceStatus
        {
            [Description("Rảnh")]
            Free = 0,
            [Description("Đang sử dụng")]
            Project,
            [Description("Đã Thanh Lý")]
            Liquidation,
            [Description("Đang Bị Hỏng")]
            broken,
        }
        public enum DeviceStatusT
        {
            [Description("")]
            Free = 0,
            [Description("")]
            Project,
            [Description("")]
            Liquidation,
            [Description("Đang Hỏng")]
            broken,
        }
        public enum RepairStatus
        {
            [Description("")]
            Free = 0,
            [Description("Đang Sửa")]
            Repair,
        }
        public enum UserStatus
        {
            [Description("Đang Hoạt Động")]
            On = 0,
            [Description("Đã Nghỉ")]
            Off,
        }
        public enum SupplierStatus
        {
            [Description("Đang Hoạt Động")]
            On = 0,
            [Description("Đã Đóng Cửa")]
            Off,
        }
        public enum ProjectStatus
        {
            [Description("Đang hoạt động")]
            Action = 1,
            [Description("Dừng hoạt động")]
            Pause,
            //[Description("Kết thúc")]
            //Finish,
        }
        public enum RequestDeviceStatus
        {
            [Description("Đang chờ duyệt")]
            NoAction = 0,
            [Description("Đã được duyệt")]
           Action ,
            [Description("Bị từ chối")]
            ActionFail,
        }
        public enum RequestApproved
        {
            [Description("Chưa phê duyệt")]
            NoAction = 0,
            [Description("Đã phê duyệt")]
            Action,
        }
        public enum StatusScheduleTest
        {
            [Description("Chưa kiểm tra")]
            NoAction = 0,
            [Description("Đã kiểm tra")]
            Action,
        }
        public enum StatusRepairDevice
        {
            [Description("Đang sửa ")]
            NoAction = 0,
            [Description("Đã sửa xong")]
            Action,
        }
        public enum StatusOfProject
        {
            [Description("Trả Về Kho")]
            Action = 0,
            [Description("Trong Phòng ban")]
            NoAction
            ,
        }

        public enum ControllerId
        {
            [Description("Device (Thiết bị)")]
            Device = 1,
            [Description("Project (Dự án)")]
            Project,
            [Description("RepairDetails (Yêu cầu sửa chữa)")]
            RepairDetails,
        }

        public enum Type_TypeCom
        {
            [Description("Thiết bị ngoài")]
            Dvout = 0,
            [Description("Thiết bị trong")]
            Dvint,
           
        }

    }

   
       
   

}