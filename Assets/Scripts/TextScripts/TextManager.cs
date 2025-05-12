using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TextManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textField;  // Подключение текста
    [SerializeField] private float charDelay = 0.05f;     // Задержка между символами
    [SerializeField] private List<string> tutorialTexts;  // Фразы для текста
    [SerializeField] private AudioSource typingAudio;
    [SerializeField] private AudioClip typingClip;
    public string _levelToLoad;

    private int currentIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        ShowNextText();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                textField.text = tutorialTexts[currentIndex];
                isTyping = false;
            }
            else
            {
                currentIndex++;
                if (currentIndex < tutorialTexts.Count)
                {
                    ShowNextText();
                }
                else
                {
                    EndTutorial();
                }
            }
        }
    }

    void ShowNextText()
    {
        typingCoroutine = StartCoroutine(TypeText(tutorialTexts[currentIndex]));
    }

    IEnumerator TypeText(string fullText)
    {
        isTyping = true;
        textField.text = "";

        foreach (char c in fullText)
        {
            textField.text += c;

            if (typingAudio && typingClip)
                typingAudio.PlayOneShot(typingClip);

            yield return new WaitForSeconds(charDelay);
        }

        isTyping = false;
    }

    void EndTutorial()
    {
        SceneManager.LoadScene(_levelToLoad);
    }
}
