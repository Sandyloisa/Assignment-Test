using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoardShuffler))] // this is to let the exchange between boardshuffler and this script to take place

public class Board : MonoBehaviour {

	
	//to determine the dimension of board
	int width;
	int height;



	int minWidth=2;
	int maxWidth=10;
	int minHeight=2;
	int maxHeight=10;




	//to set the border of from the edge of tilegroup
	public float borderSize;

	public GameObject tilePrefab; //this will be a prefab to create one square of the board that will lay down one square and then duplicate with two loops
	//to make randomly filled pieces
	public GameObject[] gamePiecePrefabs;

	//to control the speed os switching or moving of gamepiece
	float swapTime=0.5f;

	Tile[,] m_allTiles; //creating 2D array of tiles
	//keeping track of gamepieces
	GamePiece[,] m_allGamePieces;

	//to reference the boardshuffler component
	BoardShuffler m_boardShuffler;





	// Use this for initialization
	public void Start () {

		//width=5;
		//height=6;
		width=height=10;

		//width= Random.Range(minWidth, maxWidth+1);
	//	height=Random.Range(minHeight, maxHeight+1);

		// initialising 2D array
		m_allTiles = new Tile[maxWidth,maxHeight];

		//intialising array
		m_allGamePieces=new GamePiece[maxWidth,maxHeight];

		// to run the setup tile method
		SetupTiles();

		//to setup the camera properly
		SetupCamera();

		//to fill the board with random dots
		FillRandom();

		//to assign boardshuffling
		m_boardShuffler = GetComponent<BoardShuffler>();


	}

	//this is for reducing the size
	public void SlideChange (float someValue)
	{
		
		if (width > someValue) {
			width =(int)someValue;

			ShuffleBoard ();

			GameObject[] destroyingItems = GameObject.FindGameObjectsWithTag ("Respawn");
			foreach (GameObject destoryingItem in destroyingItems) {
				GameObject.Destroy (destoryingItem);
				//destoryingItem.transform.position=new Vector3(-100,-100,-10f);
			}

			// initialising 2D array
			m_allTiles = new Tile[width, height];

			//intialising array
			m_allGamePieces = new GamePiece[width, height];

			// to run the setup tile method
			SetupTiles ();

			//to setup the camera properly
			SetupCamera ();

			//to fill the board with random dots
			FillRandom ();

			//Application.LoadLevel(Application.loadedLevel);
		} else {
			GameObject[] destroyingItems = GameObject.FindGameObjectsWithTag ("Respawn");
			foreach (GameObject destoryingItem in destroyingItems) {
				GameObject.Destroy (destoryingItem);
				//destoryingItem.transform.position=new Vector3(-100,-100,-10f);
			}
			width=(int)someValue;
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

		//to assign boardshuffling
		m_boardShuffler = GetComponent<BoardShuffler>();
			
		}

	}

	public void SlideChange2 (float someValue)
	{
		
		if (height > someValue) {
			height = (int)someValue;

			ShuffleBoard ();

			GameObject[] destroyingItems = GameObject.FindGameObjectsWithTag ("Respawn");
			foreach (GameObject destoryingItem in destroyingItems) {
				GameObject.Destroy (destoryingItem);
				//destoryingItem.transform.position=new Vector3(-100,-100,-10f);
			}

			// initialising 2D array
			m_allTiles = new Tile[width, height];

			//intialising array
			m_allGamePieces = new GamePiece[width, height];

			// to run the setup tile method
			SetupTiles ();

			//to setup the camera properly
			SetupCamera ();

			//to fill the board with random dots
			FillRandom ();

			//Application.LoadLevel(Application.loadedLevel);
		} else {
			GameObject[] destroyingItems = GameObject.FindGameObjectsWithTag ("Respawn");
			foreach (GameObject destoryingItem in destroyingItems) {
				GameObject.Destroy (destoryingItem);
				//destoryingItem.transform.position=new Vector3(-100,-100,-10f);
			}
			height=(int)someValue;
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

		//to assign boardshuffling
		m_boardShuffler = GetComponent<BoardShuffler>();
			
		}

	}

	void ClearPieceAt (int x, int y)
	{
		GamePiece pieceToClear = m_allGamePieces [x, y];

		if (pieceToClear != null) {
			m_allGamePieces [x, y] = null;
			Destroy (pieceToClear.gameObject);
		}
	}




	void ClearBoard()
	{
		for(int i=0; i<width; i++)
		{
			for(int j=0; j<height; j++)
			{
				ClearPieceAt(i,j);
			}
		}
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

				//putting a tag for safety
				//tile.gameObject.tag="Respawn";

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

	GameObject GetRandomGamePiece()
    {
        int randomIdx = Random.Range(0, gamePiecePrefabs.Length);

        return gamePiecePrefabs[randomIdx];
    }

	GamePiece FillRandomAt(int x, int y, int falseYOffset = 0, float moveTime = 0.1f)
    {
        GameObject randomPiece = Instantiate(GetRandomGamePiece(), Vector3.zero, Quaternion.identity) as GameObject;

        if (randomPiece != null)
        {
            randomPiece.GetComponent<GamePiece>().Init(this);
            PlaceGamePiece(randomPiece.GetComponent<GamePiece>(), x, y);

            if (falseYOffset != 0)
            {
                randomPiece.transform.position = new Vector3(x, y + falseYOffset, 0);
                randomPiece.GetComponent<GamePiece>().Move(x, y, moveTime);
            }


            randomPiece.transform.parent = transform;
            return randomPiece.GetComponent<GamePiece>();
        }
        return null;
    }






	//variables to make the movements using mouse
	Tile m_clickedTile;
	Tile m_targetTile;

	//to allow clicking to happen
	public void ClickTile (Tile tile)//mouseDown
	{
		for (int i = 0; i < width; i++) {
				for (int j = 0; j < height; j++) {

					SpriteRenderer spriteRenderer = m_allTiles [i, j].GetComponent<SpriteRenderer> ();
					spriteRenderer.color = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);

				}
			}
		
		//Debug.Log(m_clickedTile+","+m_targetTile);
		if (m_clickedTile == null) {
			m_clickedTile = tile; //m_clickedtile will be equal to the tile object that we pass in this method only when no other tile is clicked
			//Debug.Log ("CLICKED: " + m_clickedTile);
			SpriteRenderer spriteRenderer = m_allTiles [m_clickedTile.xIndex, m_clickedTile.yIndex].GetComponent<SpriteRenderer> ();
			spriteRenderer.color = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
		} 

	}

	// to allow dragging to happen
	public void DragToTile (Tile tile)//mouseEnter
	{
		//Debug.Log("Clicked: "+m_clickedTile+" Target: "+m_targetTile);
		if (m_clickedTile != null) { //checks if there is valid clicked tile, then target is set. This is taken from previous method of clicktile
			m_targetTile = tile;
		//	SwitchTiles (m_clickedTile, m_targetTile);
			//m_clickedTile = m_targetTile;

			//m_targetTile = null;
			SpriteRenderer spriteRenderer = m_allTiles [m_targetTile.xIndex, m_targetTile.yIndex].GetComponent<SpriteRenderer> ();
			spriteRenderer.color = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);


		}



	}

	public void ReplaceTile (Tile tile)
	{
		//Debug.Log("Replacing");
	}



	//when mouse button is released
	public void ReleaseTile()//mouseUp
	{
		//Debug.Log(m_clickedTile+","+m_targetTile);
		if (m_clickedTile != null && m_targetTile != null) { //check to see if there are two valid tiles, switching will take place
			SwitchTiles(m_clickedTile, m_targetTile);
			//Debug.Log("TARGETED: "+m_targetTile);
			SpriteRenderer spriteRenderer = m_allTiles [m_targetTile.xIndex, m_targetTile.yIndex].GetComponent<SpriteRenderer> ();
			spriteRenderer.color = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
		}

		//when mouse button is released, its going to reset
		m_clickedTile = null;
		m_targetTile = null;


	}

	void Update ()
	{	
		
		if (m_clickedTile == null && m_targetTile == null) {
			
			/*for (int i = 0; i < width; i++) {
				for (int j = 0; j < height; j++) {

					SpriteRenderer spriteRenderer = m_allTiles [i, j].GetComponent<SpriteRenderer> ();
					spriteRenderer.color = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);

				}
			}*/
		}

	}






	public int readjusting=1;

	public void Toggle_Changed (bool someValue)
	{
		if (someValue == true) {
			readjusting = 1;
		} else {
			readjusting=0;
		}
	}
	//actual swapping to take place
	public void SwitchTiles (Tile clickedTile, Tile targetTile) //taking two tiles as arguments
	{
		GamePiece clickedPiece = m_allGamePieces [clickedTile.xIndex, clickedTile.yIndex]; //getting the clicked gamepiece
		GamePiece targetPiece = m_allGamePieces [targetTile.xIndex, targetTile.yIndex]; //getting the target gamepiece
		//Debug.Log(clickedTile+" ,"+targetTile);

		if (readjusting == 0) {//swapping feature
			//now to actually move it between the clicked and target gamepieces
			clickedPiece.Move (targetTile.xIndex, targetTile.yIndex, swapTime); //this will move the clicked gamepiece to the target gamepiece position
			targetPiece.Move (clickedTile.xIndex, clickedTile.yIndex, swapTime); //this will moved the target gamepiece to the clicked gamepiece position

		} else { //readjusting feature

			/* ******* This is for the readjustment feature ******* */
			clickedPiece.Move (targetTile.xIndex, targetTile.yIndex, swapTime);


			int DifferenceInX2 = Mathf.Abs (clickedTile.xIndex - targetTile.xIndex);
			int DifferenceInY2 = Mathf.Abs (clickedTile.yIndex - targetTile.yIndex);
			//Debug.Log (width - DifferenceInX2 + "," + DifferenceInY2);



			//variable creating to hold the condition of right or left movement, right +1 and left -1
			int movement = 0;
			//check whether right movement or left movement will happen
			if ((clickedTile.yIndex == targetTile.yIndex && clickedTile.xIndex > targetTile.xIndex) || (clickedTile.xIndex > targetTile.xIndex && clickedTile.yIndex < targetTile.yIndex) || (clickedTile.xIndex == targetTile.xIndex && clickedTile.yIndex < targetTile.yIndex) || (clickedTile.xIndex < targetTile.xIndex && clickedTile.yIndex < targetTile.yIndex)) {
			
				movement = 1;
			} else {
			
				movement = -1;
			}
			//Debug.Log (movement);



			//this loop takes care of the condition that if it is exchanged with the last element in x-axis or y-axis, it will change the position according to the movement
			for (int j = 0; j < DifferenceInY2; j++) {
			
				if (targetTile.xIndex == width - 1 && movement == 1) { // if the movement is happening right and the target tile is at the right most column, then change the column to first one and bring it one row down


					targetPiece.Move (0, targetTile.yIndex - 1, swapTime);


				} else if (targetTile.xIndex == width - 1 && movement == -1) { //if the movement happening left and the target tile is at the right most column, then bring the target tile to left of its original position without changing its row


					targetPiece.Move (targetTile.xIndex - 1, targetTile.yIndex, swapTime);


				} else if (targetTile.xIndex == 0 && movement == -1) { //if the movement is happening left and the target tile is at the left most column, then bring the target tile to right most column and make it go up one row
					//if (targetTile.xIndex == clickedTile.xIndex + 1) {


					targetPiece.Move (width - 1, targetTile.yIndex + 1, swapTime);


					//}
				} else if (targetTile.xIndex == 0 && movement == 1) { // if the movement is happening right and the target tile is at the left most column, then bring the target tile to one column right keeping it in the same row
					//if (targetTile.xIndex == clickedTile.xIndex + 1) {


					targetPiece.Move (targetTile.xIndex + 1, targetTile.yIndex, swapTime);


					//}
				}

			}




			//to count the number of elements between the target and clicked gameobject
			int count1 = 0; // for storing the number of row elements
			int count2 = 0; // for storing the number of elements column-wise excluding the ones already stored in count1




			for (int j = 0; j < (DifferenceInY2 - 1) * width; j++) {
				count1++;
			}
			for (int i = 0; i < (width - DifferenceInX2); i++) {
				count2++;
			}
			int totalCount = count1 + count2;//storing the number of elements between the target and clicked tile that needs to readjusts spaces
			//Debug.Log ("Count: " + totalCount);




			if (targetTile.xIndex != (width - 1) && movement == 1) { //checking whether the movement is happening right and if the targettile is not the extreme right tile
				targetPiece.Move (targetTile.xIndex + 1, targetTile.yIndex, swapTime);
			}
			if (targetTile.xIndex != (width - 1) && movement == -1) {
				targetPiece.Move (targetTile.xIndex - 1, targetTile.yIndex, swapTime);
			}

//		for (int j = 0; j < (DifferenceInY2 - 1) * width; j++) {
//			if (movement == 1) {// if its a right movement
//				GamePiece remainingPiece = m_allGamePieces [targetTile.xIndex+j, targetTile.yIndex];
//			}
//		}
			//Debug.Log ("Clicked Position: " + clickedTile.xIndex + "," + clickedTile.yIndex);
			//Debug.Log ("Target Position: " + targetTile.xIndex + "," + targetTile.yIndex);

			//to hold the position of all the elements between clicked and target tiles
			if (movement == 1) {

				for (int m = clickedTile.yIndex + 1; m < targetTile.yIndex; m++) {
					//Debug.Log("Rows no. in between: "+m);
					for (int i = 0; i < width; i++) {
						GamePiece remainingPiece = m_allGamePieces [i, m];

						if (remainingPiece.xIndex == width - 1) {
							remainingPiece.Move (0, remainingPiece.yIndex - 1, swapTime);
						} else {
							remainingPiece.Move (remainingPiece.xIndex + 1, remainingPiece.yIndex, swapTime);
						}
						//Debug.Log ("Inbetweeners: " + remainingPiece);
						//Debug.Log("Inbetweeners: "+i+","+m);
					}
				}

				if (clickedTile.yIndex != targetTile.yIndex) {
					for (int i = clickedTile.xIndex; i >= 0; i--) {
						//for (int k = clickedTile.yIndex; k < targetTile.yIndex; k++) {
						GamePiece remainingPiece = m_allGamePieces [i, clickedTile.yIndex];
						if (remainingPiece.xIndex == width - 1) {
							remainingPiece.Move (0, remainingPiece.yIndex - 1, swapTime);
						} else {
							remainingPiece.Move (remainingPiece.xIndex + 1, remainingPiece.yIndex, swapTime);
						}
						//Debug.Log ("Clicked row: " + i + "," + clickedTile.yIndex);
						//}
					}

					for (int j = targetTile.xIndex; j < width; j++) {
						//for (int l = targetTile.yIndex; l > clickedTile.yIndex; l--) {
						GamePiece remainingPiece = m_allGamePieces [j, targetTile.yIndex];
						if (remainingPiece.xIndex == width - 1) {
							remainingPiece.Move (0, remainingPiece.yIndex - 1, swapTime);
						} else {
							remainingPiece.Move (remainingPiece.xIndex + 1, remainingPiece.yIndex, swapTime);
						}
						//Debug.Log ("Target row: " + j + ","+targetTile.yIndex);
						//}
					}
				} else {
					for (int i = clickedTile.xIndex; i >= targetTile.xIndex; i--) {
						GamePiece remainingPiece = m_allGamePieces [i, clickedTile.yIndex];
						remainingPiece.Move (remainingPiece.xIndex + 1, remainingPiece.yIndex, swapTime);
					}
				}


			}

			if (movement == -1) {

				for (int m = clickedTile.yIndex - 1; m > targetTile.yIndex; m--) {
					//Debug.Log("Rows no. in between: "+m);
					for (int i = 0; i < width; i++) {

						GamePiece remainingPiece = m_allGamePieces [i, m];

						if (remainingPiece.xIndex == 0) {
							remainingPiece.Move (width - 1, remainingPiece.yIndex + 1, swapTime);
						} else {
							remainingPiece.Move (remainingPiece.xIndex - 1, remainingPiece.yIndex, swapTime);
						}
						//Debug.Log("Inbetweeners: "+i+","+m);
					}
				}

				if (clickedTile.yIndex != targetTile.yIndex) {
					for (int i = clickedTile.xIndex; i < width; i++) {

						GamePiece remainingPiece = m_allGamePieces [i, clickedTile.yIndex];

						if (remainingPiece.xIndex == 0) {
							remainingPiece.Move (width - 1, remainingPiece.yIndex + 1, swapTime);
						} else {
							remainingPiece.Move (remainingPiece.xIndex - 1, remainingPiece.yIndex, swapTime);
						}

						//for (int k = clickedTile.yIndex; k < targetTile.yIndex; k++) {
						//Debug.Log ("Clicked row: " + i + ","+clickedTile.yIndex);
						//}
					}

					for (int j = targetTile.xIndex; j >= 0; j--) {
						GamePiece remainingPiece = m_allGamePieces [j, targetTile.yIndex];

						if (remainingPiece.xIndex == 0) {
							remainingPiece.Move (width - 1, remainingPiece.yIndex + 1, swapTime);
						} else {
							remainingPiece.Move (remainingPiece.xIndex - 1, remainingPiece.yIndex, swapTime);
						}
						//for (int l = targetTile.yIndex; l > clickedTile.yIndex; l--) {
						//Debug.Log ("Target row: " + j + ","+targetTile.yIndex);
						//}
					}
				} else {
					for (int i = clickedTile.xIndex; i <= targetTile.xIndex; i++) {
						GamePiece remainingPiece = m_allGamePieces [i, clickedTile.yIndex];
						remainingPiece.Move (remainingPiece.xIndex - 1, remainingPiece.yIndex, swapTime);
					}
				}


			}

		}



	}

	//to fill gamepiece array with preset list of gamepieces
	public void FillBoardFromList (List<GamePiece> gamePieces)
	{

		//converting gamepieces to different collection structure
		//using queue because it uses first in and first out feature
		Queue<GamePiece> unusedPieces = new Queue<GamePiece> (gamePieces);


		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {

				if (m_allGamePieces [i, j] == null) { //to check if there is empty space

					//to get the next game piece from unused pieces using dequeue
					m_allGamePieces[i,j]=unusedPieces.Dequeue();


				}

			}
		}

	}

	//using the method and putting it in shuffleboard method. its like saying: "ShuffleBoard! Here, take it."
	public void ShuffleBoard ()
	{

		//to gather list of normal gamepieces on the board
		List<GamePiece> normalPieces = m_boardShuffler.RemoveNormalPieces(m_allGamePieces);

		//shuffling the list that is taken using the method from BoardShuffler
		m_boardShuffler.ShuffleList(normalPieces);

		//using the list of shuffled pieces in FillBoard from the list method and then fills the array randomly
		FillBoardFromList(normalPieces);

		//since now array is filled up, move everything to its proper place
		m_boardShuffler.MovePieces(m_allGamePieces, swapTime);


	}




}
