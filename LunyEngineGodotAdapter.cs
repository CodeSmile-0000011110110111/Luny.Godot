using Godot;
using Luny.Engine;
using Luny.Godot.Diagnostics;
using System;

namespace Luny.Godot
{
	/// <summary>
	/// Ultra-thin Godot adapter: auto-initializes and forwards lifecycle to EngineLifecycleDispatcher.
	/// </summary>
	/// <remarks>
	/// Gets instantiated as autoload singleton, automatically added by plugin.gd.
	/// </remarks>
	internal sealed partial class LunyEngineGodotAdapter : Node, IEngineAdapter
	{
		// intentionally remains private - user code must use LunyEngine.Instance!
		private static IEngineAdapter s_Instance;

		// hold on to LunyEngine reference (not a Node type)
		private ILunyEngine _lunyEngine;

		// Instantiated automatically via Globals/Autoload
		// If it doesn't instantiate, check if LunyScript plugin is enabled.
		private LunyEngineGodotAdapter() => Initialize();

		private void Initialize()
		{
			// Logging comes first, we don't want to miss anything
			LunyLogger.Logger = new GodotLogger();
			LunyLogger.LogInfo("Initializing...", typeof(LunyEngineGodotAdapter));

			s_Instance = IEngineAdapter.ValidateAdapterSingletonInstance(s_Instance, this);
			_lunyEngine = LunyEngine.CreateInstance(this);
		}

		public override void _Ready() // => OnStartup()
		{
			IEngineAdapter.AssertNotNull(s_Instance);
			IEngineAdapter.AssertLunyEngineNotNull(_lunyEngine);

			_lunyEngine?.OnStartup();
		}

		public override void _PhysicsProcess(Double delta) => _lunyEngine?.OnFixedStep(delta); // => OnFixedStep()

		public override void _Process(Double delta) // => OnUpdate() + OnLateUpdate()
		{
			_lunyEngine?.OnUpdate(delta);
			_lunyEngine?.OnLateUpdate(delta); // Godot has no separate "late update" callback
		}

		public override void _Notification(Int32 what) // => OnShutdown()
		{
			switch ((Int64)what)
			{
				case NotificationCrash:
				case NotificationWMCloseRequest:
					Shutdown();
					break;
			}
		}

		public override void _ExitTree()
		{
			// we should not exit tree with an existing instance (indicates manual removal)
			IEngineAdapter.AssertNotPrematurelyRemoved(s_Instance, _lunyEngine);
			Shutdown();
		}

		~LunyEngineGodotAdapter() => LunyLogger.LogInfo($"[{nameof(LunyEngineGodotAdapter)}] finalized {GetHashCode()}");

		private void Shutdown()
		{
			if (s_Instance == null)
				return;

			try
			{
				IEngineAdapter.ShutdownLunyEngine(s_Instance, _lunyEngine);
			}
			catch (Exception ex)
			{
				LunyLogger.LogException(ex);
			}
			finally
			{
				IEngineAdapter.ShutdownComplete(s_Instance);

				_lunyEngine = null;
				s_Instance = null;
			}
		}
	}
}
