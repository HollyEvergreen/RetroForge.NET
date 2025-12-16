namespace RetroForge.NET;

public struct Rect(int x, int y, int width, int height)
{
	public int x = x;
	public int y = y;
	public int width = width;
	public int height = height;

	public readonly (float[], int[][], float[]) ToVertArray()
	{
		return (
			[
				0, x, y, x + width, y + height, // vertices
			],
			[
				[1, 2, 0], [3, 2, 0], [1, 4, 0],// triangle 1
				[3, 2, 0], [3, 4, 0], [1, 4, 0] // triangle 2
            ],
			[ // tex coords
				0, 0, 
				1, 0, 
				0, 1, 
				1, 1
			]
		);
	}
}