using Bunifu.UI.WinForms;
using FinanceManagement.Enums;
using FinanceManagement.Exentesions;
using FinanceManagement.Message;
using FinanceManagement.ModelView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FinanceManagement.ModelTemp.ModelTemp;

namespace FinanceManagement
{
    public partial class frmFinancialPlan : Form
    {
        public FinancialPlan financialPlan = null;
        public int userID;
        public EnumActions.QueryEnum action = EnumActions.QueryEnum.Insert;
        private ValueListView valueListView = new ValueListView();
        private dbFinanceManagementEntities dbcontext = new dbFinanceManagementEntities();
        public frmFinancialPlan(int _userID = -1, string financialPlanID = null)
        {
            InitializeComponent();
            userID = _userID;
            var typeSelect = 0;
            var typeStatus = 0;
            decimal progress = 0;
            if (!string.IsNullOrEmpty(financialPlanID))
            {
                financialPlan = ExentionQuerys.GetObjectByValueId(new FinancialPlan(), int.Parse(financialPlanID), "financialPlanID", dbcontext);
                action = EnumActions.QueryEnum.Update;
                typeSelect = int.Parse(financialPlan?.cateogryID.ToString());
                typeStatus = int.Parse(financialPlan?.status.ToString());
                progress = (decimal)(financialPlan?.progress.Value);
            }
            else
            {
                financialPlan = new FinancialPlan();
                financialPlan.cateogryID = 0;
                financialPlan.status = 0;
                financialPlan.progress = 0;
            }
            cbxCategory.DataSource = valueListView.CbxListFinancialCategory;
            cbxCategory.DisplayMember = "Name";
            cbxCategory.ValueMember = "Value";
            cbxCategory.SelectedIndex = typeSelect;

            cbxStatus.DataSource = valueListView.CbxListFinancialStatus;
            cbxStatus.DisplayMember = "Name";
            cbxStatus.ValueMember = "Value";
            cbxStatus.SelectedIndex = typeStatus;
            this.numberProgress.Value = progress;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var financialNameCheck = ExentionMethods.CheckNullValueText(this.txtFinancialPlan.Text?.Trim(), this.lblFinancialName?.Text);
                ValueView result = financialNameCheck;
                if (result != null)
                {
                    bunifuSnackbar1.Show(this, result.Text + result.Message, BunifuSnackbar.MessageTypes.Warning, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                    return;
                }
                financialPlan.financialPlanName = this.txtFinancialPlan?.Text?.Trim();
                financialPlan.userID = userID;
                financialPlan.description = this.txtDescription.Text?.Trim();
                financialPlan.startDate = this.dtpStartDate.Value;
                financialPlan.endDate = this.dtpEndDate.Value;
                financialPlan.progress = this.numberProgress.Value;
                if (action == EnumActions.QueryEnum.Insert)
                {
                    financialPlan.createDate = DateTime.Now;
                }
                else
                {
                    financialPlan.modifiedDate = DateTime.Now;
                }
                ExentionQuerys.InsertUpdateObject(financialPlan);
                bunifuSnackbar1.Show(this, action == EnumActions.QueryEnum.Insert ? MessagesValue.InsertSuccess : MessagesValue.UpdateSuccess, BunifuSnackbar.MessageTypes.Error, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                this.Close();
            }
            catch (Exception ex)
            {
                bunifuSnackbar1.Show(this, MessagesValue.MessageErrorSystem, BunifuSnackbar.MessageTypes.Error, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
            }
        }
        private void GetData()
        {
            if (financialPlan != null)
            {
                this.txtFinancialPlan.Text = financialPlan?.financialPlanName?.Trim();;
                this.dtpStartDate.Value = ExentionMethods.DateTimeIsNull(financialPlan.startDate);
                this.dtpEndDate.Value = ExentionMethods.DateTimeIsNull(financialPlan.endDate);
                this.txtDescription.Text = financialPlan?.description?.Trim();
            }
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void frmFinancialPlan_Load(object sender, EventArgs e)
        {
            this.GetData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCategory.SelectedIndex != -1)
            {
                financialPlan.cateogryID =cbxCategory.SelectedIndex;
            }
        }

        private void cbxStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxStatus.SelectedIndex != -1)
            {
                financialPlan.status = cbxStatus.SelectedIndex;
                this.numberProgress.Enabled = cbxStatus.SelectedIndex != 0 && cbxStatus.SelectedIndex != 2;
                this.numberProgress.Value = ExentionMethods.GetProgess(cbxStatus.SelectedIndex, this.numberProgress.Value);
            }
        }
    }
}
