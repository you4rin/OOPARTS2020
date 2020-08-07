using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private GridManager.GameState state;
	private bool finished;
	private float time;
	public GameObject menu;
	public GameObject mainObject;
	public GameObject stageTexts;
	public GameObject clearObjects;
	public GameObject gameOverObjects;
	public GridManager gridManager;
	public Button pauseButton;

	// Start is called before the first frame update
	void Start() {
		
	}

	// Update is called once per frame
	void Update() {
		if ( finished ) {
			time += Time.deltaTime;
		}
		if ( SceneManager.GetActiveScene().name != "Main" && SceneManager.GetActiveScene().name != "StageSelect" ) {
			if ( state!=gridManager.state ) {
				if ( gridManager.state != GridManager.GameState.gaming ) {
					pauseButton.gameObject.SetActive(false);
					finished = true;
					time = 0;
				}
				else {
					pauseButton.gameObject.SetActive(true);
				}
			}
			state = gridManager.state;
		}
		if ( time > 2 ) {
			if( gridManager.state == GridManager.GameState.cleared ) {
				LoadClearPage();
			}
			else if( gridManager.state == GridManager.GameState.failed ) {
				LoadGameOverPage();
			}
		}
	}

	public void SetObjectsActive(bool state) {
		pauseButton.gameObject.SetActive(state);
		mainObject.gameObject.SetActive(state);
		stageTexts.gameObject.SetActive(state);
	}

	void LoadMenu() {
		// Activate Menu object
		SetObjectsActive(false);
		menu.gameObject.SetActive(true);
	}

	void CloseMenu() {
		// Deactivate Menu object
		menu.gameObject.SetActive(false);
		SetObjectsActive(true);
	}

	void LoadClearPage() {
		SetObjectsActive(false);
		clearObjects.gameObject.SetActive(true);
	}

	void LoadGameOverPage() {
		SetObjectsActive(false);
		gameOverObjects.gameObject.SetActive(true);
	}

	//Button Clicks
	public void QuitButtonOnClick() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void MainMenuButtonOnClick() {
		MoveToStageSelect();
	}

	public void NextStageButtonOnClick() {
		gridManager.ResetGame();
		time = 0;
		if ( SceneManager.GetActiveScene().name == "Tutorial1" ) {
			MoveToTutorial2();
		}
		else {
			MoveToStageSelect();
		}
	}

	public void RestartButtonOnClick() {
		time = 0;
		gridManager.ResetGame();
		gameOverObjects.gameObject.SetActive(false);
		SetObjectsActive(true);
	}

	public void ResumeButtonOnClick() {
		CloseMenu();
	}

	public void PauseButtonOnClick() {
		LoadMenu();
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
