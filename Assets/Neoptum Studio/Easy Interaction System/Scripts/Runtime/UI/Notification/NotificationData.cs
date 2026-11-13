using UnityEngine;

namespace EIS.Runtime.UI.Notification
{
    public class NotificationData
    {
        public Sprite icon;
        public string text;

        public Notifier.NotificationType notificationType = Notifier.NotificationType.LEFT_PANEL;

        public NotificationData()
        {
        }

        public NotificationData(Sprite icon, string text,
            Notifier.NotificationType notificationType = Notifier.NotificationType.LEFT_PANEL,
            string additionalText = "")
        {
            this.icon = icon;
            this.text = text;
            this.notificationType = notificationType;
        }
    }
}