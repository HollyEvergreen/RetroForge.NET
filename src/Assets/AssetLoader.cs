using RetroForge.NET.Assets.Asset;
using System.IO.Compression;

namespace RetroForge.NET.Assets
{
	public class AssetLoader(string path)
	{
		private readonly string LocalPath = ToLocal(path);
		private readonly ZipArchive Assets = ZipFile.Open(path, ZipArchiveMode.Update);
		private readonly List<IAsset> assets = [];
		public void LoadAllAssets()
		{
			Logger.Warn($"_path is {LocalPath}");
            try
            {
			    Assets.ExtractToDirectory(LocalPath);
            } catch (IOException err)
            {
                Logger.Error(err);
            }
            foreach (var file in Directory.EnumerateFiles(LocalPath))
            {
				Logger.Warn($"{file} is in dir {LocalPath}");
            }
            
			assets.AddRange(
				Assets.Entries.Select(
					(entry) => {
                        string _path = Path.Join(LocalPath, entry.FullName);
                        Logger.Warn($"Loading File {_path} ");
                        return ((IAsset)(_path.Split('.').Last().ToLower() switch
                        {
                            "png" => GetImageAsset(_path),
                            "jpeg" => GetImageAsset(_path),
                            "webp" => GetImageAsset(_path),
                            "frag" => GetShaderAsset(_path),
                            "vert" => GetShaderAsset(_path),
                            "tesc" => GetShaderAsset(_path),
                            "tese" => GetShaderAsset(_path),
                            "geom" => GetShaderAsset(_path),
                            "comp" => GetShaderAsset(_path),
                            "wav" => GetAudioAsset(_path),
                            "mp3" => GetAudioAsset(_path),
                            "aac" => GetAudioAsset(_path),
                            "ogg" => GetAudioAsset(_path),
                            "flac" => GetAudioAsset(_path),
                            "alac" => GetAudioAsset(_path),
                            "aiff" => GetAudioAsset(_path),
                            "txt" => GetTextAsset(_path),
                            "md" => GetTextAsset(_path),
                            "scn" => GetSceneAsset(_path),
                            "anim2d" => GetAnim2DAsset(_path),
                            "anim3d" => GetAnim3DAsset(_path),
                            _ => null!,
                        }))!;
					}
				)
			);
		}
        public static string ToLocal(string __path) => __path.Replace(".zip", null);
        public int NAssets => assets.Count;
        public string ExportAllAssets(string path)
        {
			foreach (IAsset asset in assets)
            {
				Logger.Log($"{path}/{asset.AssetType}/{asset.Name}");
			}
			return path;
		}
		public void UnloadAllAssets()
		{
			assets.ForEach((asset) => { asset.Dispose(); });
            assets.Clear();
            Directory.Delete(LocalPath, true);
            Assets.Dispose();
        }
        public IAsset GetAsset(int i) => assets[i];
        public Image GetImageAsset(string path)
        {
            try
            {
                ArgumentException.ThrowIfNullOrEmpty(path);
                return Image.CreateFromFile(path);
            }
            catch (FileNotFoundException err){
                Logger.Error(err);
            }
            return new Image();
        }
        public Audio GetAudioAsset(string path)
        {
            try {
                ArgumentException.ThrowIfNullOrEmpty(path);
            return Audio.CreateFromFile(path);
            }
            catch (FileNotFoundException err){
                Logger.Error(err);
            }
            return new Audio();
        }
        public Text GetTextAsset(string path)
        {
            try {
            ArgumentException.ThrowIfNullOrEmpty(path);
                return Text.CreateFromFile(path);
            }
            catch (FileNotFoundException err){
                Logger.Error(err);
            }
            return new Text();
        }
        public Shader GetShaderAsset(string path)
        {
            return new Shader();
        }
        public Scene GetSceneAsset(string path)
        {
            try {
            ArgumentException.ThrowIfNullOrEmpty(path);
            return Scene.CreateFromFile(path);
            }
            catch (FileNotFoundException err){
                Logger.Error(err);
            }
            return new Scene();
        }
        public Anim2D GetAnim2DAsset(string path)
        {
            try {
            ArgumentException.ThrowIfNullOrEmpty(path);
            return Anim2D.CreateFromPath(path);
            }
            catch (FileNotFoundException err){
                Logger.Error(err);
            }
            return new Anim2D();
        }
        public Anim3D GetAnim3DAsset(string path)
        {
            try {
            ArgumentException.ThrowIfNullOrEmpty(path);
            return Anim3D.CreateFromFile(path);
            }
            catch (FileNotFoundException err){
                Logger.Error(err);
            }
            return new Anim3D();
        }
        public Config GetConfigAsset(string path)
        {
            try {
                ArgumentException.ThrowIfNullOrEmpty(path);
                return Config.CreateFromFile(path);
            }
            catch (FileNotFoundException err){
                Logger.Error(err);
            }
            return new Config();
        }
    }
}