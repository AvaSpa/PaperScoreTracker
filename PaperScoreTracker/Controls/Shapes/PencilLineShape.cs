namespace PaperScoreTracker.Controls.Shapes;

using Microsoft.Maui.Graphics;

public enum LineOrientation { Horizontal, Vertical }

public class PencilLineShape : IShape
{
    public LineOrientation Orientation { get; set; } = LineOrientation.Horizontal;
    public double Jitter { get; set; } = 1.8;
    public int Seed { get; set; } = 42;
    public int PassCount { get; set; } = 2;
    public int Segments { get; set; } = 10;

    // extends the line past its own bounds on both ends to counteract
    // Border's automatic stroke-thickness inset, and to guarantee
    // overlap (not just touching) between adjacent column lines
    public double Overshoot { get; set; } = 3;

    public PathF PathForBounds(Rect rect)
    {
        var path = new PathF();
        for (int pass = 0; pass < PassCount; pass++)
            AddSketchyLine(path, rect, Seed + pass * 733, pass);
        return path;
    }

    void AddSketchyLine(PathF path, Rect rect, int seed, int pass)
    {
        var rnd = new Random(seed);
        bool horizontal = Orientation == LineOrientation.Horizontal;

        double start = (horizontal ? rect.X : rect.Y) - Overshoot;
        double length = (horizontal ? rect.Width : rect.Height) + Overshoot * 2;
        double crossCenter = horizontal ? rect.Y + rect.Height / 2 : rect.X + rect.Width / 2;
        double passOffset = (pass - (PassCount - 1) / 2.0) * (Jitter * 0.6);

        Point PointAt(int i)
        {
            double t = (double)i / Segments;
            double along = start + length * t;
            double cross = crossCenter + passOffset + (rnd.NextDouble() - 0.5) * Jitter;
            return horizontal ? new Point(along, cross) : new Point(cross, along);
        }

        var first = PointAt(0);
        path.MoveTo((float)first.X, (float)first.Y);
        for (int i = 1; i <= Segments; i++)
        {
            var p = PointAt(i);
            path.LineTo((float)p.X, (float)p.Y);
        }
    }
}