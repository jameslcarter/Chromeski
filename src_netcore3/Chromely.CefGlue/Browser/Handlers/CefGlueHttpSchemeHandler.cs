﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueHttpSchemeHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Chromely.Core;
using Chromely.Core.Infrastructure;
using Chromely.Core.RestfulService;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.Handlers
{
    /// <summary>
    /// The CefGlue http scheme handler.
    /// </summary>
    public class CefGlueHttpSchemeHandler : CefResourceHandler
    {
        private readonly IChromelyConfiguration _config;
        private readonly IChromelyRequestTaskRunner _requestTaskRunner;

        /// <summary>
        /// The ChromelyResponse object.
        /// </summary>
        private ChromelyResponse _chromelyResponse;

        /// <summary>
        /// The response in bytes.
        /// </summary>
        private byte[] _responseBytes;

        /// <summary>
        /// The completed flag.
        /// </summary>
        private bool _completed;

        /// <summary>
        /// The total bytes read.
        /// </summary>
        private int _totalBytesRead;

        public CefGlueHttpSchemeHandler(IChromelyConfiguration config, IChromelyRequestTaskRunner requestTaskRunner)
        {
            _config = config;
            _requestTaskRunner = requestTaskRunner;
        }

        /// <summary>
        /// The process request.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool ProcessRequest(CefRequest request, CefCallback callback)
        {
            var isCustomScheme = _config?.UrlSchemes?.IsUrlRegisteredCustomScheme(request.Url);
            if (isCustomScheme.HasValue && isCustomScheme.Value)
            {
                Task.Run(() =>
                {
                    using (callback)
                    {
                        try
                        {
                            var uri = new Uri(request.Url);
                            var path = uri.LocalPath;

                            var response = new ChromelyResponse();
                            if (string.IsNullOrEmpty(path))
                            {
                                response.ReadyState = (int)ReadyState.ResponseIsReady;
                                response.Status = (int)System.Net.HttpStatusCode.BadRequest;
                                response.StatusText = "Bad Request";

                                _chromelyResponse = response;
                            }
                            else
                            {
                                var parameters = request.Url.GetParameters();
                                var postData = GetPostData(request);

                                _chromelyResponse = _requestTaskRunner.Run(request.Method, path, parameters, postData);
                                string jsonData = _chromelyResponse.Data.EnsureResponseIsJsonFormat();
                                _responseBytes = Encoding.UTF8.GetBytes(jsonData);
                            }
                        }
                        catch (Exception exception)
                        {
                            Logger.Instance.Log.Error(exception);

                            _chromelyResponse =
                                new ChromelyResponse
                                    {
                                        Status = (int)HttpStatusCode.BadRequest,
                                        Data = "An error occured."
                                    };
                        }
                        finally
                        {
                            callback.Continue();
                        }
                    }
                });

                return true;
            }

            Logger.Instance.Log.Error($"Url {request.Url} is not of a registered custom scheme.");
            callback.Dispose();
            return false;
        }

        /// <summary>
        /// The get response headers.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <param name="responseLength">
        /// The response length.
        /// </param>
        /// <param name="redirectUrl">
        /// The redirect url.
        /// </param>
        protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
        {
            // unknown content-length
            // no-redirect
            responseLength = -1;
            redirectUrl = null;

            try
            {
                HttpStatusCode status = (_chromelyResponse != null) ? (HttpStatusCode)_chromelyResponse.Status : HttpStatusCode.BadRequest;
                string errorStatus = (_chromelyResponse != null) ? _chromelyResponse.Data.ToString() : "Not Found";

                var headers = response.GetHeaderMap();
                headers.Add("Cache-Control", "private");
                headers.Add("Access-Control-Allow-Origin", "*");
                headers.Add("Access-Control-Allow-Methods", "GET,POST");
                headers.Add("Access-Control-Allow-Headers", "Content-Type");
                headers.Add("Content-Type", "application/json; charset=utf-8");
                response.SetHeaderMap(headers);

                response.Status = (int)status;
                response.MimeType = "application/json";
                response.StatusText = (status == HttpStatusCode.OK) ? "OK" : errorStatus;
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error(exception);
            }
        }

        /// <summary>
        /// The read response.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <param name="bytesToRead">
        /// The bytes to read.
        /// </param>
        /// <param name="bytesRead">
        /// The bytes read.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool ReadResponse(Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
        {
            int currBytesRead = 0;

            try
            {
                if (_completed)
                {
                    bytesRead = 0;
                    _totalBytesRead = 0;
                    _responseBytes = null;
                    return false;
                }
                else
                {
                    if (_responseBytes != null)
                    {
                        currBytesRead = Math.Min(_responseBytes.Length - _totalBytesRead, bytesToRead);
                        response.Write(_responseBytes, _totalBytesRead, currBytesRead);
                        _totalBytesRead += currBytesRead;

                        if (_totalBytesRead >= _responseBytes.Length)
                        {
                            _completed = true;
                        }
                    }
                    else
                    {
                        bytesRead = 0;
                        _completed = true;
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error(exception);
            }

            bytesRead = currBytesRead;
            return true;
        }

        /// <summary>
        /// The can get cookie.
        /// </summary>
        /// <param name="cookie">
        /// The cookie.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool CanGetCookie(CefCookie cookie)
        {
            return true;
        }

        /// <summary>
        /// The can set cookie.
        /// </summary>
        /// <param name="cookie">
        /// The cookie.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool CanSetCookie(CefCookie cookie)
        {
            return true;
        }

        /// <summary>
        /// The cancel.
        /// </summary>
        protected override void Cancel()
        {
        }

        private static string GetPostData(CefRequest request)
        {
            var postDataElements = request?.PostData?.GetElements();
            if (postDataElements == null || (postDataElements.Length == 0))
            {
                return string.Empty;
            }

            var dataElement = postDataElements[0];

            switch (dataElement.ElementType)
            {
                case CefPostDataElementType.Empty:
                    break;
                case CefPostDataElementType.File:
                    break;
                case CefPostDataElementType.Bytes:
                    return Encoding.UTF8.GetString(dataElement.GetBytes());
            }

            return string.Empty;
        }
    }
}