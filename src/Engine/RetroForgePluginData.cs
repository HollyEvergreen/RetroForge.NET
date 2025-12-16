using RetroForge.NET.Assets.Asset;
using glfwImage = OpenTK.Windowing.GraphicsLibraryFramework.Image;
using RetroForge.NET.Graphics; 


namespace RetroForge.NET
{
    public class RetroForgePluginData
    {
        private StyledString? _name;
        private Image thumbnail;
        private Text name;
        private Text style;


        public Texture ThumbnailTexture { get; }
        public StyledString Name
        {
            get
            {
                _name ??= new StyledString().Render(name, style);
                return _name.Value;
            }
        }
    }
}