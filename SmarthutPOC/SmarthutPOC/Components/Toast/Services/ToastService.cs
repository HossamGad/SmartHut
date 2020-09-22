using System;
using Microsoft.AspNetCore.Components;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace SmarthutPOC.Components.Toast.Services
{
    public class ToastService : IToastService
    {
        public event Action<ToastLevel, RenderFragment, string, Guid,string> OnShow;

        public void ShowInfo(string message, string icon, Guid id, string heading = "")
        {
            ShowToast(ToastLevel.Info, message, icon, id, heading);
        }

        public void ShowInfo(RenderFragment message, string icon, Guid id, string heading = "")
        {
            ShowToast(ToastLevel.Info, message, icon, id, heading);
        }

        public void ShowSuccess(string message, string icon, Guid id, string heading = "")
        {
            ShowToast(ToastLevel.Success, message, icon, id, heading);
        }

        public void ShowSuccess(RenderFragment message, string icon, Guid id, String heading = "")
        {
            ShowToast(ToastLevel.Success, message, icon, id, heading);
        }

        public void ShowWarning(string message, string icon, Guid id, string heading = "")
        {
            ShowToast(ToastLevel.Warning, message, icon, id, heading);
        }

        public void ShowWarning(RenderFragment message, string icon, Guid id, string heading = "")
        {
            ShowToast(ToastLevel.Warning, message, icon, id, heading);
        }

        public void ShowError(string message, string icon, Guid id, string heading = "")
        {
            ShowToast(ToastLevel.Error, message, icon, id, heading);
        }

        public void ShowError(RenderFragment message, string icon, Guid id, string heading = "")
        {
            ShowToast(ToastLevel.Error, message, icon, id, heading);
        }

        public void ShowToast(ToastLevel level, string message, string icon, Guid id, string heading = "")
        {
            ShowToast(level, builder => builder.AddContent(0, message), icon, id, heading);
        }

        public void ShowToast(ToastLevel level, RenderFragment message, string icon, Guid id, string heading = "")
        {
            OnShow?.Invoke(level, message, icon, id, heading);
        }
    }
}