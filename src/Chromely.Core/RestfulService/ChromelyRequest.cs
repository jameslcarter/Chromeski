﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyRequest.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.Core.RestfulService
{
    using System.Collections.Generic;

    using LitJson;

    /// <summary>
    /// The chromely request.
    /// </summary>
    public class ChromelyRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyRequest"/> class.
        /// </summary>
        /// <param name="routePath">
        /// The route path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="postData">
        /// The post data.
        /// </param>
        public ChromelyRequest(RoutePath routePath, IDictionary<string, object> parameters, object postData)
        {
            RoutePath = routePath;
            Parameters = parameters;
            PostData = postData;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyRequest"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
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
        public ChromelyRequest(string id, RoutePath routePath, IDictionary<string, object> parameters, object postData)
        {
            Id = id;
            RoutePath = routePath;
            Parameters = parameters;
            PostData = postData;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyRequest"/> class.
        /// </summary>
        /// <param name="jsonData">
        /// The json data.
        /// </param>
        public ChromelyRequest(JsonData jsonData)
        {
            var method = jsonData.Keys.Contains("method") ? jsonData["method"].ToString() : "get";
            var url = jsonData.Keys.Contains("url") ? jsonData["url"].ToString() : string.Empty;
            RoutePath = new RoutePath(method, url);

            Id = jsonData.Keys.Contains("id") ? jsonData["id"].ToString() : Id;
            Parameters = jsonData.Keys.Contains("parameters") ? jsonData["parameters"]?.ObjectToDictionary() : Parameters;
            PostData = jsonData.Keys.Contains("postData") ? jsonData["postData"] : PostData;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the route path.
        /// </summary>
        public RoutePath RoutePath { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        public IDictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the post data.
        /// </summary>
        public object PostData { get; set; }
    }
}
