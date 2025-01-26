using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveManager : MonoBehaviour
{
    [SerializeField] private List<BasePassive> _equippedPassives = new List<BasePassive>();
    [SerializeField] private PassiveDatabase _passiveDatabase;

    [SerializeField] GameObject passiveSlot;

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

        Debug.Log("EQUIPPED PASSIVE: " + passive.name);

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

        // Equipaggio nella game HUD
        for (int i = 0; i < passiveSlot.transform.childCount; i++)
        {
            if (!passiveSlot.transform.GetChild(i).GetChild(0).gameObject.activeSelf)
            {
                passiveSlot.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = passive.passiveSprite;
                passiveSlot.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}
