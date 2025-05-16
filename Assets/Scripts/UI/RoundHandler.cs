using UnityEngine;
using UnityEngine.UI;

public class RoundHandler : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private RoundCards roundCards;

    private void Start()
    {
        playButton.onClick.AddListener(() =>
        {
            GameSignals.OnStartRound.OnNext(roundCards);
        });
    }
}
