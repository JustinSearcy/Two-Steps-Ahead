using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("Cards")]
    [SerializeField] GameObject p1Shield;
    [SerializeField] GameObject p1Heal;
    [SerializeField] GameObject p1Magic;
    [SerializeField] GameObject p1Attack;
    [SerializeField] GameObject p2Shield;
    [SerializeField] GameObject p2Heal;
    [SerializeField] GameObject p2Magic;
    [SerializeField] GameObject p2Attack;

    [Header("Action Stats")]
    [SerializeField] int healthGain = 3;
    [SerializeField] int attackDamage = 5;
    [SerializeField] int normalMagicDamage = 3;
    [SerializeField] int blockedMagicDamage = 1;
    [SerializeField] int reflectDamage = 3;

    [Header("Other")]
    [SerializeField] float darkenTime = 0.1f;
    [SerializeField] TextMeshPro p1ActionText;
    [SerializeField] TextMeshPro p2ActionText;
    [SerializeField] GameObject startTurnsButton;
    [SerializeField] AudioClip actionReveal;

    [SerializeField] List<int> playerOneMoves = new List<int>();
    [SerializeField] List<int> playerTwoMoves = new List<int>(); //Serialzied to debug

    private int turn = 1;

    PlayerOneHealth playerOneHealth;
    PlayerTwoHealth playerTwoHealth;
    SelectBlocker selectBlocker;
    UICanvas uiCanvas;
    AudioSource audioSource;

    //Shield == 1
    //Heal == 4
    //Magic == 8
    //Attack == 12

    private void Start()
    {
        playerOneHealth = FindObjectOfType<PlayerOneHealth>();
        playerTwoHealth = FindObjectOfType<PlayerTwoHealth>();
        selectBlocker = FindObjectOfType<SelectBlocker>();
        uiCanvas = FindObjectOfType<UICanvas>();
        audioSource = FindObjectOfType<AudioSource>();
        p1ActionText.text = "";
        p2ActionText.text = "";
        startTurnsButton.SetActive(false);
    }

    public int GetTurn() { return turn; }

    public void Shield()
    {
        if (turn > 0)
        {
            playerOneMoves.Add(1);
            if (playerOneMoves.Count == 3)
            {
                turn *= -1;
                selectBlocker.PlayerTwoTurn();
            }
        }
        else
        {
            playerTwoMoves.Add(1);
            if (playerTwoMoves.Count == 3)
            {
                TurnsEnded();
            }
        }
    }

    public void Heal()
    {
        if (turn > 0)
        {
            playerOneMoves.Add(4);
            if (playerOneMoves.Count == 3)
            {
                selectBlocker.PlayerTwoTurn();
                turn *= -1;
            }
        }
        else
        {
            playerTwoMoves.Add(4);
            if (playerTwoMoves.Count == 3)
            {
                TurnsEnded();
            }
        }
    }

    public void Magic()
    {
        if(turn > 0)
        {
            playerOneMoves.Add(8);
            if(playerOneMoves.Count == 3)
            {
                selectBlocker.PlayerTwoTurn();
                turn *= -1;
            }
        }
        else
        {
            playerTwoMoves.Add(8);
            if (playerTwoMoves.Count == 3)
            {
                TurnsEnded();
            }
        }
    }

    public void Attack()
    {
        if (turn > 0)
        {
            playerOneMoves.Add(12);
            if (playerOneMoves.Count == 3)
            {
                selectBlocker.PlayerTwoTurn();
                turn *= -1;
            }
        }
        else
        {
            playerTwoMoves.Add(12);
            if (playerTwoMoves.Count == 3)
            {
                TurnsEnded();
            }
        }
    }

    private void TurnsEnded()
    {
        selectBlocker.ActionPhase();
        startTurnsButton.SetActive(true);
    }

    //1. Heal
    //2. Attack ---> Check for block
    //3. Magic ---> Check for block
    //4. Block does nothing

    public void EvaluateTurn(int turn)
    {
        int outcome = playerOneMoves[turn] * playerTwoMoves[turn];
        CheckCardsPlayed(turn);
        DisplayText(turn);
        StartCoroutine(CheckOutcome(outcome, turn));
    }

    private void CheckCardsPlayed(int turn)
    {
        if (playerOneMoves[turn] != 1) { LeanTween.color(p1Shield, Color.gray, darkenTime); }
        if (playerOneMoves[turn] != 4) { LeanTween.color(p1Heal, Color.gray, darkenTime); }
        if (playerOneMoves[turn] != 8) { LeanTween.color(p1Magic, Color.gray, darkenTime); }
        if (playerOneMoves[turn] != 12) { LeanTween.color(p1Attack, Color.gray, darkenTime); }
        if (playerTwoMoves[turn] != 1) { LeanTween.color(p2Shield, Color.gray, darkenTime); }
        if (playerTwoMoves[turn] != 4) { LeanTween.color(p2Heal, Color.gray, darkenTime); }
        if (playerTwoMoves[turn] != 8) { LeanTween.color(p2Magic, Color.gray, darkenTime); }
        if (playerTwoMoves[turn] != 12) { LeanTween.color(p2Attack, Color.gray, darkenTime); }
        AudioSource.PlayClipAtPoint(actionReveal, audioSource.transform.position, 1f);
    }

    private void DisplayText(int turn)
    {
        if(playerOneMoves[turn] == 1) { p1ActionText.text = "Player 1 Reflects"; }
        else if(playerOneMoves[turn] == 4) { p1ActionText.text = "Player 1 Heals"; }
        else if (playerOneMoves[turn] == 8) { p1ActionText.text = "Player 1 Uses Magic"; }
        else if (playerOneMoves[turn] == 12) { p1ActionText.text = "Player 1 Attacks"; }

        if (playerTwoMoves[turn] == 1) { p2ActionText.text = "Player 2 Reflects"; }
        else if (playerTwoMoves[turn] == 4) { p2ActionText.text = "Player 2 Heals"; }
        else if (playerTwoMoves[turn] == 8) { p2ActionText.text = "Player 2 Uses Magic"; }
        else if (playerTwoMoves[turn] == 12) { p2ActionText.text = "Player 2 Attacks"; }
    }

    IEnumerator CheckOutcome(int outcome, int turn)
    {
        switch (outcome)
        {
            case 1: //Both players reflect
                break;

            case 4: //One player heals, one blocks
                if (playerOneMoves[turn] == 4) { playerOneHealth.Heal(healthGain); }
                else { playerTwoHealth.Heal(healthGain); }
                break;

            case 8: //One player deals less magic
                if (playerOneMoves[turn] == 8) { playerTwoHealth.TakeDamage(blockedMagicDamage); }
                else { playerOneHealth.TakeDamage(blockedMagicDamage); }
                break;
            case 12: //Player reflects attack
                if (playerOneMoves[turn] == 12) { playerOneHealth.TakeDamage(reflectDamage); }
                else { playerTwoHealth.TakeDamage(reflectDamage); }
                break;

            case 16: //Both players heal
                playerOneHealth.Heal(healthGain);
                playerTwoHealth.Heal(healthGain);
                break;

            case 32: //One healed, one magic, nothing happens
                if (playerOneMoves[turn] == 4){playerOneHealth.TakeDamage(normalMagicDamage - healthGain);}
                else{playerTwoHealth.TakeDamage(normalMagicDamage - healthGain);}
                break;

            case 48: //One heal, one attack
                if (playerOneMoves[turn] == 4) { playerOneHealth.TakeDamage(attackDamage - healthGain); }
                else { playerTwoHealth.TakeDamage(attackDamage - healthGain); }
                break;

            case 64: //Both players magic
                playerOneHealth.TakeDamage(normalMagicDamage);
                playerTwoHealth.TakeDamage(normalMagicDamage);
                break;

            case 96: //One magic, one attack
                if (playerOneMoves[turn] == 8)
                {
                    playerTwoHealth.TakeDamage(normalMagicDamage);
                    playerOneHealth.TakeDamage(attackDamage);
                }
                else
                {
                    playerOneHealth.TakeDamage(normalMagicDamage);
                    playerTwoHealth.TakeDamage(attackDamage);
                }
                break;

            case 144: //Both attack
                playerOneHealth.TakeDamage(attackDamage);
                playerTwoHealth.TakeDamage(attackDamage);
                break;

            default:
                break;
        }
        if (CheckHealth()) 
        {
            yield return new WaitForSeconds(3f);
            ResetCardColors();
            yield return new WaitForSeconds(1f);
            turn++;
            if (turn == 3)
            {
                ResetTurns();
            }
            else
            {
                EvaluateTurn(turn);
            }
        }
    }

    private bool CheckHealth() //return false if a player has died
    {
        if(playerOneHealth.GetHealth() <= 0 || playerTwoHealth.GetHealth() <= 0)
        {
            if(playerOneHealth.GetHealth() <= 0 && playerTwoHealth.GetHealth() <= 0)
            {
                uiCanvas.Tie();
                return false;
            }
            else if(playerOneHealth.GetHealth() <= 0)
            {
                uiCanvas.P2Wins();
                return false;
            }
            else 
            {
                uiCanvas.P1Wins();
                return false;
            }
        }
        else { return true; }
    }

    private void ResetTurns()
    {
        playerOneMoves.Clear();
        playerTwoMoves.Clear();
        p1ActionText.text = "";
        p2ActionText.text = "";
        turn *= -1;
        selectBlocker.PlayerOneTurn();
    }

    private void ResetCardColors()
    {
        LeanTween.color(p1Shield, Color.white, darkenTime);
        LeanTween.color(p1Heal, Color.white, darkenTime);
        LeanTween.color(p1Magic, Color.white, darkenTime);
        LeanTween.color(p1Attack, Color.white, darkenTime);
        LeanTween.color(p2Shield, Color.white, darkenTime);
        LeanTween.color(p2Heal, Color.white, darkenTime);
        LeanTween.color(p2Magic, Color.white, darkenTime);
        LeanTween.color(p2Attack, Color.white, darkenTime);
    }
}
