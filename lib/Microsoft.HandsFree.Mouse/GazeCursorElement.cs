namespace Microsoft.HandsFree.Mouse
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using System.Windows.Interop;

    /// <summary>
    /// A container element which has a cursor drawn upon.
    /// Experimentation proved that we needed a custom FrameworkElement that has a cursor drawn using the Visual layer
    /// as the simpler approach of using a Canvas containing an Ellipse interfered with the click-through processing
    /// of the activation.  Various attempts using IsHitTestVisible et al did not work.
    /// </summary>
    class GazeCursorElement : FrameworkElement
    {
        private readonly VisualCollection _children;

        private readonly DrawingVisual _cursor;
        private bool _isVisible = true;

        public int Radius { get; set; }
        public Brush Brush { get; set; }
        public Pen Pen { get; set; }
        public int MaxTrackCount { get; set; }

        private Point _lastPoint;

        private Rect _cursorThumbnailRect;
        private DrawingVisual _cursorThumbnail;
        private Brush _cursorThumbnailBrush;
        private int _cursorThumbnailRadius;

        public GazeCursorElement()
        {
            _children = new VisualCollection(this);
            _cursor = new DrawingVisual();
            _cursorThumbnail = new DrawingVisual();
            _children.Add(_cursor);
            _children.Add(_cursorThumbnail);

            CreateThumbnailRect();

            MaxTrackCount = 30;

            IsHitTestVisible = false;
        }

        private void CreateThumbnailRect()
        {
            _cursorThumbnailRadius = 4;
            _cursorThumbnailBrush = new SolidColorBrush(Colors.DarkGray);
            var pen = new Pen(_cursorThumbnailBrush, 1);
            var workArea = System.Windows.Forms.Screen.PrimaryScreen.Bounds;

            PresentationSource presentationSource;
            {
                var enumerator = PresentationSource.CurrentSources.GetEnumerator();
                enumerator.MoveNext();
                presentationSource = (PresentationSource)(enumerator.Current);
            }
            var scaleX = presentationSource.CompositionTarget.TransformToDevice.M11;
            var scaleY = presentationSource.CompositionTarget.TransformToDevice.M22;

            var height = 32;
            var width = (int)(height * (workArea.Width * 1.0 / workArea.Height));
            var top = workArea.Height - 3 * height / 2;
            var left = (13.0 * workArea.Width / 24.0) - (width / 2.0);
            var offset = _cursorThumbnailRadius / 2;
            _cursorThumbnailRect = new Rect(left / scaleX, top / scaleY, width / scaleX, height / scaleY);

            var thumbnail = new DrawingVisual();
            var drawingContext = thumbnail.RenderOpen();
            drawingContext.DrawRectangle(null, pen, _cursorThumbnailRect);
            drawingContext.Close();

            _children.Add(thumbnail);
        }

        public void DrawCursorThumbnail(Point point)
        {
            var workArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            var thumbnailPoint = new Point(_cursorThumbnailRect.Left + (point.X * _cursorThumbnailRect.Width / workArea.Width) + (_cursorThumbnailRadius / 2),
                                           _cursorThumbnailRect.Top + (point.Y * _cursorThumbnailRect.Height / workArea.Height) + (_cursorThumbnailRadius / 2));
            var drawingContext = _cursorThumbnail.RenderOpen();
            drawingContext.DrawEllipse(_cursorThumbnailBrush, null, thumbnailPoint, _cursorThumbnailRadius, _cursorThumbnailRadius);
            drawingContext.Close();

        }

        private void DrawCursor(DrawingVisual cursor, Point point)
        {
            var drawingContext = cursor.RenderOpen();
            drawingContext.DrawEllipse(Brush, null, point, Radius, Radius);
            drawingContext.Close();
        }

        private void DrawTrack(DrawingVisual line, Point point)
        {
            var drawingContext = line.RenderOpen();
            drawingContext.DrawLine(Pen, _lastPoint, point);
            drawingContext.Close();
        }

        public void SetCursor(double x, double y)
        {
            _isVisible = true;
            _lastPoint = new Point(x + Radius, y + Radius);
            DrawCursor(_cursor, _lastPoint);
        }

        public void HideCursor()
        {
            var drawingContext = _cursor.RenderOpen();
            drawingContext.Close();

            drawingContext = _cursorThumbnail.RenderOpen();
            drawingContext.Close();
        }

        public void AddCursorPosition(double x, double y, bool drawTrack)
        {
            Point newPoint = new Point(x, y);
            if (drawTrack)
            {
                DrawingVisual line = new DrawingVisual();
                _children.Add(line);
                DrawTrack(line, newPoint);

            }

            DrawingVisual circle = new DrawingVisual();
            _children.Add(circle);
            DrawCursor(circle, newPoint);
            _lastPoint = newPoint;

            while (_children.Count > MaxTrackCount)
            {
                _children.RemoveAt(0);
            }
        }

        public void ClearTracks()
        {
            _children.Clear();
            _children.Add(_cursor);
        }

        public new bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                if (!_isVisible)
                {
                    var drawingContext = _cursor.RenderOpen();
                    drawingContext.Close();
                }
            }
        }

        protected override int VisualChildrenCount
        {
            get { return _children.Count; }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= _children.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return _children[index];
        }
    }
}
