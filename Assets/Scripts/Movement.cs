using UnityEngine;
using System.Collections;
	
public class Movement : MonoBehaviour
{
	public float moveSpeed;
	public float rotationSpeed;
	
	public Transform relativeTransform;

	void Start(){
		relativeTransform = Camera.main.transform;
	}

	void Update()
	{
		Vector3 moveDirection = Vector3.zero;
		if(Input.GetKey(KeyCode.W)) moveDirection += relativeTransform.forward;
		if(Input.GetKey(KeyCode.S)) moveDirection += -relativeTransform.forward;
		if(Input.GetKey(KeyCode.A)) moveDirection += -relativeTransform.right;
		if(Input.GetKey(KeyCode.D)) moveDirection += relativeTransform.right;
	
		moveDirection.y = 0f;
	
		transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
	
		if(moveDirection != Vector3.zero)
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveDirection), rotationSpeed * Time.deltaTime);
	}
}

