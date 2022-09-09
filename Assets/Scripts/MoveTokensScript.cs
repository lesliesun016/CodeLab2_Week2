using UnityEngine;
using System.Collections;

public class MoveTokensScript : MonoBehaviour {

	protected GameManagerScript gameManager;
	protected MatchManagerScript matchManager;

	public bool move = false;

	public float lerpPercent;
	public float lerpSpeed;

	bool userSwap;

	protected GameObject exchangeToken1;
	GameObject exchangeToken2;

	Vector2 exchangeGridPos1;
	Vector2 exchangeGridPos2;

	public virtual void Start () {
		gameManager = GetComponent<GameManagerScript>();
		matchManager = GetComponent<MatchManagerScript>();
		lerpPercent = 0;
	}

	public virtual void Update () {

		if(move){
			lerpPercent += lerpSpeed;

			if(lerpPercent >= 1){
				lerpPercent = 1;
			}

			if(exchangeToken1 != null){
				ExchangeTokens();
			}
		}
	}

	//make the token move
	public void SetupTokenMove(){
		move = true;
		lerpPercent = 0;
	}

	// swap the positions of token1 and token2
	public void SetupTokenExchange(GameObject token1, Vector2 pos1,
	                               GameObject token2, Vector2 pos2, bool reversable){
		SetupTokenMove();

		exchangeToken1 = token1;
		exchangeToken2 = token2;

		exchangeGridPos1 = pos1;
		exchangeGridPos2 = pos2;

		// set up whether the two tokens should be reversable
		this.userSwap = reversable;
	}

	public virtual void ExchangeTokens(){

		// get the positions of two tokens
		Vector3 startPos = gameManager.GetWorldPositionFromGridPosition((int)exchangeGridPos1.x, (int)exchangeGridPos1.y);
		Vector3 endPos = gameManager.GetWorldPositionFromGridPosition((int)exchangeGridPos2.x, (int)exchangeGridPos2.y);

//		Vector3 movePos1 = Vector3.Lerp(startPos, endPos, lerpPercent);
//		Vector3 movePos2 = Vector3.Lerp(endPos, startPos, lerpPercent);

		Vector3 movePos1 = SmoothLerp(startPos, endPos, lerpPercent);
		Vector3 movePos2 = SmoothLerp(endPos, startPos, lerpPercent);

		// swap the positions of two tokens
		exchangeToken1.transform.position = movePos1;
		exchangeToken2.transform.position = movePos2;

		// once the swap is finished, change their positions in the grid array
		if(lerpPercent == 1){
			gameManager.gridArray[(int)exchangeGridPos2.x, (int)exchangeGridPos2.y] = exchangeToken1;
			gameManager.gridArray[(int)exchangeGridPos1.x, (int)exchangeGridPos1.y] = exchangeToken2;

            // if no tokens match and the swap is reversible, swap them back and reset their swap value to false
            if (!matchManager.GridHasMatch() && userSwap){
				SetupTokenExchange(exchangeToken1, exchangeGridPos2, exchangeToken2, exchangeGridPos1, false);
			} else {
				exchangeToken1 = null;
				exchangeToken2 = null;
				move = false;
			}
		}
	}

	// return the smoothed vector of the token (x, y, z)
	private Vector3 SmoothLerp(Vector3 startPos, Vector3 endPos, float lerpPercent){
		return new Vector3(
			Mathf.SmoothStep(startPos.x, endPos.x, lerpPercent),
			Mathf.SmoothStep(startPos.y, endPos.y, lerpPercent),
			Mathf.SmoothStep(startPos.z, endPos.z, lerpPercent));
	}

	// move a token to a single empty block
	public virtual void MoveTokenToEmptyPos(int startGridX, int startGridY,
	                                int endGridX, int endGridY,
	                                GameObject token){
	
		// get the positions of start and end
		Vector3 startPos = gameManager.GetWorldPositionFromGridPosition(startGridX, startGridY);
		Vector3 endPos = gameManager.GetWorldPositionFromGridPosition(endGridX, endGridY);

		// get the position bewteen start and end positions
		Vector3 pos = Vector3.Lerp(startPos, endPos, lerpPercent);

		// move the token to the position
		token.transform.position =	pos;

		// once the token is moved to the end pos, change it in the grid array
		if(lerpPercent == 1){
			gameManager.gridArray[endGridX, endGridY] = token;
			gameManager.gridArray[startGridX, startGridY] = null;
		}
	}

	// move tokens to all empty spaces and return the bool value whether any tokens are moved
	public virtual bool MoveTokensToFillEmptySpaces(){
		bool movedToken = false;

		for(int x = 0; x < gameManager.gridWidth; x++){
			for(int y = 1; y < gameManager.gridHeight ; y++){
				if(gameManager.gridArray[x, y - 1] == null){
					for(int pos = y; pos < gameManager.gridHeight; pos++){
						GameObject token = gameManager.gridArray[x, pos];
						if(token != null){
							MoveTokenToEmptyPos(x, pos, x, pos - 1, token);
							movedToken = true;
						}
					}
				}
			}
		}

		// once the move is finished, disable move by resetting the value to false
		if(lerpPercent == 1){
			move = false;
		}

		return movedToken;
	}
}
