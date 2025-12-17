using Godot;
using System;

namespace Luny.Godot
{
    /// <summary>
    /// Ultra-thin Godot adapter: auto-initializes and forwards lifecycle to EngineLifecycleDispatcher.
    /// </summary>
    /// <remarks>
    /// Gets instantiated as autoload singleton, automatically added by plugin.gd.
    /// </remarks>
    internal sealed partial class GodotLifecycleAdapter : Node
    {
        private static GodotLifecycleAdapter _instance;

        private IEngineLifecycleDispatcher _dispatcher;

        private static void EnsureSingleInstance(Node current)
        {
            if (_instance != null)
            {
                Throw.LifecycleAdapterSingletonDuplicationException(nameof(GodotLifecycleAdapter), _instance.Name,
                    unchecked((Int64)_instance.GetInstanceId()), current.Name, (Int64)current.GetInstanceId());
            }
        }

        // Instantiated automatically via Globals/Autoload
        // If it doesn't instantiate, check if LunyScript plugin is enabled.
        private GodotLifecycleAdapter() => Initialize();

        private void Initialize()
        {
            // Logging comes first, we don't want to miss anything
            LunyLogger.SetLogger(new GodotLogger());

            EnsureSingleInstance(this);

            _instance = this;
            _dispatcher = EngineLifecycleDispatcher.Instance;
        }

        public override void _Process(Double delta)
        {
            _dispatcher?.OnUpdate(delta);
            _dispatcher?.OnLateUpdate(delta);
        }

        public override void _PhysicsProcess(Double delta) => _dispatcher?.OnFixedStep(delta);

        public override void _ExitTree()
        {
            // we should not exit tree with an existing instance (indicates manual removal)
            if (_instance != null)
            {
                Shutdown();
                Throw.LifecycleAdapterPrematurelyRemovedException(nameof(GodotLifecycleAdapter));
            }
        }

        public override void _Notification(Int32 what)
        {
            if (what != NotificationProcess && what != NotificationPhysicsProcess)
                GD.Print($"GodotLifecycleAdapter ({GetInstanceId()}) _Notification: {GodotHelper.ToNotificationString(what)}");

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
                LunyLogger.LogInfo("[Luny] Shutting down...");
                _dispatcher?.OnShutdown();
            }
            catch (Exception ex)
            {
                LunyLogger.LogException(ex);
            }
            finally
            {
                _dispatcher = null;
                _instance = null;
                LunyLogger.LogInfo("[Luny] Shutdown complete.");
            }
        }
    }
}
