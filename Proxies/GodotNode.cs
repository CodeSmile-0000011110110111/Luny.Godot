using Luny.Engine.Bridge;
using Luny.Exceptions;
using System;
using Native = Godot;

namespace Luny.Godot.Proxies
{
	/// <summary>
	/// Godot-specific implementation wrapping Godot.Node.
	/// </summary>
	public sealed class GodotNode : LunyObject
	{
		private readonly UInt64 _nativeID;
		private Native.Node _node;
		private String _name;
		private Boolean _isDestroyed;
		private Boolean _isEnabled;
		private Native.Node.ProcessModeEnum _initialProcessMode;

		/// <summary>
		/// Gets the wrapped Godot Node.
		/// </summary>
		public Native.Node Node => _node;
		public override NativeID NativeID => _nativeID;
		public override String Name
		{
			get => IsValid ? _node.Name : _name;
			set
			{
				if (IsValid)
					_node.Name = _name = value;
			}
		}
		public override Boolean IsValid => !_isDestroyed && _node != null && Native.GodotObject.IsInstanceValid(_node) && _node.IsInsideTree();
		public override Boolean IsEnabled
		{
			get => IsValid && _isEnabled;
			set
			{
				if (_isEnabled == value)
					return;
				if (!IsValid)
					return;

				_isEnabled = value;
				var isProcessModeDisabled = IsProcessModeDisabled(_node);
				if (isProcessModeDisabled && _isEnabled)
				{
					_node.ProcessMode = _isEnabled ? _initialProcessMode : Native.Node.ProcessModeEnum.Disabled;
					SetNodeVisible(_node, _isEnabled);
				}

				InvokeOnEnableOrOnDisable(_isEnabled);
			}
		}

		private static Boolean IsProcessModeDisabled(Native.Node node) => node?.ProcessMode == Native.Node.ProcessModeEnum.Disabled;

		private static void SetNodeVisible(Native.Node node, Boolean visible)
		{
			if (node == null)
				return;

			switch (node)
			{
				case Native.CanvasItem ci when ci.Visible != visible:
					ci.Visible = visible;
					break;
				case Native.Node3D n3d when n3d.Visible != visible:
					n3d.Visible = visible;
					break;
				case Native.CanvasLayer cl when cl.Visible != visible:
					cl.Visible = visible;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(node), $"unhandled Node class: {node?.GetType()}");
			}
		}

		public GodotNode(Native.Node node)
			: base(node)
		{
			if (node == null)
				throw new ArgumentNullException(nameof(node), $"{nameof(GodotNode)} {nameof(Node)} reference must not be null.");

			_node = node;

			// stored for reference in case object reference unexpectedly becomes null or "missing"
			_name = _node.Name;
			_nativeID = _node.GetInstanceId();

			// set initial state
			_initialProcessMode = _node.ProcessMode;
			_isEnabled = !IsProcessModeDisabled(_node);
		}

		public override void Destroy()
		{
			if (!IsValid)
				return;

			IsEnabled = false; // triggers OnDisable if enabled
			InvokeOnDestroy();
			_isDestroyed = true; // Mark as destroyed (native destruction happens later)
		}

		public override void DestroyNativeObject()
		{
			if (IsValid)
				throw new LunyLifecycleException($"{nameof(DestroyNativeObject)}() called without calling {nameof(Destroy)}() first: {this}");

			_node?.QueueFree();
			_node = null;
		}
	}
}
