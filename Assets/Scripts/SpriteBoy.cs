/// <summary>
/// Date: 04_20_18
/// Author: B. Evans
/// Purpose: For Howard!
/// License and Use: Do whatever you want with it! No credit needed!
/// Description: This class handles the swapping of sprites based on camera angle changes
/// This should be attached to the GameObject that represents the player
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// This using statment includes the reorderable array/list utilities
/// </summary>
using SubjectNerd.Utilities;

/// <summary>
/// Since we are loading in sprites, we want to make sure we have a sprite renderer component or add one
/// This will automatically add one if there isn't already
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]

public class SpriteBoy : MonoBehaviour {
	///*********************************
	/// CLASS VARIABLES
	///*********************************

	/// <summary>
	/// The array of angles we will use to flag a sprite change
	/// Each angle index corresponds with the matching sprite index
	/// </summary>
	public float[] desired_angle_changes;

	/// <summary>
	/// The array of sprites we want to use for any given angle
	/// This is reorderable so that we can start the sprite at the proper angle
	/// Each sprite index corresponds with the matching angle index
	/// </summary>
	[Reorderable]
	public List<Sprite> sprites;

	/// <summary>
	/// Set based on our angle comparisons and used to determine which sprite to load from the array
	/// </summary>
	public int current_direction_index;

	/// <summary>
	/// Since we are loading in sprites, we want to make sure we have a sprite renderer reference cached
	/// </summary>
	public SpriteRenderer sprite_renderer;

	/// <summary>
	/// Since we are referencing our camera angle every frame, we want to make sure we have a main camera reference cached
	/// </summary>
	public Camera main_camera;

	/// <summary>
	/// This will keep track of our current camera angle so we can reference it every frame without 
	/// having to call main_camera.transform.localEulerAngles every time
	/// </summary>
	public Vector3 current_camera_angle;

	public float last_forward;
	public bool can_move;

	///*********************************
	/// UNITY FUNCTIONS
	///*********************************

	void Start () {

		//Cache our sprite renderer since we know we have to have one or have added one
		sprite_renderer = GetComponent<SpriteRenderer>();

		//Cache the main camera using the Unity main camera reference
		main_camera = Camera.main;
	}

	void Update () {

		//Tell this object to always face the camera
		transform.LookAt(Camera.main.transform);

		//Cache our current camera angle this frame for reference below
		current_camera_angle = main_camera.transform.localEulerAngles;

		//Keep our angle between 0 and 360. If it's less than 0, set it to 360 - itself to stay within range
		if(current_camera_angle.y < 0){
			current_camera_angle = new Vector3(current_camera_angle.x, 360-current_camera_angle.y, current_camera_angle.z);
		}

		//Subtract last forward (set in our key press inputs) from our camera angle so we can offset which sprite is our "start point"
		current_camera_angle.y = current_camera_angle.y - last_forward;

		//Make sure our angle isn't negative by adding 360 to it
		if(current_camera_angle.y < 0){
			current_camera_angle = new Vector3(current_camera_angle.x, current_camera_angle.y + 360, current_camera_angle.z);
		}

		///*********************************
		/// INPUT
		///*********************************
		//If we press forward, set our last forward to whatever our current camera angle is, since we are always facing forward with our camera
		if(can_move){
			if(Input.GetAxisRaw("Horizontal") > 0 && Input.GetAxisRaw("Vertical") > 0){
				last_forward =  main_camera.transform.localEulerAngles.y + 45;
			}
			if(Input.GetAxisRaw("Horizontal") > 0 && Input.GetAxisRaw("Vertical") == 0){
				last_forward =  main_camera.transform.localEulerAngles.y + 90;
			}
			if(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") > 0){
				last_forward =  main_camera.transform.localEulerAngles.y;
			}
			if(Input.GetAxisRaw("Horizontal") < 0 && Input.GetAxisRaw("Vertical") > 0){
				last_forward =  main_camera.transform.localEulerAngles.y + 315;
			}
			if(Input.GetAxisRaw("Horizontal") < 0 && Input.GetAxisRaw("Vertical") == 0){
				last_forward =  main_camera.transform.localEulerAngles.y + 270;
			}
			if(Input.GetAxisRaw("Horizontal") < 0 && Input.GetAxisRaw("Vertical") < 0){
				last_forward =  main_camera.transform.localEulerAngles.y + 225;
			}
			if(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") < 0){
				last_forward =  main_camera.transform.localEulerAngles.y + 180;
			}
			if(Input.GetAxisRaw("Horizontal") > 0 && Input.GetAxisRaw("Vertical") < 0){
				last_forward =  main_camera.transform.localEulerAngles.y + 135;
			}
		}

		//If last forward is negative, recalculate the angle to be between 0 and 360
		if(last_forward < 0){

			last_forward = 360-last_forward;
		}

		if(last_forward > 360){

			last_forward = last_forward - 360;
		}

		//This loops through our angles and checks to see if our current angle falls within any two of our desired angles
		for(int i = 0; i < desired_angle_changes.Length-1; i++){

			//If our current angle is in fact between two of our desired angles, set our current index to that "angle"
			if(current_camera_angle.y > desired_angle_changes[i] && current_camera_angle.y < desired_angle_changes[i+1]){

				//Our direction index is really just an arbitrary number used to track which sprite to load based on which angle index we got a hit on
				current_direction_index = i;
			}

			//If we are using offset angles, this will correct the fact that we can have an angle
			//between 337.5 and 22.5 and wraps it properly
			else if(current_camera_angle.y > desired_angle_changes[0] || current_camera_angle.y < desired_angle_changes[1]){
				current_direction_index = 0;
			}
		}

		//Set the current sprite to the specific sprite from the sprite array based on our updated index
		sprite_renderer.sprite = sprites[current_direction_index];
	}
}
