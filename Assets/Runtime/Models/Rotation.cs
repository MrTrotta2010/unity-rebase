using UnityEngine;

namespace ReBase
{
    public class Rotation
    {
        public double x;
        public double y;
        public double z;

        public Rotation(float[] rotations)
        {
            if (rotations.Length > 0)
            {
                x = rotations[0];
                if (rotations.Length > 1)
                {
                    y = rotations[1];
                    if (rotations.Length > 2) z = rotations[2];
                }
            }
        }

        public Rotation(double[] rotations)
        {
            if (rotations.Length > 0)
            {
                x = rotations[0];
                if (rotations.Length > 1)
                {
                    y = rotations[1];
                    if (rotations.Length > 2) z = rotations[2];
                }
            }
        }

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

        public double[] ToArray() {
            return new double[3] { x, y, z };
        }
    }
}
