using System.Collections.Generic;
using UnityEngine;

namespace Hapiga.Core.Runtime.Process
{
    public abstract class Process
    {

        public delegate void OnTerminateCallback();
        public event OnTerminateCallback TerminateCallback;

        public ProcessState State { get; private set; }

        public List<Process> Attached { get; private set; }

        public Process()
        {
            State = ProcessState.CREATED;
            Attached = new List<Process>(5);
        }

        public void Start()
        {
            State = ProcessState.STARTED;
            OnBegin();
        }

        public void Terminate()
        {
            State = ProcessState.TERMINATED;
        }

        public void InvokeTerminateCallback()
        {
            if (TerminateCallback != null)
            {
                //Debug.Log("count down terminate");
                TerminateCallback.Invoke();
            }
        }

        public void Cancel()
        {
            Debug.Log("count down Cancel");
            State = ProcessState.CANCELLED;
        }

        public void Attach(Process process)
        {
            Attached.Add(process);
        }

        public abstract void Update(float dt);
        public abstract void Pause(bool isPause);
        public abstract void OnBegin();
        public abstract void OnTerminate();

    }
}
