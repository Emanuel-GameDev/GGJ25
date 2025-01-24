using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
    [SerializeField] private int _level = 1;
    public int Level => _level;
    [SerializeField] private float _exp = 0f;
    public float Exp => _exp;
    [SerializeField] private float _grabExpRange = 1f;

    [SerializeField] private float _levelUpThreshold = 300f;
    [SerializeField] private float _trueLevelUpThreshold = 300f;
    [SerializeField] private float _thresholdMultiplayer = 1.5f;

    void Update()
    {
        _trueLevelUpThreshold = _levelUpThreshold * _level * _thresholdMultiplayer;
    }

    public void AddExp(float expToAdd)
    {
        _exp += expToAdd;

        if (_exp >= _trueLevelUpThreshold)
        {
            _level++;
            _exp = 0f;

            EventManager.OnPlayerLevelUp?.Invoke(_level);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent(out ExpItem expItem))
        {
            AddExp(expItem.EXPValue);
        }
    }
}
