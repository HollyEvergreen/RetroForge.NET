using NAudio.Wave;
using RetroForge.NET.Graphics;

namespace RetroForge.NET;

public struct Font
{
	Texture FontAtlas;
	int Glyphs;
	Rect GlyphDimensions;

	public readonly Rect GetGlyphRect(int index) => new Rect(GlyphDimensions.x + index * GlyphDimensions.width, 0, GlyphDimensions.width, GlyphDimensions.height);
}
