using OpenTK.Graphics.OpenGL;
using StbImageSharp;

namespace RetroForge.NET.Graphics;

public struct Texture
{
	public int ID { readonly get; private set; }
	public byte[] Data { readonly get; private set; }
	public int width;
	public int height;
	public ColorComponents components = ColorComponents.RedGreenBlueAlpha;

    public readonly ImageResult? Img => ImageResult.FromMemory(Data, components);

	
	public Texture(string path, bool upload_to_gpu = true)
	{
		StbImage.stbi_set_flip_vertically_on_load(1);
		ImageResult image = ImageResult.FromStream(File.OpenRead(path), components);
		Data = image.Data;
		if (upload_to_gpu == true) UploadToGPU(image);
		else Logger.Warn($"LOADED {path} BUT DID NOT UPLOAD TO GPU");
	}
	unsafe void UploadToGPU(ImageResult? image = null)
	{
		image ??= Img; // sets image to the currently stored image if no image is supplied
		ID = GL.GenTexture();
		GL.BindTexture(TextureTarget.Texture2d, ID);
		fixed (void* ptr = &Data[0]) 
			GL.TexImage2D(
				TextureTarget.Texture2d,
				0,
				InternalFormat.Rgba,
				image.Width,
				image.Height,
				0,
				PixelFormat.Rgba,
				PixelType.Byte,
				ptr
			);
	}
}