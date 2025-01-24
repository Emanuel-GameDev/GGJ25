using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    PlayerInputs playerInputs;
    InputAction pauseAction;

    [SerializeField] private bool inPause = false;
    void Awake()
    {
        playerInputs = new PlayerInputs();
        pauseAction = playerInputs.Player.Pause;
        pauseAction.Enable();

        pauseAction.performed += ctx => PauseAll();
    }

    public void PauseAll()
    {
        if(!inPause)
        {
            var pausableEnemyObjects = FindObjectsByType<AEnemy>(FindObjectsSortMode.None).ToList(); 
            foreach (var pausableEnemy in pausableEnemyObjects)
            {
                pausableEnemy.Pause();
            }

            var pausablePlayerObjects = FindObjectsByType<PlayerController>(FindObjectsSortMode.None).ToList();
            foreach (var pausablePlayer in pausablePlayerObjects)
            {
                pausablePlayer.Pause();
            }

            var pausableEnemySpawnerObjects = FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None).ToList();
            foreach (var pausableEnemySpawner in pausableEnemySpawnerObjects)
            {
                pausableEnemySpawner.Pause();
            } 

            inPause = true;
        }
        else
        {
            var pausableEnemyObjects = FindObjectsByType<AEnemy>(FindObjectsSortMode.None).ToList(); 
            foreach (var pausableEnemy in pausableEnemyObjects)
            {
                pausableEnemy.Unpause();
            }

            var pausablePlayerObjects = FindObjectsByType<PlayerController>(FindObjectsSortMode.None).ToList();
            foreach (var pausablePlayer in pausablePlayerObjects)
            {
                pausablePlayer.Unpause();
            }

            var pausableEnemySpawnerObjects = FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None).ToList();
            foreach (var pausableEnemySpawner in pausableEnemySpawnerObjects)
            {
                pausableEnemySpawner.Unpause();
            } 

            inPause = false;
        }
    }
}
