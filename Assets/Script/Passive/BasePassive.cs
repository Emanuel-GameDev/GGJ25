using UnityEngine;


public enum PassiveType
{
    RecuperoVitaConBolla,
    RiseOxygenGainRate,
    RisePlayerSpeed
}
public abstract class BasePassive : MonoBehaviour
{
    public string passiveName;
    public string passiveDescription;
    public Sprite passiveSprite;
    public bool activeOneTime = false;
    public bool alreadyActivated = false;
    public int TierCounter = 1;
    public PassiveType PassiveType;

    public virtual void ApplyEffect()
    {
        if(activeOneTime)
        {
            alreadyActivated = true;
        }
    }

    public virtual void UpgradeTier()
    {
        if(TierCounter == 3) 
            return;

        TierCounter++;

        if(activeOneTime)
            alreadyActivated = false;
    }
}
