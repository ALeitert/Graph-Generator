using System;
using System.Drawing;

namespace GraphGenerator
{
    public struct Vector
    {
        public Vector(PointF p)
        {
            this.X = p.X;
            this.Y = p.Y;
        }

        public Vector(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }

        public double Length
        {
            get { return Math.Sqrt(X * X + Y * Y); }
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector operator /(Vector v, double m)
        {
            return new Vector(v.X / m, v.Y / m);
        }

        public static Vector operator *(Vector v, double m)
        {
            return new Vector(v.X * m, v.Y * m);
        }

        public static Vector operator *(double m, Vector v)
        {
            return new Vector(v.X * m, v.Y * m);
        }

        public static bool operator ==(Vector v1, Vector v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y;
        }

        public static bool operator !=(Vector v1, Vector v2)
        {
            return v1.X != v2.X || v1.Y != v2.Y;
        }

        public Vector Normalize()
        {
            if (Length == 0) return this;

            return this / Length;
        }

        public PointF ToPointF()
        {
            return new PointF(Convert.ToSingle(X), Convert.ToSingle(Y));
        }

    }
}
