﻿using System;
using System.Globalization;
using System.Net;
using System.Windows.Forms;

namespace RemoteView.PageHandlers
{
    class MouseMovePageHandler: AbstractPageHandler
    {
        // screen devices list
        private Screen[] screens = Screen.AllScreens;

        public override byte[] HandleRequest(HttpListenerResponse response, string[] uri)
        {
            // must have 5 tokens
            if (uri.Length != 5)
            {
                response.StatusCode = 400;
                return BuildHTML("Error...");
            }

            int x, y;
            try
            {
                y = Convert.ToInt16(uri[3], CultureInfo.InvariantCulture);
                x = Convert.ToInt16(uri[4], CultureInfo.InvariantCulture);
            }
            catch
            {
                // parameter error
                response.StatusCode = 400;
                return BuildHTML("Error...");
            }

            int screen = GetRequestedScreenDevice(uri, screens);

            // check bounds
            Screen device = screens[screen];
            if (x < 0 || x >= device.Bounds.Width || y < 0 || y >= device.Bounds.Height)
            {
                response.StatusCode = 400;
                return BuildHTML("Error...");
            }

            // adapt to real screen bounds

            x = device.Bounds.X + x;
            y = device.Bounds.X + y;

            NativeMethods.MoveMouse(x, y);

            return BuildHTML("Updating...");
        }
    }
}
