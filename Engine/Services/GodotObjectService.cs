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
		protected override ILunyObject OnCreateEmpty(String name)
		{
			var node = new Native.Node3D { Name = name };
			AddNodeToScene(node);
			return new GodotNode(node);
		}

		protected override ILunyObject OnCreatePrimitive(PrimitiveType type, String name)
		{
			var meshInstance = new Native.MeshInstance3D { Name = name };
			meshInstance.Mesh = type switch
			{
				PrimitiveType.Cube => new Native.BoxMesh(),
				PrimitiveType.Sphere => new Native.SphereMesh(),
				PrimitiveType.Capsule => new Native.CapsuleMesh(),
				PrimitiveType.Cylinder => new Native.CylinderMesh(),
				PrimitiveType.Plane => new Native.PlaneMesh(),
				PrimitiveType.Quad => new Native.QuadMesh(),
				_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
			};
			AddNodeToScene(meshInstance);
			return new GodotNode(meshInstance);
		}

		private void AddNodeToScene(Native.Node node)
		{
			var sceneTree = (Native.SceneTree)Native.Engine.GetMainLoop();
			sceneTree?.CurrentScene?.AddChild(node);
		}
	}
}
