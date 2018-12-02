﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefLifeSpanHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Winapi.Browser
{
    using Chromely.CefGlue.Winapi.ChromeHost;
    using Xilium.CefGlue;

    /// <summary>
    /// The CefGlue client.
    /// </summary>
    public class CefGlueClient : CefClient
    {
        /// <summary>
        /// The life span handler.
        /// </summary>
        private readonly CefLifeSpanHandler mLifeSpanHandler;

        /// <summary>
        /// The load handler.
        /// </summary>
        private readonly CefLoadHandler mLoadHandler;

        /// <summary>
        /// The request handler.
        /// </summary>
        private readonly CefRequestHandler mRequestHandler;

        /// <summary>
        /// The display handler.
        /// </summary>
        private readonly CefDisplayHandler mDisplayHandler;

        /// <summary>
        /// The context menu handler.
        /// </summary>
        private readonly CefContextMenuHandler mContextMenuHandler;

        /// <summary>
        /// The focus handler.
        /// </summary>
        private readonly CefFocusHandler mFocusHandler;

        /// <summary>
        /// The keyboard handler.
        /// </summary>
        private readonly CefKeyboardHandler mKeyboardHandler;

        /// <summary>
        /// The Javascript dialog handler.
        /// </summary>
        private readonly CefJSDialogHandler mJsDialogHandler;

        /// <summary>
        /// The dialog handler.
        /// </summary>
        private readonly CefDialogHandler mDialogHandler;

        /// <summary>
        /// The drag handler.
        /// </summary>
        private readonly CefDragHandler mDragHandler;

        /// <summary>
        /// The download handler.
        /// </summary>
        private readonly CefDownloadHandler mDownloadHandler;

        /// <summary>
        /// The find handler.
        /// </summary>
        private readonly CefFindHandler mFindHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueClient"/> class.
        /// </summary>
        /// <param name="clientParams">
        /// The client params.
        /// </param>
        public CefGlueClient(CefGlueClientParams clientParams)
        {
            this.Core = clientParams.Browser;
            this.mLifeSpanHandler = clientParams.LifeSpanHandler;
            this.mLoadHandler = clientParams.LoadHandler;
            this.mRequestHandler = clientParams.RequestHandler;
            this.mDisplayHandler = clientParams.DisplayHandler;
            this.mContextMenuHandler = clientParams.ContextMenuHandler;
            this.mFocusHandler = clientParams.FocusHandler;
            this.mKeyboardHandler = clientParams.KeyboardHandler;
            this.mJsDialogHandler = clientParams.JsDialogHandler;
            this.mDialogHandler = clientParams.DialogHandler;
            this.mDragHandler = clientParams.DragHandler;
            this.mDownloadHandler = clientParams.DownloadHandler;
            this.mFindHandler = clientParams.FindHandler;
        }

        /// <summary>
        /// Gets the core.
        /// </summary>
        protected CefGlueBrowser Core { get; }

        /// <summary>
        /// The get life span handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefLifeSpanHandler"/>.
        /// </returns>
        protected override CefLifeSpanHandler GetLifeSpanHandler()
        {
            return this.mLifeSpanHandler;
        }

        /// <summary>
        /// The get load handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefLoadHandler"/>.
        /// </returns>
        protected override CefLoadHandler GetLoadHandler()
        {
            return this.mLoadHandler;
        }

        /// <summary>
        /// The get request handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefRequestHandler"/>.
        /// </returns>
        protected override CefRequestHandler GetRequestHandler()
        {
            return this.mRequestHandler;
        }

        /// <summary>
        /// The get display handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefDisplayHandler"/>.
        /// </returns>
        protected override CefDisplayHandler GetDisplayHandler()
        {
            return this.mDisplayHandler;
        }

        /// <summary>
        /// The get context menu handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefContextMenuHandler"/>.
        /// </returns>
        protected override CefContextMenuHandler GetContextMenuHandler()
        {
            return this.mContextMenuHandler;
        }

        /// <summary>
        /// The get focus handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefFocusHandler"/>.
        /// </returns>
        protected override CefFocusHandler GetFocusHandler()
        {
            return this.mFocusHandler;
        }

        /// <summary>
        /// The get keyboard handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefKeyboardHandler"/>.
        /// </returns>
        protected override CefKeyboardHandler GetKeyboardHandler()
        {
            return this.mKeyboardHandler;
        }

        /// <summary>
        /// The get js dialog handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefJSDialogHandler"/>.
        /// </returns>
        protected override CefJSDialogHandler GetJSDialogHandler()
        {
            return this.mJsDialogHandler;
        }

        /// <summary>
        /// The get dialog handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefDialogHandler"/>.
        /// </returns>
        protected override CefDialogHandler GetDialogHandler()
        {
            return this.mDialogHandler;
        }

        /// <summary>
        /// The get drag handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefDragHandler"/>.
        /// </returns>
        protected override CefDragHandler GetDragHandler()
        {
            return this.mDragHandler;
        }

        /// <summary>
        /// The get download handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefDownloadHandler"/>.
        /// </returns>
        protected override CefDownloadHandler GetDownloadHandler()
        {
            return this.mDownloadHandler;
        }

        /// <summary>
        /// The get find handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefFindHandler"/>.
        /// </returns>
        protected override CefFindHandler GetFindHandler()
        {
            return this.mFindHandler;
        }

        /// <summary>
        /// The on process message received.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="sourceProcess">
        /// The source process.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool OnProcessMessageReceived(CefBrowser browser, CefProcessId sourceProcess, CefProcessMessage message)
        {
            return CefGlueBrowserHost.BrowserMessageRouter.OnProcessMessageReceived(browser, sourceProcess, message);
        }
    }
}
