using System;
using UnityEngine;

public class Enemy_Info : MonoBehaviour
{
    public bool canMove;
   [SerializeField] private int hp;
   [SerializeField] private int maxHP;
   [SerializeField] private bool isAlive;
   [SerializeField] private Animator anim;

   private void OnEnable()
   {
       EventBroadcaster.OnGameStart += Event_GameStarted;
   }

   private void OnDisable()
   {
       EventBroadcaster.OnGameStart -= Event_GameStarted;
   }

   private void Event_GameStarted(object sender, EventArgs e)
   {
       ObjectPooler.Instance.enqueueGameObject(gameObject,"Enemy",true);
   }

   void Start()
    {
        resetSpaceship();
    }

   public void resetSpaceship()
   {
       isAlive = true;
       hp = maxHP;
       //anim.Play();
       GetComponent<Enemy_Weapon>().resetWeapons();
       canMove = true;
   }

   public void takeDamage(int Damage)
   {
       hp -= Damage;
       if (hp<=0 && isAlive)
       {
           isAlive = false;
           GetComponentInChildren<Dissolve_Shader_Values>().intensity = 10;
           GetComponentInChildren<Dissolve_Shader_Values>().setColor();
           anim.Play("Death_Animation");
           Invoke("enqueueObject",.3f);
       }
   }

   private void enqueueObject()
   {
       ObjectPooler.Instance.enqueueGameObject(gameObject,"Enemy",true);
   }
}
