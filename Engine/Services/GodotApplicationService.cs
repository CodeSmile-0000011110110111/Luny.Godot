using Luny.Engine.Services;
using System;
using Native = Godot;

namespace Luny.Godot.Engine.Services
{
	/// <summary>
	/// Godot implementation of application control provider.
	/// </summary>
	public sealed class GodotApplicationService : LunyApplicationServiceBase, ILunyApplicationService
	{
		public Boolean IsEditor => Native.Engine.IsEditorHint();

		public Boolean IsPlaying => !Native.Engine.IsEditorHint();

		public void Quit(Int32 exitCode = 0)
		{
			var tree = Native.Engine.GetMainLoop();
			// play nice since Godot doesn't post the close request notification by itself
			tree.Root.PropagateNotification(Native.Node.NotificationWMCloseRequest);
			// prefer deferred call - we don't know when and where user may call it
			tree.CallDeferred("quit", exitCode);
		}
	}
}
