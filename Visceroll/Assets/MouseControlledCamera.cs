using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.WSA;

[AddComponentMenu("Camera-Control/Smooth Mouse Look")]
public class MouseControlledCamera : MonoBehaviour {
 
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
 
	public float minimumX = -360F;
	public float maximumX = 360F;
 
	public float minimumY = -60F;
	public float maximumY = 60F;
 
	float rotationX = 0F;
	float rotationY = 0F;
 
	private List<float> rotArrayX = new List<float>();
	float rotAverageX = 0F;	
 
	private List<float> rotArrayY = new List<float>();
	float rotAverageY = 0F;
 
	public float frameCounter = 20;
 
	Quaternion originalRotation;
 
	void Update ()
	{
		if (axes == RotationAxes.MouseXAndY)
		{			
			rotAverageY = 0f;
			rotAverageX = 0f;
 
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationX += Input.GetAxis("Mouse X") * sensitivityX;
 
			rotArrayY.Add(rotationY);
			rotArrayX.Add(rotationX);
 
			if (rotArrayY.Count >= frameCounter) {
				rotArrayY.RemoveAt(0);
			}
			if (rotArrayX.Count >= frameCounter) {
				rotArrayX.RemoveAt(0);
			}
 
			for(int j = 0; j < rotArrayY.Count; j++) {
				rotAverageY += rotArrayY[j];
			}
			for(int i = 0; i < rotArrayX.Count; i++) {
				rotAverageX += rotArrayX[i];
			}
 
			rotAverageY /= rotArrayY.Count;
			rotAverageX /= rotArrayX.Count;
 
			rotAverageY = ClampAngle (rotAverageY, minimumY, maximumY);
			rotAverageX = ClampAngle (rotAverageX, minimumX, maximumX);
 
			Quaternion yQuaternion = Quaternion.AngleAxis (rotAverageY, Vector3.left);
			Quaternion xQuaternion = Quaternion.AngleAxis (rotAverageX, Vector3.up);
 
			transform.localRotation = originalRotation * xQuaternion * yQuaternion;
		}
		else if (axes == RotationAxes.MouseX)
		{			
			rotAverageX = 0f;
 
			rotationX += Input.GetAxis("Mouse X") * sensitivityX;
 
			rotArrayX.Add(rotationX);
 
			if (rotArrayX.Count >= frameCounter) {
				rotArrayX.RemoveAt(0);
			}
			for(int i = 0; i < rotArrayX.Count; i++) {
				rotAverageX += rotArrayX[i];
			}
			rotAverageX /= rotArrayX.Count;
 
			rotAverageX = ClampAngle (rotAverageX, minimumX, maximumX);
 
			Quaternion xQuaternion = Quaternion.AngleAxis (rotAverageX, Vector3.up);
			transform.localRotation = originalRotation * xQuaternion;			
		}
		else
		{			
			rotAverageY = 0f;
 
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
 
			rotArrayY.Add(rotationY);
 
			if (rotArrayY.Count >= frameCounter) {
				rotArrayY.RemoveAt(0);
			}
			for(int j = 0; j < rotArrayY.Count; j++) {
				rotAverageY += rotArrayY[j];
			}
			rotAverageY /= rotArrayY.Count;
 
			rotAverageY = ClampAngle (rotAverageY, minimumY, maximumY);
 
			Quaternion yQuaternion = Quaternion.AngleAxis (rotAverageY, Vector3.left);
			transform.localRotation = originalRotation * yQuaternion;
		}


		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.forward, out hit, 100.0f))
		{
			if (hit.transform.GetComponent<DragInteractable>() != null)
			{
				_crosshair.sprite = crosshairHover;
				if (Input.GetMouseButtonDown(0))
				{
					_isDragging = true;
					_dragTarget = hit.transform;
					if (_target == null) _target = new GameObject("target").transform;
					_target.position = transform.position + (transform.forward * 3.0f);//_dragTarget.position;
					_target.parent = transform;

					if (_dragTarget.GetComponent<Rigidbody>())
						_dragTarget.GetComponent<Rigidbody>().useGravity = false;
				}
			}
			else _crosshair.sprite = crosshairStandard;
		}
		else if (Input.GetMouseButton(0)) _crosshair.sprite = crosshairHover;
		else _crosshair.sprite = crosshairStandard;

		if (_dragTarget != null && _dragTarget.GetComponent<Rigidbody>())
		{
			_dragTarget.GetComponent<Rigidbody>().velocity = (_target.position - _dragTarget.position) * 20.0f;
		}
		
		if (Input.GetMouseButtonUp(0) && _dragTarget != null)
		{
			_isDragging = false;
			if (_dragTarget.GetComponent<Rigidbody>())
				_dragTarget.GetComponent<Rigidbody>().useGravity = true;
			_dragTarget = null;
		}
	}

	private bool _isDragging = false;
	private Transform _target, _dragTarget;
	
	[SerializeField] private UnityEngine.UI.Image _crosshair;
	public Sprite crosshairStandard, crosshairHover, crosshairDoorHover;
	void Start ()
	{
                Rigidbody rb = GetComponent<Rigidbody>();	
		if (rb)
			rb.freezeRotation = true;
		originalRotation = transform.localRotation;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;


	}
 
	public static float ClampAngle (float angle, float min, float max)
	{
		angle = angle % 360;
		if ((angle >= -360F) && (angle <= 360F)) {
			if (angle < -360F) {
				angle += 360F;
			}
			if (angle > 360F) {
				angle -= 360F;
			}			
		}
		return Mathf.Clamp (angle, min, max);
	}
}