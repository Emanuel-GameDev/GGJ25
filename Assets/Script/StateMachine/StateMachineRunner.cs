using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Burst;
using UnityEngine;

namespace StateMachineSpace
{
    public class StateMachineRunner : MonoBehaviour
    {
        public StateMachineSchema Schema;

        private StateMachine _current;
        [SerializeField] private string _currentStateName;

        private CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();

        private void Awake()
        {
            _current = Schema.StateMachine.Clone(gameObject);
        }

        private void Start()
        {
            // UniTask.RunOnThreadPool(() => _current.Init(gameObject, _cancelTokenSource.Token));
            _current.Init(gameObject, _cancelTokenSource.Token).Forget();
        }

        void Update()
        {
            _currentStateName = _current.States[_current.CurrentState].GetType().Name;
        }

        /* //TODO
        ricordare che il reset verrà chiamato ogni volta che il player
        subirà una nuova modifica (aggiunta di abilità, ...) che riguarda i suoi componenti.
        Ho modificato la logica della state machine in modo che vada a prendersi i componenti
        solo quando viene clonato lo schema. Per Aumentare l'ottimizzazione.
        */
        [BurstCompile]
        public async UniTask StateMachineReset()
        {
            _cancelTokenSource.Cancel();
            _cancelTokenSource.Dispose();

            _cancelTokenSource = new CancellationTokenSource();
            
            _current = Schema.StateMachine.Clone(gameObject);
            _current.Init(gameObject, _cancelTokenSource.Token).Forget();

            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        void OnDestroy()
        {
            _cancelTokenSource.Cancel();
            _cancelTokenSource.Dispose();
            
        }
    }
}