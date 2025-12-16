using Godot;
using System;

namespace Luny.Godot
{
	public static class GodotHelper
	{
		public static String ToNotificationString(Int64 notificationId) => notificationId switch
		{
			Node.NotificationCrash => nameof(Node.NotificationCrash),
			Node.NotificationWMCloseRequest => nameof(Node.NotificationWMCloseRequest),
			var _ => notificationId.ToString(),
		};
	}
}
