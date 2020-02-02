﻿using Chromely.Core.Configuration;
using Chromely.Core.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using static Chromely.Native.WinNativeMethods;

namespace Chromely.Native
{
    public class WindowMessageInterceptor : IChromelyFramelessController
    {
        public delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        private readonly IChromelyConfiguration _config;
        private readonly ForwardMesssageHandler[] _hitTestReplacers;

        public WindowMessageInterceptor(IChromelyConfiguration config, IntPtr browserHandle, IntPtr windowHandle)
        {
            _config = config;
            var framelessOption = _config?.WindowOptions?.FramelessOption ?? new FramelessOption();

            var childHandles = GetAllChildHandles(browserHandle);
            _hitTestReplacers = childHandles
                .Concat(new[] { browserHandle })
                .Select(h => new ForwardMesssageHandler(h, windowHandle, framelessOption, h == browserHandle))
                .ToArray();
        }

        protected virtual List<IntPtr> GetAllChildHandles(IntPtr handle)
        {
            var childHandles = new List<IntPtr>();
            GCHandle gcChildhandlesList = GCHandle.Alloc(childHandles);
            IntPtr pointerChildHandlesList = GCHandle.ToIntPtr(gcChildhandlesList);

            try
            {
                var childProc = new EnumWindowProc(EnumWindow);
                EnumChildWindows(handle, childProc, pointerChildHandlesList);
            }
            finally
            {
                gcChildhandlesList.Free();
            }

            return childHandles;
        }

        protected virtual bool EnumWindow(IntPtr hWnd, IntPtr lParam)
        {
            GCHandle gcChildhandlesList = GCHandle.FromIntPtr(lParam);
            if (gcChildhandlesList == null || gcChildhandlesList.Target == null)
            {
                return false;
            }

            List<IntPtr> childHandles = gcChildhandlesList.Target as List<IntPtr>;
            childHandles.Add(hWnd);

            return true;
        }

        private class ForwardMesssageHandler
        {
            private readonly IntPtr _handle;
            private readonly IntPtr _mainHandle;
            private readonly bool _isHost;
            private readonly IntPtr _originalWndProc;
            private readonly WndProc _wndProc;
            private readonly FramelessOption _framelessOption;

            public ForwardMesssageHandler(IntPtr handle, IntPtr mainHandle, FramelessOption framelessOption, bool isHost)
            {
                _handle = handle;
                _mainHandle = mainHandle;
                _framelessOption = framelessOption ?? new FramelessOption();
                _isHost = isHost;
                _originalWndProc = GetWindowLongPtr(_handle, (int)WindowLongFlags.GWL_WNDPROC);
                _wndProc = WndProc;
                var wndProcPtr = Marshal.GetFunctionPointerForDelegate(_wndProc);
                SetWindowLongPtr(_handle, (int)WindowLongFlags.GWL_WNDPROC, wndProcPtr);
            }

            private IntPtr WndProc(IntPtr hWnd, uint message, IntPtr wParam, IntPtr lParam)
            {
                var isForwardedArea = IsForwardedArea();
                if (isForwardedArea)
                {
                    SendMessage(_mainHandle, (int)message, wParam, lParam);
                }

                var msg = (WM)message;
                switch (msg)
                {
                    case WM.NCHITTEST:
                        {
                            if (isForwardedArea && _isHost)
                            {
                                return (IntPtr)HitTestValue.HTNOWHERE;
                            }
                            break;
                        }
                    default:
                        {
                            if (isForwardedArea)
                            {
                                return IntPtr.Zero;
                            }
                            break;
                        }
                }

                return CallWindowProc(_originalWndProc, hWnd, message, wParam, lParam);
            }

            // TODO: Enchance to configurable region.
            private bool IsForwardedArea()
            {
                GetCursorPos(out var point);
                ScreenToClient(_mainHandle, ref point);
                GetClientRect(_mainHandle, out var mainClientRect);
                var right = mainClientRect.Width - point.X;

                return point.Y <= _framelessOption.DraggableHeight && right > _framelessOption.NonDraggableRightOffsetWidth;
            }
        }

    }
}