using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {
	public Dialogue dialogue;
    bool isPlaying = false;

	public void TriggerDialogue()
	{
		FindObjectOfType<DialogueManager> ().StartDialogue (dialogue);
        isPlaying = true;
	}

	public bool Next()
	{
		return FindObjectOfType<DialogueManager> ().DisplayNextSentence ();
	}

    public void EndDialogue()
    {
        FindObjectOfType<DialogueManager>().HideDialogue();
        isPlaying = false;
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }
}
