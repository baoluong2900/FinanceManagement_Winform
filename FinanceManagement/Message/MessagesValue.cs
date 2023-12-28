namespace FinanceManagement.Message
{
    public class MessagesValue
    {
        public const string EmptyValue = "Vui lòng nhập ";

        public const string ValueIsNotNull = " không được phép bỏ trống";
        public const string TextSearchIsNotNull = "Giá trị tìm kiếm không được phép bỏ trống";

        public const string PasswordsDoNotMatch = "Mật khẩu nhập lại không khớp";

        public const string RegisterSuccess = "Đăng ký tài khoản thành công";

        public const string InsertSuccess = "Thêm mới thành công";

        public const string UpdateSuccess = "Cập nhật thành công";
        public const string DeleteSuccess = "Xóa thành công";

        public const string UserIsNotExist = "Tài khoản chưa tồn tại";

        public const string UserIsExist = "Tài khoản đã tồn tại";

        public const string PasswordIsFail = "Mật khẩu không đúng";

        public const string LocationIsExist = "Thông tin vị trí đã tồn tại";

        public const string MesageCopyObject = "Không thể copy 2 đối tượng khác dữ liệu";

        public const string SelectItemUpdate = "Vui lòng chọn dữ liệu để cập nhật";
        public const string SelectItemDelete = "Vui lòng chọn dữ liệu để xóa";

        public const string MessageUpdateIsFail = "Cập nhật dữ liệu không thành công";
        public const string MessageShowDelete = "Bạn có muốn xóa dữ liệu không?";

        public const string MessageDeleteAllTransaction = "Bạn có muốn xóa toàn bộ giao dịch không?";
        public const string MessageConfirm = "Xác nhận";

        // Error try catch
        public const string MessageErrorSystem = "Cập nhật thất bại";

        public static string GetNameMessageDelete(string valueName)
        {
            return "Bạn có muốn xóa " + "'" + valueName + "'" + " không?";
        }
    }

    public class HardValue
    {
        public const string Female = "Female";

        public const string Male = "Male";

        public const string Other = "Other";
        public const string FemaleVN = "Nữ";

        public const string MaleVN = "Nam";

        public const string OtherVN = "Khác";

        public const string DateTimeCreate = "DateTimeCreate";

        public const string DateTimeModifiedOn = "DateTimeModifiedOn";
        public const string Level = "Level";

        public const string Online = "Online";

        public const string Offline = "Offline";
    }
}