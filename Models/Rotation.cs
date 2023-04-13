using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReBase
{
    public class Rotation : MonoBehaviour
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
    }
}
