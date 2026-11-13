using System.Collections.Generic;
using System.Linq;
using EIS.Runtime.Extensions;
using EIS.Runtime.Misc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EIS.Runtime.UI.Notification
{
    /// <summary>
    /// Singleton class responsible for managing notification popups in the UI.
    /// </summary>
    public partial class Notifier
    {
        /// <summary>
        /// List of notification prefab prefabs used for different notification types.
        /// </summary>
        [Tooltip("List of notification prefab prefabs used for different notification types")] [SerializeField]
        private List<NotificationPrefab> prefabs = new List<NotificationPrefab>();


        /// <summary>
        /// References to UI elements used for laying out notifications in different positions (left).
        /// </summary>
        [Space]
        [Tooltip("References to UI elements used for laying out notifications in different positions (left)")]
        [SerializeField]
        private Transform notificationLayoutGroupLeft;

        /// <summary>
        /// References to UI elements used for laying out notifications in different positions (right).
        /// </summary>
        [Tooltip("References to UI elements used for laying out notifications in different positions (right)")]
        [SerializeField]
        private Transform notificationLayoutGroupRight;

        /// <summary>
        /// References to UI elements used for laying out notifications in different positions (center).
        /// </summary>
        [Tooltip("References to UI elements used for laying out notifications in different positions (center)")]
        [SerializeField]
        private Transform notificationLayoutGroupCenter;


        /// <summary>
        /// The audio clip to play when a notification is shown.
        /// </summary>
        [Tooltip("The audio clip to play when a notification is shown")] [Space] [SerializeField]
        private AudioClip notificationAudio;

        /// <summary>
        /// The AudioSource component used to play the notification audio clip.
        /// </summary>
        [Tooltip("The AudioSource component used to play the notification audio clip")] [SerializeField]
        private AudioSource audioSource;

        /// <summary>
        /// List of default icons that can be referenced by notification data.
        /// </summary>
        [Tooltip("List of default icons that can be referenced by notification data")] [SerializeField]
        private List<DefaultIcons> defaultIconsList = new List<DefaultIcons>();

        /// <summary>
        /// Shows a notification popup based on the provided notification data and optional duration.
        /// </summary>
        /// <param name="notificationData">Data containing information about the notification to show.</param>
        /// <param name="duration">The duration in seconds for which the notification should be displayed (default 10f).</param>
        /// <returns>The GameObject instance of the spawned notification.</returns>
        /// <exception cref="ArgumentNullException">Thrown if notificationData is null.</exception>
        public GameObject ShowNotification(NotificationData notificationData, float duration = 10f)
        {
            Debug.Assert(notificationData != null, nameof(notificationData) + " != null");

            Transform layoutGroup = GetLayoutGroup(notificationData);
            GameObject prefab = GetPrefab(notificationData);

            GameObject spawn = Object.Instantiate(prefab, layoutGroup);

            spawn.transform.SetAsFirstSibling();

            spawn.transform.localScale = Vector3.one;
            CachedComponentProvider cachedComponentProvider = spawn.GetComponent<CachedComponentProvider>();

            SetIcon(notificationData, cachedComponentProvider);
            SetText(cachedComponentProvider, "text", notificationData.text);

            PlayAudio();

            Object.Destroy(spawn, duration);

            return spawn;
        }

        /// <summary>
        /// Plays the notification audio clip.
        /// </summary>
        private void PlayAudio()
        {
            audioSource.clip = notificationAudio;
            audioSource.Play();
        }

        /// <summary>
        /// Sets the text of a UI element within the notification prefab based on the provided text index and content.
        /// </summary>
        /// <param name="cachedComponentProvider">The CachedComponentProvider component from the notification prefab.</param>
        /// <param name="textIndex">The index used to identify the text element within the prefab.</param>
        /// <param name="textContent">The text content to set for the identified element.</param>
        private static void SetText(CachedComponentProvider cachedComponentProvider, string textIndex,
            string textContent)
        {
            if (textIndex.IsNullOrEmpty() || textContent.IsNullOrEmpty())
            {
                return;
            }

            TMP_Text text = cachedComponentProvider.GetComponentByIndex<TMP_Text>(textIndex);
            text.SetText(textContent);
        }


        /// <summary>
        /// Finds the prefab corresponding to the notification type specified in the notification data.
        /// </summary>
        /// <param name="notificationData">The data containing information about the notification.</param>
        /// <returns>The GameObject instance of the prefab for the specified notification type.</returns>
        private GameObject GetPrefab(NotificationData notificationData)
        {
            return prefabs
                .First(prb => prb.notificationType == notificationData.notificationType)
                .prefab;
        }

        /// <summary>
        /// Determines the appropriate Transform (parent UI element) for laying out the notification based on its type.
        /// </summary>
        /// <param name="notificationData">The data containing information about the notification.</param>
        /// <returns>The Transform representing the layout group for the specified notification type.</returns>
        private Transform GetLayoutGroup(NotificationData notificationData)
        {
            Transform layoutGroup = notificationData.notificationType switch
            {
                NotificationType.RIGHT_PANEL => notificationLayoutGroupRight,
                NotificationType.LEFT_PANEL => notificationLayoutGroupLeft,
                _ => notificationLayoutGroupCenter
            };

            return layoutGroup;
        }

        /// <summary>
        /// Sets the notification icon based on the provided data or finds it from the default icons list.
        /// </summary>
        /// <param name="notificationData">The data containing information about the notification.</param>
        /// <param name="cachedComponentProvider">The CachedComponentProvider component from the notification prefab.</param>
        private static void SetIcon(NotificationData notificationData, CachedComponentProvider cachedComponentProvider)
        {
            if (notificationData.icon != null)
            {
                Image image = cachedComponentProvider.GetComponentByIndex<Image>("image");
                image.sprite = notificationData.icon;
            }
        }

        /// <summary>
        /// Retrieves a default icon sprite based on the provided index.
        /// </summary>
        /// <param name="index">The string index used to identify the default icon.</param>
        /// <returns>The Sprite object representing the default icon, or null if not found.</returns>
        public Sprite GetIcon(string index)
        {
            DefaultIcons firstOrDefault = defaultIconsList
                .FirstOrDefault(icon => icon.index.Equals(index));

            if (firstOrDefault == null)
                return null;

            return firstOrDefault.sprite;
        }

        /// <summary>
        /// Hides a notification by destroying its GameObject instance.
        /// </summary>
        /// <param name="gameObject">The GameObject representing the notification to hide.</param>
        public static void HideNotification(GameObject gameObject)
        {
            Object.Destroy(gameObject);
        }
    }
}