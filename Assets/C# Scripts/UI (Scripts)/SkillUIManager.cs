using Fire_Pixel.Networking;
using UnityEngine;


public class SkillUIManager : MonoBehaviour
{
#pragma warning disable UDR0001
    private static SkillUIBlock[] skillUIBlocks;
    private static TooltipHandler toolTipHandler;
#pragma warning restore UDR0001


    private void Awake()
    {
        skillUIBlocks = GetComponentsInChildren<SkillUIBlock>(true);
        toolTipHandler = GetComponent<TooltipHandler>();

        TurnManager.TurnChanged += OnGameStart;
        TurnManager.TurnStateChanged += UpdateSkillUIActiveState;
    }
    private void OnGameStart(int clientOnTurnGameId)
    {
        TurnManager.TurnChanged -= OnGameStart;

        int skillSlotCount = skillUIBlocks.Length;
        for (int i = 0; i < skillSlotCount; i++)
        {
            skillUIBlocks[i].Init();
        }

        TurnState turnState = clientOnTurnGameId == ClientManager.LocalClientGameId ?
            TurnState.Started :
            TurnState.Ended;

        UpdateSkillUIActiveState(turnState);
    }


    private void UpdateSkillUIActiveState(TurnState state)
    {
        bool isActive = state == TurnState.Started;

        int skillSlotCount = skillUIBlocks.Length;
        for (int i = 0; i < skillSlotCount; i++)
        {
            skillUIBlocks[i].UpdateSkillActiveState(isActive);
        }
    }

    public static void UpdateSkillUI(SkillSet skillSet)
    {
        int skillSlotCount = skillUIBlocks.Length;
        if (skillSet.Length > skillSlotCount)
        {
            // Randomize order so a weapon with more skills then there are skillslots, chooses random skills to fill the slots
            skillSet.RandomizeSkillOrder();
        }

        for (int i = 0; i < skillSlotCount; i++)
        {
            skillUIBlocks[i].UpdateUI(skillSet[i]);
        }
        // Update tooltip systems
        toolTipHandler.UpdateColoredWords();
    }

    private void OnDestroy()
    {
        TurnManager.TurnChanged -= OnGameStart;
        TurnManager.TurnStateChanged -= UpdateSkillUIActiveState;
    }
}
