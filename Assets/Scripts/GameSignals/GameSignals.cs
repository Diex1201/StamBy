using UniRx;

public static class GameSignals
{
    public static Subject<CardView> OnCardClicked = new Subject<CardView>();
    public static Subject<Unit> OnReshuffle = new Subject<Unit>();
    public static Subject<Unit> OnMatch = new Subject<Unit>();
    public static Subject<Unit> OnLose = new Subject<Unit>();
    public static Subject<Unit> OnWin = new Subject<Unit>();
    public static Subject<Unit> OnReloadScene = new Subject<Unit>();
    public static Subject<RoundCards> OnStartRound = new Subject<RoundCards>();

    public static void ResetAll()
    {
        OnReloadScene.Dispose();
        OnReshuffle.Dispose();
        OnWin.Dispose();
        OnLose.Dispose();
        OnMatch.Dispose();
        OnCardClicked.Dispose();
        OnStartRound.Dispose();

        OnReloadScene = new Subject<Unit>();
        OnReshuffle = new Subject<Unit>();
        OnWin = new Subject<Unit>();
        OnLose = new Subject<Unit>();
        OnMatch = new Subject<Unit>();
        OnCardClicked = new Subject<CardView>();
        OnStartRound = new Subject<RoundCards>();
    }
}
