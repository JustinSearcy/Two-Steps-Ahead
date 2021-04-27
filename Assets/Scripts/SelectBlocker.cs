using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectBlocker : MonoBehaviour
{
    [SerializeField] GameObject playerOneBlocker;
    [SerializeField] GameObject playerTwoBlocker;
    [SerializeField] TextMeshPro turnText;

    void Start()
    {
        turnText.text = "Player 1's Turn";
        playerOneBlocker.SetActive(false);
        playerTwoBlocker.SetActive(true);
    }

    public void PlayerTwoTurn()
    {
        turnText.text = "Player 2's Turn";
        playerTwoBlocker.SetActive(false);
        playerOneBlocker.SetActive(true);
    }

    public void PlayerOneTurn()
    {
        turnText.text = "Player 1's Turn";
        playerOneBlocker.SetActive(false);
        playerTwoBlocker.SetActive(true);
    }

    public void ActionPhase()
    {
        turnText.text = "";
        playerOneBlocker.SetActive(true);
        playerTwoBlocker.SetActive(true);
    }
}
