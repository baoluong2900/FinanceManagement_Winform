using Bunifu.UI.WinForms;
using FinanceManagement.Exentesions;
using FinanceManagement.Message;
using System;
using System.Linq;
using System.Windows.Forms;
using static FinanceManagement.ModelTemp.ModelTemp;

namespace FinanceManagement
{
    public partial class frmLogin : Form
    {
        private dbFinanceManagementEntities dbcontext = new dbFinanceManagementEntities();
        private string passwordAdmin = "admin";
        private string userAdmin = "admin";

        public frmLogin()
        {
            InitializeComponent();
        }

        private void bunifuPictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bunifuLabel4_Click(object sender, EventArgs e)
        {
            frmRegister formRegister = new frmRegister();
            formRegister.Show();
            this.Hide();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUserName?.Text.Trim() == userAdmin && txtPassword?.Text == passwordAdmin.Trim())
                {
                    MainAdmin formMainAdmin = new MainAdmin();
                    formMainAdmin.Show();
                    this.Hide();
                }
                else
                {
                    // check empty
                    var userNameCheck = ExentionMethods.CheckNullValueText(txtUserName?.Text, lblUserName?.Text);
                    var passwordCheck = ExentionMethods.CheckNullValueText(txtPassword?.Text, lblPassword?.Text);
                    ValueView result = userNameCheck ?? passwordCheck;
                    if (result != null)
                    {
                        bunifuSnackbar1.Show(this, result.Text + result.Message, BunifuSnackbar.MessageTypes.Warning, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                        return;
                    }

                    // get user;
                    var userExist = dbcontext.Users.FirstOrDefault(x => x.userName.ToLower() == txtUserName.Text.ToLower());
                    if (userExist == null)
                    {
                        bunifuSnackbar1.Show(this, MessagesValue.UserIsNotExist, BunifuSnackbar.MessageTypes.Error, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                        return;
                    }
                    // check pasword login
                    if (ExentionMethods.VerifyPassword(txtPassword.Text, userExist.passWord))
                    {
                        frmHome formHome = new frmHome(userExist);
                        formHome.Show();
                        this.Hide();
                    }
                    else
                    {
                        bunifuSnackbar1.Show(this, MessagesValue.PasswordIsFail, BunifuSnackbar.MessageTypes.Error, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                bunifuSnackbar1.Show(this, MessagesValue.MessageErrorSystem, BunifuSnackbar.MessageTypes.Error, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
            }

            //  this.Reset();
        }

        private void Reset()
        {
            this.txtUserName.Text = string.Empty;
            this.txtPassword.Text = string.Empty;
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
        }
    }
}