using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public UnityEngine.UI.Text scoreLabel;
    public GameObject winnerLabelObject;
    public void Update()
    {
        // get counts of Items
        int count = GameObject.FindGameObjectsWithTag("Item").Length;
        scoreLabel.text = count.ToString();

        if (count <= 0) {
            // clear the game
            winnerLabelObject.SetActive(true);
        }
    }
}
