using Luny.Engine.Bridge;
using System;

namespace Luny.Godot.Engine.Bridge
{
	public sealed class GodotPath : LunyPath
	{
		public GodotPath(String path, Boolean isNative)
			: base(path, isNative) {}

		public static implicit operator GodotPath(String nativePath) => new(nativePath, true);
	}
}
