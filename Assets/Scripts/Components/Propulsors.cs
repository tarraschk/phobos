using UnityEngine;
using System.Collections;

public class Propulsors : MonoBehaviour {
	
    public float speed = 1.0F;
	public float acceleration = 1.0f;
	public float limitSpeed = 5.0f;
	public float rotationSpeed = 5.5f;
	
    public Vector3 targetPos ;
	public bool useTargetPos = false ; 
	
    void Start() {
    }
    void Update() {
		physicsUpdate(); 
    }
	
	public bool isHasTargetPos()
	{
		return (this.useTargetPos);
	}
	
	
	public void setTargetPos(Vector3 newTargetPos) 
	{
		this.useTargetPos = true ; 
		this.targetPos = newTargetPos;	
	}
	
	public void unsetTargetPos() 
	{
		this.useTargetPos = false ; 
		//this.targetPos = Vector3.zero; 	
	}
	
	public Vector3 getTargetPos() {
		return this.targetPos;	
	}
	
	public void stop() {
		this.unsetTargetPos();
		this.speed = 0; 
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
		var remainingDistance = Vector3.Distance(targetPos, this.transform.position);
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
		var rotation = Quaternion.LookRotation(targetPos - this.transform.position);
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
	
	private bool isAccelerationPossible() {
		return (this.speed < this.limitSpeed);	
	}
	
	private void rotate(float rotationSpeed) {
		transform.Rotate(10 * Vector3.up * Time.deltaTime);
	}
	
	
}
