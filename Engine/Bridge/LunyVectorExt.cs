namespace Luny.Godot
{
	public static class LunyVectorExt
	{
		public static global::Godot.Vector3 ToGodot(this LunyVector3 v) => new(v.X, v.Y, v.Z);
		public static LunyVector3 ToLuny(this global::Godot.Vector3 v) => new(v.X, v.Y, v.Z);

		public static global::Godot.Vector2 ToGodot(this LunyVector2 v) => new(v.X, v.Y);
		public static LunyVector2 ToLuny(this global::Godot.Vector2 v) => new(v.X, v.Y);

		public static global::Godot.Quaternion ToGodot(this LunyQuaternion q) => new(q.X, q.Y, q.Z, q.W);
		public static LunyQuaternion ToLuny(this global::Godot.Quaternion q) => new(q.X, q.Y, q.Z, q.W);
	}
}
