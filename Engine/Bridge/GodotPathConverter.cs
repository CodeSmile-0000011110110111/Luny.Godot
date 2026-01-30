using Luny.Engine.Bridge;
using System;

namespace Luny.Godot.Engine.Bridge
{
	public sealed class GodotPathConverter : ILunyPathConverter
	{
		private const String ResPrefix = "res://";
		private const String UserPrefix = "user://";

		public String ToLuny(String nativePath, LunyPathType type)
		{
			if (String.IsNullOrEmpty(nativePath))
				return nativePath;

			if (nativePath.StartsWith(ResPrefix))
				return nativePath.Substring(ResPrefix.Length);
			if (nativePath.StartsWith(UserPrefix))
				return nativePath.Substring(UserPrefix.Length);

			return nativePath;
		}

		public String ToNative(String agnosticPath, LunyPathType type)
		{
			if (String.IsNullOrEmpty(agnosticPath))
				return agnosticPath;

			return type switch
			{
				LunyPathType.Save => UserPrefix + agnosticPath,
				_ => ResPrefix + agnosticPath
			};
		}
	}
}
