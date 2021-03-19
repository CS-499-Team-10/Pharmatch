using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapController : SceneController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < GetCells().Length; i++)
        {
            CreateCard();
        }
    }

    private DrugTile _firstRevealed;
	private DrugTile _secondRevealed;

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void CardTapped(DrugTile card) {
		if (_firstRevealed == null) {
			_firstRevealed = card;
			_firstRevealed.Select();
		} else {
			_secondRevealed = card;
			_secondRevealed.Select();
			TryMatch();
		}
	}

    private void TryMatch() {
		if (_firstRevealed.CheckMatch(_secondRevealed)) {
			IncrementScore(1);

			tilesOnScreen.Remove(_firstRevealed);
			tilesOnScreen.Remove(_secondRevealed);

			Destroy(_firstRevealed.gameObject);
			Destroy(_secondRevealed.gameObject);

			Invoke("CreateCard", 0.001f);
			Invoke("CreateCard", 0.001f);
		}
		else {
			// yield return new WaitForSeconds(.5f);
			Debug.Log("no");
			_firstRevealed.UnSelect();
			_secondRevealed.UnSelect();
		}
		_firstRevealed = null;
		_secondRevealed = null;
	}
}
