// Author: Murillo Gomes Yonamine
// Date: 11/10/2026

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.Utilities;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TMP_Text _message;
    [SerializeField] private Button _nextButton;
    [SerializeField] private string[] _texts =
    {
        "É hora do pique-esconde.",
        "Encontre todos os NPCs escondidos antes que o tempo acabe.",
        "Eles observam ao redor e podem fugir quando perceberem você.",
        "Mire no NPC e clique com o botão esquerdo para encontrá-lo."
    };

    private int _index;
    private int _lastAdvanceFrame = -1;

    private void OnEnable()
    {
        _nextButton.onClick.AddListener(NextText);
        ListenForAnyButtonPress();
    }

    private void OnDisable()
    {
        _nextButton.onClick.RemoveListener(NextText);
    }

    private void Start()
    {
        GameManager.Instance.EnterIntro();
        _panel.SetActive(true);
        _message.text = _texts[0];
    }

    public void NextText()
    {
        if (!_panel.activeSelf || _lastAdvanceFrame == Time.frameCount)
            return;

        _lastAdvanceFrame = Time.frameCount;
        _index++;
        if (_index < _texts.Length)
        {
            _message.text = _texts[_index];
            return;
        }

        _panel.SetActive(false);
        GameManager.Instance.StartGame();
    }

    private void ListenForAnyButtonPress()
    {
        InputSystem.onAnyButtonPress.CallOnce(OnAnyButtonPress);
    }

    private void OnAnyButtonPress(InputControl control)
    {
        NextText();
        if (_panel.activeSelf)
            ListenForAnyButtonPress();
    }
}
