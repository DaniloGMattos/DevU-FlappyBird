using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour {
	public delegate void GameDelegate();// Isso vai permitir criar eventos em scripts para ser notificado
	public static event GameDelegate OnGameStarted;
	public static event GameDelegate OnGameOverConfirmed;
	public AudioSource pressAudio;

	public static gameManager Instance;
	public GameObject starPage;
	public GameObject gameOverPage;
	public GameObject countDownPage;
	public Text scoreText;

	enum PageState {
		None,
		Start,
		GameOver,
		CountDown

	}
	
	int scoreValue = 0;
	bool isGameOver = true;
	

	public bool GameOver {get {return isGameOver;}}
	void Start () {
	ConfirmGameOver();
	}
	void Awake(){
		Instance = this;
		
	}
	void OnEnable(){
		CountDownText.OnCountdownFinished += OnCountdownFinished;
		tapController.OnPlayerDied += OnPlayerDied;
		tapController.OnPlayerScored += OnPlayerScored;




	}
	void OnDisable(){
		CountDownText.OnCountdownFinished -= OnCountdownFinished;
		tapController.OnPlayerDied -= OnPlayerDied;
		tapController.OnPlayerScored -= OnPlayerScored;


	}
	void OnCountdownFinished(){
		SetPageState(PageState.None);
		OnGameStarted();//event sent to TapController
		scoreValue = 0 ;
		isGameOver = false;
	}
	void OnPlayerDied(){
		isGameOver = true;
		int savedScore = PlayerPrefs.GetInt("HighScore");// salva os pontos localment
		if (scoreValue > savedScore){
			PlayerPrefs.SetInt("HighScore",scoreValue);
		}
		SetPageState(PageState.GameOver);

	}
	void OnPlayerScored(){
		scoreValue++;
		scoreText.text = scoreValue.ToString();


	}
	void SetPageState(PageState state){
		switch(state){
			case PageState.None:

			starPage.SetActive(false);
			gameOverPage.SetActive(false);
			countDownPage.SetActive(false);

				break;
			case PageState.Start:

			starPage.SetActive(true);
			gameOverPage.SetActive(false);
			countDownPage.SetActive(false);

				break;
			case PageState.GameOver:

			starPage.SetActive(false);
			gameOverPage.SetActive(true);
			countDownPage.SetActive(false);

				break;
			case PageState.CountDown:

			starPage.SetActive(false);
			gameOverPage.SetActive(false);
			countDownPage.SetActive(true);

				break;
		}

	}
	public void ConfirmGameOver(){
		//activated when replay button is hit
		pressAudio.Play();
		OnGameOverConfirmed();//event sent to tapController
		scoreText.text = "0";
		SetPageState(PageState.Start);
	}
	public void StartGame(){
		//activated when play button is hit
		
		SetPageState(PageState.CountDown);
		pressAudio.Play();
	}
}
