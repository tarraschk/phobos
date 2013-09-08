using UnityEngine;
using System.Collections;

/**
 * Allows an object to move like a spaceship. 
 */
public class Propulsors : MonoBehaviour {
	
	//Current speed. 
    public float speed = 1.0F;
	
	//Acceleration at each frame, use carefully. 
	public float acceleration = 1.0f;
	
	//Maximum speed. 
	public float limitSpeed = 5.0f;
	
	//How fast the ship can rotate
	public float rotationSpeed = 5.5f;
	
	//The current position target, where the ship has to go. 
    public Vector3 targetPos ;
	
	//Trigger the movement to the target position
	public bool useTargetPos = false ; 
	
	/**
	 * Main update loop
	 * */
	
    void Update() {
		physicsUpdate(); 
    }
	/**
	 * Has the ship currently a target position ? 
	 * */
	
	public bool isHasTargetPos()
	{
		return (this.useTargetPos);
	}
	
	/**
	 * *
	 */
	public void setTargetPos(Vector3 newTargetPos) 
	{
		this.useTargetPos = true ; 
		this.targetPos = newTargetPos;	
	}
	
	/**
	 * *
	 */
	public void unsetTargetPos() 
	{
		this.useTargetPos = false ; 
		//this.targetPos = Vector3.zero; 	
	}
	
	public Vector3 getTargetPos() {
		return this.targetPos;	
	}
	
	/**
	 * Stops the ship
	 * Cancel the target position
	 * */
	public void stop() {
		this.unsetTargetPos();
		this.speed = 0; 
	}
	
	/**
	 * Main movement loop. 
	 * If we have a target position, we move. 
	 * Elsewise, we are idle. 
	 */
	private void physicsUpdate() {	
		if (this.isHasTargetPos())
			this.moveBehavior();
		else 
			this.idleBehavior();
	}
	
	/**
	 * Idle = stop the ship.  
	 */
	
	private void idleBehavior() {
		this.stop();
	}
	
	/**
	 * Main move logic. 
	 * Checks the distance to the target position remaining. 
	 * Stops or accelerate the ship depending on that distance. 
	 * Also rotates the ship, depending on the target. 
	 */
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
	
	public void lookAt(Vector3 targetLook) {
		var rotation = Quaternion.LookRotation(targetLook - this.transform.position);
		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.deltaTime * this.rotationSpeed);
	}
	
	/**
	 * Turns the ship towards the current target position
	 * Requires a target position (this.targetPos); 
	 */
	private void lookAtTarget() {
		this.lookAt(this.targetPos); 
	}
	
	/**
	 * Change the speed of the ship. 
	 */
	private void throttleBrake(float acceleration) {
		this.speed = this.speed + acceleration; 
	}
	
	/**
	 * Check the maximum speed 
	 */
	private bool isAccelerationPossible() {
		return (this.speed < this.limitSpeed);	
	}
	
}
