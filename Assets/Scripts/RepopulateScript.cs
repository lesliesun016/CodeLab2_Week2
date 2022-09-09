using UnityEngine;
using System.Collections;

public class RepopulateScript : MonoBehaviour {
	
	protected GameManagerScript gameManager;

    // set up a game manager
    public virtual void Start () {
		gameManager = GetComponent<GameManagerScript>();
	}

	// fill empty tokens in the grid
	public virtual void AddNewTokensToRepopulateGrid(){
		// go through every token in the grid
		for(int x = 0; x < gameManager.gridWidth; x++){
			GameObject token = gameManager.gridArray[x, gameManager.gridHeight - 1];
			// if token is empty, add new tokens to the grid
			if(token == null){
				gameManager.AddTokenToPosInGrid(x, gameManager.gridHeight - 1, gameManager.grid);
			}
		}
	}
}
