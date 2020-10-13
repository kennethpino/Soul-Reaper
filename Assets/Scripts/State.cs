using UnityEngine;

[CreateAssetMenu(menuName = "State")]
public class State : ScriptableObject
{

    [TextArea(14, 10)] [SerializeField] string gameStory = default;
    [SerializeField] private State[] states = default;
    [SerializeField] private string location = default;
    [SerializeField] private Person[] people = default;

    public Person[] GetPeople()
    {
        return people;
    }

    public void SetPeople(Person[] value)
    {
        people = value;
    }

    public string GetGameStory()
    {
        return gameStory;
    }

    public State[] GetStates()
    {
        return states;
    }

    public string GetLocationName()
    {
        return location;
    }
}
