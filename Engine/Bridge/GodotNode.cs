using Luny.Engine.Bridge;
using System;
using Native = Godot;

namespace Luny.Godot.Engine.Bridge
{
	/// <summary>
	/// Godot-specific implementation wrapping Godot.Node.
	/// Visibility follows Unity's "active" model: enabled=processing+visible / disabled=stopped+hidden
	/// </summary>
	internal sealed class GodotNode : LunyObject
	{
		private Native.Node.ProcessModeEnum _processModeWhenEnabled;
		private Native.Node Node => Cast<Native.Node>();

		private static Boolean IsNodeVisible(Native.Node node) => node switch
		{
			Native.Node3D n3d => n3d.Visible,
			Native.CanvasItem ci => ci.Visible,
			Native.CanvasLayer cl => cl.Visible,
			var _ => false,
		};

		private static void SetNodeVisible(Native.Node node, Boolean visible) => _ = node switch
		{
			Native.Node3D n3d => n3d.Visible = visible,
			Native.CanvasItem ci => ci.Visible = visible,
			Native.CanvasLayer cl => cl.Visible = visible,
			var _ => false,
		};

		private static void SetNodeProcessingAndVisibleState(Native.Node node, Native.Node.ProcessModeEnum processMode)
		{
			node.ProcessMode = processMode;
			SetNodeVisible(node, processMode != Native.Node.ProcessModeEnum.Disabled);
		}

		public GodotNode(Native.Node node)
			: base(node, (Int64)node.GetInstanceId(), node.ProcessMode != Native.Node.ProcessModeEnum.Disabled, IsNodeVisible(node)) =>
			SetProcessModeWhenEnabled(node);

		// if a node starts "Disabled" we set its process mode to "Inherit" when enabling it,
		// otherwise a node starting "Disabled" would be .. hold your breath .. unenableable! :)
		private void SetProcessModeWhenEnabled(Native.Node node) => _processModeWhenEnabled =
			node.ProcessMode == Native.Node.ProcessModeEnum.Disabled
				? Native.Node.ProcessModeEnum.Inherit
				: node.ProcessMode;

		protected override void DestroyNativeObject() => Node?.QueueFree();
		protected override Boolean IsNativeObjectValid() => Node is {} node && Native.GodotObject.IsInstanceValid(node) && node.IsInsideTree();
		protected override String GetNativeObjectName() => Node.Name;
		protected override void SetNativeObjectName(String name) => Node.Name = name;
		protected override Boolean GetNativeObjectEnabledInHierarchy() => Node.CanProcess();
		protected override Boolean GetNativeObjectEnabled() => Node.ProcessMode != Native.Node.ProcessModeEnum.Disabled;
		protected override void SetNativeObjectEnabled() => SetNodeProcessingAndVisibleState(Node, _processModeWhenEnabled);
		protected override void SetNativeObjectDisabled() => SetNodeProcessingAndVisibleState(Node, Native.Node.ProcessModeEnum.Disabled);
		protected override void SetNativeObjectVisible() => SetNodeVisible(Node, true);
		protected override void SetNativeObjectInvisible() => SetNodeVisible(Node, false);
	}
}
