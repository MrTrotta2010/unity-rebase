// Copyright © 2023 Tiago Trotta

// This file is part of Unity ReBase.

// Unity ReBase is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// Unity ReBase is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with Unity ReBase.  If not, see <https://www.gnu.org/licenses/>

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
