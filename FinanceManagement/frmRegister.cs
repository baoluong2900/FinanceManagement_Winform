using Bunifu.UI.WinForms;
using FinanceManagement.Exentesions;
using FinanceManagement.Message;
using System;
using System.Linq;
using System.Windows.Forms;
using static FinanceManagement.ModelTemp.ModelTemp;

namespace FinanceManagement
{
    public partial class frmRegister : Form
    {
        private dbFinanceManagementEntities dbcontext = new dbFinanceManagementEntities();

        public frmRegister()
        {
            InitializeComponent();
        }

        private void bunifuPictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bunifuLabel4_Click(object sender, EventArgs e)
        {
            frmLogin formLogin = new frmLogin();
            formLogin.Show();
            this.Hide();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                // check empty
                var userNameCheck = ExentionMethods.CheckNullValueText(txtUserName?.Text, lblUserName?.Text);
                var passwordCheck = ExentionMethods.CheckNullValueText(txtPassword?.Text, lblPassword?.Text);
                var enterPasswordCheck = ExentionMethods.CheckNullValueText(txtEnterPassword?.Text, lblEnterPassword?.Text);
                ValueView result = userNameCheck ?? passwordCheck ?? enterPasswordCheck;
                if (result != null)
                {
                    bunifuSnackbar1.Show(this, result.Text + result.Message, BunifuSnackbar.MessageTypes.Warning, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                    return;
                }

                // compare password
                if (ExentionMethods.CheckComparePassword(txtPassword?.Text, txtEnterPassword?.Text))
                {
                    bunifuSnackbar1.Show(this, MessagesValue.PasswordsDoNotMatch, BunifuSnackbar.MessageTypes.Warning, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                    return;
                }

                // check user exist
                var userExist = dbcontext.Users.FirstOrDefault(x => x.userName.ToLower() == txtUserName.Text.ToLower());
                if (userExist != null)
                {
                    bunifuSnackbar1.Show(this, MessagesValue.UserIsExist, BunifuSnackbar.MessageTypes.Warning, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                    return;
                }

                // insert user
                ExentionQuerys.InsertUpdateObject(ExentionMethods.CreateAccountUser(txtUserName?.Text, txtPassword?.Text));
                bunifuSnackbar1.Show(this, MessagesValue.RegisterSuccess, BunifuSnackbar.MessageTypes.Success, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                this.Reset();
            }
            catch (Exception ex)
            {
                bunifuSnackbar1.Show(this, MessagesValue.MessageErrorSystem, BunifuSnackbar.MessageTypes.Error, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
            }
        }

        private void Reset()
        {
            this.txtUserName.Text = string.Empty;
            this.txtPassword.Text = string.Empty;
            this.txtEnterPassword.Text = string.Empty;
        }
    }
}