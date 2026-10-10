using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("BUILD")]
    public bool DesktopBuild;
    public bool AndroidBuild;

    [Header("GameMode_Info")] 
    [SerializeField] private GameObject Player;
    public int difficultyLevel;
    public List<Difficulties> difficultiesList;
    public bool gameStarted;
    public static GameManager Instance;

    [System.Serializable]
    public class Difficulties
    {
        public int ammoDropChance;
        public int healthDropChance;
        public int MeteorSpawnCount;
        public int maxThrust;
        public int maxTorque;
        public int nextDifficultyUnlock;
    }
    private void Awake()
    {
        Instance = this;
        gameStarted = false;
        
        //Events
        EventBroadcaster.OnGameStart += Event_GameStart;
        EventBroadcaster.OnGameOver += Event_GameOver;
        
        SoundManager.Inst.Play("Main",true,.1f);

    }

    private void OnDisable()
    {
        //Events
        EventBroadcaster.OnGameStart -= Event_GameStart;
        EventBroadcaster.OnGameOver -= Event_GameOver;    
    }

    private void Start()
    {
        UI_Manager.Instance.setBuildUI(DesktopBuild);
    }

    private void Update()
    {
        if (Score.Instance.score>difficultiesList[difficultyLevel].nextDifficultyUnlock)
        {
            if (difficultyLevel!=difficultiesList.Count-1)
            {
                difficultyLevel++;
            }
        }
    }

    private void Event_GameStart(object sender, EventArgs e)
    {
        difficultyLevel=0;
        Player.SetActive(true);
        gameStarted = true;
        Spaceship_Weapons.Instance.resetWeapons();
        Spaceship_Info.Instance.resetStats();
        UI_Manager.Instance.changeHealthBar(100,100);
    }

    private void Event_GameOver(object sender, EventArgs e)
    {
        UI_Manager.Instance.GameOver();
    }
}

