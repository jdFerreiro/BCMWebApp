﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using ObjCRuntime;
using UIKit;
using UserNotifications;

namespace BCMWeb.iOS
{
    public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            completionHandler(UNNotificationPresentationOptions.Alert);
        }
    }
}