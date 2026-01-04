using Luny.Engine.Services;
using System.Diagnostics;

namespace Luny.Godot.Services
{
	/// <summary>
	/// Godot implementation of Debug provider.
	/// </summary>
	public sealed class GodotEditorService : EditorServiceBase, IEditorService
	{
		/// <summary>
		/// Caution: Falls back to Debugger.Break() because Godot doesn't support player pause/resume via script.
		/// </summary>
		public void PausePlayer() => Debugger.Break();

		/*
			// Pausing the SceneTree works but it cannot be unpaused via Editor UI.
			var sceneTree = Engine.GetMainLoop() as SceneTree;
			if (sceneTree != null)
				sceneTree.Paused = true;
		*/
	}
}
