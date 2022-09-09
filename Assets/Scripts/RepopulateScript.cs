using UnityEngine;
using System.Collections;

public class RepopulateScript : MonoBehaviour {
	
	protected GameManagerScript gameManager;

    // set up a game manager
    public virtual void Start () {
		gameManager = GetComponent<GameManagerScript>();
	}

	// fill in empty tokens through the game manager
	public virtual void AddNewTokensToRepopulateGrid(){
		for(int x = 0; x < gameManager.gridWidth; x++){
			GameObject token = gameManager.gridArray[x, gameManager.gridHeight - 1];
			if(token == null){
				gameManager.AddTokenToPosInGrid(x, gameManager.gridHeight - 1, gameManager.grid);
			}
		}
	}
}
