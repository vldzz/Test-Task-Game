using UnityEngine;

namespace EIS.Runtime.UI.Notification
{
    /// <summary>
    /// Builder class used to create and configure notification data in a fluent manner.
    /// </summary>
    public partial class Notifier : MonoBehaviour
    {
        public static Notifier Instance;

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Resets the singleton instance when this GameObject is destroyed.
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }


        public class Builder
        {
            private NotificationData notificationData;

            Builder()
            {
                notificationData = new NotificationData();
            }

            public static Builder CreateNew()
            {
                return new Builder();
            }

            public Builder SetIcon(Sprite icon)
            {
                notificationData.icon = icon;
                return this;
            }

            public Builder SetIcon(string iconIndex)
            {
                notificationData.icon = Instance.GetIcon(iconIndex);
                return this;
            }

            public Builder SetText(string text)
            {
                notificationData.text = text;
                return this;
            }


            public Builder SetNotificationType(NotificationType notificationType)
            {
                notificationData.notificationType = notificationType;
                return this;
            }

            public GameObject ShowNotification(float duration = 10f)
            {
                return Instance.ShowNotification(notificationData, duration);
            }
        }
    }
}