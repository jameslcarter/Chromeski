﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Winapi.BrowserHost
{
    using System;
    using Chromely.CefGlue.Winapi.Browser;
    using Chromely.Core;
    using NetCoreEx.Geometry;

    using WinApi.User32;

    using Xilium.CefGlue;

    /// <summary>
    /// The window.
    /// </summary>
    public class Window : NativeWindow
    {
        /// <summary>
        /// The host/app/window application.
        /// </summary>
        private readonly HostBase mApplication;

        /// <summary>
        /// The host config.
        /// </summary>
        private readonly ChromelyConfiguration mHostConfig;

        /// <summary>
        /// The CefGlueBrowser object.
        /// </summary>
        private readonly CefGlueBrowser mBrowser;

        /// <summary>
        /// The browser window handle.
        /// </summary>
        private IntPtr mBrowserWindowHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class.
        /// </summary>
        /// <param name="application">
        /// The application.
        /// </param>
        /// <param name="hostConfig">
        /// The host config.
        /// </param>
        public Window(HostBase application, ChromelyConfiguration hostConfig)
            : base(hostConfig)
        {
            mHostConfig = hostConfig;
            mBrowser = new CefGlueBrowser(this, hostConfig, new CefBrowserSettings());
            mBrowser.Created += BrowserCreated;
            mApplication = application;

            ShowWindow();
        }

        /// <summary>
        /// The web browser.
        /// </summary>
        public CefGlueBrowser WebBrowser => mBrowser;

        #region Close/Dispose

        /// <summary>
        /// The close.
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            if (mBrowser != null)
            {
                var browser = mBrowser.CefBrowser;
                var host = browser.GetHost();
                host.CloseBrowser();
                host.Dispose();
                browser.Dispose();
                mBrowserWindowHandle = IntPtr.Zero;
            }
        }

        #endregion Close/Dispose

        /// <summary>
        /// The on realized.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <exception cref="NotSupportedException">
        /// Exception returned for MacOS not supported.
        /// </exception>
        protected override void OnCreate(IntPtr hwnd, int width, int height)
        {
            var windowInfo = CefWindowInfo.Create();
            windowInfo.SetAsChild(hwnd, new CefRectangle(0, 0, mHostConfig.HostWidth, mHostConfig.HostHeight));
            mBrowser.Create(windowInfo);
        }

        /// <summary>
        /// The on resize.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnSize(int width, int height)
        {
            if (mBrowserWindowHandle != IntPtr.Zero)
            {
                if (width == 0 && height == 0)
                {
                    // For windowed browsers when the frame window is minimized set the
                    // browser window size to 0x0 to reduce resource usage.
                    NativeMethods.SetWindowPos(mBrowserWindowHandle, IntPtr.Zero, 0, 0, 0, 0, WindowPositionFlags.SWP_NOZORDER | WindowPositionFlags.SWP_NOMOVE | WindowPositionFlags.SWP_NOACTIVATE);
                }
                else
                {
                    NativeMethods.SetWindowPos(mBrowserWindowHandle, IntPtr.Zero, 0, 0, width, height, WindowPositionFlags.SWP_NOZORDER);
                }
            }
        }

        /// <summary>
        /// The on exit.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnExit()
        {
            mApplication.Quit();
        }

        /// <summary>
        /// The browser created.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BrowserCreated(object sender, EventArgs e)
        {
            mBrowserWindowHandle = mBrowser.CefBrowser.GetHost().GetWindowHandle();
            if (mBrowserWindowHandle != IntPtr.Zero)
            {
                Size size = GetClientSize();
                NativeMethods.SetWindowPos(mBrowserWindowHandle, IntPtr.Zero, 0, 0, size.Width, size.Height, WindowPositionFlags.SWP_NOZORDER);
            }
        }
    }
}
