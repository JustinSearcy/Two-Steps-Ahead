using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private float highlightTime = 0.1f;
    [SerializeField] private float clickTime = 0.4f;
    [SerializeField] GameObject selectParticles;
    [SerializeField] AudioClip cardHighlight;
    [SerializeField] AudioClip cardSelect;
    [SerializeField] private float highlightVolume = 0.6f;
    [SerializeField] private float selectVolume = 1f;

    private Vector3 originalScale;
    private bool isClicking = false;

    GameController gameController;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();
        gameController = FindObjectOfType<GameController>();
        originalScale = gameObject.transform.localScale;
    }

    private void OnMouseOver()
    {
        if (!isClicking)
        {
            LeanTween.scale(gameObject, new Vector2(1.1f, 1.1f), highlightTime);
            selectParticles.SetActive(true);
        }
    }

    private void OnMouseEnter()
    {
        AudioSource.PlayClipAtPoint(cardHighlight, audioSource.transform.position, highlightVolume);
    }

    private void OnMouseExit()
    {
        LeanTween.scale(gameObject, originalScale, highlightTime);
        selectParticles.SetActive(false);
    }

    private void OnMouseDown()
    {
        StartCoroutine(Click());
    }

    IEnumerator Click()
    {
        selectParticles.SetActive(false);
        isClicking = true;
        AudioSource.PlayClipAtPoint(cardSelect, audioSource.transform.position, selectVolume);
        LeanTween.scale(gameObject, originalScale, clickTime).setEaseOutElastic();
        gameController.Shield();
        yield return new WaitForSeconds(clickTime);
        isClicking = false;
    }
}
