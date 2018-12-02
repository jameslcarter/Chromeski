﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueResourceSchemeHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Winapi.Browser.Handlers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using Chromely.Core.Helpers;
    using Chromely.Core.Infrastructure;
    using Xilium.CefGlue;

    /// <summary>
    /// The CefGlue resource scheme handler.
    /// </summary>
    public class CefGlueResourceSchemeHandler : CefResourceHandler
    {
        /// <summary>
        /// The file read in bytes.
        /// </summary>
        private byte[] mFileBytes;

        /// <summary>
        /// The mime type.
        /// </summary>
        private string mMime;

        /// <summary>
        /// The completed flag.
        /// </summary>
        private bool mCompleted;

        /// <summary>
        /// The total bytes read.
        /// </summary>
        private int mTotalBytesRead;

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
            var u = new Uri(request.Url);
            var file = u.Authority + u.AbsolutePath;

            this.mTotalBytesRead = 0;
            this.mFileBytes = null;
            this.mCompleted = false;

            if (File.Exists(file))
            {
                Task.Run(() =>
                {
                    using (callback)
                    {
                        try
                        {
                            this.mFileBytes = File.ReadAllBytes(file);

                            string extension = Path.GetExtension(file);
                            this.mMime = MimeMapper.GetMimeType(extension);
                        }
                        catch (Exception exception)
                        {
                            Log.Error(exception);
                        }
                        finally
                        {
                            callback.Continue();
                        }
                    }
                });

                return true;
            }

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
                var headers = response.GetHeaderMap();
                headers.Add("Access-Control-Allow-Origin", "*");
                response.SetHeaderMap(headers);

                response.Status = (int)HttpStatusCode.OK;
                response.MimeType = this.mMime;
                response.StatusText = "OK";
            }
            catch (Exception exception)
            {
                response.Status = (int)HttpStatusCode.BadRequest; 
                response.MimeType = "text/plain";
                response.StatusText = "Resource loading error.";

                Log.Error(exception);
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
                if (this.mCompleted)
                {
                    bytesRead = 0;
                    this.mTotalBytesRead = 0;
                    this.mFileBytes = null;
                    return false;
                }
                else
                {
                    if (this.mFileBytes != null)
                    {
                        currBytesRead = Math.Min(this.mFileBytes.Length - this.mTotalBytesRead, bytesToRead);
                        response.Write(this.mFileBytes, this.mTotalBytesRead, currBytesRead);
                        this.mTotalBytesRead += currBytesRead;

                        if (this.mTotalBytesRead >= this.mFileBytes.Length)
                        {
                            this.mCompleted = true;
                        }
                    }
                    else
                    {
                        bytesRead = 0;
                        this.mCompleted = true;
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
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
    }
}
