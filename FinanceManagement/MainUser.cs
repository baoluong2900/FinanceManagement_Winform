using Bunifu.UI.WinForms;
using FinanceManagement.Exentesions;
using FinanceManagement.Message;
using FinanceManagement.ModelView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FinanceManagement
{
    public partial class frmHome : Form
    {
        private bool sidebarExpand;
        private bool sideNotification;
        private User userLogin;
        private List<IncomeSource> incomeSources = new List<IncomeSource>();
        private List<Expens> expenses = new List<Expens>();
        private List<Debt> debts = new List<Debt>();
        private List<FinancialPlan> financialPlans = new List<FinancialPlan>();
        private List<Transaction> transactions = new List<Transaction>();
        private List<FinancialPlan> dsbFinancialPlans = new List<FinancialPlan>();
        private List<Transaction> dsbTransactions = new List<Transaction>();
        private ValueListView valueListView = new ValueListView();
        private dbFinanceManagementEntities dbcontext = new dbFinanceManagementEntities();

        public frmHome(User user = null)
        {
            InitializeComponent();
            userLogin = user;
            this.lblHiUserName.Text = "Hi, " + userLogin?.lastName?.Trim() + " " + userLogin?.firstName?.Trim();
            this.GetData(userLogin);
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            this.GetSettingDashboard();
        }

        private void bunifuButton26_Click(object sender, EventArgs e)
        {
            sidebarExpand = !sidebarExpand;
            pnlSiderbarConatiner.Size = ExentionWinforms.TotalScrollMaxMinSize(sidebarExpand, pnlSiderbarConatiner);
        }

        private void bunifuChartCanvas1_Load(object sender, EventArgs e)
        {
        }

        #region Dashboard

        private void btnHome_Click(object sender, EventArgs e)
        {
            picboxIdicator.Top = ((Control)sender).Top;
            pageDetail.SetPage("Dashboard");
            this.GetSettingDashboard();
        }

        private void GetSettingDashboard()
        {
            var listIncomeSources = ExentionQuerys.GetListObjectByValueId(new IncomeSource(), userLogin.userID, "userID", dbcontext);
            var listExpenses = ExentionQuerys.GetListObjectByValueId(new Expens(), userLogin.userID, "userID", dbcontext);
            var listDebts = ExentionQuerys.GetListObjectByValueId(new Debt(), userLogin.userID, "userID", dbcontext);


            var currentDate = DateTime.Now.Date;
            var additionalPredicateFinancials = new Func<FinancialPlan, bool>(x => (x.startDate.Value.Date >= currentDate || x.endDate.Value.Date >= currentDate) && x.status == 0  );
            var listFinancial = ExentionQuerys.GetListObjectSearchByValue(new FinancialPlan(), userLogin.userID, "userID", additionalPredicateFinancials, dbcontext);

            var listTransions = ExentionQuerys.GetListObjectByValueId(new Transaction(), userLogin.userID, "userID", dbcontext);
            // var listDebts = ;
            var totalIncomse = (decimal)listIncomeSources.Sum(x => x.incomeSourceAmount);
            var totalExpense = (decimal)listExpenses.Sum(x => x.expenseAmount);
            var totalDebt = (decimal)listDebts.Where(x=>x.isDebt == true).Sum(x => x.debtAmount);
            var totalLoan = (decimal)listDebts.Where(x=>x.isDebt == false).Sum(x => x.debtAmount);

            lblTotalIncomes.Text = ExentionMethods.FormatAmountVND(totalIncomse);
            lblTotalLoans.Text = ExentionMethods.FormatAmountVND(totalLoan+ totalDebt);
            lblTotalIncomes.Text = ExentionMethods.FormatAmountVND(totalIncomse);
            lblTotalExpenses.Text = ExentionMethods.FormatAmountVND(totalExpense);
            lblTotalBalances.Text = ExentionMethods.FormatAmountVND(totalIncomse + totalDebt - (totalExpense +totalLoan) );
            this.GetWeeklySumAmounts(listIncomeSources);
            this.GetPercentByAll(listExpenses);
            this.GetTransactionFilter(listTransions);
            this.GetFinancialFilter(listFinancial);
        }
        private void GetTransactionFilter(List<Transaction> transactions)
        {
            if (transactions != null && transactions.Count > 0)
            {
                dgvDashboardTransaction.Visible = true;
                transactions = (List<Transaction>)transactions.OrderByDescending(x=>x.createDate).Take(5).ToList();
                dgvDashboardTransaction.Rows.Clear();
                var index = 1;
                foreach (var item in transactions)
                {
                    dgvDashboardTransaction.Rows.Add(index, valueListView.CbxListFinancialCategory.FirstOrDefault(x => x.Value == item?.categoryID)?.Name, item.transactionName,
                             ExentionMethods.FormatDateddMMyyyyString(item.createDate));
                    index++;
                }
            }
            else
            {
                dgvDashboardTransaction.Visible = false;
            }
        }
        private void GetFinancialFilter(List<FinancialPlan> financialPlan)
        {
            if (financialPlan != null && financialPlan.Count > 0)
            {
                dgvDashboardFinancial.Visible = true ;
                financialPlan = (List<FinancialPlan>)financialPlan
                .OrderBy(obj => obj.startDate)
                .Take(5).ToList();
                dgvDashboardFinancial.Rows.Clear();
                var index = 1;
                foreach (var item in financialPlan)
                {
                    dgvDashboardFinancial.Rows.Add(index,item.financialPlanName?.Trim() ,valueListView.CbxListFinancialStatus.FirstOrDefault(x => x.Value == item?.status)?.Name);
                    index++;
                }
            }
            else
            {
                dgvDashboardFinancial.Visible = false;
            }
        }

        private void GetWeeklySumAmounts(List<IncomeSource> incomeSources)
        {
            if(incomeSources != null && incomeSources.Count > 0)
            {
                DateTime startDate = DateTime.Today.Date;
                int delta = DayOfWeek.Monday - startDate.DayOfWeek;
                if (delta == 1)
                    delta -= 7;

                startDate = startDate.AddDays(delta);

                var dailySums = new List<double>();

                for (int dayIndex = 0; dayIndex < 7; dayIndex++)
                {
                    DateTime currentDate = startDate.AddDays(dayIndex);
                    DateTime nextDate = currentDate.AddDays(1);

                    var daySum = incomeSources
                        .Where(i => i.incomeSourceDate.Value.Date >= currentDate.Date && i.incomeSourceDate.Value.Date < nextDate.Date)
                        .Sum(i => i.incomeSourceAmount);

                    dailySums.Add(double.Parse(daySum.ToString()));
                }
                dshboardLineChart.Data = dailySums;
                dshboardLineChart.TargetCanvas = canvasTotalIncome;
                canvasTotalIncome.Labels = valueListView.ListWeek.ToArray();
            }
 

        }

        private void GetPercentByAll(List<Expens> expenses)
        {
            if(expenses != null && expenses.Count > 0)
            {
                var totalExpensesByCategory = expenses
               .GroupBy(e => e.categoryID)
               .OrderBy(g => g.Key)
               .ToDictionary(g => g.Key, g => g.Sum(e => e.expenseAmount));

                var listSum = ExentionMethods.GetListPercelByValues(totalExpensesByCategory);
                canvasPieTotalExpenses.Labels = valueListView.CbxListTypeExpenses
                                                .Where(x => totalExpensesByCategory.ContainsKey(x.Value))
                                                .OrderBy(x => x.Value)
                                                .Select(x => x.Name)
                                                .ToArray();

                dshboaPieTotalExpenses.Data = listSum;

                dshboaPieTotalExpenses.TargetCanvas = canvasPieTotalExpenses;

            }
     
        }

        #endregion Dashboard

        #region Debts and Loan

        private void btnDebts_Click(object sender, EventArgs e)
        {
            picboxIdicator.Top = ((Control)sender).Top;
            pageDetail.SetPage("Debts");
            this.GetSettingDebts();
        }

        private void btnAddDebt_Click(object sender, EventArgs e)
        {
            frmDebtLoan formDebt = new frmDebtLoan(userLogin.userID);
            formDebt.ShowDialog();
            this.GetSettingDebts();
        }

        private void txtSearchDebts_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchDebts?.Text?.ToString()?.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                var additionalPredicate = new Func<Debt, bool>(x => ExentionMethods.SearchByContainsIgnoreCase(x.debtName.Trim(), searchText));

                var searchedPeople = ExentionQuerys.GetListObjectSearchByValue(new Debt(), userLogin.userID, "userID", additionalPredicate, dbcontext);
                if (searchedPeople != null && searchedPeople.Count > 0)
                {
                    GetListDebts(searchedPeople);
                }
                else
                {
                    dgvDebts.Rows.Clear();
                    dgvDebts.DataSource = null;
                }
            }
            else
            {
                GetListDebts(debts);
            }
        }

        private void GetSettingDebts()
        {
            var listDebts = ExentionQuerys.GetListObjectByValueId(new Debt(), userLogin.userID, "userID", dbcontext);
            debts = listDebts.ToList();
            dgvDebts.Rows.Clear();

            if (debts != null && debts.Count > 0)
            {
                this.GetListDebts(listDebts);
                barChartDebts.Data = FilterMonthlyAmountsByYearDebt(listDebts, DateTime.Now.Year, true);
                barChartLoan.Data = FilterMonthlyAmountsByYearDebt(listDebts, DateTime.Now.Year, false);
                barChartDebts.TargetCanvas = canvasDebts;
                barChartLoan.TargetCanvas = canvasDebts;
                canvasDebts.Labels = valueListView.ListMonth.ToArray();
            }
        }

        public List<double> FilterMonthlyAmountsByYearDebt(List<Debt> debts, int selectedYear, bool isDebt)
        {
            // Tạo một danh sách chứa tổng amount cho mỗi tháng trong năm được chọn
            var monthlyAmounts = Enumerable.Range(1, 12)
                .Select(month =>
                {
                    // Lấy ra ngày đầu tiên của tháng
                    DateTime startOfMonth = new DateTime(selectedYear, month, 1);

                    // Lấy ra ngày cuối cùng của tháng
                    DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                    // Tính tổng amount cho tháng hiện tại
                    return debts
                        .Where(item => item.debtDueDate?.Date >= startOfMonth && item.debtDueDate?.Date <= endOfMonth && item.isDebt == isDebt)
                        .Sum(source => (double)source.debtAmount);
                })
                .ToList();

            return monthlyAmounts;
        }

        private void GetListDebts(List<Debt> debts)
        {
            dgvDebts.Rows.Clear();
            var index = 1;
            foreach (var item in debts)
            {
                dgvDebts.Rows.Add(index, item.debtID, item.debtName
                    , ExentionMethods.FormatAmountVND((decimal)item.debtAmount)
                    , ExentionMethods.FormatDateddMMyyyyString(item.debtDueDate)
                    , (bool)item.isDebt ? ModelTemp.ModelTemp.LoanName : ModelTemp.ModelTemp.DebtName
                    , valueListView.CbxListPayment.FirstOrDefault(x => x.Value == item?.repaymentPlan)?.Name
                    , valueListView.CbxListDebtStatus.FirstOrDefault(x => x.Value == item?.debtStatus)?.Name
                    , item.description, ExentionMethods.FormatDateddMMyyyyString(item.createDate));
                index++;
            }
        }

        private void dgvDebts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex >= 0 && dgvDebts.Columns[e.ColumnIndex].Name == "deleteDebt" && e.RowIndex >= 0)
                {
                    DataGridViewRow selectedRow = dgvDebts.Rows[e.RowIndex];
                    var cellValue = dgvDebts.Rows[e.RowIndex];
                    DialogResult result = MessageBox.Show(MessagesValue.GetNameMessageDelete(cellValue.Cells[2].Value.ToString()), MessagesValue.MessageConfirm, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        Debt expenses = new Debt();
                        var item = ExentionQuerys.GetObjectByValueId(new Debt(), int.Parse(cellValue.Cells[1].Value.ToString()), "debtID", dbcontext);
                        ExentionQuerys.DeleteObject(item);
                        this.GetSettingDebts();
                        bunifuSnackbar1.Show(this, MessagesValue.DeleteSuccess, BunifuSnackbar.MessageTypes.Success, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                    }
                    else if (result == DialogResult.No)
                    {
                        return;
                    }
                }
                else if (e.ColumnIndex >= 0 && dgvDebts.Columns[e.ColumnIndex].Name == "editDebt" && e.RowIndex >= 0)
                {
                    var cellValue = dgvDebts.Rows[e.RowIndex];
                    frmDebtLoan frmDebtLoan = new frmDebtLoan(userLogin.userID, cellValue.Cells[1].Value.ToString());
                    frmDebtLoan.ShowDialog();
                    this.GetSettingDebts();
                }
            }
            catch
            {
                bunifuSnackbar1.Show(this, MessagesValue.MessageUpdateIsFail, BunifuSnackbar.MessageTypes.Error, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                return;
            }
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            GetListDebts(debts);
            this.txtSearchDebts.Text = string.Empty;
        }

        #endregion Debts and Loan

        #region IncomeSources

        private void btnIncomeSources_Click(object sender, EventArgs e)
        {
            picboxIdicator.Top = ((Control)sender).Top;
            pageDetail.SetPage("IncomeSources");
            this.GetSettingIncomeSources();
        }

        private void GetSettingIncomeSources()
        {
            var listIncomeSources = ExentionQuerys.GetListObjectByValueId(new IncomeSource(), userLogin.userID, "userID", dbcontext);
            incomeSources = listIncomeSources.ToList();
            dgvIncomeSources.Rows.Clear();

            if (incomeSources != null && incomeSources.Count > 0)
            {
                this.GetListIncomeSources(incomeSources);
                cbxListTime.DataSource = valueListView.CbxListTimes;
                cbxListTime.DisplayMember = "Name";
                cbxListTime.ValueMember = "Value";

                cbxListYears.DataSource = valueListView.CbxListYears;
                cbxListYears.DisplayMember = "Name";
                cbxListYears.ValueMember = "Value";

                ragSumAmount.Maximum = ExentionMethods.DecimalToInt((decimal)incomeSources.Sum(x => x.incomeSourceAmount));
                this.GetTotalAmountByDateRange(0, incomeSources);
                SelectYearInComboBox(DateTime.Now.Year);
                barChartIncome.Data = FilterMonthlyAmountsByYear(incomeSources, DateTime.Now.Year);
                barChartIncome.TargetCanvas = chartCanvasIncomes;
                chartCanvasIncomes.Labels = valueListView.ListMonth.ToArray();
            }
        }

        private void txtSearchIncome_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchIncome?.Text?.ToString()?.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                // Tìm kiếm các đối tượng có UserId bằng 1 và Name chứa chuỗi "John"
                var additionalPredicate = new Func<IncomeSource, bool>(x => ExentionMethods.SearchByContainsIgnoreCase(x.incomeSourceName.Trim(), searchText));

                var searchedPeople = ExentionQuerys.GetListObjectSearchByValue(new IncomeSource(), userLogin.userID, "userID", additionalPredicate, dbcontext);
                if (searchedPeople != null && searchedPeople.Count > 0)
                {
                    GetListIncomeSources(searchedPeople);
                }
                else
                {
                    dgvIncomeSources.Rows.Clear();
                    dgvIncomeSources.DataSource = null;
                }
            }
            else
            {
                GetListIncomeSources(incomeSources);
            }
        }

        private void GetListIncomeSources(List<IncomeSource> incomeSources)
        {
            dgvIncomeSources.Rows.Clear();
            var index = 1;
            foreach (var item in incomeSources)
            {
                dgvIncomeSources.Rows.Add(index, item.incomeSourceID, item.incomeSourceName
                    , ExentionMethods.FormatAmountVND((decimal)item.incomeSourceAmount)
                    , valueListView.CbxListTypeIncome.FirstOrDefault(x => x.Value == item?.categoryID)?.Name
                    , ExentionMethods.FormatDateddMMyyyyString(item.incomeSourceDate), item.description, ExentionMethods.FormatDateddMMyyyyString(item.createDate));
                index++;
            }
        }

        private void dgvIncomeSources_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex >= 0 && dgvIncomeSources.Columns[e.ColumnIndex].Name == "deleteIncomeSources" && e.RowIndex >= 0)
                {
                    DataGridViewRow selectedRow = dgvIncomeSources.Rows[e.RowIndex];
                    var cellValue = dgvIncomeSources.Rows[e.RowIndex];
                    DialogResult result = MessageBox.Show(MessagesValue.GetNameMessageDelete(cellValue.Cells[2].Value.ToString()), MessagesValue.MessageConfirm, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        IncomeSource incomeSource = new IncomeSource();
                        var item = ExentionQuerys.GetObjectByValueId(new IncomeSource(), int.Parse(cellValue.Cells[1].Value.ToString()), "incomeSourceID", dbcontext);
                        ExentionQuerys.DeleteObject(item);
                        this.GetSettingIncomeSources();
                        bunifuSnackbar1.Show(this, MessagesValue.DeleteSuccess, BunifuSnackbar.MessageTypes.Success, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                    }
                    else if (result == DialogResult.No)
                    {
                        return;
                    }
                }
                else if (e.ColumnIndex >= 0 && dgvIncomeSources.Columns[e.ColumnIndex].Name == "editIncomeSources" && e.RowIndex >= 0)
                {
                    var cellValue = dgvIncomeSources.Rows[e.RowIndex];
                    frmIncomeSources frmIncomeSources = new frmIncomeSources(userLogin.userID, cellValue.Cells[1].Value.ToString());
                    frmIncomeSources.ShowDialog();
                    this.GetSettingIncomeSources();
                }
            }
            catch
            {
                bunifuSnackbar1.Show(this, MessagesValue.MessageUpdateIsFail, BunifuSnackbar.MessageTypes.Error, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                return;
            }
        }

        private void dgvIncomeSources_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgvIncomeSources.Rows.Count > 0)
            {
                dgvIncomeSources.Rows[dgvIncomeSources.Rows.Count - 1].Visible = false;
            }
        }

        private void SelectYearInComboBox(int year)
        {
            string yearString = year.ToString();

            // Tìm một mục trong ComboBox có Text bằng với yearString
            var itemToSelect = cbxListYears.Items.Cast<object>()
                .FirstOrDefault(item => cbxListYears.GetItemText(item) == yearString);

            // Nếu tìm thấy mục, chọn nó
            if (itemToSelect != null)
            {
                cbxListYears.SelectedItem = itemToSelect;
            }
        }

        public void GetTotalAmountByDateRange(int value, List<IncomeSource> listIncomeSources)
        {
            if (listIncomeSources != null && listIncomeSources.Count > 0)
            {
                DateTime startDate = DateTime.Today.Date;

                switch (value)
                {
                    case 0: // Ngày
                            // Không cần thay đổi startDate
                        break;

                    case 1: // Tuần
                        int delta = DayOfWeek.Monday - startDate.DayOfWeek;
                        if (delta == 1) // Nếu ngày hiện tại là Chủ Nhật
                            delta -= 7; // Trừ 7 ngày để bắt đầu từ thứ Hai của tuần trước
                        startDate = startDate.AddDays(delta);
                        break;

                    case 2: // Tháng
                        startDate = new DateTime(startDate.Year, startDate.Month, 1);
                        break;

                    case 3: // Quý
                        int currentQuarter = (startDate.Month - 1) / 3 + 1;
                        startDate = new DateTime(startDate.Year, (currentQuarter - 1) * 3 + 1, 1);
                        break;

                    case 4: // Năm
                        startDate = new DateTime(startDate.Year, 1, 1);
                        break;

                    default:
                        throw new ArgumentException("Invalid value parameter.");
                }

                var sumAmount = (decimal)listIncomeSources
                    .Where(source => source.incomeSourceDate?.Date >= startDate)
                    .Sum(source => source.incomeSourceAmount);

                ragSumAmount.Value = ExentionMethods.DecimalToInt(sumAmount);
            }
        }

        public List<double> FilterMonthlyAmountsByYear(List<IncomeSource> incomeSources, int selectedYear)
        {
            // Tạo một danh sách chứa tổng amount cho mỗi tháng trong năm được chọn
            var monthlyAmounts = Enumerable.Range(1, 12)
                .Select(month =>
                {
                    // Lấy ra ngày đầu tiên của tháng
                    DateTime startOfMonth = new DateTime(selectedYear, month, 1);

                    // Lấy ra ngày cuối cùng của tháng
                    DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                    // Tính tổng amount cho tháng hiện tại
                    return incomeSources
                        .Where(source => source.incomeSourceDate?.Date >= startOfMonth && source.incomeSourceDate?.Date <= endOfMonth)
                        .Sum(source => (double)source.incomeSourceAmount);
                })
                .ToList();

            return monthlyAmounts;
        }

        private void btnViewAllIncome_Click(object sender, EventArgs e)
        {
            GetListIncomeSources(incomeSources);
            this.txtSearchIncome.Text = string.Empty;
        }

        private void cbxListTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxListTime.SelectedIndex != -1)
            {
                this.GetTotalAmountByDateRange(cbxListTime.SelectedIndex, incomeSources);
            }
        }

        private void cbxListYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxListYears.SelectedItem != null)
            {
                // Chuyển đổi giá trị được chọn từ ComboBox thành một chuỗi và sau đó thành một số nguyên (năm)
                if (int.TryParse(cbxListYears.SelectedItem.ToString(), out int selectedYear))
                {
                    barChartIncome.Data = FilterMonthlyAmountsByYear(incomeSources, selectedYear);
                    barChartIncome.TargetCanvas = chartCanvasIncomes;
                    chartCanvasIncomes.Labels = valueListView.ListMonth.ToArray();
                }
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            frmIncomeSources frmIncomeSources = new frmIncomeSources(userLogin.userID);
            frmIncomeSources.ShowDialog();
            this.GetSettingIncomeSources();
        }

        private void pageIncomeSources_Click(object sender, EventArgs e)
        {
        }

        #endregion IncomeSources

        #region Expenses

        private void btnExpenses_Click(object sender, EventArgs e)
        {
            picboxIdicator.Top = ((Control)sender).Top;
            pageDetail.SetPage("Expenses");
            this.GetSettingExpenses();
        }

        private void dgvExpenses_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex >= 0 && dgvExpenses.Columns[e.ColumnIndex].Name == "deleteExpenses" && e.RowIndex >= 0)
                {
                    DataGridViewRow selectedRow = dgvExpenses.Rows[e.RowIndex];
                    var cellValue = dgvExpenses.Rows[e.RowIndex];
                    DialogResult result = MessageBox.Show(MessagesValue.GetNameMessageDelete(cellValue.Cells[2].Value.ToString()), MessagesValue.MessageConfirm, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        Expens expenses = new Expens();
                        var item = ExentionQuerys.GetObjectByValueId(new Expens(), int.Parse(cellValue.Cells[1].Value.ToString()), "expenseID", dbcontext);
                        ExentionQuerys.DeleteObject(item);
                        this.GetSettingExpenses();
                        bunifuSnackbar1.Show(this, MessagesValue.DeleteSuccess, BunifuSnackbar.MessageTypes.Success, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                    }
                    else if (result == DialogResult.No)
                    {
                        return;
                    }
                }
                else if (e.ColumnIndex >= 0 && dgvExpenses.Columns[e.ColumnIndex].Name == "editExpenses" && e.RowIndex >= 0)
                {
                    var cellValue = dgvExpenses.Rows[e.RowIndex];
                    frmExpenses frmexpenses = new frmExpenses(userLogin.userID, cellValue.Cells[1].Value.ToString());
                    frmexpenses.ShowDialog();
                    this.GetSettingExpenses();
                }
            }
            catch
            {
                bunifuSnackbar1.Show(this, MessagesValue.MessageUpdateIsFail, BunifuSnackbar.MessageTypes.Error, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                return;
            }
        }

        private void btnAddExpenses_Click(object sender, EventArgs e)
        {
            frmExpenses frmExpenses = new frmExpenses(userLogin.userID);
            frmExpenses.ShowDialog();
            this.GetSettingExpenses();
        }

        private void GetSettingExpenses()
        {
            var listExpenses = ExentionQuerys.GetListObjectByValueId(new Expens(), userLogin.userID, "userID", dbcontext);
            expenses = listExpenses.ToList();
            dgvExpenses.Rows.Clear();

            if (expenses != null && expenses.Count > 0)
            {
                this.GetListExpenses(expenses);
                cbxTimeExpenses.DataSource = valueListView.CbxTimeExpeses;
                cbxTimeExpenses.DisplayMember = "Name";
                cbxTimeExpenses.ValueMember = "Value";

                var listHorChart = this.FilterExpensesByCategory(expenses, cbxTimeExpenses.SelectedIndex);
                if (listHorChart != null && listHorChart.Count > 0)
                {
                    this.ViewHorziChartExpenses(listHorChart);
                    this.ViewRadarChartExpenses(listExpenses);
                }
            }
        }

        public List<Expens> FilterExpensesByCategory(List<Expens> expenses, int category)
        {
            DateTime currentDate = DateTime.Now;

            switch (category)
            {
                case 0:
                    // Tháng này
                    return expenses.Where(e => e.expenseDate.Value.Month == currentDate.Month && e.expenseDate.Value.Year == currentDate.Year).ToList();

                case 1:
                    // 3 Tháng trước
                    DateTime threeMonthsAgo = currentDate.AddMonths(-3);
                    return expenses.Where(e => e.expenseDate.Value >= threeMonthsAgo && e.expenseDate.Value <= currentDate).ToList();

                case 2:
                    // 6 Tháng trước
                    DateTime sixMonthsAgo = currentDate.AddMonths(-6);
                    return expenses.Where(e => e.expenseDate.Value >= sixMonthsAgo && e.expenseDate.Value <= currentDate).ToList();

                case 3:
                    // 1 Năm trước
                    DateTime oneYearAgo = currentDate.AddYears(-1);
                    return expenses.Where(e => e.expenseDate.Value >= oneYearAgo && e.expenseDate.Value <= currentDate).ToList();

                case 4:
                    // Toàn bộ
                    return expenses;

                default:
                    // Mặc định trả về toàn bộ danh sách
                    return expenses;
            }
        }

        public void ViewHorziChartExpenses(List<Expens> expenses)
        {
            var totalExpensesByCategory = expenses
                 .GroupBy(e => e.categoryID)
                 .OrderBy(g => g.Key)
                 .ToDictionary(g => g.Key, g => g.Sum(e => e.expenseAmount));
            canvasExpenses.Labels = null;
            canvasExpenses.Labels = valueListView.CbxListTypeExpenses.Where(x => totalExpensesByCategory.ContainsKey(x.Value)).OrderBy(x => x.Value).Select(x => x.Name).ToArray();
            canvasRadarExpenses.Update();
            HorlBarChartExpenses.Data = null;
            HorlBarChartExpenses.Data = totalExpensesByCategory.Values.Select(x => double.Parse(x.Value.ToString())).ToList();
            HorlBarChartExpenses.TargetCanvas = null;
            HorlBarChartExpenses.TargetCanvas = canvasExpenses;
        }

        public void ViewRadarChartExpenses(List<Expens> expenses)
        {
            var totalExpensesByCategory = expenses
                 .GroupBy(e => e.categoryID)
                 .OrderBy(g => g.Key)
                 .ToDictionary(g => g.Key, g => g.Sum(e => e.expenseAmount));
            var r = new Random();
            canvasRadarExpenses.Labels = valueListView.CbxListTypeExpenses.Where(x => totalExpensesByCategory.ContainsKey(x.Value)).OrderBy(x => x.Value).Select(x => x.Name).ToArray();
            radarChartExpenses.Data = totalExpensesByCategory.Values.Select(x => double.Parse(x.Value.ToString())).ToList();
            canvasRadarExpenses.Update();
            radarChartExpenses.BackgroundColor = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256), r.Next(256));
            radarChartExpenses.BorderColor = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));

            radarChartExpenses.TargetCanvas = canvasRadarExpenses;
        }

        private void canvasExpenses_Load(object sender, EventArgs e)
        {
        }

        private void GetListExpenses(List<Expens> expenses)
        {
            dgvExpenses.Rows.Clear();
            var index = 1;
            foreach (var item in expenses)
            {
                dgvExpenses.Rows.Add(index, item.expenseID, item.expenseName,
                    ExentionMethods.FormatAmountVND((decimal)item.expenseAmount)
                     , valueListView.CbxListTypeExpenses.FirstOrDefault(x => x.Value == item?.categoryID)?.Name
                     , ExentionMethods.FormatDateddMMyyyyString(item.expenseDate), item.description
                     , ExentionMethods.FormatDateddMMyyyyString(item.createDate));
                index++;
            }
        }

        private void cbxTimeExpenses_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cbxTimeExpenses.SelectedIndex != -1)
            {
                var listHorChart = this.FilterExpensesByCategory(expenses, cbxTimeExpenses.SelectedIndex);
                this.ViewHorziChartExpenses(listHorChart);
            }
        }

        private void txtSerachExpenses_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSerachExpenses?.Text?.ToString()?.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                // Tìm kiếm các đối tượng có UserId bằng 1 và Name chứa chuỗi "John"
                var additionalPredicate = new Func<Expens, bool>(x => ExentionMethods.SearchByContainsIgnoreCase(x.expenseName.Trim(), searchText));

                var searchedPeople = ExentionQuerys.GetListObjectSearchByValue(new Expens(), userLogin.userID, "userID", additionalPredicate, dbcontext);
                if (searchedPeople != null && searchedPeople.Count > 0)
                {
                    GetListExpenses(searchedPeople);
                }
                else
                {
                    dgvExpenses.Rows.Clear();
                    dgvExpenses.DataSource = null;
                }
            }
            else
            {
                GetListExpenses(expenses);
            }
        }

        private void btnAllExpenses_Click(object sender, EventArgs e)
        {
            GetListExpenses(expenses);
            this.txtSerachExpenses.Text = string.Empty;
        }

        #endregion Expenses

        #region Financial Plan

        private void btnAddFinancialPlan_Click(object sender, EventArgs e)
        {
            frmFinancialPlan frmFinancialPlans = new frmFinancialPlan(userLogin.userID);
            frmFinancialPlans.ShowDialog();
            this.GetSettingFinancialPlan();
        }

        private void GetSettingFinancialPlan()
        {
            var listFinancialPlan = ExentionQuerys.GetListObjectByValueId(new FinancialPlan(), userLogin.userID, "userID", dbcontext);
            financialPlans = listFinancialPlan.ToList();
            dgvExpenses.Rows.Clear();

            if (financialPlans != null && financialPlans.Count > 0)
            {
                this.GetListFinancialPlan(financialPlans);
                this.GetDouChartFinanicalPlan();
                this.GetProgressFilterDate();
            }

        }
        private void GetProgressFilterDate()
        {
            // Lấy ngày hiện tại
            DateTime currentDate = DateTime.Now.Date;
            var filteredList = financialPlans
                .Where(obj => obj.startDate.Value.Date >= currentDate || obj.endDate.Value.Date >= currentDate)
                .OrderBy(obj => obj.startDate)
                .ToList();

            if (filteredList != null && filteredList.Count > 0)
            {
                for (int i = 0; i < Math.Min(filteredList.Count, 6); i++)
                {
                    BunifuLabel currentLabel = (BunifuLabel)this.Controls.Find($"lblTaskName{i + 1}", true).FirstOrDefault();
                    BunifuProgressBar currentProgress = (BunifuProgressBar)this.Controls.Find($"progress{i + 1}", true).FirstOrDefault();

                    if (currentLabel != null && currentProgress != null)
                    {
                        ExentionWinforms.UpdateLabelAndProgress(currentLabel, currentProgress, filteredList[i].financialPlanName, (decimal)(filteredList[i]?.progress));
                    }
                }

            }


        }

        private void GetDouChartFinanicalPlan()
        {
            var percenlFinancialByCategory = financialPlans
            .GroupBy(e => e.cateogryID)
            .OrderBy(g => g.Key)
            .ToDictionary(g => g.Key, g => (decimal?)g.Count() / financialPlans.Count * 100);

            var listSum = ExentionMethods.GetListPercelByValues(percenlFinancialByCategory);

            canvasFinancialPlanDoug.Labels = null;
            canvasFinancialPlanDoug.Labels = valueListView.CbxListFinancialCategory.Where(x => percenlFinancialByCategory.ContainsKey(x.Value)).OrderBy(x => x.Value).Select(x => x.Name).ToArray();
            canvasFinancialPlanDoug.Update();
            DouChartFinanicalPlan.Data =null;
            DouChartFinanicalPlan.Data = listSum;
            DouChartFinanicalPlan.TargetCanvas = null;
            DouChartFinanicalPlan.TargetCanvas = canvasFinancialPlanDoug;
        }

        private void GetListFinancialPlan(List<FinancialPlan> financialPlans)
        {
            dgvFincialPlan.Rows.Clear();
            var index = 1;
            foreach (var item in financialPlans)
            {
                dgvFincialPlan.Rows.Add(index, item.financialPlanID, item.financialPlanName,
                    valueListView.CbxListFinancialCategory.FirstOrDefault(x => x.Value == item?.cateogryID)?.Name,
                     ExentionMethods.FormatDateddMMyyyyString(item.startDate),
                     ExentionMethods.FormatDateddMMyyyyString(item.endDate),
                     item.progress.ToString()+" %",
                      valueListView.CbxListFinancialStatus.FirstOrDefault(x => x.Value == item?.status)?.Name,
                      item.description
                     , ExentionMethods.FormatDateddMMyyyyString(item.createDate));
                index++;
            }
        }

        private void btnFinancialPlan_Click(object sender, EventArgs e)
        {
            picboxIdicator.Top = ((Control)sender).Top;
            pageDetail.SetPage("FinancialPlan");
            this.GetSettingFinancialPlan();
        }

        private void dgvFincialPlan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex >= 0 && dgvFincialPlan.Columns[e.ColumnIndex].Name == "deleteFinancialPlan" && e.RowIndex >= 0)
                {
                    DataGridViewRow selectedRow = dgvFincialPlan.Rows[e.RowIndex];
                    var cellValue = dgvFincialPlan.Rows[e.RowIndex];
                    DialogResult result = MessageBox.Show(MessagesValue.GetNameMessageDelete(cellValue.Cells[2].Value.ToString()), MessagesValue.MessageConfirm, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        FinancialPlan financialPlan = new FinancialPlan();
                        var item = ExentionQuerys.GetObjectByValueId(new FinancialPlan(), int.Parse(cellValue.Cells[1].Value.ToString()), "financialPlanID", dbcontext);
                        ExentionQuerys.DeleteObject(item);
                        this.GetSettingFinancialPlan();
                        bunifuSnackbar1.Show(this, MessagesValue.DeleteSuccess, BunifuSnackbar.MessageTypes.Success, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                    }
                    else if (result == DialogResult.No)
                    {
                        return;
                    }
                }
                else if (e.ColumnIndex >= 0 && dgvFincialPlan.Columns[e.ColumnIndex].Name == "editFinancialPlan" && e.RowIndex >= 0)
                {
                    var cellValue = dgvFincialPlan.Rows[e.RowIndex];
                    frmFinancialPlan frmFinancialPlans = new frmFinancialPlan(userLogin.userID, cellValue.Cells[1].Value.ToString());
                    frmFinancialPlans.ShowDialog();
                    this.GetSettingFinancialPlan();
                }
            }
            catch
            {
                bunifuSnackbar1.Show(this, MessagesValue.MessageUpdateIsFail, BunifuSnackbar.MessageTypes.Error, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                return;
            }
        }

        private void txtSearchFinancialPlan_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchFinancialPlan?.Text?.ToString()?.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                // Tìm kiếm các đối tượng có UserId bằng 1 và Name chứa chuỗi "John"
                var additionalPredicate = new Func<FinancialPlan, bool>(x => ExentionMethods.SearchByContainsIgnoreCase(x.financialPlanName.Trim(), searchText));

                var searchedPeople = ExentionQuerys.GetListObjectSearchByValue(new FinancialPlan(), userLogin.userID, "userID", additionalPredicate, dbcontext);
                if (searchedPeople != null && searchedPeople.Count > 0)
                {
                    GetListFinancialPlan(searchedPeople);
                }
                else
                {
                    dgvExpenses.Rows.Clear();
                    dgvExpenses.DataSource = null;
                }
            }
            else
            {
                GetListFinancialPlan(financialPlans);
            }
        }

        private void btnALlFinancialPlan_Click(object sender, EventArgs e)
        {
            GetListFinancialPlan(financialPlans);
        }

        #endregion Financial Plan

        #region Transcations

        private void btnTransactions_Click(object sender, EventArgs e)
        {
            picboxIdicator.Top = ((Control)sender).Top;
            pageDetail.SetPage("Transactions");
            this.GetSettingTransaction();
        }

        private void GetSettingTransaction()
        {
            var listTransactions = ExentionQuerys.GetListObjectByValueId(new Transaction(), userLogin.userID, "userID", dbcontext);
            transactions = listTransactions.ToList();
            dgvTransaction.Rows.Clear();

            if (transactions != null && transactions.Count > 0)
            {
                this.GetLitsTransaction(transactions);
            }

        }
        private void txtSearchTransaction_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchTransaction?.Text?.ToString()?.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                // Tìm kiếm các đối tượng có UserId bằng 1 và Name chứa chuỗi "John"
                var additionalPredicate = new Func<Transaction, bool>(x => ExentionMethods.SearchByContainsIgnoreCase(x.transactionName.Trim(), searchText));

                var searchedPeople = ExentionQuerys.GetListObjectSearchByValue(new Transaction(), userLogin.userID, "userID", additionalPredicate, dbcontext);
                if (searchedPeople != null && searchedPeople.Count > 0)
                {
                    GetLitsTransaction(searchedPeople);
                }
                else
                {
                    dgvExpenses.Rows.Clear();
                    dgvExpenses.DataSource = null;
                }
            }
            else
            {
                GetLitsTransaction(transactions);
            }
        }
        private void GetLitsTransaction(List<Transaction> transactions)
        {
            dgvTransaction.Rows.Clear();
            var index = 1;
            foreach (var item in transactions)
            {
                dgvTransaction.Rows.Add(index, item.transactionID,item.transactionName,
                         ExentionMethods.FormatDateddMMyyyyString(item.createDate),
                    valueListView.CbxListFinancialCategory.FirstOrDefault(x => x.Value == item?.categoryID)?.Name,
                     item.description);
                index++;
            }
        }
        private void bunifuButton21_Click(object sender, EventArgs e)
        {
            GetLitsTransaction(transactions);
        }

        private void btnDeleteTransaction_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(MessagesValue.MessageDeleteAllTransaction, MessagesValue.MessageConfirm, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                //FinancialPlan financialPlan = new FinancialPlan();
                //var item = ExentionQuerys.GetObjectByValueId(new FinancialPlan(), int.Parse(cellValue.Cells[1].Value.ToString()), "financialPlanID", dbcontext);
                //   ExentionQuerys.DeleteObject(item);
                ExentionQuerys.DeleteObjects(transactions);
                this.GetSettingTransaction();
                bunifuSnackbar1.Show(this, MessagesValue.DeleteSuccess, BunifuSnackbar.MessageTypes.Success, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
            }
            else if (result == DialogResult.No)
            {
                return;
            }
        }

        #endregion Transcations

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (frmLogin frmLogins = new frmLogin())
            {
                frmLogins.ShowDialog();

            }

        }

        private void pnlSiderbarConatiner_Paint(object sender, PaintEventArgs e)
        {
        }

        private void bunifuPictureBox2_Click(object sender, EventArgs e)
        {
        }

        private void bunifuButton25_Click(object sender, EventArgs e)
        {
            frmInfomartionUser formInfomartionUser = new frmInfomartionUser(userLogin.userID);
            formInfomartionUser.ShowDialog();
            userLogin = formInfomartionUser.userLogin;
            this.GetData(userLogin);
            // this.Hide();
        }

        private void GetData(User user)
        {
            this.lblFullName.Text = ExentionMethods.GetFullName(userLogin?.lastName, userLogin?.firstName);
            this.dtpDateTimeNow.Value = DateTime.Now;
            this.lblHiUserName.Text = "Hi, " + this.lblFullName.Text;
            //this.txtAddress.Text = userLogin.address;
            //this.txtPhone.Text = userLogin.phone;
            //  this.dtpBirthday.Value = userLogin.birthday.Value;
            //radioFemale.Checked = ExentionsMethod.CheckSexUser(userLogin.sex, HardValue.Female);
            //radioMale.Checked = ExentionsMethod.CheckSexUser(userLogin.sex, HardValue.Male);
            //radioOther.Checked = ExentionsMethod.CheckSexUser(userLogin.sex, HardValue.Other);
            if (!string.IsNullOrEmpty(userLogin?.avatar))
            {
                picboxAvatar.Image = ExentionWinforms.GetAvatarFormLocal(userLogin?.avatar);
            }
        }

        private void pageDashboard_Click(object sender, EventArgs e)
        {
        }

        private void bunifuPanel1_Click(object sender, EventArgs e)
        {
        }

        private void btnShowNotificaiton_Click(object sender, EventArgs e)
        {
            //  sideNotification = !sideNotification;
            //  this.pnlNotifications.Visible = sideNotification;
        }

        private void bunifuLabel14_Click(object sender, EventArgs e)
        {
        }

        private void bunifuShadowPanel1_ControlAdded(object sender, ControlEventArgs e)
        {
        }

        private void bunifuLabel13_Click(object sender, EventArgs e)
        {
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void lblFullName_Click(object sender, EventArgs e)
        {
        }

        private void pageTasks_Click(object sender, EventArgs e)
        {
        }

        private void lblTotalLoans_Click(object sender, EventArgs e)
        {
        }

        private void bunifuShadowPanel11_ControlAdded(object sender, ControlEventArgs e)
        {

        }

        private void bunifuShadowPanel2_ControlAdded(object sender, ControlEventArgs e)
        {

        }

        private void pageDebts_Click(object sender, EventArgs e)
        {

        }
    }
}