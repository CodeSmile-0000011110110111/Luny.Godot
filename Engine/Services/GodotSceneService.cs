using Luny.Engine.Bridge;
using Luny.Engine.Services;
using Luny.Godot.Engine.Bridge;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

		public IReadOnlyList<ILunyObject> GetAllObjects()
		{
			var tree = (Native.SceneTree)Native.Engine.GetMainLoop();
			var currentScene = tree?.CurrentScene;

			if (currentScene == null)
				return Array.Empty<ILunyObject>();

			var allObjects = new List<ILunyObject>();

			// Add all nodes recursively starting from root
			void AddNodeAndChildren(Native.Node node)
			{
				allObjects.Add(new GodotNode(node));

				foreach (var child in node.GetChildren())
					AddNodeAndChildren(child);
			}

			AddNodeAndChildren(currentScene);

			return allObjects;
		}

		public ILunyObject FindObjectByName(String name)
		{
			if (String.IsNullOrEmpty(name))
				return null;

			var tree = (Native.SceneTree)Native.Engine.GetMainLoop();
			var currentScene = tree?.CurrentScene;

			if (currentScene == null)
				return null;

			// Search recursively through scene hierarchy
			Native.Node FindNodeRecursive(Native.Node node)
			{
				if (node.Name == name)
					return node;

				foreach (var child in node.GetChildren())
				{
					var found = FindNodeRecursive(child);
					if (found != null)
						return found;
				}

				return null;
			}

			var foundNode = FindNodeRecursive(currentScene);
			return foundNode != null ? new GodotNode(foundNode) : null;
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
		}

		protected override void OnServiceShutdown()
		{
			// Disconnect signals
			SceneTree.NodeAdded -= OnNativeSceneLoaded;
			SceneTree.NodeRemoved -= OnNativeSceneUnloaded;
			CurrentScene = null;
			SceneTree = null;
		}

		private void OnNativeSceneLoaded(Native.Node node)
		{
			if (node == SceneTree.CurrentScene)
			{
				CurrentScene = new GodotScene(SceneTree.CurrentScene);
				LunyLogger.LogInfo($"{nameof(OnNativeSceneLoaded)}: {CurrentScene} => {ToString()}", this);
			}
		}

		private void OnNativeSceneUnloaded(Native.Node node)
		{
			// Note: By the time NodeRemoved fires, GetTree().CurrentScene might already be null or the new scene.
			if (CurrentScene.NativeScene != SceneTree.CurrentScene)
			{
				CurrentScene = null;
				LunyLogger.LogInfo($"{nameof(OnNativeSceneUnloaded)}: {CurrentScene} => {ToString()}", this);
			}
		}
	}
}
