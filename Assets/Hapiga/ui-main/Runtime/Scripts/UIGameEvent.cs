using Hapiga.Core.Runtime.EventManager;
using UnityEngine;

namespace Hapiga.UI
{
    public class UIBackgroundShowEvent : GameEvent
    {
        public UIElement uiElement;
        public bool instantAction;
    }

    public class UIBackgroundHideEvent : GameEvent
    {
        public UIElement uiElement;
        public bool instantAction;
    }
}