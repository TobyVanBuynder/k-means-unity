using UnityEngine;
using Random = UnityEngine.Random;

namespace Utils
{
    public static class Math
    {
        public static Vector3 RandomPositionInCircle(Vector3 center, float radius)
        {
            float randomAngle = RandomAngleRad();
            float randomMagnitude = Random.Range(0, radius);

            float xPos = Mathf.Cos(randomAngle) * randomMagnitude;
            float zPos = Mathf.Sin(randomAngle) * randomMagnitude;

            return new Vector3(center.x + xPos, center.y, center.y + zPos);
        }

        public static Quaternion RandomScaledQuaternion(Vector3 scale)
        {
            Vector3 randomAngles = RandomEulerAngles();
            randomAngles.Scale(scale);
            return Quaternion.Euler(randomAngles);
        }

        public static float RandomAngleDeg()
        {
            return Random.Range(0, 360f);
        }

        public static float RandomAngleRad()
        {
            return RandomAngleDeg() * Mathf.Rad2Deg;
        }

        public static Vector3 RandomEulerAngles()
        {
            return new Vector3(RandomAngleDeg(), RandomAngleDeg(), RandomAngleDeg());
        }

        public static float ConvertTicksToMilliseconds(int ticks)
        {
            return ticks * 0.0001f;
        }

        public static float ConvertTicksToMilliseconds(long ticks)
        {
            return ticks * 0.0001f;
        }
    }
    
    // Colors
    public static class Colors
    {
        public static readonly Color purple = new Color(145/255f, 30/255f, 180/255f, 1);
        public static readonly Color orange = new Color(245/255f, 130/255f, 48/255f, 1);
        public static readonly Color lime = new Color(210/255f, 245/255f, 60/255f, 1);
        public static readonly Color pink = new Color(250/255f, 190/255f, 190/255f, 1);
        public static readonly Color brown = new Color(170/255f, 110/255f, 40/255f, 1);
        public static readonly Color olive = new Color(128/255f, 128/255f, 0, 1);
    }
}