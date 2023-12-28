using Bunifu.UI.WinForms;
using Bunifu.Framework.UI;
using FinanceManagement.Enums;
using FinanceManagement.Exentesions;
using FinanceManagement.Message;
using FinanceManagement.ModelView;
using System;
using System.Windows.Forms;
using static FinanceManagement.ModelTemp.ModelTemp;

namespace FinanceManagement
{
    public partial class frmDebtLoan : Form
    {
        public Debt debt = null;
        public int userID;
        public bool isDept = false; // true = vay mượn; false = nợ
        public EnumActions.QueryEnum action = EnumActions.QueryEnum.Insert;
        private ValueListView valueListView = new ValueListView();
        private dbFinanceManagementEntities dbcontext = new dbFinanceManagementEntities();

        public frmDebtLoan(int _userID = -1, string debtID = null)
        {
            InitializeComponent();
            userID = _userID;
            var typeSelectPayment = 0;
            var typeSelectStatus = 0;
            if (!string.IsNullOrEmpty(debtID))
            {
                debt = ExentionQuerys.GetObjectByValueId(new Debt(), int.Parse(debtID), "debtID", dbcontext);
                action = EnumActions.QueryEnum.Update;
                typeSelectPayment = int.Parse(debt?.repaymentPlan.ToString());
                typeSelectStatus = int.Parse(debt?.debtStatus.ToString());
            }
            else
            {
                debt = new Debt();
                debt.repaymentPlan = typeSelectPayment;
               debt.debtStatus = typeSelectStatus;
                debt.isDebt = true;
            }
            cbxTypePayment.DataSource = valueListView.CbxListPayment;
            cbxTypePayment.DisplayMember = "Name";
            cbxTypePayment.ValueMember = "Value";
            cbxTypePayment.SelectedIndex = typeSelectPayment;


            cbxDebtStatus.DataSource = valueListView.CbxListDebtStatus;
            cbxDebtStatus.DisplayMember = "Name";
            cbxDebtStatus.ValueMember = "Value";
            cbxDebtStatus.SelectedIndex = typeSelectStatus;
        }

        private void bunifuLabel4_Click(object sender, EventArgs e)
        {
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var debtNameCheck = ExentionMethods.CheckNullValueText(this.txtDebtName.Text?.Trim(),lblDebtName?.Text);
                var amountCheck = ExentionMethods.CheckNullValueText(this.txtAmount.Text, lblAmount?.Text);
                ValueView result = debtNameCheck ?? amountCheck;
                if (result != null)
                {
                    bunifuSnackbar1.Show(this, result.Text + result.Message, BunifuSnackbar.MessageTypes.Warning, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                    return;
                }
                debt.userID = userID;
                debt.description = this.txtDescription.Text?.Trim();
                debt.debtDueDate = this.dtpDebtDate.Value;
                debt.debtAmount = decimal.TryParse(this.txtAmount.Text, out var amount) ? (decimal?)amount : null;
                debt.debtName = this.txtDebtName.Text?.Trim();
                debt.isDebt = this.radioDebt.Checked;
                if (action == EnumActions.QueryEnum.Insert)
                {
                    debt.createDate = DateTime.Now;
                    ExentionMethods.CreateTransaction(userID, debt?.isDebt == true ? 2:3, debt.debtName, (decimal)(debt?.debtAmount));
                }
                else
                {
                    debt.modifiedDate = DateTime.Now;
                }
                ExentionQuerys.InsertUpdateObject(debt);

                bunifuSnackbar1.Show(this, action == EnumActions.QueryEnum.Insert ? MessagesValue.InsertSuccess : MessagesValue.UpdateSuccess, BunifuSnackbar.MessageTypes.Error, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                this.Close();
            }
            catch (Exception ex)
            {
                bunifuSnackbar1.Show(this, MessagesValue.MessageErrorSystem, BunifuSnackbar.MessageTypes.Error, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
            }
        }

        private void cbxTypePayment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxTypePayment.SelectedIndex != -1)
            {
                debt.repaymentPlan = cbxTypePayment.SelectedIndex;
            }
        }

        private void frmDebt_Load(object sender, EventArgs e)
        {
            this.GetData();
        }

        private void GetData()
        {
            if (debt != null)
            {
                this.txtDebtName.Text = debt?.debtName?.Trim();
                this.txtAmount.Text = debt?.debtAmount?.ToString();
                this.dtpDebtDate.Value = ExentionMethods.DateTimeIsNull(debt.debtDueDate);
                this.txtDescription.Text = debt?.description?.Trim();
                if((bool)debt?.isDebt)
                {
                    this.radioLoan.Checked = true;
                }
                else
                {
                    this.radioDebt.Checked = true;
                }
            }
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioDebt_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cbxDebtStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDebtStatus.SelectedIndex != -1)
            {
                debt.debtStatus = cbxDebtStatus.SelectedIndex;
            }
        }
    }
}