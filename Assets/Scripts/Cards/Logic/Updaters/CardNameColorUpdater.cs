using System;
using Cards.Data;
using EditorTools.Validators.Core;
using TMPro;
using UniRx;
using UnityEngine;

namespace Cards.Logic.Updaters
{
    public class CardNameColorUpdater : MonoBehaviour, IValidatable
    {
    [Header("References")]
    [SerializeField] private TMP_Text _tmp;
    [SerializeField] private CardData _cardData;

    private IDisposable _colorSubscription;

    #region MonoBehaviour

    public void OnValidate()
    {
        _tmp = GetComponent<TMP_Text>();
        _cardData = GetComponentInParent<CardData>(true);
    }

    private void OnEnable()
    {
        StartObserving();
    }

    private void OnDisable()
    {
        StopObserving();
    }

    #endregion

    private void StartObserving()
    {
        StopObserving();
        _colorSubscription = _cardData.NameColor.Subscribe(SetColor);
    }

    private void StopObserving()
    {
        _colorSubscription?.Dispose();
    }

    private void SetColor(Color color)
    {
        _tmp.color = color;
    }
    }
}
