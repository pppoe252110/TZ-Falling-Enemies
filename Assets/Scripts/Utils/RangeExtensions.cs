using UnityEngine;

namespace Game.Utils
{
    public static class RangeExtensions
    {
        public static float GetRandomValue(this Range<float> self)
        {
            var result = Random.Range(self.Min, self.Max);
            return result;
        }

        public static int GetRandomValue(this Range<int> self, bool isInclusive = false)
        {
            var result = Random.Range(self.Min, self.Max + (isInclusive ? 1 : 0));
            return result;
        }
    }
}