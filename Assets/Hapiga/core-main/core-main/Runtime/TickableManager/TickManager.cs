using System.Collections.Generic;
using Hapiga.Core.Runtime.Extensions;
using Hapiga.Core.Runtime.Singleton;
using UnityEngine;

namespace Hapiga.Core.Runtime.TickManager
{
    [DefaultExecutionOrder(-4997)]
    [Singleton("UpdateManager", true)]
    public class TickManager : Singleton<TickManager>
    {
        public Dictionary<int, List<ITickable>> TickableDict { get; private set; }
        public List<ITickable> TickableList { get; private set; }
        private List<int> keyList;

        public override void Init()
        {
            keyList = new List<int>(1000);
            TickableDict = new Dictionary<int, List<ITickable>>(1000);
            TickableList = new List<ITickable>(1000);
        }

        private void Update()
        {
            if (keyList.Count == 0)
            {
                return;
            }

            for (int j = 0; j < keyList.Count; j++)
            {
                List<ITickable> temp = TickableDict[keyList[j]];
                if (temp.IsNullOrEmpty())
                {
                    continue;
                }

                for (int i = 0; i < temp.Count; i++)
                {
                    temp[i].OnTickableUpdated(Time.deltaTime);
                }
            }
        }
        private void FixedUpdate()
        {
            if (keyList.Count == 0)
            {
                return;
            }

            for (int j = 0; j < keyList.Count; j++)
            {
                List<ITickable> temp = TickableDict[keyList[j]];
                if (temp.IsNullOrEmpty())
                {
                    continue;
                }

                for (int i = 0; i < temp.Count; i++)
                {
                    temp[i].OnTickableFixedUpdated(Time.fixedDeltaTime);
                }
            }
        }

        public void AddTickable(ITickable tickable)
        {
            int id = tickable.GetId();
            if (!TickableDict.ContainsKey(id))
            {
                TickableDict.Add(id, new List<ITickable>(500));
                keyList.Add(id);
            }

            TickableDict[id].Add(tickable);
            tickable.SetListIndex(TickableDict[id].Count - 1);
        }

        public void RemoveTickable(ITickable tickable)
        {
            TickableDict[tickable.GetId()].Remove(tickable);
        }
    }
}