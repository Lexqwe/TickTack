
namespace TickTack.Drawables;

public class ClockDrawable : IDrawable
{
    public TimeOnly CurrentTime { get; set; }

    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.FontColor = Colors.Black;
        canvas.FontSize = 24;
        float cx = rect.Center.X;
        float cy = rect.Center.Y;
        float radius = Math.Min(rect.Width, rect.Height) / 2f;

        canvas.DrawString(
            CurrentTime.ToString("HH:mm:ss"),
            cx - 50, cy - 50,
            100, 30,
            HorizontalAlignment.Center, VerticalAlignment.Center);

        canvas.DrawCircle(cx, cy, radius);

        float numberRadius = radius - 45f;
        for (int hr = 1; hr <= 12; hr++)
        {
            float angleRad = (hr * 30f - 90f) * MathF.PI / 180f;
            float cos = MathF.Cos(angleRad);
            float sin = MathF.Sin(angleRad);
            float x = cx + numberRadius * cos;
            float y = cy + numberRadius * sin;
            canvas.DrawString(
                hr.ToString(),
                x - 15,
                y - 15,
                30,
                30,
                HorizontalAlignment.Center,
                VerticalAlignment.Center
                );
        }
        for (int i = 0; i < 60; i++)
        {
            float angleRad = (i * 6f - 90f) * MathF.PI / 180f;
            float cos = MathF.Cos(angleRad);
            float sin = MathF.Sin(angleRad);
            float outerRadius = radius - 5f;
            float innerRadius = (i % 5 == 0) ? radius - 30f : radius - 15f;
            float x1 = cx + outerRadius * cos;
            float y1 = cy + outerRadius * sin;
            float x2 = cx + innerRadius * cos;
            float y2 = cy + innerRadius * sin;
            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = (i % 5 == 0) ? 3f : 1f;
            canvas.DrawLine(x1, y1, x2, y2);
        }


        float secondAngle = CurrentTime.Second * 6f;
        float minuteAngle = (CurrentTime.Minute + CurrentTime.Second / 60f) * 6f;
        float hourAngle = (CurrentTime.Hour % 12 + CurrentTime.Minute / 60f) * 30f;

        canvas.SaveState();
        canvas.Translate(cx, cy);
        canvas.Rotate(hourAngle);
        canvas.StrokeColor = Colors.Black;
        canvas.StrokeSize = 6;
        canvas.DrawLine(0, 0, 0, -(radius - 90f));
        canvas.RestoreState();

        canvas.SaveState();
        canvas.Translate(cx, cy);
        canvas.Rotate(minuteAngle);
        canvas.StrokeColor = Colors.Black;
        canvas.StrokeSize = 4;
        canvas.DrawLine(0, 0, 0, -(radius - 40f));
        canvas.RestoreState();

        canvas.SaveState();
        canvas.Translate(cx, cy);
        canvas.Rotate(secondAngle);
        canvas.StrokeColor = Colors.Red;
        canvas.StrokeSize = 2;
        canvas.DrawLine(0, 0, 0, -(radius - 20f));
        canvas.RestoreState();

        canvas.FillColor = Colors.Black;
        canvas.FillCircle(cx, cy, 5);
    }
}
