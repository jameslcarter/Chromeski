﻿// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Browser;

/// <summary>
/// Default implementation of <see cref="CefLifeSpanHandler"/>.
/// </summary>
public class DefaultLifeSpanHandler : CefLifeSpanHandler
{
    protected readonly IChromelyConfiguration _config;
    protected readonly IChromelyRequestHandler _requestHandler;
    protected readonly IChromelyRouteProvider _routeProvider;
    protected ChromiumBrowser? _browser;

    /// <summary>
    /// Initializes a new instance of <see cref="DefaultLifeSpanHandler"/>.
    /// </summary>
    /// <param name="config">Instance of <see cref="IChromelyConfiguration"/>.</param>
    /// <param name="requestHandler">Instance of <see cref="IChromelyRequestHandler"/>.</param>
    /// <param name="routeProvider">Instance of <see cref="IChromelyRouteProvider"/>.</param>
    /// <param name="window">Instance of <see cref="IChromelyWindow"/>.</param>
    public DefaultLifeSpanHandler(IChromelyConfiguration config,
                                  IChromelyRequestHandler requestHandler,
                                  IChromelyRouteProvider routeProvider,
                                  IChromelyWindow window)
    {
        _config = config;
        _requestHandler = requestHandler;
        _routeProvider = routeProvider;
        _browser = window as ChromiumBrowser;
    }

    /// <summary>
    /// Gets or sets the browser.
    /// </summary>
    public ChromiumBrowser? Browser
    {
        get { return _browser; }
        set { _browser = value; }
    }

    /// <inheritdoc/>
    protected override void OnAfterCreated(CefBrowser browser)
    {
        base.OnAfterCreated(browser);

        if (_browser is not null)
        {
            _browser.InvokeAsyncIfPossible(() => _browser.OnBrowserAfterCreated(browser));
        }
    }

    /// <inheritdoc/>
    protected override bool DoClose(CefBrowser browser)
    {
        return false;
    }

    /// <inheritdoc/>
    protected override void OnBeforeClose(CefBrowser browser)
    {
        if (_browser is not null)
        {
            _browser.InvokeAsyncIfPossible(() => _browser.OnBeforeClose());
        }
    }

    /// <inheritdoc/>
    protected override bool OnBeforePopup(CefBrowser browser, CefFrame frame, string targetUrl, string targetFrameName, CefWindowOpenDisposition targetDisposition, bool userGesture, CefPopupFeatures popupFeatures, CefWindowInfo windowInfo, ref CefClient client, CefBrowserSettings settings, ref CefDictionaryValue extraInfo, ref bool noJavascriptAccess)
    {
        if (_browser is not null)
        {
            _browser.InvokeAsyncIfPossible(() => _browser.OnBeforePopup(new BeforePopupEventArgs(frame, targetUrl, targetFrameName)));
        }

        if (_config is not null)
        {
            var isUrlExternal = _config.UrlSchemes?.IsUrlRegisteredExternalBrowserScheme(targetUrl);
            if (isUrlExternal.HasValue && isUrlExternal.Value)
            {
                BrowserLauncher.Open(_config.Platform, targetUrl);
                return true;
            }
        }

        // Sample: http://chromely.com/democontroller/showdevtools 
        // Expected to execute controller route action without return value
        var route = _routeProvider.GetRoute(targetUrl);
        if (route is not null && !route.HasReturnValue)
        {
            _requestHandler.Execute(targetUrl);
            return true;
        }

        return false;
    }
}
