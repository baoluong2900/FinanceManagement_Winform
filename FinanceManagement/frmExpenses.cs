using Bunifu.UI.WinForms;
using FinanceManagement.Enums;
using FinanceManagement.Exentesions;
using FinanceManagement.Message;
using FinanceManagement.ModelView;
using Guna.UI2.WinForms.Internal;
using Microsoft.VisualBasic.ApplicationServices;
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
    public partial class frmExpenses : Form
    {
        public Expens expenses = null;
        public int userID;
        public EnumActions.QueryEnum action = EnumActions.QueryEnum.Insert;
        private ValueListView valueListView = new ValueListView();
        private dbFinanceManagementEntities dbcontext = new dbFinanceManagementEntities();
        public frmExpenses(int _userID = -1, string expenseID = null)
        {
            InitializeComponent();
            userID = _userID;
            var typeSelect = 0;
            if (!string.IsNullOrEmpty(expenseID))
            {
                expenses = ExentionQuerys.GetObjectByValueId(new Expens(), int.Parse(expenseID), "expenseID", dbcontext);
                action = EnumActions.QueryEnum.Update;
                typeSelect = int.Parse(expenses?.categoryID.ToString());
            }
            else
            {
                expenses = new Expens();
                expenses.categoryID = 0;
            }
            cbxTypeExpenses.DataSource = valueListView.CbxListTypeExpenses;
            cbxTypeExpenses.DisplayMember = "Name";
            cbxTypeExpenses.ValueMember = "Value";
            cbxTypeExpenses.SelectedIndex = typeSelect;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var expensesNameCheck = ExentionMethods.CheckNullValueText(this.txtExpensesName.Text?.Trim(), lblSourceName?.Text);
                var amountCheck = ExentionMethods.CheckNullValueText(this.txtAmount.Text, lblAmount?.Text);
                ValueView result = expensesNameCheck ?? amountCheck;
                if (result != null)
                {
                    bunifuSnackbar1.Show(this, result.Text + result.Message, BunifuSnackbar.MessageTypes.Warning, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                    return;
                }
                expenses.userID = userID;
                expenses.description = this.txtDescription.Text?.Trim();
                expenses.expenseDate = this.dtpExpensesDate.Value;
                expenses.expenseAmount = decimal.TryParse(this.txtAmount.Text, out var amount) ? (decimal?)amount : null;
                expenses.expenseName = this.txtExpensesName.Text?.Trim();
                if (action == EnumActions.QueryEnum.Insert)
                {
                    expenses.createDate = DateTime.Now;
                    ExentionMethods.CreateTransaction(userID, 1, expenses.expenseName, (decimal)(expenses?.expenseAmount));
                }
                else
                {
                    expenses.modifiedDate = DateTime.Now;
                }
                ExentionQuerys.InsertUpdateObject(expenses);
                bunifuSnackbar1.Show(this, action == EnumActions.QueryEnum.Insert ? MessagesValue.InsertSuccess : MessagesValue.UpdateSuccess, BunifuSnackbar.MessageTypes.Error, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                this.Close();
            }
            catch (Exception ex)
            {
                bunifuSnackbar1.Show(this, MessagesValue.MessageErrorSystem, BunifuSnackbar.MessageTypes.Error, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
            }
        }
        private void cbxTypeExpenses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxTypeExpenses.SelectedIndex != -1)
            {
                expenses.categoryID = cbxTypeExpenses.SelectedIndex;
            }

        }
        private void GetData()
        {
            if (expenses != null)
            {
                this.txtExpensesName.Text = expenses?.expenseName?.Trim();
                this.txtAmount.Text = expenses?.expenseAmount?.ToString();
                this.dtpExpensesDate.Value = ExentionMethods.DateTimeIsNull(expenses.expenseDate);
                this.txtDescription.Text = expenses?.description?.Trim();
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

        private void frmExpenses_Load(object sender, EventArgs e)
        {
            this.GetData();
        }

    }
}
