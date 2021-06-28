using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialEventManager : MonoBehaviour
{
    public static TutorialEventManager instance;

    public TutorialCount currentHint;

    public GameObject secondPhaseActiveGroup;
    public GameObject thirdPhaseActiveGroup;

    public enum TutorialCount
    {
        HINT_MOVEFORWARD = 0,
        HINT_ROTATELEFT = 1,
        HINT_ROTATERIGHT = 2,
        HINT_LEFTCLICK = 3,
        HINT_RIGHTCLICK = 4,
        HINT_MOVETOMINES = 5,
        HINT_DESTROYMINES = 6,
        HINT_MOVETOENEMYSHIP = 7,
        HINT_FIGHTENEMYSHIP = 8,
        HINT_FIGHTBOSSSHIP = 9
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        currentHint = TutorialCount.HINT_MOVEFORWARD;
    }

    public void ExitTutorial() 
    {
        SceneManager.LoadScene("Assets/Arda/Scenes/MainMenu.unity");
    }

    public void PlayerKill(GameObject attackingPlayer, GameObject opposingPlayer)
    {
        if (opposingPlayer.GetComponent<Player>().playerHealth <= 0)
        {
            attackingPlayer.GetComponent<Player>().killCount++;
        }
    }
    public void CheckTutorialConditions()
    {
        Debug.Log(currentHint);
        switch (currentHint)
        {
            case TutorialCount.HINT_MOVEFORWARD:
                TutorialInfoManager.instance.PlayNextLine();
                currentHint = TutorialCount.HINT_ROTATELEFT;
                break;
            case TutorialCount.HINT_ROTATELEFT:
                TutorialInfoManager.instance.PlayNextLine();
                currentHint = TutorialCount.HINT_ROTATERIGHT;
                break;
            case TutorialCount.HINT_ROTATERIGHT:
                TutorialInfoManager.instance.PlayNextLine();
                currentHint = TutorialCount.HINT_LEFTCLICK;
                break;
            case TutorialCount.HINT_LEFTCLICK:
                TutorialInfoManager.instance.PlayNextLine();
                currentHint = TutorialCount.HINT_RIGHTCLICK;
                break;
            case TutorialCount.HINT_RIGHTCLICK:
                TutorialInfoManager.instance.PlayNextLine();
                currentHint = TutorialCount.HINT_MOVETOMINES;
                break;
            case TutorialCount.HINT_MOVETOMINES:
                TutorialInfoManager.instance.PlayNextLine();
                Init2ndPhase();
                currentHint = TutorialCount.HINT_DESTROYMINES;
                break;
            case TutorialCount.HINT_DESTROYMINES:
                TutorialInfoManager.instance.PlayNextLine();
                currentHint = TutorialCount.HINT_MOVETOENEMYSHIP;
                break;
            case TutorialCount.HINT_MOVETOENEMYSHIP:
                TutorialInfoManager.instance.PlayNextLine();
                currentHint = TutorialCount.HINT_FIGHTENEMYSHIP;
                break;
            case TutorialCount.HINT_FIGHTENEMYSHIP:
                TutorialInfoManager.instance.PlayNextLine();
                Init3rdPhase();
                currentHint = TutorialCount.HINT_FIGHTBOSSSHIP;
                break;
            case TutorialCount.HINT_FIGHTBOSSSHIP:
                TutorialInfoManager.instance.PlayNextLine();
                break;
            default:
                break;
        }
    }
    private void Init2ndPhase()
    {
        secondPhaseActiveGroup.SetActive(true);
    }

    private void Init3rdPhase()
    {
        thirdPhaseActiveGroup.SetActive(true);
    }
}
