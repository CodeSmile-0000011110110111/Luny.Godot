using Godot;
using Luny.Engine.Bridge;
using Luny.Engine.Bridge.Identity;
using Luny.Engine.Services;
using Luny.Godot.Engine.Bridge;
using System;
using System.Collections.Generic;

namespace Luny.Godot.Engine.Services
{
	public sealed class GodotAssetService : LunyAssetServiceBase
	{
		protected override T LoadNative<T>(LunyAssetPath path)
		{
			var nativePath = path.NativePath;
			var resource = ResourceLoader.Load(nativePath);
			if (resource == null)
				return null;

			if (typeof(T) == typeof(ILunyPrefab) && resource is PackedScene packedScene)
			{
				return new GodotPrefab(packedScene, path) as T;
			}

			// Add more types as needed (Mesh, Scene, etc.)
			return null;
		}

		protected override void UnloadNative(ILunyAsset asset)
		{
			// Godot uses ref counting for Resources, no explicit unload usually needed for simple mocks.
			// In real engine, we might do nothing or call something specific if needed.
		}

		protected override IReadOnlyDictionary<Type, String[]> GetExtensionMapping() => new Dictionary<Type, String[]>
		{
			{ typeof(ILunyPrefab), new[] { ".tscn", ".scn" } },
			// { typeof(ILunyMesh), new[] { ".mesh", ".res" } },
			// { typeof(ILunyScene), new[] { ".tscn", ".scn" } },
		};

		protected override T GetPlaceholder<T>(LunyAssetPath path)
		{
			if (typeof(T) == typeof(ILunyPrefab))
			{
				// In a real implementation, we'd load a "MissingPrefab" from a known Luny location.
				// For now, we'll return a GodotPrefab wrapping an empty PackedScene or similar.
				var scene = new PackedScene();
				return new GodotPrefab(scene, path) as T;
			}
			return null;
		}
	}
}
