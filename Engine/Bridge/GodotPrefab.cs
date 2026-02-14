using Godot;
using Luny.Engine.Bridge;
using System;

namespace Luny.Godot.Engine.Bridge
{
	public sealed class GodotPrefab : ILunyPrefab
	{
		private readonly PackedScene _packedScene;

		public LunyAssetID AssetID { get; internal set; }
		public Object NativeAsset => _packedScene;
		public LunyAssetPath AssetPath { get; }

		public GodotPrefab(PackedScene packedScene, LunyAssetPath assetPath)
		{
			_packedScene = packedScene ?? throw new ArgumentNullException(nameof(packedScene));
			AssetPath = assetPath ?? throw new ArgumentNullException(nameof(assetPath));

			if (!_packedScene.CanInstantiate())
				throw new ArgumentException($"PackedScene {assetPath} is not instantiable", nameof(packedScene));
		}

		public T Cast<T>() where T : class => _packedScene as T;

		public Node Instantiate() => _packedScene.Instantiate();
	}
}
