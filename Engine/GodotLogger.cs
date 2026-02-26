using Godot;
using System;

namespace Luny.Godot.Engine
{
	internal sealed class GodotLogger : ILunyLogger
	{
		public void LogInfo(Object obj) => throw new NotImplementedException(nameof(LogInfo));

		public void LogWarning(Object obj) => throw new NotImplementedException(nameof(LogWarning));

		public void LogError(Object obj) => throw new NotImplementedException(nameof(LogError));
		public void LogInfo(String message) => GD.Print(message);

		public void LogWarning(String message) => GD.PushWarning(message);

		public void LogError(String message) => GD.PushError(message);

		public void LogException(Exception exception) => GD.PushError(exception);
	}
}
