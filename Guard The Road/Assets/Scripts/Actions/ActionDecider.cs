using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDecider : MonoBehaviour, IActionObserver, IAgentObserver
{
    [SerializeField] private ActionObserver _action;

    public ActionObserver Action{
        get=>_action;
        set=>_action = value;
    }

    [SerializeField] private AgentObserver _agent;
    public AgentObserver Agent{
        get=>_agent;
        set=>_agent = value;
    }

    [SerializeField] private ActionDefinition _follow;
    [SerializeField] private ActionDefinition _attack;
    [SerializeField] private ActionDefinition _flee;
    [SerializeField] private float _meanDecisionTime = 1f;
    [SerializeField] private float _observationRange = 4f;

    private bool _isDeciding = true;

    void Start()
    {
        StartCoroutine(Decide());
    }

    IEnumerator Decide()
    {
        AgentReport report;

        while(_isDeciding)
        {
            
            report = _agent.FindAgents(_observationRange);

            if(report.foes.Count > 0 && !_attack.IsActing){
                    _action.Interrupt(_attack);
                    _action.Enqueue(_follow);
            } else {
                _action.Enqueue(_follow);
            }
                

            yield return new WaitForSeconds(Random.Range(0.9f * _meanDecisionTime, 1.1f * _meanDecisionTime));

        }

    }


   
}
