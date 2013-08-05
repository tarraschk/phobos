using UnityEngine;
using System.Collections;

public class Propulsors : MonoBehaviour {
	
    public float speed = 1.0F;
	public float acceleration = 1.0f;
	public float limitSpeed = 5.0f;
	public float rotationSpeed = 5.5f;
	public Transform position;
	
    private Transform targetPos = null;
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
	
	//Unused
	private void moveToDestinationMovement() {
		Transform diffPosDest = this.getDiffDestinationPosition(targetPos);
		if (Mathf.Abs(diffPosDest.position.x) != 0 && Mathf.Abs(diffPosDest.position.y) != 0) {
			//target.rotation.eulerAngles.y = 5;//this.getDiffAngle(diffPosDest); 
		} 
		if (Mathf.Abs(diffPosDest.rotation.eulerAngles.z) > this.rotationSpeed) {
			this.rotateToDestination(diffPosDest);
			if (Mathf.Abs(diffPosDest.position.x) < 250 && Mathf.Abs(diffPosDest.position.y) < 250) //If target is very close, we brake.
				this.throttleBrake(-this.acceleration) ; 
			else 
				this.throttleBrake(this.acceleration); 
		}
		else {
			if (this.speed < this.limitSpeed)
			{
				this.throttleBrake(this.acceleration) ; 
			}
			//this.transform.rotation.eulerAngles.z = this.target.eulerAngles.z ;
		}
		if (Mathf.Abs(diffPosDest.position.x) < 5 && Mathf.Abs(diffPosDest.position.x) < 5 ) {
			this.stop() ; 
		}
	}
	
	private double getDiffAngle(Transform diffPos) {
		return 5;
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
	
	private Transform getDiffDestinationPosition(Transform destination) {
		return destination;
		//return (Transform((destination.position.x - this.transform.position.x), (destination.position.y - this.transform.position.y), (destination.rotation.z % 360 - this.transform.rotation.z % 360)));
	}
	
}
