﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueClientParams.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using Chromely.CefGlue.Browser.Handlers;
using Chromely.Core;
using Chromely.Core.Configuration;
using Chromely.Core.Logging;
using Chromely.Core.Network;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser
{
    public class CefGlueCustomHandlers
    {
        public CefLifeSpanHandler LifeSpanHandler { get; set; }
        public CefLoadHandler LoadHandler { get; set; }
        public CefRequestHandler RequestHandler { get; set; }
        public CefDisplayHandler DisplayHandler { get; set; }
        public CefContextMenuHandler ContextMenuHandler { get; set; }
        public CefFocusHandler FocusHandler { get; set; }
        public CefKeyboardHandler KeyboardHandler { get; set; }
        public CefJSDialogHandler JsDialogHandler { get; set; }
        public CefDialogHandler DialogHandler { get; set; }
        public CefDragHandler DragHandler { get; set; }
        public CefDownloadHandler DownloadHandler { get; set; }
        public CefFindHandler FindHandler { get; set; }

        public static CefGlueCustomHandlers Parse(IChromelyContainer container, IChromelyConfiguration config, IChromelyCommandTaskRunner commandTaskRunner, CefGlueBrowser browser)
        {
            var handlers = new CefGlueCustomHandlers();
            try
            {
                // Set default handlers
                handlers.LifeSpanHandler = new CefGlueLifeSpanHandler(config, commandTaskRunner, browser);
                handlers.LoadHandler = new CefGlueLoadHandler(config, browser);
                handlers.RequestHandler = new CefGlueRequestHandler(config, commandTaskRunner, browser);
                handlers.DisplayHandler = new CefGlueDisplayHandler(config, browser);
                handlers.ContextMenuHandler = new CefGlueContextMenuHandler(config);
                handlers.DragHandler = new CefGlueDragHandler(config);

                // Update custom handlers
                var customHandlers = container.GetAllInstances(typeof(IChromelyCustomHandler));
                if (customHandlers != null && customHandlers.Any())
                {
                    foreach (var handler in customHandlers)
                    {
                        if (handler is CefLifeSpanHandler spanHandler) 
                        { 
                            if (spanHandler is CefGlueLifeSpanHandler)
                            {
                                ((CefGlueLifeSpanHandler)spanHandler).Browser = browser;
                            }
                            handlers.LifeSpanHandler = spanHandler; 
                        }

                        if (handler is CefLoadHandler loadHandler) 
                        {
                            if (loadHandler is CefGlueLoadHandler)
                            {
                                ((CefGlueLoadHandler)loadHandler).Browser = browser;
                            }
                            handlers.LoadHandler = loadHandler; 
                        }

                        if (handler is CefRequestHandler requestHandler) 
                        {
                            if (requestHandler is CefGlueRequestHandler)
                            {
                                ((CefGlueRequestHandler)requestHandler).Browser = browser;
                            }
                            handlers.RequestHandler = requestHandler; 
                        }

                        if (handler is CefDisplayHandler displayHandler) 
                        {
                            if (displayHandler is CefGlueDisplayHandler)
                            {
                                ((CefGlueDisplayHandler)displayHandler).Browser = browser;
                            }
                            handlers.DisplayHandler = displayHandler; 
                        }

                        if (handler is CefContextMenuHandler menuHandler) { handlers.ContextMenuHandler = menuHandler; }
                        if (handler is CefFocusHandler focusHandler) { handlers.FocusHandler = focusHandler; }
                        if (handler is CefKeyboardHandler keyboardHandler) { handlers.KeyboardHandler = keyboardHandler; }
                        if (handler is CefJSDialogHandler jsDialogHandler) { handlers.JsDialogHandler = jsDialogHandler; }
                        if (handler is CefDialogHandler dialogHandler) { handlers.DialogHandler = dialogHandler; }
                        if (handler is CefDragHandler dragHandler) { handlers.DragHandler = dragHandler; }
                        if (handler is CefDownloadHandler downloadHandler) { handlers.DownloadHandler = downloadHandler; }
                        if (handler is CefFindHandler cefFinderhandler) { handlers.FindHandler = cefFinderhandler; }
                    }
                }

            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error(exception);
            }

            return handlers;
        }
    }
}
