using System.Linq;
using Native = Godot;

namespace Luny.Godot
{
	public static class NodeExtensions
	{
		public static T GetComponent<T>(this Native.Node node) where T : class => node.GetChildren().OfType<T>().FirstOrDefault();
	}
}
