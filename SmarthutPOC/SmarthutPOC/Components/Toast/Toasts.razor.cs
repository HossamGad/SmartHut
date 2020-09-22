using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using SmarthutPOC.Components.Toast.Configuration;
using SmarthutPOC.Components.Toast.Services;

namespace SmarthutPOC.Components.Toast
{
    public partial class Toasts
    {
        [Inject] private IToastService ToastService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Parameter] public string InfoClass { get; set; }
        [Parameter] public string InfoIcon { get; set; }
        [Parameter] public string SuccessClass { get; set; }
        [Parameter] public string SuccessIcon { get; set; }
        [Parameter] public string WarningClass { get; set; }
        [Parameter] public string WarningIcon { get; set; }
        [Parameter] public string ErrorClass { get; set; }
        [Parameter] public string ErrorIcon { get; set; }
        [Parameter] public ToastPosition Position { get; set; } = ToastPosition.TopRight;
        [Parameter] public int Timeout { get; set; } = 10;
        [Parameter] public bool RemoveToastsOnNavigation { get; set; } = true;
        [Parameter] public bool ShowProgressBar { get; set; }

        private string PositionClass { get; set; } = string.Empty;
        internal List<ToastInstance> ToastList { get; set; } = new List<ToastInstance>();

        protected override void OnInitialized()
        {
            ToastService.OnShow += ShowToast;

            if (RemoveToastsOnNavigation)
            {
                NavigationManager.LocationChanged += ClearToasts;
            }

            PositionClass = $"position-{Position.ToString().ToLower()}";
        }

        public void RemoveToast(Guid? toastId)
        {
            InvokeAsync(() =>
            {
                var toastInstance = ToastList.SingleOrDefault(t => t.Id == toastId);
                ToastList.Remove(toastInstance);
                StateHasChanged();
            });
        }

        private void ClearToasts(object sender, LocationChangedEventArgs e)
        {
            InvokeAsync(() =>
            {
                ToastList.Clear();
                StateHasChanged();
            });
        }

        private ToastSettings BuildToastSettings(ToastLevel level, RenderFragment message, string icon, Guid? id,
            string heading)
        {
            switch (level)
            {
                case ToastLevel.Error:
                    return new ToastSettings(string.IsNullOrWhiteSpace(heading) ? "Error" : heading, message,
                        "toast-error", ErrorClass, icon, ShowProgressBar);

                case ToastLevel.Info:
                    return new ToastSettings(string.IsNullOrWhiteSpace(heading) ? "Info" : heading, message,
                        "toast-info", InfoClass, icon, ShowProgressBar);

                case ToastLevel.Success:
                    return new ToastSettings(string.IsNullOrWhiteSpace(heading) ? "Success" : heading, message,
                        "toast-success", SuccessClass, icon, ShowProgressBar);

                case ToastLevel.Warning:
                    return new ToastSettings(string.IsNullOrWhiteSpace(heading) ? "Warning" : heading, message,
                        "toast-warning", WarningClass, icon, ShowProgressBar);
            }

            throw new InvalidOperationException();
        }

        private void ShowToast(ToastLevel level, RenderFragment message, string icon, Guid id, string heading)
        {
            InvokeAsync(() =>
            {
                var settings = BuildToastSettings(level, message, icon, id, heading);
                var toast = new ToastInstance
                {
                    Id = id,
                    TimeStamp = DateTime.Now.Date,
                    ToastSettings = settings
                };
                if (!ToastList.Exists(t => t.ToastSettings.Heading == heading))
                {
                    ToastList.Add(toast);
                }

                StateHasChanged();
            });
        }
    }
}