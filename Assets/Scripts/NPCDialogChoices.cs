﻿
static class NPCDialogChoices
{
    private const string GOBACK = "Go back.";
    private const string LASTWORDS = "Any last words?";
    private const string REAPSOUL = "Reap soul.";
    private const string COMMEND = "Commend!";
    private const string CONDEMN = "Condemn...";

    public static string GetText(this NPCDialogChoicesEnum dialogChoice)
    {
        switch (dialogChoice)
        {
            case NPCDialogChoicesEnum.GoBack:
                return GOBACK;
            case NPCDialogChoicesEnum.LastWords:
                return LASTWORDS;
            case NPCDialogChoicesEnum.ReapSoul:
                return REAPSOUL;
            case NPCDialogChoicesEnum.Commend:
                return COMMEND;
            case NPCDialogChoicesEnum.Condemn:
                return CONDEMN;
            default:
                return null;
        }
    }
}
