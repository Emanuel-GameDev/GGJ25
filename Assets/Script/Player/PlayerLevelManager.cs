using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
    [SerializeField] private int _level = 1;
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

    void Update()
    {
        _trueLevelUpThreshold = _levelUpThreshold * _level * _thresholdMultiplayer;
    }

    public void AddExp(float expToAdd)
    {
        _actualExp += expToAdd;

        if (_actualExp >= _trueLevelUpThreshold)
        {
            _level++;
            _actualExp = 0f;

            //if (EventManager.OnPlayerLevelUp != null)
            //{
            //    Debug.Log($"Registered methods: {EventManager.OnPlayerLevelUp.GetInvocationList().Length}");
            //}

            EventManager.OnPlayerLevelUp?.Invoke(_level, this);
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
