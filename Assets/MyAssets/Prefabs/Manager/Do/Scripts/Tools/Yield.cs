using System.Collections.Generic;
using UnityEngine;

namespace Do.Scripts.Tools
{
    public static class Yield
    {
        private static readonly Dictionary<float, WaitForSeconds> _timeInterval = new Dictionary<float, WaitForSeconds>();
        private static readonly Dictionary<float, WaitForSecondsRealtime> _timeReal = new Dictionary<float, WaitForSecondsRealtime>();

        public static WaitForEndOfFrame EndFrame { get; } = new WaitForEndOfFrame();

        public static WaitForSeconds Wait05 { get; } = new WaitForSeconds(0.5f);

        public static WaitForSeconds Wait1 { get; } = new WaitForSeconds(1);

        public static WaitForSeconds GetTime(float time)
        {
            if (!_timeInterval.ContainsKey(time))
                _timeInterval.Add(time, new WaitForSeconds(time));
            return _timeInterval[time];
        }

        public static WaitForSecondsRealtime GetRealtime(float time)
        {
            if (!_timeReal.ContainsKey(time))
                _timeReal.Add(time, new WaitForSecondsRealtime(time));
            return _timeReal[time];
        }
    }
}
