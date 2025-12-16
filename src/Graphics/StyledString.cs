using OpenTK.Mathematics;
using RetroForge.NET.Assets.Asset;
namespace RetroForge.NET.Graphics;

public struct StyledString
{
    public Texture texture { get; private set; }

    public StyledString Render(Text text, Text style)
    {
        return Render(text._Text, style._Text);
    }
    public StyledString Render(string text, string style)
    {
        string[] _style = style.Split('\n');
        // font height in pixelFontss
        Font FontAtlasIndex = Engine.Fonts[_style[0]];
        Color3<Hsv> font_colour = FontColour(_style[1]);
        return this;
    }

    /// <summary>
    /// takes a string and returns a Color3<Hsv>
    /// </summary>
    /// <param name="EncFontColour"> string in the format `hue/sat/val`</param>
    /// <returns> Color3<Hsv> </returns>
    public static Color3<Hsv> FontColour(string EncFontColour)
    {
        if (EncFontColour == "" || EncFontColour is null)
        {
            return new(0, 0, 0);
        }
        var _colourComponents = EncFontColour.Split('/');
        return new(int.Parse(_colourComponents[0]), int.Parse(_colourComponents[1]), int.Parse(_colourComponents[2]));
        
    }
}