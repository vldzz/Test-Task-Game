using System;
using UnityEngine;

namespace EIS.Runtime.UI.Notification
{
    public partial class Notifier
    {
        [Serializable]
        public class DefaultIcons
        {
            public string index;
            public Sprite sprite;
        }

        [Serializable]
        public class NotificationPrefab
        {
            public NotificationType notificationType;
            public GameObject prefab;
        }

        public enum NotificationType
        {
            LEFT_PANEL,
            CENTER_PANEL,
            RIGHT_PANEL
        }
    }
}