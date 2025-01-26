using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveManager : MonoBehaviour
{
    [SerializeField] private List<BasePassive> _equippedPassives = new List<BasePassive>();
    public PassiveDatabase _passiveDatabase;

    [SerializeField] GameObject passiveSlot;
    [SerializeField] List<Outline> outlines;

    public static PassiveManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

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
        if (level % 5 != 0)
            return;
        //else if (ControllerPlayersManager.Instance.Players.Count == 2)
        //{
        //    if (!ControllerPlayersManager.Instance.Players[0].gameObject.GetComponentInChildren<PlayerLevelManager>()) 
        //    {
        //        Debug.Log("passive manager There is no PlayerLevelManager");
        //    }
        //    int levelSomma = ControllerPlayersManager.Instance.Players[0].gameObject.GetComponentInChildren<PlayerLevelManager>()._level
        //    + ControllerPlayersManager.Instance.Players[1].gameObject.GetComponentInChildren<PlayerLevelManager>()._level;

        //    if (levelSomma % 5 != 0)
        //        return;
        //}

        Debug.Log("IS EQUIPPING Passive level: " + level);

        var index = Random.Range(0, _passiveDatabase.passiveList.Count);
        var passive = _passiveDatabase.passiveList[index];

        Debug.Log("EQUIPPED PASSIVE: " + passive.name);

        if (_equippedPassives.Contains(passive))
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

        //COntrollo outline
        for (int i = 0; i < _equippedPassives.Count; i++)
        {
            if (!outlines[i].enabled)
            {
                outlines[i].enabled = true;
            }
        }
    }
}
