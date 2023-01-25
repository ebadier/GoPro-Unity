/******************************************************************************************************************************************************
* MIT License																																		  *
*																																					  *
* Copyright (c) 2023																																  *
* Emmanuel Badier <emmanuel.badier@gmail.com>																										  *
* University of Geneva																																  *
* 																																					  *
* Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),  *
* to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,  *
* and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:		  *
* 																																					  *
* The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.					  *
* 																																					  *
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, *
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 																							  *
* IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 		  *
* TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.							  *
******************************************************************************************************************************************************/

namespace GoPro
{
	public static class GoProCommands
	{
		public static string FormatCommand(string pIP, string pCommand)
		{
			return string.Format("http://{0}:8080{1}", pIP, pCommand);
		}

		// Enable/Disable control over USB
		// Note : Enabling control over USB disable MTP (access to files as a drive already mounted) !
		public const string USBControlON = "/gopro/camera/control/wired_usb?p=1";
		public const string USBControlOFF = "/gopro/camera/control/wired_usb?p=0";

		// Start/Stop Photo/Video/Timelapse
		// Need control over USB
		public const string ShutterON = "/gopro/camera/shutter/start";
		public const string ShutterOFF = "/gopro/camera/shutter/stop";

		// Webcam
		// Don't need control over USB
		// Webcam mode is default to : 1920x1080@30Hz
		public enum WebcamResolution { HD = 7, FullHD = 12 }
		public enum WebcamFoV { Large = 0, Narrow = 2, SuperView = 3, Linear = 4 }
		public const string WebcamStart = "/gopro/webcam/start?res={0}&fov={1}";
		public const string WebcamStop = "/gopro/webcam/stop";
		public const string WebcamExit = "/gopro/webcam/exit";
		// To get video streaming from VLC for example
		// In VLC : the minimum network cache can be set to 250ms (lower value crashes), for a total delay (GoPro => VLC) of 450ms
		public const string WebcamEndPoint = "udp://@0.0.0.0:8554";
	}
}