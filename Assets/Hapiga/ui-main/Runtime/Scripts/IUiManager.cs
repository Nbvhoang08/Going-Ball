namespace Hapiga.UI
{
    public interface IUiManager
    {
        void InitBackgroundQueue(int initCount = 3);
        UIBackground GetBackgroundPanel();
        void HideBackgroundPanel(UIBackground bg, bool instantAction);
    }
}