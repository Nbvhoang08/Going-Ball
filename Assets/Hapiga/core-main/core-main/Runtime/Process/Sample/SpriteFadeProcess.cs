using System;
using Hapiga.Core.Runtime.Pool;
using UnityEngine;

namespace Hapiga.Core.Runtime.Process
{
	public class SpriteFadeProcess : Process, IPoolable
	{
		protected SpriteRenderer spriteRenderer;
		protected float fromAlpha;
		protected float toAlpha;
		protected float t { get; private set; }
		protected float duration;
		protected float delay;
		protected bool isDelaying;
		protected Action onCompletedAction;
		//protected AnimationCurve easeCurve;
		private Color curColor = new Color(1, 1, 1, 1);

		public SpriteFadeProcess()
		{
		}

		public void SetInfo(SpriteRenderer _spriteRenderer, float _toAlpha, float _duration, float _delay = 0, Action _onCompletedAction = null)
		{
			spriteRenderer = _spriteRenderer;
			fromAlpha = spriteRenderer.color.a;
			toAlpha = _toAlpha;
			duration = _duration;
			delay = _delay;
			//easeCurve = _easeCurve;
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
			if(spriteRenderer ==null)
			{
				Cancel();
				return;
			}
			curColor.a = Mathf.Lerp(fromAlpha, toAlpha, t / duration);
			spriteRenderer.color = curColor;
		}

		public override void Pause(bool isPause)
		{
		}

		public void Reset()
		{
			t = 0;
		}
	}
}
