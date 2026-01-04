using Luny.Engine.Bridge;
using Luny.Engine.Services;
using Luny.Godot.Proxies;
using System;
using System.Collections.Generic;
using System.IO;
using Native = Godot;

namespace Luny.Godot.Services
{
	/// <summary>
	/// Godot implementation of scene information provider.
	/// </summary>
	public sealed class GodotSceneService : SceneServiceBase, ISceneService
	{
		public String CurrentSceneName
		{
			get
			{
				var tree = (Native.SceneTree)Native.Engine.GetMainLoop();
				var currentScene = tree?.CurrentScene;
				return currentScene?.Name ?? Path.GetFileNameWithoutExtension(currentScene?.SceneFilePath) ?? String.Empty;
			}
		}

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
	}
}
