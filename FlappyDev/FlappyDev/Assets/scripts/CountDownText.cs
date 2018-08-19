using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CountDownText : MonoBehaviour {
	public delegate void CountdownFinished();
	public static event CountdownFinished OnCountdownFinished;

	Text countDown;

	void OnEnable(){
		countDown = GetComponent<Text>();
		countDown.text = "3";
		StartCoroutine("Countdown");
	}

	IEnumerator Countdown() {//espera um certo tempo ate fazer algo
		int count = 3;
		for (int i = 0; i < count; i++){
			countDown.text = (count - i).ToString();
			yield return new WaitForSeconds(1);
		}

		OnCountdownFinished();

	}

}
