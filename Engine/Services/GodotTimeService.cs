using Luny.Engine.Services;
using System;
using Native = Godot;

namespace Luny.Godot.Engine.Services
{
	/// <summary>
	/// Godot implementation of time service provider.
	/// </summary>
	public sealed class GodotTimeService : LunyTimeServiceBase, ILunyTimeService
	{
		// int cast: even at 10,000 fps it'll take 29.2 million years until overflow!
		public Int64 EngineFrameCount => (Int64)Native.Engine.GetProcessFrames();
		public Double ElapsedSeconds => Native.Time.GetTicksMsec() / 1000.0;
	}
}
