using UnityEngine;

public class RisePlayersSpeed : BasePassive
{
    public float aumentoVelocita = 10f;
    public override void ApplyEffect()
    {
        if(!alreadyActivated)
        {
            foreach(var player in ControllerPlayersManager.Instance.Players)
            {
                player.GetComponent<PlayerController>().RiseSpeed(aumentoVelocita * TierCounter);
            }
        }
        
        base.ApplyEffect();
    }
}
