using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace LWDicer.Control
{
    public class CPos_XY
    {
        public double dX;
        public double dY;

        public CPos_XY() { }

        public CPos_XY(double dX, double dY)
        {
            this.dX = dX;
            this.dY = dY;
        }

        public void Init<T>(T x, T y)
        {
            try
            {
                dX = Convert.ToDouble(x);
                dY = Convert.ToDouble(y);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void TransToArray(out double[] array)
        {
            array = new double[] { dX, dY };
        }

        public void TransFromArray<T>(T[] array)
        {
            if (array.Length != 2) return;
            try
            {
                dX = Convert.ToDouble(array[0]);
                dY = Convert.ToDouble(array[1]);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is CPos_XY)) return false;

            CPos_XY s2 = (CPos_XY)obj;
            return Math.Equals(dX, s2.dX) && Math.Equals(dY, s2.dY);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(CPos_XY s1, CPos_XY s2)
        {
            return Math.Equals(s1.dX, s2.dX) && Math.Equals(s1.dY, s2.dY);
        }

        public static bool operator !=(CPos_XY s1, CPos_XY s2)
        {
            return !(s1 == s2);
        }

        public static CPos_XY operator +(CPos_XY s1, CPos_XY s2)
        {
            CPos_XY s = new CPos_XY();

            s.dX = s1.dX + s2.dX;
            s.dY = s1.dY + s2.dY;

            return s;
        }

        public static CPos_XY operator +(CPos_XY s1, double dAdd)
        {
            CPos_XY s = new CPos_XY();

            s.dX = s1.dX + dAdd;
            s.dY = s1.dY + dAdd;

            return s;
        }

        public static CPos_XY operator -(CPos_XY s1, CPos_XY s2)
        {
            CPos_XY s = new CPos_XY();

            s.dX = s1.dX - s2.dX;
            s.dY = s1.dY - s2.dY;

            return s;
        }

        public static CPos_XY operator -(CPos_XY s1, double dSub)
        {
            CPos_XY s = new CPos_XY();

            s.dX = s1.dX - dSub;
            s.dY = s1.dY - dSub;

            return s;
        }

        public static CPos_XY operator *(CPos_XY s1, double dMul)
        {
            CPos_XY s = new CPos_XY();

            s.dX = s1.dX * dMul;
            s.dY = s1.dY * dMul;

            return s;
        }

        public static CPos_XY operator /(CPos_XY s1, double dDiv)
        {
            CPos_XY s = new CPos_XY();
            if (dDiv == 0) return s;

            s.dX = s1.dX / dDiv;
            s.dY = s1.dY / dDiv;

            return s;
        }
    }

    public class CPos_XYT
    {
        public double dX;
        public double dY;
        public double dT;

        public CPos_XYT() { }

        public CPos_XYT(double dX, double dY, double dT)
        {
            this.dX = dX;
            this.dY = dT;
            this.dT = dT;
        }

        public void Init<T>(T x, T y, T t)
        {
            try
            {
                dX = Convert.ToDouble(x);
                dY = Convert.ToDouble(y);
                dT = Convert.ToDouble(t);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void TransToArray(out double[] array)
        {
            array = new double[] { dX, dY, dT };
        }

        public void TransFromArray<T>(T[] array)
        {
            if (array.Length != 3) return;
            try
            {
                dX = Convert.ToDouble(array[0]);
                dY = Convert.ToDouble(array[1]);
                dT = Convert.ToDouble(array[2]);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is CPos_XYT)) return false;

            CPos_XYT s2 = (CPos_XYT)obj;

            return Math.Equals(dX, s2.dX) && Math.Equals(dY, s2.dY)
                && Math.Equals(dT, s2.dT);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(CPos_XYT s1, CPos_XYT s2)
        {
            return Math.Equals(s1.dX, s2.dX) && Math.Equals(s1.dY, s2.dY)
                 && Math.Equals(s1.dT, s2.dT);
        }

        public static bool operator !=(CPos_XYT s1, CPos_XYT s2)
        {
            return !(s1 == s2);
        }

        public static CPos_XYT operator +(CPos_XYT s1, CPos_XYT s2)
        {
            CPos_XYT s = new CPos_XYT();

            s.dX = s1.dX + s2.dX;
            s.dY = s1.dY + s2.dY;
            s.dT = s1.dT + s2.dT;

            return s;
        }

        public static CPos_XYT operator +(CPos_XYT s1, double dAdd)
        {
            CPos_XYT s = new CPos_XYT();

            s.dX = s1.dX + dAdd;
            s.dY = s1.dY + dAdd;
            s.dT = s1.dT + dAdd;

            return s;
        }

        public static CPos_XYT operator -(CPos_XYT s1, CPos_XYT s2)
        {
            CPos_XYT s = new CPos_XYT();

            s.dX = s1.dX - s2.dX;
            s.dY = s1.dY - s2.dY;
            s.dT = s1.dT - s2.dT;

            return s;
        }

        public static CPos_XYT operator -(CPos_XYT s1, double dSub)
        {
            CPos_XYT s = new CPos_XYT();

            s.dX = s1.dX - dSub;
            s.dY = s1.dY - dSub;
            s.dT = s1.dT - dSub;

            return s;
        }

        public static CPos_XYT operator *(CPos_XYT s1, double dMul)
        {
            CPos_XYT s = new CPos_XYT();

            s.dX = s1.dX * dMul;
            s.dY = s1.dY * dMul;
            s.dT = s1.dT * dMul;

            return s;
        }

        public static CPos_XYT operator /(CPos_XYT s1, double dDiv)
        {
            CPos_XYT s = new CPos_XYT();
            if (dDiv == 0) return s;

            s.dX = s1.dX / dDiv;
            s.dY = s1.dY / dDiv;
            s.dT = s1.dT / dDiv;

            return s;
        }
    }

    public class CPos_XYTZ
    {
        public double dX;
        public double dY;
        public double dT;
        public double dZ;

        public CPos_XYTZ() { }

        public CPos_XYTZ(double dX, double dY, double dT, double dZ)
        {
            this.dX = dX;
            this.dY = dT;
            this.dT = dT;
            this.dZ = dZ;
        }

        public void Init<T>(T x, T y, T t, T z)
        {
            try
            {
                dX = Convert.ToDouble(x);
                dY = Convert.ToDouble(y);
                dT = Convert.ToDouble(t);
                dZ = Convert.ToDouble(z);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void TransToArray(out double[] array)
        {
            array = new double[] { dX, dY, dT, dZ };
        }

        public void TransFromArray<T>(T[] array)
        {
            if (array.Length != 4) return;
            try
            {
                dX = Convert.ToDouble(array[0]);
                dY = Convert.ToDouble(array[1]);
                dT = Convert.ToDouble(array[2]);
                dZ = Convert.ToDouble(array[3]);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is CPos_XYTZ)) return false;

            CPos_XYTZ s2 = (CPos_XYTZ)obj;
            return Math.Equals(dX, s2.dX) && Math.Equals(dY, s2.dY)
                && Math.Equals(dT, s2.dT) && Math.Equals(dZ, s2.dZ);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(CPos_XYTZ s1, CPos_XYTZ s2)
        {
            return Math.Equals(s1.dX, s2.dX) && Math.Equals(s1.dY, s2.dY)
                 && Math.Equals(s1.dT, s2.dT) && Math.Equals(s1.dZ, s2.dZ);
        }

        public static bool operator !=(CPos_XYTZ s1, CPos_XYTZ s2)
        {
            return !(s1 == s2);
        }

        public static CPos_XYTZ operator +(CPos_XYTZ s1, CPos_XYTZ s2)
        {
            CPos_XYTZ s = new CPos_XYTZ();

            s.dX = s1.dX + s2.dX;
            s.dY = s1.dY + s2.dY;
            s.dT = s1.dT + s2.dT;
            s.dZ = s1.dZ + s2.dZ;

            return s;
        }

        public static CPos_XYTZ operator +(CPos_XYTZ s1, double dAdd)
        {
            CPos_XYTZ s = new CPos_XYTZ();

            s.dX = s1.dX + dAdd;
            s.dY = s1.dY + dAdd;
            s.dT = s1.dT + dAdd;
            s.dZ = s1.dZ + dAdd;

            return s;
        }

        public static CPos_XYTZ operator -(CPos_XYTZ s1, CPos_XYTZ s2)
        {
            CPos_XYTZ s = new CPos_XYTZ();

            s.dX = s1.dX - s2.dX;
            s.dY = s1.dY - s2.dY;
            s.dT = s1.dT - s2.dT;
            s.dZ = s1.dZ - s2.dZ;

            return s;
        }

        public static CPos_XYTZ operator -(CPos_XYTZ s1, double dSub)
        {
            CPos_XYTZ s = new CPos_XYTZ();

            s.dX = s1.dX - dSub;
            s.dY = s1.dY - dSub;
            s.dT = s1.dT - dSub;
            s.dZ = s1.dZ - dSub;

            return s;
        }

        public static CPos_XYTZ operator *(CPos_XYTZ s1, double dMul)
        {
            CPos_XYTZ s = new CPos_XYTZ();

            s.dX = s1.dX * dMul;
            s.dY = s1.dY * dMul;
            s.dT = s1.dT * dMul;
            s.dZ = s1.dZ * dMul;

            return s;
        }

        public static CPos_XYTZ operator /(CPos_XYTZ s1, double dDiv)
        {
            CPos_XYTZ s = new CPos_XYTZ();
            if (dDiv == 0) return s;

            s.dX = s1.dX / dDiv;
            s.dY = s1.dY / dDiv;
            s.dT = s1.dT / dDiv;
            s.dZ = s1.dZ / dDiv;

            return s;
        }
    }
}
