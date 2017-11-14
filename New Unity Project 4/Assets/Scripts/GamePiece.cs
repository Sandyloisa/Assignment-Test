using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour { // this class will be very similar to tile class

	public int xIndex;
	public int yIndex;

	//to reference a board object
	Board m_board;

	//to check if the movement is still happening or not
	bool m_isMoving=false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


		
	}

	//pass reference to the board that it created the game piece
	public void Init(Board board)
	{

		m_board = board;
		//these will be called when creating tiles

	}

	//method to set x and y coordinate
	public void SetCoord(int x,int y) //not used Init because it will be used frequently just after creating gamepiece
	{

		xIndex = x;
		yIndex = y;

	}

	//to make it move. i like to move it move it!!!
	public void Move (int destX, int destY, float timeToMove) //destX and destY will allow the movement in x and y axes only and timeToMove will control the length of action of movement
	{
		//to start with the coroutine for the movement of dots
		if (!m_isMoving) { // if its  not moving, then only starting the coroutine

			StartCoroutine (MoveRoutine (new Vector3 (destX, destY, 0), timeToMove));

		}
	}

	IEnumerator MoveRoutine(Vector3 destination, float timeToMove)
	{

		//to store the start position
		Vector3 startPosition = transform.position;

		bool reachedDestination = false; // to check if the target dot has actually reached the destination grid. we will keep on lerping gamepiece towards destination and when it reaches near the desination, it will be set to true

		float elapsedTime = 0f; // to hold the time passed while moving

		m_isMoving = true; //this is set to true because the movement is already invoked now
		while (!reachedDestination) {

			//check to see if it reached the destination
			if (Vector3.Distance (transform.position, destination) < 0.01f) { //0.01 is almost near the final destination
				reachedDestination=true; // thus once it reaches the destination position, the boolean value will turn to true to come out of the loop

				/*transform.position=destination; //this will put the gameobject's final position exactly to the destination position
				// to set xindex and yindex once reached the destination
				SetCoord((int)destination.x, (int)destination.y);*/

				if (m_board != null) {
					m_board.PlaceGamePiece (this, (int)destination.x, (int)destination.y);
				}

				break; // breaking without running the next lines because it already reached the destination now and is not really needed

			}

			// if not reached the destination, it will keep an ongoing timer to check the amount of time passed
			elapsedTime +=Time.deltaTime; //time.deltatime is the time in seconds that it took the last frame to runs

			float t = Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f); //t=1 when reached to the end position and t=0 when starts


			t=t*t*t*(6*t*t-15*t+10); // this is just to get a very smooth transition of position during the lerping. Very good thing

			transform.position = Vector3.Lerp (startPosition, destination,t); //to make it lerp from startposition to end position and t is used to control lerping
			//when t=0, it puts in the startposition and when t=1, it puts in the destination position

			yield return null; // this tells the engine to wait until the next frame to resume execution of the while loop
		}

		m_isMoving=false; // it is falsed because now its not moving anymore

	}

}
