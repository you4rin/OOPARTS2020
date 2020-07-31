using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private bool tutorial1;
	private bool tutorial2;
	public GameObject menu;
	public GameObject mainObject;
	public GameObject stageTexts;
	public GameObject clearObjects;
	public GameObject gameOverObjects;
	public GridManager gridManager;
	public Button pauseButton;
	// Start is called before the first frame update
	void Start() {
		tutorial1 = tutorial2 = false;
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

	public void HideStageTexts() {
		// Hide Texts
		stageTexts.gameObject.SetActive(false);
	}

	public void ShowStageTexts() {
		// Show Texts
		stageTexts.gameObject.SetActive(true);
		mainObject.gameObject.SetActive(false);
	}

	public void LoadClearPage() {
		HideStageTexts();

	}

	public void RollBack1() {
		// Rollback to before the tutorial1
		tutorial1 = false;
	}
	
	public void RollBack2() {
		//Rollback to before the tutorial2
		tutorial2 = false;
	}

	public void PlayButtonOnClick() {
		if ( tutorial2 ) {
			MoveToStageSelect();
		}
		else if ( tutorial1 ) {
			tutorial2 = true;
			MoveToTutorial2();
		}
		else {
			tutorial1 = true;
			MoveToTutorial1();
		}
	}

	public void QuitButtonOnClick() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void MainMenuButtonOnClick() {
		if ( SceneManager.GetActiveScene().name == "Tutorial1" ) {
			RollBack1();
			gridManager.ResetGame();
		}
		if( SceneManager.GetActiveScene().name == "Tutorial2" ) {
			RollBack2();
			gridManager.ResetGame();
		}
		if ( gridManager.IsFinished() ) {
			gridManager.ResetGame();
			// if문에 게임오버 여부 판단도 필요
		}
		MoveToMain();
		ShowStageTexts();
		CloseMenu();
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
