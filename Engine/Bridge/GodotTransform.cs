using Luny.Engine.Bridge;
using System;
using System.Collections.Generic;
using Native = Godot;

namespace Luny.Godot.Engine.Bridge
{
	internal sealed class GodotTransform : LunyTransform
	{
		private readonly Native.Node3D _node;

		public override LunyVector3 Position
		{
			get => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(Position)}");
			set => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(Position)}");
		}

		public override LunyQuaternion Rotation
		{
			get => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(Rotation)}");
			set => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(Rotation)}");
		}

		public override LunyVector3 EulerAngles
		{
			get => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(EulerAngles)}");
			set => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(EulerAngles)}");
		}

		public override LunyVector3 LocalPosition
		{
			get => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(LocalPosition)}");
			set => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(LocalPosition)}");
		}

		public override LunyQuaternion LocalRotation
		{
			get => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(LocalRotation)}");
			set => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(LocalRotation)}");
		}

		public override LunyVector3 LocalEulerAngles
		{
			get => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(LocalEulerAngles)}");
			set => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(LocalEulerAngles)}");
		}

		public override LunyVector3 LocalScale
		{
			get => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(LocalScale)}");
			set => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(LocalScale)}");
		}

		public override LunyVector3 Forward => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(Forward)}");
		public override LunyVector3 Back => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(Back)}");
		public override LunyVector3 Up => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(Up)}");
		public override LunyVector3 Down => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(Down)}");
		public override LunyVector3 Right => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(Right)}");
		public override LunyVector3 Left => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(Left)}");

		public override LunyTransform Parent
		{
			get => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(Parent)}");
			set => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(Parent)}");
		}

		public override LunyTransform Root => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(Root)}");
		public override Int32 ChildCount => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(ChildCount)}");
		public override IEnumerable<LunyTransform> Children =>
			throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(Children)}");

		internal GodotTransform(Native.Node3D node) => _node = node;

		public override LunyTransform GetChild(Int32 index) =>
			throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(GetChild)}");

		public override void SetParent(LunyTransform parent, Boolean worldPositionStays = true) =>
			throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(SetParent)}");

		public override Boolean IsChildOf(LunyTransform parent) =>
			throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(IsChildOf)}");

		public override Int32 GetSiblingIndex() => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(GetSiblingIndex)}");

		public override void SetSiblingIndex(Int32 index) =>
			throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(SetSiblingIndex)}");

		public override void SetAsFirstSibling() => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(SetAsFirstSibling)}");
		public override void SetAsLastSibling() => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(SetAsLastSibling)}");
		public override void DetachChildren() => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(DetachChildren)}");

		public override LunyVector3 TransformPoint(LunyVector3 point) =>
			throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(TransformPoint)}");

		public override LunyVector3 InverseTransformPoint(LunyVector3 point) =>
			throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(InverseTransformPoint)}");

		public override LunyVector3 TransformDirection(LunyVector3 direction) =>
			throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(TransformDirection)}");

		public override LunyVector3 InverseTransformDirection(LunyVector3 direction) =>
			throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(InverseTransformDirection)}");

		public override LunyVector3 TransformVector(LunyVector3 vector) =>
			throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(TransformVector)}");

		public override LunyVector3 InverseTransformVector(LunyVector3 vector) =>
			throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(InverseTransformVector)}");

		public override void LookAt(LunyVector3 worldPosition) =>
			throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(LookAt)}");

		public override void LookAt(LunyVector3 worldPosition, LunyVector3 worldUp) =>
			throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(LookAt)}");

		public override void LookAt(ILunyObject target) => throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(LookAt)}");

		public override void LookAt(ILunyObject target, LunyVector3 worldUp) =>
			throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(LookAt)}");

		public override void Rotate(LunyVector3 eulerAngles, LunySpace space = LunySpace.Self) =>
			throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(Rotate)}");

		public override void Rotate(LunyVector3 axis, Single angle, LunySpace space = LunySpace.Self) =>
			throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(Rotate)}");

		public override void OrbitAround(LunyVector3 worldPoint, LunyVector3 axis, Single angle) =>
			throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(OrbitAround)}");

		public override void Translate(LunyVector2 translation, LunySpace space = LunySpace.Self) =>
			throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(Translate)}");

		public override void Translate(LunyVector3 translation, LunySpace space = LunySpace.Self) =>
			throw new NotImplementedException($"{nameof(GodotTransform)}.{nameof(Translate)}");
	}
}
