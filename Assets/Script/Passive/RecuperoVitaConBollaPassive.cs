using System.Threading;
using Cysharp.Threading.Tasks;

public class RecuperoVitaConBollaPassive : BasePassive
{
    public float recuperoVitaConBolla = 0.2f;
    CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    bool isRegenerating = false;

    public override void ApplyEffect()
    {
        bool isCarringBubble = false;
        PlayerStats playerCarringBubble = null;
        foreach (var player in ControllerPlayersManager.Instance.Players)
        {
            if(player.GetComponent<PlayerStats>().CarryBubble)
            {
                isCarringBubble = true;
                playerCarringBubble = player.GetComponent<PlayerStats>();
                break;
            }
        }

        if (isCarringBubble
            && !isRegenerating)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            StartRecuperoVitaConBollaPassive(playerCarringBubble).Forget();
        }
    }

    public async UniTask StartRecuperoVitaConBollaPassive(PlayerStats playerStats)
    {
        isRegenerating = true;
        while(playerStats.CarryBubble)
        {
            playerStats.RegenerateHealth(recuperoVitaConBolla * TierCounter);
            await UniTask.Delay(1000, cancellationToken: _cancellationTokenSource.Token);

            if(_cancellationTokenSource.IsCancellationRequested)
            {
                break;
            }
        }
        isRegenerating = false;
    }

    public void RemoveEffect()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        _cancellationTokenSource = new CancellationTokenSource();
    }
}
