using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellScript : MonoBehaviour {

	Color notAliveColor = new Color (0 / 255f, 147 / 255f, 198 / 255f);
	Color aliveColor = new Color (0 / 255f, 119 / 255f, 12 / 255f);
	Color goalCellColor = new Color (214/ 255f, 44/ 255f, 132/ 255f);
	public static GameManager test;

	//Each cell is either alive or not
	public bool alive = false;
	//This is used to 'cheaply' signal to the Update loop that we need to change the color
	//	i.e. if the previous state is different than the current state, we need to update the color
	bool previousAliveState = false;
	public bool newAliveState = false;
	public bool isGoalCell = false;

	public int x = -1;
	public int y = -1;

	Renderer rend;

	// Use this for initialization
	void Start () {
		rend = gameObject.GetComponent<Renderer> ();
		test = FindObjectOfType<GameManager> ();

		updateCellColor();
	}
	
	// Update is called once per frame
	void Update () {
		if (alive != previousAliveState) {
			//This means that something changed the cellType since the last update call, this means
			//	we need to set the color to the right color.
			updateCellColor ();
		} else if (isGoalCell) {
			updateCellColor ();
		} else {
			//This is where we update the 'previous cell type' so that we can automatically update the color
			previousAliveState = alive;
			updateCellColor ();
		}

		if (isGoalCell && alive){
			WinState ();
		}
	}

	void OnMouseDown() {
		if (!isGoalCell){
			alive = !alive;
		}else{
			
		}

	}



	void updateCellColor() {
		if (alive) {
			rend.material.color = aliveColor;
		} else if (isGoalCell) {
			rend.material.color = goalCellColor;
		} else {
			rend.material.color = notAliveColor;
		}
	}

	void WinState(){
		GameManager.score += GameManager.generationCount;
		test.scoreValue.text = "Score: " + GameManager.score; 
		GameManager.ResetGrid ();
		GameManager.simulate = false;
		test.simToggle.isOn = false;
	}
}
