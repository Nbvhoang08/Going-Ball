using System;
using Hapiga.Core.Runtime.Pool;
using Hapiga.Core.Runtime.Utils;
using UnityEngine;

namespace Hapiga.Core.Runtime.Process
{
	public class Vector3InterpolateProcess : Process, IPoolable
	{
		protected Vector3 fromVector3;
		protected Vector3 toVector3;
		protected float t { get; private set; }
		protected float duration;
		protected float delay;
		protected bool isDelaying;
		protected Action onCompletedAction;
		protected AnimationCurve easeCurve;
		protected Vector3 curVector3 = Vector3.zero;
		public Vector3InterpolateProcess()
		{
		}

		public virtual void SetInfo(Vector3 _fromVector3, Vector3 _toVector3, float _duration, float _delay = 0,
			AnimationCurve _easeCurve = null, Action _onCompletedAction = null)
		{
			fromVector3 = _fromVector3;
			toVector3 = _toVector3;
			duration = _duration;
			delay = _delay;
			easeCurve = _easeCurve;
			isDelaying = delay > 0;
			onCompletedAction = _onCompletedAction;
			Attached.Clear();
		}

		public override void OnBegin()
		{
			t = 0f;
		}

		public override void OnTerminate()
		{
			t = duration;
			TimeUpdated();
			onCompletedAction?.Invoke();
		}

		public override void Update(float dt)
		{
			t += dt;
			if (isDelaying)
			{
				if (t >= delay)
				{
					t = 0;
					isDelaying = false;
				}
				return;
			}

			if (t >= duration)
			{
				Terminate();
				return;
			}
			TimeUpdated();
		}

		public virtual void TimeUpdated()
		{
			float dt;
			dt = easeCurve == null ? t / duration : easeCurve.Evaluate(GameHelper.Remap(t, 0, duration, 0, 1));
			curVector3 = Vector3.LerpUnclamped(fromVector3, toVector3, dt);
		}

		public void Reset()
		{
			t = 0;
		}

		public override void Pause(bool isPause)
		{
		}
	}
}
