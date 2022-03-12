using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	public LayerMask layerMask;
	public GameObject camara;
	public GameObject camRot;
	public float distance;
	public float zoomSpeed, smoothSpeed, mouseRotationSpeed, camHeight, camMoveSpeed, moveSmoothSpeed;

	private Vector3 camLocalPosition;
	private Vector3 pos;
	private bool followSim;
	private float xMouse, yMouse;
	private Ray ray;
	private RaycastHit hit;
	private Vector3 savedMousePosition;
	private bool fix;

	void Update ()
	{
		CamCotrol(10, 350, 14, 0);
	}

	void CamCotrol (int zoomInLimit, int zoomOutLimit, int moveSpeedDivision, int lowerRotLimit) {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		distance -= Input.GetAxis ("Mouse ScrollWheel") * (-camara.transform.localPosition.z * 32) * Time.unscaledDeltaTime;//Mathf.Abs(camRot.transform.position.z - camara.transform.localPosition.z);
		distance = Mathf.Clamp (distance, zoomInLimit, zoomOutLimit);
		camara.transform.localPosition = Vector3.Lerp(camara.transform.localPosition, new Vector3(0, 0, -distance), smoothSpeed * Time.unscaledDeltaTime);
		camLocalPosition = camara.transform.localPosition;
		pos = transform.position;
		ray = new Ray(new Vector3(pos.x, pos.y + 50, pos.z), Vector3.down);
		if (Physics.Raycast(ray, out hit, 100f, layerMask))
		{
			pos.y = hit.point.y + camHeight;
		}
		//pos.x = Mathf.Clamp(pos.x, moveLimits[0], moveLimits[1]);
		//pos.z = Mathf.Clamp(pos.z, moveLimits[2], moveLimits[3]);
		transform.position = Vector3.Lerp(transform.position, pos, moveSmoothSpeed * Time.unscaledDeltaTime);
		if (Input.GetKey (KeyCode.Mouse1))
		{
			//camMoveSpeed = Vector3.Distance(Input.mousePosition, savedMousePosition);
			transform.Translate(new Vector3((Input.mousePosition.x - (Screen.width / 2)) / (4000 / (distance / moveSpeedDivision)), 0, (Input.mousePosition.y - (Screen.height / 2)) / (4000 / (distance / moveSpeedDivision))), Space.Self);
			//transform.position = pos;
		}
		if (Input.GetKey (KeyCode.Mouse2))
		{
			/*if (fix)
				{
					savedMousePosition = Input.mousePosition;
					fix = false;
				}
*/
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			xMouse = transform.localEulerAngles.y;
			yMouse = camRot.transform.localEulerAngles.x;
			xMouse -= Input.GetAxis ("Mouse X") * mouseRotationSpeed * Time.unscaledDeltaTime;
			yMouse += Input.GetAxis ("Mouse Y") * mouseRotationSpeed * Time.unscaledDeltaTime;
			yMouse = Mathf.Clamp(yMouse, lowerRotLimit, 90f);
			camRot.transform.localEulerAngles = new Vector3(yMouse, 0);
			//camRot.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(yMouse, 0)), 1 * Time.deltaTime);
			//transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, xMouse)), 1 * Time.deltaTime);
			transform.localEulerAngles = new Vector3(0, xMouse);
		}else {
			fix = true;
			yMouse = Mathf.Clamp(yMouse, lowerRotLimit, 90f);
			camRot.transform.localEulerAngles = new Vector3(yMouse, 0);
		}
	}

	// void OnGUI ()
	// {
	// 	GUI.Box(GUIResize.ResizeGUI(new Rect(400, 400, 5, 5)), "");
	// }
}
