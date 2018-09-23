using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // se eu criar usar esse script em qualquer objeto, vai criar um riged body
public class tapController : MonoBehaviour {
public delegate void PlayerDelegate();
public static event PlayerDelegate OnPlayerDied;
public static event PlayerDelegate OnPlayerScored;
public AudioSource tapAudio;
public AudioSource scoreAudio;
public AudioSource dieAudio;
public Animator anim;
bool didFlap = false;

public float tapForce = 10f;
public float tiltSmoth = 5f;
public Vector3 startPosition;
Rigidbody2D rigedBody;
Quaternion downRotation; // É uma forma diferente de rotação mais segura
Quaternion fowardRotation;
gameManager game;
	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.Portrait;
		rigedBody = GetComponent<Rigidbody2D>();
		downRotation = Quaternion.Euler(0,0,-90); // estou pegando um vetor 3d e convertendo em quaternion para usar na rotação e esses numeros sao os angulos 
		fowardRotation = Quaternion.Euler(0,0,35);
		game = gameManager.Instance;
		rigedBody.simulated = false;
		anim = GetComponent<Animator>();
		anim.SetBool("isFlapping",didFlap);
		

		
	}
	void OnEnable(){
		gameManager.OnGameStarted += OnGameStarted;
		gameManager.OnGameOverConfirmed += OnGameOverConfirmed;

	}
	void OnDisable(){
		gameManager.OnGameStarted -= OnGameStarted;
		gameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
	}
	void OnGameStarted(){
		rigedBody.velocity = Vector3.zero;//se eu nao botar isso o player vai ficar com toda aquela gravidade acumulada
		rigedBody.simulated = true;

	}
	void OnGameOverConfirmed(){
		transform.localPosition = startPosition;// se eu estiver dentro de um parent é bom usar local
		transform.rotation = Quaternion.identity;


	}
	
	// Update is called once per frame
	void Update () {
		if (game.GameOver) return;
		if (Input.GetMouseButtonDown(0)){
			didFlap = true;
			anim.SetBool("isFlapping",didFlap);
			tapAudio.Play();
			transform.rotation = fowardRotation; //BONUS  : transform.rotation é um quaternium, nao é um vector3
			rigedBody.velocity = Vector3.zero;
			rigedBody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
		}
		transform.rotation = Quaternion.Lerp(transform.rotation,downRotation,tiltSmoth * Time.deltaTime);//lert vai de um source value até um target value em um certo tempo ( o tiltsmoth define o quao rápido vamos nos mover até certa ponto)
		
	}
	void OnTriggerEnter2D(Collider2D col){
		Debug.Log("ok");
		if (col.gameObject.tag == "scoreZone"){
			//TODO: Register a score event
			OnPlayerScored();//event sent to GameManager
			//TODO :play sound
			scoreAudio.Play();

		}
		if (col.gameObject.tag == "deadZone"){
			rigedBody.simulated = false;
			//TODO : Register a dead event
			OnPlayerDied();//event sent to GameManager
			//TODO : play sound
			dieAudio.Play();

		}

	}
}
