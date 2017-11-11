using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	//to establish a relationship between a single tile and gameboard that owns it
	public int xIndex;
	public int yIndex;

	//to reference a board object
	Board m_board;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//when creating a tile, set the x-coordinate and y-coordinate and associate the board with this tile
	//to initialise these variables with correct value
	public void Init(int x, int y, Board board)
	{

		xIndex = x;
		yIndex = y;
		m_board = board;
		//these will be called when creating tiles

	}

	//clicking a tile and setting it as an active one
	void OnMouseDown()
	{
		if (m_board != null) {
			m_board.ClickTile (this);
		}
	}

	//if activeTile is clicked and dragged to another tile, then new tile is set as target tile
	void OnMouseEnter()
	{
		if (m_board != null) {
			m_board.DragToTile (this);
		}
	}

	//if targetTile is adjacent to checkedTile, then switch or swap takes place. clickedTile and targetTile is cleared once the mouse button is released
	void OnMouseUp()
	{
		if (m_board != null) {
			m_board.ReleaseTile ();
		}
	}
}
