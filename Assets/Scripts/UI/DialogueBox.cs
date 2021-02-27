using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    public Dialogue[] Dialogues;

    public Text Text;
    public GameObject Holder;

    private int dialogueIndex = -1;
    private int speachIndex = 0;

    private Dialogue currentDialogue;
    private float inputDelay;
    public void ActiveDialoge()
    {
        dialogueIndex++;

        if(dialogueIndex >= Dialogues.Length)
        {
            return;
        }

        speachIndex = 0;
        currentDialogue = Dialogues[dialogueIndex];
        inputDelay = 0.25f;
        Text.text = currentDialogue.Speach[speachIndex];
        Holder.SetActive(true);

    }

    public bool FreezePlayer()
    {
        if (currentDialogue != null)
            return currentDialogue.FreezePlayer;
        else
            return false;
    }

    private void Update()
    {
        if (inputDelay > 0)
            inputDelay -= Time.deltaTime;
    }

    public void NextDialogue()
    {
        if (currentDialogue == null)
            return;

        if (inputDelay <= 0)
        {
            speachIndex++;
            inputDelay = 0.25f;

            if (speachIndex >= currentDialogue.Speach.Length)
            {
                EndDialog();
                return;
            }

            Text.text = currentDialogue.Speach[speachIndex];
        }
    }

    public void EndDialog()
    {
        Text.text = "";
        currentDialogue = null;
        Holder.SetActive(false);
    }
}


[System.Serializable]
public class Dialogue
{
    public bool FreezePlayer;
    public string[] Speach;
}

