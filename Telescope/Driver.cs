//tabs=4
// --------------------------------------------------------------------------------
// Custom driver for ZEQ25
//
// ASCOM Telescope driver for iOptronZEQ25
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam
//				erat, sed diam voluptua. At vero eos et accusam et justo duo
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Telescope interface version: 1.0
// Author:		Alan Duffy <alan.duffy@usask.ca>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 18-FEB-2019	XXX	6.0.0	Initial edit, created from ASCOM driver template
// --------------------------------------------------------------------------------
//

// This is used to define code in the template that is specific to one class implementation
// unused code canbe deleted and this definition removed.
#define Telescope
#define Server

using ASCOM.Astrometry.AstroUtils;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

namespace ASCOM.iOptronZEQ25
{
    //
    // Your driver's DeviceID is ASCOM.iOptronZEQ25.Telescope
    //
    // The Guid attribute sets the CLSID for ASCOM.iOptronZEQ25.Telescope
    // The ClassInterface/None addribute prevents an empty interface called
    // _iOptronZEQ25 from being created and used as the [default] interface
    //
    // TODO Replace the not implemented exceptions with code to implement the function or
    // throw the appropriate ASCOM exception.
    //

    /// <summary>
    /// ASCOM Telescope Driver for iOptronZEQ25.
    /// </summary>
    [Guid("35f11bab-d72a-475d-abe3-af2f5a4598c1")]
    [ProgId("ASCOM.iOptronZEQ25.Telescope")]
    [ServedClassName("iOptronZEQ25")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Telescope : ReferenceCountedObjectBase, ITelescopeV3
    {

        /// <summary>
        /// Variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        /// </summary>
        internal static TraceLogger tl;

        internal static string traceStateDefault = "false";

        // Constants used for Profile persistence
        internal static string traceStateProfileName = "Trace Level";

        // TODO Change the descriptive string for your driver then remove this line
        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        internal string driverDescription = "ASCOM Telescope Driver for iOptronZEQ25.";

        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        internal string driverID; // = "ASCOM.iOptronZEQ25.Telescope";
        // Variables to hold the currrent device configuration

        /// <summary>
        /// Private variable to hold an ASCOM AstroUtilities object to provide the Range method
        /// </summary>
        private AstroUtils astroUtilities;

        /// <summary>
        /// Private variable to hold the connected state
        /// </summary>
        private bool connectedState;

        /// <summary>
        /// Private variable to hold an ASCOM Utilities object
        /// </summary>
        private Util utilities;
        private double targetRightAscension = SharedResources.INVALID_DOUBLE;
        private double targetDeclination = SharedResources.INVALID_DOUBLE;
        private bool internalSlewing;

        /// <summary>
        /// Initializes a new instance of the <see cref="iOptronZEQ25"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Telescope()
        {
            driverID = Marshal.GenerateProgIdForType(this.GetType());
            driverDescription = GetDriverDescription();

            tl = new TraceLogger("", "iOptronZEQ25")
            {
                Enabled = false
            };

            LogMessage("Telescope", "Starting initialisation");

            connectedState = false; // Initialise connected to false
            utilities = new Util(); //Initialise util object
            astroUtilities = new AstroUtils(); // Initialise astro utilities object

            TrackingRate = DriveRates.driveSidereal;

            LogMessage("Telescope", "Completed initialisation");
        }

        ~Telescope()
        {
            if (IsConnected)
            {
                Connected = false;
            }
        }

        private string GetDriverDescription()
        {
            string description;
            if (this.GetType().GetCustomAttributes(typeof(ServedClassNameAttribute), true).FirstOrDefault() is ServedClassNameAttribute attr)
            {
                description = attr.DisplayName;
            }
            else
            {
                description = this.GetType().Assembly.FullName;
            }
            return description;
        }
        //
        // PUBLIC COM INTERFACE ITelescopeV3 IMPLEMENTATION
        //

        #region Common properties and methods.

        public bool Connected
        {
            get
            {
                LogMessage("Connected", "Get {0}", IsConnected);
                return IsConnected;
            }
            set
            {
                //lock (ConnectionLock)
                {
                    LogMessage("Connected", "Set {0}", value);
                    if (value == IsConnected)
                        return;

                    if (value)
                    {
                        LogMessage("Connected Set", "Connecting to Telescope Hardware");
                        // connect to the device
                        TelescopeHardware.Connected = true; // increase connection count
                        System.Threading.Thread.Sleep(250); // Allow command to process
                        connectedState = true;
                    }
                    else
                    {
                        LogMessage("Connected Set", "Disconnecting from Telescope Hardware");
                        TelescopeHardware.Connected = false; // decrease connection count
                        System.Threading.Thread.Sleep(250); // Allow command to process
                        connectedState = false;
                    }
                }
            }
        }

        public string Description
        {
            // TODO customise this device description
            get
            {
                LogMessage("Description Get", driverDescription);
                return driverDescription;
            }
        }

        public string DriverInfo
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                // TODO customise this driver description
                string driverInfo = "Information about the driver itself. Version: " + String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        public short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                LogMessage("InterfaceVersion Get", "3");
                return Convert.ToInt16("3");
            }
        }

        public string Name
        {
            get
            {
                string name = "Short driver name - please customise";
                LogMessage("Name Get", name);
                return name;
            }
        }

        public ArrayList SupportedActions
        {
            get
            {
                LogMessage("SupportedActions Get", "Returning empty arraylist");
                return new ArrayList();
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            LogMessage("", "Action {0}, parameters {1} not implemented", actionName, actionParameters);
            throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        }

        public void CommandBlind(string command, bool raw)
        {
            CheckConnected("CommandBlind");
            // Call CommandString and return as soon as it finishes
            TelescopeHardware.CommandBlind(command, raw);
        }

        public bool CommandBool(string command, bool raw)
        {
            CheckConnected("CommandBool");
            return TelescopeHardware.CommandBool(command, raw);
        }

        public string CommandString(string command, bool raw)
        {
            CheckConnected("CommandString");
            return TelescopeHardware.CommandString(command, raw);
        }

        public void Dispose()
        {
            // Clean up the tracelogger and util objects
            //tl.Enabled = false;
            //tl.Dispose();
            //tl = null;
            utilities.Dispose();
            utilities = null;
            astroUtilities.Dispose();
            astroUtilities = null;
        }

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            // consider only showing the setup dialog if not connected
            // or call a different dialog if connected
            //if (TelescopeHardware.IsConnected)
            //    System.Windows.Forms.MessageBox.Show("Already connected, just press OK");

            if (TelescopeHardware.Connected)
                throw new InvalidOperationException("The hardware is connected, cannot do SetupDialog()");

            using (SetupDialogForm F = new SetupDialogForm())
            {
                var result = F.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    // WriteProfile(); // Persist device configuration values to the ASCOM Profile store
                }
            }
        }

        #endregion Common properties and methods.

        #region ITelescope Implementation

        public AlignmentModes AlignmentMode
        {
            get
            {
                LogMessage("AlignmentMode Get", "algGermanPolar");
                return AlignmentModes.algGermanPolar;
            }
        }

        public double Altitude
        {
            get
            {
                //SharedResources.TrafficStart(SharedResources.MessageType.Gets, "Altitude: ");
                //CheckCapability(TelescopeHardware.CanAltAz, "Altitude", false);
                //SharedResources.TrafficEnd(utilities.DegreesToDMS(TelescopeHardware.Altitude));
                return TelescopeHardware.Altitude;
            }
        }

        public double ApertureArea
        {
            get
            {
                LogMessage("ApertureArea Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("ApertureArea", false);
            }
        }

        public double ApertureDiameter
        {
            get
            {
                LogMessage("ApertureDiameter Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("ApertureDiameter", false);
            }
        }

        public bool AtHome
        {
            get
            {
                //SharedResources.TrafficLine(SharedResources.MessageType.Polls, "AtHome: " + TelescopeHardware.AtHome);
                return TelescopeHardware.AtHome;
            }
        }

        public bool AtPark
        {
            get
            {
                LogMessage("AtPark", "Get - " + false.ToString());
                return false;
            }
        }

        public double Azimuth
        {
            get
            {
                //SharedResources.TrafficStart(SharedResources.MessageType.Polls, "Azimuth: ");
                //CheckCapability(TelescopeHardware.CanAltAz, "Azimuth", false);
                //SharedResources.TrafficEnd(utilities.DegreesToDMS(TelescopeHardware.Azimuth));
                return TelescopeHardware.Azimuth;
            }
        }

        // CanFindHome
        public bool CanFindHome
        {
            get
            {
                return true;
            }
        }

        public bool CanPark
        {
            get
            {
                return false;
            }
        }

        public bool CanPulseGuide
        {
            get
            {
                return true;
            }
        }

        public bool CanSetDeclinationRate
        {
            get
            {
                return false;
            }
        }

        public bool CanSetGuideRates
        {
            get
            {
                return true;
            }
        }

        public bool CanSetPark
        {
            get
            {
                return false;
            }
        }

        public bool CanSetPierSide
        {
            get
            {
                return false;
            }
        }

        public bool CanSetRightAscensionRate
        {
            get
            {
                return false;
            }
        }

        public bool CanSetTracking
        {
            get
            {
                return true;
            }
        }

        public bool CanSlew
        {
            get
            {
                return true;
            }
        }

        public bool CanSlewAltAz
        {
            get
            {
                return false;
            }
        }

        public bool CanSlewAltAzAsync
        {
            get
            {
                return false;
            }
        }

        public bool CanSlewAsync // (required by StellariumScope)
        {
            get
            {
                return true;
            }
        }

        public bool CanSync
        {
            get
            {
                return true;
            }
        }

        public bool CanSyncAltAz
        {
            get
            {
                return false;
            }
        }

        public bool CanUnpark
        {
            get
            {
                return false;
            }
        }

        public double Declination
        {
            get
            {
                //SharedResources.TrafficStart(SharedResources.MessageType.Gets, "Declination: ");
                //SharedResources.TrafficEnd(utilities.DegreesToDMS(TelescopeHardware.Declination));
                return TelescopeHardware.Declination;
            }
        }

        public double DeclinationRate
        {
            get
            {
                double declination = 0.0;
                LogMessage("DeclinationRate", "Get - " + declination.ToString());
                return declination;
            }
            set
            {
                LogMessage("DeclinationRate Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("DeclinationRate", true);
            }
        }

        public bool DoesRefraction
        {
            get
            {
                LogMessage("DoesRefraction Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("DoesRefraction", false);
            }
            set
            {
                LogMessage("DoesRefraction Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("DoesRefraction", true);
            }
        }

        public EquatorialCoordinateType EquatorialSystem
        {
            get
            {
                //SharedResources.TrafficStart(SharedResources.MessageType.Other, "EquatorialSystem: ");
                //string output = "";
                EquatorialCoordinateType eq = EquatorialCoordinateType.equOther;

                switch (TelescopeHardware.EquatorialSystem)
                {
                    case 0:
                        eq = EquatorialCoordinateType.equOther;
                        //output = "Other";
                        break;

                    case 1:
                        eq = EquatorialCoordinateType.equTopocentric;
                        //output = "Local";
                        break;

                    case 2:
                        eq = EquatorialCoordinateType.equJ2000;
                        //output = "J2000";
                        break;

                    case 3:
                        eq = EquatorialCoordinateType.equJ2050;
                        //output = "J2050";
                        break;

                    case 4:
                        eq = EquatorialCoordinateType.equB1950;
                        //output = "B1950";
                        break;
                }
                //SharedResources.TrafficEnd(output);
                return eq;
            }
        }

        public double FocalLength
        {
            get
            {
                LogMessage("FocalLength Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("FocalLength", false);
            }
        }

        public double GuideRateDeclination
        {
            get
            {
                //SharedResources.TrafficStart(SharedResources.MessageType.Gets, "GuideRateDeclination: ");
                //SharedResources.TrafficEnd(TelescopeHardware.GuideRateDeclination.ToString(CultureInfo.InvariantCulture));
                return TelescopeHardware.GuideRateDeclination;
            }
            set
            {
                //SharedResources.TrafficStart(SharedResources.MessageType.Gets, "GuideRateDeclination->: ");
                //SharedResources.TrafficEnd(value.ToString(CultureInfo.InvariantCulture));
                TelescopeHardware.GuideRateDeclination = value;
            }
        }

        public double GuideRateRightAscension
        {
            get
            {
                //SharedResources.TrafficStart(SharedResources.MessageType.Other, "GuideRateRightAscension: ");
                //SharedResources.TrafficEnd(TelescopeHardware.GuideRateRightAscension.ToString(CultureInfo.InvariantCulture));
                return TelescopeHardware.GuideRateRightAscension;
            }
            set
            {
                //SharedResources.TrafficStart(SharedResources.MessageType.Other, "GuideRateRightAscension->: ");
                //SharedResources.TrafficEnd(value.ToString(CultureInfo.InvariantCulture));
                TelescopeHardware.GuideRateRightAscension = value;
            }
        }

        public bool IsPulseGuiding
        {
            get
            {
                //SharedResources.TrafficStart(SharedResources.MessageType.Polls, "IsPulseGuiding: ");
                // TODO Is this correct, should it just return false?
                //CheckCapability(TelescopeHardware.CanPulseGuide, "IsPulseGuiding", false);
                //SharedResources.TrafficEnd(TelescopeHardware.IsPulseGuiding.ToString());
                return TelescopeHardware.IsPulseGuiding;
            }
        }

        public double RightAscension
        {
            get
            {
                //SharedResources.TrafficStart(SharedResources.MessageType.Gets, "Right Ascension: ");
                //SharedResources.TrafficEnd(utilities.HoursToHMS(TelescopeHardware.RightAscension));
                return TelescopeHardware.RightAscension;
            }
        }

        public double RightAscensionRate
        {
            get
            {
                double rightAscensionRate = 0.0;
                LogMessage("RightAscensionRate", "Get - " + rightAscensionRate.ToString());
                return rightAscensionRate;
            }
            set
            {
                LogMessage("RightAscensionRate Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("RightAscensionRate", true);
            }
        }

        public PierSide SideOfPier
        {
            get
            {
                return TelescopeHardware.SideOfPier;
            }
            set
            {
                LogMessage("SideOfPier Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SideOfPier", true);
            }
        }

        public double SiderealTime
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

                LogMessage("SiderealTime", "Get - " + siderealTime.ToString());
                return siderealTime;
            }
        }

        public double SiteElevation
        {
            get
            {
                //SharedResources.TrafficStart(SharedResources.MessageType.Other, "SiteElevation: ");
                //CheckCapability(TelescopeHardware.CanLatLongElev, "SiteElevation", false);
                //SharedResources.TrafficEnd(TelescopeHardware.Elevation.ToString(CultureInfo.InvariantCulture));
                return TelescopeHardware.Elevation;
            }
            set
            {
                //SharedResources.TrafficLine(SharedResources.MessageType.Other, "SiteElevation: ->");
                //CheckCapability(TelescopeHardware.CanLatLongElev, "SiteElevation", true);
                CheckRange(value, -300, 10000, "SiteElevation");
                //SharedResources.TrafficEnd(value.ToString(CultureInfo.InvariantCulture));
                TelescopeHardware.Elevation = value;
            }
        }

        public double SiteLatitude
        {
            get
            {
                //SharedResources.TrafficStart(SharedResources.MessageType.Other, "SiteLatitude: ");
                //CheckCapability(TelescopeHardware.CanLatLongElev, "SiteLatitude", false);
                //SharedResources.TrafficEnd(TelescopeHardware.SiteLatitude.ToString(CultureInfo.InvariantCulture));
                return TelescopeHardware.SiteLatitude;
            }
            set
            {
                //SharedResources.TrafficLine(SharedResources.MessageType.Other, "SiteLatitude: ->");
                //CheckCapability(TelescopeHardware.CanLatLongElev, "SiteLatitude", true);
                CheckRange(value, -90, 90, "SiteLatitude");
                //SharedResources.TrafficEnd(value.ToString(CultureInfo.InvariantCulture));
                TelescopeHardware.SiteLatitude = value;
            }
        }

        public double SiteLongitude
        {
            get
            {
                //SharedResources.TrafficStart(SharedResources.MessageType.Other, "SiteLongitude: ");
                //CheckCapability(TelescopeHardware.CanLatLongElev, "SiteLongitude", false);
                //SharedResources.TrafficEnd(TelescopeHardware.Longitude.ToString(CultureInfo.InvariantCulture));
                return TelescopeHardware.SiteLongitude;
            }
            set
            {
                //SharedResources.TrafficStart(SharedResources.MessageType.Other, "SiteLongitude: ->");
                //CheckCapability(TelescopeHardware.CanLatLongElev, "SiteLongitude", true);
                CheckRange(value, -180, 180, "SiteLongitude");
                //SharedResources.TrafficEnd(value.ToString(CultureInfo.InvariantCulture));
                TelescopeHardware.SiteLongitude = value;
            }
        }

        public bool Slewing
        {
            get
            {
                return TelescopeHardware.Slewing || internalSlewing;
            }
        }

        public short SlewSettleTime
        {
            get
            {
                LogMessage("SlewSettleTime Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SlewSettleTime", false);
            }
            set
            {
                LogMessage("SlewSettleTime Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SlewSettleTime", true);
            }
        }

        public double TargetDeclination
        {
            get
            {
                //SharedResources.TrafficStart(SharedResources.MessageType.Gets, "TargetDeclination: ");
                //CheckCapability(TelescopeHardware.CanSlew, "TargetDeclination", false);
                CheckRange(TelescopeHardware.TargetDeclination, -90, 90, "TargetDeclination");
                //SharedResources.TrafficEnd(utilities.DegreesToDMS(TelescopeHardware.TargetDeclination));}
                if (targetDeclination == SharedResources.INVALID_DOUBLE)
                    throw new ASCOM.ValueNotSetException("TargetDeclination");
                return targetDeclination;
            }
            set
            {
                //SharedResources.TrafficStart(SharedResources.MessageType.Gets, "TargetDeclination:-> ");
                //CheckCapability(TelescopeHardware.CanSlew, "TargetDeclination", true);
                CheckRange(value, -90, 90, "TargetDeclination");
                //SharedResources.TrafficEnd(utilities.DegreesToDMS(value));
                targetDeclination = value;
            }
        }

        public double TargetRightAscension
        {
            get
            {
                //SharedResources.TrafficStart(SharedResources.MessageType.Gets, "TargetRightAscension: ");
                if (targetRightAscension == SharedResources.INVALID_DOUBLE)
                    throw new ASCOM.ValueNotSetException("TargetRightAscension");
                return targetRightAscension;
            }
            set
            {
                //SharedResources.TrafficStart(SharedResources.MessageType.Gets, "TargetRightAscension:-> ");
                CheckRange(value, 0, 24, "TargetRightAscension");
                //SharedResources.TrafficEnd(utilities.DegreesToHMS(value));
                targetRightAscension = value;
            }
        }

        public bool Tracking
        {
            get
            {
                //SharedResources.TrafficLine(SharedResources.MessageType.Polls, "Tracking: " + TelescopeHardware.Tracking.ToString());
                return TelescopeHardware.Tracking;
            }
            set
            {
                //SharedResources.TrafficLine(SharedResources.MessageType.Polls, "Tracking:-> " + value.ToString());
                TelescopeHardware.SetTracking = value;
            }
        }

        public DriveRates TrackingRate
        {
            get
            {
                return TelescopeHardware.TrackingRate;
            }
            set
            {
                TelescopeHardware.TrackingRate = value;
            }
        }

        public ITrackingRates TrackingRates
        {
            get
            {
                ITrackingRates trackingRates = new TrackingRates();
                LogMessage("TrackingRates", "Get - ");
                foreach (DriveRates driveRate in trackingRates)
                {
                    LogMessage("TrackingRates", "Get - " + driveRate.ToString());
                }
                return trackingRates;
            }
        }

        public DateTime UTCDate
        {
            get
            {
                DateTime utcDate = DateTime.UtcNow;
                LogMessage("UTCDate", "Get - " + String.Format("MM/dd/yy HH:mm:ss", utcDate));
                return utcDate;
            }
            set
            {
                LogMessage("UTCDate Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("UTCDate", true);
            }
        }

        public void AbortSlew()
        {
            //SharedResources.TrafficStart(SharedResources.MessageType.Slew, "AbortSlew: ");
            internalSlewing = false;
            TelescopeHardware.AbortSlew();
            //SharedResources.TrafficEnd("(done)");
        }

        public IAxisRates AxisRates(TelescopeAxes Axis)
        {
            LogMessage("AxisRates", "Get - " + Axis.ToString());
            return new AxisRates(Axis);
        }

        public bool CanMoveAxis(TelescopeAxes Axis) // ++ axisPrimary true, axisSecondary true
        {
            LogMessage("CanMoveAxis", "Get - " + Axis.ToString());
            switch (Axis)
            {
                case TelescopeAxes.axisPrimary: return true;
                case TelescopeAxes.axisSecondary: return true;
                case TelescopeAxes.axisTertiary: return false;
                default: throw new InvalidValueException("CanMoveAxis", Axis.ToString(), "0 to 2");
            }
        }

        public PierSide DestinationSideOfPier(double RightAscension, double Declination)
        {
            LogMessage("DestinationSideOfPier Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("DestinationSideOfPier", false);
        }

        public void FindHome()
        {
            //SharedResources.TrafficStart(SharedResources.MessageType.Slew, "FindHome: ");
            //CheckCapability(TelescopeHardware.CanFindHome, "FindHome");
            //CheckParked("FindHome");
            TelescopeHardware.FindHome();
            //SharedResources.TrafficStart(SharedResources.MessageType.Slew, "(done)");
        }

        public void MoveAxis(TelescopeAxes Axis, double Rate)
        {
            CheckRate(Axis, Rate);
            internalSlewing = (Rate != 0 ? true : false);
            TelescopeHardware.MoveAxis(Axis, Rate);
        }

        public void Park()
        {
            LogMessage("Park", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("Park");
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            TelescopeHardware.PulseGuide(Direction, Duration);
        }

        public void SetPark()
        {
            //SharedResources.TrafficStart(SharedResources.MessageType.Other, "Set Park: ");
            //CheckCapability(TelescopeHardware.CanSetPark, "SetPark");
            TelescopeHardware.ParkAltitude = TelescopeHardware.Altitude;
            TelescopeHardware.ParkAzimuth = TelescopeHardware.Azimuth;
            //SharedResources.TrafficEnd("(done)");
        }

        public void SlewToAltAz(double Azimuth, double Altitude)
        {
            LogMessage("SlewToAltAz", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("SlewToAltAz");
        }

        public void SlewToAltAzAsync(double Azimuth, double Altitude)
        {
            LogMessage("SlewToAltAzAsync", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("SlewToAltAzAsync");
        }

        public void SlewToCoordinates(double RightAscension, double Declination)
        {
            LogMessage("SlewToCoordinates", "RightAscension " + RightAscension);
            LogMessage("SlewToCoordinates", "Declincation " + Declination);
            CheckRange(RightAscension, 0, 24, "SlewToCoordinates", "RightAscension");
            CheckRange(Declination, -90, 90, "SlewToCoordinates", "Declination");
            targetRightAscension = RightAscension;
            targetDeclination = Declination;
            TelescopeHardware.SlewToCoordinates(RightAscension, Declination);
        }

        public void SlewToCoordinatesAsync(double RightAscension, double Declination)
        {
            LogMessage("SlewToCoordinatesAsync", "RightAscension " + RightAscension);
            LogMessage("SlewToCoordinatesAsync", "Declincation " + Declination);
            CheckRange(RightAscension, 0, 24, "SlewToCoordinates", "RightAscension");
            CheckRange(Declination, -90, 90, "SlewToCoordinates", "Declination");
            targetRightAscension = RightAscension;
            targetDeclination = Declination;
            TelescopeHardware.SlewToCoordinatesAsync(RightAscension, Declination);
        }

        public void SlewToTarget()
        {
            TelescopeHardware.TargetRightAscension = targetRightAscension;
            TelescopeHardware.TargetDeclination = targetDeclination;
            TelescopeHardware.SlewToTarget();

        }

        public void SlewToTargetAsync()
        {
            TelescopeHardware.TargetRightAscension = targetRightAscension;
            TelescopeHardware.TargetDeclination = targetDeclination;
            TelescopeHardware.SlewToTargetAsync();
        }

        public void SyncToAltAz(double Azimuth, double Altitude)
        {
            LogMessage("SyncToAltAz", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("SyncToAltAz");
        }

        public void SyncToCoordinates(double RightAscension, double Declination)
        {
            LogMessage("SyncToCoordinates", "RightAscension " + RightAscension);
            LogMessage("SyncToCoordinates", "Declincation " + Declination);
            CheckRange(RightAscension, 0, 24, "SyncToCoordinates", "RightAscension");
            CheckRange(Declination, -90, 90, "SyncToCoordinates", "Declination");
            targetRightAscension = RightAscension;
            targetDeclination = Declination;
            TelescopeHardware.SyncToCoordinates(RightAscension, Declination);
        }

        public void SyncToTarget()
        {
            TelescopeHardware.SyncToCoordinates(targetRightAscension, targetDeclination);
        }

        public void Unpark()
        {
            LogMessage("Unpark", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("Unpark");
        }

        #endregion ITelescope Implementation

        #region private methods
        private void CheckRate(TelescopeAxes axis, double rate)
        {
            IAxisRates rates = AxisRates(axis);
            if (rate == 0 & axis != TelescopeAxes.axisTertiary)
            {
                return;
            }
            string ratesStr = string.Empty;
            foreach (Rate item in rates)
            {
                if (Math.Abs(rate) >= item.Minimum && Math.Abs(rate) <= item.Maximum)
                {
                    return;
                }
                ratesStr = string.Format("{0}, {1} to {2}", ratesStr, item.Minimum, item.Maximum);
            }
            throw new InvalidValueException("MoveAxis", rate.ToString(CultureInfo.InvariantCulture), ratesStr);
        }

        private static void CheckRange(double value, double min, double max, string propertyOrMethod, string valueName)
        {
            if (double.IsNaN(value))
            {
                //SharedResources.TrafficEnd(string.Format(CultureInfo.CurrentCulture, "{0}:{1} value has not been set", propertyOrMethod, valueName));
                throw new ValueNotSetException(propertyOrMethod + ":" + valueName);
            }
            if (value < min || value > max)
            {
                //SharedResources.TrafficEnd(string.Format(CultureInfo.CurrentCulture, "{0}:{4} {1} out of range {2} to {3}", propertyOrMethod, value, min, max, valueName));
                throw new InvalidValueException(propertyOrMethod, value.ToString(CultureInfo.CurrentCulture), string.Format(CultureInfo.CurrentCulture, "{0}, {1} to {2}", valueName, min, max));
            }
        }

        private static void CheckRange(double value, double min, double max, string propertyOrMethod)
        {
            if (double.IsNaN(value))
            {
                //SharedResources.TrafficEnd(string.Format(CultureInfo.CurrentCulture, "{0} value has not been set", propertyOrMethod));
                throw new ValueNotSetException(propertyOrMethod);
            }
            if (value < min || value > max)
            {
                //SharedResources.TrafficEnd(string.Format(CultureInfo.CurrentCulture, "{0} {1} out of range {2} to {3}", propertyOrMethod, value, min, max));
                throw new InvalidValueException(propertyOrMethod, value.ToString(CultureInfo.CurrentCulture), string.Format(CultureInfo.CurrentCulture, "{0} to {1}", min, max));
            }
        }

        //private static void CheckVersionOne(string property, bool accessorSet)
        //{
        //    CheckVersionOne(property);
        //    if (TelescopeHardware.VersionOneOnly)
        //    {
        //        SharedResources.TrafficEnd(property + " invalid in version 1");
        //        throw new PropertyNotImplementedException(property, accessorSet);
        //    }
        //}

        //private static void CheckVersionOne(string property)
        //{
        //    if (TelescopeHardware.VersionOneOnly)
        //    {
        //        SharedResources.TrafficEnd(property + " is not implemented in version 1");
        //        throw new System.NotImplementedException(property);
        //    }
        //}

        //private static void CheckCapability(bool capability, string method)
        //{
        //    if (!capability)
        //    {
        //        SharedResources.TrafficEnd(string.Format(CultureInfo.CurrentCulture, "{0} not implemented in {1}", capability, method));
        //        throw new MethodNotImplementedException(method);
        //    }
        //}

        //private static void CheckCapability(bool capability, string property, bool setNotGet)
        //{
        //    if (!capability)
        //    {
        //        SharedResources.TrafficEnd(string.Format(CultureInfo.CurrentCulture, "{2} {0} not implemented in {1}", capability, property, setNotGet ? "set" : "get"));
        //        throw new PropertyNotImplementedException(property, setNotGet);
        //    }
        //}

        //private static void CheckParked(string property)
        //{
        //    if (TelescopeHardware.AtPark)
        //    {
        //        SharedResources.TrafficEnd(string.Format(CultureInfo.CurrentCulture, "{0} not possible when parked", property));
        //        throw new ParkedException(property);
        //    }
        //}

        /// <summary>
        /// Checks the slew type and tracking state and raises an exception if they don't match.
        /// </summary>
        /// <param name="raDecSlew">if set to <c>true</c> this is a Ra Dec slew if  <c>false</c> an Alt Az slew.</param>
        /// <param name="method">The method name.</param>
        //private static void CheckTracking(bool raDecSlew, string method)
        //{
        //    if (raDecSlew != TelescopeHardware.Tracking)
        //    {
        //        SharedResources.TrafficEnd(string.Format(CultureInfo.CurrentCulture, "{0} not possible when tracking is {1}", method, TelescopeHardware.Tracking));
        //        throw new ASCOM.InvalidOperationException(string.Format("{0} is not allowed when tracking is {1}", method, TelescopeHardware.Tracking));
        //    }
        //}

        #endregion


        #region Private properties and methods

        // here are some useful properties and methods that can be used as required
        // to help with driver development

        //#region ASCOM Registration

        //// Register or unregister driver for ASCOM. This is harmless if already
        //// registered or unregistered.
        ////
        ///// <summary>
        ///// Register or unregister the driver with the ASCOM Platform.
        ///// This is harmless if the driver is already registered/unregistered.
        ///// </summary>
        ///// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        //private static void RegUnregASCOM(bool bRegister)
        //{
        //    using (var P = new ASCOM.Utilities.Profile())
        //    {
        //        P.DeviceType = "Telescope";
        //        if (bRegister)
        //        {
        //            P.Register(driverID, driverDescription);
        //        }
        //        else
        //        {
        //            P.Unregister(driverID);
        //        }
        //    }
        //}

        ///// <summary>
        ///// This function registers the driver with the ASCOM Chooser and
        ///// is called automatically whenever this class is registered for COM Interop.
        ///// </summary>
        ///// <param name="t">Type of the class being registered, not used.</param>
        ///// <remarks>
        ///// This method typically runs in two distinct situations:
        ///// <list type="numbered">
        ///// <item>
        ///// In Visual Studio, when the project is successfully built.
        ///// For this to work correctly, the option <c>Register for COM Interop</c>
        ///// must be enabled in the project settings.
        ///// </item>
        ///// <item>During setup, when the installer registers the assembly for COM Interop.</item>
        ///// </list>
        ///// This technique should mean that it is never necessary to manually register a driver with ASCOM.
        ///// </remarks>
        //[ComRegisterFunction]
        //public static void RegisterASCOM(Type t)
        //{
        //    RegUnregASCOM(true);
        //}

        ///// <summary>
        ///// This function unregisters the driver from the ASCOM Chooser and
        ///// is called automatically whenever this class is unregistered from COM Interop.
        ///// </summary>
        ///// <param name="t">Type of the class being registered, not used.</param>
        ///// <remarks>
        ///// This method typically runs in two distinct situations:
        ///// <list type="numbered">
        ///// <item>
        ///// In Visual Studio, when the project is cleaned or prior to rebuilding.
        ///// For this to work correctly, the option <c>Register for COM Interop</c>
        ///// must be enabled in the project settings.
        ///// </item>
        ///// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
        ///// </list>
        ///// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
        ///// </remarks>
        //[ComUnregisterFunction]
        //public static void UnregisterASCOM(Type t)
        //{
        //    RegUnregASCOM(false);
        //}

        //#endregion

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool IsConnected
        {
            get
            {
                // TODO check that the driver hardware connection exists and is connected to the hardware
                return connectedState;
            }
        }

        //public static string comPort { get; internal set; }

        /// <summary>
        /// Log helper function that takes formatted strings and arguments
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal static void LogMessage(string identifier, string message, params object[] args)
        {
            var msg = string.Format(message, args);
            tl.LogMessage(identifier, msg);
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        //internal void ReadProfile()
        //{
        //    using (Profile driverProfile = new Profile())
        //    {
        //        driverProfile.DeviceType = "Telescope";
        //        tl.Enabled = Convert.ToBoolean(driverProfile.GetValue(driverID, traceStateProfileName, string.Empty, traceStateDefault));
        //        comPort = driverProfile.GetValue(driverID, comPortProfileName, string.Empty, comPortDefault);
        //    }
        //}

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        //internal void WriteProfile()
        //{
        //    using (Profile driverProfile = new Profile())
        //    {
        //        driverProfile.DeviceType = "Telescope";
        //        driverProfile.WriteValue(driverID, traceStateProfileName, tl.Enabled.ToString());
        //        driverProfile.WriteValue(driverID, comPortProfileName, comPort.ToString());
        //    }
        //}

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new ASCOM.NotConnectedException(message);
            }
        }

        #endregion Private properties and methods
    }
}