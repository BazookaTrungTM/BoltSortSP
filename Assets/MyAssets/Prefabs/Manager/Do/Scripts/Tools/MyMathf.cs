using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Do.Scripts.Tools
{
    public static class MyMathf
    {
        public static int Random(int min, int max)
        {
            if (max <= min)
                return 0;
            var result = new List<int>();
            for (var i = min; i < max; i++)
            {
                result.Add(i);
            }
            var count = max - min;
            var randoms = new List<int>();
            for (var i = count - 1; i >= 0; i--)
            {
                var pick = result[UnityEngine.Random.Range(0, i + 1)];
                randoms.Add(pick);
                result.Remove(pick);
            }
            return randoms[UnityEngine.Random.Range(0, count)];
        }
        public static int Random(List<int> percents)
        {
            if (percents.Count <= 0)
                return -1;
            var total = percents.Sum();
            var r = UnityEngine.Random.Range(0, total);
            for (var i = 0; i < percents.Count; i++)
            {
                if (r < percents[i])
                    return i;
                r -= percents[i];
            }
            return -1;
        }
        public static float RoundToFloat(float f, int digits)
        {
            return (float) Math.Round(f, digits);
        }
        public static Vector2 RotateVector2(Vector2 v, float degrees)
        {
            return Quaternion.Euler(0, 0, degrees) * v;
        }

        public static Quaternion LookDirection(Vector3 dir, Vector3 rota)
        {
            var quaternion = Quaternion.LookRotation(dir, rota);
            quaternion.x = 0f;
            quaternion.y = 0f;
            return quaternion;
        }
    }
}
