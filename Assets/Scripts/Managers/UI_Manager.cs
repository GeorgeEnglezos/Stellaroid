    using UnityEngine;
    using UnityEngine.UI;
    public class UI_Manager : MonoBehaviour {
        
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject ControlsPanel;
    [SerializeField] private GameObject androidPanel;
    [SerializeField] private GameObject windowsPanel;

    public static UI_Manager Instance;
    private void Awake()
    {
        Instance = this;
    }


    public void setBuildUI(bool isItDesktop)
    {

        if (!isItDesktop)
        {
            androidPanel.SetActive(true);
            windowsPanel.SetActive(false);
        }
        else
        {            
            androidPanel.SetActive(false);
            windowsPanel.SetActive(true);
        }
    }
    public void changeHealthBar(int maxHp, int currentHp)
    {
        healthBar.maxValue = maxHp;
        healthBar.value = currentHp;    
    }
    public void GameOver()
    {
        GameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }



    public void startGame()
    {
        ControlsPanel.SetActive(false);
        EventBroadcaster.call_Event_Game_Start();
    }

    public void continueGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void restartGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        GameOverPanel.SetActive(false);
        EventBroadcaster.call_Event_Game_Start();
    }


        void Update()
        {
            if ((Input.GetKey(KeyCode.Escape)||Input.GetKey(KeyCode.P)) && GameManager.Instance.gameStarted)
            {
                pauseGame();
            }
        }
        private void pauseGame()
         {
             pausePanel.SetActive(true);
             Time.timeScale = 0;
         }
    }
