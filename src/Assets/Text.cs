namespace RetroForge.NET.Assets
{
    namespace Asset
    {
        public struct Text : IAsset
        {
            public readonly AssetType AssetType => AssetType.Text;
            public readonly string Name => ToString()!;
            public string _Text { get; private set; }
            public static Text CreateFromZippedFile(Stream file)
            {
                string str = "";
                while (true)
                {
                    var _byte = file.ReadByte();
                    if (_byte == -1)
                    {
                        break;
                    }
                    str += (char)_byte;
                }
                return new() { _Text = str };
            }
            public static async Task<Text> CreateFromFileAsync(string fullName) => new() { _Text = await File.ReadAllTextAsync(fullName) };
            public static Text CreateFromFile(string fullName) => new() { _Text = File.ReadAllText(fullName) };
            public readonly void Dispose(){}
        }
    }
}