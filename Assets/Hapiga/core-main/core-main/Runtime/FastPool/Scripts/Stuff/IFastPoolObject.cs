using UnityEngine;
using System.Collections;

namespace Hapiga.Core.Runtime.Pool
{
    public interface IFastPoolItem
    {
        void OnFastInstantiate();
        void OnFastDestroy();
    }
}