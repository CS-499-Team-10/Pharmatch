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

    private DrugTile firstSelected;
	private DrugTile secondSelected;

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void CardTapped(DrugTile card) {
		if (firstSelected == null) {
			firstSelected = card;
			firstSelected.Select();
		} else {
			secondSelected = card;
			secondSelected.Select();
			TryMatch();
		}
	}

    private void TryMatch() {
		if (firstSelected.CheckMatch(secondSelected)) {
			IncrementScore(1);

			tilesOnScreen.Remove(firstSelected);
			tilesOnScreen.Remove(secondSelected);

			Destroy(firstSelected.gameObject);
			Destroy(secondSelected.gameObject);

			Invoke("CreateCard", 0.001f);
			Invoke("CreateCard", 0.001f);
		}
		else {
			Debug.Log("no");
			firstSelected.UnSelect();
			secondSelected.UnSelect();
		}
		firstSelected = null;
		secondSelected = null;
	}
}
