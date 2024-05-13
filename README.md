# OnvifCoreFormsApp
WinForms App for ONVIF Service testing purposes. Logs into the camera using IP, username and password. Can display main stream and sub streams, also includes several other functionality to analyse your camera's capabilities

![Screenshot_4](https://github.com/ozdoganoguzhan/ONVIF-WinForms/assets/30651481/70659fe8-6f85-421d-872e-3594dab2d099)

## Usage
You can easily use this app to monitor your IP Camera with ONVIF Compliancy. After you enter the IP, username and password for your camera and press start, it will connect to that camera and get active media streams. Those are listed on the list in the middle, from which you can select the stream that you want to display on the left. The section on the right is for console outputs for additional information we gather.

## Compatibility
Compatible with any ONVIF Compliant IP Camera including the ones that I've tested personally below:
* Hikvision
* Dahua
* Reolink
* Uniwiz (Uniview)

## Additional Functionality
There are some code which I commented out, you can enable them by uncommenting AND adding the required ONVIF WSDL for that specific function.

### Event Subscription and Motion Cells
Includes commented out code (Because I wasn't sure at the time if it was the best solution) for event subscription and setting the Motion Cells variable. Also includes functions to decode and encode Pack Bits which is needed for Motion Cells.

**Motion Cells:** This is the area in which motion detection is run. ONVIF compliant cameras store this area as a base64 encoded string, which when decoded, translates into a Pack Bits encoded string. Long story short, Pack Bits is the algorithm that is used to shorten that 22 x 18 character string (Because that's how many rectangles the motion area has).    

After you decode that string too, you get the 0's and 1's that represent each rectangle (Which you can see in your camera's web interface) and you can manipulate those to change the area. 

The code is commented out because UI doesn't have any binding to this. However, you can still edit the code and set your Active Cells.
