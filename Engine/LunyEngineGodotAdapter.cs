using Godot;
using Luny.Engine;
using System;

namespace Luny.Godot.Engine
{
	/// <summary>
	/// Ultra-thin Godot adapter: auto-initializes and forwards lifecycle to EngineLifecycleDispatcher.
	/// </summary>
	/// <remarks>
	/// Gets instantiated as autoload singleton, automatically added by plugin.gd.
	/// </remarks>
	internal sealed partial class LunyEngineGodotAdapter : Node, ILunyEngineNativeAdapter
	{
		// intentionally remains private - user code must use LunyEngine.Instance!
		private static ILunyEngineNativeAdapter s_Instance;

		// hold on to LunyEngine reference (not a Node type)
		private ILunyEngineAdapter _lunyEngine;

		// Instantiated automatically via Globals/Autoload
		// If it doesn't instantiate, check if LunyScript plugin is enabled.
		private LunyEngineGodotAdapter() => Initialize();

		private void Initialize()
		{
			// Logging comes first, we don't want to miss anything
			LunyLogger.Logger = new GodotLogger();
			LunyTraceLogger.LogInfoInitializing(this);

			s_Instance = ILunyEngineNativeAdapter.ValidateAdapterSingletonInstance(s_Instance, this);
			_lunyEngine = LunyEngine.CreateInstance(this);

			LunyTraceLogger.LogInfoInitialized(this);
		}

		public override void _Ready() // => OnStartup()
		{
			ILunyEngineNativeAdapter.ThrowIfAdapterNull(s_Instance);
			ILunyEngineNativeAdapter.ThrowIfLunyEngineNull(_lunyEngine);

			_lunyEngine?.OnEngineStartup(this);
		}

		public override void _PhysicsProcess(Double delta) => _lunyEngine?.OnEngineFixedStep(delta, this); // => OnFixedStep()

		public override void _Process(Double delta) // => OnUpdate() + OnLateUpdate()
		{
			_lunyEngine?.OnEngineUpdate(delta, this);
			_lunyEngine?.OnEngineLateUpdate(delta, this); // emulate "late update"
		}

		public override void _Notification(Int32 what) // => OnShutdown()
		{
			switch ((Int64)what)
			{
				case NotificationCrash:
				case NotificationWMCloseRequest:
					ILunyEngineNativeAdapter.IsApplicationQuitting = true;
					Shutdown();
					Destroy();
					break;
			}
		}

		public override void _ExitTree() => Destroy();

		private void Destroy()
		{
			LunyTraceLogger.LogInfoDestroying(this);

			// we should not exit tree with an existing instance (indicates manual removal)
			ILunyEngineNativeAdapter.ThrowIfPrematurelyRemoved(s_Instance, _lunyEngine);

			LunyTraceLogger.LogInfoDestroyed(this);
			ILunyEngineNativeAdapter.EndLogging();
		}

		~LunyEngineGodotAdapter() => LunyTraceLogger.LogInfoFinalized(this);

		private void Shutdown()
		{
			if (s_Instance == null)
				return;

			try
			{
				ILunyEngineNativeAdapter.Shutdown(s_Instance, _lunyEngine);
			}
			catch (Exception ex)
			{
				LunyLogger.LogException(ex);
			}
			finally
			{
				ILunyEngineNativeAdapter.ShutdownComplete(s_Instance);

				_lunyEngine = null;
				s_Instance = null;
			}
		}
	}
}
