using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceService;

namespace OnvifCoreFormsApp
{
    public class DeviceConfigurator
    {
        private void setDeviceGateway(DeviceClient device, string defaultGatewayIp, bool ipv6 = false)
        {
            // virgül ile ayrılmış adresleri ayırarak IPv4'e dönüştürür.
            string[] temp = { defaultGatewayIp };
            if (ipv6)
            {
                device.SetNetworkDefaultGateway(null, temp);
            }
            else
            {
                device.SetNetworkDefaultGateway(temp, null);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dns">New DNS addresses seperated with a comma.
        /// ex: "8.8.8.8,8.8.4.4"</param>
        private void SetDeviceDNS(DeviceClient device, string dns)
        {
            string[] addresses = dns.Split(",");
            List<DeviceService.IPAddress> dnsIps = new();
            foreach (string address in addresses)
            {
                var dnsIp = new DeviceService.IPAddress();
                dnsIp.Type = DeviceService.IPType.IPv4;
                dnsIp.IPv4Address = address.Trim();
                dnsIps.Add(dnsIp);
            }
            if (dnsIps.Count > 0)
            {
                device.SetDNS(false, null, dnsIps.ToArray());
            }
        }
        /// <summary>
        /// Set the IP of the currently logged in device. Returns true
        /// if a reboot is needed.
        /// </summary>
        /// <param name="device">Current device object</param>
        /// <param name="ip">The new IP address. Must be Local IP (ex. 192.168.1.108)</param>
        /// <param name="useDhcp">Set this to true if you want the IP to be set by DHCP.</param>
        /// <returns></returns>
        /// <exception cref="InvalidDataException"></exception>
        private bool SetDeviceIpv4(DeviceClient device, string ip, bool useDhcp = false)
        {
            if (useDhcp)
            {
                return false;
            }
            else if (ip.Length > 1)
            {
                DeviceService.NetworkInterface[] networkInterfaces = device.GetNetworkInterfaces();
                var enInterface = networkInterfaces.FirstOrDefault(x => x.Enabled);
                var netConf = new NetworkInterfaceSetConfiguration();
                netConf.Enabled = true;
                netConf.EnabledSpecified = true;
                netConf.MTUSpecified = (enInterface.Info != null) && (enInterface.Info.MTUSpecified);
                if (netConf.MTUSpecified)
                {
                    netConf.MTU = enInterface.Info!.MTU;
                }
                netConf.IPv4 = new IPv4NetworkInterfaceSetConfiguration();
                netConf.IPv4.DHCP = false;
                netConf.IPv4.DHCPSpecified = true;
                netConf.IPv4.Enabled = true;
                netConf.IPv4.EnabledSpecified = true;
                netConf.IPv4.Manual = new DeviceService.PrefixedIPv4Address[1];
                netConf.IPv4.Manual[0] = new DeviceService.PrefixedIPv4Address();
                netConf.IPv4.Manual[0].Address = ip;
                netConf.IPv4.Manual[0].PrefixLength = enInterface.IPv4.Config.Manual[0].PrefixLength;

                return device.SetNetworkInterfaces(enInterface.token, netConf);
            }
            else
            {
                throw new InvalidDataException("SetDeviceIpv4 parameters are invalid.");
            }
        }
    }
}
