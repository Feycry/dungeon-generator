using System.Drawing;
using System.Drawing.Imaging;

public static class DrawingTest
{
	public static void Run()
	{
		int width = 400, height = 400;
		using var bmp = new Bitmap(width, height);
		using var g = Graphics.FromImage(bmp);
		g.Clear(Color.White);

		// Draw nodes (circles)
		var nodes = new[] { (100, 100), (300, 100), (200, 300) };
		foreach (var (x, y) in nodes)
		{
			g.FillEllipse(Brushes.LightBlue, x - 20, y - 20, 40, 40);
			g.DrawEllipse(Pens.Black, x - 20, y - 20, 40, 40);
		}

		// Draw edges (lines)
		g.DrawLine(Pens.Black, nodes[0].Item1, nodes[0].Item2, nodes[1].Item1, nodes[1].Item2);
		g.DrawLine(Pens.Black, nodes[1].Item1, nodes[1].Item2, nodes[2].Item1, nodes[2].Item2);
		g.DrawLine(Pens.Black, nodes[2].Item1, nodes[2].Item2, nodes[0].Item1, nodes[0].Item2);

		// Save image
	string fileName = Path.Combine("../../../../images", "graph_test.png");
	bmp.Save(fileName, ImageFormat.Png);
	System.Console.WriteLine($"Graph image saved as {fileName}");
	}
}
