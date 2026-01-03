using Godot;
using Luny.Godot.Proxies;
using Luny.Proxies;
using Luny.Services;
using System;
using System.Collections.Generic;
using System.IO;

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
				var tree = (SceneTree)Engine.GetMainLoop();
				var currentScene = tree?.CurrentScene;
				return currentScene?.Name ?? Path.GetFileNameWithoutExtension(currentScene?.SceneFilePath) ?? String.Empty;
			}
		}

		public IReadOnlyList<ILunyObject> GetAllObjects()
		{
			var tree = (SceneTree)Engine.GetMainLoop();
			var currentScene = tree?.CurrentScene;

			if (currentScene == null)
				return Array.Empty<ILunyObject>();

			var allObjects = new List<ILunyObject>();

			// Add all nodes recursively starting from root
			void AddNodeAndChildren(Node node)
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

			var tree = (SceneTree)Engine.GetMainLoop();
			var currentScene = tree?.CurrentScene;

			if (currentScene == null)
				return null;

			// Search recursively through scene hierarchy
			Node FindNodeRecursive(Node node)
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
