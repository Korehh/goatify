using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class notificationscript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {   // Remove notification displayed
            AndroidNotificationCenter.CancelAllDisplayedNotifications();
        // Create the android notification channel to send message through
        var channel = new AndroidNotificationChannel()
                {
                    Id = "channel_id",
                    Name = "Notification Channel",
                    Importance = Importance.Default,
                    Description = "Reminder notifications",
                };
                AndroidNotificationCenter.RegisterNotificationChannel(channel);

                // Create the notification that is going to be
                var notification = new AndroidNotification();
                notification.Title = "Hey! Gutom na goat mo  ";
                notification.Text = "pakainin mo na kasi , wag mo na patagalan!!!!";
                notification.FireTime = System.DateTime.Now.AddSeconds(5);

                //Send The Notification
                var id = AndroidNotificationCenter.SendNotification(notification, "channel_id");

                //if the script is run and mesage is already schedule ,  cancel it and re=sched another message
                if(AndroidNotificationCenter.CheckScheduledNotificationStatus(id) == NotificationStatus.Scheduled)
                {
                    AndroidNotificationCenter.CancelAllNotifications();
                    AndroidNotificationCenter.SendNotification(notification,"channel_id");
                }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
