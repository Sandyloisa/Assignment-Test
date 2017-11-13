using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartingBoard : MonoBehaviour {

	public void ButtonPress ()
	{

		Application.LoadLevel(Application.loadedLevel);

	}
}