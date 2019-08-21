using ASCOM.Astrometry.AstroUtils;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;
using System.ComponentModel;
using System.Threading;
using System.Timers;

namespace ASCOM.iOptronZEQ25
{
    public static class TelescopeHardware
    {
        //Object used to lock the CommandStrings
        private static readonly object cmdLock = new object();

        /// <summary>
        /// Private variable to hold an ASCOM AstroUtilities object to provide the Range method
        /// </summary>
        private static AstroUtils astroUtilities;

        private static bool internalSlewing = false;
        private static bool IsSlewing = false;
        private static Mutex protectSerial = new Mutex();
        private static System.Timers.Timer s_wTimer;

        //private static System.Windows.Forms.Timer s_wTimer;
        //private static System.Timers.Timer s_wTimer;

        private static System.Threading.Timer s_wTimerSlewing;

        private static int s_z = 0;
        private static double targetDeclination = SharedResources.INVALID_DOUBLE;
        private static double targetRightAscension = SharedResources.INVALID_DOUBLE;

        private static ASCOM.Utilities.TraceLogger tl;
        private static bool TrackingState;
        private static Util utilities;
        private static BackgroundWorker worker;

        static TelescopeHardware()
        {
            tl = new ASCOM.Utilities.TraceLogger("", "iOptron ZEQ25 Telescope Hardware")
            {
                Enabled = Properties.Settings.Default.Trace
            };
            utilities = new Util(); //Initialise util object
            astroUtilities = new AstroUtils(); // Initialise astro utilities object

            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;

            //Timer timer = new Timer(250);
            //timer.Elapsed += Timer_Elapsed;
            //timer.Start();

            s_wTimer = new System.Timers.Timer
            {
                Interval = 500 // average task completion is 333 ms
            };
            s_wTimer.Elapsed += WTimer_Tick;
            s_wTimer.Enabled = false;

            // Initialize the internal slewing timer
            s_wTimerSlewing = new System.Threading.Timer(OnSlewSettleTimeEvent, null, Timeout.Infinite, Timeout.Infinite);
        }

        public static double Altitude { get; internal set; }

        public static bool AtHome { get; internal set; }

        public static double Azimuth { get; internal set; }

        public static double Declination { get; internal set; }

        public static double Elevation { get; internal set; }

        public static int EquatorialSystem
        {
            //equOther              0 Custom or unknown equinox and/or reference frame.
            //equTopocentric        1 Local topocentric; this is the most common for amateur telescopes.
            //equJ2000              2 J2000 equator/equinox, ICRS reference frame.
            //equJ2050              3 J2050 equator/equinox, ICRS reference frame.
            //equB1950              4 B1950 equinox, FK4 reference frame.
            get { return (int)EquatorialCoordinateType.equTopocentric; }
        }

        public static double GuideRateDeclination
        {
            //Command: “:AG#”
            //Response: “n.nn#”
            //This command returns the guide rate.
            //The current Declination movement rate offset for telescope guiding (degrees/sec)
            get
            {
                String response = CommandString(":AG#", false);
                tl.LogMessage("GuideRateDeclination Get - ", response);
                response = response.Replace("#", "");
                return SharedResources.SiderealRateDPS * Convert.ToDouble(response);
            }
            //Command: “:RGnnn#”
            //Response: “1”
            //Selects guide rate nnn*0.01x sidereal rate. nnn is in the range of 10 to 90, and 100.
            //The current Declination movement rate offset for telescope guiding (degrees/sec)
            set
            {
                int guiderate = (int)((value / SharedResources.SiderealRateDPS) * 100);
                String command = ":RG" + guiderate.ToString().PadLeft(3, '0') + "#";
                CommandBool(command, false);
                tl.LogMessage("GuideRateDeclination Set", command);
            }
        }

        //public static double FocalLength
        //{
        //    get { return Convert.ToDouble(Properties.Settings.Default.FocalLength); }
        //    //set
        //    //{
        //    //    focalLength = value;
        //    //    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "FocalLength", value.ToString(CultureInfo.InvariantCulture));
        //    //}
        //}
        public static double GuideRateRightAscension
        {
            //Command: “:AG#”
            //Response: “n.nn#”
            //This command returns the guide rate.
            //The current Declination movement rate offset for telescope guiding (degrees/sec)
            get
            {
                String response = CommandString(":AG#", false);
                tl.LogMessage("GuideRateRightAscension Get - ", response);
                response = response.Replace("#", "");
                return SharedResources.SiderealRateDPS * Convert.ToDouble(response);
            }
            //Command: “:RGnnn#”
            //Response: “1”
            //Selects guide rate nnn*0.01x sidereal rate. nnn is in the range of 10 to 90, and 100.
            //The current Declination movement rate offset for telescope guiding (degrees/sec)
            set
            {
                int guiderate = (int)((value / SharedResources.SiderealRateDPS) * 100);
                String command = ":RG" + guiderate.ToString().PadLeft(3, '0') + "#";
                CommandBool(command, false);
                tl.LogMessage("GuideRateRightAscension Set", command);
            }
        }

        public static bool IsConnected { get; private set; }

        public static bool IsPulseGuiding { get; internal set; }

        public static double ParkAltitude { get; internal set; }

        public static double ParkAzimuth { get; internal set; }

        public static double RightAscension { get; internal set; }

        public static bool SetTracking
        {
            set
            {
                if (!IsConnected)
                    return;
                if (value)
                {
                    tl.LogMessage("Tracking", "Set - 1");
                    CommandBool(":ST1#", false);
                    Tracking = true;
                }
                else
                {
                    tl.LogMessage("Tracking", "Set - 0");
                    CommandBool(":ST0#", false);
                    Tracking = false;
                }
            }
        }

        public static PierSide SideOfPier { get; internal set; }

        public static double SiteLatitude { get; internal set; }

        public static double SiteLongitude { get; internal set; }

        public static bool Slewing
        {
            get
            {
                return IsSlewing || internalSlewing;
            }
            set
            {
                if (value == IsSlewing) return; // no change in slewing state
                if (value)
                {
                    AtHome = false;
                    IsSlewing = true;
                    return;
                }
                else
                {
                    IsSlewing = false;
                    UpdateSideOfPier();
                }
            }
        }

        public static double TargetDeclination
        {
            //get { return targetRaDec.Y; }
            //set { targetRaDec.Y = value; }
            get
            {
                // Note: ":GD#" gets the current declination! Not commanded declination.
                //String response = CommandString(":GD#", false);
                //tl.LogMessage("Declination", "Get - " + response);
                //response = response.Replace("#", "");
                //response = response.Replace("*", ":");
                //double targetdeclination = utilities.DMSToDegrees(response);
                //tl.LogMessage("TargetDeclination", "Get - " + targetdeclination);
                //return targetDeclination;
                if (targetDeclination == SharedResources.INVALID_DOUBLE)
                    throw new ASCOM.ValueNotSetException("TargetDeclination");
                return targetDeclination;
            }
            set
            {
                targetDeclination = value;
                String DDMMSS = utilities.DegreesToDMS(value, "*", ":", "");
                String sign = (value < 0) ? "" : "+";
                String Command = ":Sd " + sign + DDMMSS + "#";
                tl.LogMessage("TargetDeclination", "Set - Sending Command " + Command);
                CommandBool(Command, false);
            }
        }

        public static double TargetRightAscension
        {
            //get { return targetRaDec.X; }
            //set { targetRaDec.X = value; }
            get
            {
                //String response = CommandString(":GR#", false);
                //tl.LogMessage("TargetRightAscension", "Get - " + response);
                //response = response.Replace("#", "");
                //double rightAscension = utilities.HMSToHours(response);
                //tl.LogMessage("TargetRightAscension", "Get - " + rightAscension);
                //return targetRightAscension;
                if (targetRightAscension == SharedResources.INVALID_DOUBLE)
                    throw new ASCOM.ValueNotSetException("TargetRightAscension");
                return targetRightAscension;
            }
            set
            {
                targetRightAscension = value;
                String HHMMSS = utilities.HoursToHMS(value);
                String Command = ":Sr " + HHMMSS + "#";
                tl.LogMessage("TargetRightAscension", "Set - Sending Command " + Command);
                CommandBool(Command, false);
            }
        }

        public static bool Tracking { get; internal set; }

        public static DriveRates TrackingRate { get; set; }

        public static double SiderealTime
        {
            get
            {
                // Now using NOVAS 3.1
                double siderealTime = 0.0;
                using (var novas = new ASCOM.Astrometry.NOVAS.NOVAS31())
                {
                    var jd = utilities.DateUTCToJulian(DateTime.UtcNow);
                    novas.SiderealTime(jd, 0, novas.DeltaT(jd),
                        ASCOM.Astrometry.GstType.GreenwichApparentSiderealTime,
                        ASCOM.Astrometry.Method.EquinoxBased,
                        ASCOM.Astrometry.Accuracy.Reduced, ref siderealTime);
                }

                // Allow for the longitude
                siderealTime += SiteLongitude / 360.0 * 24.0;

                // Reduce to the range 0 to 24 hours
                siderealTime = astroUtilities.ConditionRA(siderealTime);

                //tl.LogMessage("SiderealTime", "Get - " + siderealTime.ToString());
                return siderealTime;
            }
        }

        public static double HourAngle
        {
            get
            {
                return astroUtilities.ConditionHA(SiderealTime - RightAscension);
            }
        }

        internal static bool Connected
        {
            get { return IsConnected; }
            set
            {
                lock (cmdLock)
                {
                    if (value)
                    {
                        if (s_z == 0)
                            Open();
                        s_z++;
                    }
                    else
                    {
                        s_z--;
                        if (s_z <= 0)
                        {
                            Close();
                        }
                    }
                }
            }
        }

        public static void Park()
        {
            Tracking = false;
        }

        public static void SlewToAltAz(double Azimuth, double Altitude)
        {
            tl.LogMessage("SlewToAltAz", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("SlewToAltAz");
        }

        public static void SlewToAltAzAsync(double Azimuth, double Altitude)
        {
            tl.LogMessage("SlewToAltAzAsync", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("SlewToAltAzAsync");
        }

        public static void Start()
        {
            IsConnected = false;
        }

        internal static void AbortSlew()
        {
            //if (!Slewing)
            //{
            //    tl.LogMessage("AbortSlew", "Sending Command :q#");
            //    CommandBlind(":q#", false);
            //}

            // Command: “:Q#”
            // Response: “1”
            // This command will stop slewing. Tracking and moving by arrow keys will not be affected.
            tl.LogMessage("AbortSlew", "Sending Command :Q#");
            bool response = CommandBool(":Q#", false);
            if (response)
            {
                tl.LogMessage("AbortSlew", "success");
            }
            else
            {
                tl.LogMessage("AbortSlew", "Error");
            }
        }

        internal static bool CanMoveAxis(TelescopeAxes axis)
        {
            switch (axis)
            {
                case TelescopeAxes.axisPrimary: return true;
                case TelescopeAxes.axisSecondary: return true;
                case TelescopeAxes.axisTertiary: return false;
                default: throw new InvalidValueException("CanMoveAxis", axis.ToString(), "0 to 2");
            }
        }

        internal static void CommandBlind(string command, bool raw)
        {
            CheckConnected("CommandBlind");
            //tl.LogMessage("CommandBlind", "command: " + command);
            // Use Mutex to ensure that only one command is in progress at a time
            if (protectSerial.WaitOne(1000)) // Wait for up to 1000 milliseconds
            {
                try
                {
                    //SharedResources.SharedSerial.ClearBuffers();
                    SharedResources.SharedSerial.Transmit(command);
                    tl.LogMessage("CommandBlind", "command sent: " + command);
                    System.Threading.Thread.Sleep(250); // Allow command to process
                }
                // Only catch timeout exceptions.
                catch (TimeoutException err)
                {
                    tl.LogMessage("CommandBlind", "Timeout exception: " + command + " Error: " + err.Message);
                    throw new ASCOM.DriverException(err.Message, err);
                }
                finally
                {
                    protectSerial.ReleaseMutex();
                }
            }
            else
            {
                tl.LogMessage("CommandBlind", "Mutex - Timeout on command : " + command);
            }
        }

        internal static bool CommandBool(string command, bool raw)
        {
            CheckConnected("CommandBool");
            //tl.LogMessage("CommandBool", "command: " + command);
            // Use Mutex to ensure that only one command is in progress at a time
            if (protectSerial.WaitOne(1000)) // Wait for up to 1000 milliseconds
            {
                try
                {
                    //SharedResources.SharedSerial.ClearBuffers();
                    SharedResources.SharedSerial.Transmit(command);
                    tl.LogMessage("CommandBool", "command sent: " + command);
                    bool response = (SharedResources.SharedSerial.ReceiveCounted(1) == "1");
                    tl.LogMessage("CommandBool", command + " response: " + response.ToString());
                    return response;
                }
                // Only catch timeout exceptions.
                catch (TimeoutException err)
                {
                    tl.LogMessage("CommandBool", "Timeout exception: " + command + " Error: " + err.Message);
                    throw new ASCOM.DriverException(err.Message, err);
                }
                catch (Exception ex)
                {
                    tl.LogMessage("CommandBool", "unhandled exception: " + command + " Error: " + ex.Message);
                    throw new ASCOM.DriverException(ex.Message, ex);
                }
                finally
                {
                    protectSerial.ReleaseMutex();
                }
            }
            else
            {
                tl.LogMessage("CommandBool", "Mutex - Timeout on command: " + command);
                return false;
            }
        }

        internal static string CommandString(string command, bool raw)
        {
            CheckConnected("CommandString");
            //tl.LogMessage("CommandString", "command: " + command);
            // Use Mutex to ensure that only one command is in progress at a time
            if (protectSerial.WaitOne(1000)) // Wait for up to 1000 milliseconds
            {
                try
                {
                    //SharedResources.SharedSerial.ClearBuffers();
                    SharedResources.SharedSerial.Transmit(command);
                    tl.LogMessage("CommandString", "command sent: " + command);
                    string response = SharedResources.SharedSerial.ReceiveTerminated("#");
                    tl.LogMessage("CommandString", "response: " + response.ToString());
                    return response;
                }
                // Only catch timeout exceptions.
                catch (TimeoutException err)
                {
                    tl.LogMessage("CommandString", "Timeout exception: " + command + " Error: " + err.Message);
                    throw new ASCOM.DriverException(err.Message, err);
                }
                finally
                {
                    protectSerial.ReleaseMutex();
                }
            }
            else
            {
                tl.LogMessage("CommandBool", "Mutex - Timeout on command: " + command);
                throw new ASCOM.DriverException("CommandString");
            }
        }

        internal static void CommandUpdateRaDec()
        {
            CheckConnected("CommandTest");
            // Use Mutex to ensure that only one command is in progress at a time
            if (protectSerial.WaitOne(1000)) // Wait for up to 1000 milliseconds
            {
                try
                {
                    tl.LogMessage("CommandUpdateRaDec", "started");
                    int start = System.Environment.TickCount;
                    SharedResources.SharedSerial.Transmit(":GR#:");
                    string GR = SharedResources.SharedSerial.ReceiveTerminated("#");
                    SharedResources.SharedSerial.Transmit(":GD#:");
                    string GD = SharedResources.SharedSerial.ReceiveTerminated("#");
                    UpdateRightAscension(GR);
                    UpdateDeclination(GD);
                    int stop = System.Environment.TickCount;
                    tl.LogMessage("CommandUpdateRaDec", "Exit : " + (stop - start));
                    //UpdateSlewing(SE);
                }
                // Only catch timeout exceptions.
                catch (TimeoutException err)
                {
                    tl.LogMessage("CommandTest", "Timeout exception: " + " Error: " + err.Message);
                    throw new ASCOM.DriverException(err.Message, err);
                }
                finally
                {
                    protectSerial.ReleaseMutex();
                }
            }
            else
            {
                throw new ASCOM.DriverException("CommandTest");
            }
        }

        internal static void FindHome()
        {
            //if (AtPark)
            //{
            //    throw new ParkedException("Cannot find Home when Parked");
            //}
            Tracking = false;
            internalSlewing = true;
            s_wTimerSlewing.Change(2500, Timeout.Infinite);
            //Command: “:MH#”
            //Respond: “1”
            //This command will slew to the “home” position immediately.
            tl.LogMessage("FindHome", "Finding Home");
            CommandBool(":MH#", false);
        }

        internal static void MoveAxis(TelescopeAxes axis, double rate)
        {
            if (rate != 0)
            {
                internalSlewing = true;
                //s_wTimerSlewing.Change(10000, Timeout.Infinite); // presumes commands should not take more than 10 seconds
                TrackingState = Tracking;
            }
            //Command: “:SRn#”
            //Response: “1”
            //Sets the moving rate used for the N-S-E-W buttons.
            // For n, specify an integer from 1 to 9.
            // 1 stands for 1x sidereal tracking rate, 2 stands for 2x,
            // 3 stands for 8x, 4 stands for 16x, 5 stands for 64x,
            // 6 stands for 128x, 7 stands for 256x, 8 stands for 512x,
            // 9 stands for maximum speed(larger than 512x).
            //Command: “:mn#” “:me#” “:ms#” “:mw#”
            //Response: (none)
            tl.LogMessage("MoveAxis", "Axis: " + axis);
            tl.LogMessage("MoveAxis", "Rate: " + rate);
            String command;
            bool response = false;
            int direction = Math.Sign(rate);
            int speed = (int)(Math.Abs(rate) / SharedResources.SiderealRateDPS);
            int speed_int = speed;
            // Round up to nearest power of 2 using bitwise method
            speed--;
            speed |= speed >> 1;  // divided by 2
            speed |= speed >> 2;  // divided by 4
            speed |= speed >> 4;  // divided by 16
            speed |= speed >> 8;  // divided by 256
            speed |= speed >> 16; // divided by
            speed++; // next power of 2
                     // Note: 4x and 32x are not supported so will need to be ignored or substituted
            int x = speed >> 1; // previous power of 2
            // next power of 2 - requested speed        (proximity to next power of 2)
            // requested speed - previous power of 2    (proximity to previous power of 2)
            // set speed_power to nearest power of 2
            int speed_power = (speed - speed_int) > (speed_int - x) ? x : speed;
            tl.LogMessage("MoveAxis", "Speed: " + speed + " x SiderealRateDPS");
            switch (speed_power)
            {
                case (0):
                    CommandBlind(":q#", false); // Stop moving
                    Tracking = TrackingState;
                    SetTracking = TrackingState;
                    // SetTracking prior to setting slewing flag
                    internalSlewing = false;
                    IsSlewing = false;
                    return;

                case (1):
                    response = CommandBool(":SR1#", false);
                    break;

                case (2):
                    response = CommandBool(":SR2#", false);
                    break;

                case (4):
                    response = CommandBool(":SR2#", false); // 4x not supported set to 2x instead
                    break;

                case (8):
                    response = CommandBool(":SR3#", false);
                    break;

                case (16):
                    response = CommandBool(":SR4#", false);
                    break;

                case (32):
                    response = CommandBool(":SR4#", false); // 32x not supported set to 16x instead
                    break;

                case (64):
                    response = CommandBool(":SR5#", false);
                    break;

                case (128):
                    response = CommandBool(":SR6#", false);
                    break;

                case (256):
                    response = CommandBool(":SR7#", false);
                    break;

                case (512):
                    response = CommandBool(":SR8#", false);
                    break;

                case (1024):
                    response = CommandBool(":SR9#", false);
                    break;
            }
            if (response == false)
            {
                tl.LogMessage("MoveAxis", "Error setting speed!");
                return;
            }
            switch (axis)
            {
                case TelescopeAxes.axisPrimary:
                    //command = (direction == -1) ? ":me#" : ":mw#";
                    // Note: due to iOptron ZEQ25 error (East and West are swapped!)
                    command = (direction == -1) ? ":mw#" : ":me#";
                    CommandBlind(command, false);
                    s_wTimerSlewing.Change(2500, Timeout.Infinite);
                    break;

                case TelescopeAxes.axisSecondary:
                    if (SideOfPier == PierSide.pierEast)
                    {
                        command = (direction == -1) ? ":ms#" : ":mn#";
                        CommandBlind(command, false);
                        s_wTimerSlewing.Change(2500, Timeout.Infinite);
                    }
                    if (SideOfPier == PierSide.pierWest)
                    {
                        // Note: due to iOptron ZEQ25 error (North and South are swapped!)
                        command = (direction == -1) ? ":mn#" : ":ms#";
                        CommandBlind(command, false);
                        s_wTimerSlewing.Change(2500, Timeout.Infinite);
                    }
                    //moving = true;
                    break;
            }
        }

        //set when MoveAxis is initiated, restored when it ended
        internal static void PulseGuide(GuideDirections direction, int duration)
        {
            //Command: “:MnXXXXX#” “:MsXXXXX#” “:MeXXXXX#” “:MwXXXXX#”
            //Response: (none)
            //Command motion for XXXXX milliseconds in the direction specified at the currently selected guide
            //rate. If XXXXX has a value of zero, motion is continuous and requires a “:Mx00000#” command
            //to terminate.x is the same direction of the previous command.
            // XXXXX is in the range of 0 to 32767.
            IsPulseGuiding = true;
            String XXXXX = duration.ToString().PadLeft(5, '0');
            switch (direction)
            {
                case ASCOM.DeviceInterface.GuideDirections.guideEast:
                    //CommandBlind(":Me" + XXXXX + "#", false);
                    // Note: due to iOptron ZEQ25 error (East and West are swapped!)
                    CommandBlind(":Mw" + XXXXX + "#", false);
                    break;

                case ASCOM.DeviceInterface.GuideDirections.guideNorth:
                    // Automatically takes care of DEC flip after meridian flip
                    if (SideOfPier == PierSide.pierEast)
                    {
                        CommandBlind(":Mn" + XXXXX + "#", false);
                    }
                    if (SideOfPier == PierSide.pierWest)
                    {
                        CommandBlind(":Ms" + XXXXX + "#", false);
                    }
                    break;

                case ASCOM.DeviceInterface.GuideDirections.guideSouth:
                    // Automatically takes care of DEC flip after meridian flip
                    if (SideOfPier == PierSide.pierEast)
                    {
                        CommandBlind(":Ms" + XXXXX + "#", false);
                    }
                    if (SideOfPier == PierSide.pierWest)
                    {
                        CommandBlind(":Mn" + XXXXX + "#", false);
                    }
                    break;

                case ASCOM.DeviceInterface.GuideDirections.guideWest:
                    //CommandBlind(":Mw" + XXXXX + "#", false);
                    // Note: due to iOptron ZEQ25 error (East and West are swapped!)
                    CommandBlind(":Me" + XXXXX + "#", false);
                    break;

                default:
                    break;
            }
            Thread.Sleep(duration);
            IsPulseGuiding = false;
        }

        internal static void SlewToCoordinates(double rightAscension, double declination)
        {
            internalSlewing = true;
            tl.LogMessage("SlewToCoordinates", "RightAscension " + rightAscension);
            tl.LogMessage("SlewToCoordinates", "Declincation " + declination);
            TargetRightAscension = rightAscension;
            TargetDeclination = declination;
            SlewToTarget();
        }

        internal static void SlewToCoordinatesAsync(double rightAscension, double declination)
        {
            internalSlewing = true;
            tl.LogMessage("SlewToCoordinatesAsync", "RightAscension " + rightAscension);
            tl.LogMessage("SlewToCoordinatesAsync", "Declincation " + declination);
            TargetRightAscension = rightAscension;
            TargetDeclination = declination;
            SlewToTargetAsync();
        }

        internal static void SlewToTarget()
        {
            internalSlewing = true;
            s_wTimerSlewing.Change(2500, Timeout.Infinite);
            //Server.s_MainForm.labelSlew.ForeColor = Color.Red;
            AtHome = false;
            tl.LogMessage("SlewToTarget", "Sending :MS#");
            CommandBool(":MS#", false);
            // Wait for Slew to Finish and return
            WaitForSlewToEnd();
        }

        internal static void SlewToTargetAsync()
        {
            internalSlewing = true;
            s_wTimerSlewing.Change(2500, Timeout.Infinite);
            //Server.s_MainForm.labelSlew.ForeColor = Color.Red;
            AtHome = false;
            tl.LogMessage("SlewToTargetAsync", "Sending :MS#");
            CommandBool(":MS#", false);
        }

        internal static void SyncToCoordinates(double rightAscension, double declination)
        {
            TargetRightAscension = rightAscension;
            TargetDeclination = declination;
            SyncToTarget();
            RightAscension = rightAscension;
            Declination = declination;
        }

        internal static void SyncToTarget()
        {
            tl.LogMessage("SyncToTarget", "Sending Command :CM#");
            CommandBool(":CM#", false);
        }

        internal static void WaitForSlewToEnd()
        {
            tl.LogMessage("WaitForSlewToEnd", "Slewing : " + Slewing);
            int when = System.Environment.TickCount + 60000; // slew should not take more tham 45 seconds
            while (System.Environment.TickCount < when && Slewing)
                System.Threading.Thread.Sleep(500);
        }

        private static void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new ASCOM.NotConnectedException(message);
            }
        }

        private static void Close()
        {
            IsConnected = false; // Signal that connection state is now false
            tl.LogMessage("Close()", "invoked");
            // No drivers should be connected now. Close communications.
            // System.Windows.Forms.MessageBox.Show("Disconnecting from Telescope Hardware");
            // Use Mutex to ensure that no command is in progress when closing serial line
            if (protectSerial.WaitOne(5000)) // Wait for up to 5000 milliseconds
            {
                tl.LogMessage("Close()", "protectSerial.WaitOne(5000) okay");
                try
                {
                    SharedResources.SharedSerial.Connected = false;
                }
                // Only catch timeout exceptions.
                catch (TimeoutException err)
                {
                    tl.LogMessage("close()", "Timeout exception: Error: " + err.Message);
                    throw new ASCOM.DriverException(err.Message, err);
                }
                finally
                {
                    protectSerial.ReleaseMutex();
                }
            }
            else
            {
                tl.LogMessage("close", "Timeout");
            }
        }

        private static string CommandStringInt(string command, bool v2)
        {
            CheckConnected("CommandStringInt");
            // Use Mutex to ensure that only one command is in progress at a time
            if (protectSerial.WaitOne(1000)) // Wait for up to 1000 milliseconds
            {
                try
                {
                    //SharedResources.SharedSerial.ClearBuffers();
                    SharedResources.SharedSerial.Transmit(command);
                    return SharedResources.SharedSerial.ReceiveCounted(1);
                }
                // Only catch timeout exceptions.
                catch (TimeoutException err)
                {
                    tl.LogMessage("CommandStringInt", "Timeout exception: " + command + " Error: " + err.Message);
                    throw new ASCOM.DriverException(err.Message, err);
                }
                finally
                {
                    protectSerial.ReleaseMutex();
                }
            }
            else
            {
                throw new ASCOM.DriverException("CommandStringInt");
            }
        }

        private static string CommandSequence(string command, bool v2)
        {
            CheckConnected("CommandStringInt");
            // Use Mutex to ensure that only one command is in progress at a time
            if (protectSerial.WaitOne(1000)) // Wait for up to 1000 milliseconds
            {
                try
                {
                    //SharedResources.SharedSerial.ClearBuffers();
                    SharedResources.SharedSerial.Transmit(command);
                    return SharedResources.SharedSerial.ReceiveCounted(1);
                }
                // Only catch timeout exceptions.
                catch (TimeoutException err)
                {
                    tl.LogMessage("CommandStringInt", "Timeout exception: " + command + " Error: " + err.Message);
                    throw new ASCOM.DriverException(err.Message, err);
                }
                finally
                {
                    protectSerial.ReleaseMutex();
                }
            }
            else
            {
                throw new ASCOM.DriverException("CommandStringInt");
            }
        }

        private static void OnSlewSettleTimeEvent(object state)
        {
            s_wTimerSlewing.Change(Timeout.Infinite, Timeout.Infinite);
            internalSlewing = false; // Slew start/stop should have finished
        }

        private static void Open()
        {
            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.COMPort))
            {
                throw new ASCOM.ValueNotSetException("COMPort");
            }
            SharedResources.SharedSerial.PortName = Properties.Settings.Default.COMPort;
            SharedResources.SharedSerial.Speed = SerialSpeed.ps9600;
            SharedResources.SharedSerial.DataBits = 8;
            SharedResources.SharedSerial.StopBits = ASCOM.Utilities.SerialStopBits.One;
            SharedResources.SharedSerial.Parity = ASCOM.Utilities.SerialParity.None;
            SharedResources.SharedSerial.ReceiveTimeout = 5;
            SharedResources.Connected = true; // increment count
            SharedResources.SharedSerial.ClearBuffers();
            //System.Threading.Thread.Sleep(1500); // Allow Arduino to reset
            tl.LogMessage("TelescopeHarware", "Open() - connecting to mount");
            try
            {
                String initialization_response, mount_type;
                //Command: “:V#”
                //Response: “V1.00#”
                //This command is the first initialization command of iOptron products.
                SharedResources.SharedSerial.Transmit(":V#");
                initialization_response = SharedResources.SharedSerial.ReceiveTerminated("#");
                //Command: “:MountInfo#”
                //Response: “8407”,“8497”,“8408” ,“8498”
                //This command gets the mount type. “8407” means iEQ45 EQ mode or iEQ30, “8497” means iEQ45
                //AA mode, “8408” means ZEQ25, “8498” means SmartEQ.
                SharedResources.SharedSerial.Transmit(":MountInfo#");
                mount_type = SharedResources.SharedSerial.ReceiveCounted(4);
                if (initialization_response == "V1.00#" && mount_type == "8408")
                {
                    tl.LogMessage("TelescopeHarware", "Open() - :V# V1.00#");
                    tl.LogMessage("TelescopeHarware", "Open() - :MountInfo# 8408");
                    IsConnected = true;
                    UpdateSiteLatitude();
                    UpdateSiteLongitude();
                    Properties.Settings.Default.Save();
                    s_wTimer.Enabled = true;
                }
                else
                {
                    // Problem connecting to ZEQ25
                    IsConnected = false; // possibly redundant
                    s_z = 0; // No connections.
                    SharedResources.SharedSerial.Connected = false; // Not ZEQ25. Disconnect!
                    SharedResources.connections = 0; // Telescope Hardware equals 1 connection
                    // setting count explicitely to zero
                }
            }
            // Only catch timeout exceptions.
            catch (TimeoutException err)
            {
                IsConnected = false; // possibly redundant
                tl.LogMessage("CommandString", "Timeout exception: " + err.Message);
                SharedResources.SharedSerial.Connected = false; // Not ZEQ25. Disconnect!
                SharedResources.connections = 0;
                // setting count explicitely to zero
                throw new ASCOM.DriverException(err.Message, err);
            }
        }

        public static void UpdateAltAz()
        {
            // Day offset and Local Siderial Time
            //double LST = (100.46 + 0.985647 * dayOffset + Long + 15 * (Date.Hour + Date.Minute / 60d) + 360) % 360;
            double LST = 360 * SiderealTime / 24; // degrees
            double RA = 360 * RightAscension / 24; // degrees
            double Dec = Declination; // degrees
            double Lat = SiteLatitude; // degrees

            // Hour Angle
            double HA = (LST - RA + 360) % 360;

            // HA, DEC, Lat to Alt, AZ
            double x = Math.Cos(HA * (Math.PI / 180)) * Math.Cos(Dec * (Math.PI / 180));
            double y = Math.Sin(HA * (Math.PI / 180)) * Math.Cos(Dec * (Math.PI / 180));
            double z = Math.Sin(Dec * (Math.PI / 180));

            double xhor = x * Math.Cos((90 - Lat) * (Math.PI / 180)) - z * Math.Sin((90 - Lat) * (Math.PI / 180));
            double yhor = y;
            double zhor = x * Math.Sin((90 - Lat) * (Math.PI / 180)) + z * Math.Cos((90 - Lat) * (Math.PI / 180));

            Azimuth = Math.Atan2(yhor, xhor) * (180 / Math.PI) + 180;
            Altitude = Math.Asin(zhor) * (180 / Math.PI);
        }

        //private static void UpdateAltitude()
        //{
        //    //double RA = utilities.HMSToDegrees(RightAscension);
        //    //double HourAngleDeg = Math.PI * RightAscension / 24.0;
        //    //Altitude = Math.Asin(Math.Sin(Declination) * Math.Sin(SiteLatitude) + Math.Cos(Declination) * Math.Cos(SiteLatitude) * Math.Cos(HourAngle));

        //    if (Slewing || !Connected)
        //    {
        //        return;
        //    }
        //    //Command: “:GA#”
        //    //Response: “sDD*MM:SS#”
        //    //Gets the current Altitude.
        //    //SharedResources.TrafficStart(SharedResources.MessageType.Gets, "Altitude: ");
        //    String response = CommandString(":GA#", false);
        //    response = response.Replace("#", "");
        //    response = response.Replace("*", ":");
        //    Altitude = utilities.DMSToDegrees(response);
        //    tl.LogMessage("Altitude", "Get - " + response);
        //    //Server.s_MainForm.Altitude(altAzm.Y);
        //    //SharedResources.TrafficEnd(utilities.DegreesToDMS(TelescopeHardware.Altitude));
        //}

        private static void UpdateAtHome()
        {
            if (Slewing || !IsConnected || Declination != 90.0)
            {
                AtHome = false;
                return;
            }
            // Command: “:AH#”
            // Respond: “0” The telescope is not at “home” position,
            // “1” The telescope is at “home” position.
            // This command returns whether the telescope is at “home” position.
            bool response = CommandBool(":AH#", false);
            AtHome = response;
            //tl.LogMessage("AtHome", "Get - " + response.ToString());
        }

        //private static void UpdateAzimuth()
        //{
        //    // (degrees, North zero and increasing clockwise, i.e., 90 East, 180 South, 270 West)
        //    if (Slewing || !IsConnected)
        //    {
        //        return;
        //    }
        //    //Command: “:GZ#”
        //    //Response: “DDD*MM:SS#”
        //    //Gets the current Azimuth.
        //    String response = CommandString(":GZ#", false);
        //    response = response.Replace("#", "");
        //    response = response.Replace("*", ":");
        //    Azimuth = utilities.DMSToDegrees(response);
        //    tl.LogMessage("Azimuth", "Get - " + response);
        //}

        private static void UpdateDeclination()
        {
            //Command: “:GD#”
            //Response: “sDD*MM:SS#”
            String response = CommandString(":GD#", false);
            if (response != null)
            {
                response = response.Replace("#", "");
                response = response.Replace("*", ":");
                Declination = utilities.DMSToDegrees(response);
            }
            //tl.LogMessage("Declination", "Get - " + Declination);
        }

        private static void UpdateDeclination(string response)
        {
            response = response.Replace("#", "");
            response = response.Replace("*", ":");
            Declination = utilities.DMSToDegrees(response);
            //tl.LogMessage("UpdateDeclination", "" + Declination);
        }

        private static void UpdateDisplay()
        {
            //SiderealTime = AstronomyFunctions.LocalSiderealTime(SiteLongitude);
            //// display the values, must be done on the UI thread
            //Server.s_MainForm.SiderealTime(SiderealTime);
            //Server.s_MainForm.Altitude(Altitude);
            //Server.s_MainForm.Azimuth(Azimuth);
            //Server.s_MainForm.RightAscension(RightAscension);
            //Server.s_MainForm.Declination(Declination);
            //Server.s_MainForm.Tracking();
            //Server.s_MainForm.LedPier(SideOfPier);
            //if (AtPark) Server.s_MainForm.lblPARK.ForeColor = Color.Red;
            //else Server.s_MainForm.lblPARK.ForeColor = Color.SaddleBrown;
            //if (AtHome) Server.s_MainForm.lblHOME.ForeColor = Color.Red;
            //else Server.s_MainForm.lblHOME.ForeColor = Color.SaddleBrown;
            //if (!Slewing) Server.s_MainForm.labelSlew.ForeColor = Color.SaddleBrown;
            //else Server.s_MainForm.labelSlew.ForeColor = Color.Red;
        }

        private static void UpdatePositions()
        {
            // Prevent overloading ZEQ25 with commands handling all updates here
            //s_wTimer.Enabled = false; // prevent UpdatePositions being called while it is busy
            if (IsConnected)
            {
                tl.LogMessage("UpdatePositions", "Enter");
                int start = System.Environment.TickCount;
                UpdateSlewing();
                CommandUpdateRaDec();
                //UpdateRightAscension();
                //UpdateDeclination();
                UpdateAltAz();
                // if not slewing
                UpdateAtHome();
                UpdateTracking();
                //UpdateAltitude(); // if not slewing
                //UpdateAzimuth(); // if not slewing
                UpdateSideOfPier(); // if not slewing
                //UpdateDisplay();
                int stop = System.Environment.TickCount;
                tl.LogMessage("UpdatePositions", "Exit : " + (stop - start));
            }
            //s_wTimer.Enabled = true; // re-enable the timer
        }

        private static void UpdateRightAscension()
        {
            //Command: “:GR#”
            //Response: “HH:MM:SS#”
            //Gets the current Right Ascension.
            String response = CommandString(":GR#", false);
            //tl.LogMessage("RightAscension", "Get - " + response);
            if (response != null)
            {
                response = response.Replace("#", "");
                RightAscension = utilities.HMSToHours(response);
            }
            //tl.LogMessage("RightAscension", "Get - " + RightAscension);
        }

        private static void UpdateRightAscension(string response)
        {
            response = response.Replace("#", "");
            RightAscension = utilities.HMSToHours(response);
            //tl.LogMessage("UpdateRightAscension", "" + RightAscension);
        }

        private static void UpdateSideOfPier()
        {
            if (AtHome || !Connected)
            {
                return;
            }
            //Command: “:pS#”
            //Response: “0” East, “1” West.
            tl.LogMessage("UpdateSideOfPier", "");
            //throw new ASCOM.PropertyNotImplementedException("SideOfPier", false);
            // if :pS# returns  true telescope is on west side pointing east (or west through the pole)
            // if :pS# returns false telescope is on east side pointing west (or east through the pole)
            //bool throughThePole = (HourAngle < -6 || HourAngle > +6);

            // Report SideofPier at HA -9, +9: WE
            // Report SideofPier at HA -3, +3: WE

            // pierWest is returned when the mount is observing at an hour angle between -6.0 and 0.0
            // pierEast is returned when the mount is observing at an hour angle between 0.0 and + 6.0

            SideOfPier = (HourAngle < 0) ? PierSide.pierWest : PierSide.pierEast;

            //bool result = CommandBool(":pS#", false);
            ////if (Azimuth < 90 || Azimuth > 270)
            //if (HourAngle < -6 || HourAngle > +6)
            //{
            //    SideOfPier = result ? PierSide.pierWest : PierSide.pierEast;
            //}
            //else
            //{
            //    SideOfPier = result ? PierSide.pierEast : PierSide.pierWest;
            //}
        }

        private static void UpdateSiteLatitude()
        {
            //Command: “:Gt#”
            //Response: “sDD*MM:SS#”
            //Gets the current latitude. Note the return value will be in signed format, North is positive.
            String response = CommandString(":Gt#", false);
            response = response.Replace("#", "");
            response = response.Replace("*", ":");
            SiteLatitude = utilities.DMSToDegrees(response);
            tl.LogMessage("UpdateSiteLatitude", "Get - " + SiteLatitude);
            Properties.Settings.Default.Latitude = SiteLatitude;
        }

        private static void UpdateSiteLongitude()
        {
            //Command: “:Gg#”
            //Response: “sDDD*MM:SS#”
            //Gets the current longitude. Note the return value will be in signed format, East is positive.
            String response = CommandString(":Gg#", false);
            tl.LogMessage("UpdateSiteLongitude", "Get - " + response);
            response = response.Replace("#", "");
            response = response.Replace("*", ":");
            SiteLongitude = utilities.DMSToDegrees(response);
            tl.LogMessage("UpdateSiteLongitude", "Get - " + SiteLongitude);
            Properties.Settings.Default.Longitude = SiteLongitude;
        }

        private static void UpdateSlewing()
        {
            //Command: “:SE?#”
            //Response: “0” not in slewing, “1” in slewing.
            //This command get the slewing status.
            tl.LogMessage("UpdateSlewing", "started");
            int start = System.Environment.TickCount;
            bool slewing = CommandBool(":SE?#", false);
            Slewing = slewing || internalSlewing; /// slewing is true if slewing started even if mount not yet reporting slewing
            //tl.LogMessage("UpdateSlewing", slewing.ToString());
            int stop = System.Environment.TickCount;
            tl.LogMessage("UpdateSlewing", "Exit : " + (stop - start));

        }

        //private static void UpdateSlewing(bool slewing)
        //{
        //    if (IsConnected)
        //    {
        //        Slewing = slewing;
        //        //tl.LogMessage("UpdateSlewing", slewing.ToString());
        //    }
        //}

        private static void UpdateTracking()
        {
            if (Slewing || !IsConnected)
            {
                return;
            }
            bool tracking = CommandBool(":AT#", false);
            Tracking = tracking;
            //tl.LogMessage("Tracking", "Get - " + tracking.ToString());
        }

        private static void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            UpdatePositions();
        }

        private static void WTimer_Tick(object sender, ElapsedEventArgs e)
        {
            if (!worker.IsBusy)
                worker.RunWorkerAsync();
        }

        //private static void WTimer_Tick(object sender, EventArgs e)
        //{
        //    UpdatePositions();
        //}
    }
}