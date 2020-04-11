﻿using Chromely.Core;
using Chromely.Core.Configuration;
using System;
using System.Drawing;

namespace Chromely.Core.Host
{
    public interface IChromelyNativeHost
    {
        event EventHandler<CreatedEventArgs> Created;
        event EventHandler<MovingEventArgs> Moving;
        event EventHandler<SizeChangedEventArgs> SizeChanged;
        event EventHandler<CloseEventArgs> Close;
        IntPtr Handle { get; }
        void CreateWindow(IWindowOptions options, bool debugging);
        IntPtr GetNativeHandle();
        void Run();
        Size GetWindowClientSize();
        float GetWindowDpiScale();
        void ResizeBrowser(IntPtr browserWindow, int width, int height);
        void Exit();
        void MessageBox(string message, int type);
    }
}
