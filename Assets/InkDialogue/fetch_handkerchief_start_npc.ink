=== fetchHandkerchiefStart ===
{ FetchHandkerchiefQuestState :
    - "REQUIREMENTS_NOT_MET": -> requirementsNotMet
    - "CAN_START": -> canStart
    - "IN_PROGRESS": -> inProgress
    - "CAN_FINISH": -> canFinish
    - "FINISHED": -> finished
    - else: -> END
}

= requirementsNotMet
Sorry sir. I do not know you.
-> END

= canStart
Will you fetch me my handkerchief and bring it back to me?
* [Yes]
    ~ StartQuest(FetchHandkerchiefQuestId)
    Many thanks good sir!
* [No]
    Oh, ok then. Come back if you change your mind.
- -> END

= inProgress
Hm? What do you want?
* [Nothing, I guess.]
    -> END
* { FetchHandkerchiefQuestState == "CAN_FINISH" } [Here is your handkerchief.]
    ~ FinishQuest(FetchHandkerchiefQuestId)
    By jove! You've found the blasted thing!
-> END

= canFinish
-> inProgress

= finished
Thanks for fetching my handkerchief, is there anything I could do for you?
-> END