using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MONSTER BEHAVIOUR
/// 
/// DETERMINA COMO O MONSTRO IRÁ REAGIR A ATAQUES E COMO IRÁ ATACAR
/// 
/// DEV: IVAN CORREIA LIMA COQUEIRO
/// DEV: WILLIAN TERUYA KIMURA
/// ULTIMA ATUALIZAÇÃO: 12-08-2021
/// V. 1.0
/// </summary>

public class Monster : MonoBehaviour
{
 
    Animator monsterAnim;               //declara a MEF do Monstro
    public BoxCollider monsterArmR;
    public BoxCollider monsterArmL;
    private Rigidbody rbMonster;
    //  private Transform target;
    private Transform pivotTransform;
    public GameObject playerTarget;
    Vector3 v3;
    //public Vector3 target;
  //  public Vector3 monsterPos;
    public Transform positions;
    public float speed;
    public float dist;
    //determina a vida atual do monstro e a vida máxima que ele pode ter
    public int maxHealth = 150;
    public int currentHealth;
    public bool isInvulnerable = false;
    public HealthbarScript monsterHealth;
    public bool isAlive = true;
    public bool isAttacking = false;
    public GameObject HealthBarViewer;
    // Inicio do jogo antes do primeiro update
    void Start()
    {
        //inicia o componente de animação no script
        monsterAnim = GetComponent<Animator>();
        monsterArmR = GetComponent<BoxCollider>();
        monsterArmL = GetComponent<BoxCollider>();
        rbMonster = GetComponent<Rigidbody>();

        //torna a vida da personagem em 100% no inicio da fase
        currentHealth = maxHealth;
        monsterHealth.SetMaxHealth(currentHealth);
    }


    //adiciona a variavel dano ao monstro
    void TakeDamage(int damage)
    {
        if(isInvulnerable == true)
            return;

        currentHealth -= damage;
        monsterHealth.SetHealth(currentHealth);

        if (currentHealth == 70 || currentHealth == 40)
            monsterAnim.SetTrigger("isRoaring");

        if(currentHealth <= 30)
        {
            monsterAnim.SetTrigger("Dead");
            isAlive = false;
            speed = 0;
            HealthBarViewer.SetActive(false);
        }
    }

    // Update chamado uma vez por frame
    void Update()
    {
        speed = 2.5f;
        HealthBarViewer.SetActive(true);
             if (isAlive == false)
             {
                return;
             }

        isInvulnerable = false;
        //funções de debug
        if (Input.GetKeyDown(KeyCode.K))
            TakeDamage(10);

        
        pivotTransform = playerTarget.transform;
        RunTowards();

        var newRotation = Quaternion.LookRotation(playerTarget.transform.position - transform.position, Vector3.forward);
            newRotation.z = 0.0f;
            newRotation.x = 0.0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 8);

    }

    //função que aciona o ataque simples
    public void BasicAttack()
    {
        monsterAnim.SetTrigger("basicAttackUsage");
    }
    
    //função que faz o monstro correr atrás do player
    public void RunTowards()
    {
        Vector3 target = new Vector3(playerTarget.transform.position.x, playerTarget.transform.position.y, playerTarget.transform.position.z);
        Vector3 monsterPos = new Vector3(rbMonster.position.x, 0, rbMonster.position.z);


        dist = Vector3.Distance(target, monsterPos);
        Debug.Log(dist);

        if (dist >= 50)
        {
            monsterAnim.SetTrigger("idle");
            currentHealth = maxHealth;
            monsterHealth.SetMaxHealth(currentHealth);
            HealthBarViewer.SetActive(false);
        }
        else if (dist < 4.0f)
        {
            BasicAttack();
        }
        else
        { 
            monsterAnim.SetTrigger("Walk");
            transform.position = Vector3.MoveTowards(monsterPos, target, speed * Time.fixedDeltaTime);
        }

    }

    //função que realiza trigger caso collide entre em contato
    void OnTriggerEnter(Collider attack)
    {
        if (isAlive == false)
        {
            return;
        }

        if (attack.gameObject.tag == "Player")
        {
            Debug.Log(attack.gameObject.transform.position);
            attack.gameObject.transform.Translate(pivotTransform.position.x, pivotTransform.position.y, pivotTransform.position.z);        // Move the object upward in world space 1 unit/second.
           // attack.gameObject.transform.Translate(Vector3.back * Time.deltaTime, Space.World);
        }
    
    }   

   
}
