using Bunifu.UI.WinForms;
using FinanceManagement.Exentesions;
using FinanceManagement.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static FinanceManagement.ModelTemp.ModelTemp;
using static FinanceManagement.ModelView.ValueListView;

namespace FinanceManagement
{
    public partial class frmInfomartionUser : Form
    {
        public User userLogin;
        public int userID;
        public bool isLoadLocation = false;
        private bool isUpdateAvatar;
        private dbFinanceManagementEntities dbcontext = new dbFinanceManagementEntities();
        private List<Location> locations = new List<Location>();
        private List<Location> districts = new List<Location>();
        private List<Location> wards = new List<Location>();

        public frmInfomartionUser(int _userID = -1)
        {
            InitializeComponent();
            userID = _userID;
            if (_userID != -1)
            {
                userLogin = ExentionQuerys.GetObjectByValueId(new User(), userID, "userID", dbcontext);
            }
            else
            {
                userLogin = new User();
            }
            isUpdateAvatar = false;
            ExentionWinforms.GetDateddMMyyyy(dtpBirthday, userLogin.birthday);
            this.GetData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void bunifuTextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void frmInfomartionUser_Load(object sender, EventArgs e)
        {
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GetData()
        {
            this.txtFirstName.Text = userLogin?.firstName?.Trim();
            this.txtLastName.Text = userLogin?.lastName?.Trim();
            this.txtAddress.Text = userLogin?.address?.Trim();
            this.txtPhone.Text = userLogin?.phone?.Trim();
            this.txtPasswordEnter.Enabled = false;
            this.txtPasswordNew.Enabled = false;
            //  this.dtpBirthday.Value = userLogin.birthday.Value;
            radioFemale.Checked = ExentionMethods.CheckSexUser(userLogin?.sex, HardValue.Female);
            radioMale.Checked = ExentionMethods.CheckSexUser(userLogin?.sex, HardValue.Male);
            radioOther.Checked = ExentionMethods.CheckSexUser(userLogin?.sex, HardValue.Other);
            if (!string.IsNullOrEmpty(userLogin?.avatar))
            {
                picboxAvatar.Image = ExentionWinforms.GetAvatarFormLocal(userLogin?.avatar);
            }
            locations = ExentionQuerys.GetListObjectSearchByValue(new Location(), -1, string.Empty, item => item.levels == 0, dbcontext);
            cbxLocationID.DisplayMember = "Name"; // Chọn thuộc tính cần hiển thị
            cbxLocationID.ValueMember = "Value"; // Chọn thuộc tính cần lấy giá trị
            cbxLocationID.SelectedIndex = -1;
            cbxLocationID.DataSource = ExentionMethods.ConvertLisCbxLocations(locations.Select(x => x.name).ToList());

            if (!string.IsNullOrEmpty(userLogin.locationID))
            {
                cbxLocationID.SelectedIndex = ExentionMethods.FindIndexComboxLocations((List<ComboBoxItem>)cbxLocationID.DataSource, userLogin?.locationID?.Trim(), locations);
                cbxLocationID.DisplayMember = "Name"; // Chọn thuộc tính cần hiển thị
                cbxLocationID.ValueMember = "Value"; // Chọn thuộc tính cần lấy giá trị
                cbxLocationID.DataSource = ExentionMethods.ConvertLisCbxLocations(locations.Select(x => x.name).ToList());

                districts = ExentionQuerys.GetListObjectSearchByValue(new Location(), -1, string.Empty, item => item.levels == 1 && item.parent.Trim() == userLogin.locationID?.Trim(), dbcontext);
                cbxDistrict.DisplayMember = "Name"; // Chọn thuộc tính cần hiển thị
                cbxDistrict.ValueMember = "Value"; // Chọn thuộc tính cần lấy giá trị
                cbxDistrict.DataSource = ExentionMethods.ConvertLisCbxLocations(districts.Select(x => x.name).ToList());

                if (!string.IsNullOrEmpty(userLogin.district))
                {
                    cbxDistrict.SelectedIndex = ExentionMethods.FindIndexComboxLocations((List<ComboBoxItem>)cbxDistrict.DataSource, userLogin?.district, districts);
                    cbxWard.Enabled = true;

                    wards = ExentionQuerys.GetListObjectSearchByValue(new Location(), -1, string.Empty, item => item.levels == 2 && item.parent.Trim() == userLogin.district?.Trim(), dbcontext);
                    cbxWard.DisplayMember = "Name"; // Chọn thuộc tính cần hiển thị
                    cbxWard.ValueMember = "Value"; // Chọn thuộc tính cần lấy giá trị
                    cbxWard.DataSource = ExentionMethods.ConvertLisCbxLocations(wards.Select(x => x.name).ToList());
                    if (!string.IsNullOrEmpty(userLogin.district))
                    {
                        cbxWard.SelectedIndex = ExentionMethods.FindIndexComboxLocations((List<ComboBoxItem>)cbxWard.DataSource, userLogin?.ward, wards);
                    }
                    else
                    {
                        cbxWard.SelectedIndex = -1;
                        cbxWard.Enabled = false;
                    }
                }
                else
                {
                    cbxWard.SelectedIndex = -1;
                    cbxDistrict.SelectedIndex = -1;
                    cbxDistrict.Enabled = false;
                    cbxWard.Enabled = false;
                }
            }
            else
            {
                cbxLocationID.SelectedIndex = -1;
                cbxDistrict.Enabled = false;
                cbxWard.Enabled = false;
            }
            isLoadLocation = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                userLogin.firstName = this.txtFirstName.Text.Trim();
                userLogin.lastName = this.txtLastName.Text.Trim();
                userLogin.address = this.txtAddress.Text.Trim();
                userLogin.phone = this.txtPhone.Text.Trim();
                userLogin.birthday = this.dtpBirthday.Value;
                userLogin.sex = ExentionMethods.CheckdSexUser(radioMale.Checked, radioFemale.Checked, radioOther.Checked);

                if (!string.IsNullOrEmpty(txtPasswordCurrent.Text.Trim()))
                {
                    if (ExentionMethods.VerifyPassword(txtPasswordCurrent.Text.Trim(), userLogin.passWord))
                    {
                        var passwordCheck = ExentionMethods.CheckNullValueText(txtPasswordNew?.Text, lblPasswordNew?.Text);
                        var enterPasswordCheck = ExentionMethods.CheckNullValueText(txtPasswordEnter?.Text, lblPasswordEnter?.Text);
                        ValueView result = passwordCheck ?? enterPasswordCheck;
                        if (result != null)
                        {
                            bunifuSnackbar1.Show(this, result.Text + result.Message, BunifuSnackbar.MessageTypes.Warning, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                            return;
                        }

                        // compare password
                        if (ExentionMethods.CheckComparePassword(txtPasswordNew?.Text, txtPasswordEnter?.Text))
                        {
                            bunifuSnackbar1.Show(this, MessagesValue.PasswordsDoNotMatch, BunifuSnackbar.MessageTypes.Warning, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                            return;
                        }

                        userLogin.passWord = ExentionMethods.HashPassword(txtPasswordNew?.Text);
                    }
                    else
                    {
                        bunifuSnackbar1.Show(this, MessagesValue.PasswordIsFail, BunifuSnackbar.MessageTypes.Error, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
                        return;
                    }
                }

                if (picboxAvatar.Image != null && isUpdateAvatar)
                {
                    var targetFilePath = ExentionWinforms.CreateAvatar();
                    userLogin.avatar = targetFilePath;
                    picboxAvatar.Image.Save(targetFilePath);
                }
                ExentionQuerys.InsertUpdateObject(userLogin);
                this.Close();
            }
            catch (Exception ex)
            {
                bunifuSnackbar1.Show(this, MessagesValue.MessageErrorSystem, BunifuSnackbar.MessageTypes.Error, 1000, "", BunifuSnackbar.Positions.TopRight, BunifuSnackbar.Hosts.FormOwner);
            }
        }

        private void picboxAvatar_Click(object sender, EventArgs e)
        {
            var openAvatar = ExentionWinforms.UploadAvatar();
            if (openAvatar != null)
            {
                picboxAvatar.Image = openAvatar;
                isUpdateAvatar = true;
            }
        }

        private void bunifuTextBox3_TextChanged(object sender, EventArgs e)
        {
        }

        private void cbxWard_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void cbxLocationID_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cbxLocationID.SelectedIndex != -1 && isLoadLocation)
            {
                userLogin.locationID = ExentionMethods.FindValueComboxLocations((List<ComboBoxItem>)cbxLocationID.DataSource, cbxLocationID.SelectedIndex, locations);

                districts = ExentionQuerys.GetListObjectSearchByValue(new Location(), -1, string.Empty, item => item.levels == 1 && item.parent.Trim() == userLogin.locationID?.Trim(), dbcontext);
                cbxDistrict.DisplayMember = "Name"; // Chọn thuộc tính cần hiển thị
                cbxDistrict.ValueMember = "Value"; // Chọn thuộc tính cần lấy giá trị
                isLoadLocation = false;
                cbxDistrict.DataSource = ExentionMethods.ConvertLisCbxLocations(districts.Select(x => x.name).ToList());
                cbxDistrict.Enabled = true;
                cbxDistrict.SelectedIndex = -1;
                cbxDistrict.SelectedItem = null;
                userLogin.district = string.Empty;
                cbxWard.Enabled = false;
                cbxWard.SelectedIndex = -1;
                userLogin.ward = string.Empty;
                isLoadLocation = true;
            }
        }

        private void cbxDistrict_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cbxDistrict.SelectedIndex != -1 && isLoadLocation)
            {
                userLogin.district = ExentionMethods.FindValueComboxLocations((List<ComboBoxItem>)cbxDistrict.DataSource, cbxDistrict.SelectedIndex, districts);

                wards = ExentionQuerys.GetListObjectSearchByValue(new Location(), -1, string.Empty, item => item.levels == 2 && item.parent.Trim() == userLogin.district?.Trim(), dbcontext);
                cbxWard.DisplayMember = "Name"; // Chọn thuộc tính cần hiển thị
                cbxWard.ValueMember = "Value"; // Chọn thuộc tính cần lấy giá trị
                isLoadLocation = false;
                cbxWard.DataSource = ExentionMethods.ConvertLisCbxLocations(wards.Select(x => x.name).ToList());
                cbxWard.Enabled = true;
                cbxWard.SelectedIndex = -1;
                cbxWard.SelectedItem = null;
                cbxWard.SelectedIndex = -1;
                userLogin.ward = string.Empty;
                isLoadLocation = true;
            }
        }

        private void cbxWard_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cbxWard.SelectedIndex != -1 && isLoadLocation)
            {
                userLogin.ward = ExentionMethods.FindValueComboxLocations((List<ComboBoxItem>)cbxWard.DataSource, cbxWard.SelectedIndex, wards);
            }
        }

        private void txtPasswordCurrent_TextChange(object sender, EventArgs e)
        {
            this.txtPasswordEnter.Enabled = !string.IsNullOrEmpty(this.txtPasswordCurrent.Text.Trim());
            this.txtPasswordNew.Enabled = !string.IsNullOrEmpty(this.txtPasswordCurrent.Text.Trim());
        }
    }
}