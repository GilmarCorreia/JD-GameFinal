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

    Vector3 v3;

    //determina a vida atual do monstro e a vida máxima que ele pode ter
    public int maxHealth = 150;
    public int currentHealth;

    public HealthbarScript monsterHealth;

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
        currentHealth -= damage;
    }

    // Update chamado uma vez por frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.V))
            BasicAttack();
        if (Input.GetKeyDown(KeyCode.B))
            PowerAttack();
        //se a barra de espaço for pressionada, o monstro sofrerá dano
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
            monsterHealth.SetHealth(currentHealth);
            monsterAnim.SetTrigger("isRoaring");
        }


        //se o boneco morrer ele cai no chão e não levanta mais
        if (currentHealth <= 30)
        {
            monsterAnim.SetTrigger("Dead");
        }


    }

    //função que aciona o ataque simples
    public void BasicAttack()
    {
        monsterAnim.SetTrigger("basicAttackUsage");
    }

    //função que aciona o ataque forte
    public void PowerAttack()
    {
        monsterAnim.SetTrigger("powerAttackUsage");
    }

    //função que realiza trigger caso collide entre em contato

    void OnTriggerEnter(Collider attack)
    {
        if (attack.gameObject.tag == "Player")
        {
            Debug.Log(attack.gameObject.name);
            Vector3 moveDirection = monsterArmR.transform.position - rbMonster.transform.position;
            //rbMonster.AddForce(moveDirection.normalized * 500f);
            //healthbar.value -= 20;
            attack.gameObject.transform.Translate(Vector3.back * Time.deltaTime);        // Move the object upward in world space 1 unit/second.
            attack.gameObject.transform.Translate(Vector3.up * Time.deltaTime, Space.World);
            moveDirection = attack.gameObject.transform.position - rbMonster.transform.position;
            Debug.Log(attack.gameObject.name);
        }
    }

    //define a velocidade do monstro conforme ele anda
    public void MonsterWalk(float blend)
    {
        //aqui será definida a seguinte lógica;
        //1. se o player estiver entre 1m  a 2m de distancia (raycasting) o monstro corre e dá dano ao player
        
        //2. se o player estiver entre 0.5  a 1m de distancia (raycasting) o monstro anda até ele 

        //3. se o player estiver grudado nele (0 a 0.5m) o monstro executa ou um powerAttack ou um basicAttack, definido pela vida do monstro
    }
}
