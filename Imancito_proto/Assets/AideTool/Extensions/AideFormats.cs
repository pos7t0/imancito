using UnityEngine;
using System;

namespace AideTool.Extensions
{
    public static class AideFormats
    {
        public static string FormatTimeSpan(this TimeSpan span)
        {
            return $"{span.Minutes} : {span.Seconds.ToString().PadLeft(2, '0')}";
        }

        public static int TimeSpanSeconds(this TimeSpan span)
        {
            return span.Seconds + (span.Minutes * 60) + (span.Hours * 3600);
        }

        public static Sprite TextureToSprite(this Texture2D texture, Vector2 rectDimensions)
        {
            return Sprite.Create(texture, new Rect(Vector2.zero, rectDimensions), Vector2.one * 0.5f);
        }

        public static float[] VectorToFloatArray(this Vector3 vector)
        {
            return new float[] { vector.x, vector.y, vector.z };
        }

        public static Vector3 ToXZVector3(this Vector2 vector)
        {
            return new(vector.x, 0f, vector.y);
        }

        public static Vector3 ToXZVector3(this Vector3 vector)
        {
            return new(vector.x, 0f, vector.z);
        }
    }
}
