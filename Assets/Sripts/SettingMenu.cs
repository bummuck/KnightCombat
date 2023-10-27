using UnityEngine;
using UnityEngine.Audio;

public class SettingMenu : MonoBehaviour
{
    public void SetVolume (float volume)
    {
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Debug.Log("Fullscreen" + isFullScreen);
        Screen.fullScreen = isFullScreen;
    }
}
