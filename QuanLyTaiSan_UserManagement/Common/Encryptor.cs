using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using QuanLyTaiSan_UserManagement.Models;
namespace QuanLyTaiSan_UserManagement
{
    public static class Encryptor
    {
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //Chuyển kiểu chuổi thành kiểu byte
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
            //mã hóa chuỗi đã chuyển
            byte[] result = md5.Hash;
            //tạo đối tượng StringBuilder (làm việc với kiểu dữ liệu lớn)
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }
    }
}