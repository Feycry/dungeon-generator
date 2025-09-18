using SkiaSharp;

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

		using var surface = SKSurface.Create(new SKImageInfo(imgWidth, imgHeight));
		var canvas = surface.Canvas;
		canvas.Clear(SKColors.White);

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
			using var paint = new SKPaint();
			paint.Color = SKColors.Black;
			paint.TextSize = 14;
			paint.Typeface = SKTypeface.FromFamilyName("Arial");
			var textBounds = new SKRect();
			paint.MeasureText(title, ref textBounds);
			float textX = (imgWidth - textBounds.Width) / 2f;
			float textY = 20 - textBounds.Top; // Adjust for baseline
			canvas.DrawText(title, textX, textY, paint);
		}

		// Optionally, draw the dungeon rectangle
		var rectX = offsetX + marginX * scale;
		var rectY = offsetY + marginY * scale;
		var rectW = dungeonWidth * scale;
		var rectH = dungeonHeight * scale;
		using (var paint = new SKPaint())
		{
			paint.Color = SKColors.LightGray;
			paint.Style = SKPaintStyle.Stroke;
			paint.StrokeWidth = 1;
			canvas.DrawRect((float)rectX, (float)rectY, (float)rectW, (float)rectH, paint);
		}

		// Draw edges (lines)
		using (var linePaint = new SKPaint())
		{
			linePaint.Color = SKColors.Black;
			linePaint.Style = SKPaintStyle.Stroke;
			linePaint.StrokeWidth = 1;
			
			foreach (var (x1, y1, x2, y2) in lines)
			{
				float sx1 = (float)(offsetX + (x1 + marginX) * scale);
				float sy1 = (float)(offsetY + (y1 + marginY) * scale);
				float sx2 = (float)(offsetX + (x2 + marginX) * scale);
				float sy2 = (float)(offsetY + (y2 + marginY) * scale);
				canvas.DrawLine(sx1, sy1, sx2, sy2, linePaint);
			}
		}

		// Draw nodes (circles)
		using (var fillPaint = new SKPaint())
		{
			fillPaint.Color = SKColors.LightBlue;
			fillPaint.Style = SKPaintStyle.Fill;
			
			using (var strokePaint = new SKPaint())
			{
				strokePaint.Color = SKColors.Black;
				strokePaint.Style = SKPaintStyle.Stroke;
				strokePaint.StrokeWidth = 1;
				
				foreach (var (x, y) in points)
				{
					float sx = (float)(offsetX + (x + marginX) * scale);
					float sy = (float)(offsetY + (y + marginY) * scale);
					canvas.DrawCircle(sx, sy, 8, fillPaint);
					canvas.DrawCircle(sx, sy, 8, strokePaint);
				}
			}
		}

		// Save the image
		using var image = surface.Snapshot();
		using var data = image.Encode(SKEncodedImageFormat.Png, 100);
		using var stream = File.OpenWrite(fileName);
		data.SaveTo(stream);
	}
}
