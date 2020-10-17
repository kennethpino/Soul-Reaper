using System;
using UnityEngine;

public class Player : ScriptableObject
{
    public int SoulsReaped { get; set; } = 0;

    //Will mark the person as being reaped
    public string Reward(Person person)
    {
        Debug.Log("***Reward***");
        person.PersonState.IsReaped = true;
        SoulsReaped++;
        person.PersonState.IsBeingReaped = false;
        person.PersonState.IsRewarded = true;
        return person.RewardRaction;
    }

    //Will mark the person as being reaped
    public string Condemn(Person person)
    {
        Debug.Log("***Condemn***");
        person.PersonState.IsReaped = true;
        SoulsReaped++;
        person.PersonState.IsBeingReaped = false;
        person.PersonState.IsCondemned = true;
        return person.CondemnReaction;
    }

    //Will set the status as ongoing reaping
    //The purpose of this middle stage is so that the player
    //can choose reward or condemning before reaping
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
