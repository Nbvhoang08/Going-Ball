namespace Hapiga.Core.Runtime.Process
{
    public class InterpolateProcess : TimedProcess
    {

        protected readonly float startValue;
        protected readonly float difference;

        public float CurrentValue
        {
            get
            {
                return startValue + difference * TimePortion;
            }
        }

        public InterpolateProcess(float duration, float startValue, float endValue) : base(duration)
        {
            this.startValue = startValue;
            difference = endValue - startValue;
        }

    }
}

