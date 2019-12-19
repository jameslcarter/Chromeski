﻿namespace Chromely.Core.Configuration
{
    public class CefDownloadOptions : ICefDownloadOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether Cef binaries should automatically be downloaded when missing.
        /// </summary>
        public bool AutoDownloadWhenMissing { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether  Cef binaries should be downloaded silently.
        /// </summary>
        public bool DownloadSilently { get; set; }

        public CefDownloadOptions()
        {
            AutoDownloadWhenMissing = true;
            DownloadSilently = false;
        }

        public CefDownloadOptions(bool autoDownload, bool silentDownload)
        {
            AutoDownloadWhenMissing = autoDownload;
            DownloadSilently = silentDownload;
        }
    }
}