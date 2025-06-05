using UnityEngine;

namespace Hapiga.Core.Runtime.TickManager
{
    public class TickableBehaviour : MonoBehaviour, ITickable
    {
        protected bool isAdded;
        private int listIndex = -1;

        #region Unity Methods

        public void OnEnable()
        {
            OnTickableEnabled();
        }

        public void OnDisable()
        {
            OnTickableDisabled();
        }

        #endregion Unity Methods

        #region Tickable Functions

        public virtual void OnTickableDisabled()
        {
            if (isAdded)
            {
                TickManager.Instance.RemoveTickable(this);
                isAdded = false;
            }
        }

        public virtual void OnTickableEnabled()
        {
            TickManager.Instance.AddTickable(this);
            isAdded = true;
        }

        public virtual void OnTickableUpdated(float dt)
        {
        }

        public virtual void OnTickableFixedUpdated(float dt)
        {
            
        }
        public virtual int GetId()
        {
            return typeof(TickableBehaviour).GetHashCode();
        }

        public void SetListIndex(int index)
        {
            listIndex = index;
        }

        public int GetListIndex()
        {
            return listIndex;
        }

        #endregion Tickable Functions
    }
}