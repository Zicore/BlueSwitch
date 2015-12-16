using System.Drawing;

namespace BlueSwitch.Base.Utils
{
    public class MathUtils
    {
        /// <summary>
        /// Rectangle Intersection from topLeft and bottomRight
        /// </summary>
        /// <param name="topLeft">TopLeft of rectangle</param>
        /// <param name="bottomRight">BottomRight of rectangle</param>
        /// <param name="value">Vector2 we check for intersection</param>
        /// <returns></returns>
        public static bool RectangleIntersects(PointF topLeft, PointF bottomRight, PointF value)
        {
            float width = bottomRight.X - topLeft.X;
            float height = bottomRight.Y - topLeft.Y;

            if (value.X > topLeft.X && value.X < topLeft.X + width)
            {
                if (value.Y > topLeft.Y && value.Y < topLeft.Y + height)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Rectangle Intersection from topLeft and bottomRight
        /// </summary>
        /// <param name="topLeft">TopLeft of rectangle</param>
        /// <param name="bottomRight">BottomRight of rectangle</param>
        /// <param name="value">Vector2 we check for intersection</param>
        /// <returns></returns>
        public static bool RectangleIntersects(RectangleF rect, PointF value)
        {
            float width = rect.Width;
            float height = rect.Height;

            if (value.X > rect.X && value.X < rect.X + width)
            {
                if (value.Y > rect.Y && value.Y < rect.Y + height)
                    return true;
            }

            return false;
        }


        /// <summary>
        /// returns the center of a rectangle
        /// </summary>
        /// <param name="topLeft">TopLeft of rectangle</param>
        /// <param name="bottomRight">BottomRight of rectangle</param>
        /// <returns>center of rectangle</returns>
        public static PointF GetRectangleCenter(PointF topLeft, PointF bottomRight)
        {
            float width = bottomRight.X - topLeft.X;
            float height = bottomRight.Y - topLeft.Y;

            return GetRectangleCenter(width, height);
        }


        /// <summary>
        /// returns the center of a rectangle
        /// </summary>
        /// <param name="topLeft">TopLeft of rectangle</param>
        /// <param name="bottomRight">BottomRight of rectangle</param>
        /// <returns>center of rectangle</returns>
        public static PointF GetRectangleCenter(float width, float height)
        {
            return new PointF(width / 2.0f, height / 2.0f); // rectangle mittelpunkt
        }

        /// <summary>
        /// returns the center of a rectangle
        /// </summary>
        /// <param name="topLeft">TopLeft of rectangle</param>
        /// <param name="bottomRight">BottomRight of rectangle</param>
        /// <returns>center of rectangle</returns>
        public static SizeF GetRectangleCenter(SizeF size)
        {
            return new SizeF(size.Width / 2.0f, size.Height / 2.0f); // rectangle mittelpunkt
        }
    }
}
