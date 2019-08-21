using ASCOM.DeviceInterface;
using System;
using System.Collections;

namespace ASCOM.iOptronZEQ25
{
    public class TelescopeDriver : ReferenceCountedObjectBase, ITelescopeV3
    {
        public AlignmentModes AlignmentMode => throw new System.NotImplementedException();
        public double Altitude => TelescopeHardware.Altitude;
        public double ApertureArea => throw new System.NotImplementedException();
        public double ApertureDiameter => throw new System.NotImplementedException();
        public bool AtHome => TelescopeHardware.AtHome;
        public bool AtPark => throw new System.NotImplementedException();
        public double Azimuth => TelescopeHardware.Azimuth;
        public bool CanFindHome => true;
        public bool CanPark => false;
        public bool CanPulseGuide => true;
        public bool CanSetDeclinationRate => false;
        public bool CanSetGuideRates => true;
        public bool CanSetPark => false;
        public bool CanSetPierSide => false;
        public bool CanSetRightAscensionRate => true;
        public bool CanSetTracking => true;
        public bool CanSlew => true;
        public bool CanSlewAltAz => false;
        public bool CanSlewAltAzAsync => false;
        public bool CanSlewAsync => true;
        public bool CanSync => true;
        public bool CanSyncAltAz => false;
        public bool CanUnpark => false;
        public bool Connected { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public double Declination => TelescopeHardware.Declination;
        public double DeclinationRate { get => 0.0; set => throw new System.NotImplementedException(); }
        public string Description => throw new System.NotImplementedException();
        public bool DoesRefraction { get => false; set => throw new System.NotImplementedException(); }
        public string DriverInfo => throw new System.NotImplementedException();
        public string DriverVersion => throw new System.NotImplementedException();
        public EquatorialCoordinateType EquatorialSystem => (EquatorialCoordinateType)TelescopeHardware.EquatorialSystem;
        public double FocalLength => throw new System.NotImplementedException();
        public double GuideRateDeclination { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public double GuideRateRightAscension { get => TelescopeHardware.GuideRateRightAscension; set => TelescopeHardware.GuideRateRightAscension = value; }
        public short InterfaceVersion => throw new System.NotImplementedException();
        public bool IsPulseGuiding => TelescopeHardware.IsPulseGuiding;
        public string Name => throw new System.NotImplementedException();
        public double RightAscension => TelescopeHardware.RightAscension;
        public double RightAscensionRate { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public PierSide SideOfPier { get => TelescopeHardware.SideOfPier; set => throw new System.NotImplementedException(); }
        public double SiderealTime => throw new System.NotImplementedException();
        public double SiteElevation { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public double SiteLatitude { get => TelescopeHardware.SiteLatitude; set => throw new System.NotImplementedException(); }
        public double SiteLongitude { get => TelescopeHardware.SiteLongitude; set => throw new System.NotImplementedException(); }
        public bool Slewing => throw new System.NotImplementedException();
        public short SlewSettleTime { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public ArrayList SupportedActions => throw new System.NotImplementedException();
        public double TargetDeclination { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public double TargetRightAscension { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public bool Tracking { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public DriveRates TrackingRate { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public ITrackingRates TrackingRates => throw new System.NotImplementedException();

        public DateTime UTCDate { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void AbortSlew()
        {
            TelescopeHardware.AbortSlew();
        }

        public string Action(string ActionName, string ActionParameters)
        {
            throw new System.NotImplementedException();
        }

        public IAxisRates AxisRates(TelescopeAxes Axis)
        {
            throw new System.NotImplementedException();
        }

        public bool CanMoveAxis(TelescopeAxes Axis)
        {
            return TelescopeHardware.CanMoveAxis(Axis);
        }

        public void CommandBlind(string Command, bool Raw = false)
        {
            throw new System.NotImplementedException();
        }

        public bool CommandBool(string Command, bool Raw = false)
        {
            throw new System.NotImplementedException();
        }

        public string CommandString(string Command, bool Raw = false)
        {
            throw new System.NotImplementedException();
        }

        public PierSide DestinationSideOfPier(double RightAscension, double Declination)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void FindHome()
        {
            TelescopeHardware.FindHome();
        }

        public void MoveAxis(TelescopeAxes Axis, double Rate)
        {
            TelescopeHardware.MoveAxis(Axis, Rate);
        }

        public void Park()
        {
            throw new System.NotImplementedException();
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            TelescopeHardware.PulseGuide(Direction, Duration);
        }

        public void SetPark()
        {
            throw new System.NotImplementedException();
        }

        public void SetupDialog()
        {
            throw new System.NotImplementedException();
        }

        public void SlewToAltAz(double Azimuth, double Altitude)
        {
            throw new System.NotImplementedException();
        }

        public void SlewToAltAzAsync(double Azimuth, double Altitude)
        {
            throw new System.NotImplementedException();
        }

        public void SlewToCoordinates(double RightAscension, double Declination)
        {
            TelescopeHardware.SlewToCoordinates(RightAscension, Declination);
        }

        public void SlewToCoordinatesAsync(double RightAscension, double Declination)
        {
            TelescopeHardware.SlewToCoordinatesAsync(RightAscension, Declination);
        }

        public void SlewToTarget()
        {
            TelescopeHardware.SlewToTarget();
        }

        public void SlewToTargetAsync()
        {
            TelescopeHardware.SlewToTargetAsync();
        }

        public void SyncToAltAz(double Azimuth, double Altitude)
        {
            throw new System.NotImplementedException();
        }

        public void SyncToCoordinates(double RightAscension, double Declination)
        {
            TelescopeHardware.SyncToCoordinates(RightAscension, Declination);
        }

        public void SyncToTarget()
        {
            TelescopeHardware.SyncToTarget();
        }

        public void Unpark()
        {
            throw new System.NotImplementedException();
        }
    }
}