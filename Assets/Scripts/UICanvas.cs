using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    [SerializeField] GameObject p1Wins;
    [SerializeField] GameObject p2Wins;
    [SerializeField] GameObject p1Tie;
    [SerializeField] GameObject p2Tie;
    [SerializeField] GameObject playAgainButton;
    [SerializeField] GameObject quitButton;
    [SerializeField] float winBannerTime = 2f;
    [SerializeField] float bannerWaitTime = 3f;
    [SerializeField] float buttonYPos = -270f;
    [SerializeField] float buttonSeparation = 100f;
    [SerializeField] float buttonDelay = 0.2f;

    public void P1Wins()
    {
        StartCoroutine(WaitAndLoadWin(p1Wins));
        StartCoroutine(ButtonLoad());
    }
    public void P2Wins()
    {
        StartCoroutine(WaitAndLoadWin(p2Wins));
        StartCoroutine(ButtonLoad());
    }

    public void Tie()
    {
        StartCoroutine(WaitAndLoadWin(p1Tie));
        StartCoroutine(WaitAndLoadWin(p2Tie));
        StartCoroutine(ButtonLoad());
    }

    IEnumerator WaitAndLoadWin(GameObject banner)
    {
        yield return new WaitForSeconds(bannerWaitTime);
        LeanTween.moveLocalY(banner, 0f, winBannerTime).setEaseOutBounce();
    }

    IEnumerator ButtonLoad()
    {
        yield return new WaitForSeconds(bannerWaitTime + buttonDelay);
        LeanTween.moveLocalY(playAgainButton, buttonYPos, winBannerTime).setEaseInOutBack();
        yield return new WaitForSeconds(buttonDelay);
        LeanTween.moveLocalY(quitButton, buttonYPos - buttonSeparation, winBannerTime).setEaseInOutBack();
    }
}
