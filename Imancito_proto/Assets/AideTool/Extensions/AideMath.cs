using System;

namespace AideTool.Extensions
{
    public static class AideMath
    {
        /// <summary>
        /// Returns true or false if a number is between two other numbers
        /// </summary>
        /// <param name="value">Number to be inspected</param>
        /// <param name="min">Lowest number in range of inspection</param>
        /// <param name="max">Highest number in range of inspection</param>
        /// <returns></returns>
        public static bool Between(this float value, float min, float max)
        {
            if(value >= min && value <= max) 
                return true;
            return false;
        }

        /// <summary>
        /// Returns true or false if a number is between two other numbers
        /// </summary>
        /// <param name="value">Number to be inspected</param>
        /// <param name="min">Lowest number in range of inspection</param>
        /// <param name="max">Highest number in range of inspection</param>
        /// <returns></returns>
        public static bool Between(this int value, int min, int max)
        {
            if(value >= min && value <= max) 
                return true;
            return false;
        }

        /// <summary>
        /// Checks if all conditions are true
        /// </summary>
        /// <param name="args">Conditions to check</param>
        /// <returns></returns>
        public static bool AndCheck(params bool[] args)
        {
            foreach (bool arg in args)
                if (!arg)
                    return false;
            return true;
        }

        /// <summary>
        /// Checks if one of the conditions is true
        /// </summary>
        /// <param name="args">Conditions to check</param>
        /// <returns></returns>
        public static bool OrCheck(params bool[] args)
        {
            foreach (bool arg in args)
                if (arg)
                    return true;
            return false;
        }

        /// <summary>
        /// Checks if the difference between two numbers is near zero
        /// </summary>
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// <param name="offset">How near the difference between numbers can be for the result to be true</param>
        /// <returns></returns>
        public static bool MoreOrLess(float a, float b, float offset)
        {
            float result = Math.Abs(a - b);
            if (result <= offset)
                return true;
            return false;
        }

        /// <summary>
        /// Returns a number between two numbers
        /// </summary>
        /// <param name="value">Value to return</param>
        /// <param name="min">Lowest number to return</param>
        /// <param name="max">Highest number to return</param>
        /// <returns></returns>
        public static float ClampBetween(this float value, float min, float max)
        {
            if (value < min)
                return min;
            if (value > max) 
                return max;
            return value;
        }

        /// <summary>
        /// Returns a number between 0f and 1f
        /// </summary>
        /// <param name="value">Value to return</param>
        /// <returns></returns>
        public static float ClampBetweenOne(this float value)
        {
            return value.ClampBetween(0f, 1f);
        }

        /// <summary>
        /// Returns a value between a and b using a linear function depending on t
        /// </summary>
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// <param name="t">Horizontal dimension on the linear function. Must be between 0f and 1f</param>
        /// <returns></returns>
        public static float LinearLerp(float a, float b, float t)
        {
            t = t.ClampBetweenOne();
            return a + (b - a) * t;
        }

        public static float WeightedLinearLerp(float a, float weightA, float b, float weightB, float t)
        {
            t = t.ClampBetweenOne();
            weightA = weightA.ClampBetween(0f, 0.45f);
            weightB = weightB.ClampBetween(0f, 0.45f);

            if (t > 1f - weightB)
                return b;

            if (t < weightA)
                return a;

            float padding = weightA + weightB;
            float m = (b - a) / (1f - padding);
            float n = -1f * m * weightA;
            float result = m * t + n;

            return result;
        }

        public static float QuadraticLerp(float a, float b, float t)
        {
            t = t.ClampBetweenOne();
            float aX = -1f * (b - a);
            float bX = 2f * (b - a);
            float cX = a;
            float result = aX * MathF.Pow(t, 2f) + bX * t + cX;

            return result;
        }
    }
}
