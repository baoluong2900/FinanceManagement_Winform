using Bunifu.UI.WinForms;
using FinanceManagement.Enums;
using FinanceManagement.Exentesions;
using FinanceManagement.Message;
using FinanceManagement.ModelView;
using System;
using System.Windows.Forms;
using static FinanceManagement.ModelTemp.ModelTemp;

namespace FinanceManagement
{
    public partial class frmIncomeSources : Form
    {
        public IncomeSource incomeSource = null;
        public int userID;
        public EnumActions.QueryEnum action = EnumActions.QueryEnum.Insert;
        private ValueListView valueListView = new ValueListView();
        private dbFinanceManagementEntities dbcontext = new dbFinanceManagementEntities();

        public frmIncomeSources(int _userID = -1, string incomeSourceID = null)
        {
            InitializeComponent();
            userID = _userID;
            var typeSelect = 0;
            if (!string.IsNullOrEmpty(incomeSourceID))
            {
                incomeSource = ExentionQuerys.GetObjectByValueId(new IncomeSource(), int.Parse(incomeSourceID), "incomeSourceID", dbcontext);
                action = EnumActions.QueryEnum.Update;
                typeSelect = int.Parse(incomeSource?.categoryID.ToString());
            }
            else
            {
                incomeSource = new IncomeSource();
                incomeSource.categoryID = 0;
            }
            cbxTypeIncomeSource.DataSource = valueListView.CbxListTypeIncome;
            cbxTypeIncomeSource.DisplayMember = "Name";
            cbxTypeIncomeSource.ValueMember = "Value";
            cbxTypeIncomeSource.SelectedIndex = typeSelect;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var sourceNameCheck = ExentionMethods.CheckNullValueText(this.txtSourceName.Text?.Trim(), lblSourceName?.Text);
                var amountCheck = ExentionMethods.CheckNullValueText(this.txtAmount.Text, lblAmount?.Text);
                ValueView result = sourceNameCheck ?? amountCheck;
                if (result != null)
                {
                    bunifuSnackbar1.Show(this, result.Text + result.Message, BunifuSnackbar.MessageTypes.Warning, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                    return;
                }
                incomeSource.userID = userID;
                incomeSource.description = this.txtDescription.Text?.Trim();
                incomeSource.incomeSourceDate = this.dtpStartDate.Value;
                incomeSource.incomeSourceAmount = decimal.TryParse(this.txtAmount.Text, out var amount) ? (decimal?)amount : null;
                incomeSource.incomeSourceName = this.txtSourceName.Text?.Trim();
                if (action == EnumActions.QueryEnum.Insert)
                {
                    incomeSource.createDate = DateTime.Now;
                    ExentionMethods.CreateTransaction(userID, 0, incomeSource.incomeSourceName, (decimal)(incomeSource?.incomeSourceAmount));
                }
                else
                {
                    incomeSource.modifiedDate = DateTime.Now;
                }
                ExentionQuerys.InsertUpdateObject(incomeSource);
                bunifuSnackbar1.Show(this, action == EnumActions.QueryEnum.Insert ? MessagesValue.InsertSuccess : MessagesValue.UpdateSuccess, BunifuSnackbar.MessageTypes.Error, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                this.Close();
            }
            catch (Exception ex)
            {
                bunifuSnackbar1.Show(this, MessagesValue.MessageErrorSystem, BunifuSnackbar.MessageTypes.Error, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmIncomeSources_Load(object sender, EventArgs e)
        {
            this.GetData();
        }

        private void GetData()
        {
            if (incomeSource != null)
            {
                this.txtSourceName.Text = incomeSource?.incomeSourceName?.Trim();
                this.txtAmount.Text = incomeSource?.incomeSourceAmount?.ToString();
                this.dtpStartDate.Value = ExentionMethods.DateTimeIsNull(incomeSource.incomeSourceDate);
                this.txtDescription.Text = incomeSource?.description?.Trim();
            }
        }

        private void cbxTypeIncomeSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxTypeIncomeSource.SelectedIndex != -1)
            {
                incomeSource.categoryID = cbxTypeIncomeSource.SelectedIndex;
            }
        }
    }
}