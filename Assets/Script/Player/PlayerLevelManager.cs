using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLevelManager : MonoBehaviour
{
    [SerializeField] public int _level = 1;
    public int Level => _level;
    [SerializeField] private float _actualExp = 0f;
    public float ActualExp => _actualExp;
    [SerializeField] private float _grabExpRange = 1f;

    [SerializeField] private float _levelUpThreshold = 300f;
    [SerializeField] private float _trueLevelUpThreshold = 300f;
    [SerializeField] private float _thresholdMultiplayer = 1f;

    public PlayerHandler _playerHandler;
    private CircleCollider2D _collider;
    
    void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        EventManager.OnPlayerLevelUp += (level, manager) => { BarsUI.instance.SetExp((float)0, GetComponentInParent<PlayerInput>().playerIndex);
                                                              BarsUI.instance.SetMaxExp((float)_trueLevelUpThreshold, GetComponentInParent<PlayerInput>().playerIndex);
        };
        BarsUI.instance.SetMaxExp((float)_trueLevelUpThreshold, GetComponentInParent<PlayerInput>().playerIndex);
    }

    void Update()
    {
        _trueLevelUpThreshold = _levelUpThreshold * _level * _thresholdMultiplayer;
    }

    public void AddExp(float expToAdd)
    {
        _actualExp += expToAdd;

        BarsUI.instance.SetExp(_actualExp, GetComponentInParent<PlayerInput>().playerIndex);


        if (_actualExp >= _trueLevelUpThreshold)
        {
            _level++;
            _actualExp = 0f;

            //if (EventManager.OnPlayerLevelUp != null)
            //{
            //    Debug.Log($"Registered methods: {EventManager.OnPlayerLevelUp.GetInvocationList().Length}");
            //}
            EventManager.OnPlayerLevelUp?.Invoke(_level, this);

            //SI lo so che è sbagliato perché c'è l'evento sopra
            GameHUDmanager.instance.UpdateLvlText(this, _level);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent(out ExpItem expItem))
        {
            AddExp(expItem.EXPValue);
            Destroy(expItem.gameObject);
        }
    }

    public void ChangeGrabRange()
    {
        _collider.radius = _grabExpRange;
    }
}
