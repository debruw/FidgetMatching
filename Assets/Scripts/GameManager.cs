using System.Collections;
using System.Collections.Generic;
using TapticPlugin;
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
    public MainCharacterScript maincharacter;

    public int currentLevel = 1;
    int MaxLevelNumber = 5;
    public bool isGameStarted, isGameOver, isTableTurning, isPlayersTurn;

    #region UI Elements
    public GameObject WinPanel, LosePanel, InGamePanel;
    public Button TapToStartButton;
    public Text LevelText;
    public GameObject PlayText, ContinueText;
    public GameObject Tutorial1, Tutorial2;
    #endregion

    public void ButtonPressed(GameButton.ButtonType type)
    {
        PlayerHandsAnimator.SetTrigger(type.ToString());
    }

    public IEnumerator WaitAndGameWin()
    {
        Debug.Log("Win");
        isGameOver = true;
        maincharacter.bcgTRS.Speed = 0;
        SoundManager.Instance.StopAllSounds();
        SoundManager.Instance.playSound(SoundManager.GameSounds.Win);

        yield return new WaitForSeconds(1f);

        if (PlayerPrefs.GetInt("VIBRATION") == 1)
            TapticManager.Impact(ImpactFeedback.Light);

        currentLevel++;
        PlayerPrefs.SetInt("LevelId", currentLevel);
        WinPanel.SetActive(true);
    }

    public IEnumerator WaitAndGameLose()
    {
        Debug.Log("Lose");
        isGameOver = true;
        maincharacter.bcgTRS.Speed = 0;
        SoundManager.Instance.playSound(SoundManager.GameSounds.Lose);

        yield return new WaitForSeconds(1f);

        if (PlayerPrefs.GetInt("VIBRATION") == 1)
            TapticManager.Impact(ImpactFeedback.Medium);

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

        maincharacter.bcgTRS.Speed = maincharacter.zSpeed;
        maincharacter.PlayerAnimator.SetTrigger("Run");

        if (currentLevel == 1)
        {
            Tutorial1.SetActive(true);
        }
    }
}
