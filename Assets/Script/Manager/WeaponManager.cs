using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public WeaponDatabase database;
    public static WeaponManager instance;

    public GameObject[] ActualWeaponPool = new GameObject[3];
    public PlayerLevelManager ActualLevelingPlayer;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else 
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        EventManager.OnPlayerLevelUp += SetUpChooseWeaponCanvas;
    }

    private void OnDisable()
    {
        // EventManager.OnPlayerLevelUp -= EquipRandomWeapon;
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
            Debug.Log("PlayerID: " + playerControllerRef.PlayerID);
            if(playerControllerRef.gameObject == ControllerPlayersManager.Instance.Players[0].gameObject)
            {
                var weaponPoolPanel1 = GameObject.FindGameObjectsWithTag("Player1WeaponPool")[0];
                var weaponPoolPanel2 = GameObject.FindGameObjectsWithTag("Player2WeaponPool")[0];
                weaponPoolPanel1.SetActive(true);
                weaponPoolPanel2.SetActive(false);

                var passivesPool = GameObject.FindGameObjectsWithTag("PassivePool")[0];
                passivesPool.SetActive(false);
            }
            else if(ControllerPlayersManager.Instance.Players.Count > 1 && playerControllerRef.gameObject == ControllerPlayersManager.Instance.Players[1].gameObject)
            {
                var weaponPoolPanel1 = GameObject.FindGameObjectsWithTag("Player1WeaponPool")[0];
                var weaponPoolPanel2 = GameObject.FindGameObjectsWithTag("Player2WeaponPool")[0];
                weaponPoolPanel1.SetActive(false);
                weaponPoolPanel2.SetActive(true);

                var passivesPool = GameObject.FindGameObjectsWithTag("PassivePool")[0];
                passivesPool.SetActive(false);
            }

            //TODO 
            //Poolla 3 armi
            //Sostituisci le immagini di base con le icone delle armi
            //Script che aumenta e dimnuisce un indice per la scelta dell'arma
            //Premi tasto per assegnare l'arma scelta al player 

            ActualLevelingPlayer = levelManager;

            for(int i = 0; i < 3; i++)
            {
                int randomIndex = Random.Range(0, database.weaponDatabase.Count);
                GameObject randomWeaponPrefab = database.weaponDatabase[randomIndex];
                ActualWeaponPool[i] = randomWeaponPrefab;
            }
        }

        
    }

    public void CleanActualPool(int Index)
    {

        List<BaseWeapon> playerWeapons = ActualLevelingPlayer._playerHandler.GetEquippedWeapons();
        BaseWeapon existingWeapon = playerWeapons.Find(w => w.GetType() == ActualWeaponPool[Index].GetType());

        if (existingWeapon != null)
            {
                // Se l'arma � gi� equipaggiata, potenziala
                existingWeapon.UpgradeTier();
                Debug.Log($"{existingWeapon.name} � stata potenziata!");
            }
            else
            {
                // Altrimenti, equipaggia la nuova arma
                GameObject instantiatedWeapon = Instantiate(ActualWeaponPool[Index]);

                // Aggiungi l'arma istanziata alla lista delle armi equipaggiate di PlayerHandler
                ActualLevelingPlayer._playerHandler.EquipWeapon(instantiatedWeapon);

                Debug.Log($"{instantiatedWeapon.name} � stata equipaggiata!");
            }

        for(int i = 0; i < 3; i++)
        {
            ActualWeaponPool[i] = null;
        }

        ActualLevelingPlayer = null;

        var canvas = GameObject.FindGameObjectsWithTag("ChooseWeaponCanvas");
        canvas[0].SetActive(false);

        var pauseManager = FindAnyObjectByType<PauseManager>();
        
        if(pauseManager != null)
        {
            pauseManager.pauseAction.Disable();
            pauseManager.PauseAll();
        }
    }

}
