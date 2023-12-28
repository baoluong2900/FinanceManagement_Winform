using Bunifu.UI.WinForms;
using FinanceManagement.Enums;
using FinanceManagement.Exentesions;
using FinanceManagement.Message;
using FinanceManagement.ModelView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static FinanceManagement.ModelTemp.ModelTemp;

namespace FinanceManagement
{
    public partial class MainAdmin : Form
    {
        private bool sidebarExpand;
        private string recID;
        private int indexSelect;
        private dbFinanceManagementEntities dbcontext = new dbFinanceManagementEntities();
        private ValueListView valueListView = new ValueListView();

        public MainAdmin()
        {
            InitializeComponent();
        }

        private void MainAdmin_Load(object sender, EventArgs e)
        {
            this.GetSettingDashboard();
        }

        private void bunifuLabel1_Click(object sender, EventArgs e)
        {
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
        }

        private void bunifuPictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void pageLocation_Click(object sender, EventArgs e)
        {
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            picboxIdicator.Top = ((Control)sender).Top;
            pageDetail.SetPage("Home");
            this.GetSettingDashboard();
        }
        private void GetSettingDashboard()
        {
           
            var listIncomeSources = ExentionQuerys.GetListObjectByValueId(new IncomeSource(), -1,string.Empty, dbcontext);
            var listExpenses = ExentionQuerys.GetListObjectByValueId(new Expens(),-1,string.Empty, dbcontext);
            var listDebts = ExentionQuerys.GetListObjectByValueId(new Debt(), -1, string.Empty, dbcontext);
            var listUsers = ExentionQuerys.GetListObjectByValueId(new User(), -1, string.Empty, dbcontext);
            lblCountDetbs.Text = listDebts.Count.ToString();
            lblCountExpenses.Text = listExpenses.Count.ToString();
            lblCountIncomes.Text = listIncomeSources.Count.ToString();
            lblCountUsers.Text = listUsers.Count.ToString();

            canvasDashboard.Labels = valueListView.ListMonth.ToArray();
            if (listIncomeSources != null && listIncomeSources.Count > 0)
            {
                barchartIncome.Data = ExentionQuerys.FilterMonthlyAmountsByYear(listIncomeSources, d => d.createDate, DateTime.Now.Year);
                barchartIncome.TargetCanvas = canvasDashboard;
            }
            if (listExpenses != null && listExpenses.Count > 0)
            {
                barchartExpenses.Data = ExentionQuerys.FilterMonthlyAmountsByYear(listExpenses, d => d.createDate, DateTime.Now.Year);
                barchartExpenses.TargetCanvas = canvasDashboard;
            }
            if (listDebts != null && listDebts.Count > 0)
            {
                barchartDebts.Data = ExentionQuerys.FilterMonthlyAmountsByYear(listDebts, d => d.createDate, DateTime.Now.Year);
                barchartDebts.TargetCanvas = canvasDashboard;
            }

        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            picboxIdicator.Top = ((Control)sender).Top;
            pageDetail.SetPage("Users");
            this.GetSettingUsers();
        }
        private void GetSettingUsers()
        {
            var listUsers = ExentionQuerys.GetListObjectByValueId(new User(), -1, string.Empty, dbcontext);
            if (listUsers != null && listUsers.Count > 0)
            {
                this.GetListUsers(listUsers);
            }
        }
        private void GetListUsers(List<User> users)
        {
            dgvAdminUsers.Rows.Clear();
            var index = 1;
            foreach (var item in users)
            {
                dgvAdminUsers.Rows.Add(index, item.userID, item.userName?.Trim(), ExentionMethods.GetFullName(item?.lastName, item?.firstName),
                    ExentionMethods.GetSexUser(item.sex),
                    ExentionMethods.FormatDateddMMyyyyString(item.birthday),
                    item.address,
                    item.phone,
                    ExentionMethods.FormatDateddMMyyyyString(item.lastLogin),
                    ExentionMethods.GetStatus((bool)item.active));
                index++;
            }
        }
        private void dgvAdminUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var cellValue = dgvAdminUsers.Rows[e.RowIndex];
                this.recID = cellValue.Cells[8].Value.ToString();
                this.txtAdminUserName.Text = cellValue.Cells[2].Value.ToString();
            }
        }

        private void btnTransactions_Click(object sender, EventArgs e)
        {
            picboxIdicator.Top = ((Control)sender).Top;
            pageDetail.SetPage("Transactions");
            this.GetSettingTransaction();
        }
        private void GetSettingTransaction()
        {
            var listTransactions = ExentionQuerys.GetListObjectByValueId(new Transaction(), -1, string.Empty, dbcontext);
            if (listTransactions != null && listTransactions.Count > 0)
            {
                this.GetLitsTransaction(listTransactions);
            }

        }
        private void GetLitsTransaction(List<Transaction> transactions)
        {
            dgvAdminTransaction.Rows.Clear();
            var index = 1;
            foreach (var item in transactions)
            {
                dgvAdminTransaction.Rows.Add(index, item.transactionID, item.transactionName,
                         ExentionMethods.FormatDateddMMyyyyString(item.createDate),
                    valueListView.CbxListFinancialCategory.FirstOrDefault(x => x.Value == item?.categoryID)?.Name,
                     item.description);
                index++;
            }
        }

        private void btnIncomeSoures_Click(object sender, EventArgs e)
        {
            picboxIdicator.Top = ((Control)sender).Top;
            pageDetail.SetPage("IncomeSoures");
            this.GetSettingIncomeSources();
        }
        private void GetSettingIncomeSources()
        {
            var listIncomeSources = ExentionQuerys.GetListObjectByValueId(new IncomeSource(), -1, string.Empty, dbcontext);

            if (listIncomeSources != null && listIncomeSources.Count > 0)
            {
                this.GetListIncomeSources(listIncomeSources);
            }
        }
        private void GetListIncomeSources(List<IncomeSource> incomeSources)
        {
            dgvAdminIncomeSources.Rows.Clear();
            var index = 1;
            foreach (var item in incomeSources)
            {
                dgvAdminIncomeSources.Rows.Add(index, item.incomeSourceID, item.incomeSourceName
                    , ExentionMethods.FormatAmountVND((decimal)item.incomeSourceAmount)
                    , valueListView.CbxListTypeIncome.FirstOrDefault(x => x.Value == item?.categoryID)?.Name
                    , ExentionMethods.FormatDateddMMyyyyString(item.incomeSourceDate), item.description, ExentionMethods.FormatDateddMMyyyyString(item.createDate));
                index++;
            }
        }

        private void btnExpenses_Click(object sender, EventArgs e)
        {
            picboxIdicator.Top = ((Control)sender).Top;
            pageDetail.SetPage("Expenses");
            this.GetSettingExpenses();
        }
        private void GetSettingExpenses()
        {
            var listExpenses = ExentionQuerys.GetListObjectByValueId(new Expens(), -1, string.Empty, dbcontext);

            if (listExpenses != null && listExpenses.Count > 0)
            {
                this.GetListExpenses(listExpenses);
            }
        }
        private void GetListExpenses(List<Expens> expenses)
        {
            dgvAdminExpenses.Rows.Clear();
            var index = 1;
            foreach (var item in expenses)
            {
                dgvAdminExpenses.Rows.Add(index, item.expenseID, item.expenseName,
                    ExentionMethods.FormatAmountVND((decimal)item.expenseAmount)
                     , valueListView.CbxListTypeExpenses.FirstOrDefault(x => x.Value == item?.categoryID)?.Name
                     , ExentionMethods.FormatDateddMMyyyyString(item.expenseDate), item.description
                     , ExentionMethods.FormatDateddMMyyyyString(item.createDate));
                index++;
            }
        }
        private void btnDebt_Click(object sender, EventArgs e)
        {
            picboxIdicator.Top = ((Control)sender).Top;
            pageDetail.SetPage("Debts");
            this.GetAllDebts();
        }
        private void GetAllDebts()
        {
            var listDebts = ExentionQuerys.GetListObjectByValueId(new Debt(), -1, string.Empty, dbcontext);
            if (listDebts != null && listDebts.Count > 0)
            {
                this.GetListDebts(listDebts);
            }
        }
        private void GetListDebts(List<Debt> debts)
        {
            dgvAdminDebts.Rows.Clear();
            var index = 1;
            foreach (var item in debts)
            {
                dgvAdminDebts.Rows.Add(index, item.debtID, item.debtName
                    , ExentionMethods.FormatAmountVND((decimal)item.debtAmount)
                    , ExentionMethods.FormatDateddMMyyyyString(item.debtDueDate)
                    , (bool)item.isDebt ? ModelTemp.ModelTemp.LoanName : ModelTemp.ModelTemp.DebtName
                    , valueListView.CbxListPayment.FirstOrDefault(x => x.Value == item?.repaymentPlan)?.Name
                    , valueListView.CbxListDebtStatus.FirstOrDefault(x => x.Value == item?.debtStatus)?.Name
                    , item.description, ExentionMethods.FormatDateddMMyyyyString(item.createDate));
                index++;
            }
        }

        private void btnFinancalPlan_Click(object sender, EventArgs e)
        {
            picboxIdicator.Top = ((Control)sender).Top;
            pageDetail.SetPage("FinancalPlans");
            this.GetSettingFinancialPlan();
        }
        private void GetSettingFinancialPlan()
        {
            var listFinancialPlan = ExentionQuerys.GetListObjectByValueId(new FinancialPlan(), -1, string.Empty, dbcontext);

            if (listFinancialPlan != null && listFinancialPlan.Count > 0)
            {
                this.GetListFinancialPlan(listFinancialPlan);
            }

        }
        private void GetListFinancialPlan(List<FinancialPlan> financialPlans)
        {
            dgvAdminFincialPlan.Rows.Clear();
            var index = 1;
            foreach (var item in financialPlans)
            {
                dgvAdminFincialPlan.Rows.Add(index, item.financialPlanID, item.financialPlanName,
                    valueListView.CbxListFinancialCategory.FirstOrDefault(x => x.Value == item?.cateogryID)?.Name,
                     ExentionMethods.FormatDateddMMyyyyString(item.startDate),
                     ExentionMethods.FormatDateddMMyyyyString(item.endDate),
                     item.progress.ToString() + " %",
                      valueListView.CbxListFinancialStatus.FirstOrDefault(x => x.Value == item?.status)?.Name,
                      item.description
                     , ExentionMethods.FormatDateddMMyyyyString(item.createDate));
                index++;
            }
        }



        private void bunifuDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            frmLogin formLogin = new frmLogin();
            formLogin.Show();
            this.Hide();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
        }

        private void bunifuIconButton1_Click(object sender, EventArgs e)
        {
        }

        private void bunifuButton21_Click(object sender, EventArgs e)
        {
        }

        private void bunifuButton22_Click(object sender, EventArgs e)
        {
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
        }

        private void bunifuLabel2_Click(object sender, EventArgs e)
        {
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
        }

        private void bunifuLabel8_Click(object sender, EventArgs e)
        {
        }

        #region More function locations

        private void btnLocations_Click(object sender, EventArgs e)
        {
            dgvLocations.Rows.Clear();
            this.ResetValue();
            picboxIdicator.Top = ((Control)sender).Top;
            pageDetail.SetPage("Locations");

            var locations = dbcontext.Locations.ToList();
            var index = 1;
            var listParent = locations.Where(x => x.levels == 1 || x.levels == 2).Distinct().Select(x => x.parent).ToList();
            var listName = locations.Where(x => listParent.Contains(x.locationNo)).ToList();
            foreach (var item in locations)
            {
                this.AddRowLocations(item, listName, index, EnumActions.QueryEnum.Insert);
                index++;
            }
        }

        private void btnInsertLocation_Click(object sender, EventArgs e)
        {
            this.InsertOrUpdateLocations(EnumActions.QueryEnum.Insert);
        }

        private void btnUpdateLocation_Click(object sender, EventArgs e)
        {
            this.InsertOrUpdateLocations(EnumActions.QueryEnum.Update, this.recID);
        }

        private void btnDeleteLocation_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.recID))
                {
                    bunifuSnackbar1.Show(this, MessagesValue.SelectItemDelete, BunifuSnackbar.MessageTypes.Warning, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                    return;
                }
                var item = this.dbcontext.Locations.FirstOrDefault(x => x.locationNo == this.recID);
                if (item != null)
                {
                    DialogResult result = MessageBox.Show(MessagesValue.MessageShowDelete, MessagesValue.MessageConfirm, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        ExentionQuerys.DeleteObject(item);
                        this.AddRowLocations(null, null, 0, EnumActions.QueryEnum.Delete);
                        bunifuSnackbar1.Show(this, MessagesValue.DeleteSuccess, BunifuSnackbar.MessageTypes.Success, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                    }
                    else if (result == DialogResult.No)
                    {
                        return;
                    }
                }
            }
            catch
            {
                bunifuSnackbar1.Show(this, MessagesValue.MessageUpdateIsFail, BunifuSnackbar.MessageTypes.Error, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                return;
            }
        }

        private void btnResetLocation_Click(object sender, EventArgs e)
        {
            this.ResetValue();
        }

        private void btnSearchLocation_Click(object sender, EventArgs e)
        {
            this.SearchRowLocations(txtSearchLocation?.Text, null, null);
        }

        private void btnAllLocation_Click(object sender, EventArgs e)
        {
            this.SearchRowLocations(string.Empty, null, null);
        }

        private void btnModifiedDateLocation_Click(object sender, EventArgs e)
        {
            this.SearchRowLocations(string.Empty, HardValue.DateTimeModifiedOn, string.Empty);
        }

        private void btnLevelLocation_Click(object sender, EventArgs e)
        {
            this.SearchRowLocations(string.Empty, string.Empty, HardValue.Level);
        }

        private void drdLevelLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drdLevelLocations.SelectedIndex != -1)
            {
                dpdParentLocations.Enabled = drdLevelLocations.SelectedIndex != 0;

                this.GetLocationByLevel(drdLevelLocations.SelectedIndex, string.Empty);
            }
        }

        private void dpdParentLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void InsertOrUpdateLocations(EnumActions.QueryEnum action, string recId = null)
        {
            try
            {
                // check empty
                var nameLocationCheck = ExentionMethods.CheckNullValueText(txtNameLocation?.Text, lblNameLocation?.Text);
                var levelLocationCheck = ExentionMethods.CheckNullValueCbx(drdLevelLocations.SelectedIndex, lblLevelLocation?.Text);
                var parenlLocationCheck = dpdParentLocations.Enabled && dpdParentLocations.DataSource != null ? ExentionMethods.CheckNullValueCbx(dpdParentLocations.SelectedIndex, lblParentLocation?.Text) : null;
                ValueView result = nameLocationCheck ?? levelLocationCheck ?? parenlLocationCheck;
                if (result != null)
                {
                    bunifuSnackbar1.Show(this, result.Text + result.Message, BunifuSnackbar.MessageTypes.Warning, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                    return;
                }

                // check locations exist
                var locationExist = dbcontext.Locations.FirstOrDefault(x =>
                x.name.Trim().ToLower() == txtNameLocation.Text.Trim().ToLower()
                );
                var locationUpdate = new Location();
                if (action == EnumActions.QueryEnum.Insert)
                {
                    if (locationExist != null)
                    {
                        bunifuSnackbar1.Show(this, MessagesValue.LocationIsExist, BunifuSnackbar.MessageTypes.Warning, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                        return;
                    }
                    UpdateLocationObject(locationUpdate, txtNameLocation.Text, txtSluqLocations.Text, txtNameWithTypeLocation.Text, txtDescriptionLocations.Text, drdLevelLocations.SelectedIndex, dpdParentLocations?.SelectedValue?.ToString(),
                        EnumActions.QueryEnum.Insert
                        );
                    locationUpdate.createDate = DateTime.Now;
                }
                else
                {
                    if (string.IsNullOrEmpty(recId))
                    {
                        bunifuSnackbar1.Show(this, MessagesValue.SelectItemUpdate, BunifuSnackbar.MessageTypes.Warning, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                        return;
                    }
                    locationUpdate = dbcontext.Locations.FirstOrDefault(x => x.locationNo == recId);
                    if (locationExist != null && locationUpdate.locationNo != locationExist.locationNo)
                    {
                        bunifuSnackbar1.Show(this, MessagesValue.LocationIsExist, BunifuSnackbar.MessageTypes.Warning, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                        return;
                    }
                    if (locationUpdate != null)
                    {
                        UpdateLocationObject(locationUpdate, txtNameLocation.Text, txtSluqLocations.Text, txtNameWithTypeLocation.Text, txtDescriptionLocations.Text, drdLevelLocations.SelectedIndex, dpdParentLocations?.SelectedValue?.ToString(),
                                 EnumActions.QueryEnum.Update
                            );

                        locationUpdate.modifiedDate = DateTime.Now;
                    }
                }
                var messageValue = action == EnumActions.QueryEnum.Insert ? MessagesValue.InsertSuccess : MessagesValue.UpdateSuccess;
                ExentionQuerys.InsertUpdateObject(locationUpdate);
                bunifuSnackbar1.Show(this, messageValue, BunifuSnackbar.MessageTypes.Success, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                this.ResetValue();
                var listName = dbcontext.Locations.Where(x => x.locationNo.Trim() == locationUpdate.parent.Trim()).ToList();
                this.AddRowLocations(locationUpdate, listName, dgvLocations.RowCount+1, action);
            }
            catch
            {
                bunifuSnackbar1.Show(this, MessagesValue.MessageUpdateIsFail, BunifuSnackbar.MessageTypes.Error, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                return;
            }
        }

        private void UpdateLocationObject(Location location, string name, string slug, string nameWithType, string description, int levels, string parent, EnumActions.QueryEnum queryEnum)
        {
            location.locationNo = queryEnum == EnumActions.QueryEnum.Insert ? Guid.NewGuid().ToString() : location.locationNo;
            location.name = name.Trim();
            location.slug = slug.Trim();
            location.nameWithType = nameWithType.Trim();
            location.description = description.Trim();
            location.levels = levels;
            location.parent = levels == 0 ? string.Empty : parent;
        }

        private void ResetValue()
        {
            drdLevelLocations.SelectedIndex = -1;
            dpdParentLocations.Enabled = false;
            txtNameLocation.Text = string.Empty;
            txtDescriptionLocations.Text = string.Empty;
            txtNameWithTypeLocation.Text = string.Empty;
            txtSluqLocations.Text = string.Empty;

            drdLevelLocations.SelectedItem = null;
            drdLevelLocations.SelectedIndex = -1;
            drdLevelLocations.ResetText();

            dpdParentLocations.SelectedItem = null;
            dpdParentLocations.SelectedIndex = -1;
            dpdParentLocations.ResetText();
            this.recID = string.Empty;
            this.indexSelect = -1;
            //   dgvLocations.Rows.Clear();
        }

        private void AddRowLocations(Location location, List<Location> listName, int index, EnumActions.QueryEnum queryEnum)
        {
            if (queryEnum == EnumActions.QueryEnum.Insert)
            {
                dgvLocations.Rows.Add(false, index, location.name.Trim(), ExentionMethods.GetNameParentListLocations(listName, location.parent),
                      ExentionMethods.GetNameLevelLocation((int)location.levels)
                    , location.nameWithType, location.slug, location.description, location.locationNo, location.levels, location.parent);
            }
            else if (queryEnum == EnumActions.QueryEnum.Update)
            {
                dgvLocations.Rows.Clear();
                var locations = dbcontext.Locations.ToList();
                var indexRow = 0;
                var listParent = locations.Where(x => x.levels == 1 || x.levels == 2).Distinct().Select(x => x.parent).ToList();
                var listNames = locations.Where(x => listParent.Contains(x.locationNo)).ToList();
                foreach (var item in locations)
                {
                    this.AddRowLocations(item, listNames, indexRow, EnumActions.QueryEnum.Insert);
                    indexRow++;
                }
            }
            else if (queryEnum == EnumActions.QueryEnum.Delete)
            {
                if (this.indexSelect >= 0 && this.indexSelect < dgvLocations.Rows.Count)
                {
                    // Xóa dòng từ DataGridView
                    dgvLocations.Rows.RemoveAt(this.indexSelect);

                    // Cập nhật lại số thứ tự cho danh sách trong DataGridView
                    for (int i = 0; i < dgvLocations.Rows.Count; i++)
                    {
                        dgvLocations.Rows[i].Cells["No"].Value = i + 1;
                    }
                }
            }
        }

        private void SearchRowLocations(string textSearch, string dateTimeModifiedOn, string level)
        {
            dgvLocations.Rows.Clear();
            var locations = dbcontext.Locations.ToList();
            if (!string.IsNullOrEmpty(textSearch))
            {
                locations = locations.Where(x => x.name.Trim().IndexOf(textSearch.Trim(), StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }
            else if (!string.IsNullOrEmpty(dateTimeModifiedOn))
            {
                locations = locations.OrderByDescending(x => x.modifiedDate).ToList();
            }
            else if (!string.IsNullOrEmpty(level))
            {
                locations = locations.OrderBy(x =>
                {
                    if (x.levels == 0)
                        return 0;
                    if (x.levels == 1)
                        return 1;
                    if (x.levels == 2)
                        return 2;
                    return 3;
                }).ToList();
            }
            var indexRow = 0;
            var listParent = locations.Where(x => x.levels == 1 || x.levels == 2).Distinct().Select(x => x.parent).ToList();
            var listNames = locations.Where(x => listParent.Contains(x.locationNo)).ToList();
            foreach (var item in locations)
            {
                this.AddRowLocations(item, listNames, indexRow, EnumActions.QueryEnum.Insert);
                indexRow++;
            }
        }

        private void GetLocationByLevel(int level, string paretNo)
        {
            dpdParentLocations.DisplayMember = "Name"; // Chọn thuộc tính cần hiển thị
            dpdParentLocations.ValueMember = "Value"; // Chọn thuộc tính cần lấy giá trị
            dpdParentLocations.SelectedItem = null;
            dpdParentLocations.SelectedIndex = -1;
            dpdParentLocations.ResetText();
            if (level != 0)
            {
                var levelParent = level == 1 ? 0 : 1;
                var locations = dbcontext.Locations.Where(x => x.levels == levelParent)
                    .Select(x => new
                    {
                        Value = x.locationNo,
                        Name = x.name,
                    })
                    .ToList();
                dpdParentLocations.DataSource = locations;
                if (!string.IsNullOrEmpty(paretNo))
                {
                    dpdParentLocations.SelectedItem = locations.FirstOrDefault(x => x.Value == paretNo);
                }
            }
        }

        private void SetSettingLocation(string name, string nameWithType, string slug, string description, string levelNo, string parentNo)
        {
            txtNameLocation.Text = name.Trim();
            txtDescriptionLocations.Text = description.Trim();
            txtNameWithTypeLocation.Text = nameWithType.Trim();
            txtSluqLocations.Text = slug.Trim();
            drdLevelLocations.SelectedIndex = int.Parse(levelNo);
            dpdParentLocations.Enabled = drdLevelLocations.SelectedIndex != 0;
            this.GetLocationByLevel(drdLevelLocations.SelectedIndex, parentNo);
        }

        private void dgvLocations_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var cellValue = dgvLocations.Rows[e.RowIndex];
                this.SetSettingLocation(
                    cellValue.Cells[2].Value.ToString(),
                     cellValue.Cells[5].Value.ToString(),
                         cellValue.Cells[6].Value.ToString(),
                     cellValue.Cells[7].Value.ToString(),
                        cellValue.Cells[9].Value.ToString(),
                       cellValue.Cells[10].Value.ToString()
                    );
                this.recID = cellValue.Cells[8].Value.ToString();
                this.indexSelect = e.RowIndex;
            }
        }

        #endregion More function locations

        private void pageUsers_Click(object sender, EventArgs e)
        {
        }

        private void guna2GroupBox1_Click(object sender, EventArgs e)
        {
        }

        private void bunifuLabel13_Click(object sender, EventArgs e)
        {
        }

        private void pageHome_Click(object sender, EventArgs e)
        {
        }

        private void pageDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void bunifuTextBox4_TextChanged(object sender, EventArgs e)
        {
        }

        private void bunifuLabel12_Click(object sender, EventArgs e)
        {
        }

        private void bunifuTextBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void bunifuLabel3_Click(object sender, EventArgs e)
        {
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
        }

        private void bunifuButton21_Click_1(object sender, EventArgs e)
        {
        }

        private void bunifuButton22_Click_1(object sender, EventArgs e)
        {
        }

        private void bunifuButton23_Click(object sender, EventArgs e)
        {
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void bunifuIconButton1_Click_1(object sender, EventArgs e)
        {
        }

        private void bunifuTextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void bunifuLabel2_Click_1(object sender, EventArgs e)
        {
        }

        private void bunifuTextBox3_TextChanged(object sender, EventArgs e)
        {
        }

        private void bunifuLabel4_Click(object sender, EventArgs e)
        {
        }

        private void bunifuDropdown2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void bunifuLabel11_Click(object sender, EventArgs e)
        {
        }

        private void bunifuLabel7_Click(object sender, EventArgs e)
        {
        }

        private void bunifuDropdown1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void bunifuDropdown3_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void tabPageDebts_Click(object sender, EventArgs e)
        {

        }

        private void lblTotalExpenses_Click(object sender, EventArgs e)
        {

        }
    }
}