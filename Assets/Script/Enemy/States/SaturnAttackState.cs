using System;
using Cysharp.Threading.Tasks;
using StateMachineSpace;
using UnityEngine;

[Serializable]
public class SaturnAttackState : State
{
    public GameObject projectilePrefab;
    public float force;

    public float cooldown;

    public override UniTask OnEnter(GameObject agent)
    {
        return UniTask.CompletedTask;
    }

    public async override UniTask OnUpdate(GameObject agent)
    {

        var projectileInst = GameObject.Instantiate(projectilePrefab, agent.transform.position, Quaternion.identity);
        projectileInst.GetComponent<Rigidbody2D>().AddForce((agent.transform.position - BubbleController.Instance.transform.position) * force, ForceMode2D.Impulse);
        
        await UniTask.Delay((int)(cooldown * 1000));
        
        await base.OnUpdate(agent);
    }

    public override UniTask OnExit(GameObject agent)
    {
        return base.OnExit(agent);
    }

    public override void OnClone(ref State newObject, GameObject agent)
    {
        base.OnClone(ref newObject, agent);
        ((SaturnAttackState)newObject).projectilePrefab = projectilePrefab;
        ((SaturnAttackState)newObject).force = force;
        ((SaturnAttackState)newObject).cooldown = cooldown;
    }
}
