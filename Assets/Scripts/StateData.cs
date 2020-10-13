using System;

[Serializable]
public struct StateData
{
    public string stateName;
    public string stateLocation;
    public string stateText;

    public StateData(string name, string location, string gameText)
    {
        stateName = name;
        stateLocation = location;
        stateText = gameText;
    }
}