using UnityEngine;
using UnityEngine.UI;

namespace Hapiga.Core.Runtime.Process
{
    public class FadeInProcess : InterpolateProcess
    {

        private Graphic graphic;

        public FadeInProcess(float duration, Graphic graphic) : base(duration, 1f, 0f)
        {
            this.graphic = graphic;
        }

        public FadeInProcess(float duration, Graphic graphic, bool fromCurrentAlpha) : base(duration, (fromCurrentAlpha) ? graphic.color.a : 1f, 0f)
        {
            this.graphic = graphic;
        }

        public FadeInProcess(float duration, Graphic graphic, float endAlpha) : base(duration, 1f, endAlpha)
        {
            this.graphic = graphic;
        }

        public FadeInProcess(float duration, Graphic graphic, float startAlpha, float endAlpha) : base(duration, startAlpha, endAlpha)
        {
            this.graphic = graphic;
        }

        public FadeInProcess(float duration, Graphic graphic, float endAlpha, bool fromCurrentAlpha) : base(duration, (fromCurrentAlpha) ? graphic.color.a : 1f, 0f)
        {
            this.graphic = graphic;
        }

        public override void TimeUpdated()
        {
            Color color = graphic.color;
            color.a = CurrentValue;
            graphic.color = color;
        }

    }
}

