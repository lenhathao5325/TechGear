namespace TechGear.Models
{
    public enum AlertType { Success, Info, Warning }
    public class AlertModel
    {
        public AlertType Type { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}