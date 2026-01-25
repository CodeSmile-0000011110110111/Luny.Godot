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
		public void ReloadScene() => throw new NotImplementedException(nameof(ReloadScene));

		public IReadOnlyList<ILunyObject> GetObjects(IReadOnlyCollection<String> objectNames)
		{
			var currentScene = SceneTree?.CurrentScene;
			if (currentScene == null)
				return Array.Empty<ILunyObject>();

			var foundObjects = new List<ILunyObject>();

			// Add all nodes recursively starting from root
			void AddNodeAndChildren(Native.Node node)
			{
				if (objectNames.Contains<String>(node.Name))
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
			SceneTree.NodeAdded += OnNativeSceneLoaded;
			SceneTree.NodeRemoved += OnNativeSceneUnloaded;
		}

		protected override void OnServiceStartup()
		{
			CurrentScene = new GodotScene(SceneTree.CurrentScene);
			LunyLogger.LogInfo($"{nameof(OnServiceInitialize)}: CurrentScene={CurrentScene}", this);

			InvokeOnSceneLoaded(CurrentScene);
		}

		protected override void OnServiceShutdown()
		{
			// Disconnect signals
			SceneTree.NodeAdded -= OnNativeSceneLoaded;
			SceneTree.NodeRemoved -= OnNativeSceneUnloaded;
			CurrentScene = null;
			SceneTree = null;
		}

		private void OnNativeSceneUnloaded(Native.Node node)
		{
			// Note: When NodeRemoved fires, GetTree().CurrentScene is the new scene (or null).
			if (CurrentScene.NativeScene != SceneTree.CurrentScene)
			{
				LunyTraceLogger.LogTrace($"{nameof(OnNativeSceneUnloaded)}: {CurrentScene} => {ToString()}", this);
				CurrentScene = null;
			}
		}

		private void OnNativeSceneLoaded(Native.Node node)
		{
			if (node == SceneTree.CurrentScene)
			{
				CurrentScene = new GodotScene(SceneTree.CurrentScene);
				LunyTraceLogger.LogTrace($"{nameof(OnNativeSceneLoaded)}: {CurrentScene} => {ToString()}", this);
			}
		}
	}
}
