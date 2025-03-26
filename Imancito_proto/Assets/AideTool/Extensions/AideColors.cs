using System;
using UnityEngine;

namespace AideTool.Extensions
{
    public static class AideColors
    {
        public static Color Dark => new(0.1412f, 0.1608f, 0.1804f);
        public static Color LightDark => new(0.1765f, 0.1765f, 0.1804f);
        public static Color White => new(0.9725f, 0.9765f, 0.9804f);
        public static Color DarkWhite => new(0.7725f, 0.7765f, 0.7804f);
        public static Color Gray => new(0.2039f, 0.2275f, 0.251f);
        public static Color DarkGray => new(0.0392f, 0.0471f, 0.051f);
        public static Color LightGray => new(0.251f, 0.251f, 0.251f);
        public static Color Blue => new(0f, 0.1f, 1f);
        public static Color DarkBlue => new(0f, 0.388f, 0.8f);
        public static Color LightBlue => new(0.2f, 0.5882f, 1f);
        public static Color Indigo => new(0.4f, 0.0627f, 0.949f);
        public static Color DarkIndigo => new(0.3216f, 0.051f, 0.749f);
        public static Color LightIndigo => new(0.5216f, 0.2549f, 0.949f);
        public static Color Lila => new(0.4353f, 0.2588f, 0.7569f);
        public static Color DarkLila => new(0.3216f, 0.1922f, 0.5608f);
        public static Color LightLila => new(0.533f, 0.4118f, 0.7608f);
        public static Color Rosa => new(0.9098f, 0.2431f, 0.549f);
        public static Color DarkRosa => new(0.7098f, 0.1922f, 0.4353f);
        public static Color LightRosa => new(0.9098f, 0.4275f, 0.651f);
        public static Color Red => new(0.8431f, 0.1333f, 0.102f);
        public static Color DarkRed => new(0.6392f, 0.1509f, 0.0784f);
        public static Color LightRed => new(0.8392f, 0.298f, 0.2706f);
        public static Color Orange => new(0.9922f, 0.4941f, 0.0784f);
        public static Color DarkOrange => new(0.7882f, 0.3922f, 0.0627f);
        public static Color LightOrange => new(0.9882f, 0.5961f, 0.2784f);
        public static Color Yellow => new(1f, 0.7569f, 0.0275f);
        public static Color DarkYellow => new(0.8f, 0.6078f, 0.0235f);
        public static Color LightYellow => new(1f, 0.8078f, 0.2314f);
        public static Color Green => new(0.1569f, 0.6549f, 0.2706f);
        public static Color DarkGreen => new(0.1908f, 0.451f, 0.1882f);
        public static Color LightGreen => new(0.2863f, 0.651f, 0.3725f);
        public static Color Teal => new(0.1255f, 0.7882f, 0.5922f);
        public static Color DarkTeal => new(0.0941f, 0.5882f, 0.4431f);
        public static Color LightTeal => new(0.2863f, 0.7882f, 0.6392f);
        public static Color Cyan => new(0.1098f, 0.8039f, 0.9098f);
        public static Color DarkCyan => new(0.0863f, 0.6275f, 0.7098f);
        public static Color LightCyan => new(0.2902f, 0.8275f, 0.09098f);

        public static Color Clear => new(0f, 0f, 0f, 0f);
        public static Color Transparent => Clear;

        public static Color ChangeOpacity(this Color color, float opacity)
        {
            if (opacity > 1f)
                opacity = 1f;

            if (opacity < 0f)
                opacity = 0f;

            return new Color(color.r, color.g, color.b, opacity);
        }

        public static Color QuadraticLerp(Color a, Color b, float t)
        {
            float rc = AideMath.QuadraticLerp(a.r, b.r, t);
            float gc = AideMath.QuadraticLerp(a.g, b.g, t);
            float bc = AideMath.QuadraticLerp(a.b, b.b, t);
            float ac = AideMath.QuadraticLerp(a.a, b.a, t);
            return new Color(rc, gc, bc, ac);
        }

        public static Color HexToColor(string hex)
        {
            bool isvalidFormat = true;

            if (hex.Length < 3 && hex.Length > 9)
                isvalidFormat = false;

            string validChars = "#0123456789abcdef";
            foreach (char c in hex.ToLower())
                if (!validChars.Contains(c))
                    isvalidFormat = false;

            if (!isvalidFormat)
                throw new FormatException(nameof(hex));

            if (hex[0] == '#')
                hex = hex.Remove(0, 1);

            if (hex.Length == 8)
                return ARGBHexToColor(hex);

            if (hex.Length == 6)
                return RGBHexToColor(hex);

            if(hex.Length == 4)
            {
                hex = DuplicateChannels(hex);
                return ARGBHexToColor(hex);
            }

            hex = DuplicateChannels(hex);
            return RGBHexToColor(hex);
        }

        private static float[] ProcessChannelCodes(string hex)
        {
            int length = hex.Length / 2;

            byte[] channelBytes = new byte[length];
            for (int i = 0; i < length; i++)
            {
                string channelHex = hex.Substring(i * 2, 2);
                channelBytes[i] = Convert.ToByte(channelHex, 16);
            }

            float[] channelPercs = new float[length];
            for(int i = 0; i < length; i++)
                channelPercs[i] = (float)channelBytes[i] / (float)byte.MaxValue;

            return channelPercs;
        }

        private static Color ARGBHexToColor(string argb)
        {
            float[] channels = ProcessChannelCodes(argb);
            return new Color(channels[1], channels[2], channels[3], channels[0]);
        }

        private static Color RGBHexToColor(string rgb)
        {
            float[] channels = ProcessChannelCodes(rgb);
            return new Color(channels[0], channels[1], channels[2]);
        }

        private static string DuplicateChannels(string hex)
        {
            string result = string.Empty;

            foreach (char c in hex)
                result = $"{result}{c}{c}";

            return result;
        }

        public static string ColorToHex(this Color color)
        {
            byte[] channels = new byte[]
            {
                (byte) (color.a * byte.MaxValue),
                (byte) (color.r * byte.MaxValue),
                (byte) (color.g * byte.MaxValue),
                (byte) (color.b * byte.MaxValue)
            };

            string[] channelsHex = new string[]
            {
                channels[0].ToString("x2"),
                channels[1].ToString("x2"),
                channels[2].ToString("x2"),
                channels[3].ToString("x2")
            };

            if (channels[0] == byte.MaxValue)
                return $"#{channelsHex[1]}{channelsHex[2]}{channelsHex[3]}";

            return $"#{channelsHex[0]}{channelsHex[1]}{channelsHex[2]}{channelsHex[3]}";
        }

        public static Texture MakeTexture(this Color color)
        {
            Color32[] pix = new Color32[16];
            for (int i = 0; i < pix.Length; ++i)
                pix[i] = color;

            Texture2D result = new(4, 4);
            result.SetPixels32(pix);
            result.Apply();
            return result;
        }
    }
}
