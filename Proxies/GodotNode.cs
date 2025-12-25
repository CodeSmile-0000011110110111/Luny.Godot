using Godot;
using Luny.Proxies;
using System;
using SystemObject = System.Object;

namespace Luny.Godot.Proxies
{
	/// <summary>
	/// Godot-specific implementation wrapping Godot.Node.
	/// </summary>
	public sealed class GodotNode : LunyObject
	{
		private readonly Node _node;
		private UInt64 _nativeID;
		private String _name;

		/// <summary>
		/// Gets the wrapped Godot Node.
		/// </summary>
		public Node Node => _node;
		public override NativeID NativeID => _nativeID;
		public override String Name => _name;
		public override Boolean IsValid => _node != null && GodotObject.IsInstanceValid(_node) && _node.IsInsideTree();
		public override Boolean Enabled
		{
			get => IsValid && _node.IsProcessing();
			set
			{
				if (IsValid)
					_node.SetProcess(value);
			}
		}

		public GodotNode(Node node)
		{
			_node = node;

			_name = _node.Name;
			_nativeID = _node.GetInstanceId();
		}

		public override SystemObject GetNativeObject() => _node;
	}
}
