namespace Hapiga.Core.Runtime.Process
{
	public class TimedProcess : Process {

		protected float t { get; private set; }
		protected readonly float duration;

		protected float TimePortion {
			get {
				return t / duration;
			}
		}

		public TimedProcess(float duration) {
			this.duration = duration;
			t = 0f;
		}

		public override void OnBegin() {
			
		}

		public override void OnTerminate() {
			t = duration;
			TimeUpdated();
		}

		public override void Update(float dt) {
			t += dt;
			if (t >= duration) {
				Terminate();
				return;
			}
			TimeUpdated();
		}

		public virtual void TimeUpdated() {

		}

		public override void Pause(bool isPause)
		{		
		}
	}
}

