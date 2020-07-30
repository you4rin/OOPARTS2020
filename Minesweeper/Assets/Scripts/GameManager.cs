using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private bool firstPlay;
	public GameObject menu;
	public GameObject mainObject;
	public Button pauseButton;
	// Start is called before the first frame update
	void Start() {
		firstPlay = true;
	}

	// Update is called once per frame
	void Update() {

	}

	public void LoadMenu() {
		// Activate Menu object
		pauseButton.gameObject.SetActive(false);
		mainObject.gameObject.SetActive(false);
		menu.gameObject.SetActive(true);
	}

	public void CloseMenu() {
		// Deactivate Menu object
		menu.gameObject.SetActive(false);
		mainObject.gameObject.SetActive(true);
		pauseButton.gameObject.SetActive(true);
	}

	public void PlayButtonOnClick() {
		if ( firstPlay ) {
			firstPlay = false;
			MoveToTutorial1();
		}
		else {
			MoveToStageSelect();
		}
	}

	public void QuitButtonOnClick() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	//Scene transitions
	public void MoveToMain() {
		SceneManager.LoadScene("Main");
	}
	
	public void MoveToStageSelect() {
		SceneManager.LoadScene("StageSelect");
	}

	public void MoveToTutorial1() {
		SceneManager.LoadScene("Tutorial1");
	}

	public void MoveToTutorial2() {
		SceneManager.LoadScene("Tutorial2");
	}

	public void MoveToStage1() {
		SceneManager.LoadScene("Stage1");
	}

	public void MoveToStage2() {
		SceneManager.LoadScene("Stage2");
	}

	public void MoveToStage3() {
		SceneManager.LoadScene("Stage3");
	}
}
