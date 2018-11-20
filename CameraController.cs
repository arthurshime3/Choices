using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public GameObject player;
	public float speed = 0.1f;
	public GameObject rightBarrier, leftBarrier, belowBarrier, aboveBarrier;

    private Vector3 offset, nextPosition, zoomedInOffset;

	void Start ()
	{
		transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, -90);
		offset = transform.position - player.transform.position;
		offset = new Vector3 (offset.x, offset.y + 3, offset.z);
        zoomedInOffset = new Vector3(offset.x, offset.y, offset.z + 10);
		nextPosition = Vector3.zero;

        if (player.transform.position.x >= rightBarrier.transform.position.x)
            nextPosition.x = rightBarrier.transform.position.x;
        else if (player.transform.position.x <= leftBarrier.transform.position.x)
            nextPosition.x = leftBarrier.transform.position.x;
        else
            nextPosition.x = player.transform.position.x;

        if (player.transform.position.y >= aboveBarrier.transform.position.y)
            nextPosition.y = aboveBarrier.transform.position.y;
        else if (player.transform.position.y <= belowBarrier.transform.position.y)
            nextPosition.y = belowBarrier.transform.position.y;
        else
            nextPosition.y = player.transform.position.y;
        transform.position = nextPosition + offset;
	}

	void LateUpdate ()
	{
//		camera.orthographicSize = (Screen.height / 100f) / 4f;
//		transform.position = player.transform.position + offset;

		nextPosition.z = player.transform.position.z;

		if (player.transform.position.x >= rightBarrier.transform.position.x)
			nextPosition.x = rightBarrier.transform.position.x;
		else if (player.transform.position.x <= leftBarrier.transform.position.x)
			nextPosition.x = leftBarrier.transform.position.x;
		else
			nextPosition.x = player.transform.position.x;

		if (player.transform.position.y >= aboveBarrier.transform.position.y)
			nextPosition.y = aboveBarrier.transform.position.y;
		else if (player.transform.position.y <= belowBarrier.transform.position.y)
			nextPosition.y = belowBarrier.transform.position.y;
		else
			nextPosition.y = player.transform.position.y;

        if (player.GetComponent<Player3D>().IsDashing())
            transform.position = Vector3.Lerp (transform.position, nextPosition + zoomedInOffset, speed);
        else
            transform.position = Vector3.Lerp(transform.position, nextPosition + offset, speed);
	}
}
