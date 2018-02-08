using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

// 3rd person game-like camera controller
// keeps camera behind the player and aimed at aiming point
public class Player_Camera : NetworkBehaviour {
	
	public Transform player;
	public Texture crosshair; // crosshair - removed it for quick and easy setup. ben0bi
	// if you add the crosshair, you need to drag a crosshair texture on the "crosshair" variable in the inspector 
	
	public Transform aimTarget; // that was public and a gameobject had to be dragged on it. - ben0bi
	
	public float smoothingTime = 20.0f; // it should follow it faster by jumping (y-axis) (previous: 0.1 or so) ben0bi
	public Vector3 pivotOffset = new Vector3(0f, 0.7f,  0.0f); // offset of point from player transform (?) ben0bi
	public Vector3 camOffset   = new Vector3(0f, 0.5f, -3.4f); // offset of camera from pivotOffset (?) ben0bi
	public Vector3 closeOffset = new Vector3(0f, 1.7f, 0.0f); // close offset of camera from pivotOffset (?) ben0bi


	public float horizontalAimingSpeed = 500f; // was way to lame for me (270) ben0bi
	public float verticalAimingSpeed = 500f;   // --"-- (270) ben0bi
	public float maxVerticalAngle = 80f;
	public float minVerticalAngle = -80f;
	public float range = 25.0f;
	public float glanceSensitivity = 2.0f;
	public float glanceDelay = 2.0f;
		
	//public float mouseSensitivity = 0.3f;

    public float angleH = 0;
    public float angleV = 0;
    public Transform cam;
    private Camera thisCam;
    public float maxCamDist = 1.0f;
    public LayerMask mask;
    public Vector3 smoothPlayerPos;
    public float mouseXInput;
    public float mouseYInput;
    public float glanceXPosition;
    public float glanceYPosition;
    public float glanceYMax;
    public float glanceYMin;

    public bool hasZoom;
    public float closeClippingPlane = 0.18f;
    public float zoomCloseClippingPlane;

    public bool isControllable = true;

    Character_Stats charStats;
    public UserSettings_HG userSettings;

    ParticleSystemRenderer[] cloudRenderers;
    bool isAboveClouds;

	// Use this for initialization
	void Start () 
	{
		charStats = GetComponentInParent<Character_Stats> ();
        // Add player's own layer to mask
        mask = 1 << player.gameObject.layer;
		// Add Igbore Raycast layer to mask
		mask |= 1 << LayerMask.NameToLayer("Ignore Raycast");
		// Invert mask
		mask = ~mask;
		
		cam = transform;
        thisCam = GetComponent<Camera>();
        smoothPlayerPos = player.position;
		glanceYPosition = camOffset.y;
		glanceYMax = glanceYPosition + 1.0f;
		glanceYMin = camOffset.y;
        maxCamDist = 3;

        cloudRenderers = GameObject.Find("Clouds").GetComponentsInChildren<ParticleSystemRenderer>();
        Debug.Log(cloudRenderers.ToString());
	}
	
	// Update is called once per frame
	void LateUpdate () {

		if (Time.deltaTime == 0 || Time.timeScale == 0 || player == null) 
			return;
        // if you want to set up an xbox controller or something, you need to uncomment the 
        // commented axes below in the source. 
        // (unity->edit->Project Settings->input, check the parameters behind the @ below.)
        // you can set up a new axis in the inspector by typing in a bigger number in the size property at the top.
        // I removed this, so you can quick and easy add this script to your game. - ben0bi
        // @joystick 3rd axis
        if (isControllable)
        {
            mouseXInput = Mathf.Clamp(Input.GetAxis("Mouse X")/* + Input.GetAxis("Horizontal2") */, -1.0f, 1.0f);
            mouseYInput = Mathf.Clamp(Input.GetAxis("Mouse Y") /* + Input.GetAxis("Vertical2") */, -1.0f, 1.0f);
        }
        else
        {
            mouseXInput = 0f;
            mouseYInput = 0f;
        }

		angleH += mouseXInput * horizontalAimingSpeed * userSettings.mouseSensitivity * Time.deltaTime;

        if (userSettings.invertYAxis)
        {
           angleV += mouseYInput * verticalAimingSpeed * userSettings.mouseSensitivity * Time.deltaTime;
        }
        else
        {
          angleV -= mouseYInput * verticalAimingSpeed *userSettings.mouseSensitivity * Time.deltaTime;
        }

		// limit vertical angle
		angleV = Mathf.Clamp(angleV, minVerticalAngle, maxVerticalAngle);

		if (mouseXInput > 0) {
			glanceXPosition = Mathf.Lerp(glanceXPosition, 1.0f, Time.smoothDeltaTime * glanceDelay * smoothingTime);
		}
		if(mouseXInput < 0) {
			glanceXPosition = Mathf.Lerp(glanceXPosition, -1.0f, Time.smoothDeltaTime*glanceDelay*smoothingTime);
		}
		if (mouseYInput > 0) {
			glanceYPosition = Mathf.Lerp (glanceYPosition, glanceYMax, Time.smoothDeltaTime * glanceDelay * smoothingTime);
		} 
		if (mouseYInput < 0) {
			glanceYPosition = Mathf.Lerp (glanceYPosition, glanceYMin, Time.smoothDeltaTime * glanceDelay * smoothingTime);
		}

		camOffset = new Vector3 (glanceXPosition * glanceSensitivity, glanceYPosition * glanceSensitivity, camOffset.z);
	
		
		// Before changing camera, store the prev aiming distance.
		// If we're aiming at nothing (the sky), we'll keep this distance.
		//float prevDist = (aimTarget.position - cam.position).magnitude;
		
		// Set aim rotation (add -  to invert angleV
		Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0);
		Quaternion camYRotation = Quaternion.Euler(0, angleH, 0);
		cam.rotation = aimRotation;
		
		// Find far and close position for the camera
		smoothPlayerPos = Vector3.Lerp(smoothPlayerPos, player.position, smoothingTime * Time.deltaTime);
		smoothPlayerPos.x = player.position.x;
		smoothPlayerPos.z = player.position.z;

	
		Vector3 farCamPoint = smoothPlayerPos + camYRotation * pivotOffset + aimRotation * camOffset;
		Vector3 closeCamPoint = player.position + camYRotation * closeOffset;
		float farDist = Vector3.Distance(farCamPoint, closeCamPoint);
		
//		// Smoothly increase maxCamDist up to the distance of farDist
		maxCamDist = Mathf.Lerp(maxCamDist, farDist, 5 * Time.deltaTime);
		
		// Make sure camera doesn't intersect geometry
		// Move camera towards closeOffset if ray back towards camera position intersects something 
		RaycastHit hit;
		Vector3 closeToFarDir = (farCamPoint - closeCamPoint) / farDist;
		float padding = 0.3f;
        if (Physics.Raycast(closeCamPoint, closeToFarDir, out hit, maxCamDist + padding, mask))
        {
            maxCamDist = hit.distance - padding;
        }

        if (hasZoom)
        {
            thisCam.nearClipPlane = zoomCloseClippingPlane;
            cam.position = Vector3.Lerp(cam.position, closeCamPoint, 15 * Time.deltaTime);
        }
        else
        {
            thisCam.nearClipPlane = closeClippingPlane;
            cam.position = Vector3.Lerp(cam.position, closeCamPoint + closeToFarDir * maxCamDist, 15 * Time.deltaTime);
        }

        // Do a raycast from the camera to find the distance to the point we're aiming at.
        float aimTargetDist;
		if (Physics.Raycast(cam.position, cam.forward, out hit, charStats.GetStat("primaryAbilityRangeBase") * charStats.GetStat("primaryAbilityRangeMultiplier") * 0.10f, mask)) {
			aimTargetDist = hit.distance + 0.05f;
		}
		else {
			aimTargetDist = charStats.GetStat("primaryAbilityRangeBase") * charStats.GetStat("primaryAbilityRangeMultiplier") * 0.10f; 
			
		}
		// Set the aimTarget position according to the distance we found.
		// Make the movement slightly smooth.
	
		aimTarget.position = Vector3.Lerp(aimTarget.position, cam.position + cam.forward * aimTargetDist, Time.smoothDeltaTime * smoothingTime);

        if (GetComponentInParent<NetworkIdentity>().isLocalPlayer)
        {
            UpdateNonLocalPlayerUI();
        }

        if (!isAboveClouds && player.transform.position.y >= 44f)
        {
            if (cloudRenderers.Length == 0)
            {
                cloudRenderers = GameObject.Find("Clouds").GetComponentsInChildren<ParticleSystemRenderer>();
            }

            foreach (ParticleSystemRenderer renderer in cloudRenderers)
            {
                renderer.sortingOrder = 1;
            }

            isAboveClouds = true;
        }
        else if(isAboveClouds && player.transform.position.y < 44f)
        {
            foreach (ParticleSystemRenderer renderer in cloudRenderers)
            {
                renderer.sortingOrder = 0;
            }

            isAboveClouds = false;
        }
    }

    void UpdateNonLocalPlayerUI()
    {
        GameObject[] nonLocalPlayerUIs = GameObject.FindGameObjectsWithTag("NonLocalUI");
        foreach (GameObject uI in nonLocalPlayerUIs)
        {
            Quaternion newRotationY = cam.transform.rotation;
            newRotationY.x = 0;
            newRotationY.z = 0;
            uI.transform.rotation = newRotationY;
        }
    }

    public void SetTarget(Transform t)
	{
		player=t;
	}
    
}