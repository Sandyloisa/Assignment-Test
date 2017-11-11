using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {


	//to determine the dimension of board
	public int width;
	public int height;

	//to set the border of from the edge of tilegroup
	public float borderSize;

	public GameObject tilePrefab; //this will be a prefab to create one square of the board that will lay down one square and then duplicate with two loops
	//to make randomly filled pieces
	public GameObject[] gamePiecePrefabs;

	//to control the speed os switching or moving of gamepiece
	public float swapTime=0.5f;

	Tile[,] m_allTiles; //creating 2D array of tiles
	//keeping track of gamepieces
	GamePiece[,] m_allGamePieces;


	// Use this for initialization
	void Start () {
		
		// initialising 2D array
		m_allTiles = new Tile[width,height];

		//intialising array
		m_allGamePieces=new GamePiece[width,height];

		// to run the setup tile method
		SetupTiles();

		//to setup the camera properly
		SetupCamera();

		//to fill the board with random dots
		FillRandom();
	}
	
	//creating a method to set up tile grid
	void SetupTiles()
	{
		for (int i = 0; i < width; i++) { //for x-axis
			for (int j = 0; j < height; j++) { //for y-axis

				//instantiate the prefab of tile
				GameObject tile = Instantiate (tilePrefab, new Vector3 (i, j, 0), Quaternion.identity) as GameObject; // quaternion.identity because we dont need any rotation and as GameObject to cast it as a gameobject

				//rename the object when we instatiate it
				tile.name = "Tile(" + i + "," + j + ")";

				//to store the array in 2D array, we are grabbing the tile component
				m_allTiles [i, j] = tile.GetComponent<Tile> ();

				//to parent the tiles to the board
				tile.transform.parent = transform;

				//to call from tile class
				m_allTiles[i,j].Init(i,j,this);

			}
		}
	}

	//to setup camera for better placement of grid group
	void SetupCamera()
	{
		//to center the camera
		Camera.main.transform.position=new Vector3((float)(width-1)/2f, (float)(height-1)/2f,-10f);

		//for calculating the orthographic size
		float aspectRatio=(float)Screen.width/(float)Screen.height;

		float verticalSize = (float)height / 2f + (float)borderSize;
		float horizontalSize = ((float)width / 2f + (float)borderSize) / aspectRatio;

		//to take larger of two numbers
		Camera.main.orthographicSize=(verticalSize>horizontalSize)?verticalSize:horizontalSize;

	}

	//returns a random piece from array of prefabs
	GameObject GetRandomPiece()
	{

		int randomIdx = Random.Range (0,gamePiecePrefabs.Length); //choosing a random in a range of total number of prefabs

		return gamePiecePrefabs [randomIdx]; //return the random object
	}

	// to keep the movement within the scope of the array
	bool IsWithinBounds(int x, int y)
	{
		return (x >= 0 && x < width && y >= 0 && y < height);
	}

	//place the given gamepiece at specified location of the board
	public void PlaceGamePiece(GamePiece gamePiece, int x, int y)
	{

		gamePiece.transform.position = new Vector3 (x, y, 0); // set the position
		gamePiece.transform.rotation = Quaternion.identity; //for no rotation
		if(IsWithinBounds(x,y))
		{
			m_allGamePieces [x, y] = gamePiece;// update the correct element of m_allGamePieces array
		}
		//calling the SetCoord method from GamePiece class
		gamePiece.SetCoord(x,y);
	}



	//to fill the whole board randomly
	void FillRandom()
	{

		for (int i = 0; i < width; i++) { //for x-axis
			for (int j = 0; j < height; j++) { //for y-axis

				//to instantiate random gameobject that we get from GetRandomPiece method
				GameObject randomPiece=Instantiate(GetRandomPiece(), Vector3.zero, Quaternion.identity) as GameObject;

				//now actually placing the created randompiece
				if (randomPiece != null) {
					randomPiece.GetComponent<GamePiece> ().Init (this);
					PlaceGamePiece (randomPiece.GetComponent<GamePiece> (), i, j);
					randomPiece.transform.parent = transform; //this is done to keep the hierarchy clean and clear by putting it inside the board object
				}
			}
		}

	}

	//variables to make the movements using mouse
	Tile m_clickedTile;
	Tile m_targetTile;

	//to allow clicking to happen
	public void ClickTile(Tile tile)
	{
		if (m_clickedTile == null) {
			m_clickedTile = tile; //m_clickedtile will be equal to the tile object that we pass in this method only when no other tile is clicked
		}
	}

	// to allow dragging to happen
	public void DragToTile(Tile tile)
	{
		if (m_clickedTile != null) { //checks if there is valid clicked tile, then target is set. This is taken from previous method of clicktile
			m_targetTile = tile; 
		}
	}

	//when mouse button is released
	public void ReleaseTile()
	{
		if (m_clickedTile != null && m_targetTile != null) { //check to see if there are two valid tiles, switching will take place
			SwitchTiles(m_clickedTile, m_targetTile);
		}

		//when mouse button is released, its going to reset
		m_clickedTile = null;
		m_targetTile = null;
	}

	//actual swapping to take place
	void SwitchTiles(Tile clickedTile, Tile targetTile) //taking two tiles as arguments
	{
		GamePiece clickedPiece = m_allGamePieces [clickedTile.xIndex, clickedTile.yIndex]; //getting the clicked gamepiece
		GamePiece targetPiece = m_allGamePieces [targetTile.xIndex, targetTile.yIndex]; //getting the target gamepiece

		//now to actually move it between the clicked and target gamepieces
		clickedPiece.Move(targetTile.xIndex, targetTile.yIndex, swapTime); //this will move the clicked gamepiece to the target gamepiece position
		targetPiece.Move(clickedTile.xIndex, clickedTile.yIndex, swapTime); //this will moved the target gamepiece to the clicked gamepiece position


	}
}
