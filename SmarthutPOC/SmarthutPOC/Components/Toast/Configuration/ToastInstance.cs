using System;

namespace SmarthutPOC.Components.Toast.Configuration
{
    public class ToastInstance
    {
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public ToastSettings ToastSettings { get; set; }
    }
}