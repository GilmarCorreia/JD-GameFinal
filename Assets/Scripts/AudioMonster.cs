using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMonster : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	public AudioClip audioStep;
	public AudioClip audioRoar;
	public AudioClip audioBite;
    public AudioClip audioAtq;
    public AudioClip audioDie;
	
	private AudioSource audio;
	
	/*void Start(){
		
		AudioSource audio;
		
	}*/
	
	void AudioStep(){
		
		audio.loop = false;
		audio.clip = audioStep;
		audio.Play();
		
	}

    void AudioRoar(){
		
		audio.loop = false;
		audio.clip = audioRoar;
		audio.Play();
		
	}
	
    void AudioBite(){
		
		audio.loop = false;
		audio.clip = audioBite;
		audio.Play();
		
	}
	
    void AudioAtq(){
		
		audio.loop = false;
		audio.clip = audioAtq;
		audio.Play();
		
	}
    	
    void AudioDie(){
		
		audio.loop = false;
		audio.clip = audioDie;
		audio.Play();
		
	}
	
	
	
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    //    
		
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}

/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MONSTER BEHAVIOUR
/// 
/// DETERMINA COMO O MONSTRO IR� REAGIR A ATAQUES E COMO IR� ATACAR
/// 
/// DEV: IVAN CORREIA LIMA COQUEIRO
/// DEV: WILLIAN TERUYA KIMURA
/// ULTIMA ATUALIZA��O: 12-08-2021
/// V. 1.0
/// </summary>

public class Monster : MonoBehaviour
{
    Animator monsterAnim;               //declara a MEF do Monstro
    public BoxCollider monsterArmR;
    public BoxCollider monsterArmL;
    private Rigidbody rbMonster;
    private Transform target;
    public AudioClip audioStep;
	public AudioClip audioRoar;
	public AudioClip audioBite;
    public AudioClip audioAtq;
    public AudioClip audioDie;

    AudioSource audio;
    AudioSource audio2;

    Vector3 v3;

    //determina a vida atual do monstro e a vida m�xima que ele pode ter
    public int maxHealth = 150;
    public int currentHealth;

    public HealthbarScript monsterHealth;

    // Inicio do jogo antes do primeiro update
    void Start()
    {
        //inicia o componente de anima��o no script
        monsterAnim = GetComponent<Animator>();
        monsterArmR = GetComponent<BoxCollider>();
        monsterArmL = GetComponent<BoxCollider>();
        rbMonster = GetComponent<Rigidbody>();
        //allAudioSources = FindObjectsOfType(AudioSource) as AudioSource[];
        //audio = allAudioSources[0];
        //audio2 = allAudioSources[1];


        //torna a vida da personagem em 100% no inicio da fase
        currentHealth = maxHealth;
        monsterHealth.SetMaxHealth(currentHealth);
    }

    internal void lookAtPlayer()
    {
        throw new NotImplementedException();
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
        //se a barra de espa�o for pressionada, o monstro sofrer� dano
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
            monsterHealth.SetHealth(currentHealth);
            monsterAnim.SetTrigger("isRoaring");
        }


        //se o boneco morrer ele cai no ch�o e n�o levanta mais
        if (currentHealth <= 30)
        {
            monsterAnim.SetTrigger("Dead");
        }


    }

    //fun��o que aciona o ataque simples
    public void BasicAttack()
    {
        monsterAnim.SetTrigger("basicAttackUsage");
    }

    //fun��o que aciona o ataque forte
    public void PowerAttack()
    {
        monsterAnim.SetTrigger("powerAttackUsage");
    }

    //fun��o que realiza trigger caso collide entre em contato

    void OnTriggerEnter(Collider attack)
    {
        if (attack.gameObject.tag == "Player")
        {
            Debug.Log(attack.gameObject.transform.position);
            //Vector3 interpolatedPosition = Vector3.
                //Lerp(target.position,(rbMonster.position-target.position), 0.05);
            attack.gameObject.transform.Translate(target.position.x, target.position.y, target.position.z+1);        // Move the object upward in world space 1 unit/second.
           // attack.gameObject.transform.Translate(Vector3.up * Time.deltaTime, Space.World);
        }
    }

    //define a velocidade do monstro conforme ele anda
    public void MonsterWalk(float blend)
    {
        //aqui ser� definida a seguinte l�gica;
        //1. se o player estiver entre 1m  a 2m de distancia (raycasting) o monstro corre e d� dano ao player
        
        //2. se o player estiver entre 0.5  a 1m de distancia (raycasting) o monstro anda at� ele 

        //3. se o player estiver grudado nele (0 a 0.5m) o monstro executa ou um powerAttack ou um basicAttack, definido pela vida do monstro
    }
    	
	void AudioStep(){
		
		GetComponent<AudioSource>().loop = false;
		GetComponent<AudioSource>().clip = audioStep;
		GetComponent<AudioSource>().Play();
		
	}

    void AudioRoar(){
		
		GetComponent<AudioSource>().loop = false;
		GetComponent<AudioSource>().clip = audioRoar;
		GetComponent<AudioSource>().Play();
		
	}
	
    void AudioBite(){
		
		GetComponent<AudioSource>().loop = false;
		GetComponent<AudioSource>().clip = audioBite;
		GetComponent<AudioSource>().Play();
		
	}
	
    void AudioAtq(){
		
		GetComponent<AudioSource>().loop = false;
		GetComponent<AudioSource>().clip = audioAtq;
		GetComponent<AudioSource>().Play();
		
	}
    	
    void AudioDie(){
		
		GetComponent<AudioSource>().loop = false;
		GetComponent<AudioSource>().clip = audioDie;
		GetComponent<AudioSource>().Play();
		
	}
}*/

