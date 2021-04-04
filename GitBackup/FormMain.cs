using GitBackup.Properties;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GitBackup
{
    public partial class FormMain : Form
    {
        #region Private Fields

        private bool messageShowing = false;
        private static readonly string newLine = Environment.NewLine;

        #endregion

        #region Public Methods

        public FormMain()
        {
            InitializeComponent();
            MaximumSize = new Size(int.MaxValue, Height);
            MinimumSize = Size;
        }

        #endregion

        #region Private Methods

        private void AddComboBoxItems(ComboBox comboBox, ICollection collection)
        {
            if (collection == null)
            {
                return;
            }

            comboBox.BeginUpdate();

            try
            {
                foreach (string item in collection)
                {
                    comboBox.Items.Add(item);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                comboBox.EndUpdate();
            }

            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
        }

        private void Initialize()
        {
            Settings settings;

            try
            {
                settings = Settings.Default;
                MainEngine.Command = settings.Command;
                MainEngine.GitPath = settings.GitBin;
                MainEngine.NewLine = Unescape(settings.NewLine);
            }
            catch (Exception exception)
            {
                ShowErrorMessage(exception.Message);
                return;
            }

            try
            {
                AddComboBoxItems(comboBoxLocalBranch, settings.Branches);
            }
            catch (Exception exception)
            {
                ShowErrorMessage(exception.Message);
            }

            try
            {
                AddComboBoxItems(comboBoxDestination, settings.Destinations);
            }
            catch (Exception exception)
            {
                ShowErrorMessage(exception.Message);
            }
        }

        private void ShowErrorMessage(string message)
        {
            ShowMessage(message, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private DialogResult ShowMessage(string text, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            if (messageShowing)
            {
                return DialogResult.None;
            }

            messageShowing = true;
            DialogResult result = MessageBox.Show(this, text, Text, buttons, icon);
            messageShowing = false;
            return result;
        }

        private string Unescape(string value)
        {
            return value
                .Replace("\\n", "\n")
                .Replace("\\r", "\r")
                .Replace("\\t", "\t");
        }

        #endregion

        // Designer's Methods

        private void buttonBackup_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            string branchName;
            string destination;

            try
            {
                string branch = comboBoxLocalBranch.Text;
                branchName = Path.GetFileName(branch);
                destination = Path.Combine(comboBoxDestination.Text,
                    string.Format("{0:D4}{1:D2}{2:D2}{3:D2}{4:D2}{5:D2}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second));
                MainEngine.Export(branch, destination);
            }
            catch (Exception exception)
            {
                string message = exception.Message;
                Exception innerException = exception.InnerException;

                if (innerException != null)
                {
                    message += string.Format("{0}({1})", newLine, innerException.Message);
                }

                ShowErrorMessage(message);
                return;
            }

            ShowMessage(string.Format("{0}{1}has been successfully backed up to{1}{2}.", branchName, newLine, destination), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void load(object sender, EventArgs e)
        {
            Initialize();
        }
    }
}
