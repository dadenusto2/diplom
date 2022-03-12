using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	[SerializeField]private float speed;
	[SerializeField]private float xSensitivity;
	[SerializeField]private float ySensitivity;

	void Update ()
	{
		float x = Input.GetAxis ("Horizontal") * speed * Time.deltaTime;
		float z = Input.GetAxis ("Vertical") * speed * Time.deltaTime;
		float y = 0;
		if (Input.GetKey (KeyCode.Space))
			y += 1 * speed * Time.deltaTime;
		if (Input.GetKey (KeyCode.LeftShift))
			y -= 1 * speed * Time.deltaTime;

		transform.Translate (new Vector3 (x, y, z));

		float X = Input.GetAxis ("Mouse X") * xSensitivity;
		float Y = -Input.GetAxis ("Mouse Y") * ySensitivity;

		transform.Rotate (new Vector3 (Y, X, 0));
		Quaternion r = transform.rotation;
		transform.rotation = Quaternion.Euler (new Vector3 (r.eulerAngles.x, r.eulerAngles.y, 0));
	}
}
