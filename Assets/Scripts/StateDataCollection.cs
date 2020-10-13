using System;
using System.Collections.Generic;

[Serializable]
public class StateDataCollection
{
    public List<StateData> stateDatas;

    public StateDataCollection() => stateDatas = new List<StateData>();
}