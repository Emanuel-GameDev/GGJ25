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

    public GameObject schermataMorte;

    public static GameHUDmanager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        //EventManager.OnPlayerDeath += Die();

        schermataMorte.SetActive(false);
    }

    private void OnDisable()
    {
        //EventManager.OnPlayerDeath -= Die();
    }


    public void Die()
    {
        if (schermataMorte.activeSelf) return;

        schermataMorte.SetActive(true);

        EventManager.OnPlayerDeath?.Invoke();
    }

    public void UpdateLvlText(PlayerLevelManager lvlManager, int lvl)
    {
        if (lvlManager.gameObject.GetComponentInParent<PlayerInput>().playerIndex == 0)
        {
            lvlRefP1.GetComponentInChildren<TextMeshProUGUI>().text = lvl.ToString();
        }
        else if (lvlManager.gameObject.GetComponentInParent<PlayerInput>().playerIndex == 1)
        {
            lvlRefP2.GetComponentInChildren<TextMeshProUGUI>().text = lvl.ToString();
        }
    }

    public void LoadWeaponSprites(PlayerHandler handler, int ID)
    {
        if (ID ==0)
        {
            List<BaseWeapon> equippedWeaponsP1 = handler.GetEquippedWeapons();

            for (int i = 0; i < equippedWeaponsP1.Count; i++)
            {
                slotRecapP1.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = equippedWeaponsP1[i].sprite;

                slotRecapP1.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
            }

        }
        else if (ID ==1)
        {
            List<BaseWeapon> equippedWeaponsP2 = handler.GetEquippedWeapons();

            for (int i = 0; i < equippedWeaponsP2.Count; i++)
            {
                slotRecapP2.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = equippedWeaponsP2[i].sprite;
                slotRecapP2.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
            }

        }
    }

}
