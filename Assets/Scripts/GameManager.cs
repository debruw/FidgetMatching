using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Application.targetFrameRate = 60;

        PlayerPrefs.SetInt("FromMenu", 1);
        if (PlayerPrefs.GetInt("FromMenu") == 1)
        {
            ContinueText.SetActive(false);
            PlayerPrefs.SetInt("FromMenu", 0);
        }
        else
        {
            PlayText.SetActive(false);
        }
        currentLevel = PlayerPrefs.GetInt("LevelId");
        LevelText.text = "LEVEL " + currentLevel.ToString();
    }

    public ItemManager itemManager;

    public Animator AIHandsAnimator, PlayerHandsAnimator;
    public int AIPoints, PlayerPoints;

    public int currentLevel = 1;
    int MaxLevelNumber = 1;
    public bool isGameStarted, isGameOver, isTableTurning;

    #region UI Elements
    public GameObject WinPanel, LosePanel, InGamePanel;
    public Button TapToStartButton;
    public Text LevelText;
    public GameObject PlayText, ContinueText;
    #endregion

    public void ButtonPressed(bool whoPressed, GameButton.ButtonType type)
    {
        if (whoPressed)
        {//Player Pressed
            PlayerHandsAnimator.SetTrigger(type.ToString());
        }
        else
        {//AIPressed
            AIHandsAnimator.SetTrigger(type.ToString());
        }
    }

    public IEnumerator WaitAndGameWin()
    {
        Debug.Log("Win");
        isGameOver = true;
        //SoundManager.Instance.StopAllSounds();
        //SoundManager.Instance.playSound(SoundManager.GameSounds.Win);

        yield return new WaitForSeconds(1f);

        //if (PlayerPrefs.GetInt("VIBRATION") == 1)
        //    TapticManager.Impact(ImpactFeedback.Light);

        currentLevel++;
        PlayerPrefs.SetInt("LevelId", currentLevel);
        WinPanel.SetActive(true);
    }

    public IEnumerator WaitAndGameLose()
    {
        Debug.Log("Lose");
        isGameOver = true;
        //SoundManager.Instance.playSound(SoundManager.GameSounds.Lose);

        yield return new WaitForSeconds(1f);

        //if (PlayerPrefs.GetInt("VIBRATION") == 1)
        //    TapticManager.Impact(ImpactFeedback.Medium);

        LosePanel.SetActive(true);
    }

    public void TapToNextButtonClick()
    {
        if (currentLevel > MaxLevelNumber)
        {
            int rand = Random.Range(1, MaxLevelNumber);
            if (rand == PlayerPrefs.GetInt("LastRandomLevel"))
            {
                rand = Random.Range(1, MaxLevelNumber);
            }
            else
            {
                PlayerPrefs.SetInt("LastRandomLevel", rand);
            }
            SceneManager.LoadScene("Level" + rand);
        }
        else
        {
            SceneManager.LoadScene("Level" + currentLevel);
        }
    }

    public void TapToTryAgainButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TapToStartButtonClick()
    {
        isGameStarted = true;
        TapToStartButton.gameObject.SetActive(false);
        StartCoroutine(ItemManager.Instance.aiController.WaitAndThrowRandomAIObject());
    }
}
