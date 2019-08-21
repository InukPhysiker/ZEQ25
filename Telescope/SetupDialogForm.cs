using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.Utilities;
using ASCOM.iOptronZEQ25;

namespace ASCOM.iOptronZEQ25
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        public SetupDialogForm()
        {
            InitializeComponent();
            // Initialise current values of user settings from the ASCOM Profile
            InitUI();
        }

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here
            // Update the state variables with results from the dialogue
            if (comboBoxComPort.Items.Count == 0)
            {
                MessageBox.Show("No available COM Port!", "ASCOM Driver for ZEQ25", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Properties.Settings.Default.COMPort = (string)comboBoxComPort.SelectedItem;
                Properties.Settings.Default.Trace = chkTrace.Checked;
                Properties.Settings.Default.Save();
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Properties.Settings.Default.Reload();
            Close();
        }

        private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
        {
            try
            {
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void InitUI()
        {
            Properties.Settings.Default.Reload();
            chkTrace.Checked = Properties.Settings.Default.Trace;
            // set the list of com ports to those that are currently available
            comboBoxComPort.Items.Clear();
            comboBoxComPort.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());      // use System.IO because it's static
            // select the current port if possible
            if (comboBoxComPort.Items.Contains(Properties.Settings.Default.COMPort))
            {
                comboBoxComPort.SelectedItem = Properties.Settings.Default.COMPort;
            }
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            //this.TopLevel = true;

            // this bizarre sequence seems to be required to bring the setup dialog to the front.
            this.TopMost = true;
            this.Activate();
            this.BringToFront();
            this.TopMost = false;

        }

    }
}