using System.Collections.Generic;
using UnityEngine;

public class PassiveManager : MonoBehaviour
{
    [SerializeField] private List<BasePassive> _equippedPassives = new List<BasePassive>();
    [SerializeField] private PassiveDatabase _passiveDatabase;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        EventManager.OnPlayerLevelUp += EquipPassive;
    }

    void OnDisable()
    {
        EventManager.OnPlayerLevelUp -= EquipPassive;
    }

    void Update()
    {
        foreach (var passive in _equippedPassives)
        {
            passive.ApplyEffect();
        }
    }


    private void EquipPassive(int level, PlayerLevelManager playerLevelManager)
    {
        if(ControllerPlayersManager.Instance.Players.Count == 1 && level%5 != 0)
            return;
        else if(ControllerPlayersManager.Instance.Players.Count == 2 )
        {
            var levelSomma = ControllerPlayersManager.Instance.Players[0].gameObject.GetComponent<PlayerLevelManager>().Level 
            + ControllerPlayersManager.Instance.Players[1].gameObject.GetComponent<PlayerLevelManager>().Level;

            if(levelSomma%5 != 0)
                return;
        } 

        Debug.Log("IS EQUIPPING Passive level: " + level);

        var index = Random.Range(0, _passiveDatabase.passiveList.Count);
        var passive = _passiveDatabase.passiveList[index];

        if(_equippedPassives.Contains(passive))
        {
            _equippedPassives[_equippedPassives.IndexOf(passive)].UpgradeTier();
            _equippedPassives[_equippedPassives.IndexOf(passive)].ApplyEffect();
        }
        else
        {
            passive.ApplyEffect();
            _equippedPassives.Add(passive);
        }
    }
}
