using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
	public Text nameText, dialogueText;
	public Animator animator;
	Queue<string> sentences;

	// Use this for initialization
	void Start () {
		sentences = new Queue<string> ();
	}
	
	public void StartDialogue(Dialogue dialogue)
	{
		animator.SetBool ("isOpen", true);

		nameText.text = dialogue.name;
		sentences.Clear ();

		foreach (string sentence in dialogue.sentences) 
		{
			sentences.Enqueue (sentence);
		}

		DisplayNextSentence ();
	}

	public bool DisplayNextSentence()
	{
		if (sentences.Count == 0) 
		{
			return false;
		}

		string sentence = sentences.Dequeue ();
		StopAllCoroutines ();
		StartCoroutine (TypeSentence (sentence));
		return true;
	}

	IEnumerator TypeSentence (string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence) 
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	public void HideDialogue()
	{
		animator.SetBool ("isOpen", false);
	}
}
