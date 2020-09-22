using System;
using System.Threading;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Protocol;
using SmarthutPOC.Components.Toast.Configuration;
using SmarthutPOC.Helpers;

namespace SmarthutPOC.Components.Toast
{
    public partial class Toast : IDisposable
    {
        [CascadingParameter] private Toasts ToastsContainer { get; set; }
        [Parameter] public Guid? ToastId { get; set; }
        [Parameter] public ToastSettings ToastSettings { get; set; }
        [Parameter] public int Timeout { get; set; }

        private CountdownTimer _countdownTimer;
        private int _progress = 100;

        protected override void OnInitialized()
        {
            _countdownTimer = new CountdownTimer(Timeout);
            _countdownTimer.OnTick += CalculateProgress;
            _countdownTimer.OnElapsed += () => { Close(); };
            _countdownTimer.Start();
        }

        private void Close()
        {
            ToastsContainer.RemoveToast(ToastId);
        }

        private async void CalculateProgress(int percentComplete)
        {
            _progress = 100 - percentComplete;
            await InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            _countdownTimer.Dispose();
            _countdownTimer = null;
        }
    }
}