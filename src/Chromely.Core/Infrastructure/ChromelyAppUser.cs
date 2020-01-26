﻿namespace Chromely.Core.Infrastructure
{
    public abstract class ChromelyAppUser
    {
        private static ChromelyAppUser instance;
        public static ChromelyAppUser App
        {
            get
            {
                if (instance == null)
                {
                    //Ambient Context can't return null, so we assign Local Default
                    instance = new CurrentAppSettings();
                }

                return instance;
            }
            set
            {
                instance = (value == null) ? new CurrentAppSettings() : value;
            }
        }

        public virtual IChromelyAppSettings Properties { get; set; }
    }
}

