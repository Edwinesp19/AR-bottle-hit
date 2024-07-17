using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public event Action OnMainMenu;
    public event Action OnMoreMenu;
    public event Action OnCloseMenu;
    public event Action OnItemsMenu;
    public event Action OnARPosition;
    public static GameManager instance;

    public bool isLevelFinished = false;

    public TMP_Text scoreText;
    public TMP_Text pausedText;

    private float currentScore = 0;

    // public Image backgroundLayout;
    public GameObject gameMenuLayout;
    public RawImage gameMenuLayoutBackground;

    public TMP_Text titleMenuLayout;
    public TMP_Text scoreMenuLayout;
    public TMP_Text textoTiempo;

    AudioSource audioSource;
    public AudioClip sonidoGanar;
    public AudioClip sonidoPausa;
    public AudioClip sonidoClick;
    public AudioClip sonidoPerder;

    public AudioSource backgroundMusic;

    private Color winColor = new Color32(0, 255, 0, 130); // Green
    private Color loseColor = new Color32(255, 0, 0, 130); // Red

    public bool isGamePaused = false;

    public float MaxTime;
    private float time;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentScore = 0;
        scoreText.text = string.Format("Score: {0}", currentScore);
        time = MaxTime;
        audioSource = GetComponent<AudioSource>();
        isGamePaused = false;
        isLevelFinished = false;
        CloseMenu();
        HideGameMenuLayout();
    }

    void Update()
    {

        //para manejar el tiempo
        if (time > 0 && isLevelFinished == false && isGamePaused == false)
        {
            time -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            textoTiempo.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
        else
        {
            if (isGamePaused == false)
            {

                time = 0;
                textoTiempo.text = "00:00";

                if (isLevelFinished == false)
                {
                    GameOver();
                }
            }

        }
        if (isGamePaused == true)
        {
            pausedText.text = "Pausado";
            return;
        }
        if (isLevelFinished)
        {
            return;
        }
        GameObject bottleList = GameObject.FindGameObjectWithTag("Bottles");
        //check if the parent of this object has childrens
        if (bottleList.transform.childCount == 0)
        {
            GameWin();
        }
        else
        {
            Debug.Log("There are still bottles: " + bottleList.transform.childCount);
        }
    }


    public void UpdateScore(float score)
    {
        if (isLevelFinished)
        {
            return;
        }
        currentScore += score;
        scoreText.text = string.Format("Score: {0}", currentScore);

    }

    public void MainMenu()
    {

        OnMainMenu?.Invoke();
        Debug.Log("Main Menu Activated");
    }
    public void MoreMenu()
    {
        pausedText.text = "";
        if (MaxTime >= time)
        {
            pausedText.text = "Pausado";

            isGamePaused = true;
            audioSource.PlayOneShot(sonidoPausa, 1f);
            backgroundMusic.DOPitch(0.7f, 1f);
        }
        else if (isGamePaused == true)
        {
            isGamePaused = false;
            backgroundMusic.DOPitch(1f, 1f);
        }


        OnMoreMenu?.Invoke();
        Debug.Log("More Menu Activated");
    }
    public void CloseMenu()
    {
        pausedText.text = "";
        isGamePaused = false;
        backgroundMusic.DOPitch(1f, 1f);
        OnCloseMenu?.Invoke();
        Debug.Log("Close Menu Activated");
    }

    public void ItemsMenu()
    {
        OnItemsMenu?.Invoke();
        Debug.Log("Items Menu Activated ");
    }

    public void ARPosition()
    {
        OnARPosition?.Invoke();
        Debug.Log("AR Position Activated");
    }

    public void QuitApp()
    {
        Application.Quit();
    }


    void GameWin()
    {
        audioSource.PlayOneShot(sonidoGanar, 1f);
        isLevelFinished = true;
        titleMenuLayout.text = "Ganaste";
        scoreMenuLayout.text = string.Format("Score: {0}", currentScore);
        gameMenuLayoutBackground.color = winColor;
        gameMenuLayout.SetActive(true);
    }
    void GameOver()
    {
        audioSource.PlayOneShot(sonidoPerder, 1f);
        isLevelFinished = true;
        titleMenuLayout.text = "Perdiste";
        scoreMenuLayout.text = string.Format("Score: {0}", currentScore);
        gameMenuLayoutBackground.color = loseColor;
        gameMenuLayout.SetActive(true);

        //change the background music pitch from 1 to downgrade to 0 in 3 seconds
        audioSource.DOPitch(0, 4f);
        backgroundMusic.DOPitch(0, 6f);

    }

    public void RestartGame()
    {

        audioSource.PlayOneShot(sonidoClick, 1f);
        StartCoroutine(Wait(2f));
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        audioSource.PlayOneShot(sonidoClick, 1f);
        Debug.Log("Exit Game");
        StartCoroutine(Wait(2f));
        Application.Quit();
    }
    public void GoHome()
    {
        audioSource.PlayOneShot(sonidoClick, 1f);
        Debug.Log("Go Home Game");
        StartCoroutine(Wait(2f));
        SceneManager.LoadScene("menu-inicio");

    }

    void HideGameMenuLayout()
    {
        gameMenuLayout.SetActive(false);

    }

    private IEnumerator Wait(float time)
    {
        //Wait for random amount of time
        yield return new WaitForSeconds(time);
    }

}
