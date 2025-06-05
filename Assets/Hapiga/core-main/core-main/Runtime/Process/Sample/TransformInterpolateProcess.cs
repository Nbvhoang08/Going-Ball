using System;
using Hapiga.Core.Runtime.Pool;
using UnityEngine;

namespace Hapiga.Core.Runtime.Process
{
	public class TransformInterpolateProcess : Vector3InterpolateProcess, IPoolable
	{
		protected Transform transform;
		private bool isWorldInterpolate;

		public TransformInterpolateProcess()
		{
		}	

		public void SetInfo(Transform _transform, Vector3 _fromPos, Vector3 _toPos, float _duration, bool _isWorldInterpolate = true, float _delay = 0,
			AnimationCurve _easeCurve = null,Action _onCompletedAction = null)
		{
			base.SetInfo(_fromPos, _toPos, _duration, _delay, _easeCurve, _onCompletedAction);
			transform = _transform;
			isWorldInterpolate = _isWorldInterpolate;
		}

		public override void TimeUpdated()
		{
			base.TimeUpdated();
			if(isWorldInterpolate)
			{
				transform.position = curVector3;
			}
			else
			{
				transform.localPosition = curVector3;
			}
		}
	}
}
