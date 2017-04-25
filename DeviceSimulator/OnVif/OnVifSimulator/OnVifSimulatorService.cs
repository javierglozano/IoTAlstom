using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnVifSimulator;

namespace OnVifSimulator
{
    public class OnVifSimulatorService : IOnVifDevice
    {
        private DiscoveryMode _discoveryMode = DiscoveryMode.NonDiscoverable;
        private HostnameInformation _hostName = new HostnameInformation();
        private DeviceCapabilities _deviceCapabilities;
        private GetDeviceInformationResponse _d = new GetDeviceInformationResponse();
        private string _firmwareVersion;
        private string _hwId;
        private string _manufacturer;
        private string _model;
        private string _serialNumber;

        public OnVifSimulatorService ()
        {
            Random r = new Random();
            _hostName.Name = "hostName";
            _firmwareVersion = "FAKE-FIRMWARE v1.0";
            _hwId = String.Format("HWID_{0}", r.Next(10000));
            _manufacturer = "FAKE-MANUFACTURER";
            _model = "FAKE-MODEL-ACME";
            _serialNumber = String.Format("SN-{0}-{1}-{2}", r.Next(100), r.Next(5), r.Next(1000));
        }

        void IOnVifDevice.AddIPAddressFilter(IPAddressFilter IPAddressFilter)
        {
            throw new NotImplementedException();
        }

        AddScopesResponse IOnVifDevice.AddScopes(AddScopesRequest request)
        {
            throw new NotImplementedException();
        }

        CreateCertificateResponse IOnVifDevice.CreateCertificate(CreateCertificateRequest request)
        {
            throw new NotImplementedException();
        }

        void IOnVifDevice.CreateDot1XConfiguration(Dot1XConfiguration Dot1XConfiguration)
        {
            throw new NotImplementedException();
        }

        string IOnVifDevice.CreateStorageConfiguration(StorageConfigurationData StorageConfiguration)
        {
            throw new NotImplementedException();
        }

        CreateUsersResponse IOnVifDevice.CreateUsers(CreateUsersRequest request)
        {
            throw new NotImplementedException();
        }

        DeleteCertificatesResponse IOnVifDevice.DeleteCertificates(DeleteCertificatesRequest request)
        {
            throw new NotImplementedException();
        }

        DeleteDot1XConfigurationResponse IOnVifDevice.DeleteDot1XConfiguration(DeleteDot1XConfigurationRequest request)
        {
            throw new NotImplementedException();
        }

        DeleteGeoLocationResponse IOnVifDevice.DeleteGeoLocation(DeleteGeoLocationRequest request)
        {
            throw new NotImplementedException();
        }

        void IOnVifDevice.DeleteStorageConfiguration(string Token)
        {
            throw new NotImplementedException();
        }

        DeleteUsersResponse IOnVifDevice.DeleteUsers(DeleteUsersRequest request)
        {
            throw new NotImplementedException();
        }

        BinaryData IOnVifDevice.GetAccessPolicy()
        {
            throw new NotImplementedException();
        }

        GetCACertificatesResponse IOnVifDevice.GetCACertificates(GetCACertificatesRequest request)
        {
            throw new NotImplementedException();
        }

        GetCapabilitiesResponse IOnVifDevice.GetCapabilities(GetCapabilitiesRequest request)
        {
            throw new NotImplementedException();
        }

        GetCertificateInformationResponse IOnVifDevice.GetCertificateInformation(GetCertificateInformationRequest request)
        {
            throw new NotImplementedException();
        }

        GetCertificatesResponse IOnVifDevice.GetCertificates(GetCertificatesRequest request)
        {
            throw new NotImplementedException();
        }

        GetCertificatesStatusResponse IOnVifDevice.GetCertificatesStatus(GetCertificatesStatusRequest request)
        {
            throw new NotImplementedException();
        }

        bool IOnVifDevice.GetClientCertificateMode()
        {
            throw new NotImplementedException();
        }

        GetDeviceInformationResponse IOnVifDevice.GetDeviceInformation(GetDeviceInformationRequest request)
        {
            Console.WriteLine("GetDeviceInformation() operation requested");
            return new GetDeviceInformationResponse(_manufacturer, _model, _firmwareVersion, _serialNumber, _hwId);
        }

        DiscoveryMode IOnVifDevice.GetDiscoveryMode()
        {
            Console.WriteLine("GetDiscoveryMode() operation requested!");
            return this._discoveryMode;
        }

        DNSInformation IOnVifDevice.GetDNS()
        {
            throw new NotImplementedException();
        }

        GetDot11CapabilitiesResponse IOnVifDevice.GetDot11Capabilities(GetDot11CapabilitiesRequest request)
        {
            throw new NotImplementedException();
        }

        Dot11Status IOnVifDevice.GetDot11Status(string InterfaceToken)
        {
            throw new NotImplementedException();
        }

        Dot1XConfiguration IOnVifDevice.GetDot1XConfiguration(string Dot1XConfigurationToken)
        {
            throw new NotImplementedException();
        }

        GetDot1XConfigurationsResponse IOnVifDevice.GetDot1XConfigurations(GetDot1XConfigurationsRequest request)
        {
            throw new NotImplementedException();
        }

        GetDPAddressesResponse IOnVifDevice.GetDPAddresses(GetDPAddressesRequest request)
        {
            throw new NotImplementedException();
        }

        DynamicDNSInformation IOnVifDevice.GetDynamicDNS()
        {
            throw new NotImplementedException();
        }

        GetEndpointReferenceResponse IOnVifDevice.GetEndpointReference(GetEndpointReferenceRequest request)
        {
            throw new NotImplementedException();
        }

        GetGeoLocationResponse IOnVifDevice.GetGeoLocation(GetGeoLocationRequest request)
        {
            throw new NotImplementedException();
        }

        HostnameInformation IOnVifDevice.GetHostname()
        {
            Console.WriteLine("GetHostname() operation requested!");
            return this._hostName;
        }

        IPAddressFilter IOnVifDevice.GetIPAddressFilter()
        {
            throw new NotImplementedException();
        }

        NetworkGateway IOnVifDevice.GetNetworkDefaultGateway()
        {
            throw new NotImplementedException();
        }

        GetNetworkInterfacesResponse IOnVifDevice.GetNetworkInterfaces(GetNetworkInterfacesRequest request)
        {
            throw new NotImplementedException();
        }

        GetNetworkProtocolsResponse IOnVifDevice.GetNetworkProtocols(GetNetworkProtocolsRequest request)
        {
            throw new NotImplementedException();
        }

        NTPInformation IOnVifDevice.GetNTP()
        {
            throw new NotImplementedException();
        }

        GetPkcs10RequestResponse IOnVifDevice.GetPkcs10Request(GetPkcs10RequestRequest request)
        {
            throw new NotImplementedException();
        }

        GetRelayOutputsResponse IOnVifDevice.GetRelayOutputs(GetRelayOutputsRequest request)
        {
            throw new NotImplementedException();
        }

        DiscoveryMode IOnVifDevice.GetRemoteDiscoveryMode()
        {
            throw new NotImplementedException();
        }

        RemoteUser IOnVifDevice.GetRemoteUser()
        {
            throw new NotImplementedException();
        }

        GetScopesResponse IOnVifDevice.GetScopes(GetScopesRequest request)
        {
            throw new NotImplementedException();
        }

        DeviceServiceCapabilities IOnVifDevice.GetServiceCapabilities()
        {
            throw new NotImplementedException();
        }

        GetServicesResponse IOnVifDevice.GetServices(GetServicesRequest request)
        {
            throw new NotImplementedException();
        }

        StorageConfiguration IOnVifDevice.GetStorageConfiguration(string Token)
        {
            throw new NotImplementedException();
        }

        GetStorageConfigurationsResponse IOnVifDevice.GetStorageConfigurations(GetStorageConfigurationsRequest request)
        {
            throw new NotImplementedException();
        }

        GetSystemBackupResponse IOnVifDevice.GetSystemBackup(GetSystemBackupRequest request)
        {
            throw new NotImplementedException();
        }

        SystemDateTime IOnVifDevice.GetSystemDateAndTime()
        {
            throw new NotImplementedException();
        }

        SystemLog IOnVifDevice.GetSystemLog(SystemLogType LogType)
        {
            throw new NotImplementedException();
        }

        SupportInformation IOnVifDevice.GetSystemSupportInformation()
        {
            throw new NotImplementedException();
        }

        GetSystemUrisResponse IOnVifDevice.GetSystemUris(GetSystemUrisRequest request)
        {
            throw new NotImplementedException();
        }

        GetUsersResponse IOnVifDevice.GetUsers(GetUsersRequest request)
        {
            throw new NotImplementedException();
        }

        GetWsdlUrlResponse IOnVifDevice.GetWsdlUrl(GetWsdlUrlRequest request)
        {
            Console.WriteLine("GetWsdlUrl operation requested!");
            GetWsdlUrlResponse res = new GetWsdlUrlResponse();
            res.WsdlUrl = "http://172.0.1.213:5357/OnVifSimulatorService";
            return res;
        }

        NetworkZeroConfiguration IOnVifDevice.GetZeroConfiguration()
        {
            throw new NotImplementedException();
        }

        LoadCACertificatesResponse IOnVifDevice.LoadCACertificates(LoadCACertificatesRequest request)
        {
            throw new NotImplementedException();
        }

        LoadCertificatesResponse IOnVifDevice.LoadCertificates(LoadCertificatesRequest request)
        {
            throw new NotImplementedException();
        }

        LoadCertificateWithPrivateKeyResponse IOnVifDevice.LoadCertificateWithPrivateKey(LoadCertificateWithPrivateKeyRequest request)
        {
            throw new NotImplementedException();
        }

        void IOnVifDevice.RemoveIPAddressFilter(IPAddressFilter IPAddressFilter)
        {
            throw new NotImplementedException();
        }

        RemoveScopesResponse IOnVifDevice.RemoveScopes(RemoveScopesRequest request)
        {
            throw new NotImplementedException();
        }

        RestoreSystemResponse IOnVifDevice.RestoreSystem(RestoreSystemRequest request)
        {
            throw new NotImplementedException();
        }

        ScanAvailableDot11NetworksResponse IOnVifDevice.ScanAvailableDot11Networks(ScanAvailableDot11NetworksRequest request)
        {
            throw new NotImplementedException();
        }

        string IOnVifDevice.SendAuxiliaryCommand(string AuxiliaryCommand)
        {
            throw new NotImplementedException();
        }

        void IOnVifDevice.SetAccessPolicy(BinaryData PolicyFile)
        {
            throw new NotImplementedException();
        }

        SetCertificatesStatusResponse IOnVifDevice.SetCertificatesStatus(SetCertificatesStatusRequest request)
        {
            throw new NotImplementedException();
        }

        void IOnVifDevice.SetClientCertificateMode(bool Enabled)
        {
            throw new NotImplementedException();
        }

        void IOnVifDevice.SetDiscoveryMode(DiscoveryMode DiscoveryMode)
        {
            Console.WriteLine("SetDiscoveryMode operation requested!");
            this._discoveryMode = DiscoveryMode;
        }

        SetDNSResponse IOnVifDevice.SetDNS(SetDNSRequest request)
        {
            throw new NotImplementedException();
        }

        void IOnVifDevice.SetDot1XConfiguration(Dot1XConfiguration Dot1XConfiguration)
        {
            throw new NotImplementedException();
        }

        SetDPAddressesResponse IOnVifDevice.SetDPAddresses(SetDPAddressesRequest request)
        {
            throw new NotImplementedException();
        }

        SetDynamicDNSResponse IOnVifDevice.SetDynamicDNS(SetDynamicDNSRequest request)
        {
            throw new NotImplementedException();
        }

        SetGeoLocationResponse IOnVifDevice.SetGeoLocation(SetGeoLocationRequest request)
        {
            throw new NotImplementedException();
        }

        SetHostnameResponse IOnVifDevice.SetHostname(SetHostnameRequest request)
        {
            Console.WriteLine("SetHostname operation requested!");
            _hostName.Name = request.Name;
            SetHostnameResponse res = new SetHostnameResponse();
            return res;
        }

        bool IOnVifDevice.SetHostnameFromDHCP(bool FromDHCP)
        {
            throw new NotImplementedException();
        }

        void IOnVifDevice.SetIPAddressFilter(IPAddressFilter IPAddressFilter)
        {
            throw new NotImplementedException();
        }

        SetNetworkDefaultGatewayResponse IOnVifDevice.SetNetworkDefaultGateway(SetNetworkDefaultGatewayRequest request)
        {
            throw new NotImplementedException();
        }

        bool IOnVifDevice.SetNetworkInterfaces(string InterfaceToken, NetworkInterfaceSetConfiguration NetworkInterface)
        {
            throw new NotImplementedException();
        }

        SetNetworkProtocolsResponse IOnVifDevice.SetNetworkProtocols(SetNetworkProtocolsRequest request)
        {
            throw new NotImplementedException();
        }

        SetNTPResponse IOnVifDevice.SetNTP(SetNTPRequest request)
        {
            throw new NotImplementedException();
        }

        void IOnVifDevice.SetRelayOutputSettings(string RelayOutputToken, RelayOutputSettings Properties)
        {
            throw new NotImplementedException();
        }

        void IOnVifDevice.SetRelayOutputState(string RelayOutputToken, RelayLogicalState LogicalState)
        {
            throw new NotImplementedException();
        }

        void IOnVifDevice.SetRemoteDiscoveryMode(DiscoveryMode RemoteDiscoveryMode)
        {
            throw new NotImplementedException();
        }

        void IOnVifDevice.SetRemoteUser(RemoteUser RemoteUser)
        {
            throw new NotImplementedException();
        }

        SetScopesResponse IOnVifDevice.SetScopes(SetScopesRequest request)
        {
            throw new NotImplementedException();
        }

        void IOnVifDevice.SetStorageConfiguration(StorageConfiguration StorageConfiguration)
        {
            throw new NotImplementedException();
        }

        void IOnVifDevice.SetSystemDateAndTime(SetDateTimeType DateTimeType, bool DaylightSavings, TimeZone TimeZone, DateTime UTCDateTime)
        {
            throw new NotImplementedException();
        }

        void IOnVifDevice.SetSystemFactoryDefault(FactoryDefaultType FactoryDefault)
        {
            throw new NotImplementedException();
        }

        SetUserResponse IOnVifDevice.SetUser(SetUserRequest request)
        {
            throw new NotImplementedException();
        }

        void IOnVifDevice.SetZeroConfiguration(string InterfaceToken, bool Enabled)
        {
            throw new NotImplementedException();
        }

        StartFirmwareUpgradeResponse IOnVifDevice.StartFirmwareUpgrade(StartFirmwareUpgradeRequest request)
        {
            throw new NotImplementedException();
        }

        StartSystemRestoreResponse IOnVifDevice.StartSystemRestore(StartSystemRestoreRequest request)
        {
            throw new NotImplementedException();
        }

        string IOnVifDevice.SystemReboot()
        {
            throw new NotImplementedException();
        }

        string IOnVifDevice.UpgradeSystemFirmware(AttachmentData Firmware)
        {
            throw new NotImplementedException();
        }
    }
}