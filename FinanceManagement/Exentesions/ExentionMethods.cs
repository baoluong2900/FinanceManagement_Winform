using FinanceManagement.Message;
using FinanceManagement.ModelView;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using static FinanceManagement.ModelTemp.ModelTemp;
using static FinanceManagement.ModelView.ValueListView;

namespace FinanceManagement.Exentesions
{
    public class ExentionMethods
    {
        public dbFinanceManagementEntities dbcontext = new dbFinanceManagementEntities();
        public static ValueListView valueListView = new ValueListView();

        public static ValueView CheckNullValueText(string value, string text)
        {
            if (string.IsNullOrEmpty(value?.Trim()))
            {
                return new ValueView(text, MessagesValue.ValueIsNotNull);
            }
            return null;
        }

        public static ValueView CheckNullValueCbx(int value, string text)
        {
            if (value == -1)
            {
                return new ValueView(text, MessagesValue.ValueIsNotNull);
            }
            return null;
        }

        public static bool CheckComparePassword(string passwordFirst, string passwordSecond)
        {
            return passwordFirst.Trim().ToLower() != passwordSecond.Trim().ToLower();
        }

        public static User CreateAccountUser(string userName, string password)
        {
            var obj = new User();
            obj.createDate = DateTime.Now;
            obj.userName = userName;
            obj.active = true;
            obj.passWord = HashPassword(password);
            return obj;
        }

        public static bool CheckSexUser(string sex, string typeCombx)
        {
            return sex == typeCombx;
        }
        public static string GetSexUser(string sex)
        {
            if (sex?.Trim() == HardValue.Female) return HardValue.FemaleVN;
            if (sex?.Trim() == HardValue.Male) return HardValue.MaleVN;
            if (sex?.Trim() == HardValue.Other) return HardValue.OtherVN;
            return string.Empty;
        }

        public static string CheckdSexUser(bool isMale, bool isFemale, bool isOther)
        {
            if (isMale) return HardValue.Male;
            if (isFemale) return HardValue.Female;
            if (isOther) return HardValue.Other;
            return string.Empty;
        }

        public static string GetFullName(string lastName, string firstName)
        {
            return lastName + " " + firstName;
        }
        public static string GetStatus(bool status)
        {
            return status ? HardValue.Online : HardValue.Offline;
        }

        public static DateTime? FormatDateMMddyyyy(DateTime? value)
        {
            DateTime parsedDate;
            if (DateTime.TryParseExact(value?.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                return parsedDate.Date; // Trả về đối tượng DateTime nếu chuyển đổi thành công
            }
            return DateTime.Now.Date; ;
        }

        public static DateTime? FormatDateddMMyyyy(DateTime? value)
        {
            DateTime parsedDate;
            if (DateTime.TryParseExact(value?.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                return parsedDate.Date; // Trả về đối tượng DateTime nếu chuyển đổi thành công
            }
            return DateTime.Now.Date;
        }

        public static string FormatDateddMMyyyyString(DateTime? value)
        {
            //DateTime parsedDate;
            //if (DateTime.TryParseExact(value?.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            //{
            //    return parsedDate.Date; // Trả về đối tượng DateTime nếu chuyển đổi thành công
            //}
            //return DateTime.Now.Date;
            if(value != null)
            {
                return value?.ToString("dd/MM/yyyy");
            }
            return string.Empty;
        }

        #region Mã hóa mật khẩu

        public static string HashPassword(string password)
        {
            // Generate a salt and hash the password
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Verify the entered password against the stored hash
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        #endregion Mã hóa mật khẩu

        #region Location

        public static string GetNameLevelLocation(int parent)
        {
            return parent == 0 ? "Tỉnh/Thành" : parent == 1 ? "Quận/Huyện" : "Phường/Xã";
        }

        public static string GetNameParentListLocations(List<Location> locations, string parent)
        {
            if (locations != null && locations.Count > 0)
            {
                if (!string.IsNullOrEmpty(parent))
                {
                    return locations.FirstOrDefault(x => x.locationNo.Trim() == parent.Trim())?.name?.Trim() ?? string.Empty;
                }
            }
            return string.Empty;
        }

        public static string GetNameParentLocation(Location locations, string parent)
        {
            if (locations != null)
            {
                if (!string.IsNullOrEmpty(parent))
                {
                    return locations.name?.Trim();
                }
            }
            return string.Empty;
        }

        #endregion Location

        public static string GetAutoNumber(string value)
        {
            DateTime appointmentTime = DateTime.Now;
            var day = appointmentTime.Day;
            var month = appointmentTime.Month;
            var year = appointmentTime.Year;
            var hour = appointmentTime.Hour;
            var minute = appointmentTime.Minute;
            var second = appointmentTime.Second;

            // Tạo chuỗi autonumber
            var autonumber = value + day.ToString("00") + month.ToString("00") + year.ToString("0000") + hour.ToString("00") + minute.ToString("00") + second.ToString("00");

            return autonumber;
        }

        public static DateTime DateTimeIsNull(DateTime? value)
        {
            if (value.HasValue)
            {
                return value.Value;
            }
            return DateTime.Now;
        }

        public static int DecimalToInt(decimal value)
        {
            int totalAmount;
            if (decimal.TryParse(value.ToString(), out decimal roundedValue))
            {
                totalAmount = Convert.ToInt32(roundedValue);
            }
            else
            {
                totalAmount = 0;
            }
            return totalAmount;
        }

        public static bool SearchByContainsIgnoreCase(string value, string textSearch)
        {
            return !string.IsNullOrEmpty(value) && value.IndexOf(textSearch, StringComparison.OrdinalIgnoreCase) > -1;
        }
        public static string FormatAmountVND(decimal value)
        {
            // Tạo một CultureInfo cho vùng địa lý Việt Nam
            CultureInfo cultureInfo = new CultureInfo("vi-VN");

            // Định dạng số decimal với CultureInfo
            return value.ToString("N0", cultureInfo).Replace('.', ',') + " VNĐ";
        }
        public static List<ComboBoxItem> ConvertLisCbxLocations(List<String> locationsName)
        {
            if (locationsName == null || locationsName.Count <= 0) return null;
            var result = new List<ComboBoxItem>();

            for (int i = 0; i < locationsName.Count; i++)
            {
                var obj = new ComboBoxItem(i, locationsName[i]);
                result.Add(obj);
            }
            return result;
        }

        public static int FindIndexComboxLocations(List<ComboBoxItem> comboxList, string value, List<Location> locations)
        {
            if(locations != null && locations.Count > 0)
            {
                var obj = locations.FirstOrDefault(x => x.locationNo?.Trim() == value?.Trim());
                if (obj != null)
                {
                    return comboxList.FindIndex(x => x.Name.Trim() == obj.name.Trim());
                }
            }

            return -1;
        }
        public static string FindValueComboxLocations(List<ComboBoxItem> comboxList, int index, List<Location> locations)
        {
            if (locations != null && locations.Count > 0)
            {
                var obj = comboxList.FirstOrDefault(x => x.Value == index);
                if (obj != null)
                {
                    return locations.FirstOrDefault(x => x.name.Trim() == obj.Name.Trim()).locationNo.Trim();
                }
            }

            return string.Empty;
        }
        public static List<double> GetListPercelByValues(Dictionary<int?,decimal?> arrays)
        {
            double totalExpenseAmount = double.Parse(arrays.Values.Sum(x => x.Value).ToString());

            var listSum = arrays
                .OrderBy(x => x.Key)
                .Select(x => Math.Round((double)x.Value / totalExpenseAmount * 100, 2))
                .ToList();

            // Kiểm tra tổng của các giá trị trong test
            double roundedTotal = Math.Round(listSum.Sum(), 2);
            if (roundedTotal != 100.0)
            {
                // Điều chỉnh giá trị cuối cùng để tổng là 100%
                listSum[listSum.Count - 1] += 100.0 - roundedTotal;
            }
            return listSum;
        }
        public static decimal GetProgess(int status, decimal value )
        {
            if (status == 0) return 0;
            if (status == 2) return 100;
            return value;
        }
        public static void CreateTransaction(int userID, int category, string name, decimal money)
        {
            var transaction = new Transaction();
            transaction.userID = userID;
            transaction.transactionName = valueListView.CbxListFinancialCategory.FirstOrDefault(x=>x.Value.Equals(category)).Name +" "+ name.Trim()+ " " + ExentionMethods.FormatAmountVND(money);
            transaction.categoryID = category;
            transaction.description = "Tạo giao dịch " + valueListView.CbxListFinancialCategory.FirstOrDefault(x => x.Value.Equals(category)).Name.Trim();
            transaction.createDate = DateTime.Now;
            ExentionQuerys.InsertUpdateObject(transaction);
        }
    }
}