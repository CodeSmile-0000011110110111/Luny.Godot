using Luny.Engine.Bridge;
using System;
using System.Runtime.CompilerServices;
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
		private Native.Node Node
		{
			get
			{
				if (!IsNativeObjectValid())
					return null;

				return (Native.Node)NativeObject;
			}
		}

		public static ILunyObject ToLunyObject(Native.Node node)
		{
			if (node == null)
				return null;

			var instanceId = (Int64)node.GetInstanceId();
			if (TryGetCached(instanceId, out var lunyObject))
				return lunyObject;

			return new GodotNode(node, instanceId);
		}

		private static Boolean IsNodeVisible(Native.Node node) => node switch
		{
			Native.Node3D n3d => n3d.Visible,
			Native.CanvasItem ci => ci.Visible,
			Native.CanvasLayer cl => cl.Visible,
			var _ => false,
		};

		private GodotNode(Native.Node node, Int64 instanceId)
			: base(node, instanceId, node?.ProcessMode != Native.Node.ProcessModeEnum.Disabled, IsNodeVisible(node)) =>
			SetProcessModeWhenEnabled();

		private void SetNodeVisible(Boolean visible) => _ = Node switch
		{
			Native.Node3D n3d => n3d.Visible = visible,
			Native.CanvasItem ci => ci.Visible = visible,
			Native.CanvasLayer cl => cl.Visible = visible,
			var _ => false,
		};

		private void SetNodeProcessingAndVisibleState(Native.Node.ProcessModeEnum processMode)
		{
			var node = Node;
			if (node == null)
				return;

			node.ProcessMode = processMode;
			SetNodeVisible(processMode != Native.Node.ProcessModeEnum.Disabled);
		}

		// if a node starts "Disabled" we set its process mode to "Inherit" when enabling it,
		// otherwise a node starting "Disabled" would be .. hold your breath .. unenableable! :)
		private void SetProcessModeWhenEnabled()
		{
			var node = Node;
			if (node == null)
				return;

			var processMode = node.ProcessMode;
			_processModeWhenEnabled = processMode == Native.Node.ProcessModeEnum.Disabled ? Native.Node.ProcessModeEnum.Inherit : processMode;
		}

		protected override void DestroyNativeObject()
		{
			var node = Node;
			if (node == null)
				return;

			node.QueueFree();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override Boolean IsNativeObjectValid()
		{
			var node = NativeObject as Native.Node; // must use NativeObject here to avoid stackoverflow
			if (node == null)
				return false;

			return Native.GodotObject.IsInstanceValid(node) && node.IsInsideTree();
		}

		protected override String GetNativeObjectName()
		{
			var node = Node;
			if (node == null)
				return "<null>";

			return node.Name;
		}

		protected override void SetNativeObjectName(String name)
		{
			var node = Node;
			if (node == null)
				return;

			node.Name = name;
		}

		protected override Boolean GetNativeObjectEnabledInHierarchy()
		{
			var node = Node;
			if (node == null)
				return false;

			return node.CanProcess();
		}

		protected override Boolean GetNativeObjectEnabled()
		{
			var node = Node;
			if (node == null)
				return false;

			return node.ProcessMode != Native.Node.ProcessModeEnum.Disabled;
		}

		protected override void SetNativeObjectEnabled() => SetNodeProcessingAndVisibleState(_processModeWhenEnabled);
		protected override void SetNativeObjectDisabled() => SetNodeProcessingAndVisibleState(Native.Node.ProcessModeEnum.Disabled);
		protected override void SetNativeObjectVisible() => SetNodeVisible(true);
		protected override void SetNativeObjectInvisible() => SetNodeVisible(false);
	}
}
