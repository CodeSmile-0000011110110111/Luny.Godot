using Godot;
using System;

namespace Luny.Godot
{
    internal sealed class GodotLogger : ILunyLogger
    {
        public void LogInfo(String message) => GD.Print(message);

        public void LogWarning(String message) => GD.PushWarning(message);

        public void LogError(String message) => GD.PushError(message);

        public void LogException(Exception exception) => GD.PushError(exception);
    }
}
