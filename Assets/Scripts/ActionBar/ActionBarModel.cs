using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;
using UniRx;
using System;

public class ActionBarModel : IDisposable
{
    private List<CardView> cardsInBar = new();
    private readonly IActionBarView view;
    private readonly CompositeDisposable disposables = new();
    private bool isBusy = false;
    public bool IsBusy => isBusy;
    [Inject] public ActionBarModel(IActionBarView view)
    {
        this.view = view;
        //Подписываемся на клик, заодно фильтруем, чтобы игнорировать клики, пока карточка не переместилась в слот
        GameSignals.OnCardClicked
            .Where(_ => !isBusy)
            .Subscribe(async card =>
            {
                isBusy = true;
                await TryAddCard(card);
                isBusy = false;
            }).AddTo(disposables);
        GameSignals.OnReshuffle.Subscribe(_ => cardsInBar.Clear()).AddTo(disposables);
    }
    public async Task<bool> TryAddCard(CardView card)
    {
        cardsInBar.Add(card);
        card.CardPhysic.SetPhysicsActive(false);
        await view.MoveCardToSlotAsync(card, cardsInBar.Count - 1);
        await CheckForTriplets();
        if (IsLose())
        {
            GameSignals.OnLose.OnNext(Unit.Default);
        }
        return true;
    }
    private async Task CheckForTriplets()
    {
        for (int i = 0; i < cardsInBar.Count; i++)
        {
            CardModel model = cardsInBar[i].CardModel;
            int count = 1;
            List<CardView> matches = new() { cardsInBar[i] };
            List<int> slotIndices = new() { i };

            for (int j = 0; j < cardsInBar.Count; j++)
            {
                if (i == j) continue;
                var other = cardsInBar[j].CardModel;
                if (IsMatch(model, other))
                {
                    count++;
                    matches.Add(cardsInBar[j]);
                    slotIndices.Add(j);
                }
                if (count == 3) break;
            }

            if (count == 3)
            {
                GameSignals.OnMatch.OnNext(Unit.Default);
                view.HighlightSlots(slotIndices, true);
                await Task.Delay(1000);
                view.HighlightSlots(slotIndices, false);
                RemoveMatchesCard(matches);
                MoveCardsInActionBar();

                break;
            }
        }
    }
    private void RemoveMatchesCard(List<CardView> matches)
    {
        foreach (CardView card in matches)
        {
            cardsInBar.Remove(card);
            if (card != null && card.gameObject != null)
                GameObject.Destroy(card.gameObject);
        }
    }
    private void MoveCardsInActionBar()
    {
        for (int i = 0; i < cardsInBar.Count; i++)
        {
            if (cardsInBar[i] != null && cardsInBar[i].gameObject != null)
                view.MoveCardToSlotAsync(cardsInBar[i], i);
        }
    }
    private bool IsLose() => cardsInBar.Count >= view.MaxSlots;
    private bool IsMatch(CardModel first, CardModel second)
    {
        return first.FigureType == second.FigureType &&
               first.ColorType == second.ColorType &&
               first.AnimalType == second.AnimalType;
    }
    public int CardsCount => cardsInBar.Count;
    public void Dispose()
    {
        disposables.Dispose();
    }
}
