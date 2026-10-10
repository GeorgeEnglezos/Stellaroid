using System;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score Instance;
    public float score;
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        EventBroadcaster.OnGameStart += call_Event_Game_Start;
        EventBroadcaster.onHitAsteroid += event_hitAsteroid;
        EventBroadcaster.onDestroyAsteroid += event_destroyAsteroid;
    }

    private void OnDisable()
    {
        EventBroadcaster.onHitAsteroid -= event_hitAsteroid;
        EventBroadcaster.onDestroyAsteroid -= event_destroyAsteroid;
        EventBroadcaster.OnGameStart -= call_Event_Game_Start;
        
    }
    private void call_Event_Game_Start(object sender, EventArgs e)
    {
        score = 0;
        text.text = "Score:0";
    }

    private void event_destroyAsteroid(object sender, EventBroadcaster.ListEventArgs e)
    {
        score += int.Parse(e.Data[0])*7;
        text.text = "score:" + score;

    }

    private void event_hitAsteroid(object sender, EventBroadcaster.ListEventArgs e)
    {
        score += int.Parse(e.Data[0]);
        text.text = "score:" + score;
    }
}
