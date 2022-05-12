using System;

namespace DanielLochner.Assets
{
    public class TimerUtility
    {
        public static void OnTimer(ref float timeLeft, float lapTime, float timeStep, Action onTimerEnd)
        {
            if (timeLeft < 0)
            {
                onTimerEnd?.Invoke();
                timeLeft = lapTime;
            }
            timeLeft -= timeStep;
        }
    }
}