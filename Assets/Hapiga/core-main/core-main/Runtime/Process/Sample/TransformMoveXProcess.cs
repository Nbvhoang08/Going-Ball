using UnityEngine;

namespace Hapiga.Core.Runtime.Process
{
    public class TransformMoveXProcess : LinearInterpolateProcess
    {
        protected Transform transform;
        private Vector3 curPos;
        public TransformMoveXProcess(Transform transform, float start, float end, float speed) : base(start, end, speed, 0.2f)
        {
            this.transform = transform;
            curPos = transform.position;
        }

        public override void TimedUpdate()
        {
            curPos.x = Current;
            transform.position = curPos;
        }

        public void SetInfo(float start, float end)
        {
            Reset();
            this.start = start;
            this.end = end;
            Current = start;
            if (start > end)
            {
                speed = -Mathf.Abs(speed);
            }
            else
            {
                speed = Mathf.Abs(speed);
            }
        }
        public void Reset()
        {
            start = transform.position.x;
            Current = start;
        }
    }
}

