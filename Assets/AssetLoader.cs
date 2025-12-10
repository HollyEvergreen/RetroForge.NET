using RetroForge.NET.Assets.Asset;
using System.IO.Compression;

namespace RetroForge.NET.Assets
{
	public class AssetLoader(string path)
	{
		private readonly ZipArchive Assets = ZipFile.Open(path, ZipArchiveMode.Update);
		private readonly List<IAsset> assets = [];
		public void LoadAllAssets()
		{
			assets.AddRange(
				Assets.Entries.Select(
					(entry) => {
                        Logger.Warn(entry.FullName);
                        IAsset asset = entry.FullName.Split('.').Last() switch
						{ 
							"png" => GetImageAsset(entry.FullName),
							"frag" => GetShaderAsset(entry.FullName),
							"vert" => GetShaderAsset(entry.FullName),
							"wav" => GetAudioAsset(entry.FullName),
							"txt" => GetTextAsset(entry.FullName),
							"scn" => GetSceneAsset(entry.FullName),
							"anim2d" => GetAnim2DAsset(entry.FullName),
							"anim3d" => GetAnim3DAsset(entry.FullName),
							_ => null!,
						};
						return asset!;
					}
				)
			);
		}

        public string ExportAllAssets(string path)
        {
			foreach (IAsset asset in assets)
            {
				Logger.Log($"{path}/{asset.AssetType}/{asset.Name}");
			}
			return path;
		}

        public IAsset GetAsset(int i) => assets[i];
		public Image GetImageAsset(string path) => Image.CreateFromFile(path);
		public Audio GetAudioAsset(string path) => Audio.CreateFromFile(path);
		public Text GetTextAsset(string path) => Text.CreateFromZippedFile(Assets.GetEntry(path)!.Open());
		public Shader GetShaderAsset(string path) => Shader.CreateFromFile(path);
		public Scene GetSceneAsset(string path) => Scene.CreateFromFile(path);
		public Anim2D GetAnim2DAsset(string path) => Anim2D.CreateFromPath(path);
		public Anim3D GetAnim3DAsset(string path) => Anim3D.CreateFromFile(path);

		public Image GetImageAsset(ZipArchiveEntry Entry) => Image.CreateFromFile(Entry.FullName);
		public Audio GetAudioAsset(ZipArchiveEntry Entry) => Audio.CreateFromFile(Entry.FullName);
		public Text GetTextAsset(ZipArchiveEntry Entry) => Text.CreateFromZippedFile(Entry.Open());
		public Shader GetShaderAsset(ZipArchiveEntry Entry) => Shader.CreateFromFile(Entry.FullName);
		public Scene GetSceneAsset(ZipArchiveEntry Entry) => Scene.CreateFromFile(Entry.FullName);
		public Anim2D GetAnim2DAsset(ZipArchiveEntry Entry) => Anim2D.CreateFromPath(Entry.FullName);
		public Anim3D GetAnim3DAsset(ZipArchiveEntry Entry) => Anim3D.CreateFromFile(Entry.FullName);
	}
}
