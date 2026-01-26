using Luny.Engine.Bridge;
using System;
using Node = Godot.Node;

namespace Luny.Godot.Engine.Bridge
{
	public sealed class GodotScene : LunyScene
	{
		private String _name;
		public override String Name => _name ??= System.IO.Path.GetFileNameWithoutExtension(((Node)NativeScene)?.SceneFilePath);

		public GodotScene(Node nativeScene)
			: base(nativeScene, nativeScene != null ? new GodotPath(nativeScene.SceneFilePath) : null) {}
	}
}
