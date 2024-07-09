using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings instance;

    public bool GameModeKidsDream, GameModeNormal;
    public bool funModes;

    public float timeForGame, timeFor3DMove;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void ResetGameMode()
    {
        GameModeKidsDream = false;
        GameModeNormal = false;
    }

    public void ChangeGameModeToKidsDream()
    {
        ResetGameMode();
        GameModeKidsDream = true;
    }

    public void ChangeGameModeToNormal()
    {
        ResetGameMode();
        GameModeNormal = true;
    }

    public void SetFunModes(bool value)
    {
        funModes = value;
    }
}
