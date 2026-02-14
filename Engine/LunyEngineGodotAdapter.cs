using Godot;
using Luny.Engine;
using Luny.Engine.Bridge;
using Luny.Godot.Engine.Bridge;
using System;

namespace Luny.Godot.Engine
{
	/// <summary>
	/// Ultra-thin Godot adapter: auto-initializes and forwards lifecycle to EngineLifecycleDispatcher.
	/// </summary>
	/// <remarks>
	/// Gets instantiated as autoload singleton, automatically added by plugin.gd.
	/// </remarks>
	internal sealed partial class LunyEngineGodotAdapter : Node, ILunyEngineNativeAdapter, ILunyEngineNativeAdapterInternal
	{
		// intentionally remains private - user code must use LunyEngine.Instance!
		private static ILunyEngineNativeAdapter s_Instance;

		// hold on to LunyEngine reference (not a Node type)
		private ILunyEngineLifecycle _lunyEngine;
		public NativeEngine Engine => NativeEngine.Godot;

		// Instantiated automatically via Globals/Autoload
		// If it doesn't instantiate, check if LunyScript plugin is enabled.
		internal LunyEngineGodotAdapter() => Initialize();

		private void Initialize()
		{
			// Logging comes first, we don't want to miss anything
			LunyLogger.Logger = new GodotLogger();
			LunyPath.Converter = new GodotPathConverter();
			LunyTraceLogger.LogInfoInitializing(this);
			_lunyEngine = ILunyEngineNativeAdapter.CreateEngine(ref s_Instance, this);
			LunyTraceLogger.LogInfoInitialized(this);
		}

		public void SimulateQuit_UnitTestOnly() => _Notification((Int32)NotificationWMCloseRequest);

		public override void _Ready()
		{
			ILunyEngineNativeAdapter.ThrowIfAdapterNull(s_Instance);
			ILunyEngineNativeAdapter.ThrowIfLunyEngineNull(_lunyEngine);
			ILunyEngineNativeAdapter.Startup(s_Instance, _lunyEngine); // => OnStartup()
		}

		public override void _PhysicsProcess(Double deltaTime) =>
			ILunyEngineNativeAdapter.Heartbeat(deltaTime, s_Instance, _lunyEngine); // => OnFixedStep()

		public override void _Process(Double deltaTime)
		{
			ILunyEngineNativeAdapter.FrameUpdate(deltaTime, s_Instance, _lunyEngine); // => OnUpdate()
			ILunyEngineNativeAdapter.FrameLateUpdate(s_Instance, _lunyEngine); // => OnLateUpdate()
		}

		public override void _Notification(Int32 what)
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
				ILunyEngineNativeAdapter.Shutdown(s_Instance, _lunyEngine); // => OnShutdown()
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

	// stub to ensure 'partial' keyword isn't removed by "Code Cleanup" runs
	internal sealed partial class LunyEngineGodotAdapter {}
}
