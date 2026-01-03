using Godot;
using Luny.Services;
using System;

namespace Luny.Godot.Services
{
	/// <summary>
	/// Godot implementation of time service provider.
	/// </summary>
	public sealed class GodotTimeService : TimeServiceBase, ITimeService
	{
		// downcast isn't an issue: even at 10,000 fps it would take nearly 30 million years before it overflows!
		public Int64 EngineFrameCount => (Int64)Engine.GetProcessFrames();
		public Double ElapsedSeconds => Time.GetTicksMsec() / 1000.0;
	}
}
