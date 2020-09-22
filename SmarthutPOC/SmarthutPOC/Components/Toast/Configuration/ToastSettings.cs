using System;
using Microsoft.AspNetCore.Components;

namespace SmarthutPOC.Components.Toast.Configuration
{
    public class ToastSettings
    {
        public string Heading { get; set; }
        public RenderFragment Message { get; set; }
        public string BaseClass { get; set; }
        public string AdditionalClasses { get; set; }
        public string Icon { get; set; }
        public bool ShowProgressBar { get; set; }

        public ToastSettings(string heading, RenderFragment message, string baseClass, string additionalClasses,
            string icon, bool showProgressBar)
        {
            Heading = heading;
            Message = message;
            BaseClass = baseClass;
            AdditionalClasses = additionalClasses;
            Icon = icon;
            ShowProgressBar = showProgressBar;
        }

        
    }
}