using Luny.Engine.Bridge;
using Luny.Engine.Bridge.Enums;
using Luny.Engine.Services;
using Luny.Godot.Engine.Bridge;
using System;
using Native = Godot;

namespace Luny.Godot.Engine.Services
{
	public sealed class GodotObjectService : LunyObjectServiceBase, ILunyObjectService
	{
		public ILunyObject CreateEmpty(String name)
		{
			var node = new Native.Node3D { Name = name };
			AddNodeToScene(node);
			return new GodotNode(node);
		}

		public ILunyObject CreatePrimitive(String name, LunyPrimitiveType type)
		{
			var meshInstance = new Native.MeshInstance3D { Name = name };
			meshInstance.Mesh = type switch
			{
				LunyPrimitiveType.Cube => new Native.BoxMesh(),
				LunyPrimitiveType.Sphere => new Native.SphereMesh(),
				LunyPrimitiveType.Capsule => new Native.CapsuleMesh(),
				LunyPrimitiveType.Cylinder => new Native.CylinderMesh(),
				LunyPrimitiveType.Plane => new Native.PlaneMesh(),
				LunyPrimitiveType.Quad => new Native.QuadMesh(),
				var _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
			};
			AddNodeToScene(meshInstance);
			return new GodotNode(meshInstance);
		}

		private void AddNodeToScene(Native.Node node)
		{
			var sceneTree = (Native.SceneTree)Native.Engine.GetMainLoop();
			if (sceneTree == null)
			{
				LunyLogger.LogError($"can't add node {node}: SceneTree is null", this);
				return;
			}
			if (sceneTree.CurrentScene == null)
			{
				LunyLogger.LogError($"can't add node {node}: SceneTree.CurrentScene is null", this);
				return;
			}

			sceneTree.CurrentScene.AddChild(node);
		}
	}
}
