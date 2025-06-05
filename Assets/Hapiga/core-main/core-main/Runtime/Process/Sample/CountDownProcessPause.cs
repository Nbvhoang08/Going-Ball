using UnityEngine;

namespace Hapiga.Core.Runtime.Process
{
    public class CountDownProcessPause : Process {

        public delegate void OnTimeUpdate(int _current);
        public event OnTimeUpdate TimeUpdateCallback;

        protected int startNumber;
        protected int endNumber;
        private float timer = 0f;
        private float pausedDuration = 0;
        private float startPauseTime = 0;
        private bool isPause = false;
        public int Current { get; set; }

        public CountDownProcessPause()
        {
            this.startNumber = 0;
            this.endNumber = 0;
        }
        public CountDownProcessPause(int _startNumber, int _endNumber)
        {
            this.startNumber = _startNumber;
            this.endNumber = _endNumber;
        }

        public void SetInfo(int _startNumber, int _endNumber)
        {
            this.startNumber = _startNumber;
            this.endNumber = _endNumber;
        }
        public override void Update(float dt)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer && isPause)
            {
                return;
            }
        
            timer += dt;
            if (timer >= 1)
            {
                Current--;
                TimedUpdate();

                if (Current <= endNumber)
                {
                    Terminate();
                    return;
                }
                timer = 0;
            }
        }

        public override void OnBegin()
        {
            timer = 0;
            pausedDuration = 0;
            startPauseTime = 0;
            Current = startNumber;
            if (TimeUpdateCallback != null)
            {
                TimeUpdateCallback.Invoke(Current);
            }
        }

        public override void OnTerminate()
        {
            Current = endNumber;
            //TimedUpdate();
        }

        public virtual void TimedUpdate()
        {
            if(TimeUpdateCallback != null)
            {
                TimeUpdateCallback.Invoke(Current);
            }
        }

        public override void Pause(bool isPause)
        {
            this.isPause = isPause;
        }
    }
}
