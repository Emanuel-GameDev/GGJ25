using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveDatabase", menuName = "ScriptableObjects/PassiveDatabase")]
public class PassiveDatabase : ScriptableObject
{
    public List<BasePassive> passiveList;
}
