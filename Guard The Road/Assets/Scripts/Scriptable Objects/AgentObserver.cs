using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Agent Observer")]
public class AgentObserver : Observer
{
    public const int HUMAN = 0;
    public const int GOBLIN = 1;
    public string agentName;
    public int team;
    public Vector3 position;
    public AgentObserver target;

    public void Hit(AgentObserver attacker, WeaponObserver weapon)
    {
        Debug.Log(agentName + " was hit by " + attacker.agentName);
    }

    public AgentReport FindAgents(float maximumDistance)
    {
        return GameState.instance.FindAgents(this, maximumDistance);
    }

    public override Observer Clone()
    {
        AgentObserver clone = ScriptableObject.CreateInstance<AgentObserver>();

        clone.target = target;
        clone.position = position;

        return clone;
    }

    public override void Inject(GameObject agent)
    {
        IAgentObserver[] slots = agent.GetComponentsInChildren<IAgentObserver>();

        foreach(IAgentObserver slot in slots)
        {
            slot.Agent = this;
        }
    }

}
