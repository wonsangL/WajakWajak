using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {
    private GameManager Instance;

    public GameObject MainMenu;
    public GameObject GamePanel;

    public AudioClip AudioClip;
    public GameObject ScorePanel;
    public GameObject ReadyPanel;
    public GameObject PressAnyKey;
    public GameObject Go;
    public GameObject InGamePanel;
    public GameObject Cookie;
    public GameObject Timer;
    public GameObject First;
    public GameObject Second;
    public GameObject Third;

    public string speech = "";

    private Sprite[] Cookies;
    private Sprite[] Scores;
    private Sprite[] Counts;
    private string previousMsg = "";

    private AudioSource audioSource;
    private float score = 0f;
    private int timer = 10;
    private bool isVibrate = false;
    private bool isStart = false;

    private void Awake()
    {
        InitSprite();
        audioSource = transform.gameObject.GetComponent<AudioSource>();
    }

	// Update is called once per frame
	void Update () {
		if((Input.touchCount > 0 || Input.GetKey(KeyCode.A)) && !isStart)
        {
             StopAllCoroutines(); //stop fadein/out coroutine
             ReadyPanel.SetActive(false);
             isStart = true;

             StartCoroutine(StartGame());
        }
        else
        {
            Color color = PressAnyKey.GetComponent<Image>().color;

            if (color.a >= 1)
                StartCoroutine(FadeOut(PressAnyKey));
            else if (color.a <= 0)
                StartCoroutine(FadeIn(PressAnyKey, 1));
        }
	}

    IEnumerator StartGame()
    {
        Go.SetActive(true);

        yield return new WaitForSeconds(1);

        Go.SetActive(false);
        InGamePanel.SetActive(true);

        StartCoroutine(CountWord());
    }

    IEnumerator CountWord()
    {
        while(timer > 0 && 140 - score > 0)
        {
            int input = 0;

            if (!previousMsg.Equals(speech))
            {
                input = speech.Length / 3;

                if(input > 0)
                    audioSource.PlayOneShot(AudioClip);

                previousMsg = speech;
            }
            
            score += input;
            Debug.Log("current score: " + score + ", input: " + input + ", speech length: " + speech.Length);

            if (score >= 70 && !isVibrate)
            {
                Handheld.Vibrate();
                isVibrate = true;
            }
               
            timer -= 1;
            Timer.GetComponent<Image>().sprite = Counts[timer];
            yield return new WaitForSeconds(1);
        }

        InGamePanel.SetActive(false);

        int index = (int)((score > 140 ? 140 : score) / 140 * 8);
        Debug.Log("score: " + score + " index: " + index);
        Cookie.GetComponent<Image>().sprite = Cookies[index];

        yield return new WaitForSeconds(1);
        ScorePanel.SetActive(true);

        score = score > 140 ? 0 : score;

        Third.GetComponent<Image>().sprite = Scores[((int)score % 10)];
        score /= 10;
        Second.GetComponent<Image>().sprite = Scores[((int)score % 10)];
        score /= 10;
        First.GetComponent<Image>().sprite = Scores[((int)score % 10)];       
    }

    IEnumerator FadeIn(GameObject paramObject, float alpha)
    {
        Color color = paramObject.GetComponent<Image>().color;

        while (color.a < alpha)
        {
            color.a += 0.1f;
            paramObject.GetComponent<Image>().color = color;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator FadeOut(GameObject paramObject)
    {
        Color color = paramObject.GetComponent<Image>().color;

        while (color.a > 0)
        {
            color.a -= 0.1f;
            paramObject.GetComponent<Image>().color = color;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void InitSprite()
    {
        string path;

        Cookies = new Sprite[9];
        
        for(int i = 0; i < 9; i++)
        {
            path = "Sprites/chocolate_" + (i + 1);
            Cookies[i] = Resources.Load<Sprite>(path);
        }

        Scores = new Sprite[10];

        for (int i = 0; i < 10; i++)
        {
            path = "Sprites/Score/" + i;
            Scores[i] = Resources.Load<Sprite>(path);
        }

        Counts = new Sprite[11];

        for (int i = 0; i < 11; i++)
        {
            path = "Sprites/Count/" + i;
            Counts[i] = Resources.Load<Sprite>(path);
        }
    }

    public GameManager GetInstance()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        return Instance;
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnExit()
    {
        ScorePanel.SetActive(false);
        MainMenu.SetActive(true);
        GamePanel.SetActive(false);
        InitFeild();
    }

    public void OnRetry()
    {
        //fade out panel and fade
        ScorePanel.SetActive(false);
        ReadyPanel.SetActive(true);
        InitFeild();
    }

    public void OnGame()
    {
        MainMenu.SetActive(false);
        GamePanel.SetActive(true);
    }

    private void InitFeild()
    {
        StopAllCoroutines();
        Cookie.GetComponent<Image>().sprite = Cookies[0];
        score = 0;
        timer = 10;
        isVibrate = false;
        isStart = false;

        Color color = PressAnyKey.GetComponent<Image>().color;
        color.a = 1f;

        PressAnyKey.GetComponent<Image>().color = color;
    }

}
