using Bunifu.UI.WinForms;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FinanceManagement.Exentesions
{
    public class ExentionWinforms
    {
        private const int maxWidth = 335; // Đặt giới hạn chiều rộng label

        public static Image GetAvatarFormLocal(string avatar)
        {
            // Đường dẫn đầy đủ đến tệp hình ảnh
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, avatar);

            // Kiểm tra xem tệp hình ảnh có tồn tại
            if (File.Exists(fullPath))
            {
                // Load hình ảnh từ đường dẫn và gán cho PictureBox
                return Image.FromFile(fullPath);
            }
            return null;
        }

        public static void GetDateddMMyyyy(DateTimePicker dateTimePicker, DateTime? value)
        {
            dateTimePicker.Format = DateTimePickerFormat.Custom;
            dateTimePicker.CustomFormat = "dd/MM/yyyy";
            dateTimePicker.Value = (DateTime)ExentionMethods.FormatDateddMMyyyy(value);
            //    return dateTimePicker;
        }

        public static string CreateAvatar()
        {
            string targetDirectory = "avt";
            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }
            var image = ExentionMethods.GetAutoNumber("avatars") + "" + ".jpg";
            string targetFilePath = Path.Combine(targetDirectory, image);

            return targetFilePath;
        }

        public static Image UploadAvatar()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All Files|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return new Bitmap(openFileDialog.FileName);
            }
            return null;
        }

        public static Size TotalScrollMaxMinSize(bool sidebarExpand, Panel pnlSiderbarConatiner)
        {
            return new Size(sidebarExpand ? pnlSiderbarConatiner.MaximumSize.Width : pnlSiderbarConatiner.MinimumSize.Width, pnlSiderbarConatiner.Height);
        }

        public static void UpdateLabelAndProgress(BunifuLabel label, BunifuProgressBar progress, string labelText, decimal progressValue)
        {
            label.Visible = true;
            progress.Visible = true;

            // Cập nhật giá trị cho Label
            label.Text = labelText;

            // Kiểm tra và thực hiện các hành động nếu chiều rộng vượt quá giới hạn
            if (label.Width > maxWidth)
            {
                label.AutoSize = false;
                label.AutoSizeHeightOnly = false;
                label.AutoEllipsis = true;
                label.Width = maxWidth;
            }

            // Use Convert.ToInt32 to handle rounding and overflow gracefully
            int progressIntValue = Convert.ToInt32(progressValue);
            progress.Value = progressIntValue;
        }
    }
}