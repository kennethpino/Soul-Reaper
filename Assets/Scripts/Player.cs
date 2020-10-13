using UnityEngine;

public class Player : ScriptableObject
{
    //Will mark the person as being reaped
    public string Commend(Person person)
    {
        Debug.Log("***Commend***");
        person.PersonState.IsReaped = true;
        person.PersonState.IsBeingReaped = false;
        return person.CommendReaction;
    }

    //Will mark the person as being reaped
    public string Condemn(Person person)
    {
        Debug.Log("***Condemn***");
        person.PersonState.IsReaped = true;
        person.PersonState.IsBeingReaped = false;
        return person.CondemnReaction;
    }

    //Will set the status as ongoing reaping
    //The purpose of this middle stage is so that the player
    //can choose commend or condemning before reaping
    public string ReapSoul(Person person)
    {
        Debug.Log("***ReapSoul***");
        person.PersonState.IsBeingReaped = true;
        return string.Format("{0}'s fate rests in your hands.", person.PersonName);
    }

    //Gets the persons last words and enables reaping
    public string GetLastWords(Person person)
    {
        Debug.Log("***GetLastWords***");
        person.PersonState.LastWordsShared = true;
        return person.LastWords;
    }
}
