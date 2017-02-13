//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace ActivatableButtonsForms
{
    using System;
    using System.Drawing;

    /// <summary>
    /// Represents a color value in the Hue-Saturation-Brightness color space.
    /// Can be used to convert between RGB and HSB colors.
    /// </summary>
    public class HsbColor
    {
        private float _hue;
        private float _saturation;
        private float _brightness;

        public HsbColor(float hue, float saturation, float brightness)
        {
            _hue = ClampPeriodical(hue);
            _saturation = Clamp(saturation);
            _brightness = Clamp(brightness);
        }

        public HsbColor(Color rgb)
        {
            var cmax = Math.Max(rgb.R, Math.Max(rgb.G, rgb.B));
            var cmin = Math.Min(rgb.R, Math.Min(rgb.G, rgb.B));

            _brightness = ((float)cmax) / 255.0f;
            _saturation = (cmax != 0) ? ((float)(cmax - cmin)) / ((float)cmax) : 0;

            if (_saturation != 0)
            {
                float redc = ((float)(cmax - rgb.R)) / ((float)(cmax - cmin));
                float greenc = ((float)(cmax - rgb.G)) / ((float)(cmax - cmin));
                float bluec = ((float)(cmax - rgb.B)) / ((float)(cmax - cmin));
                if (rgb.R == cmax)
                    _hue = bluec - greenc;
                else if (rgb.G == cmax)
                    _hue = 2.0f + redc - bluec;
                else
                    _hue = 4.0f + greenc - redc;

                _hue = _hue / 6.0f;
                if (_hue < 0)
                    _hue = _hue + 1.0f;
            }
            else
            {
                _hue = 0;
            }
        }

        public float Hue
        {
            get { return _hue; }
            set { _hue = ClampPeriodical(value); }
        }

        public float Saturation
        {
            get { return _saturation; }
            set { _saturation = Clamp(value); }
        }

        public float Brightness
        {
            get { return _brightness; }
            set { _brightness = Clamp(value); }
        }

        public Color ToRgb()
        {
            if (_saturation != 0)
            {
                float h = (_hue - (float)Math.Floor(_hue)) * 6.0f;
                float f = h - (float)Math.Floor(h);
                float p = _brightness * (1.0f - _saturation);
                float q = _brightness * (1.0f - _saturation * f);
                float t = _brightness * (1.0f - (_saturation * (1.0f - f)));
                switch ((int)h)
                {
                    case 0:
                        return Color.FromArgb((int)(_brightness * 255.0f + 0.5f), (int)(t * 255.0f + 0.5f), (int)(p * 255.0f + 0.5f));
                    case 1:
                        return Color.FromArgb((int)(q * 255.0f + 0.5f), (int)(_brightness * 255.0f + 0.5f), (int)(p * 255.0f + 0.5f));
                    case 2:
                        return Color.FromArgb((int)(p * 255.0f + 0.5f), (int)(_brightness * 255.0f + 0.5f), (int)(t * 255.0f + 0.5f));
                    case 3:
                        return Color.FromArgb((int)(p * 255.0f + 0.5f), (int)(q * 255.0f + 0.5f), (int)(_brightness * 255.0f + 0.5f));
                    case 4:
                        return Color.FromArgb((int)(t * 255.0f + 0.5f), (int)(p * 255.0f + 0.5f), (int)(_brightness * 255.0f + 0.5f));
                    case 5:
                        return Color.FromArgb((int)(_brightness * 255.0f + 0.5f), (int)(p * 255.0f + 0.5f), (int)(q * 255.0f + 0.5f));
                }
            }

            int c = (int)(_brightness * 255.0f + 0.5f);
            return Color.FromArgb(c, c, c);
        }

        private static float Clamp(float value)
        {
            return Math.Max(0, Math.Min(1, value));
        }

        private static float ClampPeriodical(float value)
        {
            // remove any full periods, constraining the value to [-1, 1)
            value = value - (float)Math.Floor(value);

            // push into [0, 1)
            if (value < 0)
            {
                value += 1;
            }

            return value;
        }
    }
}
