using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardShuffler : MonoBehaviour {



	//first to remove all the gamepieces from the game and store them in a list of gamepieces
	public List<GamePiece>RemoveNormalPieces(GamePiece[,] allPieces)
	{
		//pass the separate pieces of gamepieces
		//reserving empty list of gamepieces
		List<GamePiece> normalPieces = new List<GamePiece>();

		int width = allPieces.GetLength (0);
		int height = allPieces.GetLength (1);

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {

				//add these gamepieces to the list
				normalPieces.Add(allPieces[i,j]);

				//clear out all the components from allPieces array
				allPieces[i,j]=null;

			}
		}
		return normalPieces;
	}

	//to make the shuffling/swapping actually happen
	public void ShuffleList(List<GamePiece> piecesToShuffle)
	{

		int maxCount = piecesToShuffle.Count;

		for (int i = 0; i < maxCount - 1; i++) { //maxCount - 1 because we dont need to swap the last item on the list

			//to get random number between i and maxCount
			int r = Random.Range(i,maxCount);

			//if the random number is same as the counter of loop number, then we dont need to do anything and proceed with the next loop
			if (r == i) {
				continue;
			}

			//otherwise swap the two gamepieces
			//actual swapping mechanism
			GamePiece temp=piecesToShuffle[r];
			piecesToShuffle [r] = piecesToShuffle [i];
			piecesToShuffle[i]=temp;
				

		}

	}

	//to move everything where it is supposed to be
	public void MovePieces (GamePiece[,] allPieces, float swapTime = 0.5f) //swapTime to control the speed of the movement while shuffling positions
	{
		int width = allPieces.GetLength (0);
		int height = allPieces.GetLength (1);

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {

				if (allPieces [i, j] != null) {
					allPieces [i, j].Move (i, j, swapTime);
				}

			}
		}

	}
}
