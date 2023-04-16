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

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace GoPro
{
	/// <summary>
	/// Control a GoPro Camera using OpenGoPro HTTP API (https://gopro.github.io/OpenGoPro/http_2_0)
	/// </summary>
	public sealed class GoProManager : MonoBehaviour
	{
        public float connectionCheckRate = 2f;

        public bool IsConnected { get { return !string.IsNullOrEmpty(IPAddress); } }
        public string IPAddress { get; private set; }

        public Action Connected;
        public Action Disconnected;

        private bool _wasConnected = false;

        public void EnableUSBControl(Action<bool, string, string> pOnCommandSent)
		{
            string url = GoProCommands.FormatCommand(IPAddress, GoProCommands.USBControlON);
            _HTTPRequest(url, "EnableUSBControl", pOnCommandSent);
            //StartCoroutine(_HTTPRequest(url, "EnableUSBControl", pOnCommandSent));
        }

        public void DisabeUSBControl(Action<bool, string, string> pOnCommandSent)
        {
            string url = GoProCommands.FormatCommand(IPAddress, GoProCommands.USBControlOFF);
            _HTTPRequest(url, "DisabeUSBControl", pOnCommandSent);
            //StartCoroutine(_HTTPRequest(url, "DisabeUSBControl", pOnCommandSent));
        }

        public void ShutterON(Action<bool, string, string> pOnCommandSent)
		{
            string url = GoProCommands.FormatCommand(IPAddress, GoProCommands.ShutterON);
            _HTTPRequest(url, "ShutterON", pOnCommandSent);
            //StartCoroutine(_HTTPRequest(url, "ShutterON", pOnCommandSent));
        }

        public void ShutterOFF(Action<bool, string, string> pOnCommandSent)
		{
            string url = GoProCommands.FormatCommand(IPAddress, GoProCommands.ShutterOFF);
            _HTTPRequest(url, "ShutterOFF", pOnCommandSent);
            //StartCoroutine(_HTTPRequest(url, "ShutterOFF", pOnCommandSent));
        }

        private void OnEnable()
		{
            InvokeRepeating("_CheckConnection", 0f, connectionCheckRate);
		}

        private void OnDisable()
        {
            CancelInvoke("_CheckConnection"); // stop repeating
            _CheckConnection(); // but we need to update the last status because it depends on "enabled"
        }

        private void _HTTPRequest(string pURL, string pCommandName, Action<bool, string, string> pOnCommandSent)
        {
            bool success = false;
            string msg = "";
            UnityWebRequest www = null;
            try
            {
                www = UnityWebRequest.Get(pURL);
                www.SendWebRequest().completed += (AsyncOperation op) =>
                {
                    success = www.isDone && (!www.isNetworkError) && (!www.isHttpError);
                    msg = success ? www.downloadHandler.text : www.error;
                    www.Dispose();
                    if (pOnCommandSent != null)
                    {
                        pOnCommandSent(success, pCommandName, msg);
                    }
                };
                
            }
            catch(Exception e)
			{
                success = false;
                msg = e.ToString();
                if(www != null)
				{
                    www.Dispose();
                }
                    
                if (pOnCommandSent != null)
                {
                    pOnCommandSent(success, pCommandName, msg);
                }
            }

            /*
			// Coroutine version : cannot be used with try/catch
            bool success = false;
			string msg = "GoPro disconnected !";
			if (IsConnected)
			{
				UnityWebRequest www = UnityWebRequest.Get(pURL);
                yield return www.SendWebRequest();
                success = www.isDone && (!www.isNetworkError) && (!www.isHttpError);
				msg = success ? www.downloadHandler.text : www.error;
                www.Dispose();
			}

			if (pOnCommandSent != null)
			{
				pOnCommandSent(success, pCommandName, msg);
			}
            */
        }

        private void _CheckConnection()
		{
            _wasConnected = IsConnected;
            IPAddress = enabled ? _GetGoProIPAddress() : string.Empty;
            if (IsConnected)
            {
                if (!_wasConnected && (Connected != null))
                {
                    Connected();
                }
            }
            else
            {
                if (_wasConnected && (Disconnected != null))
                {
                    Disconnected();
                }
            }
        }

		private static string _GetGoProIPAddress()
		{
            string strHostName = Dns.GetHostName();
            IPHostEntry iphostentry = Dns.GetHostEntry(strHostName);
            foreach (IPAddress ip in iphostentry.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    byte[] abytes = ip.GetAddressBytes();
                    if (abytes[0] == 172 && abytes[1] >= 20 && abytes[1] <= 29 && abytes[3] >= 50 && abytes[3] <= 70)
                    {
                        StringBuilder sb = new StringBuilder(ip.ToString());
                        sb[sb.Length - 1] = '1';
                        return sb.ToString();
                    }
                }
            }
            return string.Empty;
        }
	}
}
