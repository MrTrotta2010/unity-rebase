namespace ReBase
{
    public class Articulation
    {
        public const int HipCenter = 1;
        public const int Spine = 2;
        public const int ShoulderCenter = 3;
        public const int Head = 4;
        public const int LeftShoulder = 5;
        public const int LeftElbow = 6;
        public const int LeftWrist = 7;
        public const int LeftHand = 8;
        public const int RightShoulder = 9;
        public const int RightElbow = 10;
        public const int RightWrist = 11;
        public const int RightHand = 12;
        public const int LeftHip = 13;
        public const int LeftKnee = 14;
        public const int LeftAnkle = 15;
        public const int LeftFoot = 16;
        public const int RightHip = 17;
        public const int RightKnee = 18;
        public const int RightAnkle = 19;
        public const int RightFoot = 20;

        public static bool CompareArticulationLists(int[] listA, int[] listB)
        {
            if (listA.Length != listB.Length) return false;
            for (int i = 0; i < listA.Length; i++)
            {
                if (listA[i] != listB[i]) return false;
            }
            return true;
        }
    }
}