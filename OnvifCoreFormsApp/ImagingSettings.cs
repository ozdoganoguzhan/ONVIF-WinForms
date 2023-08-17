using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImagingService;
using DeviceService;
using System.ServiceModel;

namespace OnvifCoreFormsApp
{
    public class ImagingSettings
    {
  /*      private ImagingOptions20? GetImagingOptions(ImagingPortClient imaging)
        {
            if (imaging == null) return null;

            return imaging.GetOptions(null);
        }
        private ImagingPortClient? GetImagingPort(DeviceClient device, string username, string password)
        {
            if (device.GetServices(false).FirstOrDefault(s => s.Namespace == "http://www.onvif.org/ver20/media/wsdl") != null)
            {
                var imagingPort = new ImagingPortClient(Binding, new EndpointAddress(deviceUri.ToString()));
                imagingPort.ClientCredentials.HttpDigest.ClientCredential.Password = password;
                imagingPort.ClientCredentials.HttpDigest.ClientCredential.UserName = username;
                return imagingPort;
            }
            else return null;
        }*/
    }
}
