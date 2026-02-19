using Godot;
using Luny.Engine.Bridge;

namespace Luny.Godot.Engine.Bridge
{
	public static class LunyVectorExt
	{
		public static Vector3 ToGodot(this LunyVector3 v) => new(v.X, v.Y, v.Z);
		public static LunyVector3 ToLuny(this Vector3 v) => new(v.X, v.Y, v.Z);

		public static Vector2 ToGodot(this LunyVector2 v) => new(v.X, v.Y);
		public static LunyVector2 ToLuny(this Vector2 v) => new(v.X, v.Y);

		public static Quaternion ToGodot(this LunyQuaternion q) => new(q.X, q.Y, q.Z, q.W);
		public static LunyQuaternion ToLuny(this Quaternion q) => new(q.X, q.Y, q.Z, q.W);
	}
}
