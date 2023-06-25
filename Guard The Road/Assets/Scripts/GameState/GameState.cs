using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState instance;

    private List<AgentObserver> _agents;

    void Awake()
    {
        if(instance == null){
            instance = this;
        }
    }

    void Start()
    {
        // find all agents that are currently in the scene
        ObserverManager[] observers = GameObject.FindObjectsByType<ObserverManager>(FindObjectsSortMode.None);
        _agents = new List<AgentObserver>();

        foreach(ObserverManager observer in observers)
        {
            _agents.Add(observer.Agent);
        }
    }

    public AgentReport FindAgents(AgentObserver investigator, float maximumDistance)
    {
        AgentReport report = new AgentReport();

        foreach(AgentObserver agent in _agents)
        {
            if(investigator.team == agent.team){
                AddAgentToReport(agent, ref report.friends, ref report.friendDistances, investigator.position, maximumDistance);
            } else {
                AddAgentToReport(agent, ref report.foes, ref report.foeDistances, investigator.position, maximumDistance);
            }
        }
        return report;
    }

    private void AddAgentToReport(AgentObserver agent, ref List<AgentObserver> report, ref List<float> reportedDistances, Vector3 location, float maximumDistance)
    {
        float distanceToAgent = (location - agent.position).magnitude;

        if(distanceToAgent <= maximumDistance && distanceToAgent > 0){
            report.Add(agent);
            reportedDistances.Add(distanceToAgent);
        }
    } 

}
