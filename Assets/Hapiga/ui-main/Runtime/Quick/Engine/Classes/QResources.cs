using UnityEngine;

namespace QuickEngine
{
    public partial class QResources
    {
        private static Font fontAwesome;
        public static Font FontAwesome { get { if (fontAwesome == null) { fontAwesome = Resources.Load("Quick/Fonts/FontAwesome") as Font; } return fontAwesome; } }
    }
}
