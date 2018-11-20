using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour {
	public GameObject[] scenes;
	int sceneNum;
    bool playing, finished;

	void Start () {
		sceneNum = 0;
        playing = false;
        finished = false;

		foreach (GameObject obj in scenes)
			obj.GetComponent<CutsceneFrameController> ().Clear ();

		Play();
	}
	
	void Update () {
		if (playing) 
		{
			if (scenes [sceneNum].GetComponent<CutsceneFrameController> ().numOfChoices > 1) 
			{
				if (scenes [sceneNum].GetComponent<CutsceneFrameController> ().choiceNum == -1) 
				{
					if (Input.GetKeyDown (KeyCode.A)) 
					{
						if (IsValidTransition (sceneNum + 1, "A"))
							NextFrame (1);
					}	
					if (Input.GetKeyDown (KeyCode.D)) 
					{
						if (scenes [sceneNum].GetComponent<CutsceneFrameController> ().numOfChoices > 2) 
						{
							if (IsValidTransition (sceneNum + 3, "D"))
								NextFrame (3);
						} 
						else 
						{
							if (IsValidTransition (sceneNum + 2, "D"))
								NextFrame (2);
						}
					}
					if (Input.GetKeyDown (KeyCode.S)) 
					{
						if (scenes [sceneNum].GetComponent<CutsceneFrameController> ().numOfChoices > 2) 
							if (IsValidTransition (sceneNum + 2, "S"))
								NextFrame (2);
					}
				} 
				else 
				{
                    if (Input.GetKeyDown (KeyCode.Return)) 
					{
						if (IsValidTransition (sceneNum 
								+ (scenes [sceneNum].GetComponent<CutsceneFrameController> ().numOfChoices - scenes [sceneNum].GetComponent<CutsceneFrameController> ().choiceNum), 
								"Enter"))
							NextFrame (scenes [sceneNum].GetComponent<CutsceneFrameController> ().numOfChoices - scenes [sceneNum].GetComponent<CutsceneFrameController> ().choiceNum);
					}
				}
			} 
			else 
			{
                if (Input.GetKeyDown (KeyCode.Return)) 
				{
					//if (IsValidTransition(sceneNum + 1, "Enter")) 
					//{
					NextFrame (1);
					//}
				}
			}
		}
	}

	void NextFrame(int n)
	{
        if (finished)
        {
            Exit();
            return;
        }
		scenes [sceneNum].GetComponent<CutsceneFrameController> ().FadeOut ();
		sceneNum += n;
		scenes [sceneNum].GetComponent<CutsceneFrameController> ().FadeIn ();
        if (scenes[sceneNum].GetComponent<CutsceneFrameController>().lastCutscene)
            finished = true;
	}

	bool IsValidTransition(int sceneTarget, string transitionKey)
	{
		return scenes [sceneTarget].GetComponent<CutsceneFrameController> ().transitionKey.Equals (transitionKey);
	}

	public void Play()
	{
		playing = true;
		scenes [sceneNum].GetComponent<CutsceneFrameController> ().FadeIn ();
	}

	public void SetSceneNum (int n)
	{
		sceneNum = n;
	}

    void Exit()
    {
        GlobalController.instance.ExitCutscene();
    }
}
