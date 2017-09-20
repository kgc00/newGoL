using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	//This will store all the cells. Note that because it is 'static' we can refer to it from other
	//	scripts without needing to call "GameObject.Find" and such.
	public static CellScript[,] grid;

	public static int numCellsW = 30;
	public static int numCellsH = 30;

	float cellWidth = 1f;
	float cellHeight = 1f;
	float cellSpacing = 0.1f;

	float simulationTimer = 0;
	float simulationRate = 0.2f;

	public static bool simulate = false;
	public static int generationCount = 0;
	public static int score = 0;
	bool noVisuals = true;
	float sinIntensity = 0f;

	public GameObject pe;
	GameObject go;
	public Text scoreValue;
	public Text attemptText;
	bool areAnyAlive = false;
	public Toggle simToggle;

	// Use this for initialization
	void Start () {
		//Instantiate the empty two dimesnional array
		grid = new CellScript[GameManager.numCellsW, GameManager.numCellsH];
		scoreValue.text = "Score: " + score;

		//Using 'nested for loops', instantiate cubes with cell scripts in a way such that
		//	each cell will be places in a top left oriented coodinate system.
		//	I.e. the top left cell will have the x, y coordinates of (0,0), and the bottom right will
		//	have the coodinate (numCellsW-1, numCellsH-1)
		for (int y = 0; y < GameManager.numCellsH; y++) {
			for (int x = 0; x < GameManager.numCellsW; x++) {
				if (x == 2 && y == 28) {
					GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Sphere);
						CellScript cellScript = cube.AddComponent<CellScript> ();
						cellScript.x = x;
						cellScript.y = y;
						GameManager.grid [x, y] = cellScript;
						//Create the position of the cube that represents the cell, based on the cell's x,y coordinate
						//	Note that by making the x,y positions be something like "x * (cellWidth + cellSpacing)" we 
						//	can have arbitrarily sized cells with spacing.
						Vector3 pos = new Vector3 (x * (cellWidth + cellSpacing), y * (cellHeight + cellSpacing), 0);
						cube.transform.position = pos;
					cellScript.isGoalCell = true;
					cellScript.alive = false;
				} else {
					GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
					CellScript cellScript = cube.AddComponent<CellScript> ();
					cellScript.x = x;
					cellScript.y = y;
					GameManager.grid [x, y] = cellScript;
					//Create the position of the cube that represents the cell, based on the cell's x,y coordinate
					//	Note that by making the x,y positions be something like "x * (cellWidth + cellSpacing)" we 
					//	can have arbitrarily sized cells with spacing.
					Vector3 pos = new Vector3 (x * (cellWidth + cellSpacing), y * (cellHeight + cellSpacing), 0);
					cube.transform.position = pos;
				}
			}
		}
	}

	void Update() {
//		if (Input.GetKey(KeyCode.Space)){Visuals ();}
		//Debug.Log (sinIntensity);
		if (generationCount > 40 && noVisuals){Visuals();noVisuals=false;}
		if (generationCount == 79 || generationCount == 119 || generationCount == 200) {noVisuals = true;CameraScript.camStuff = true;}
		if (GameManager.simulate) {
			simulationTimer -= Time.deltaTime;
			if (simulationTimer < 0) {
				simulationTimer = simulationRate;
					generationCount++;
					attemptText.text = "This Run: " + generationCount;

//				if (Random.Range(0, 1000) > 950){
//				Visuals ();
//				}
				//Update grid based on sweet game of life rules
				for (int y = 0; y < GameManager.numCellsH; y++) {
					for (int x = 0; x < GameManager.numCellsW; x++) {
						//GameManager.grid[x, y]
						List<CellScript> neighbors = GameManager.getNeighbors (x, y);
						List<CellScript> liveNeighbors = new List<CellScript> ();
						foreach (CellScript neighbor in neighbors) {
							if (neighbor.alive) {
								liveNeighbors.Add (neighbor);
							}
						}
						GameManager.grid [x, y].newAliveState = GameManager.grid [x, y].alive;

					
						//Apply the 4 rules from Conway's Game of Life (https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life)
						//1. Any live cell with fewer than two live neighbours dies, as if caused by underpopulation.
						if (GameManager.grid [x, y].alive && liveNeighbors.Count < 2) {
							GameManager.grid [x, y].newAliveState = false;
						}
						//2. Any live cell with two or three live neighbours lives on to the next generation.
						else if (GameManager.grid [x, y].alive && (liveNeighbors.Count == 2 || liveNeighbors.Count == 3)) {
							GameManager.grid [x, y].newAliveState = true;
						}
						//3. Any live cell with more than three live neighbours dies, as if by overpopulation.
						else if (GameManager.grid [x, y].alive && liveNeighbors.Count > 3) {
							GameManager.grid [x, y].newAliveState = false;
						}
						//4. Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
						else if (!GameManager.grid [x, y].alive && liveNeighbors.Count == 3) {
							GameManager.grid [x, y].newAliveState = true;
						}
					}
				}
				//Now that we have looped through all of the cells in the grid, and stored what their alive status should
				//	be in each cell's newAliveState variable, update each cell's alive state to be that value.
				for (int y = 0; y < GameManager.numCellsH; y++) {
					for (int x = 0; x < GameManager.numCellsW; x++) {
						GameManager.grid [x, y].alive = GameManager.grid [x, y].newAliveState;
						if (GameManager.grid [x, y].newAliveState) {
							CellScript newAliveCell = GameManager.grid [x, y];
						}
					}
				}
			}
		}
	}

	public static List<CellScript> getNeighbors(int x, int y) {
		List<CellScript> neighbors = new List<CellScript> ();

		//Collect all the cells in surrounding 8 cells of the cells at grid[x,y]
		for (int i = Mathf.Max(0, x - 1); i <= Mathf.Min(GameManager.numCellsW - 1, x + 1); i++) {
			for (int j = Mathf.Max(0, y - 1); j <= Mathf.Min(GameManager.numCellsH - 1, y + 1); j++) {
				if ((i == x && j == y) == false) {
					neighbors.Add (GameManager.grid [i, j]);
				}
			}
		}

		return neighbors;
	}

	//This function is called by the UI toggle's event system (look at the Toggle child of the Canvas)
	public void toggleSimulate(bool value) {
		GameManager.simulate = value;
		generationCount = 0;
	}

	public static void ResetGrid(){
		foreach(CellScript cell in grid){
			cell.alive = false;
			cell.newAliveState = false;
			generationCount = 0;
		}
	}

	void Visuals(){
		int randomH = Random.Range (0, numCellsH);
		int randomW = Random.Range (0, numCellsW);
		foreach(CellScript cell in grid){
			if (cell.x == randomH && cell.y == randomW) {
				sinIntensity = Mathf.Sin (Random.Range(0,10));
				go = Instantiate (pe, cell.transform.position, cell.transform.rotation);
				go.transform.position -= new Vector3 (0,0, 10);
				go.GetComponent<Light> ().intensity = sinIntensity;
			}
		}
	}
}