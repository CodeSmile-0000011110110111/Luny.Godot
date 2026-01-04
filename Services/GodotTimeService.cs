using Luny.Engine.Services;
using System;
using Native = Godot;

namespace Luny.Godot.Services
{
	/// <summary>
	/// Godot implementation of time service provider.
	/// </summary>
	public sealed class GodotTimeService : TimeServiceBase, ITimeService
	{
		// downcast isn't an issue: even at 10,000 fps it would take nearly 30 million years before it overflows!
		public Int64 EngineFrameCount => (Int64)Native.Engine.GetProcessFrames();
		public Double ElapsedSeconds => Native.Time.GetTicksMsec() / 1000.0;
	}
}
