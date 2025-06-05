using UnityEngine;
using UnityEditor;
using QuickEditor;

namespace Hapiga.UI.Editor
{
    public partial class DUIResources
    {

        private static string _IMAGES;
        public static string IMAGES { get { if(string.IsNullOrEmpty(_IMAGES)) { _IMAGES = DUI.PATH + "/Images/"; } return _IMAGES; } }

        private static string _BARS;
        public static string BARS { get { if(string.IsNullOrEmpty(_BARS)) { _BARS = IMAGES + "Bars/"; } return _BARS; } }

        //private static string _CONTROLPANEL;
        //public static string CONTROLPANEL { get { if(string.IsNullOrEmpty(_CONTROLPANEL)) { _CONTROLPANEL = IMAGES + "ControlPanel/"; } return _CONTROLPANEL; } }

        private static string _HEADERS;
        public static string HEADERS { get { if(string.IsNullOrEmpty(_HEADERS)) { _HEADERS = IMAGES + "Headers/"; } return _HEADERS; } }

        private static string _ICONS;
        public static string ICONS { get { if(string.IsNullOrEmpty(_ICONS)) { _ICONS = IMAGES + "Icons/"; } return _ICONS; } }


        //BARS
        public static QTexture presetCaret0 = new QTexture(BARS, "presetCaret0" + QResources.IsProSkinTag);
        public static QTexture presetCaret1 = new QTexture(BARS, "presetCaret1" + QResources.IsProSkinTag);
        public static QTexture presetCaret2 = new QTexture(BARS, "presetCaret2" + QResources.IsProSkinTag);
        public static QTexture presetCaret3 = new QTexture(BARS, "presetCaret3" + QResources.IsProSkinTag);
        public static QTexture presetCaret4 = new QTexture(BARS, "presetCaret4" + QResources.IsProSkinTag);
        public static QTexture presetCaret5 = new QTexture(BARS, "presetCaret5" + QResources.IsProSkinTag);
        public static QTexture presetCaret6 = new QTexture(BARS, "presetCaret6" + QResources.IsProSkinTag);
        public static QTexture presetCaret7 = new QTexture(BARS, "presetCaret7" + QResources.IsProSkinTag);
        public static QTexture presetCaret8 = new QTexture(BARS, "presetCaret8" + QResources.IsProSkinTag);
        public static QTexture presetCaret9 = new QTexture(BARS, "presetCaret9" + QResources.IsProSkinTag);
        public static QTexture presetCaret10 = new QTexture(BARS, "presetCaret10" + QResources.IsProSkinTag);

        public static QTexture moveCaret0 = new QTexture(BARS, "moveCaret0" + QResources.IsProSkinTag);
        public static QTexture moveCaret1 = new QTexture(BARS, "moveCaret1" + QResources.IsProSkinTag);
        public static QTexture moveCaret2 = new QTexture(BARS, "moveCaret2" + QResources.IsProSkinTag);
        public static QTexture moveCaret3 = new QTexture(BARS, "moveCaret3" + QResources.IsProSkinTag);
        public static QTexture moveCaret4 = new QTexture(BARS, "moveCaret4" + QResources.IsProSkinTag);
        public static QTexture moveCaret5 = new QTexture(BARS, "moveCaret5" + QResources.IsProSkinTag);
        public static QTexture moveCaret6 = new QTexture(BARS, "moveCaret6" + QResources.IsProSkinTag);
        public static QTexture moveCaret7 = new QTexture(BARS, "moveCaret7" + QResources.IsProSkinTag);
        public static QTexture moveCaret8 = new QTexture(BARS, "moveCaret8" + QResources.IsProSkinTag);
        public static QTexture moveCaret9 = new QTexture(BARS, "moveCaret9" + QResources.IsProSkinTag);
        public static QTexture moveCaret10 = new QTexture(BARS, "moveCaret10" + QResources.IsProSkinTag);

        public static QTexture rotateCaret0 = new QTexture(BARS, "rotateCaret0" + QResources.IsProSkinTag);
        public static QTexture rotateCaret1 = new QTexture(BARS, "rotateCaret1" + QResources.IsProSkinTag);
        public static QTexture rotateCaret2 = new QTexture(BARS, "rotateCaret2" + QResources.IsProSkinTag);
        public static QTexture rotateCaret3 = new QTexture(BARS, "rotateCaret3" + QResources.IsProSkinTag);
        public static QTexture rotateCaret4 = new QTexture(BARS, "rotateCaret4" + QResources.IsProSkinTag);
        public static QTexture rotateCaret5 = new QTexture(BARS, "rotateCaret5" + QResources.IsProSkinTag);
        public static QTexture rotateCaret6 = new QTexture(BARS, "rotateCaret6" + QResources.IsProSkinTag);
        public static QTexture rotateCaret7 = new QTexture(BARS, "rotateCaret7" + QResources.IsProSkinTag);
        public static QTexture rotateCaret8 = new QTexture(BARS, "rotateCaret8" + QResources.IsProSkinTag);
        public static QTexture rotateCaret9 = new QTexture(BARS, "rotateCaret9" + QResources.IsProSkinTag);
        public static QTexture rotateCaret10 = new QTexture(BARS, "rotateCaret10" + QResources.IsProSkinTag);

        public static QTexture scaleCaret0 = new QTexture(BARS, "scaleCaret0" + QResources.IsProSkinTag);
        public static QTexture scaleCaret1 = new QTexture(BARS, "scaleCaret1" + QResources.IsProSkinTag);
        public static QTexture scaleCaret2 = new QTexture(BARS, "scaleCaret2" + QResources.IsProSkinTag);
        public static QTexture scaleCaret3 = new QTexture(BARS, "scaleCaret3" + QResources.IsProSkinTag);
        public static QTexture scaleCaret4 = new QTexture(BARS, "scaleCaret4" + QResources.IsProSkinTag);
        public static QTexture scaleCaret5 = new QTexture(BARS, "scaleCaret5" + QResources.IsProSkinTag);
        public static QTexture scaleCaret6 = new QTexture(BARS, "scaleCaret6" + QResources.IsProSkinTag);
        public static QTexture scaleCaret7 = new QTexture(BARS, "scaleCaret7" + QResources.IsProSkinTag);
        public static QTexture scaleCaret8 = new QTexture(BARS, "scaleCaret8" + QResources.IsProSkinTag);
        public static QTexture scaleCaret9 = new QTexture(BARS, "scaleCaret9" + QResources.IsProSkinTag);
        public static QTexture scaleCaret10 = new QTexture(BARS, "scaleCaret10" + QResources.IsProSkinTag);

        public static QTexture fadeCaret0 = new QTexture(BARS, "fadeCaret0" + QResources.IsProSkinTag);
        public static QTexture fadeCaret1 = new QTexture(BARS, "fadeCaret1" + QResources.IsProSkinTag);
        public static QTexture fadeCaret2 = new QTexture(BARS, "fadeCaret2" + QResources.IsProSkinTag);
        public static QTexture fadeCaret3 = new QTexture(BARS, "fadeCaret3" + QResources.IsProSkinTag);
        public static QTexture fadeCaret4 = new QTexture(BARS, "fadeCaret4" + QResources.IsProSkinTag);
        public static QTexture fadeCaret5 = new QTexture(BARS, "fadeCaret5" + QResources.IsProSkinTag);
        public static QTexture fadeCaret6 = new QTexture(BARS, "fadeCaret6" + QResources.IsProSkinTag);
        public static QTexture fadeCaret7 = new QTexture(BARS, "fadeCaret7" + QResources.IsProSkinTag);
        public static QTexture fadeCaret8 = new QTexture(BARS, "fadeCaret8" + QResources.IsProSkinTag);
        public static QTexture fadeCaret9 = new QTexture(BARS, "fadeCaret9" + QResources.IsProSkinTag);
        public static QTexture fadeCaret10 = new QTexture(BARS, "fadeCaret10" + QResources.IsProSkinTag);

        //HEADERS
        public static QTexture headerUIElement = new QTexture(HEADERS, "headerUIElement" + QResources.IsProSkinTag);
        public static QTexture headerUICanvas = new QTexture(HEADERS, "headerUICanvas" + QResources.IsProSkinTag);
        public static QTexture headerUIButton = new QTexture(HEADERS, "headerUIButton" + QResources.IsProSkinTag);
        //ICONS

        public static QTexture miniIconShow = new QTexture(ICONS, "miniIconShow" + QResources.IsProSkinTag); //32x32
        public static QTexture miniIconHide = new QTexture(ICONS, "miniIconHide" + QResources.IsProSkinTag); //32x32
     

        private static Font m_Sansation;
        public static Font Sansation { get { if(m_Sansation == null) { m_Sansation = AssetDatabase.LoadAssetAtPath<Font>(DUI.PATH + "/Fonts/" + "Sansation-Regular.ttf"); } return m_Sansation; } }

        private static string m_ImagesPath;
        public static string ImagesPath { get { if(string.IsNullOrEmpty(m_ImagesPath)) { m_ImagesPath = DUI.PATH + "/Images/"; } return m_ImagesPath; } }


        //NOTIFICATION WINDOW
        private static string m_ImagesNotificationWindowPath;
        public static string ImagesNotificationWindowPath { get { if(string.IsNullOrEmpty(m_ImagesNotificationWindowPath)) { m_ImagesNotificationWindowPath = ImagesPath + "NotificationWindow/"; } return m_ImagesNotificationWindowPath; } }

    }
}
