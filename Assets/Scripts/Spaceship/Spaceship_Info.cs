using UnityEngine;

public class Spaceship_Info : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]private int maxHP;
    [SerializeField]private int currentHP;
    [SerializeField]private bool isAlive;
    [Header("Components")]
    [SerializeField] private Animator _dissolveAnimator;
    private Rigidbody2D rb;
    public static Spaceship_Info Instance;
 
    private void Awake()
    {
        Instance = this;
        EventBroadcaster.onAsteroidCrush += event_crushed;
        rb = GetComponent<Rigidbody2D>();

    }
    private void OnDisable()
    {
        EventBroadcaster.onAsteroidCrush -= event_crushed;
    }

    public void resetStats()
    {
        Invoke("makeSpaceshipAppear",.5f);
        transform.position = new Vector3(0, 0, 0);
        currentHP = maxHP;
        isAlive = true;
    }

    private void makeSpaceshipAppear()
    {
        _dissolveAnimator.Play("Spawn_Animation");
    }

    private void event_crushed(object sender, EventBroadcaster.ListEventArgs e)
    {
        if (int.Parse(e.Data[0])<2)
        {
            currentHP -= 2;
            
        }else if (int.Parse(e.Data[0])<4)
        {
            currentHP -= 5;
        }
        else
        {
            currentHP -= 10;
        }
        UI_Manager.Instance.changeHealthBar(maxHP,currentHP);
        if (currentHP <= 0 && isAlive)
        {        
            isAlive = false;
            _dissolveAnimator.Play("Death_Animation");
            Invoke("callGameOver",.9f);
        }
    }

    public void takeDamage(int Damage)
    {
        currentHP -= Damage;
        UI_Manager.Instance.changeHealthBar(maxHP,currentHP);
        if (currentHP <= 0 && isAlive)
        {        
            isAlive = false;
            _dissolveAnimator.Play("Death_Animation");
            Invoke("callGameOver",.9f);
        }
    }
    public void increaseHealth(int hp)
    {
        if (currentHP+hp>maxHP)
        {
            currentHP = maxHP;
        }
        else
        {
            currentHP += hp;
        }
        UI_Manager.Instance.changeHealthBar(100,currentHP);

    }
    private void callGameOver()
    {
        rb.linearVelocity=Vector2.zero;
        EventBroadcaster.call_Event_Game_Over();
        
    }
}
