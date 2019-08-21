using ASCOM.Controls;
using System;
using System.Windows.Forms;

namespace ASCOM.iOptronZEQ25
{
    public partial class Form1 : Form
    {

        private ASCOM.DriverAccess.Telescope driver;

        delegate void SetTextCallback(string text);

        private Utilities.Util util = new ASCOM.Utilities.Util();

        public Form1()
        {
            InitializeComponent();
            SetUIState();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsConnected)
                driver.Connected = false;

            Properties.Settings.Default.Save();
        }

        private void buttonChoose_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DriverId = ASCOM.DriverAccess.Telescope.Choose(Properties.Settings.Default.DriverId);
            SetUIState();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                driver.Connected = false;
            }
            else
            {
                driver = new ASCOM.DriverAccess.Telescope(Properties.Settings.Default.DriverId);
                driver.Connected = true;
            }
            SetUIState();
        }

        private void SetUIState()
        {
            buttonConnect.Enabled = !string.IsNullOrEmpty(Properties.Settings.Default.DriverId);
            buttonChoose.Enabled = !IsConnected;
            buttonConnect.Text = IsConnected ? "Disconnect" : "Connect";
        }

        private bool IsConnected
        {
            get
            {
                return ((this.driver != null) && (driver.Connected == true));
            }
        }

        //private void buttonSetup_Click(object sender, EventArgs e)
        //{
        //    DoSetupDialog();
        //    SetSlewButtons();
        //}

        private void buttonTraffic_Click(object sender, EventArgs e)
        {
            //SharedResources.TrafficForm.Show();
        }

        private void SetSlewButtons()
        {
            if (driver.SiteLatitude < 0)
            {
                buttonSlewUp.Text = "S";
                buttonSlewDown.Text = "N";
                buttonSlewRight.Text = "E";
                buttonSlewLeft.Text = "W";
            }
            else
            {
                buttonSlewUp.Text = "N";
                buttonSlewDown.Text = "S";
                buttonSlewRight.Text = "E";
                buttonSlewLeft.Text = "W";
            }
        }

        public void SiderealTime(double value)
        {
            SetTextCallback setText = new SetTextCallback(SetLstText);
            string text = util.HoursToHMS(value);
            try { this.Invoke(setText, text); }
            catch { }
        }

        public void RightAscension(double value)
        {
            SetTextCallback setText = new SetTextCallback(SetRaText);
            string text = util.HoursToHMS(value);
            try { this.Invoke(setText, text); }
            catch { }
        }

        public void Declination(double value)
        {
            SetTextCallback setText = new SetTextCallback(SetDecText);
            string text = util.DegreesToDMS(value);
            try { this.Invoke(setText, text); }
            catch { }
        }

        public void Altitude(double value)
        {
            SetTextCallback setText = new SetTextCallback(SetAltitudeText);
            string text = util.DegreesToDMS(value);
            try { this.Invoke(setText, text); }
            catch { }
        }

        public void Azimuth(double value)
        {
            SetTextCallback setText = new SetTextCallback(SetAzimuthText);
            string text = util.DegreesToDMS(value);
            try { this.Invoke(setText, text); }
            catch { }
        }

        //public void ParkButton(string value)
        //{
        //    SetTextCallback setText = new SetTextCallback(SetParkButtonText);
        //    try { this.Invoke(setText, value); }
        //    catch { }
        //}

        //private void frmMain_Load(object sender, EventArgs e)
        //{
        //    SetSlewButtons();
        //    driver.Start();
        //}

        #region Thread Safe Callback Functions
        private void SetLstText(string text)
        {
            labelLst.Text = text;
        }
        private void SetRaText(string text)
        {
            labelRa.Text = text;
        }
        private void SetDecText(string text)
        {
            labelDec.Text = text;
        }
        private void SetAltitudeText(string text)
        {
            labelAlt.Text = text;
        }
        private void SetAzimuthText(string text)
        {
            labelAz.Text = text;
        }

        //private void SetParkButtonText(string text)
        //{
        //    buttonUnpark.Text = text;
        //}

        #endregion

        #region slew/guide control using buttons

        //private static double GuideDuration()
        //{
        //    double duration = driver.GuideDurationShort;

        //    if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
        //    {
        //        duration = driver.GuideDurationMedium;
        //    }
        //    else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
        //    {
        //        duration = driver.GuideDurationLong;
        //    }
        //    return duration;
        //}

        //private static void SetSlewSpeed()
        //{
        //    if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
        //    {
        //        //driver.SlewSpeed = SlewSpeed.SlewMedium;
        //    }
        //    else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
        //    {
        //        //driver.SlewSpeed = SlewSpeed.SlewSlow;
        //    }
        //    else
        //    {
        //        //driver.SlewSpeed = SlewSpeed.SlewFast;
        //    }
        //}

        private void buttonSlewUp_MouseDown(object sender, MouseEventArgs e)
        {
            //StartSlew(SlewDirection.SlewUp);
            //driver.Action("SetSpeed")
            //switch (driver.AxisRates(DeviceInterface.TelescopeAxes.axisPrimary))
            //{
            //    case "1":
            //        break;
            //    default:
            //        break;
                    
            //}
            //driver.MoveAxis(DeviceInterface.TelescopeAxes.axisSecondary, driver.Speed);
        }

        private void buttonSlewUp_MouseUp(object sender, MouseEventArgs e)
        {
            //SetPulseGuideParms(guideRate, 0.0);
            driver.MoveAxis(DeviceInterface.TelescopeAxes.axisSecondary, 0);
        }

        private void buttonSlewDown_MouseDown(object sender, MouseEventArgs e)
        {
            //StartSlew(SlewDirection.SlewDown);
            //driver.MoveAxis(DeviceInterface.TelescopeAxes.axisSecondary, -driver.Speed);
        }

        private void buttonSlewDown_MouseUp(object sender, MouseEventArgs e)
        {
            //SetPulseGuideParms(-guideRate, 0.0);
            driver.MoveAxis(DeviceInterface.TelescopeAxes.axisSecondary, 0);
        }

        private void buttonSlewRight_MouseDown(object sender, MouseEventArgs e)
        {
            //StartSlew(SlewDirection.SlewRight);
            //driver.MoveAxis(DeviceInterface.TelescopeAxes.axisPrimary, driver.Speed);
        }

        private void buttonSlewRight_MouseUp(object sender, MouseEventArgs e)
        {
            //SetPulseGuideParms(0.0, guideRate);
            driver.MoveAxis(DeviceInterface.TelescopeAxes.axisPrimary, 0);
        }

        private void buttonSlewLeft_MouseDown(object sender, MouseEventArgs e)
        {
            //StartSlew(SlewDirection.SlewLeft);
            //driver.MoveAxis(DeviceInterface.TelescopeAxes.axisPrimary, -driver.Speed);
        }

        private void buttonSlewLeft_MouseUp(object sender, MouseEventArgs e)
        {
            //SetPulseGuideParms(0.0, -guideRate);
            driver.MoveAxis(DeviceInterface.TelescopeAxes.axisPrimary, 0);
        }

        private void buttonSlewStop_Click(object sender, EventArgs e)
        {
            driver.AbortSlew();
        }
        #endregion

        private void checkBoxTrack_CheckedChanged(object sender, EventArgs e)
        {
            if (driver.Tracking == checkBoxTrack.Checked)
                return;
            driver.Tracking = checkBoxTrack.Checked;
        }

        public void Tracking()
        {
            if (driver.Tracking == checkBoxTrack.Checked)
                return;
            // this avoids triggering the checked changed event
            checkBoxTrack.CheckState = driver.Tracking ? CheckState.Checked : CheckState.Unchecked;
        }

        public void LedPier(ASCOM.DeviceInterface.PierSide sideOfPier)
        {
            if (sideOfPier == ASCOM.DeviceInterface.PierSide.pierEast)
            {
                ledPierEast.Status = TrafficLight.Green;
                ledPierEast.Visible = true;
                ledPierWest.Visible = false;
            }
            else
            {
                ledPierWest.Status = TrafficLight.Red;
                ledPierWest.Visible = true;
                ledPierEast.Visible = false;
            }
        }

        //private void buttonUnpark_Click(object sender, EventArgs e)
        //{
        //    if (driver.IsParked)
        //    {
        //        driver.ChangePark(false);
        //        driver.Tracking = true;
        //    }
        //    else
        //    {
        //        driver.ChangePark(true);
        //        driver.Tracking = false;
        //        driver.Park();
        //    }
        //}

        private void buttonHome_Click(object sender, EventArgs e)
        {
            driver.FindHome();
        }

        private void comboBoxSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            // driver.Speed = Convert.ToDouble(comboBoxSpeed.Text.Trim().TrimEnd('x')) * 0.004178;
            // SiderealRateDPS = 0.004178
        }
    }
}
