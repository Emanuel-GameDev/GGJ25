using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControllerAssignmentManager : DeviceManager
{
    [Header("UI References")]
    [Tooltip("Grid to add cards to")]
    [SerializeField] private GameObject _controllerGrid;

    [Header("Debug")]
    private static ControllerAssignmentManager _instance;
    public static ControllerAssignmentManager Instance => _instance;
    private PlayerInputs playerInputAsset;

    protected override void Awake()
    {
        DontDestroyOnLoad(gameObject);

        base.Awake();
        _instance = this;

        playerInputAsset = new PlayerInputs();
        playerInputAsset.ControllerAssignment.GoNext.performed += StartGame;
        playerInputAsset.Enable();
        
        SceneManager.sceneLoaded += SendPlayersData;
    }

    private void SendPlayersData(Scene scene, LoadSceneMode sceneMode)
    {
        //specificare la scena di assegnazione di controlli
        if (scene.name == "ControllerSelectionScene")
        {
            Debug.Log("Scene already loaded");
            return;

        }

        Debug.Log("SendPlayersData");

        ControllerPlayersManager.Instance.CopyPlayerInputManagerData(PlayerInputData.Clone(_playersData));
        
        Destroy(this.gameObject);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        onPlayerRemoved += RemoveCards;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        onPlayerRemoved -= RemoveCards;

        playerInputAsset.ControllerAssignment.GoNext.performed -= StartGame;
        playerInputAsset.Disable();

        SceneManager.sceneLoaded -= SendPlayersData;
    }

    protected override void AddPlayer(PlayerInput player)
    {
        // Debug.Log($"Adding player: {player.playerIndex}");

        if(_controllerGrid == null)
            return;

        base.AddPlayer(player);

        _playersData.Add(PlayerInputData.BuildPlayerData(player));

        AddCards(player);
    }

    protected override void RemovePlayer(PlayerInput player)
    {
        // Debug.Log($"Removing player: {player.playerIndex}");

        //base.RemovePlayer(player);

        // Debug.Log("Removing player: " + player.playerIndex);

        _playersData.Remove(PlayerInputData.BuildPlayerData(player));
    }

    private void AddCards(PlayerInput cardToAdd)
    {
        // Debug.Log($"Adding card: {cardToAdd.playerIndex}");
        cardToAdd.transform.SetParent(_controllerGrid.transform);
        cardToAdd.transform.localScale = new Vector3(1f, 1f, 1f);

        UICardUpdate(cardToAdd.gameObject);
    }

    private void UICardUpdate(GameObject cardObj)
    {
        // Debug.Log($"Modifying card: {cardObj.name}");
        Image background = cardObj.GetComponent<Image>();
        background.color = 
            new Color(  
                        Random.Range(0f, 1f), 
                        Random.Range(0f, 1f), 
                        Random.Range(0f, 1f)
                    );
    
        TextMeshProUGUI text = cardObj.GetComponentInChildren<TextMeshProUGUI>();
        text.text = 
            $"Player N. {cardObj.GetComponent<PlayerInput>().playerIndex} \n {cardObj.GetComponent<PlayerInput>().devices[0].name}";
    
        Image icon = cardObj.GetComponentInChildren<Image>();
        //TODO: Add icon
    }

    private void RemoveCards(PlayerInput cardToRemove)
    {
        _controllerGrid.GetComponentsInChildren<PlayerInput>()
                        .ToList()
                        .ForEach(x => 
                        {
                            if(x == cardToRemove)
                            { 
                                Destroy(x.gameObject);
                                return;
                            }
                        });
    }

    private void StartGame(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("GameScene");
    }
}
