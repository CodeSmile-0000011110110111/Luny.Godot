using Luny.Engine.Bridge;
using System;

namespace Luny.Godot.Engine.Bridge
{
	public sealed class GodotPath : LunyPath
	{
		public static implicit operator GodotPath(String enginePath) => new(enginePath);

		public GodotPath(String nativePath)
			: base(nativePath) {}

		// Godot paths must remove their prefix
		// TODO: cache substring upon use
		protected override String ToEngineAgnosticPath(String nativePath) =>
			nativePath.StartsWith("res://") ? nativePath.Substring("res://".Length) : nativePath;
	}
}
