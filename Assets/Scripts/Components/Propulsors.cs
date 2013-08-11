using UnityEngine;
using System.Collections;

public class Propulsors : MonoBehaviour {
	
    public float speed = 1.0F;
	public float acceleration = 1.0f;
	public float limitSpeed = 5.0f;
	public float rotationSpeed = 5.5f;
	public Transform position;
	
    public Transform targetPos = null;
	private GameObject target = null;
	
    void Start() {
    }
    void Update() {
		physicsUpdate(); 
		//rigidbody.AddForce(transform.forward * speed, ForceMode.VelocityChange);
		//rigidbody.velocity = transform.forward * speed;
    }
	
	public bool isHasTargetPos()
	{
		return (this.targetPos != null);
	}
	
	public void setTargetPos(Transform newTargetPos) 
	{
		this.targetPos = newTargetPos;	
	}
	
	public void unsetTargetPos() 
	{
		this.targetPos = null; 	
	}
	
	public Transform getTargetPos() {
		return this.targetPos;	
	}
	
	private void physicsUpdate() {	
		if (this.isHasTargetPos())
			this.moveBehavior();
		else 
			this.idleBehavior();
	}
	
	private void idleBehavior() {
		this.stop();
	}
	
	private void moveBehavior() {
		lookAtTarget();
		var remainingDistance = Vector3.Distance(targetPos.transform.position, this.transform.position);
	    //this.transform.LookAt(this.target.transform);
		if (remainingDistance < 5)
			this.stop();
		else if (remainingDistance < (transform.forward.x * this.speed * Time.deltaTime * 2)) 
		{
			this.throttleBrake(-this.acceleration);
		}
		else {
			if (isAccelerationPossible())
			{
				this.throttleBrake(this.acceleration) ; 
			}
		}
		this.speed = (Mathf.Clamp(this.speed, -this.limitSpeed, this.limitSpeed));
		transform.position += transform.forward * this.speed * Time.deltaTime;
	}
	
	private void lookAtTarget() {
		var rotation = Quaternion.LookRotation(targetPos.transform.position - this.transform.position);
		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.deltaTime * this.rotationSpeed);
	}
	
	
	private void rotateToDestination(Transform diffPosDest) {
		if (diffPosDest.rotation.eulerAngles.z > 0) {
			if (Mathf.Abs(diffPosDest.rotation.eulerAngles.z) > 180) {
				this.rotate(-this.rotationSpeed);
			}
			else this.rotate(this.rotationSpeed);
		}
		else {
			if (Mathf.Abs(diffPosDest.rotation.eulerAngles.z) > 180) {
				this.rotate(this.rotationSpeed);
			}
			else this.rotate(-this.rotationSpeed);
		}
	}
	
	private void throttleBrake(float acceleration) {
		this.speed = this.speed + acceleration; 
	}
	
	private void stop() {
		this.unsetTargetPos();
		this.speed = 0; 
	}
	
	private bool isAccelerationPossible() {
		return (this.speed < this.limitSpeed);	
	}
	
	private void rotate(float rotationSpeed) {
		transform.Rotate(10 * Vector3.up * Time.deltaTime);
	}
	
	
}
