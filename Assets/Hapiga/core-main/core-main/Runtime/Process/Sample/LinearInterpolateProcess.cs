using UnityEngine;

namespace Hapiga.Core.Runtime.Process
{
	public class LinearInterpolateProcess : Process
	{
		public delegate void OnTimeUpdate(float _current);
		public event OnTimeUpdate TimeUpdateCallback;
		protected float start, end;
		public float Current { get; set; }
		protected float speed;
		protected float epsilon;

		public LinearInterpolateProcess(float start, float end, float speed, float _epsilon)
		{
			this.start = start;
			this.end = end;
			this.speed = speed;
			Current = start;
			epsilon = _epsilon;

		}

		public override void Update(float dt)
		{
			if (Mathf.Abs(Current - end) > epsilon)
			{
				Current += speed * dt;
				TimedUpdate();
			}
			else
			{
				Terminate();
			}
		}

		public override void OnBegin()
		{
		}

		public override void OnTerminate()
		{
			Current = end;
			TimedUpdate();
		}

		public virtual void TimedUpdate()
		{
			if (TimeUpdateCallback != null)
			{
				TimeUpdateCallback.Invoke(Current);
			}
		}

		public override void Pause(bool isPause)
		{		
		}
	}
}

