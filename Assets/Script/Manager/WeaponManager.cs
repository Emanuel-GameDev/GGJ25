using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public WeaponDatabase database;
    public static WeaponManager Instance;

    public GameObject[] ActualWeaponPoolPlayer1 = new GameObject[3];
    public GameObject[] ActualWeaponPoolPlayer2 = new GameObject[3];
    public PlayerLevelManager ActualLevelingPlayer1;
    public PlayerLevelManager ActualLevelingPlayer2;




    public TextMeshProUGUI textNome1P1;
    public TextMeshProUGUI textNome2P1;
    public TextMeshProUGUI textNome3P1;
    public TextMeshProUGUI textDesc1P1;
    public TextMeshProUGUI textDesc2P1;
    public TextMeshProUGUI textDesc3P1;
    public Image sprite1P1;
    public Image sprite2P1;
    public Image sprite3P1;

    [Space]

    public TextMeshProUGUI textNome1P2;
    public TextMeshProUGUI textNome2P2;
    public TextMeshProUGUI textNome3P2;
    public TextMeshProUGUI textDesc1P2;
    public TextMeshProUGUI textDesc2P2;
    public TextMeshProUGUI textDesc3P2;
    public Image sprite1P2;
    public Image sprite2P2;
    public Image sprite3P2;

    [Space]

    public TextMeshProUGUI textNome1Passive;
    public TextMeshProUGUI textNome2Passive;
    public TextMeshProUGUI textNome3Passive;
    public TextMeshProUGUI textDesc1Passive;
    public TextMeshProUGUI textDesc2Passive;
    public TextMeshProUGUI textDesc3Passive;
    public Image sprite1Passive;
    public Image sprite2Passive;
    public Image sprite3Passive;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        EventManager.OnPlayerLevelUp += SetUpChooseWeaponCanvas;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerLevelUp -= SetUpChooseWeaponCanvas;
    }

    public void EquipRandomWeapon(int level, PlayerLevelManager levelManager)
    {
        if (database.weaponDatabase.Count > 0)
        {
            int randomIndex = Random.Range(0, database.weaponDatabase.Count);
            GameObject randomWeaponPrefab = database.weaponDatabase[randomIndex];
            BaseWeapon weaponPrefab = randomWeaponPrefab.GetComponent<BaseWeapon>();

            List<BaseWeapon> playerWeapons = levelManager._playerHandler.GetEquippedWeapons();
            BaseWeapon existingWeapon = playerWeapons.Find(w => w.GetType() == weaponPrefab.GetType());

            if (existingWeapon != null)
            {
                // Se l'arma � gi� equipaggiata, potenziala
                existingWeapon.UpgradeTier();
                Debug.Log($"{existingWeapon.name} � stata potenziata!");
            }
            else
            {
                // Altrimenti, equipaggia la nuova arma
                GameObject instantiatedWeapon = Instantiate(randomWeaponPrefab);

                // Aggiungi l'arma istanziata alla lista delle armi equipaggiate di PlayerHandler
                levelManager._playerHandler.EquipWeapon(instantiatedWeapon);

                Debug.Log($"{instantiatedWeapon.name} � stata equipaggiata!");
            }
        }
        else
        {
            Debug.LogWarning("Weapon database is empty!");
        }
    }

    private void SetUpChooseWeaponCanvas(int level, PlayerLevelManager levelManager)
    {
        if (ControllerPlayersManager.Instance.Players.Count == 1 && level % 5 == 0)
            return;
        else if (ControllerPlayersManager.Instance.Players.Count == 2)
        {
            var levelSomma = ControllerPlayersManager.Instance.Players[0].gameObject.GetComponent<PlayerLevelManager>().Level
            + ControllerPlayersManager.Instance.Players[1].gameObject.GetComponent<PlayerLevelManager>().Level;

            if (levelSomma % 5 == 0)
                return;
        }

        var canvas = GameObject.FindGameObjectsWithTag("ChooseWeaponCanvas");
        var pauseManager = FindAnyObjectByType<PauseManager>();

        if (pauseManager != null)
        {
            pauseManager.pauseAction.Disable();
            pauseManager.PauseAll();
        }

        if (canvas.Length > 0)
        {
            var canvasScript = canvas[0].GetComponent<Canvas>();
            canvasScript.enabled = true;

            if (levelManager.GetComponentInParent<PlayerInput>().playerIndex == 0)
            {
                var weaponPoolPanel1 = GameObject.FindGameObjectsWithTag("Player1WeaponPool")[0];

                weaponPoolPanel1.gameObject.GetComponent<Image>().enabled = true;

                for (int i = 0; i < weaponPoolPanel1.transform.childCount; i++)
                {
                    weaponPoolPanel1.transform.GetChild(i).gameObject.GetComponent<Image>().enabled = true;

                    for (int j = 0; j < weaponPoolPanel1.transform.GetChild(i).childCount; j++)
                    {
                        weaponPoolPanel1.transform.GetChild(i).GetChild(j).gameObject.SetActive(true);
                    }
                }

                weaponPoolPanel1.gameObject.GetComponent<WeaponPoolSelector>().enabled = true;



                var passivesPool = GameObject.FindGameObjectsWithTag("PassivePool")[0];
                passivesPool.gameObject.GetComponent<Image>().enabled = true;

                for (int i = 0; i < passivesPool.transform.childCount; i++)
                {
                    passivesPool.transform.GetChild(i).gameObject.GetComponent<Image>().enabled = true;

                    for (int j = 0; j < passivesPool.transform.GetChild(i).childCount; j++)
                    {
                        passivesPool.transform.GetChild(i).GetChild(j).gameObject.SetActive(true);
                    }
                }

                // Carico dati su UI PASSIVE

                textNome1Passive.text = PassiveManager.Instance._passiveDatabase.passiveList[0].passiveName;
                textNome2Passive.text = PassiveManager.Instance._passiveDatabase.passiveList[1].passiveName;
                textNome3Passive.text = PassiveManager.Instance._passiveDatabase.passiveList[2].passiveName;
                textDesc1Passive.text = PassiveManager.Instance._passiveDatabase.passiveList[0].passiveDescription;
                textDesc2Passive.text = PassiveManager.Instance._passiveDatabase.passiveList[1].passiveDescription;
                textDesc3Passive.text = PassiveManager.Instance._passiveDatabase.passiveList[2].passiveDescription;
                sprite1Passive.sprite = PassiveManager.Instance._passiveDatabase.passiveList[0].passiveSprite;
                sprite2Passive.sprite = PassiveManager.Instance._passiveDatabase.passiveList[1].passiveSprite;
                sprite3Passive.sprite = PassiveManager.Instance._passiveDatabase.passiveList[2].passiveSprite;


                ActualLevelingPlayer1 = levelManager;
                for (int i = 0; i < 3; i++)
                {
                    int randomIndex = Random.Range(0, database.weaponDatabase.Count);
                    GameObject randomWeaponPrefab = database.weaponDatabase[randomIndex];
                    ActualWeaponPoolPlayer1[i] = randomWeaponPrefab;
                }

                // carico dati delle armi su UI
                if (!ActualWeaponPoolPlayer1[0].TryGetComponent(out BaseWeapon a))
                    textNome1P1.text = ActualWeaponPoolPlayer1[0].GetComponentInChildren<BaseWeapon>().name;
                else
                    textNome1P1.text = ActualWeaponPoolPlayer1[0].GetComponent<BaseWeapon>().name;

                if (!ActualWeaponPoolPlayer1[1].TryGetComponent(out BaseWeapon b))
                    textNome2P1.text = ActualWeaponPoolPlayer1[1].GetComponentInChildren<BaseWeapon>().name;
                else
                    textNome2P1.text = ActualWeaponPoolPlayer1[1].GetComponent<BaseWeapon>().name;

                if (!ActualWeaponPoolPlayer1[1].TryGetComponent(out BaseWeapon c))
                    textNome3P1.text = ActualWeaponPoolPlayer1[2].GetComponentInChildren<BaseWeapon>().name;
                else
                    textNome3P1.text = ActualWeaponPoolPlayer1[2].GetComponent<BaseWeapon>().name;

                textDesc1P1.text = ActualWeaponPoolPlayer1[0].GetComponent<BaseWeapon>().description;
                textDesc2P1.text = ActualWeaponPoolPlayer1[1].GetComponent<BaseWeapon>().description;
                textDesc3P1.text = ActualWeaponPoolPlayer1[2].GetComponent<BaseWeapon>().description;

                sprite1P1.sprite = ActualWeaponPoolPlayer1[0].GetComponent<BaseWeapon>().sprite;
                sprite2P1.sprite = ActualWeaponPoolPlayer1[1].GetComponent<BaseWeapon>().sprite;
                sprite3P1.sprite = ActualWeaponPoolPlayer1[2].GetComponent<BaseWeapon>().sprite;
            }
            else if (levelManager.GetComponentInParent<PlayerInput>().playerIndex == 1)
            {
                var weaponPoolPanel2 = GameObject.FindGameObjectsWithTag("Player2WeaponPool")[0];

                weaponPoolPanel2.gameObject.GetComponent<Image>().enabled = true;

                for (int i = 0; i < weaponPoolPanel2.transform.childCount; i++)
                {
                    weaponPoolPanel2.transform.GetChild(i).gameObject.GetComponent<Image>().enabled = true;

                    for (int j = 0; j < weaponPoolPanel2.transform.GetChild(i).childCount; j++)
                    {
                        weaponPoolPanel2.transform.GetChild(i).GetChild(j).gameObject.SetActive(true);
                    }
                }


                // PASSIVE ------------------------


                var passivesPool = GameObject.FindGameObjectsWithTag("PassivePool")[0];
                passivesPool.gameObject.GetComponent<Image>().enabled = true;

                for (int i = 0; i < passivesPool.transform.childCount; i++)
                {
                    passivesPool.transform.GetChild(i).gameObject.GetComponent<Image>().enabled = true;

                    for (int j = 0; j < passivesPool.transform.GetChild(i).childCount; j++)
                    {
                        passivesPool.transform.GetChild(i).GetChild(j).gameObject.SetActive(true);
                    }
                }

                // Carico dati su UI PASSIVE

                textNome1Passive.text = PassiveManager.Instance._passiveDatabase.passiveList[0].passiveName;
                textNome2Passive.text = PassiveManager.Instance._passiveDatabase.passiveList[1].passiveName;
                textNome3Passive.text = PassiveManager.Instance._passiveDatabase.passiveList[2].passiveName;
                textDesc1Passive.text = PassiveManager.Instance._passiveDatabase.passiveList[0].passiveDescription;
                textDesc2Passive.text = PassiveManager.Instance._passiveDatabase.passiveList[1].passiveDescription;
                textDesc3Passive.text = PassiveManager.Instance._passiveDatabase.passiveList[2].passiveDescription;
                sprite1Passive.sprite = PassiveManager.Instance._passiveDatabase.passiveList[0].passiveSprite;
                sprite2Passive.sprite = PassiveManager.Instance._passiveDatabase.passiveList[1].passiveSprite;
                sprite3Passive.sprite = PassiveManager.Instance._passiveDatabase.passiveList[2].passiveSprite;


                // PASSIVE ------------------------

                ActualLevelingPlayer2 = levelManager;

                for (int i = 0; i < 3; i++)
                {
                    int randomIndex = Random.Range(0, database.weaponDatabase.Count);
                    GameObject randomWeaponPrefab = database.weaponDatabase[randomIndex];
                    ActualWeaponPoolPlayer2[i] = randomWeaponPrefab;
                }

                // carico dati delle armi su UI
                
                if (ActualWeaponPoolPlayer2[0].GetComponent<BaseWeapon>() == null)
                    textNome1P2.text = ActualWeaponPoolPlayer2[0].GetComponentInChildren<BaseWeapon>().name;
                else
                    textNome1P2.text = ActualWeaponPoolPlayer2[0].GetComponent<BaseWeapon>().name;

                if (ActualWeaponPoolPlayer2[1].GetComponent<BaseWeapon>() == null)
                    textNome2P2.text = ActualWeaponPoolPlayer2[1].GetComponentInChildren<BaseWeapon>().name;
                else
                    textNome2P2.text = ActualWeaponPoolPlayer2[1].GetComponent<BaseWeapon>().name;
                
                if (ActualWeaponPoolPlayer2[2].GetComponent<BaseWeapon>() == null)
                    textNome3P2.text = ActualWeaponPoolPlayer2[2].GetComponentInChildren<BaseWeapon>().name;
                else
                    textNome3P2.text = ActualWeaponPoolPlayer2[2].GetComponent<BaseWeapon>().name;

                textNome2P2.text = ActualWeaponPoolPlayer2[1].GetComponent<BaseWeapon>().name;
                textNome3P2.text = ActualWeaponPoolPlayer2[2].GetComponent<BaseWeapon>().name;

                textDesc1P2.text = ActualWeaponPoolPlayer2[0].GetComponent<BaseWeapon>().description;
                textDesc2P2.text = ActualWeaponPoolPlayer2[1].GetComponent<BaseWeapon>().description;
                textDesc3P2.text = ActualWeaponPoolPlayer2[2].GetComponent<BaseWeapon>().description;

                sprite1P2.sprite = ActualWeaponPoolPlayer1[0].GetComponent<BaseWeapon>().sprite;
                sprite2P2.sprite = ActualWeaponPoolPlayer1[1].GetComponent<BaseWeapon>().sprite;
                sprite3P2.sprite = ActualWeaponPoolPlayer1[2].GetComponent<BaseWeapon>().sprite;
            }

        }
    }

    public void CleanActualPoolFirstPlayer(int Index)
    {
        List<BaseWeapon> playerWeapons = ActualLevelingPlayer1._playerHandler.GetEquippedWeapons();


        BaseWeapon existingWeapon = playerWeapons.Find(w => w.GetType() == ActualWeaponPoolPlayer1[Index].GetComponent<BaseWeapon>().GetType());

        if (existingWeapon != null)
        {
            existingWeapon.UpgradeTier();
            Debug.Log($"{existingWeapon.name} � stata potenziata!");
        }
        else
        {
            // Altrimenti, equipaggia la nuova arma
            GameObject instantiatedWeapon = Instantiate(ActualWeaponPoolPlayer1[Index]);

            // Aggiungi l'arma istanziata alla lista delle armi equipaggiate di PlayerHandler
            ActualLevelingPlayer1._playerHandler.EquipWeapon(instantiatedWeapon);

            Debug.Log($"{instantiatedWeapon.name} � stata equipaggiata!");
        }

        for (int i = 0; i < 3; i++)
        {
            ActualWeaponPoolPlayer1[i] = null;
        }

        ActualLevelingPlayer1 = null;

        var weaponPoolPanel1 = GameObject.FindGameObjectsWithTag("Player1WeaponPool")[0];

        weaponPoolPanel1.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
        weaponPoolPanel1.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
        weaponPoolPanel1.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
        weaponPoolPanel1.gameObject.GetComponent<Image>().enabled = false;
        weaponPoolPanel1.gameObject.GetComponent<WeaponPoolSelector>().enabled = false;


        var canvas = GameObject.FindGameObjectsWithTag("ChooseWeaponCanvas");
        var canvasScript = canvas[0].GetComponent<Canvas>();
        canvasScript.enabled = false;

        var pauseManager = FindAnyObjectByType<PauseManager>();


        if (pauseManager != null
        && ActualLevelingPlayer2 == null)
        {
            pauseManager.pauseAction.Disable();
            pauseManager.PauseAll();
        }
    }

    public void CleanActualPoolSecondPlayer(int Index)
    {
        List<BaseWeapon> playerWeapons = ActualLevelingPlayer2._playerHandler.GetEquippedWeapons();
        BaseWeapon existingWeapon = playerWeapons.Find(w => w.GetType() == ActualWeaponPoolPlayer2[Index].GetType());

        if (existingWeapon != null)
        {
            existingWeapon.UpgradeTier();
            Debug.Log($"{existingWeapon.name} � stata potenziata!");
        }
        else
        {
            // Altrimenti, equipaggia la nuova arma
            GameObject instantiatedWeapon = Instantiate(ActualWeaponPoolPlayer2[Index]);

            // Aggiungi l'arma istanziata alla lista delle armi equipaggiate di PlayerHandler
            ActualLevelingPlayer2._playerHandler.EquipWeapon(instantiatedWeapon);

            Debug.Log($"{instantiatedWeapon.name} � stata equipaggiata!");
        }

        for (int i = 0; i < 3; i++)
        {
            ActualWeaponPoolPlayer2[i] = null;
        }

        ActualLevelingPlayer2 = null;

        var weaponPoolPanel2 = GameObject.FindGameObjectsWithTag("Player1WeaponPool")[0];

        weaponPoolPanel2.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
        weaponPoolPanel2.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
        weaponPoolPanel2.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
        weaponPoolPanel2.gameObject.GetComponent<Image>().enabled = false;
        weaponPoolPanel2.gameObject.GetComponent<WeaponPoolSelector>().enabled = false;


        var canvas = GameObject.FindGameObjectsWithTag("ChooseWeaponCanvas");
        var canvasScript = canvas[0].GetComponent<Canvas>();
        canvasScript.enabled = false;

        var pauseManager = FindAnyObjectByType<PauseManager>();


        if (pauseManager != null
        && ActualLevelingPlayer1 == null)
        {
            pauseManager.pauseAction.Disable();
            pauseManager.PauseAll();
        }
    }

}
