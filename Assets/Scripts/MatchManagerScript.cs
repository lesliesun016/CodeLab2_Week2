using UnityEngine;
using System.Collections;

public class MatchManagerScript : MonoBehaviour {

	protected GameManagerScript gameManager;

	public virtual void Start () {
		gameManager = GetComponent<GameManagerScript>();
	}

	// check whether the entire grid has any matched tokens
	public virtual bool GridHasMatch(){
		bool match = false;
		
		for(int x = 0; x < gameManager.gridWidth; x++){
			for(int y = 0; y < gameManager.gridHeight ; y++){
				if(x < gameManager.gridWidth - 2){
					match = match || GridHasHorizontalMatch(x, y);
				}
			}
		}

		return match;
	}

	// check whether 3 tokens match on the horizontal line
	public bool GridHasHorizontalMatch(int x, int y){
		// get 3 tokens in a row
		GameObject token1 = gameManager.gridArray[x + 0, y];
		GameObject token2 = gameManager.gridArray[x + 1, y];
		GameObject token3 = gameManager.gridArray[x + 2, y];
		
		// check whether all 3 tokens have the same sprite (color)
		if(token1 != null && token2 != null && token3 != null){
			SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
			SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
			SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();
			
			return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);
		} else {
			return false;
		}
	}

	// return how many tokens match on the horizontal line
	public int GetHorizontalMatchLength(int x, int y){
		int matchLength = 1;
		
		// get the first token
		GameObject first = gameManager.gridArray[x, y];

		if(first != null){

			// get the sprite of the first token
			SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>();
			
			// find how many tokens have the same sprite as the first token's on the same horizontal line
			for(int i = x + 1; i < gameManager.gridWidth; i++){
				GameObject other = gameManager.gridArray[i, y];

				if(other != null){
					SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>();

					if(sr1.sprite == sr2.sprite){
						matchLength++;
					} else {
						break;
					}
				} else {
					break;
				}
			}
		}
		
		return matchLength;
	}
		
	// remove all the matched tokens and return the number of removed tokens
	public virtual int RemoveMatches(){
		int numRemoved = 0;

		// go through the entire grid
		for(int x = 0; x < gameManager.gridWidth; x++){
			for(int y = 0; y < gameManager.gridHeight ; y++){
				if(x < gameManager.gridWidth - 2){

					// find how many tokens match on the horizontal line
					int horizonMatchLength = GetHorizontalMatchLength(x, y);

					// remove all the matched tokens if the number of matched token is larger than 2
					if(horizonMatchLength > 2){

						for(int i = x; i < x + horizonMatchLength; i++){
							GameObject token = gameManager.gridArray[i, y]; 
							Destroy(token);

							gameManager.gridArray[i, y] = null;
							numRemoved++;
						}
					}
				}
			}
		}
		
		return numRemoved;
	}
}
