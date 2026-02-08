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
		public override Boolean IsEditor => Native.Engine.IsEditorHint();

		public override Boolean IsPlaying => !Native.Engine.IsEditorHint();

		public override void Quit(Int32 exitCode = 0)
		{
			var tree = (Native.SceneTree)Native.Engine.GetMainLoop();
			// play nice since Godot doesn't post the close request notification by itself
			tree.Root.PropagateNotification((Int32)Native.Node.NotificationWMCloseRequest);
			// prefer deferred call - we don't know when and where user may call it
			tree.CallDeferred("quit", exitCode);
		}
	}
}
