using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;


public class Player_Controller : NetworkBehaviour {

    //Player_Controller contains general controls, which reference Character_Stats for variables and specialized functions.

    //Behaviour Delegate Formats
    public delegate void OnLeftClick();
    public delegate void OnRightClick();
    public delegate void OnEPress();
    public delegate void OnSpacePress();
    public delegate void Auras(GameObject target);

    //Can Ability Be Used Bool.
    public bool isLanding = false;

    //Behaviour variables
    public OnRightClick rightClickBehaviour;
    public OnLeftClick leftClickBehaviour;
    public OnEPress pressEBehaviour;
    public OnSpacePress pressSpaceBehaviour;
    public Auras auras;

    //Object reference definitions.
    public GameObject player;
    public Transform shooter;
    public Transform aimTarget;
    public Rigidbody rigidBody;

    //Movement definitions.
    public Vector3 moveDirection = Vector3.zero;
    private float lockCameraTimer = 0.0f;
    private float speedSmoothing = 10.0f;
    private float moveSpeed = 0.0f;
    public float moveSpeedEffectModifier = 1f;
    public float friction = 0.33f;
    private float slow = 1f;
    public bool isMoving = false;
    public bool canJump = true;
    public bool jumping = false;
    public bool jumpingReachedApex = false;
    public float lastJumpButtonTime = 0f;
    public float lastJumpTime = -1.0f;
    public float inAirControlAcceleration = 5.0f;
    public float jumpRepeatTime = 0.05f;
    public float jumpTimeout = 0.15f;
    //private float lastGroundedTime = 0.0f;
    public float groundedTimeout = 0.25f;
    public float gravity = 20.0f;
    public int jumpCount = 0;
    private CollisionFlags collisionFlags;


    //Primary Ability
    public double primaryAbilityDelay;
    public ParticleSystem primaryParticles;

    //Secondary Ability
    public double secondaryAbilityDelay;
    public ParticleSystem secondaryParticles;

    //Tertiary Ability
    public double tertiaryAbilityDelay;
    public ParticleSystem tertiaryParticles;
    public Transform tertiaryAbilityOrigin;

    //Movement ParticleSystems
    public ParticleSystem jumpParticles;
    public ParticleSystem flyingParticles;

    //Explosion effect definitions.
    private float explosiveForce;
    private float radiusInner;
    private float radiusOutter;
    private float damage;
    private Collider hitInner;
    private Collider hitOutter;

    public bool hasLaser = false;

    //Camera definitions.
    public Camera playerCameraObject;
    public Player_Camera playerCamera;

    //Physics definitions.
    public float verticalDistanceAboveGround;
    public float verticalSpeed;
    public Vector3 inAirVelocity = Vector3.zero;

    //Beam definitions.
    LineRenderer line;
    public bool beamIsHoldable;
    public double beamStart;
    public Transform beamOrigin;

    public Player_Stats playerStats;
    public Character_Stats charStats;
    public Animator anim;

    //Projectile GameObject Array, and the Transforms the instantiate on.
    public GameObject[] projectileArray;
    public Transform[] projectileOriginsArray;

    //Character Specific Particle Systems.
    public ParticleSystem[] particleArray;
    public ParticleSystem etherDamageParticles;

    //Is the character controllable?
    public bool isControllable;
    public bool knockedBack = false;

    //Does character's abilities have a zoom effect?
    float primaryZoom;
    float secondaryZoom;
    float tertiaryZoom;

    bool isZoomActive = false;

    //Inputs
    float h;
    float v;
    bool j;
    bool fireOne;
    bool fireTwo;
    bool fireThree;

    //Arena side references.
    public Transform blueSide;
    public Transform redSide;

    //Local GamePlayer.
    public bool localGamePlayerBool = false;
    public GamePlayer localGamePlayer;
    public int team;

    void Start() {
        playerStats = GetComponent<Player_Stats>();
        charStats = GetComponent<Character_Stats>();
        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        playerCamera = playerCameraObject.GetComponent<Player_Camera>();

        //Side References.
        blueSide = GameObject.Find("Blue Side").GetComponent<Transform>();
        redSide = GameObject.Find("Red Side").GetComponent<Transform>();

        //Assign Character Abilities
        GetAbilitiesForCharacter();

        primaryZoom = charStats.GetStat("primaryZoom");
        secondaryZoom = charStats.GetStat("secondaryZoom");
        tertiaryZoom = charStats.GetStat("tertiaryZoom");

        //Auras
        if (isServer)
        {
         InvokeRepeating("AuraInvoke", 1.0f, 1.0f);
        }

        if (hasLaser == true) {
            Debug.Log("hasLaser");
            line = shooter.GetComponent<LineRenderer>();
            line.enabled = false;
        }
        if (isLocalPlayer)
        {
            localGamePlayerBool = true;
        }
    }

    //
    void FixedUpdate()
    {
        RaycastHit hit;
        Ray landingRay = new Ray(transform.position, Vector3.down);


        if (Physics.Raycast(landingRay, out hit))
        {
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
            {
                verticalDistanceAboveGround = hit.distance;
            }
        }
    }

    void LateUpdate()
    {
        if (playerStats.syncSecondMobilityTalentLevel == 3 && jumpCount >= 3)
        {
            j = Input.GetButton("Jump");
        }
    }


    // Update is called once per frame
    void Update() {
        if (isControllable && isLocalPlayer)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
            if (jumpCount < 2)
            {
                j = Input.GetButtonDown("Jump");
            }
            fireOne = Input.GetButton("Fire1");
            fireTwo = Input.GetButton("Fire2");
            fireThree = Input.GetButton("Fire3");

            if (j)
            {
                pressSpaceBehaviour();
            }


            CheckIfAbilityUsed(fireOne, fireTwo, fireThree);


        }
        else
        {
            //Kill inputs if not controllable.
            h = 0f;
            v = 0f;
            j = false;

            fireOne = false;
            fireTwo = false;
            fireThree = false;
        }

        if (isLocalPlayer)
        {
            //May need to go back up into isControllable if statement.
            UpdateSmoothedMovementDirection(h, v);
            Animating(h, v, j, verticalDistanceAboveGround);

            //Rotate character with the Camera.
            RotateCharacterWithCamera();
            shooter.transform.rotation = playerCameraObject.transform.rotation;

            //Current factor applied to movement to slow. 1 = no slow, 0 = slow.
            //If knocked back, then lerp from 1 to 0 for slow, by the value of friction. 
            if (knockedBack)
            {
                slow -= Mathf.Lerp(0f, 1f, friction);

                Debug.Log(slow);

                if (slow <= 0f)
                {
                    knockedBack = false;
                    isControllable = true;
                }

                //If movement is equal to the zero vector, then set knockedBack to false, and controllablility to true. 
            }
            else
            {
                if(slow < 1f)
                {
                    slow += Mathf.Lerp(0f, 1f, friction);
                }
            }

            //Current movement.
            Vector3 movement = moveDirection * moveSpeed * Mathf.Clamp01(slow) + new Vector3(0, verticalSpeed, 0) + inAirVelocity;

            //Multiply by the time to render each frame, and the slow factor.
            movement *= Time.deltaTime;



            CharacterController characterController = GetComponent<CharacterController>();
            collisionFlags = characterController.Move(movement);
        }

        if (IsGrounded())
        {

            inAirVelocity = Vector3.zero;
            jumpCount = 0;
            if (jumping)
            {
                jumping = false;
                anim.SetBool("IsJumping", false);
                anim.SetBool("IsFlying", false);
            }
        }

        ApplyGravity();
    }

    void UpdateSmoothedMovementDirection(float h, float v)
    {
        Transform cameraTransform = playerCameraObject.transform;
        bool grounded = IsGrounded();

        // Forward vector relative.
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;

        // Right vector relative to the camera.
        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        bool wasMoving = isMoving;
        isMoving = Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f;

        // Target direction relative to the camera.
        Vector3 targetDirection = h * right + v * forward;

        // Grounded controls
        if (grounded)
        {
            lockCameraTimer += Time.deltaTime;
            if (isMoving != wasMoving)
                lockCameraTimer = 0.0f;

            if (targetDirection != Vector3.zero)
            {
                lockCameraTimer += Time.deltaTime;
                if (isMoving != wasMoving)
                    lockCameraTimer = 0.0f;

                moveDirection = targetDirection.normalized;
            }

            // Smooth the speed based on the current target direction
            float curSmooth = speedSmoothing * Time.deltaTime;
            float targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);

            targetSpeed *= charStats.GetStat("runSpeedBase") * charStats.GetStat("runSpeedMultiplier") * moveSpeedEffectModifier;
            moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
        }
        // In air controls
        else
        {

            // Lock camera while in air
            if (jumping)
                lockCameraTimer = 0.0f;

            if (isMoving)
                inAirVelocity += targetDirection.normalized * Time.deltaTime * inAirControlAcceleration;
        }
    }

    void ApplyGravity()
    {
            // When we reach the apex of the jump we send out a message
            if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0f)
            {
                jumpingReachedApex = true;
                SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
            }

            if (IsGrounded() && !jumping)
                verticalSpeed = 0.0f;
            else
                verticalSpeed -= gravity * Time.deltaTime;
//        }
    }

    void GetAbilitiesForCharacter()
    {
        if (transform.name.Contains("Gideon"))
        {
            //Gideon
            leftClickBehaviour = charStats.GideonPrimaryAbility;
            rightClickBehaviour = charStats.GideonSecondaryAbility;
            pressEBehaviour = charStats.GideonTertiaryAbility;
            pressSpaceBehaviour = charStats.GideonSpace;
            auras = charStats.GideonAuras;
        }
    }

    void CheckIfAbilityUsed(bool one, bool two, bool three) {
        if (three)
        {
            if (tertiaryZoom >= 1 && !isZoomActive)
            {
                playerCamera.hasZoom = true;
                isZoomActive = true;
            }
            if (tertiaryZoom >= 1 || (playerStats.energy >= charStats.GetStat("tertiaryAbilityEnergyCostBase") * charStats.GetStat("tertiaryAbilityEnergyCostMultiplier")) && (Network.time - tertiaryAbilityDelay) >= (charStats.GetStat("tertiaryAbilityRateBase") * charStats.GetStat("tertiaryAbilityRateMultiplier")))
            {
                anim.SetBool("TertiaryAbility", true);

                //Set last time ability was used if the abilty doesn't have zoom, otherwise the value needs to be set in the function in Character_Stats.
                if (tertiaryZoom == 0f)
                {
                    tertiaryAbilityDelay = Network.time;
                }
            }
            else
            {
                anim.SetBool("TertiaryAbility", false);
            }
        }
        else if (two)
        {
            if (secondaryZoom >= 1 && !isZoomActive)
            {
                playerCamera.hasZoom = true;
                isZoomActive = true;
            }
            if (((playerStats.energy >= charStats.GetStat("secondaryAbilityEnergyCostBase") * charStats.GetStat("secondaryAbilityEnergyCostMultiplier")) && secondaryZoom >= 1 && one && (Network.time - secondaryAbilityDelay) >= (charStats.GetStat("secondaryAbilityRateBase") * charStats.GetStat("secondaryAbilityRateMultiplier"))) || (secondaryZoom == 0 && (Network.time - secondaryAbilityDelay) >= (charStats.GetStat("secondaryAbilityRateBase") * charStats.GetStat("secondaryAbilityRateMultiplier"))))
            {
                anim.SetBool("SecondaryAbility", true);

                //Set last time ability was used if the abilty doesn't have zoom, otherwise the value needs to be set in the function in Character_Stats.
                if (secondaryZoom == 0f)
                {
                    secondaryAbilityDelay = Network.time;
                }
            }
            else
            {
                anim.SetBool("SecondaryAbility", false);
            }
        }
        else if (one)
        {
                if (primaryZoom >= 1 && !isZoomActive)
            {
                playerCamera.hasZoom = true;
                isZoomActive = true;
            }
            if (primaryZoom >= 1 || (playerStats.energy >= charStats.GetStat("primaryAbilityEnergyCostBase") * charStats.GetStat("primaryAbilityEnergyCostMultiplier")) && (Network.time - primaryAbilityDelay) >= (charStats.GetStat("primaryAbilityRateBase") * charStats.GetStat("primaryAbilityRateMultiplier")))
            {
                anim.SetBool("PrimaryAbility", true);

                //Set last time ability was used if the abilty doesn't have zoom, otherwise the value needs to be set in the function in Character_Stats.
                if (primaryZoom == 0f)
                {
                    primaryAbilityDelay = Network.time;
                }

            }
            else
            {
                anim.SetBool("PrimaryAbility", false);
            }
        }
        else 
        {
            anim.SetBool("PrimaryAbility", false);
            anim.SetBool("SecondaryAbility", false);
            anim.SetBool("TertiaryAbility", false);
            playerCamera.hasZoom = false;
            isZoomActive = false;
        }
    }

    public void UseAura(GameObject target)
    {
        if (isServer)
        {
            auras(target);
        }
    }

    void UsePrimaryAbility()
    {
        if (isLocalPlayer)
        {
            CmdPrimaryAbility();
        }
    }

    void UseSecondaryAbility()
    {
        if (isLocalPlayer)
        {
            CmdSecondaryAbility();
        }
    }

    void UseTertiaryAbility()
    {
        if (isLocalPlayer)
        {
            CmdAbility();
        }
    }

    public void PlayPrimaryAbilityParticles()
    {
        ParticleSystem[] particles = primaryParticles.GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem particle in particles)
        {
            particle.Play();
        }
    }

    public void PlaySecondaryAbilityParticles()
    {
        ParticleSystem[] particles = secondaryParticles.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particles)
        {
            particle.Play();
        }
    }

    public void PlayTertiaryAbilityParticles()
    {
        ParticleSystem[] particles = tertiaryParticles.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particles)
        {
            particle.Play();
        }
    }

    public void PlayJumpParticles()
    {
        ParticleSystem[] particles = jumpParticles.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particles)
        {
            particle.Play();
        }
    }

    public void PlayFlyingParticles()
    {
        ParticleSystem[] particles = flyingParticles.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particles)
        {
            particle.Play();
        }
    }

    public void StopParticles()
    {
        foreach(ParticleSystem particle in particleArray)
        {
            if (particle.isPlaying)
            {
                particle.Stop();
            }
        }
    }

    [Command]
    void CmdPrimaryAbility()
    {
        leftClickBehaviour();
    }

    [Command]
    void CmdSecondaryAbility()
    {
        rightClickBehaviour();
    }

    [Command]
    void CmdAbility()
    {
        pressEBehaviour();
    }

    //Character general animation.
    void Animating(float h, float v, bool j, float jumpingPosition)
    {
        bool running = v != 0f;
        anim.SetBool("IsRunning", running);

        //Passes values for the Running Blend Tree
        anim.SetFloat("HorizontalDirection", h);
        anim.SetFloat("VerticalDirection", v);

        anim.SetFloat("JumpingPosition", jumpingPosition);
        anim.SetFloat("JumpVelocity", verticalSpeed);

    }


    //Generic Laser Function
    public IEnumerator Beam()
    {
        line.enabled = true;
        Debug.Log("Beam()");

        if (beamIsHoldable)
        {
            while (Input.GetButton("Fire1"))
            {
                Debug.Log("isHoldable");
                Ray ray = new Ray(transform.position, shooter.transform.forward);
                line.SetPosition(0, ray.origin);
                line.SetPosition(1, ray.GetPoint(charStats.GetStat("secondaryAbilityRangeBase") * charStats.GetStat("secondaryAbilityRangeMultiplier")));

                yield return null;
            }
        }
        else
        {
            while (Input.GetButton("Fire1") && (Network.time - beamStart) >= charStats.GetStat("secondaryBeamDuration"))
            {
                Debug.Log("isNotHoldable");
                Ray ray = new Ray(beamOrigin.position, shooter.transform.forward);
                line.SetPosition(0, ray.origin);
                line.SetPosition(1, ray.GetPoint(charStats.GetStat("secondaryAbilityRangeBase") * charStats.GetStat("secondaryAbilityRangeMultiplier")));

                yield return null;
            }
        }

        line.enabled = false;
    }

    void RadiusEffects(Vector3 explosionPos, int attackType, bool objectFriendlyFire) {
        //primary Ability
        if (attackType == 0)
        {
            radiusInner = charStats.GetStat("primaryAbilityInnerRadiusBase") * charStats.GetStat("primaryAbilityInnerRadiusMultiplier");
            radiusOutter = charStats.GetStat("primaryAbilityOutterRadiusBase") * charStats.GetStat("primaryAbilityOutterRadiusMultiplier");
            explosiveForce = charStats.GetStat("primaryAbilityExplosiveForce");
        }
        //Secondary Ability
        else if (attackType == 1)
        {
            radiusInner = charStats.GetStat("secondaryAbilityInnerRadiusBase") * charStats.GetStat("secondaryAbilityInnerRadiusMultiplier");
            radiusOutter = charStats.GetStat("secondaryAbilityOutterRadiusBase") * charStats.GetStat("secondaryAbilityOutterRadiusMultiplier");
            explosiveForce = charStats.GetStat("secondaryAbilityExplosiveForce");
        }
        //Tertiary Ability
        else if (attackType == 2)
        {
            radiusInner = charStats.GetStat("tertiaryAbilityInnerRadiusBase") * charStats.GetStat("tertiaryAbilityInnerRadiusMultiplier");
            radiusOutter = charStats.GetStat("tertiaryAbilityOutterRadiusBase") * charStats.GetStat("tertiaryAbilityOutterRadiusMultiplier");
            explosiveForce = charStats.GetStat("tertiaryAbilityExplosiveForce");
        }

            Collider[] collidersInner = Physics.OverlapSphere(explosionPos, 0.5f * radiusInner);
            Collider[] collidersOutter = Physics.OverlapSphere(explosionPos, 1.0f * radiusOutter);

        if (objectFriendlyFire)
        {
            foreach (Collider hit in collidersInner)
            {
                if (hit.gameObject.tag == "destructible")
                {
                    DamageObject(hit, attackType);
                }
                if (hit.gameObject.tag == "Player" && hit.GetComponent<Player_Controller>().localGamePlayer.team != localGamePlayer.team)
                {
                    DamagePlayer(hit, attackType);
                }
            }

            foreach (Collider hit in collidersOutter)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (hit.gameObject.tag == "destructible")
                {
                    DamageObject(hit, attackType);
                    if (rb != null)
                    {
                        rb.AddExplosionForce(explosiveForce, explosionPos, radiusOutter, 3.0f);
                    }
                }
                else if (hit.gameObject.tag == "Player" && hit.GetComponent<Player_Controller>().localGamePlayer.team != localGamePlayer.team)
                {
                        DamagePlayer(hit, attackType);
                        if (rb != null)
                        {
                            rb.AddExplosionForce(explosiveForce*2, explosionPos, radiusOutter, 3.0f);
                        }
                }
            }
        }
        else
        {
            foreach (Collider hit in collidersInner)
            {
                if (hit.gameObject.tag == "destructible" && localGamePlayer.team != hit.GetComponent<Object_Stats>().objectTeam)
                {
                    DamageObject(hit, attackType);
                }
                if (hit.gameObject.tag == "Player" && hit.GetComponent<Player_Controller>().localGamePlayer.team != localGamePlayer.team)
                {
                    if (hit.gameObject != player)
                    {
                        DamagePlayer(hit, attackType);
                    }
                }
            }

            foreach (Collider hit in collidersOutter)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (hit.gameObject.tag == "destructible" && localGamePlayer.team != hit.GetComponent<Object_Stats>().objectTeam)
                {
                    DamageObject(hit, attackType);
                    if (rb != null)
                    {
                        rb.AddExplosionForce(explosiveForce, explosionPos, radiusOutter, 3.0f);
                    }
                }
                else if (hit.gameObject.tag == "Player" && hit.GetComponent<Player_Controller>().localGamePlayer.team != localGamePlayer.team)
                {
                    if (hit != isLocalPlayer)
                    {
                        DamagePlayer(hit, attackType);
                        if (rb != null)
                        {
                            rb.AddExplosionForce(explosiveForce * 2, explosionPos, radiusOutter, 3.0f);
                        }
                    }
                }
            }
        }

    }

    //Damage is Applied on the Server
    [Server]
    public void ApplyDamage(Vector3 pos, int attackType, bool objectFriendlyFire) {
        RadiusEffects(pos, attackType, objectFriendlyFire);
    }

    void DamageObject(Collider go, int attackType) {
            //primary Ability
            if (attackType == 0) {
                CmdTellServerObjectDamage(go.gameObject, charStats.GetStat("primaryAbilityDamageBase") * charStats.GetStat("primaryAbilityDamageMultiplier") * 0.5f);
            }
            //Secondary Ability
            if (attackType == 1) {
                CmdTellServerObjectDamage(go.gameObject, charStats.GetStat("secondaryAbilityDamageBase") * charStats.GetStat("secondaryAbilityDamageMultiplier") * 0.5f);
            }
            //Tertiary Ability
            if (attackType == 2) {
                CmdTellServerObjectDamage(go.gameObject, charStats.GetStat("tertiaryAbilityDamageBase") * charStats.GetStat("tertiaryAbilityDamageMultiplier") * 0.5f);
            }
    }

    void DamagePlayer(Collider go, int attackType) {
            if (attackType == 0) {
                CmdTellServerPlayerDamage(go.gameObject, charStats.GetStat("primaryAbilityDamageBase") * charStats.GetStat("primaryAbilityDamageMultiplier") * 0.5f);
            }
            if (attackType == 1) {
                CmdTellServerPlayerDamage(go.gameObject, charStats.GetStat("secondaryAbilityDamageBase") * charStats.GetStat("secondaryAbilityDamageMultiplier") * 0.5f);
            }
            if (attackType == 2) {
                CmdTellServerPlayerDamage(go.gameObject, charStats.GetStat("tertiaryAbilityDamageBase") * charStats.GetStat("tertiaryAbilityDamageMultiplier") * 0.5f);
            }
    }

    public void ApplyEffect(GameObject player, string effectName, float effectStrength, float effectDuration)
    {
        player.GetComponent<Player_Stats>().TakeEffect(effectName, effectStrength, effectDuration);
    }

    [Command]
    void CmdTellServerPlayerDamage(GameObject uniqueID, float dmg) {
        uniqueID.gameObject.GetComponent<Player_Stats>().TakeDamage(dmg);
    }

    [Command]
    void CmdTellServerPlayerStatusEffects(GameObject uniqueID, string effectName, float effectStrength, float effectDuration)
    {
        uniqueID.gameObject.GetComponent<Player_Stats>().TakeEffect(effectName, effectStrength, effectDuration);
    }

    [Command]
    void CmdTellServerObjectDamage(GameObject uniqueID, float dmg) {
        uniqueID.gameObject.GetComponent<Object_Stats>().TakeDamage(dmg);
    }

    [ClientRpc]
    public void RpcSetProjectileParent(int projectileArrayIndex, GameObject projectile)
    {
        projectile.transform.SetParent(projectileOriginsArray[projectileArrayIndex]);
        projectile.transform.localPosition = Vector3.zero;
    }

    public void RotateCharacterWithCamera()
    {
        Quaternion lookDir = playerCamera.transform.rotation;
        lookDir.x = 0;
        lookDir.z = 0;

        transform.rotation = lookDir;
    }

    public bool IsGrounded()
    {
        return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.moveDirection.y > 0.01f)
            return;
    }

    public void DidJump()
    {
        jumping = true;
        jumpingReachedApex = false;
        lastJumpTime = Time.time;
        lastJumpButtonTime = Time.time;
    }

    public float CalculateJumpVerticalSpeed(float targetJumpHeight)
    {
        return Mathf.Sqrt(2 * targetJumpHeight * gravity);
    }

    public IEnumerator WaitFor(bool condition)
    {
        yield return new WaitUntil(() => condition == true);
    }

}
