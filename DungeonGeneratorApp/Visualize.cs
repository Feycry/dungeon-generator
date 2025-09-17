using System.Drawing;
using System.Drawing.Imaging;

namespace DungeonGeneratorApp;

public static class Visualize
{
	// Draws a graph from points and lines, saves to fileName
	// Optionally, category and stepNumber can be provided for title
	public static void DrawGraph(
		List<(double x, double y)> points,
		List<(double x1, double y1, double x2, double y2)> lines,
		string fileName,
		string? category = null,
		int? stepNumber = null)
	{
		int imgWidth = 800, imgHeight = 600;
		// Get dungeon bounds
		var (dungeonWidth, dungeonHeight) = DebugSnapshotManager.Instance.GetDungeonBounds();
		// Margin as a fraction of dungeon size (adjusted to accommodate super triangle)
		double marginFrac = 0.3; // Reduced from 1.0 to 0.3 for better balance
		double marginX = dungeonWidth * marginFrac;
		double marginY = dungeonHeight * marginFrac;
		double totalWidth = dungeonWidth + 2 * marginX;
		double totalHeight = dungeonHeight + 2 * marginY;

		// Scaling factors
		double scaleX = imgWidth / totalWidth;
		double scaleY = imgHeight / totalHeight;
		double scale = Math.Min(scaleX, scaleY);

		// Offset to center the dungeon
		double offsetX = (imgWidth - dungeonWidth * scale) / 2.0 - marginX * scale;
		double offsetY = (imgHeight - dungeonHeight * scale) / 2.0 - marginY * scale;

		using var bmp = new Bitmap(imgWidth, imgHeight);
		using var g = Graphics.FromImage(bmp);
		g.Clear(Color.White);

		// Draw title (step number and category) at the top
		string title = "";
		if (stepNumber.HasValue)
		{
			title = $"step {stepNumber.Value}";
			if (!string.IsNullOrEmpty(category))
			{
				title += $": {category}";
			}
		}
		else if (!string.IsNullOrEmpty(category))
		{
			title = category;
		}
		if (title.Length > 0)
		{
			using var font = new Font("Arial", 14);
			var textSize = g.MeasureString(title, font);
			float textX = (imgWidth - textSize.Width) / 2f;
			float textY = 8;
			g.DrawString(title, font, Brushes.Black, textX, textY);
		}

		// Optionally, draw the dungeon rectangle
		var rectX = offsetX + marginX * scale;
		var rectY = offsetY + marginY * scale;
		var rectW = dungeonWidth * scale;
		var rectH = dungeonHeight * scale;
		using (var pen = new Pen(Color.LightGray, 1))
		{
			g.DrawRectangle(pen, (float)rectX, (float)rectY, (float)rectW, (float)rectH);
		}

		// Draw edges (lines)
		foreach (var (x1, y1, x2, y2) in lines)
		{
			float sx1 = (float)(offsetX + (x1 + marginX) * scale);
			float sy1 = (float)(offsetY + (y1 + marginY) * scale);
			float sx2 = (float)(offsetX + (x2 + marginX) * scale);
			float sy2 = (float)(offsetY + (y2 + marginY) * scale);
			g.DrawLine(Pens.Black, sx1, sy1, sx2, sy2);
		}

		// Draw nodes (circles)
		foreach (var (x, y) in points)
		{
			float sx = (float)(offsetX + (x + marginX) * scale);
			float sy = (float)(offsetY + (y + marginY) * scale);
			g.FillEllipse(Brushes.LightBlue, sx - 8, sy - 8, 16, 16);
			g.DrawEllipse(Pens.Black, sx - 8, sy - 8, 16, 16);
		}

		bmp.Save(fileName, ImageFormat.Png);
	}
}
