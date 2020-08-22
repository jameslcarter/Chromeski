﻿using System;
using Chromely.CefGlue.Browser;
using Chromely.CefGlue.Browser.EventParams;
using Chromely.CefGlue.BrowserWindow;
using Chromely.Core;
using Chromely.Core.Configuration;
using Chromely.Core.Host;
using Chromely.Core.Logging;
using Chromely.Core.Network;
using Chromely.Native;
using Xilium.CefGlue;
using Xilium.CefGlue.Wrapper;

namespace Chromely.Windows
{
    public class Window : NativeWindow, IWindow
    {
        private readonly IChromelyContainer _container;
        private readonly IChromelyConfiguration _config;
        private readonly IChromelyCommandTaskRunner _commandTaskRunner;
        private readonly CefMessageRouterBrowserSide _browserMessageRouter;

        private IChromelyFramelessController _framelessController;
        private IntPtr _browserWindowHandle;
        private bool _isFramelessControllerInitialized = false;

        public Window(IChromelyNativeHost nativeHost, IChromelyContainer container, IChromelyConfiguration config, IChromelyCommandTaskRunner commandTaskRunner, CefMessageRouterBrowserSide browserMessageRouter)
            : base(nativeHost, config)
        {
            _container = container;
            _config = config;
            _commandTaskRunner = commandTaskRunner;
            _browserMessageRouter = browserMessageRouter;
            Browser = new CefGlueBrowser(this, _container, config, _commandTaskRunner, _browserMessageRouter, new CefBrowserSettings());
            
            // Set event handler
            Browser.SetEventHandlers(_container);

            Browser.Created += OnBrowserCreated;

            // 'Created' event sometimes tries to attach interceptors too early, while all windows is not created yet,
            // so it's better to use 'FrameLoadStart'.
            Browser.FrameLoadStart += OnFrameLoadStart;

            ShowWindow();
        }

        public CefGlueBrowser Browser { get; private set; }

        /// <summary>
        /// Gets the window handle.
        /// </summary>
        public IntPtr HostHandle => Handle;
         

        public void CenterToScreen()
        {
            //TODO: Implement
        }

        public void Exit()
        {
            Quit();
        }

        #region Dispose

        public void Dispose()
        {
            try
            {
                if (Browser != null)
                {
                    // A hack - until fix is found for why OnBeforeClose is not called in CefGlueLifeHandler.cs
                    Browser?.OnBeforeClose();

                    Browser?.Dispose();
                    Browser = null;
                    _browserWindowHandle = IntPtr.Zero;
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error(exception);
            }
        }

        #endregion Dispose

        protected override void OnCreated(object sender, CreatedEventArgs createdEventArgs)
        {
            Handle = createdEventArgs.Window;
            WinXID = createdEventArgs.WinXID;
            Browser.HostHandle = createdEventArgs.Window;

            var windowInfo = CefWindowInfo.Create();
            windowInfo.SetAsChild(createdEventArgs.WinXID, new CefRectangle(0, 0, _config.WindowOptions.Size.Width, _config.WindowOptions.Size.Height));

            Browser.Create(windowInfo);
        }

        protected override void OnMoving(object sender, MovingEventArgs movingEventArgs)
        {
            if (_browserWindowHandle != IntPtr.Zero)
            {
                Browser.CefBrowser.GetHost().NotifyMoveOrResizeStarted();
            }
        }

        protected override void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            if (_browserWindowHandle != IntPtr.Zero)
            {
                ResizeBrowser(_browserWindowHandle, sizeChangedEventArgs.Width, sizeChangedEventArgs.Height);
            }
        }

        protected override void OnClose(object sender, CloseEventArgs closeChangedEventArgs)
        {
            Dispose();
            Core.Infrastructure.ChromelyAppUser.App.Properties.Save(_config);
        }

        private void OnBrowserCreated(object sender, EventArgs e)
        {
            _browserWindowHandle = Browser.CefBrowser.GetHost().GetWindowHandle();
            if (_browserWindowHandle != IntPtr.Zero)
            {

                ResizeBrowser(_browserWindowHandle);
            }
        }

        private void OnFrameLoadStart(object sender, EventArgs e)
        {
            if (_isFramelessControllerInitialized)
            {
                return;
            }

            if (_config.Platform == ChromelyPlatform.Windows)
            {
                var windowFrameless = _config.WindowOptions == null ? false : _config.WindowOptions.WindowFrameless;
                var framelessOption = _config.WindowOptions?.FramelessOption;

                if (windowFrameless &&
                    framelessOption != null &&
                    framelessOption.UseDefaultFramelessController)
                {
                    _isFramelessControllerInitialized = true;
                    _framelessController = new WindowMessageInterceptor(_config, _browserWindowHandle, _nativeHost);
                }
            }
        }
    }
}