using Luny.Engine.Services;
using System;

namespace Luny.Godot.Engine.Services
{
	/// <summary>
	/// Godot implementation of input service.
	/// In real Godot builds, uses _UnhandledInput and Input.IsAction* methods.
	/// Mock version exposes Simulate* methods for testing.
	/// </summary>
	public sealed partial class GodotInputService : LunyInputServiceBase
	{
		public override void SetControlScheme(String schemeName) => throw new NotImplementedException(nameof(SetControlScheme));
	}

	// stub to preserve 'partial' keyword
	public sealed partial class GodotInputService {}
}
