using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public GameObject panelMain, panelSettings;

    public GameObject CheckBox_FunModes;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        LoadSettings();
    }

    public void OpenNormal()
    {
        Settings.instance.ChangeGameModeToNormal();
        SceneManager.LoadScene("Normal");
    }

    public void OpenKidsDream()
    {
        Settings.instance.ChangeGameModeToKidsDream();
        SceneManager.LoadScene("KidsDream");
    }

    public void OpenSettings()
    {
        panelMain.SetActive(false);
        panelSettings.SetActive(true);
    }

    public void CloseSettings()
    {
        panelMain.SetActive(true);
        panelSettings.SetActive(false);
    }

    public void FunModesCheckBox()
    {
        var t = CheckBox_FunModes.GetComponent<Toggle>();
        Settings.instance.SetFunModes(t.isOn);
    }

    public void LoadSettings()
    {
        CheckBox_FunModes.GetComponent<Toggle>().isOn = Settings.instance.funModes;
    }
}
