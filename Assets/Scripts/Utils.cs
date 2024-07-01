using UnityEngine;
using Random = UnityEngine.Random;

public static class Utils
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
}
