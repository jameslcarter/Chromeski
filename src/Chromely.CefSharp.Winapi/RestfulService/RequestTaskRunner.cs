﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestTaskRunner.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable StyleCop.SA1210
namespace Chromely.CefSharp.Winapi.RestfulService
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Chromely.Core.Infrastructure;
    using Chromely.Core.RestfulService;

    using global::CefSharp;

    /// <summary>
    /// The request task runner.
    /// </summary>
    public static class RequestTaskRunner
    {
        /// <summary>
        /// The run async.
        /// </summary>
        /// <param name="requestId">
        /// The request identifier.
        /// </param>
        /// <param name="routePath">
        /// The route routePath.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="postData">
        /// The post data.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static Task<ChromelyResponse> RunAsync(string requestId, RoutePath routePath, object parameters, object postData)
        {
            var response = new ChromelyResponse(requestId);
            if (string.IsNullOrEmpty(routePath.Path))
            {
                response.ReadyState = (int)ReadyState.ResponseIsReady;
                response.Status = (int)System.Net.HttpStatusCode.BadRequest;
                response.StatusText = "Bad Request";

                return Task.FromResult(response);
            }

            if (routePath.Path.ToLower().Equals("/info"))
            {
                response = GetInfo();
                return Task.FromResult(response);
            }

            response = ExcuteRoute(requestId, routePath, parameters, postData);
            return Task.FromResult(response);
        }

        /// <summary>
        /// The run.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// Generic exception - Route path not valid.
        /// </exception>
        public static ChromelyResponse Run(IRequest request)
        {
            bool isCustomScheme = UrlSchemeProvider.IsUrlOfRegisteredCustomScheme(request.Url);

            if (!isCustomScheme)
            {
                throw new Exception($"Url {request.Url} is not of a registered custom scheme.");
            }

            var uri = new Uri(request.Url);
            string path = uri.LocalPath;
            var parameters = request.Url.GetParameters();
            object postData = GetPostData(request);

            var routePath = new RoutePath(request.Method, path);
            return Run(string.Empty, routePath, parameters, postData);
        }

        /// <summary>
        /// The run.
        /// </summary>
        /// <param name="requestId">
        /// The request identifier.
        /// </param>
        /// <param name="routePath">
        /// The route path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="postData">
        /// The post data.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        public static ChromelyResponse Run(string requestId, RoutePath routePath, object parameters, object postData)
        {
            var response = new ChromelyResponse(requestId);
            if (string.IsNullOrEmpty(routePath.Path))
            {
                response.ReadyState = (int)ReadyState.ResponseIsReady;
                response.Status = (int)System.Net.HttpStatusCode.BadRequest;
                response.StatusText = "Bad Request";

                return response;
            }

            if (routePath.Path.ToLower().Equals("/info"))
            {
                return GetInfo();
            }

            return ExcuteRoute(requestId, routePath, parameters, postData);
        }

        /// <summary>
        /// The excute route.
        /// </summary>
        /// <param name="requestId">
        /// The request identifier.
        /// </param>
        /// <param name="routePath">
        /// The route path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="postData">
        /// The post data.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// Generic exception - Route path not valid.
        /// </exception>
        private static ChromelyResponse ExcuteRoute(string requestId, RoutePath routePath, object parameters, object postData)
        {
            var route = ServiceRouteProvider.GetRoute(routePath);

            if (route == null)
            {
                throw new Exception($"Route for path = {routePath} is null or invalid.");
            }

            var response = route.Invoke(requestId, routePath, parameters: parameters?.ToObjectDictionary(), postData: postData);
            response.ReadyState = (int)ReadyState.ResponseIsReady;
            response.Status = (int)System.Net.HttpStatusCode.OK;
            response.StatusText = "OK";

            return response;
        }

        /// <summary>
        /// The get info.
        /// </summary>
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        private static ChromelyResponse GetInfo()
        {
            var response = new ChromelyResponse();

            var bitness = Environment.Is64BitProcess ? "x64" : "x86";
            string chromeVersion =
                $"Chromium: {Cef.ChromiumVersion}, CEF: {Cef.CefVersion}, CefSharp: {Cef.CefSharpVersion}, Environment: {bitness}";

            var infoItemDic = new Dictionary<string, string>
            {
                {
                    "divObjective",
                    "To build HTML5 desktop apps using embedded Chromium without WinForm or WPF. Uses Windows and Linux native GUI API. It can be extended to use WinForm or WPF. Main form of communication with Chromium rendering process is via Ajax HTTP/XHR requests using custom schemes and domains (CefGlue, CefSharp) and .NET/Javascript integration (CefSharp)."
                },
                {
                    "divPlatform",
                    "Cross-platform - Windows, Linux. Built on CefGlue, CefSharp, NET Standard 2.0, .NET Core 2.0, .NET Framework 4.61 and above."
                },
                { "divVersion", chromeVersion }
            };

            response.ReadyState = (int)ReadyState.ResponseIsReady;
            response.Status = (int)System.Net.HttpStatusCode.OK;
            response.StatusText = "OK";
            response.Data = infoItemDic;

            return response;
        }

        /// <summary>
        /// The get post data.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetPostData(IRequest request)
        {
            var elements = request?.PostData?.Elements;

            if (elements == null || (elements.Count == 0))
            {
                return string.Empty;
            }

            var dataElement = elements[0];
            return dataElement.GetBody();
        }
    }
}