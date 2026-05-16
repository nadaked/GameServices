using System.Globalization;
using UnityEngine;

namespace GameServices.Runtime.Save
{
    internal static class SaveValueSerializer
    {
        private const char Separator = '|';

        public static string ToString(Vector2 value)
        {
            return Join(value.x, value.y);
        }

        public static string ToString(Vector3 value)
        {
            return Join(value.x, value.y, value.z);
        }

        public static string ToString(Quaternion value)
        {
            return Join(value.x, value.y, value.z, value.w);
        }

        public static string ToString(Color value)
        {
            return Join(value.r, value.g, value.b, value.a);
        }

        public static bool TryParseVector2(string value, out Vector2 result)
        {
            if (TryParseFloats(value, 2, out var values))
            {
                result = new Vector2(values[0], values[1]);
                return true;
            }

            result = default;
            return false;
        }

        public static bool TryParseVector3(string value, out Vector3 result)
        {
            if (TryParseFloats(value, 3, out var values))
            {
                result = new Vector3(values[0], values[1], values[2]);
                return true;
            }

            result = default;
            return false;
        }

        public static bool TryParseQuaternion(string value, out Quaternion result)
        {
            if (TryParseFloats(value, 4, out var values))
            {
                result = new Quaternion(values[0], values[1], values[2], values[3]);
                return true;
            }

            result = default;
            return false;
        }

        public static bool TryParseColor(string value, out Color result)
        {
            if (TryParseFloats(value, 4, out var values))
            {
                result = new Color(values[0], values[1], values[2], values[3]);
                return true;
            }

            result = default;
            return false;
        }

        public static string ToJson<T>(T value)
        {
            return JsonUtility.ToJson(value);
        }

        public static bool TryParseJson<T>(string value, out T result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = default;
                return false;
            }

            try
            {
                result = JsonUtility.FromJson<T>(value);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

        private static string Join(params float[] values)
        {
            var result = values[0].ToString(CultureInfo.InvariantCulture);

            for (var i = 1; i < values.Length; i++)
            {
                result = $"{result}{Separator}{values[i].ToString(CultureInfo.InvariantCulture)}";
            }

            return result;
        }

        private static bool TryParseFloats(string value, int count, out float[] values)
        {
            values = null;

            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            var parts = value.Split(Separator);
            if (parts.Length != count)
            {
                return false;
            }

            values = new float[count];
            for (var i = 0; i < count; i++)
            {
                if (!float.TryParse(parts[i], NumberStyles.Float, CultureInfo.InvariantCulture, out values[i]))
                {
                    values = null;
                    return false;
                }
            }

            return true;
        }
    }
}
