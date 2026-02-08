using Godot;
using Luny.Engine.Services;
using System;

namespace Luny.Godot.Engine.Services
{
	/// <summary>
	/// Godot implementation of Debug provider.
	/// </summary>
	public sealed class GodotDebugService : LunyDebugServiceBase, ILunyDebugService
	{
		public override void LogInfo(String message) => GD.Print(message);

		public override void LogWarning(String message) => GD.PushWarning(message);

		public override void LogError(String message) => GD.PushError(message);

		public override void LogException(Exception exception) => GD.PushError(exception);
	}
}
