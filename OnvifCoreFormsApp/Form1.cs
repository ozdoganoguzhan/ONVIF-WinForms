/*
 * A little forewarning before you start reading this code:
 * This was my first time working with ONVIF, so I was experimenting a lot.
 * However this doesn't mean that it will do any harm to your camera, so feel free to use it,
 * I just wanted to clarify the reason as to why this code is so messy.
 */


using System.ServiceModel.Channels;
using System.Text;
using System.ServiceModel;
using DeviceService;
using Media2Service;
using AnalyticsService;
using System.ServiceModel.Security;

namespace OnvifCoreFormsApp
{
    public partial class Form1 : Form
    {
        UriBuilder deviceUri;
        MediaProfile[] profiles;
        DeviceClient device;
        Service[] deviceServices;
        Media2Client media;
        private string camIP;
        private string camUsername;
        private string camPassword;

        public Form1()
        {
            InitializeComponent();

        }

        private async void loginButton_Click(object sender, EventArgs e)
        {
            if (usernameTextBox.Text.Length == 0 || passwordTextBox.Text.Length == 0 || ipTextBox.Text.Length == 0)
            {
                MessageBox.Show("Please enter all fields");
                return;
            }
            camUsername = usernameTextBox.Text;
            camPassword = passwordTextBox.Text;
            camIP = ipTextBox.Text;
            device = LoginToDevice(camIP, camUsername, camPassword);
            deviceServices = device.GetServices(false);

            if (deviceServices != null)
            {
                foreach (Service service in deviceServices)
                {
                    listBox2.Items.Add(service.Namespace);
                }
            }

            Media2Client? media = GetDeviceMedia(device, camUsername, camPassword);
            if (media != null)
            {
                this.media = media;
                var profilesSupported = media.GetServiceCapabilities()
                    .ProfileCapabilities.ConfigurationsSupported;
                profiles = media.GetProfiles(null, profilesSupported);
                if (profiles != null)
                {
                    foreach (var p in profiles)
                    {
                        listBox1.Items.Add(p.Name);
                    }

                    var options = media.GetVideoEncoderConfigurationOptions(null, profiles[0].token);
                    foreach (var option in options)
                    {
                        codecBox.Items.Add(option.Encoding);
                        for (float i = option.QualityRange.Min; i <= option.QualityRange.Max; i++)
                        {
                            qualityBox.Items.Add(option.Encoding + ": " + i);
                        }
                        foreach (var resOpt in option.ResolutionsAvailable)
                        {
                            resolutionBox.Items.Add(option.Encoding + ": " + resOpt.Width + "x" + resOpt.Height);
                        }
                    }

//                    This part is for analytics configuration. Uncomment it if your camera supports analytics.
 /*                    I know I could've used GetDeviceCapabilities, but I felt too lazy to do it because this app
 *                    is only for testing purposes. Maybe I'll add it later.
 *                    
 *                    var analytic = profiles[0].Configurations.Analytics;
                    listBox2.Items.Add(analytic.Name);

                    foreach (var conf in analytic.AnalyticsEngineConfiguration.AnalyticsModule)
                    {
                        listBox2.Items.Add(conf.Name);
                        listBox2.Items.Add(conf.Type);
                        foreach (var item in conf.Parameters.SimpleItem)
                        {
                            listBox2.Items.Add(item.Name);
                            listBox2.Items.Add(item.Value);
                        }
                        foreach (var item in conf.Parameters.ElementItem)
                        {
                            listBox2.Items.Add(item.Name);
                        }
                    }

                    foreach (var conf in analytic.RuleEngineConfiguration.Rule)
                    {
                        listBox2.Items.Add(conf.Name);
                        listBox2.Items.Add(conf.Type);
                        listBox2.Items.Add(conf.Parameters);

                        if (conf.Parameters.SimpleItem != null)
                        {
                            foreach (var item in conf.Parameters.SimpleItem)
                            {
                                if (item.Value != null)
                                {
                                    listBox2.Items.Add(item.Name);
                                    listBox2.Items.Add(item.Value);
                                }
                            }
                        }
                    }*/
                    RuleEnginePortClient? ruleClient = GetRuleEnginePortClient(device, camUsername, camPassword);
                    var videoAnalytics = media.GetAnalyticsConfigurations(null, null);
                    foreach (var conf in videoAnalytics)
                    {
                        foreach (var rule in conf.RuleEngineConfiguration.Rule)
                        {
                            if (rule.Name == "MyMotionDetectorRule" || rule.Name == "Region1")
                            {
                                foreach (var item in rule.Parameters.SimpleItem)
                                {
                                    listBox2.Items.Add("ANALYTICS: " + item.Name);
                                    if (item.Name == "ActiveCells")
                                    {
                                        var temp = Convert.FromBase64String(item.Value);
                                        listBox2.Items.Add("ENCODED: " + Convert.ToHexString(temp));
                                        byte[] decodedArray = PackBitsDecode(temp);
                                        listBox2.Items.Add("DECODED: " + Convert.ToHexString(decodedArray));
                                    }
                                    else listBox2.Items.Add("ANALYTICS: " + item.Value);
                                }

                            }
                        }
                    }
//                    These are for Media service, NOT Media2. Uncomment if you want to use Media service.
 /*                    
 *                    HttpTransportBindingElement httpTransport = new HttpTransportBindingElement();
                    httpTransport.AuthenticationScheme = System.Net.AuthenticationSchemes.Digest;

                    var binding = new CustomBinding(new TextMessageEncodingBindingElement(MessageVersion.Soap12WSAddressing10,
                        Encoding.UTF8), httpTransport);
                    var media1 = new MediaClient(binding, new EndpointAddress(deviceUri.ToString()));
                    media1.ClientCredentials.UserName.UserName = camUsername;
                    media1.ClientCredentials.UserName.Password = camPassword;
                    media1.ClientCredentials.HttpDigest.ClientCredential.UserName = camUsername;
                    media1.ClientCredentials.HttpDigest.ClientCredential.Password = camPassword;
                    var media1config = media1.GetVideoAnalyticsConfigurations();

                    foreach (var conf in media1config)
                    {
                        foreach (var rule in conf.RuleEngineConfiguration.Rule)
                        {
                            foreach (var item in rule.Parameters.SimpleItem)
                            {
                                listBox2.Items.Add("NAME: " + item.Name);
                                listBox2.Items.Add("VALUE: " + item.Value);
                            }

                        }

                    }*/


                    var videoRules = ruleClient.GetRules(videoAnalytics[0].token);

                    foreach (var videoRule in videoRules)
                    {
                        listBox2.Items.Add("TYPE: " + videoRule.Type);


                    }
                    var cellMotionRule = videoRules.FirstOrDefault(x => x.Type.ToString() ==
                    "http://www.onvif.org/ver10/schema:CellMotionDetector");
                    if (cellMotionRule != null)
                    {
                        foreach (var param in cellMotionRule.Parameters.SimpleItem)
                        {
                            if (param.Name == "ActiveCells")
                            {
                                param.Value = Convert.ToBase64String(Convert.FromHexString("cf00"));
                                ruleClient.ModifyRules(videoAnalytics[0].token, videoRules);
                            }
                        }
                    }

                    /*                    This part is for event subscription. It was just an experiment, so it's not finished.
                     *                    
                     *                    HttpTransportBindingElement httpTransport = new HttpTransportBindingElement();
                                        httpTransport.AuthenticationScheme = System.Net.AuthenticationSchemes.Digest;

                                        var binding = new CustomBinding(new TextMessageEncodingBindingElement(MessageVersion.Soap12WSAddressing10,
                                            Encoding.UTF8), httpTransport);
                                        var eventPort = new EventPortTypeClient(binding, new EndpointAddress(deviceUri.ToString()));
                                        eventPort.ClientCredentials.UserName.UserName = camUsername;
                                        eventPort.ClientCredentials.UserName.Password = camPassword;
                                        eventPort.ClientCredentials.HttpDigest.ClientCredential.UserName = camUsername;
                                        eventPort.ClientCredentials.HttpDigest.ClientCredential.Password = camPassword;
                                        GetEventPropertiesResponse eventProps = await GetEventProperties(eventPort);
                                        CreatePullPointClient client = new(binding, new EndpointAddress(deviceUri.ToString()));*/

                }
            }
        }

/*        public async Task<GetEventPropertiesResponse> GetEventProperties(EventPortTypeClient eventPort)
        {
            return await eventPort.GetEventPropertiesAsync(null);
        }*/

        // I don't remember why I made these static, although I think it was because I first implemented these functions in another class.
        public static byte[]? PackBitsEncode(byte[] decodedArray)
        {
            List<byte> encodedList = new();
            byte prevDb = 128;
            byte reps = 1;
            byte cpy = 1;
            List<byte> cpyList = new();

            for (int i = 0; i < decodedArray.Length; i++)
            {
                if (decodedArray[i] == decodedArray[i + 1])
                {
                    reps++;
                    continue;
                }
                if (reps >= 2)
                {
                    var temp = decodedArray[i];
                    encodedList.Add((byte)(257 - reps));
                    encodedList.Add(decodedArray[i]);
                    reps = 1;
                }
                else
                {

                }
            }
            return encodedList.ToArray();
        }
        public static byte[] PackBitsDecode(byte[] encodedArray)
        {
            List<byte> decodedList = new();
            int leap = 1;
            for (int i = 0; i < encodedArray.Length; i += leap)
            {
                int convertedBytes = 0;
                byte conv = encodedArray[i];
                if (conv >= 0 && conv <= 127)
                {
                    byte cpy = (byte)(conv + 1);
                    for (int j = 0; j < cpy; j++)
                    {
                        decodedList.Add(encodedArray[i + 1 + j]);
                        convertedBytes++;
                    }
                    leap = 1 + convertedBytes;
                }
                else if (conv >= 129 && conv <= 255)
                {
                    byte rep = (byte)((256 - conv) + 1);
                    for (int j = 0; j < rep; j++)
                    {
                        decodedList.Add(encodedArray[i + 1]);
                    }
                    leap = 2;
                }
                else leap = 2;
            }
            return decodedList.ToArray();
        }
        private void SetMediaConfig(Media2Client media, string profileToken, string encoding, int quality,
            Media2Service.VideoResolution2 resolution)
        {
            var profilesSupported = media.GetServiceCapabilities().ProfileCapabilities.ConfigurationsSupported;
            foreach (var ps in profilesSupported)
            {
                listBox2.Items.Add(ps);
            }

            var profile = media.GetProfiles(profileToken, profilesSupported)[0];
            var vac = profile.Configurations.VideoEncoder;
            vac.Encoding = encoding;
            vac.Quality = quality;
            vac.Resolution = resolution;

            media.SetVideoEncoderConfiguration(vac);
        }

        private MediaProfile[] GetMediaProfiles(Media2Client media)
        {
            var profilesSupported = media.GetServiceCapabilities().ProfileCapabilities.ConfigurationsSupported;
            var profiles = media.GetProfiles(null, profilesSupported);
            return profiles;
        }

        private RuleEnginePortClient? GetRuleEnginePortClient(DeviceClient device, string username, string password)
        {
            if (device.GetServices(false).FirstOrDefault(s => s.Namespace == "http://www.onvif.org/ver20/analytics/wsdl") != null)
            {
                HttpTransportBindingElement httpTransport = new HttpTransportBindingElement();
                httpTransport.AuthenticationScheme = System.Net.AuthenticationSchemes.Digest;

                var binding = new CustomBinding(new TextMessageEncodingBindingElement(MessageVersion.Soap12WSAddressing10, Encoding.UTF8), httpTransport);
                var rulePort = new RuleEnginePortClient(binding, new EndpointAddress(deviceUri.ToString()));
                rulePort.ClientCredentials.UserName.UserName = username;
                rulePort.ClientCredentials.UserName.Password = password;
                rulePort.ClientCredentials.HttpDigest.ClientCredential.UserName = username;
                rulePort.ClientCredentials.HttpDigest.ClientCredential.Password = password;

                return rulePort;
            }
            else return null;
        }

        private Media2Client? GetDeviceMedia(DeviceClient device, string username, string password)
        {
            if (device.GetServices(false).FirstOrDefault(s => s.Namespace == "http://www.onvif.org/ver20/media/wsdl") != null)
            {
                HttpTransportBindingElement httpTransport = new HttpTransportBindingElement();
                httpTransport.AuthenticationScheme = System.Net.AuthenticationSchemes.Digest;

                var binding = new CustomBinding(new TextMessageEncodingBindingElement(MessageVersion.Soap12WSAddressing10,
                    Encoding.UTF8), httpTransport);
                var tempMedia = new Media2Client(binding, new EndpointAddress(deviceUri.ToString()));
                tempMedia.ClientCredentials.UserName.UserName = username;
                tempMedia.ClientCredentials.UserName.Password = password;
                tempMedia.ClientCredentials.HttpDigest.ClientCredential.UserName = username;
                tempMedia.ClientCredentials.HttpDigest.ClientCredential.Password = password;

                return tempMedia;
            }
            else return null;
        }
        private DeviceClient LoginToDevice(string ip, string username, string password)
        {
            deviceUri = new UriBuilder("http://" + ip + "/onvif/device_service");
            HttpTransportBindingElement httpTransport = new HttpTransportBindingElement();
            httpTransport.AuthenticationScheme = System.Net.AuthenticationSchemes.Digest;
            var binding = new CustomBinding(new TextMessageEncodingBindingElement(MessageVersion.Soap12WSAddressing10,
                Encoding.UTF8), httpTransport);

            DeviceClient tempDevice = new DeviceClient(binding, new EndpointAddress(deviceUri.ToString()));

            tempDevice.ClientCredentials.HttpDigest.ClientCredential.UserName = username;
            tempDevice.ClientCredentials.HttpDigest.ClientCredential.Password = password;


            return tempDevice;
        }

        private void playMediaStream(Media2Client media, MediaProfile profile)
        {
            UriBuilder uri = new UriBuilder(media.GetStreamUri("RtspOverHttp", profile.token));
            uri.Host = deviceUri.Host;
            uri.Port = deviceUri.Port;
            uri.Scheme = "rtsp";

            string[] options = { ":rtsp-http", ":rtsp-http-port=" + uri.Port,
                ":rtsp-user=" + camUsername, ":rtsp-pwd=" + camPassword, };
            video.VlcMediaPlayer.Play(uri.Uri, options);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (profiles != null && listBox1.SelectedIndex >= 0)
            {
                playMediaStream(media, profiles[listBox1.SelectedIndex]);
            }
        }
        
        private void VlcLibDirectoryNeeded(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        {

            e.VlcLibDirectory = new DirectoryInfo("C:\\Program Files\\VideoLAN\\VLC"); // REPLACE WITH YOUR OWN INSTALLATION PATH OF VLC

        }

        private void video_Click(object sender, EventArgs e)
        {

        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            var res = new Media2Service.VideoResolution2();
            string[] selectedRes = resolutionBox.SelectedItem.ToString().Split("x");
            int widthPart = int.Parse(selectedRes[0].Split(":")[1].Trim());
            int heightPart = int.Parse(selectedRes[1].Trim());
            res.Width = widthPart;
            res.Height = heightPart;
            SetMediaConfig(media, profiles[0].token, codecBox.SelectedItem.ToString(),
                Convert.ToInt32(qualityBox.SelectedItem.ToString().Split(":")[1].Trim()), res);
        }
    }
}