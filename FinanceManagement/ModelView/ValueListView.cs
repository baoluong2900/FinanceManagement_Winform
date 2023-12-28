using System.Collections.Generic;

namespace FinanceManagement.ModelView
{
    public class ValueListView
    {
        public class ComboBoxItem
        {
            public int Value { get; set; }
            public string Name { get; set; }

            public ComboBoxItem(int value, string name)
            {
                Name = name;
                Value = value;
            }

            public override string ToString()
            {
                return Name;
            }
        }

        public List<ComboBoxItem> CbxListTimes { get; set; } = new List<ComboBoxItem> {
            new ComboBoxItem(0,"Ngày"),
            new ComboBoxItem(1,"Tuần"),
            new ComboBoxItem(2,"Tháng"),
            new ComboBoxItem(3,"Quý"),
            new ComboBoxItem(4,"Năm"),
        };

        public List<ComboBoxItem> CbxListYears { get; set; } = new List<ComboBoxItem> {
            new ComboBoxItem(0,"2020"),
            new ComboBoxItem(1,"2021"),
            new ComboBoxItem(2,"2022"),
            new ComboBoxItem(3,"2023"),
            new ComboBoxItem(4,"2024"),
        };

        public List<ComboBoxItem> CbxTimeExpeses { get; set; } = new List<ComboBoxItem> {
            new ComboBoxItem(0,"Tháng này"),
            new ComboBoxItem(1,"3 Tháng trước"),
            new ComboBoxItem(2,"6 Tháng trước"),
            new ComboBoxItem(3,"1 Năm trước"),
            new ComboBoxItem(4,"Toàn bộ"),
        };

        public List<ComboBoxItem> CbxListTypeIncome { get; set; } = new List<ComboBoxItem> {
            new ComboBoxItem(0,"Lương"),
            new ComboBoxItem(1,"Tiền thưởng"),
            new ComboBoxItem(2,"Tiền trợ cấp"),
            new ComboBoxItem(3,"Khác"),
        };

        public List<ComboBoxItem> CbxListTypeExpenses { get; set; } = new List<ComboBoxItem> {
            new ComboBoxItem(0,"Giáo dục"),
            new ComboBoxItem(1,"Nhu cầu thiết yếu"),
            new ComboBoxItem(2,"Mong muốn"),
            new ComboBoxItem(3,"Tiết kiệm"),
            new ComboBoxItem(4,"Khác"),
        };

        public List<ComboBoxItem> CbxListPayment { get; set; } = new List<ComboBoxItem> {
            new ComboBoxItem(0,"Tiền mặt"),
            new ComboBoxItem(1,"Chuyển khoản ngân hàng"),
            new ComboBoxItem(2,"Chuyển khoản qua momo"),
            new ComboBoxItem(3,"Khác"),
        };

        public List<ComboBoxItem> CbxListDebtStatus { get; set; } = new List<ComboBoxItem> {
            new ComboBoxItem(0,"Chưa hoàn tất"),
            new ComboBoxItem(1,"Đã hoàn tất"),
        };

        public List<ComboBoxItem> CbxListFinancialStatus { get; set; } = new List<ComboBoxItem> {
            new ComboBoxItem(0,"Chưa thực hiện"),
            new ComboBoxItem(1,"Đang thực hiện"),
            new ComboBoxItem(2,"Hoàn tất"),
            new ComboBoxItem(3,"Hoãn lại"),
        };
        public List<ComboBoxItem> CbxListFinancialCategory { get; set; } = new List<ComboBoxItem> {
            new ComboBoxItem(0,"Nguồn thu"),
            new ComboBoxItem(1,"Chi tiêu"),
            new ComboBoxItem(2,"Nợ"),
            new ComboBoxItem(3,"Vay mượn"),
            new ComboBoxItem(4,"Khác"),
        };

        public List<string> ListMonth { get; set; } = new List<string>
        {
            "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        };

        public List<string> ListWeek { get; set; } = new List<string>
        {
            "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"
        };

        public List<string> ListYear { get; set; } = new List<string>
        {
            "2018", "2019", "2020", "2021", "2022", "2023", "2024"
        };
    }
}