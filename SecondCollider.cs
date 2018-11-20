using UnityEngine;
using System.Collections;

public class SecondCollider : MonoBehaviour 
{
	bool touchingWall = false;

	public bool CollidingWall()
	{
		return touchingWall;
	}

	void Update()
	{
		
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.CompareTag("Wall")) 
		{
			touchingWall = true;
		}
		if (col.gameObject.CompareTag ("Floor")) 
		{
			touchingWall = false;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.CompareTag("Wall")) 
		{
			touchingWall = false;
		}
	}
}


