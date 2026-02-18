using Luny.Engine.Services;
using System;

namespace Luny.Godot.Engine.Services
{
	/// <summary>
	/// Godot implementation of input service.
	/// In real Godot builds, uses _UnhandledInput and Input.IsAction* methods.
	/// Mock version exposes Simulate* methods for testing.
	/// </summary>
	public sealed partial class GodotInputService : LunyInputServiceBase, ILunyInputService
	{
		/// <summary>
		/// Simulates axis input for testing. In real Godot, this would come from _UnhandledInput.
		/// </summary>
		public void SimulateAxisInput(String actionName, LunyVector2 value) =>
			RaiseDirectionalInput(actionName, value);

		/// <summary>
		/// Simulates button press for testing. In real Godot, this would come from _UnhandledInput.
		/// </summary>
		public void SimulateButtonInput(String actionName, Boolean pressed, Single analogValue = 1f) =>
			RaiseButtonInput(actionName, pressed, analogValue);
	}

	// stub to preserve 'partial' keyword
	public sealed partial class GodotInputService {}
}
