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

using UnityEngine;

namespace GoPro
{
	[RequireComponent(typeof(GoProManager))]
	public sealed class GoProManagerTest : MonoBehaviour
	{
		private GoProManager _goProManager;

		void Awake()
		{
			_goProManager = GetComponent<GoProManager>();
			_goProManager.Connected += () => Debug.Log("[GoProManagerTest] GoPro connected : " + _goProManager.IPAddress);
			_goProManager.Disconnected += () => Debug.LogWarning("[GoProManagerTest] GoPro disconnected !");
		}

		[ContextMenu("EnableUSBControl")]
		public void EnableUSBControl()
		{
			_goProManager.EnableUSBControl(_OnCommandSent);
		}

		[ContextMenu("DisabeUSBControl")]
		public void DisabeUSBControl()
		{
			_goProManager.DisabeUSBControl(_OnCommandSent);
		}

		[ContextMenu("ShutterON")]
		public void ShutterON()
		{
			_goProManager.ShutterON(_OnCommandSent);
		}

		[ContextMenu("ShutterOFF")]
		public void ShutterOFF()
		{
			_goProManager.ShutterOFF(_OnCommandSent);
		}

		private void _OnCommandSent(bool pSuccess, string pCommandName, string pMsg)
		{
			if(pSuccess)
			{
				Debug.Log(string.Format("[GoProManagerTest] {0} succeed.", pCommandName));
			}
			else
			{
				Debug.LogWarning(string.Format("[GoProManagerTest] {0} failed : {1}", pCommandName, pMsg));
			}
		}
	}
}
