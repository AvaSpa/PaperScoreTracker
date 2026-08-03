namespace PaperScoreTracker.Controls.Shapes;

public class PencilBorderShape : IShape
{
    public double CornerRadius { get; set; } = 6;
    public double Jitter { get; set; } = 2.5;
    public int Seed { get; set; } = 12345;      // fixed = stable wobble across relayouts
    public int PassCount { get; set; } = 2;     // "redrawn" pencil lines
    public int PointsPerEdge { get; set; } = 6;

    public PathF PathForBounds(Rect rect)
    {
        var path = new PathF();
        for (var pass = 0; pass < PassCount; pass++)
        {
            var inset = pass * 0.8;
            var passRect = rect.Inflate(-inset, -inset);
            AddSketchyRoundedRect(path, passRect, Seed + pass * 733);
        }
        return path;
    }

    void AddSketchyRoundedRect(PathF path, Rect rect, int seed)
    {
        var rnd = new Random(seed);
        var pts = BuildRoundedRectOutline(rect, CornerRadius, PointsPerEdge);

        Point Jittered(Point p) => new(
            p.X + (rnd.NextDouble() - 0.5) * Jitter,
            p.Y + (rnd.NextDouble() - 0.5) * Jitter);

        var first = Jittered(pts[0]);
        path.MoveTo((float)first.X, (float)first.Y);
        for (var i = 1; i < pts.Count; i++)
        {
            var p = Jittered(pts[i]);
            path.LineTo((float)p.X, (float)p.Y);
        }
        path.Close();
    }

    static List<Point> BuildRoundedRectOutline(Rect r, double radius, int perEdge)
    {
        radius = Math.Min(radius, Math.Min(r.Width, r.Height) / 2);
        var pts = new List<Point>();

        void Corner(double cx, double cy, double startAngle, double endAngle)
        {
            var steps = 4;
            for (var i = 0; i <= steps; i++)
            {
                var t = startAngle + (endAngle - startAngle) * i / steps;
                pts.Add(new Point(cx + radius * Math.Cos(t), cy + radius * Math.Sin(t)));
            }
        }

        void Edge(Point a, Point b)
        {
            for (var i = 0; i <= perEdge; i++)
            {
                var t = (double)i / perEdge;
                pts.Add(new Point(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t));
            }
        }

        double left = r.X, top = r.Y, right = r.X + r.Width, bottom = r.Y + r.Height;

        Edge(new Point(left + radius, top), new Point(right - radius, top));
        Corner(right - radius, top + radius, -Math.PI / 2, 0);
        Edge(new Point(right, top + radius), new Point(right, bottom - radius));
        Corner(right - radius, bottom - radius, 0, Math.PI / 2);
        Edge(new Point(right - radius, bottom), new Point(left + radius, bottom));
        Corner(left + radius, bottom - radius, Math.PI / 2, Math.PI);
        Edge(new Point(left, bottom - radius), new Point(left, top + radius));
        Corner(left + radius, top + radius, Math.PI, Math.PI * 1.5);

        return pts;
    }
}