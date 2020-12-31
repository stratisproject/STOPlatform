namespace Stratis.STOPlatform.Models
{
    public class AlertMessage
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public AlertType Type { get; set; } = AlertType.Success;
        public string ClassValue
        {
            get
            {
                switch (Type)
                {
                    case AlertType.Success: return "green white-text";
                    case AlertType.Info: return "blue white-text";
                    case AlertType.Danger:
                    case AlertType.Warning: return "red white-text";
                    default: return null;
                }
            }
        }
        public bool Dismissable { get; set; } = true;

        public string Icon
        {
            get
            {
                switch (Type)
                {
                    case AlertType.Success: return "check";
                    case AlertType.Info: return "info";
                    case AlertType.Danger:
                    case AlertType.Warning: return "error";
                    default: return null;
                }

            }
        }
    }
}

public enum AlertType
{
    Success, Info, Warning, Danger
}

