using Godot;
using Luny.Diagnostics;
using Luny.Exceptions;
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
    internal sealed partial class LunyEngineGodotAdapter : Node
    {
        private static LunyEngineGodotAdapter _instance;

        private ILunyEngine _lunyEngine;

        private static void EnsureSingleInstance(Node current)
        {
            if (_instance != null)
            {
                LunyThrow.EngineAdapterSingletonDuplicationException(nameof(LunyEngineGodotAdapter), _instance.Name,
                    unchecked((Int64)_instance.GetInstanceId()), current.Name, (Int64)current.GetInstanceId());
            }
        }

        // Instantiated automatically via Globals/Autoload
        // If it doesn't instantiate, check if LunyScript plugin is enabled.
        private LunyEngineGodotAdapter() => CallDeferred(nameof(Initialize));

        private void Initialize()
        {
            // Logging comes first, we don't want to miss anything
            LunyLogger.Logger = new GodotLogger();

            EnsureSingleInstance(this);

            _instance = this;
            _lunyEngine = LunyEngine.Instance;
            _lunyEngine.OnStartup();
        }

        public override void _PhysicsProcess(Double delta) => _lunyEngine?.OnFixedStep(delta);

        public override void _Process(Double delta)
        {
            _lunyEngine?.OnUpdate(delta);
            _lunyEngine?.OnLateUpdate(delta);
        }

        public override void _ExitTree()
        {
            // we should not exit tree with an existing instance (indicates manual removal)
            if (_instance != null)
            {
                Shutdown();
                LunyThrow.EngineAdapterPrematurelyRemovedException(nameof(LunyEngineGodotAdapter));
            }
        }

        public override void _Notification(Int32 what)
        {
            if (what != NotificationProcess && what != NotificationPhysicsProcess)
                LunyLogger.LogInfo($"_Notification: {GodotHelper.ToNotificationString(what)}", this);

            switch ((Int64)what)
            {
                case NotificationCrash:
                case NotificationWMCloseRequest:
                    Shutdown();
                    break;
            }
        }

        private void Shutdown()
        {
            if (_instance == null)
                return;

            try
            {
                LunyLogger.LogInfo("Shutting down...", this);
                _lunyEngine?.OnShutdown();
            }
            catch (Exception ex)
            {
                LunyLogger.LogException(ex);
            }
            finally
            {
                _lunyEngine = null;
                _instance = null;
                LunyLogger.LogInfo("Shutdown complete.", this);
            }
        }
    }
}
