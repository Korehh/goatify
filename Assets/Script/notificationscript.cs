using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.UI;


public class notificationscript : MonoBehaviour
{
    public InputField timeInputField; // Reference to the input field for setting the notification time
    public Button startButton; // Reference to the button for triggering the notification

    private bool notificationStarted = false; // Flag to check if the notification has been started

    void Start()
    {
        // Add an event listener to the start button
        startButton.onClick.AddListener(StartNotification);
    }

    private void StartNotification()
    {
        if (notificationStarted)
        {
            Debug.Log("Notification already started.");
            return;
        }

        // Remove any previously displayed notifications
        AndroidNotificationCenter.CancelAllDisplayedNotifications();

        // Create the Android notification channel
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Notification Channel",
            Importance = Importance.Default,
            Description = "Reminder notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        // Set the desired notification time based on user input
        string timeString = timeInputField.text;
        System.DateTime notificationTime = System.DateTime.Today;
        if (!string.IsNullOrEmpty(timeString))
        {
            System.DateTime time = System.DateTime.ParseExact(timeString, "h:mm tt", null);
            notificationTime = new System.DateTime(
                notificationTime.Year,
                notificationTime.Month,
                notificationTime.Day,
                time.Hour,
                time.Minute,
                time.Second
            );
        }

        // Create the notification
        var notification = new AndroidNotification();
        notification.Title = "Hey! Gutom na goat mo";
        notification.Text = "Pakainin mo na kasi, wag mo na patagalan!!!!";
        notification.FireTime = notificationTime;

        // Send the notification
        var id = AndroidNotificationCenter.SendNotification(notification, "channel_id");

        // If a notification is already scheduled, cancel it and reschedule another one
        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(id) == NotificationStatus.Scheduled)
        {
            AndroidNotificationCenter.CancelAllNotifications();
            AndroidNotificationCenter.SendNotification(notification, "channel_id");
        }

        notificationStarted = true;
        Debug.Log("Notification started for " + notificationTime.ToString("h:mm tt"));
    }
}