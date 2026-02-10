using Fire_Pixel.Networking;
using UnityEngine;


public class SkillUIHandler : MonoBehaviour
{
#pragma warning disable UDR0001
    private static SkillUIBlock[] skillUIBlocks;
    private static TooltipHandler toolTipHandler;
#pragma warning restore UDR0001


    private void Awake()
    {
        skillUIBlocks = GetComponentsInChildren<SkillUIBlock>(true);
        toolTipHandler = GetComponent<TooltipHandler>();

        UpdateSkillUIActiveState(TurnState.Ended);

        TurnManager.TurnChanged += OnGameStart;
        TurnManager.TurnStateChanged += UpdateSkillUIActiveState;
    }
    private void OnGameStart(int clientOnTurnGameId)
    {
        TurnState turnState = clientOnTurnGameId == ClientManager.LocalClientGameId ?
            TurnState.Started :
            TurnState.Ended;

        UpdateSkillUIActiveState(turnState);
        TurnManager.TurnChanged -= OnGameStart;
    }


    private void UpdateSkillUIActiveState(TurnState state)
    {
        bool isActive = state == TurnState.Started;

        int skillCount = skillUIBlocks.Length;
        for (int i = 0; i < skillCount; i++)
        {
            skillUIBlocks[i].UpdateSkillActiveState(isActive);
        }
    }

    public static void UpdateSkillUI(SkillSet skillSet)
    {
        int skillCount = skillSet.Length;
        for (int i = 0; i < skillCount; i++)
        {
            skillUIBlocks[i].UpdateUI(skillSet[i]);
        }
        // Update tooltip systems
        toolTipHandler.UpdateColoredWords();
    }

    private void OnDestroy()
    {
        TurnManager.TurnStateChanged -= UpdateSkillUIActiveState;
        TurnManager.TurnChanged -= OnGameStart;
    }
}
