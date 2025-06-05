using UnityEngine;
using UnityEngine.UI;

namespace Hapiga.Core.Runtime.Process
{
    public class FadeOutProcess : InterpolateProcess
    {

        private Graphic graphic;

        public FadeOutProcess(float duration, Graphic graphic) : base(duration, 0f, 1f)
        {
            this.graphic = graphic;
        }

        public FadeOutProcess(float duration, Graphic graphic, bool fromCurrentAlpha) : base(duration, (fromCurrentAlpha) ? graphic.color.a : 0f, 1f)
        {
            this.graphic = graphic;
        }

        public FadeOutProcess(float duration, Graphic graphic, float endAlpha) : base(duration, 0f, endAlpha)
        {
            this.graphic = graphic;
        }

        public FadeOutProcess(float duration, Graphic graphic, float startAlpha, float endAlpha) : base(duration, startAlpha, endAlpha)
        {
            this.graphic = graphic;
        }

        public FadeOutProcess(float duration, Graphic graphic, float endAlpha, bool fromCurrentAlpha) : base(duration, (fromCurrentAlpha) ? graphic.color.a : 0f, 1f)
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

