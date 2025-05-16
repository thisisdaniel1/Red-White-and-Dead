using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class FetchHandkerchiefQuestStep : QuestStep
{
    [Header("Config")]
    [SerializeField] private string startText = "blank";
    [SerializeField] private string finishText = "blank";

    private void Start()
    {
        string status = startText;
        ChangeState("", status);
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            string status = finishText;
            ChangeState("", status);
            FinishQuestStep();
        }
    }

    protected override void SetQuestStepState(string state)
    {
        // no state is needed for this quest step
    }
}
