using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    
    [SerializeField] private GameObject funnel;
    [SerializeField] private float spawnYpos = 77f;
    [SerializeField] private float xRangeWithFunnel = 24f;
    [SerializeField] private float xRangeWithoutFunnel = 15f;
    [SerializeField] private int maxAttempts = 20;
    [SerializeField] private float cardWidth = 2f;
    [SerializeField] private int spawnDelay = 200;
    [SerializeField] private bool useFunnel;
    private RoundCards roundCards;
    private readonly CompositeDisposable disposables = new();
    private int currentCards;

    private void Start()
    {
        funnel.SetActive(useFunnel);

        GameSignals.OnStartRound.
            Subscribe(_ =>
            {
                OnStartRound(_);
            })
            .AddTo(disposables);

        GameSignals.OnReshuffle
            .Subscribe(_ => SpawnCards(currentCards))
            .AddTo(disposables);

        GameSignals.OnMatch
           .Subscribe(_ =>
           {
               currentCards -= 3;
               CheckWin();
           })
           .AddTo(disposables);
    }
    private void OnStartRound(RoundCards cards)
    {
        roundCards = cards;
        currentCards = roundCards.SpawnCount;
        SpawnCards(currentCards);
    }
    private async void SpawnCards(int count)
    {
        List<GameObject> spawnList = GenerateCardSpawnList(count);
        for (int i = 0; i < count && i < spawnList.Count; i++)
        {
            Vector2 spawnPos = GetRandomSpawnPosition();
            Instantiate(spawnList[i], spawnPos, Quaternion.identity);
            await Task.Delay(spawnDelay);
        }
    }
    private Vector2 GetRandomSpawnPosition()
    {
        int attempt = 0;
        Vector2 spawnPos = Vector2.zero;
        bool found = false;

        while (attempt < maxAttempts && !found)
        {
            float xRange = useFunnel ? xRangeWithFunnel : xRangeWithoutFunnel;
            float randomX = Random.Range(-xRange, xRange);
            spawnPos = new Vector2(randomX, spawnYpos);

            Collider2D hit = Physics2D.OverlapBox(spawnPos, new Vector2(cardWidth, cardWidth), 0f);
            if (hit == null)
                found = true;
            else
                attempt++;
        }
        return spawnPos;
    }
    private List<GameObject> GenerateCardSpawnList(int count)
    {
        var cardList = new List<GameObject>();
        int uniqueCount = roundCards.Prefabs.Length;
        int totalTriplets = count / 3;

        int tripletsPerType = totalTriplets / uniqueCount;
        int remainingTriplets = totalTriplets % uniqueCount;

        for (int i = 0; i < uniqueCount; i++)
        {
            for (int t = 0; t < tripletsPerType * 3; t++)
            {
                cardList.Add(roundCards.Prefabs[i]);
            }
        }

        var indices = new List<int>();
        for (int i = 0; i < uniqueCount; i++)
            indices.Add(i);
        
        for (int i = 0; i < indices.Count; i++)
        {
            int rnd = Random.Range(i, indices.Count);
            (indices[i], indices[rnd]) = (indices[rnd], indices[i]);
        }
        for (int i = 0; i < remainingTriplets; i++)
        {
            int idx = indices[i % indices.Count];
            for (int t = 0; t < 3; t++)
            {
                cardList.Add(roundCards.Prefabs[idx]);
            }
        }

        
        for (int i = 0; i < cardList.Count; i++)
        {
            int rnd = Random.Range(i, cardList.Count);
            (cardList[i], cardList[rnd]) = (cardList[rnd], cardList[i]);
        }

        return cardList;
    }
    private void CheckWin()
    {
        if (currentCards <= 0)
        {
            GameSignals.OnWin.OnNext(Unit.Default);
        }
    }
}
