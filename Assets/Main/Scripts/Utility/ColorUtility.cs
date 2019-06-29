using System.Text;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 颜色值相关工具类。
    /// </summary>
    public static class ColorUtility
    {
        /// <summary>
        /// 根据物品质量级别获取相应颜色值。
        /// </summary>
        /// <param name="quality">质量级别。</param>
        /// <returns>颜色值。</returns>
        public static Color GetColorForQuality(int quality)
        {
            return Constant.Quality.Colors[quality];
        }

        /// <summary>
        /// 获取由两个颜色各自较大分量组成的新颜色。
        /// </summary>
        /// <param name="x">第一个颜色。</param>
        /// <param name="y">第二个颜色。</param>
        /// <returns></returns>
        public static Color GetMaxColorComponents(Color x, Color y)
        {
            float r = Mathf.Max(x.r, y.r);
            float g = Mathf.Max(x.g, y.g);
            float b = Mathf.Max(x.b, y.b);
            float a = Mathf.Max(x.a, y.a);
            return new Color(r, g, b, a);
        }

        /// <summary>
        /// 把Color格式的颜色改成16进制格式颜色值。
        /// </summary>
        /// <param name="x">Color格式的颜色值。</param>
        /// <returns>16进制格式颜色值</returns>
        public static string ColorToHexColor(Color32 color)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(((int)(color.r)).ToString("X2"));
            sb.Append(((int)(color.g)).ToString("X2"));
            sb.Append(((int)(color.b)).ToString("X2"));
            sb.Append(((int)(color.a)).ToString("X2"));

            return sb.ToString();
        }

        public static string AddColorToString(Color color, string str)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[");
            sb.Append(ColorToHexColor(color));
            sb.Append("]");
            sb.Append(str);
            sb.Append("[-]");

            return sb.ToString();
        }

        public static string AddStringColorToString(string color, string str)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[");
            sb.Append(color);
            sb.Append("]");
            sb.Append(str);
            sb.Append("[-]");

            return sb.ToString();
        }
    }
}
