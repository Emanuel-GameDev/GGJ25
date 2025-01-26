using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameHUDmanager : MonoBehaviour
{
    public GameObject slotRecapP1;
    public GameObject slotRecapP2;

    public GameObject lvlRefP1;
    public GameObject lvlRefP2;

    public static GameHUDmanager instance;

    private void Start()
    {
        LoadWeaponSprites();
    }

    public void UpdateLvlText(PlayerLevelManager lvlManager, int lvl)
    {
        if (lvlManager.gameObject.GetComponent<PlayerInput>().playerIndex == 0)
        {
            lvlRefP1.GetComponent<TextMeshProUGUI>().text = lvl.ToString();
        }
        else if (lvlManager.gameObject.GetComponent<PlayerInput>().playerIndex == 1)
        {
            lvlRefP2.GetComponent<TextMeshProUGUI>().text = lvl.ToString();
        }
    }

    public void LoadWeaponSprites()
    {
        List<BaseWeapon> equippedWeaponsP1 = WeaponManager.Instance.ActualLevelingPlayer1._playerHandler.GetEquippedWeapons();

        List<BaseWeapon> equippedWeaponsP2 = WeaponManager.Instance.ActualLevelingPlayer2._playerHandler.GetEquippedWeapons();

        for (int i = 0; i < slotRecapP1.transform.childCount; i++)
        {
            slotRecapP1.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = equippedWeaponsP1[i].sprite;
        }

        for (int i = 0; i < slotRecapP2.transform.childCount; i++)
        {
            slotRecapP1.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = equippedWeaponsP2[i].sprite;
        }
    }

}
