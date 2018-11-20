using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : RaycastController {
    public CollisionInfo collisions;
    public GameObject shadow;
    Vector3 maxSize;
    RaycastHit hit;
    bool withinRange;

    new void Start()
	{
        maxSize = shadow.transform.localScale;
        //shadow.transform.localScale = Vector3.zero;
        withinRange = true;
	}

	void Update()
	{
        //shadow.transform.localScale -= new Vector3(1f, 0f, 1f) * Time.deltaTime;
        withinRange = false;
        VerticalCollisions();
        if (withinRange)
            shadow.transform.localScale = maxSize / hit.distance;
        else
            shadow.transform.localScale = Vector3.zero;

        if (shadow.transform.localScale.magnitude > maxSize.magnitude)
            shadow.transform.localScale = maxSize;

        //print(shadow.transform.localScale);
	}

    void VerticalCollisions()
    {
        float rayLength = 50f;
        Vector3 rayOrigin = shadow.transform.position;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayLength, collisionMask))
        {
            withinRange = true;
        }
    }

	public struct CollisionInfo
	{
		public bool above, below, left, right;

		public void Reset()
		{
			above = below = left = right = false;
		}
	}
}
