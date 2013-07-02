using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silhouette.Engine
{
    public class Interpolator
    {
        public enum InterpolatorState
        {
            Started,
            Stopped
        }

        public enum InterpolatorType
        {
            Linear
        }

        public InterpolatorState State { get; set; }
        public InterpolatorType Type { get; set; }
        public float Range { get; set; }
        public float Progress { get { return (float) CurrentTime / Duration; } }
        public int Duration { get; set; }
        public int CurrentTime { get; set; }
        public float StartValue { get; set; }
        public float CurrentValue { get; set; }
        public float EndValue { get; set; }
        private float StepSize { get; set; }

        public Interpolator(float start, float end, int duration)
        {
            StartValue = start;
            CurrentValue = start;
            EndValue = end;
            CurrentTime = 0;
            Duration = duration;
            Range = EndValue - StartValue;
            StepSize = (float) Range / Duration;
            State = InterpolatorState.Started;
            Type = InterpolatorType.Linear;
        }

        public void Update(int dt)
        {
            if (State == InterpolatorState.Stopped)
            {
                return;
            }
            if (Progress >= 1)
            {
                State = InterpolatorState.Stopped;
                return;
            }


            if (Type == InterpolatorType.Linear) 
            {
                CurrentTime += dt;
                CurrentValue += (StepSize * dt);
            }
        }
    }
}
