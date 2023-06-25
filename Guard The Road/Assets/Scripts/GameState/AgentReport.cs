using System.Collections.Generic;

public class AgentReport
{
    public List<AgentObserver> friends;
    public List<AgentObserver> foes;

    public List<float> friendDistances;
    public List<float> foeDistances;

    public AgentReport()
    {
        friends = new List<AgentObserver>();
        foes = new List<AgentObserver>();

        friendDistances = new List<float>();
        foeDistances = new List<float>();

    }


    
}
