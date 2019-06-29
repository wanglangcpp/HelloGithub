using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    ///
    /// </summary>
    public static class ConverterEx
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="boolString"></param>
        /// <returns></returns>
        public static bool? ParseBool(string boolString)
        {
            if (string.IsNullOrEmpty(boolString) || boolString.ToLower() == "null")
            {
                return null;
            }

            return bool.Parse(boolString);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="intString"></param>
        /// <returns></returns>
        public static int? ParseInt(string intString)
        {
            if (string.IsNullOrEmpty(intString) || intString.ToLower() == "null")
            {
                return null;
            }

            return int.Parse(intString);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="intArrayString"></param>
        /// <returns></returns>
        public static int[] ParseIntArray(string intArrayString)
        {
            if (string.IsNullOrEmpty(intArrayString) || intArrayString.ToLower() == "null")
            {
                return new int[0];
            }

            string[] splitString = intArrayString.Split(',');
            int[] intArray = new int[splitString.Length];
            for (int i = 0; i < splitString.Length; i++)
            {
                intArray[i] = int.Parse(splitString[i]);
            }

            return intArray;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="floatString"></param>
        /// <returns></returns>
        public static float? ParseFloat(string floatString)
        {
            if (string.IsNullOrEmpty(floatString) || floatString.ToLower() == "null")
            {
                return null;
            }

            return float.Parse(floatString);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="floatArrayString"></param>
        /// <returns></returns>
        public static float[] ParseFloatArray(string floatArrayString)
        {
            if (string.IsNullOrEmpty(floatArrayString) || floatArrayString.ToLower() == "null")
            {
                return new float[0];
            }

            string[] splitString = floatArrayString.Split(',');
            float[] floatArray = new float[splitString.Length];
            for (int i = 0; i < splitString.Length; i++)
            {
                floatArray[i] = float.Parse(splitString[i]);
            }

            return floatArray;
        }

        /// <summary>
        /// 将输入字符串解析为字符串数组，以逗号作为分隔符。
        /// </summary>
        /// <param name="stringArrayString">输入字符串。</param>
        /// <returns></returns>
        public static string[] ParseStringArray(string stringArrayString)
        {
            if (string.IsNullOrEmpty(stringArrayString) || stringArrayString.ToLower() == "null")
            {
                return new string[0];
            }

            return stringArrayString.Split(',');
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="vector2String"></param>
        /// <returns></returns>
        public static Vector2? ParseVector2(string vector2String)
        {
            if (string.IsNullOrEmpty(vector2String) || vector2String.ToLower() == "null")
            {
                return null;
            }

            float[] floatArray = ParseFloatArray(vector2String);
            if (floatArray.Length < 2)
            {
                return null;
            }

            return new Vector2(floatArray[0], floatArray[1]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="vector3String"></param>
        /// <returns></returns>
        public static Vector3? ParseVector3(string vector3String)
        {
            if (string.IsNullOrEmpty(vector3String) || vector3String.ToLower() == "null")
            {
                return null;
            }

            float[] floats = ParseFloatArray(vector3String);
            if (floats.Length < 3)
            {
                return null;
            }

            return new Vector3(floats[0], floats[1], floats[2]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumString"></param>
        /// <returns></returns>
        public static T? ParseEnum<T>(string enumString) where T : struct
        {
            if (string.IsNullOrEmpty(enumString) || enumString.ToLower() == "null")
            {
                return null;
            }

            return (T)Enum.Parse(typeof(T), enumString);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string GetString<T>(T o)
        {
            if (o == null)
            {
                return "null";
            }
            return o.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string GetStringFromVector2(Vector2? v)
        {
            if (v == null)
            {
                return "null";
            }
            return string.Format("{0},{1}", v.Value.x, v.Value.y);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string GetStringFromVector3(Vector3? v)
        {
            if (v == null)
            {
                return "null";
            }
            return string.Format("{0},{1},{2}", v.Value.x, v.Value.y, v.Value.z);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string GetStringFromArray<T>(T[] array)
        {
            if (array == null)
            {
                return string.Empty;
            }

            string[] elems = new string[array.Length];
            for (int i = 0; i < array.Length; ++i)
            {
                elems[i] = array[i].ToString();
            }

            return string.Join(",", elems);
        }
    }
}
