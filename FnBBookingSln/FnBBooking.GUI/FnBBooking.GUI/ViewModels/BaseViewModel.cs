namespace FnBBooking.GUI.ViewModels
{
    public class BaseViewModel
    {
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}

