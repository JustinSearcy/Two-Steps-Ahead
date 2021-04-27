using UnityEngine;
using System.Collections;
using TMPro;

public class TurnsButtons : MonoBehaviour
{
    public void TurnResults()
    {
        FindObjectOfType<GameController>().EvaluateTurn(0);
    }
}
