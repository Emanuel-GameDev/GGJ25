using UnityEngine;

public class AumentoRecuperoOssigeno : BasePassive
{
    public int aumentoRecuperoOssigeno = 10;
    public override void ApplyEffect()
    {
        if(alreadyActivated)
            return;

        foreach (var player in ControllerPlayersManager.Instance.Players)
        {
            player.GetComponent<PlayerStats>().RiseOxygenGainRate(aumentoRecuperoOssigeno * TierCounter);
        }

        base.ApplyEffect();
    }
}
