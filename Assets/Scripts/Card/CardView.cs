using System;
using UnityEngine;
using UniRx;
public class CardView : MonoBehaviour
{
    [SerializeField] private CardModel cardModel;
    public CardModel CardModel => cardModel;
    private CardPhysic cardPhysic;
    public CardPhysic CardPhysic => cardPhysic;
    private IDisposable reshuffleSub;

    private void Awake()
    {
        cardPhysic = GetComponent<CardPhysic>();
    }
    private void OnEnable()
    {
        reshuffleSub = GameSignals.OnReshuffle.Subscribe(_ => Destroy(gameObject));
    }
    private void OnMouseDown()
    {
        GameSignals.OnCardClicked.OnNext(this);
    }
    private void OnDisable()
    {
        reshuffleSub?.Dispose();
    }
}
