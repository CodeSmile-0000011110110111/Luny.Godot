using Luny.Engine.Bridge;
using Luny.Engine.Services;
using Luny.Godot.Engine.Bridge;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Native = Godot;

namespace Luny.Godot.Engine.Services
{
	/// <summary>
	/// Godot implementation of scene information provider.
	/// </summary>
	public sealed class GodotSceneService : LunySceneServiceBase, ILunySceneService
	{
		[NotNull] private static Native.SceneTree SceneTree { get; set; }
		public void ReloadScene() => SceneTree?.ReloadCurrentScene();

		public IReadOnlyList<ILunyObject> GetObjects(IReadOnlyCollection<String> objectNames)
		{
			var currentScene = SceneTree?.CurrentScene;
			if (currentScene == null)
				return Array.Empty<ILunyObject>();

			var foundObjects = new List<ILunyObject>();

			// Add all nodes recursively starting from root
			void AddNodeAndChildren(Native.Node node)
			{
				if (objectNames.Contains(node.Name.ToString()))
					foundObjects.Add(GodotNode.ToLunyObject(node));

				foreach (var child in node.GetChildren())
					AddNodeAndChildren(child);
			}

			AddNodeAndChildren(currentScene);

			return foundObjects.AsReadOnly();
		}

		public ILunyObject FindObjectByName(String name)
		{
			if (String.IsNullOrEmpty(name))
				return null;

			var currentScene = SceneTree?.CurrentScene;
			if (currentScene == null)
				return null;

			var foundNode = FindNodeRecursive(currentScene, name);
			return foundNode != null ? GodotNode.ToLunyObject(foundNode) : null;
		}

		private Native.Node FindNodeRecursive(Native.Node node, String name)
		{
			if (node.Name == name)
				return node;

			foreach (var child in node.GetChildren())
			{
				var found = FindNodeRecursive(child, name);
				if (found != null)
					return found;
			}

			return null;
		}

		protected override void OnServiceInitialize()
		{
			SceneTree = (Native.SceneTree)Native.Engine.GetMainLoop();

			// Connect signals using the Godot += syntax
			SceneTree.NodeAdded += OnNodeAdded;
			SceneTree.NodeRemoved += OnNodeRemoved;
		}

		protected override void OnServiceStartup()
		{
			if (SceneTree.CurrentScene != null)
			{
				CurrentScene = new GodotScene(SceneTree.CurrentScene);
				LunyLogger.LogInfo($"{nameof(OnServiceStartup)}: CurrentScene={CurrentScene}", this);
				InvokeOnSceneLoaded(CurrentScene);
			}
			else
				LunyLogger.LogWarning($"{nameof(OnServiceStartup)}: SceneTree.CurrentScene is NULL!", this);
		}

		protected override void OnServiceShutdown()
		{
			// Disconnect signals
			SceneTree.NodeAdded -= OnNodeAdded;
			SceneTree.NodeRemoved -= OnNodeRemoved;
			CurrentScene = null;
			SceneTree = null;
		}

		private void OnNodeRemoved(Native.Node node)
		{
			// Note: When NodeRemoved fires, GetTree().CurrentScene is the new scene (or null).
			if (CurrentScene != null && CurrentScene.NativeScene != SceneTree.CurrentScene)
			{
				LunyTraceLogger.LogTrace($"{nameof(OnNodeRemoved)}: {CurrentScene} => {ToString()}", this);
				InvokeOnSceneUnloaded(CurrentScene);
				CurrentScene = null;
			}
		}

		private void OnNodeAdded(Native.Node node)
		{
			LunyLogger.LogInfo($"{nameof(OnNodeAdded)}: node={node.Name}, CurrentScene={SceneTree.CurrentScene?.Name}", this);
			if (node == SceneTree.CurrentScene)
			{
				CurrentScene = new GodotScene(SceneTree.CurrentScene);
				LunyLogger.LogInfo($"{nameof(OnNodeAdded)}: {CurrentScene} => {ToString()}", this);
				InvokeOnSceneLoaded(CurrentScene);
			}
		}
	}
}
