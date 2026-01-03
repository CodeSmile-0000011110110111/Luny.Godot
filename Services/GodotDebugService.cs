using Godot;
using Luny.Services;
using System;

namespace Luny.Godot.Services
{
	/// <summary>
	/// Godot implementation of Debug provider.
	/// </summary>
	public sealed class GodotDebugService : DebugServiceBase, IDebugService
	{
		public void LogInfo(String message) => GD.Print(message);

		public void LogWarning(String message) => GD.PushWarning(message);

		public void LogError(String message) => GD.PushError(message);

		public void LogException(Exception exception) => GD.PushError(exception);
	}
}
