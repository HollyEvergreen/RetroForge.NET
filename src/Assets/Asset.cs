using System.Linq;
using System.Threading.Tasks;

namespace RetroForge.NET.Assets.Asset;

public struct Asset : IAsset
{
	public readonly AssetType AssetType => AssetType.RAW;
	public readonly string Name => ToString()!;
	private ReadOnlyMemory<byte> data;
	public static Asset CreateFromSpan(Span<byte> data) => new() { data = data.ToArray() };
	public readonly void Dispose() { }
}