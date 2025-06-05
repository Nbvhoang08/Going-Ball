using System.Collections.Generic;
using UnityEngine;

namespace Hapiga.Core.Runtime.Process
{
    public class ProcessManager
    {
        private List<Process> processes;
        private List<Process> toRemove;
        private List<Process> toStart;

        public ProcessManager()
        {
            processes = new List<Process>(100);
            toRemove = new List<Process>(20);
            toStart = new List<Process>(20);
        }

        public void LaunchProcess(Process process)
        {
            processes.Add(process);
            process.Start();
        }

        public void UpdateProcesses(float dt)
        {
            for (int i = 0; i < processes.Count; i++)
            {
                processes[i].Update(dt);
                switch (processes[i].State)
                {
                    case ProcessState.TERMINATED:
                        TerminateProcess(processes[i]);
                        break;
                    case ProcessState.CANCELLED:
                        CancelProcess(processes[i]);
                        break;
                }
            }
            //foreach (Process process in processes) {
            //             process.Update(dt);
            //             switch (process.State) {
            //                 case ProcessState.TERMINATED:
            //                     TerminateProcess(process);
            //                     break;
            //                 case ProcessState.CANCELLED:
            //                     CancelProcess(process);
            //                     break;
            //             }
            //}

            UpdateProcessList();
        }

        public void PauseProcesses(bool isPause)
        {
            for (int i = 0; i < processes.Count; i++)
            {
                if (processes[i].State != ProcessState.TERMINATED && processes[i].State != ProcessState.CANCELLED)
                {
                    processes[i].Pause(isPause);
                }
            }

            //foreach (Process process in processes)
            //{
            //    if (process.State != ProcessState.TERMINATED && process.State != ProcessState.CANCELLED)
            //    { process.Pause(isPause); }             
            //}
        }

        private void TerminateProcess(Process process)
        {
            process.OnTerminate();
            process.InvokeTerminateCallback();
            if (process.State == ProcessState.TERMINATED)
            {
                toRemove.Add(process);
            }

            if (process.Attached.Count > 0)
            {
                for (int i = 0; i < process.Attached.Count; i++)
                {
                    toStart.Add(process.Attached[i]);
                }
            }

            //foreach (Process attached in process.Attached) {
            //    toStart.Add(attached);
            //}
        }

        private void CancelProcess(Process process)
        {
            toRemove.Add(process);
        }

        private void UpdateProcessList()
        {
            for (int i = 0; i < toRemove.Count; i++)
            {
                processes.Remove(toRemove[i]);
            }

            //foreach (Process remProc in toRemove) {
            //             processes.Remove(remProc);
            //         }
            toRemove.Clear();
            for (int i = 0; i < toStart.Count; i++)
            {
                LaunchProcess(toStart[i]);
            }

            //foreach (Process staProc in toStart) {
            //             LaunchProcess(staProc);
            //         }
            toStart.Clear();
        }
    }
}