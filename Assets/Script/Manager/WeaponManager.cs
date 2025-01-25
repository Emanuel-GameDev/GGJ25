using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public WeaponDatabase database;
    public static WeaponManager Instance;

    public GameObject[] ActualWeaponPoolPlayer1 = new GameObject[3];
    public GameObject[] ActualWeaponPoolPlayer2 = new GameObject[3];
    public PlayerLevelManager ActualLevelingPlayer1;
    public PlayerLevelManager ActualLevelingPlayer2;

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
        if(ControllerPlayersManager.Instance.Players.Count == 1 && level%5 == 0)
            return;
        else if(ControllerPlayersManager.Instance.Players.Count == 2 )
        {
            var levelSomma = ControllerPlayersManager.Instance.Players[0].gameObject.GetComponent<PlayerLevelManager>().Level 
            + ControllerPlayersManager.Instance.Players[1].gameObject.GetComponent<PlayerLevelManager>().Level;

            if(levelSomma%5 == 0)
                return;
        } 

        var canvas = GameObject.FindGameObjectsWithTag("ChooseWeaponCanvas");
        var pauseManager = FindAnyObjectByType<PauseManager>();
        
        if(pauseManager != null)
        {
            pauseManager.pauseAction.Disable();
            pauseManager.PauseAll();
        }

        if(canvas.Length > 0)
        {
            var canvasScript = canvas[0].GetComponent<Canvas>();
            canvasScript.enabled = true;

            var playerControllerRef = levelManager.gameObject.GetComponentInParent<PlayerController>();
            // Debug.Log("PlayerID: " + playerControllerRef.PlayerID);
            if(playerControllerRef.gameObject == ControllerPlayersManager.Instance.Players[0].gameObject)
            {
                var weaponPoolPanel1 = GameObject.FindGameObjectsWithTag("Player1WeaponPool")[0];
                
                weaponPoolPanel1.gameObject.GetComponent<Image>().enabled = true;
                weaponPoolPanel1.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                weaponPoolPanel1.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true;
                weaponPoolPanel1.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = true;
                weaponPoolPanel1.gameObject.GetComponent<WeaponPoolSelector>().enabled = true;


                // if()
                // weaponPoolPanel2.gameObject.GetComponent<Image>().enabled = false;
                // weaponPoolPanel2.transform.GetChild()

                var passivesPool = GameObject.FindGameObjectsWithTag("PassivePool")[0];
                passivesPool.gameObject.GetComponent<Image>().enabled = false;
                passivesPool.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
                passivesPool.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
                passivesPool.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = false;

                ActualLevelingPlayer1 = levelManager;
                for(int i = 0; i < 3; i++)
                {
                    int randomIndex = Random.Range(0, database.weaponDatabase.Count);
                    GameObject randomWeaponPrefab = database.weaponDatabase[randomIndex];
                    ActualWeaponPoolPlayer1[i] = randomWeaponPrefab;
                }
            }
            else if(ControllerPlayersManager.Instance.Players.Count > 1 
                    && playerControllerRef.gameObject == ControllerPlayersManager.Instance.Players[1].gameObject)
            {
                var weaponPoolPanel2 = GameObject.FindGameObjectsWithTag("Player2WeaponPool")[0];

                weaponPoolPanel2.gameObject.GetComponent<Image>().enabled = true;
                weaponPoolPanel2.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                weaponPoolPanel2.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true;
                weaponPoolPanel2.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = true;
                weaponPoolPanel2.gameObject.GetComponent<WeaponPoolSelector>().enabled = true;

                

                var passivesPool = GameObject.FindGameObjectsWithTag("PassivePool")[0];
                passivesPool.gameObject.GetComponent<Image>().enabled = false;
                passivesPool.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
                passivesPool.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
                passivesPool.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = false;

                ActualLevelingPlayer2 = levelManager;

                for(int i = 0; i < 3; i++)
                {
                    int randomIndex = Random.Range(0, database.weaponDatabase.Count);
                    GameObject randomWeaponPrefab = database.weaponDatabase[randomIndex];
                    ActualWeaponPoolPlayer2[i] = randomWeaponPrefab;
                }
            }

            //TODO 
            //Poolla 3 armi
            //Sostituisci le immagini di base con le icone delle armi
            //Script che aumenta e dimnuisce un indice per la scelta dell'arma
            //Premi tasto per assegnare l'arma scelta al player 

            
        }

        
    }

    public void CleanActualPoolFirstPlayer(int Index)
    {
        List<BaseWeapon> playerWeapons = ActualLevelingPlayer1._playerHandler.GetEquippedWeapons();

        foreach (BaseWeapon weapon in playerWeapons)
        {
            Debug.Log("player 1 equipped weapon: " + weapon);
        }

        foreach (GameObject weapon in ActualWeaponPoolPlayer1)
        {
            Debug.Log("player 1 database weapon: " + weapon);
        }

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

        for(int i = 0; i < 3; i++)
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
        
        if(pauseManager != null
        && ActualLevelingPlayer2 == null)
        {
            pauseManager.pauseAction.Disable();
            pauseManager.PauseAll();
        }
    }

    public void CleanActualPoolSecondPlayer(int Index)
    {
        List<BaseWeapon> playerWeapons = ActualLevelingPlayer2._playerHandler.GetEquippedWeapons();
        BaseWeapon existingWeapon = playerWeapons.Find(w => w.name == ActualWeaponPoolPlayer2[Index].name);

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

        for(int i = 0; i < 3; i++)
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
        
        if(pauseManager != null
        && ActualLevelingPlayer1 == null)
        {
            pauseManager.pauseAction.Disable();
            pauseManager.PauseAll();
        }
    }

}
