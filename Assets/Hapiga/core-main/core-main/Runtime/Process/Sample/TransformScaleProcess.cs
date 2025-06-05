using System;
using UnityEngine;

namespace Hapiga.Core.Runtime.Process
{
	public class TransformScaleProcess : Vector3InterpolateProcess
	{
		protected Transform transform;

		public TransformScaleProcess()
		{
		}

		public void SetInfo(Transform _transform, Vector3 _fromScale, Vector3 _toScale, float _duration, float _delay = 0,
			AnimationCurve _easeCurve = null, Action _onCompletedAction = null)
		{
			base.SetInfo(_fromScale, _toScale, _duration, _delay, _easeCurve, _onCompletedAction);
			transform = _transform;
		}

		public override void TimeUpdated()
		{
			if (transform == null)
			{
				Cancel();
				return;
			}
			base.TimeUpdated();
			transform.localScale = curVector3;
		}
	}
}
