using ASCOM.DeviceInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.iOptronZEQ25
{
    class TelescopeTemplate : ITelescopeV3
    {
        public bool Connected { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public string Description => throw new System.NotImplementedException();

        public string DriverInfo => throw new System.NotImplementedException();

        public string DriverVersion => throw new System.NotImplementedException();

        public short InterfaceVersion => throw new System.NotImplementedException();

        public string Name => throw new System.NotImplementedException();

        public ArrayList SupportedActions => throw new System.NotImplementedException();

        public AlignmentModes AlignmentMode => throw new System.NotImplementedException();

        public double Altitude => throw new System.NotImplementedException();

        public double ApertureArea => throw new System.NotImplementedException();

        public double ApertureDiameter => throw new System.NotImplementedException();

        public bool AtHome => throw new System.NotImplementedException();

        public bool AtPark => throw new System.NotImplementedException();

        public double Azimuth => throw new System.NotImplementedException();

        public bool CanFindHome => throw new System.NotImplementedException();

        public bool CanPark => throw new System.NotImplementedException();

        public bool CanPulseGuide => throw new System.NotImplementedException();

        public bool CanSetDeclinationRate => throw new System.NotImplementedException();

        public bool CanSetGuideRates => throw new System.NotImplementedException();

        public bool CanSetPark => throw new System.NotImplementedException();

        public bool CanSetPierSide => throw new System.NotImplementedException();

        public bool CanSetRightAscensionRate => throw new System.NotImplementedException();

        public bool CanSetTracking => throw new System.NotImplementedException();

        public bool CanSlew => throw new System.NotImplementedException();

        public bool CanSlewAltAz => throw new System.NotImplementedException();

        public bool CanSlewAltAzAsync => throw new System.NotImplementedException();

        public bool CanSlewAsync => throw new System.NotImplementedException();

        public bool CanSync => throw new System.NotImplementedException();

        public bool CanSyncAltAz => throw new System.NotImplementedException();

        public bool CanUnpark => throw new System.NotImplementedException();

        public double Declination => throw new System.NotImplementedException();

        public double DeclinationRate { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public bool DoesRefraction { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public EquatorialCoordinateType EquatorialSystem => throw new System.NotImplementedException();

        public double FocalLength => throw new System.NotImplementedException();

        public double GuideRateDeclination { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public double GuideRateRightAscension { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public bool IsPulseGuiding => throw new System.NotImplementedException();

        public double RightAscension => throw new System.NotImplementedException();

        public double RightAscensionRate { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public PierSide SideOfPier { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public double SiderealTime => throw new System.NotImplementedException();

        public double SiteElevation { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public double SiteLatitude { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public double SiteLongitude { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public bool Slewing => throw new System.NotImplementedException();

        public short SlewSettleTime { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public double TargetDeclination { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public double TargetRightAscension { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public bool Tracking { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public DriveRates TrackingRate { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public ITrackingRates TrackingRates => throw new System.NotImplementedException();

        public DateTime UTCDate { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void AbortSlew()
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        public void MoveAxis(TelescopeAxes Axis, double Rate)
        {
            throw new System.NotImplementedException();
        }

        public void Park()
        {
            throw new System.NotImplementedException();
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        public void SlewToCoordinatesAsync(double RightAscension, double Declination)
        {
            throw new System.NotImplementedException();
        }

        public void SlewToTarget()
        {
            throw new System.NotImplementedException();
        }

        public void SlewToTargetAsync()
        {
            throw new System.NotImplementedException();
        }

        public void SyncToAltAz(double Azimuth, double Altitude)
        {
            throw new System.NotImplementedException();
        }

        public void SyncToCoordinates(double RightAscension, double Declination)
        {
            throw new System.NotImplementedException();
        }

        public void SyncToTarget()
        {
            throw new System.NotImplementedException();
        }

        public void Unpark()
        {
            throw new System.NotImplementedException();
        }
    }
}
