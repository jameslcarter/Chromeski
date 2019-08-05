﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NativeWindow.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using CefSharp;
using Chromely.Core;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using NetCoreEx.Geometry;
using WinApi.DwmApi;
using WinApi.Gdi32;
using WinApi.Kernel32;
using WinApi.User32;

namespace Chromely.CefSharp.Winapi.BrowserWindow
{
    /// <summary>
    /// The native window.
    /// </summary>
    internal class NativeWindow
    {
        /// <summary>
        /// The host config.
        /// </summary>
        private readonly ChromelyConfiguration _hostConfig;

        /// <summary>
        /// WindowProc ref : prevent GC Collect
        /// </summary>
        private WindowProc _windowProc;

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeWindow"/> class.
        /// </summary>
        public NativeWindow()
        {
            Handle = IntPtr.Zero;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeWindow"/> class.
        /// </summary>
        /// <param name="hostConfig">
        /// Chromely configuration.
        /// </param>
        public NativeWindow(ChromelyConfiguration hostConfig)
        {
            Handle = IntPtr.Zero;
            _hostConfig = hostConfig;
        }

        /// <summary>
        /// Gets the handle.
        /// </summary>
        public IntPtr Handle { get; private set; }

        /// <summary>
        /// The run message loop.
        /// </summary>
        public static void RunMessageLoop()
        {
            while (User32Methods.GetMessage(out Message msg, IntPtr.Zero, 0, 0) != 0)
            {
                if (ChromelyConfiguration.Instance.HostPlacement.Frameless)
                {
                    Cef.DoMessageLoopWork();
                }

                User32Methods.TranslateMessage(ref msg);
                User32Methods.DispatchMessage(ref msg);
            }
        }

        /// <summary>
        /// The exit.
        /// </summary>
        public static void Exit()
        {
            User32Methods.PostQuitMessage(0);
        }

        /// <summary>
        /// The show window.
        /// </summary>
        public void ShowWindow()
        {
            CreateWindow();
        }

        /// <summary>
        /// The close window externally.
        /// </summary>
        public void CloseWindowExternally()
        {
            User32Methods.PostQuitMessage(0);
        }

        /// <summary>
        /// The get client size.
        /// </summary>
        /// <returns>
        /// The <see cref="Size"/>.
        /// </returns>
        public Size GetClientSize()
        {
            var size = new Size();
            if (Handle != IntPtr.Zero)
            {
                User32Methods.GetClientRect(Handle, out var rectangle);
                size.Width = rectangle.Width;
                size.Height = rectangle.Height;
            }

            return size;
        }

        /// <summary>
        /// The get window size.
        /// </summary>
        /// <returns>
        /// The <see cref="Size"/>.
        /// </returns>
        public Size GetWindowSize()
        {
            var size = new Size();
            if (Handle != IntPtr.Zero)
            {
                User32Methods.GetWindowRect(Handle, out var rectangle);
                size.Width = rectangle.Width;
                size.Height = rectangle.Height;
            }

            return size;
        }

        /// <summary>
        /// The center to screen.
        /// </summary>
        /// <param name="useWorkArea">
        /// The use work area.
        /// </param>
        public void CenterToScreen(bool useWorkArea = true)
        {
            var monitor = User32Methods.MonitorFromWindow(Handle, MonitorFlag.MONITOR_DEFAULTTONEAREST);
            User32Helpers.GetMonitorInfo(monitor, out var monitorInfo);
            var screenRect = useWorkArea ? monitorInfo.WorkRect : monitorInfo.MonitorRect;
            var midX = screenRect.Width / 2;
            var midY = screenRect.Height / 2;
            var size = GetWindowSize();
            var left = midX - (size.Width / 2);
            var top = midY - (size.Height / 2);

            User32Methods.SetWindowPos(
                Handle,
                IntPtr.Zero,
                left,
                top,
                -1,
                -1,
                WindowPositionFlags.SWP_NOACTIVATE | WindowPositionFlags.SWP_NOSIZE | WindowPositionFlags.SWP_NOZORDER);
        }

        /// <summary>
        /// The on create.
        /// </summary>
        /// <param name="hwnd">
        /// The hwnd.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        protected virtual void OnCreate(IntPtr hwnd, int width, int height)
        {
        }

        /// <summary>
        /// The on size.
        /// </summary>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        protected virtual void OnSize(int width, int height)
        {
        }

        /// <summary>
        /// The on exit.
        /// </summary>
        protected virtual void OnExit()
        {
        }

        /// <summary>
        /// The create window.
        /// </summary>
        private void CreateWindow()
        {
            var instanceHandle = Kernel32Methods.GetModuleHandle(IntPtr.Zero);

            _windowProc = WindowProc;
            
            var wc = new WindowClassEx
            {
                Size = (uint)Marshal.SizeOf<WindowClassEx>(),
                ClassName = "chromelywindow",
                CursorHandle = User32Helpers.LoadCursor(IntPtr.Zero, SystemCursor.IDC_ARROW),
                IconHandle = GetIconHandle(),
                Styles = WindowClassStyles.CS_HREDRAW | WindowClassStyles.CS_VREDRAW,
                BackgroundBrushHandle = new IntPtr((int)StockObject.WHITE_BRUSH),
                WindowProc = _windowProc,
                InstanceHandle = instanceHandle
            };

            var resReg = User32Methods.RegisterClassEx(ref wc);
            if (resReg == 0)
            {
                Log.Error("chromelywindow registration failed");
                return;
            }

            var styles = GetWindowStyles(_hostConfig.HostPlacement.State);
   
            var placement = _hostConfig.HostPlacement;

            NativeMethods.RECT rect;
            rect.Left = placement.Left;
            rect.Top = placement.Top;
            rect.Right = placement.Left + placement.Width;
            rect.Bottom = placement.Top + placement.Height;
            NativeMethods.AdjustWindowRectEx(ref rect, styles.Item1, false, styles.Item2);

            var hwnd = User32Methods.CreateWindowEx(
                styles.Item2,
                wc.ClassName,
                _hostConfig.HostPlacement.Frameless ? string.Empty : _hostConfig.HostTitle,
                styles.Item1,
                rect.Left,
                rect.Top,
                rect.Right - rect.Left,
                rect.Bottom - rect.Top,   
                IntPtr.Zero,
                IntPtr.Zero,
                instanceHandle,
                IntPtr.Zero);

            if (hwnd == IntPtr.Zero)
            {
                Log.Error("chromelywindow creation failed");
                return;
            }

            User32Methods.ShowWindow(Handle, styles.Item3);
            User32Methods.UpdateWindow(Handle);
        }

        /// <summary>
        /// The window proc.
        /// </summary>
        /// <param name="hwnd">
        /// The hwnd.
        /// </param>
        /// <param name="umsg">
        /// The umsg.
        /// </param>
        /// <param name="wParam">
        /// The w param.
        /// </param>
        /// <param name="lParam">
        /// The l param.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        private IntPtr WindowProc(IntPtr hwnd, uint umsg, IntPtr wParam, IntPtr lParam)
        {
            var msg = (WM)umsg;
            switch (msg)
            {
                case WM.ACTIVATE:
                    {
                        if (_hostConfig.HostPlacement.Frameless)
                        {
                            var frameSizeY = User32Methods.GetSystemMetrics(SystemMetrics.SM_CYFRAME);
                            var frameSizeX = User32Methods.GetSystemMetrics(SystemMetrics.SM_CXFRAME);
                            var frameMargins = new Margins(frameSizeX, frameSizeX, frameSizeY, frameSizeY);
                            DwmApiMethods.DwmExtendFrameIntoClientArea(Handle, ref frameMargins);
                            User32Methods.SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, WindowPositionFlags.SWP_NOZORDER | WindowPositionFlags.SWP_NOOWNERZORDER | WindowPositionFlags.SWP_NOMOVE | WindowPositionFlags.SWP_NOSIZE | WindowPositionFlags.SWP_FRAMECHANGED);
                        }
                        break;
                    }

                case WM.CREATE:
                    {
                        Handle = hwnd;
                        var size = GetClientSize();
                        OnCreate(hwnd, size.Width, size.Height);
                        break;
                    }

                case WM.ERASEBKGND:
                    return new IntPtr(1);

                case WM.SIZE:
                    {
                        var size = GetClientSize();
                        OnSize(size.Width, size.Height);
                        break;
                    }

                case WM.CLOSE:
                    {
                        OnExit();
                        Exit();
                        Environment.Exit(0);
                        break;
                    }

                case WM.NCHITTEST:
                    if (_hostConfig.HostPlacement.Frameless)
                    {
                        return (IntPtr)NativeMethods.HT_CAPTION;
                    }
                    break;
            }

            return User32Methods.DefWindowProc(hwnd, umsg, wParam, lParam);
        }

        /// <summary>
        /// The get window styles.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// The <see cref="Tuple"/>.
        /// </returns>
        private Tuple<WindowStyles, WindowExStyles, ShowWindowCommands> GetWindowStyles(WindowState state)
        {
            if (_hostConfig.UseHostCustomCreationStyle)
            {
                var customCreationStyle = _hostConfig.HostCustomCreationStyle as WindowCreationStyle;
                if (customCreationStyle != null)
                {
                    return GetWindowStyles(customCreationStyle, state);
                }
            }

            var styles = WindowStyles.WS_OVERLAPPEDWINDOW | WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS;
            var exStyles = WindowExStyles.WS_EX_APPWINDOW | WindowExStyles.WS_EX_WINDOWEDGE;

            if (_hostConfig.HostPlacement.NoResize)
            {
                styles = WindowStyles.WS_OVERLAPPEDWINDOW & ~WindowStyles.WS_THICKFRAME | WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS;
                styles &= ~WindowStyles.WS_MAXIMIZEBOX;
            }

            if (_hostConfig.HostPlacement.NoMinMaxBoxes)
            {
                styles &= ~WindowStyles.WS_MINIMIZEBOX;
                styles &= ~WindowStyles.WS_MAXIMIZEBOX;
            }

            if (_hostConfig.HostPlacement.Frameless)
            {
                styles = WindowStyles.WS_CAPTION | WindowStyles.WS_POPUP | WindowStyles.WS_THICKFRAME | WindowStyles.WS_MINIMIZEBOX | WindowStyles.WS_MAXIMIZEBOX | WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS;
            }

            if (_hostConfig.HostPlacement.KioskMode)
            {
                styles &= ~(WindowStyles.WS_CAPTION | WindowStyles.WS_THICKFRAME);
                exStyles &= ~(WindowExStyles.WS_EX_DLGMODALFRAME | WindowExStyles.WS_EX_WINDOWEDGE | WindowExStyles.WS_EX_CLIENTEDGE | WindowExStyles.WS_EX_STATICEDGE);
            }

            switch (state)
            {
                case WindowState.Normal:
                    return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommands>(styles, exStyles, ShowWindowCommands.SW_SHOWNORMAL);

                case WindowState.Maximize:
                    {
                        styles |= WindowStyles.WS_MAXIMIZE;
                        return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommands>(styles, exStyles, ShowWindowCommands.SW_SHOWMAXIMIZED);
                    }

                case WindowState.Fullscreen:
                    {
                        styles |= WindowStyles.WS_MAXIMIZE;
                        exStyles = WindowExStyles.WS_EX_TOOLWINDOW;
                        return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommands>(styles, exStyles, ShowWindowCommands.SW_SHOWMAXIMIZED);
                    }
            }
            
            return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommands>(styles, exStyles, ShowWindowCommands.SW_SHOWNORMAL);
        }

        /// <summary>
        /// The get window styles.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// The <see cref="Tuple"/>.
        /// </returns>
        private Tuple<WindowStyles, WindowExStyles, ShowWindowCommands> GetWindowStyles(WindowCreationStyle customCreationStyle, WindowState state)
        {
            var styles = customCreationStyle.WindowStyles;
            var exStyles = customCreationStyle.WindowExStyles;

            switch (state)
            {
                case WindowState.Normal:
                    return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommands>(styles, exStyles, ShowWindowCommands.SW_SHOWNORMAL);

                case WindowState.Maximize:
                    return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommands>(styles, exStyles, ShowWindowCommands.SW_SHOWMAXIMIZED);

                case WindowState.Fullscreen:
                    return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommands>(styles, exStyles, ShowWindowCommands.SW_SHOWMAXIMIZED);
            }

            return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommands>(styles, exStyles, ShowWindowCommands.SW_SHOWNORMAL);
        }

        /// <summary>
        /// The get icon handle.
        /// </summary>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        private IntPtr GetIconHandle()
        {
            var hIcon = NativeMethods.LoadIconFromFile(_hostConfig.HostIconFile);
            return hIcon ?? User32Helpers.LoadIcon(IntPtr.Zero, SystemIcon.IDI_APPLICATION);
        }
    }
}