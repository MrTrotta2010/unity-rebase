using UnityEngine;

namespace ReBase
{
    public class Rotation
    {
        public double x;
        public double y;
        public double z;

        public Rotation(double x, double y, double z)
		{
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Rotation(double x, double y)
		{
            this.x = x;
            this.y = y;
        }

        public Vector3 ToVector3()
		{
            return new Vector3((float)x, (float)y, (float)z);
		}
    }
}
