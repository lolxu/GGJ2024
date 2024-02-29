using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModalWindow : MonoBehaviour
{
    [Header("Window")] 
    [SerializeField] private Transform _window;
    
    [Header("Header")] 
    [SerializeField] private Transform _headerArea;
    [SerializeField] private TextMeshProUGUI _titleField;

    [Header("Content")] 
    [SerializeField] private Transform _contentArea;
    [SerializeField] private Transform _verticalLayoutArea;
    [SerializeField] private Image _heroImage;
    [SerializeField] private TextMeshProUGUI _heroText;
    [Space()] 
    [SerializeField] private Transform _horizontalLayoutArea;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _iconText;

    [Header("Footer")] 
    [SerializeField] private Transform _footerArea;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _declineButton;
    [SerializeField] private Button _alternateButton;

    private Action onConfirmAction;
    private Action onDeclineAction;
    private Action onAlternateAction;

    public void Confirm()
    {
        onConfirmAction?.Invoke();
        Close();
    }

    public void Decline()
    {
        onDeclineAction?.Invoke();
        Close();
    }

    public void Alternate()
    {
        onAlternateAction?.Invoke();
        Close();
    }

    private void Close()
    {
        _window.DOScale(Vector3.zero, 0.25f)
            .SetEase(Ease.InOutSine)
            .OnComplete(()=>
            {
                gameObject.GetComponent<Image>().DOColor(Color.clear, 0.25f).SetEase(Ease.InOutSine).OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
            });
    }

    private void Show()
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
            _window.DOScale(Vector3.one, 0.25f)
                .SetEase(Ease.InOutSine);
        }
    }

    public void ShowAsHero(string title, Sprite imageToShow, string message, Action confirmAction, Action declineAction,
        Action alternativeAction = null)
    {

        _horizontalLayoutArea.gameObject.SetActive(false);

        // Hide title if there is none
        bool hasTitle = !string.IsNullOrEmpty(title);
        _headerArea.gameObject.SetActive(hasTitle);
        _titleField.text = title;

        _heroImage.sprite = imageToShow;
        _heroText.text = message;
        
        onConfirmAction = confirmAction;

        bool hasDecline = declineAction != null;
        _declineButton.gameObject.SetActive(hasDecline);
        onDeclineAction = declineAction;

        bool hasAlternate = (alternativeAction != null);
        _alternateButton.gameObject.SetActive(hasAlternate);
        onAlternateAction = alternativeAction;
        
        Show();
    }
}
