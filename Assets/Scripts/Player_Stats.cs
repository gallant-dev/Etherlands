using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player_Stats : NetworkBehaviour {

    [SyncVar(hook = "OnHealthChange")]
    public float health;
    [SyncVar]
    public float energy;
    [SyncVar]
    public float charge;

    [SyncVar(hook = "MoveSpeedEffectHook")]
    public float moveSpeedEffectModifier;

    //Damage Tree Talents
    [SyncVar(hook = "OnSyncFirstDamageTalentLevelChange")]
    public int syncFirstDamageTalentLevel;//PrimaryAbility
    [SyncVar(hook = "OnSyncSecondDamageTalentLevelChange")]
    public int syncSecondDamageTalentLevel;//SecondaryAbility
    [SyncVar(hook = "OnSyncThirdDamageTalentLevelChange")]
    public int syncThirdDamageTalentLevel;//SpeedOfAbility

    //Defense Tree Talents
    [SyncVar(hook = "OnSyncFirstDefenseTalentLevelChange")]
    public int syncFirstDefenseTalentLevel;//Armour
    [SyncVar(hook = "OnSyncSecondDefenseTalentLevelChange")]
    public int syncSecondDefenseTalentLevel;//Health
    [SyncVar(hook = "OnSyncThirdDefenseTalentLevelChange")]
    public int syncThirdDefenseTalentLevel;//Health Regeneration

    //Support Tree Talents
    [SyncVar(hook = "OnSyncFirstSupportTalentLevelChange")]
    public int syncFirstSupportTalentLevel;//SupportAbility
    [SyncVar(hook = "OnSyncSecondSupportTalentLevelChange")]
    public int syncSecondSupportTalentLevel;//Energy
    [SyncVar(hook = "OnSyncThirdSupportTalentLevelChange")]
    public int syncThirdSupportTalentLevel;//EnergyRegeneration

    //Mobility Tree Talents
    [SyncVar(hook = "OnSyncFirstMobilityTalentLevelChange")]
    public int syncFirstMobilityTalentLevel;//MobilityAbility
    [SyncVar(hook = "OnSyncSecondMobilityTalentLevelChange")]
    public int syncSecondMobilityTalentLevel;//JumpLevel
    [SyncVar(hook = "OnSyncThirdMobilityTalentLevelChange")]
    public int syncThirdMobilityTalentLevel;//Speed

    //Effect Integer
    [SyncVar]
    public string effectString;

    public List<Effect> effectList = new List<Effect>();

    private Text healthText;
    private Text energyText;
    private Text chargeText;


    //Talent Text Regerences
    private Text damageFirstTalentText;
    private Text damageSecondTalentText;
    private Text damageThirdTalentText;
    private Text defenseFirstTalentText;
    private Text defenseSecondTalentText;
    private Text defenseThirdTalentText;
    private Text supportFirstTalentText;
    private Text supportSecondTalentText;
    private Text supportThirdTalentText;
    private Text mobilityFirstTalentText;
    private Text mobilitySecondTalentText;
    private Text mobilityThirdTalentText;

    //Talent Crystal Refences
    private RectTransform damageFirstTalentCrystal;
    private RectTransform damageSecondTalentCrystal;
    private RectTransform damageThirdTalentCrystal;
    private RectTransform defenseFirstTalentCrystal;
    private RectTransform defenseSecondTalentCrystal;
    private RectTransform defenseThirdTalentCrystal;
    private RectTransform mobilityFirstTalentCrystal;
    private RectTransform mobilitySecondTalentCrystal;
    private RectTransform mobilityThirdTalentCrystal;
    private RectTransform supportFirstTalentCrystal;
    private RectTransform supportSecondTalentCrystal;
    private RectTransform supportThirdTalentCrystal;

    //Slider References
    private Slider healthSliderRight;
    private Slider healthSliderLeft;
    private Slider energySliderRight;
    private Slider energySliderLeft;
    public Slider nonLocalHealthSliderRight;
    public Slider nonLocalHealthSliderLeft;
    public Slider nonlocalEnergySliderRight;
    public Slider nonlocalEnergySliderLeft;

    //Effect Icon
    private CanvasRenderer levelUpEffectIcon;
    private CanvasRenderer maxHealthEffectIcon;
    private CanvasRenderer healthRegenEffectIcon;
    private CanvasRenderer maxEnergyEffectIcon;
    private CanvasRenderer energyRegenEffectIcon;
    private CanvasRenderer damageEffectIcon;
    private CanvasRenderer armourEffectIcon;
    private CanvasRenderer moveSpeedEffectIcon;
    private CanvasRenderer stunEffectIcon;


    private float maxHealth;
    private float maxEnergy;
    private float healthRegen;
    private float energyRegen;
    private float maxHealthEffectModifier = 1f;
    private float maxEnergyEffectModifier = 1f;
    private float healthRegenEffectModifier = 1f;
    private float energyRegenEffectModifier = 1f;
    
    //Death and Respawn References.
    private bool shouldDie = false;
    public bool isDead = false;
    public float timeOfDeath;
    public float respawnTime;
    public Vector3 respawnPoint;
    public bool respawned = false;
    private Text respawnCountdownText;


    public delegate void DieDelegate();
    public event DieDelegate EventDie;

    Character_Stats charStats;
    Player_Menus menu;
    Player_Controller controller;
    ButtonBehaviour buttonBehaviour;

    float secondTick;

    void Start() {
        //Start server repeating functions.
        if (isServer)
        {
            InvokeRepeating("ChargeAccumulation", 2, 1f);
            InvokeRepeating("CheckEffects", 2, 1f);
        }

        //GetCharacterStats
        charStats = GetComponent<Character_Stats>();
        menu = GetComponent<Player_Menus>();
        controller = GetComponent<Player_Controller>();

        EventDie += DisablePlayer;

        SetPlayerStats(0, charStats.GetStat("maxHealthBase"));//Health
        SetPlayerStats(1, charStats.GetStat("maxEnergyBase"));//Energy
        SetPlayerStats(2, 100); // Charge

        SetPlayerTalents(1, 0);
        SetPlayerTalents(2, 0);
        SetPlayerTalents(3, 0);
        SetPlayerTalents(4, 0);
        SetPlayerTalents(5, 0);
        SetPlayerTalents(6, 0);
        SetPlayerTalents(7, 0);
        SetPlayerTalents(8, 0);
        SetPlayerTalents(9, 0);
        SetPlayerTalents(10, 0);
        SetPlayerTalents(11, 0);
        SetPlayerTalents(12, 0);

        if (isLocalPlayer)
        {
            //ButtonBehaviour
            buttonBehaviour = GameObject.Find("Match Menus").GetComponent<ButtonBehaviour>();

            //Stat Text References
            healthText = menu.healthText;
            energyText = menu.energyText;
            chargeText = menu.chargeText;

            //Talent Text References
            damageFirstTalentText = menu.damageFirstTalentText;
            damageSecondTalentText = menu.damageSecondTalentText;
            damageThirdTalentText = menu.damageSecondTalentText;
            defenseFirstTalentText = menu.defenseFirstTalentText;
            defenseSecondTalentText = menu.defenseSecondTalentText;
            defenseThirdTalentText = menu.defenseThirdTalentText;
            supportFirstTalentText = menu.supportFirstTalentText;
            supportSecondTalentText = menu.supportSecondTalentText;
            supportThirdTalentText = menu.supportThirdTalentText;
            mobilityFirstTalentText = menu.mobilityFirstTalentText;
            mobilitySecondTalentText = menu.mobilitySecondTalentText;
            mobilityThirdTalentText = menu.mobilityThirdTalentText;

            //RespawnTimer
            respawnCountdownText = GameObject.Find("LocalRespawnCountdownText").GetComponent<Text>();
        }


        if (isLocalPlayer) {
            //Local Slider References
            healthSliderLeft = GameObject.Find("LowerUIHealthSliderLeft").GetComponent<Slider>();
            healthSliderRight = GameObject.Find("LowerUIHealthSliderRight").GetComponent<Slider>();
            energySliderLeft = GameObject.Find("LowerUIEnergySliderLeft").GetComponent<Slider>();
            energySliderRight = GameObject.Find("LowerUIEnergySliderRight").GetComponent<Slider>();

            //Local Effect Icon References
            levelUpEffectIcon = GameObject.Find("LevelUpEffectIcon").GetComponent<CanvasRenderer>();
            maxHealthEffectIcon = GameObject.Find("MaxHealthEffectIcon").GetComponent<CanvasRenderer>();
            healthRegenEffectIcon = GameObject.Find("HealthRegenEffectIcon").GetComponent<CanvasRenderer>();
            maxEnergyEffectIcon = GameObject.Find("MaxEnergyEffectIcon").GetComponent<CanvasRenderer>();
            energyRegenEffectIcon = GameObject.Find("EnergyRegenEffectIcon").GetComponent<CanvasRenderer>();
            damageEffectIcon = GameObject.Find("DamageEffectIcon").GetComponent<CanvasRenderer>();
            armourEffectIcon = GameObject.Find("ArmourEffectIcon").GetComponent<CanvasRenderer>();
            moveSpeedEffectIcon = GameObject.Find("MoveSpeedEffectIcon").GetComponent<CanvasRenderer>();
            stunEffectIcon = GameObject.Find("StunEffectIcon").GetComponent<CanvasRenderer>();

        }
        else
        {
            //Slider refences for non local players
            healthSliderLeft = nonLocalHealthSliderLeft;
            healthSliderRight = nonLocalHealthSliderRight;
            energySliderLeft = nonlocalEnergySliderLeft;
            energySliderRight = nonlocalEnergySliderRight;
        }

    }


    // Update is called once per frame

    void Update() {
        maxHealth = charStats.GetStat("maxHealthBase") * charStats.GetStat("maxHealthMultiplier") * maxHealthEffectModifier;
        maxEnergy = charStats.GetStat("maxEnergyBase") * charStats.GetStat("maxEnergyMultiplier") * maxEnergyEffectModifier;

        CheckCondition();
        UpdateHealthAndEnergyUI();

        if (isLocalPlayer) {
            //Set Talent Texts
            SetDamageFirstTalentText();
            SetDamageSecondTalentText();
            SetDamageThirdTalentText();
            SetDefenseFirstTalentText();
            SetDefenseSecondTalentText();
            SetDefenseThirdTalentText();
            SetSupportFirstTalentText();
            SetSupportSecondTalentText();
            SetSupportThirdTalentText();
            SetMobilityFirstTalentText();
            SetMobilitySecondTalentText();
            SetMobilityThirdTalentText();

            //StatusEffectIcons
            UpdateStatusEffectIcons();
        }
    }

    void LateUpdate() {
        if (isServer)
        {
            CheckToRespawnPlayer();
        }
        Regenerate();
        if (isLocalPlayer) {
            UpdateStatTexts();
            RespawnCountdownTimer();
        }
    }

    void UpdateStatusEffectIcons()
    {
        //UpgradeAvailable
        if (charge >= 100)
        {
            levelUpEffectIcon.SetAlpha(Mathf.Lerp(levelUpEffectIcon.GetAlpha(), 1f, 1f * Time.deltaTime));
        }
        else
        {
            levelUpEffectIcon.SetAlpha(Mathf.Lerp(levelUpEffectIcon.GetAlpha(), 0f, 1.5f * Time.deltaTime));
        }

        //MaxHealthEffectIcon
        if (effectString.Contains("mxHlth"))
        {
            maxHealthEffectIcon.SetAlpha(Mathf.Lerp(maxHealthEffectIcon.GetAlpha(), 1f, 1f * Time.deltaTime));
        }
        else
        {
            maxHealthEffectIcon.SetAlpha(Mathf.Lerp(maxHealthEffectIcon.GetAlpha(), 0f, 1.5f * Time.deltaTime));
        }

        //HealthRegenEffectIcon
        if (effectString.Contains("rgnHlth"))
        {
            healthRegenEffectIcon.SetAlpha(Mathf.Lerp(healthRegenEffectIcon.GetAlpha(), 1f, 1f * Time.deltaTime));
        }
        else
        {
            healthRegenEffectIcon.SetAlpha(Mathf.Lerp(healthRegenEffectIcon.GetAlpha(), 0f, 1.5f * Time.deltaTime));
        }

        //MaxEnergyEffectIcon
        if (effectString.Contains("mxNrg"))
        {
            maxEnergyEffectIcon.SetAlpha(Mathf.Lerp(maxEnergyEffectIcon.GetAlpha(), 1f, 1f * Time.deltaTime));
        }
        else
        {
            maxEnergyEffectIcon.SetAlpha(Mathf.Lerp(maxEnergyEffectIcon.GetAlpha(), 0f, 1.5f * Time.deltaTime));
        }

        //EnergyRegenEffectIcon
        if (effectString.Contains("nrgRgn"))
        {
            energyRegenEffectIcon.SetAlpha(Mathf.Lerp(energyRegenEffectIcon.GetAlpha(), 1f, 1f * Time.deltaTime));
        }
        else
        {
            energyRegenEffectIcon.SetAlpha(Mathf.Lerp(energyRegenEffectIcon.GetAlpha(), 0f, 1.5f * Time.deltaTime));
        }

        //DamageEffectIcon
        if (effectString.Contains("dmg"))
        {
            damageEffectIcon.SetAlpha(Mathf.Lerp(damageEffectIcon.GetAlpha(), 1f, 1f * Time.deltaTime));
        }
        else
        {
            damageEffectIcon.SetAlpha(Mathf.Lerp(damageEffectIcon.GetAlpha(), 0f, 1.5f * Time.deltaTime));
        }

        //DamageEffectIcon
        if (effectString.Contains("rmr"))
        {
            armourEffectIcon.SetAlpha(Mathf.Lerp(armourEffectIcon.GetAlpha(), 1f, 1f * Time.deltaTime));
        }
        else
        {
            armourEffectIcon.SetAlpha(Mathf.Lerp(armourEffectIcon.GetAlpha(), 0f, 1.5f * Time.deltaTime));
        }

        //MoveSpeedEffectIcon
        if (effectString.Contains("mvSpd"))
        {
            moveSpeedEffectIcon.SetAlpha(Mathf.Lerp(moveSpeedEffectIcon.GetAlpha(), 1f, 1f * Time.deltaTime));
        }
        else
        {
            moveSpeedEffectIcon.SetAlpha(Mathf.Lerp(moveSpeedEffectIcon.GetAlpha(), 0f, 1.5f * Time.deltaTime));
        }

        //StunEffectIcon
        if (effectString.Contains("rmr"))
        {
            stunEffectIcon.SetAlpha(Mathf.Lerp(stunEffectIcon.GetAlpha(), 1f, 1f * Time.deltaTime));
        }
        else
        {
            stunEffectIcon.SetAlpha(Mathf.Lerp(stunEffectIcon.GetAlpha(), 0f, 1.5f * Time.deltaTime));
        }
    }



    void Regenerate() {
        if (isServer && !isDead)
        {
            if (health >= maxHealth)
            {
                healthRegen = 0f;
            }
            else
            {
                healthRegen = charStats.GetStat("healthRegenBase") * charStats.GetStat("healthRegenMultiplier") * healthRegenEffectModifier;
            }

            if(energy >= maxEnergy)
            {
                energyRegen = 0f;
            }
            else
            {
                energyRegen = charStats.GetStat("energyRegenBase") * charStats.GetStat("energyRegenMultiplier") * energyRegenEffectModifier;
            }

            health += (healthRegen * Time.deltaTime);
            energy += (energyRegen * Time.deltaTime);
            
        }
    }


    void ChargeAccumulation() {
            ++charge;
    }

    void CheckCondition() {
        if (health <= 0 && !shouldDie && !isDead) {
            shouldDie = true;
        }

        if (health <= 0 && shouldDie) {
            if (EventDie != null) {
                EventDie();
            }
        }
    }

    void CheckEffects()
    {
        int effectListCount = effectList.Count;
        if (effectListCount != 0)
        {
            List<Effect> removeList = new List<Effect>();
            string newEffectString = effectString;

            foreach (Effect effect in effectList)
            {
                double timeLapsedSinceStart = Network.time - effect.startTime;

                if (timeLapsedSinceStart >= effect.duration)
                {

                    if (effect.name == "healthRegen")
                    {
                        healthRegenEffectModifier = 1f;
                        effectString.Replace("hlthRgn", "");
                    }
                    else if (effect.name == "energyRegen")
                    {
                        energyRegenEffectModifier = 1f;
                        effectString.Replace("nrgRgn", "");
                    }
                    else if (effect.name == "moveSpeed" && moveSpeedEffectModifier != 1f)
                    {
                        
                        moveSpeedEffectModifier = 1f;
                        effectString = newEffectString.Replace("mvSpd", "");
                    }
                    else if (effect.name == "stn" && moveSpeedEffectModifier != 1f)
                    {

                        moveSpeedEffectModifier = 1f;
                        effectString = newEffectString.Replace("stn", "");
                    }

                    removeList.Add(effect);
                }
            }

            foreach (Effect effect in removeList)
            {
                effectList.Remove(effect);
            }

        }

    }

    public void UpdateHealthAndEnergyUI() {
        //Convert Health to Int and set UI sliders
        float newSliderValue = health / maxHealth;
        healthSliderLeft.value = newSliderValue;
        healthSliderRight.value = newSliderValue;

        if (isLocalPlayer) {
            int healthInt = (int)health;
            int healthMax = (int)maxHealth;
            healthText.text = healthInt.ToString() + "/" + healthMax.ToString();
        }
        //Convert Energy to Int and set UI Sliders
        energySliderLeft.value = energy / maxEnergy;
        energySliderRight.value = energy / maxEnergy;
        if (isLocalPlayer) {
            int energyInt = (int)energy;
            int energyMax = (int)maxEnergy;
            energyText.text = energyInt.ToString() + "/" + energyMax.ToString();
        }
    }

    public void UpdateStatTexts() {

        //Convert Health Regen to doubke
        double hlthRgn = (double)healthRegen;

        //Set Charge Text
        if (isLocalPlayer) {
            chargeText.text = charge.ToString();
        }

        //Convert energy regen to double
        double nrgRgn = (double)energyRegen;

        //Convert armour to double
        double rmr = (double)(charStats.GetStat("armourBase") * charStats.GetStat("armourMultiplier"));

        //Convert run speed to double
        double rnSpd = (double)(charStats.GetStat("runSpeedBase") * charStats.GetStat("runSpeedMultiplier"));

        //Convert run speed to double
        double jmpHght = (double)(charStats.GetStat("jumpHeightBase") * charStats.GetStat("jumpHeightMultiplier"));

        //Convert Primary damage to double
        double prmrTtkDmg = (double)(charStats.GetStat("primaryAbilityDamageMultiplier") * charStats.GetStat("primaryAbilityDamageBase"));

        //Convert Primary Ability Outter Radius
        //double prmrTtckTtrRds = (double)(charStats.GetStat("primaryAbilityOutterRadiusBase") * charStats.GetStat("primaryAbilityOutterRadiusMultiplier"));

        //Convert Primary Ability Speed
        double prmrTtckSpd = (double)(charStats.GetStat("primaryAbilityRateBase") * charStats.GetStat("primaryAbilityRateMultiplier"));

        //Convert Primary Ability Range
        double prmrTtlRng = (double)(charStats.GetStat("primaryAbilityRangeBase") * charStats.GetStat("primaryAbilityRangeMultiplier"));

        //Convert Primary Energy Cost
        //double prmrNrgCst = (double)(charStats.GetStat("primaryAbilityEnergyCostBase") * charStats.GetStat("primaryAbilityEnergyCostMultiplier"));

        //Convert Secondary damage to double
        double scndrTtkDmg = (double)(charStats.GetStat("secondaryAbilityDamageBase") * charStats.GetStat("secondaryAbilityDamageMultiplier"));

        //Convert Secondary Ability Outter Radius
        //double scndrTtckTtrRds = (double)(charStats.GetStat("secondaryAbilityOutterRadiusBase") * charStats.GetStat("secondaryAbilityOutterRadiusMultiplier"));

        //Convert Secondary Ability Speed
        double scndrTtckSpd = (double)(charStats.GetStat("secondaryAbilityRateBase") * charStats.GetStat("secondaryAbilityRateMultiplier"));

        //Convert Secondary Ability Range
        double scndrTtlRng = (double)(charStats.GetStat("secondaryAbilityRangeBase") * charStats.GetStat("secondaryAbilityRangeMultiplier"));

        //Convert Secondary Energy Cost
        //double scndrNrgCst = (double)(charStats.GetStat("secondaryAbilityEnergyCostBase") * charStats.GetStat("secondaryAbilityEnergyCostMultiplier"));

        //Convert Secondary damage to double
        double trtrTtkDmg = (double)(charStats.GetStat("tertiaryAbilityDamageBase") * charStats.GetStat("tertiaryAbilityDamageMultiplier"));

        //Convert Tertiary Ability Speed
        double trtrTtckSpd = (double)(charStats.GetStat("tertiaryAbilityRateBase") * charStats.GetStat("tertiaryAbilityRateMultiplier"));

        //Convert Tertiary Ability Range
        double trtrTtckRng = (double)(charStats.GetStat("tertiaryAbilityRangeBase") * charStats.GetStat("tertiaryAbilityRangeMultiplier"));

        menu.healthRegenStatTextValue.text = hlthRgn.ToString();
        menu.energyRegenStatTextValue.text = nrgRgn.ToString();
        menu.ArmourStatTextValue.text = rmr.ToString();
        menu.RunSpeedStatTextValue.text = rnSpd.ToString();
        menu.jumpHeightStatTextValue.text = jmpHght.ToString();
        menu.primaryAbilityDamageStatTextValue.text = prmrTtkDmg.ToString();
        menu.primaryAbilitySpeedTextValue.text = prmrTtckSpd.ToString();
        menu.primaryAbilityRangeStatTextValue.text = prmrTtlRng.ToString();
        menu.secondaryAbilityDamageStatTextValue.text = scndrTtkDmg.ToString();
        menu.secondaryAbilitySpeedStatTextValue.text = scndrTtckSpd.ToString();
        menu.secondaryAbilityRangeStatTextValue.text = scndrTtlRng.ToString();
        menu.tertiaryAbilityDamageStatTextValue.text = trtrTtkDmg.ToString();
        menu.tertiaryAbilitySpeedStatTextValue.text = trtrTtckSpd.ToString();
        menu.tertiaryAbilityRangeStatTextValue.text = trtrTtckRng.ToString();
    }


    //Set Talent Text Functions
    public void SetDamageFirstTalentText() {
        damageFirstTalentText.text = syncFirstDamageTalentLevel.ToString() + " " + charStats.firstDamageTalentName;
    }

    public void SetDamageSecondTalentText() {
        damageSecondTalentText.text = syncSecondDamageTalentLevel.ToString() + " " + charStats.secondDamageTalentName;
    }

    public void SetDamageThirdTalentText() {
        damageThirdTalentText.text = syncThirdDamageTalentLevel.ToString() + " " + charStats.thirdDamageTalentName;
    }

    public void SetDefenseFirstTalentText() {
        defenseFirstTalentText.text = syncFirstDefenseTalentLevel.ToString() + " " + charStats.firstDefenseTalentName;
    }

    public void SetDefenseSecondTalentText() {
        defenseSecondTalentText.text = syncSecondDefenseTalentLevel.ToString() + " " + charStats.secondDefenseTalentName;
    }

    public void SetDefenseThirdTalentText() {
        defenseThirdTalentText.text = syncThirdDefenseTalentLevel.ToString() + " " + charStats.thirdDefenseTalentName;
    }

    public void SetSupportFirstTalentText() {
        supportFirstTalentText.text = syncFirstSupportTalentLevel.ToString() + " " + charStats.firstSupportTalentName;
    }

    public void SetSupportSecondTalentText() {
        supportSecondTalentText.text = syncSecondSupportTalentLevel.ToString() + " " + charStats.secondSupportTalentName;
    }

    public void SetSupportThirdTalentText() {
        supportThirdTalentText.text = syncThirdSupportTalentLevel.ToString() + " " + charStats.thirdSupportTalentName;

    }

    public void SetMobilityFirstTalentText() {
        mobilityFirstTalentText.text = syncFirstMobilityTalentLevel.ToString() + " " + charStats.thirdMobilityTalentName;
    }

    public void SetMobilitySecondTalentText() {
        mobilitySecondTalentText.text = syncSecondMobilityTalentLevel.ToString() + " " + charStats.secondMobilityTalentName;
    }

    public void SetMobilityThirdTalentText() {
        mobilityThirdTalentText.text = syncThirdMobilityTalentLevel.ToString() + " " + charStats.thirdMobilityTalentName;
    }

    //Is only called on the server.
    public void TakeDamage(float damage) {
        //Each armour should equal 10% reduction.
        if (isServer)
        {
            float damageTaken = Mathf.Abs(damage * (1 - ((charStats.GetStat("armourBase") * charStats.GetStat("armourMultiplier") * 0.10f))));
            health -= damageTaken;
        }
    }

    public void HealHealth(float amount)
    {
        health += amount;
    }

    
    //Server side.
    public void TakeEffect(string effectName, float effectStrength, float effectDuration)
    {
        if (effectList.Find(effect => effect.name == effectName) == null)
        {
            effectList.Add(new Effect(effectName, effectStrength, effectDuration, Network.time));
        }
        else if (!effectList.Contains(new Effect(effectName, effectStrength, effectDuration, Network.time)))
        {
            Effect lastEffect = effectList.Find(effect => effect.name == effectName);
            lastEffect.strength = effectStrength;
            lastEffect.duration = effectDuration;
            lastEffect.startTime = Network.time;
        }
        else
        {
            return;
        }

        if (effectName == "maxHealth" && !effectString.Contains("mxHlth"))
        {
            if (!effectString.Contains("mxHlth"))
            {
                effectString += "mxHlth";
            }
            //Overwrite old strength with new strength
            if (maxHealthEffectModifier <= effectStrength)
            {
                maxHealthEffectModifier = effectStrength;
            }
        }
        else if (effectName == "healthRegen" && !effectString.Contains("hlthRgn"))
        {
            if (!effectString.Contains("hlthRgn"))
            {
                effectString += "hlthRgn";
            }
            //Overwrite old strength with new strength
            if (healthRegenEffectModifier <= effectStrength)
            {
                healthRegenEffectModifier = effectStrength;
            }
        }
        else if (effectName == "maxEnergy" && !effectString.Contains("mxNrg"))
        {
            if (!effectString.Contains("mxNrg"))
            {
                effectString += "mxNrg";
            }
            //Overwrite old strength with new strength
            if (maxEnergyEffectModifier <= effectStrength)
            {
                maxEnergyEffectModifier = effectStrength;
            }

        }
        else if (effectName == "energyRegen" && !effectString.Contains("nrgRgn"))
        {
            if (!effectString.Contains("nrgRgn"))
            {
                effectString += "nrgRgn";
            }
            //Overwrite old strength with new strength
            if (energyRegenEffectModifier <= effectStrength)
            {
                energyRegenEffectModifier = effectStrength;
            }

        }
        else if (effectName == "moveSpeed")
        {
            if (!effectString.Contains("mvSpd"))
            {
                effectString += "mvSpd";
            }
            //Overwrite old strength with new strength
            if (moveSpeedEffectModifier <= effectStrength)
            {
                moveSpeedEffectModifier = effectStrength;
            }
        }
        else if (effectName == "stn")
        {
            if (!effectString.Contains("stn"))
            {
                effectString += "stn";
            }
                moveSpeedEffectModifier = 0f;
        }
    }
    
    public void TakeEnergy(float drain) {
        energy -= drain;
    }

    public void AddCharge(int addedCharge) {
        charge += addedCharge;
    }

    public void TakeCharge(int takenCharge) {
        charge -= takenCharge;
    }

    public void SetPlayerStats(int stat, float value) {
        if (stat == 0) {
            health = value;
        }
        if (stat == 1) {
            energy = value;
        }
        if (stat == 2) {
            charge = value;
        }
    }

    void SetPlayerTalents(int talent, int value) {
        if (talent == 1)
        {
            syncFirstDamageTalentLevel = value;
        }
        else if (talent == 2)
        {
            syncSecondDamageTalentLevel = value;
        }
        else if (talent == 3)
        {
            syncThirdDamageTalentLevel = value;
        }
        else if (talent == 4)
        {
            syncFirstDefenseTalentLevel = value;
        }
        else if (talent == 5)
        {
            syncSecondDefenseTalentLevel = value;
        }
        else if (talent == 6)
        {
            syncThirdDefenseTalentLevel = value;
        }
        else if (talent == 7)
        {
            syncFirstSupportTalentLevel = value;
        }
        else if (talent == 8)
        {
            syncSecondSupportTalentLevel = value;
        }
        else if (talent == 9)
        {
            syncThirdSupportTalentLevel = value;
        }
        else if (talent == 10)
        {
            syncFirstMobilityTalentLevel = value;
        }
        else if (talent == 11)
        {
            syncSecondMobilityTalentLevel = value;
        }
        else if (talent == 12)
        {
            syncThirdMobilityTalentLevel = value;
        }
        else
        {
            return;
        }
    }

    public void GetCharacterStats() {
        charStats.CheckCharacter();
    }

    //Increase Damage Tree Talents

    [Command]
    public void CmdIncreaseFirstDamageTalentLevel()
    {
        ++syncFirstDamageTalentLevel;
        TakeCharge(100);
    }

    [Command]
    public void CmdIncreaseSecondDamageTalentLevel()
    {
        ++syncSecondDamageTalentLevel;
        TakeCharge(100);
    }

    [Command]
    public void CmdIncreaseThirdDamageTalentLevel()
    {
        ++syncThirdDamageTalentLevel;
        TakeCharge(100);
    }

    //Increases Defense Tree Talents
    [Command]
    public void CmdIncreaseFirstDefenseTalentLevel() {
        ++syncFirstDefenseTalentLevel;
        TakeCharge(100);
    }
    
    [Command]
    public void CmdIncreaseSecondDefenseTalentLevel()
    {
        ++syncSecondDefenseTalentLevel;
        TakeCharge(100);
    }

    [Command]
    public void CmdIncreaseThirdDefenseTalentLevel()
    {
        ++syncThirdDefenseTalentLevel;
        TakeCharge(100);
    }

    //Support Tree Talents
    [Command]
    public void CmdIncreaseFirstSupportTalentLevel()
    {
        ++syncFirstSupportTalentLevel;
        TakeCharge(100);
    }
    
    [Command]
    public void CmdIncreaseSecondSupportTalentLevel()
    {
        ++syncSecondSupportTalentLevel;
        TakeCharge(100);
    }

    [Command]
    public void CmdIncreaseThirdSupportTalentLevel()
    {
        ++syncThirdSupportTalentLevel;
        TakeCharge(100);
    }

    //Mobility Tree Talents
    [Command]
    public void CmdIncreaseFirstMobilityTalentLevel()
    {
        ++syncFirstMobilityTalentLevel;
        TakeCharge(100);
    }
    
    [Command]
    public void CmdIncreaseSecondMobilityTalentLevel()
    {
        ++syncSecondMobilityTalentLevel;
        TakeCharge(100);
    }

    [Command]
    public void CmdIncreaseThirdMobilityTalentLevel()
    {
        ++syncThirdMobilityTalentLevel;
        TakeCharge(100);
    }

    void MoveSpeedEffectHook(float newSpeedModifier)
    {
        if (isLocalPlayer)
        {
            controller.moveSpeedEffectModifier = newSpeedModifier;
        }
    }

    void OnDisable()
    {
        EventDie -= DisablePlayer;
    }

    [Server]
    void CheckToRespawnPlayer()
    {
        respawnTime = charStats.GetStat("respawnCooldownBase") * charStats.GetStat("respawnCooldownMultiplier");

        if (isDead && (Network.time - timeOfDeath) >= respawnTime)
        {
            isDead = false;
            shouldDie = false;
            health = charStats.GetStat("maxHealthBase") * charStats.GetStat("maxHealthMultiplier");
            energy = charStats.GetStat("maxEnergyBase") * charStats.GetStat("maxEnergyMultiplier");
            RpcEnablePlayer(respawnPoint);
        }
    }

    void DisablePlayer()
    {
        //Set time of death.
        timeOfDeath = (float)Network.time;

        if (isServer)
        {
            health = 0f;
        }

        //GetComponent<CharacterController>().enabled = false;
        GetComponent<Player_Controller>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer ren in renderers)
        {
            ren.enabled = false;
        }

        isDead = true;

        if (isLocalPlayer)
        {
            GetComponent<Player_Controller>().isControllable = false;
            controller.aimTarget.gameObject.SetActive(false);
        }

        EventDie -= DisablePlayer;
    }

    [ClientRpc]
    void RpcEnablePlayer(Vector3 rspwnPnt)
    {
        isDead = false;
        shouldDie = false;

        //GetComponent<CharacterController>().enabled = true;
        GetComponent<Player_Controller>().enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer ren in renderers)
        {
            ren.enabled = true;
        }

        if (isLocalPlayer)
        {

            GetComponent<Player_Controller>().isControllable = true;
            controller.aimTarget.gameObject.SetActive(true);
            transform.position = rspwnPnt;
        }

        respawned = true;
    }

    void OnHealthChange(float hlth)
    {
        health = hlth;

        if(respawned && health > 0)
        {
            EventDie += DisablePlayer;
            respawned = false;
        }
    }

    void OnSyncFirstDamageTalentLevelChange(int newVal)
    {
        syncFirstDamageTalentLevel = newVal;

        GetCharacterStats();

        if (isLocalPlayer && newVal != 0)
        {
            SetDamageFirstTalentText();
            RectTransform button = GameObject.Find("DamageFirstTalentButton").GetComponent<RectTransform>();
            buttonBehaviour.UpdateLeftTalentText("DamageFirstTalentButton");
            buttonBehaviour.ActivateAllDisabledButtons();

            //Set's the new positions and scales of the first damage talent crystal
            if (syncFirstDamageTalentLevel == 1)
            {
                Vector3 newPosition = new Vector3(-65f, 18f, 0f);
                Vector3 newScale = new Vector3(3f, 2f, 1f);
                button.GetComponent<Image>().color = Color.red;
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
            else if (syncFirstDamageTalentLevel == 2)
            {
                Vector3 newPosition = new Vector3(-78f, 20f, 0f);
                Vector3 newScale = new Vector3(4f, 3f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
            else if (syncFirstDamageTalentLevel == 3)
            {
                Vector3 newPosition = new Vector3(-90f, 23f, 0f);
                Vector3 newScale = new Vector3(6, 6f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
        }
    }

    void OnSyncSecondDamageTalentLevelChange(int newVal)
    {
        syncSecondDamageTalentLevel = newVal;

        GetCharacterStats();

        if (isLocalPlayer && newVal != 0)
        {
            SetDamageSecondTalentText();
            RectTransform button = GameObject.Find("DamageSecondTalentButton").GetComponent<RectTransform>();
            buttonBehaviour.UpdateLeftTalentText("DamageSecondTalentButton");
            buttonBehaviour.ActivateAllDisabledButtons();

            //Set's the new positions and scales of the second damage talent crystal
            if (syncSecondDamageTalentLevel == 1)
            {
                Vector3 newPosition = new Vector3(-50f, 50f, 0f);
                Vector3 newScale = new Vector3(3f, 2f, 1f);
                button.GetComponent<Image>().color = Color.red;
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
            else if (syncSecondDamageTalentLevel == 2)
            {
                Vector3 newPosition = new Vector3(-58f, 58f, 0f);
                Vector3 newScale = new Vector3(4f, 3f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
            else if (syncSecondDamageTalentLevel == 3)
            {
                Vector3 newPosition = new Vector3(-68f, 68f, 0f);
                Vector3 newScale = new Vector3(6f, 6f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
        }
    }

    void OnSyncThirdDamageTalentLevelChange(int newVal)
    {
        syncThirdDamageTalentLevel = newVal;

        GetCharacterStats();

        if (isLocalPlayer && newVal != 0)
        {
            SetDamageThirdTalentText();
            RectTransform button = GameObject.Find("DamageThirdTalentButton").GetComponent<RectTransform>();
            buttonBehaviour.UpdateLeftTalentText("DamageThirdTalentButton");
            buttonBehaviour.ActivateAllDisabledButtons();

            //Set's the new positions and scales of the second damage talent crystal
            if (syncThirdDamageTalentLevel == 1)
            {
                Vector3 newPosition = new Vector3(-19f, 70f, 0f);
                Vector3 newScale = new Vector3(3f, 2f, 1f);
                button.GetComponent<Image>().color = Color.red;
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
            else if (syncThirdDamageTalentLevel == 2)
            {
                Vector3 newPosition = new Vector3(-20f, 77f, 0f);
                Vector3 newScale = new Vector3(4f, 3f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
            else if (syncThirdDamageTalentLevel == 3)
            {
                Vector3 newPosition = new Vector3(-23, 90f, 0f);
                Vector3 newScale = new Vector3(6f, 6f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
        }
    }

    void OnSyncFirstDefenseTalentLevelChange(int newVal)
    {
        syncFirstDefenseTalentLevel = newVal;

        GetCharacterStats();

        if (isLocalPlayer && newVal != 0)
        {
            SetDefenseFirstTalentText();
            RectTransform button = GameObject.Find("DefenseFirstTalentButton").GetComponent<RectTransform>();
            buttonBehaviour.UpdateLeftTalentText("DefenseFirstTalentButton");
            buttonBehaviour.ActivateAllDisabledButtons();

            //Set's the new positions and scales of the second damage talent crystal
            if (syncFirstDefenseTalentLevel == 1)
            {
                Vector3 newPosition = new Vector3(19f, 70f, 0f);
                Vector3 newScale = new Vector3(3f, 2f, 1f);
                button.GetComponent<Image>().color = Color.blue;
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
            else if (syncFirstDefenseTalentLevel == 2)
            {
                Vector3 newPosition = new Vector3(20f, 77f, 0f);
                Vector3 newScale = new Vector3(4f, 3f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
            else if (syncFirstDefenseTalentLevel == 3)
            {
                Vector3 newPosition = new Vector3(23, 90f, 0f);
                Vector3 newScale = new Vector3(6f, 6f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
        }
    }

    void OnSyncSecondDefenseTalentLevelChange(int newVal)
    {
        syncSecondDefenseTalentLevel = newVal;

        GetCharacterStats();

        if (isLocalPlayer && newVal != 0)
        {
            SetDefenseSecondTalentText();
            RectTransform button = GameObject.Find("DefenseSecondTalentButton").GetComponent<RectTransform>();
            buttonBehaviour.UpdateLeftTalentText("DefenseSecondTalentButton");
            buttonBehaviour.ActivateAllDisabledButtons();

            //Set's the new positions and scales of the second damage talent crystal
            if (syncSecondDefenseTalentLevel == 1)
            {
                Vector3 newPosition = new Vector3(50f, 50f, 0f);
                Vector3 newScale = new Vector3(3f, 2f, 1f);
                button.GetComponent<Image>().color = Color.blue;
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
            else if (syncSecondDefenseTalentLevel == 2)
            {
                Vector3 newPosition = new Vector3(58f, 58f, 0f);
                Vector3 newScale = new Vector3(4f, 3f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
            else if (syncSecondDefenseTalentLevel == 3)
            {
                Vector3 newPosition = new Vector3(68f, 68f, 0f);
                Vector3 newScale = new Vector3(6f, 6f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
        }
    }

    void OnSyncThirdDefenseTalentLevelChange(int newVal)
    {
        syncThirdDefenseTalentLevel = newVal;

        GetCharacterStats();

        if (isLocalPlayer && newVal != 0)
        {
            SetDefenseThirdTalentText();
            RectTransform button = GameObject.Find("DefenseThirdTalentButton").GetComponent<RectTransform>();
            buttonBehaviour.UpdateLeftTalentText("DefenseThirdTalentButton");
            buttonBehaviour.ActivateAllDisabledButtons();

            //Set's the new positions and scales of the second damage talent crystal
            if (syncThirdDefenseTalentLevel == 1)
            {
                Vector3 newPosition = new Vector3(65f, 18f, 0f);
                Vector3 newScale = new Vector3(3f, 2f, 1f);
                button.GetComponent<Image>().color = Color.blue;
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
            else if (syncThirdDefenseTalentLevel == 2)
            {
                Vector3 newPosition = new Vector3(78f, 20f, 0f);
                Vector3 newScale = new Vector3(4f, 3f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
            else if (syncThirdDefenseTalentLevel == 3)
            {
                Vector3 newPosition = new Vector3(90f, 23f, 0f);
                Vector3 newScale = new Vector3(6, 6f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
        }
    }

    void OnSyncFirstMobilityTalentLevelChange(int newVal)
    {
        syncFirstMobilityTalentLevel = newVal;

        GetCharacterStats();

        if (isLocalPlayer && newVal != 0)
        {
            SetMobilityFirstTalentText();
            RectTransform button = GameObject.Find("MobilityFirstTalentButton").GetComponent<RectTransform>();
            buttonBehaviour.UpdateLeftTalentText("MobilityFirstTalentButton");
            buttonBehaviour.ActivateAllDisabledButtons();

            //Set's the new positions and scales of the second damage talent crystal
            if (syncFirstMobilityTalentLevel == 1)
            {
                Vector3 newPosition = new Vector3(65f, -18f, 0f);
                Vector3 newScale = new Vector3(3f, 2f, 1f);
                button.GetComponent<Image>().color = new Color(0.988f, 0.322f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
            else if (syncFirstMobilityTalentLevel == 2)
            {
                Vector3 newPosition = new Vector3(78f, -20f, 0f);
                Vector3 newScale = new Vector3(4f, 3f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
            else if (syncFirstMobilityTalentLevel == 3)
            {
                Vector3 newPosition = new Vector3(90f, -23f, 0f);
                Vector3 newScale = new Vector3(6, 6f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
        }
    }

    void OnSyncSecondMobilityTalentLevelChange(int newVal)
    {
        syncSecondMobilityTalentLevel = newVal;

        GetCharacterStats();

        if (isLocalPlayer && newVal != 0)
        {
            SetMobilitySecondTalentText();
            RectTransform button = GameObject.Find("MobilitySecondTalentButton").GetComponent<RectTransform>();
            buttonBehaviour.UpdateLeftTalentText("MobilitySecondTalentButton");
            buttonBehaviour.ActivateAllDisabledButtons();

            //Set's the new positions and scales of the second damage talent crystal
            if (syncSecondMobilityTalentLevel == 1)
            {
                Vector3 newPosition = new Vector3(50f, -50f, 0f);
                Vector3 newScale = new Vector3(3f, 2f, 1f);
                button.GetComponent<Image>().color = new Color(0.988f, 0.322f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
            else if (syncSecondMobilityTalentLevel == 2)
            {
                Vector3 newPosition = new Vector3(58f, -58f, 0f);
                Vector3 newScale = new Vector3(4f, 3f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
            else if (syncSecondMobilityTalentLevel == 3)
            {
                Vector3 newPosition = new Vector3(68f, -68f, 0f);
                Vector3 newScale = new Vector3(6f, 6f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
        }
    }

    void OnSyncThirdMobilityTalentLevelChange(int newVal)
    {
        syncThirdMobilityTalentLevel = newVal;

        GetCharacterStats();

        if (isLocalPlayer && newVal != 0)
        {
            SetMobilitySecondTalentText();
            RectTransform button = GameObject.Find("MobilityThirdTalentButton").GetComponent<RectTransform>();
            buttonBehaviour.UpdateLeftTalentText("MobilityThirdTalentButton");
            buttonBehaviour.ActivateAllDisabledButtons();

            //Set's the new positions and scales of the second damage talent crystal
            if (syncThirdMobilityTalentLevel == 1)
            {
                Vector3 newPosition = new Vector3(19f, -70f, 0f);
                Vector3 newScale = new Vector3(3f, 2f, 1f);
                button.GetComponent<Image>().color = new Color(0.988f, 0.322f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
            else if (syncThirdMobilityTalentLevel == 2)
            {
                Vector3 newPosition = new Vector3(20f, -77f, 0f);
                Vector3 newScale = new Vector3(4f, 3f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
            else if (syncThirdMobilityTalentLevel == 3)
            {
                Vector3 newPosition = new Vector3(23, -90f, 0f);
                Vector3 newScale = new Vector3(6f, 6f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
        }
    }

    void OnSyncFirstSupportTalentLevelChange(int newVal)
    {
        syncFirstSupportTalentLevel = newVal;

        GetCharacterStats();

        if (isLocalPlayer && newVal != 0)
        {
            SetSupportFirstTalentText();
            RectTransform button = GameObject.Find("SupportFirstTalentButton").GetComponent<RectTransform>();
            buttonBehaviour.UpdateLeftTalentText("SupportFirstTalentButton");
            buttonBehaviour.ActivateAllDisabledButtons();

            //Set's the new positions and scales of the second damage talent crystal
            if (syncFirstSupportTalentLevel == 1)
            {
                Vector3 newPosition = new Vector3(-19f, -70f, 0f);
                Vector3 newScale = new Vector3(3f, 2f, 1f);
                button.GetComponent<Image>().color = Color.green;
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
            else if (syncFirstSupportTalentLevel == 2)
            {
                Vector3 newPosition = new Vector3(-20f, -77f, 0f);
                Vector3 newScale = new Vector3(4f, 3f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
            else if (syncFirstSupportTalentLevel == 3)
            {
                Vector3 newPosition = new Vector3(-23, -90f, 0f);
                Vector3 newScale = new Vector3(6f, 6f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
        }
    }

    void OnSyncSecondSupportTalentLevelChange(int newVal)
    {
        syncSecondSupportTalentLevel = newVal;

        GetCharacterStats();

        if (isLocalPlayer && newVal != 0)
        {
            SetSupportSecondTalentText();
            RectTransform button = GameObject.Find("SupportSecondTalentButton").GetComponent<RectTransform>();
            buttonBehaviour.UpdateLeftTalentText("SupportSecondTalentButton");
            buttonBehaviour.ActivateAllDisabledButtons();

            //Set's the new positions and scales of the second damage talent crystal
            if (syncSecondSupportTalentLevel == 1)
            {
                Vector3 newPosition = new Vector3(-50f, -50f, 0f);
                Vector3 newScale = new Vector3(3f, 2f, 1f);
                button.GetComponent<Image>().color = Color.green;
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
            else if (syncSecondSupportTalentLevel == 2)
            {
                Vector3 newPosition = new Vector3(-58f, -58f, 0f);
                Vector3 newScale = new Vector3(4f, 3f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
            else if (syncSecondSupportTalentLevel == 3)
            {
                Vector3 newPosition = new Vector3(-68f, -68f, 0f);
                Vector3 newScale = new Vector3(6f, 6f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
        }
    }

    void OnSyncThirdSupportTalentLevelChange(int newVal)
    {
        syncThirdSupportTalentLevel = newVal;

        GetCharacterStats();

        if (isLocalPlayer && newVal != 0)
        {
            SetSupportThirdTalentText();
            RectTransform button = GameObject.Find("SupportThirdTalentButton").GetComponent<RectTransform>();
            buttonBehaviour.UpdateLeftTalentText("SupportThirdTalentButton");
            buttonBehaviour.ActivateAllDisabledButtons();

            //Set's the new positions and scales of the second damage talent crystal
            if (syncThirdSupportTalentLevel == 1)
            {
                Vector3 newPosition = new Vector3(-65f, -18f, 0f);
                Vector3 newScale = new Vector3(3f, 2f, 1f);
                button.GetComponent<Image>().color = Color.green;
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
            else if (syncThirdSupportTalentLevel == 2)
            {
                Vector3 newPosition = new Vector3(-78f, -20f, 0f);
                Vector3 newScale = new Vector3(4f, 3f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
            else if (syncThirdSupportTalentLevel == 3)
            {
                Vector3 newPosition = new Vector3(-90f, -23f, 0f);
                Vector3 newScale = new Vector3(6, 6f, 1f);
                button.localPosition = newPosition;
                button.localScale = newScale;
            }
        }
    }

    void RespawnCountdownTimer()
    {
        if (isDead && !menu.menuBool)
        {
            respawnTime = charStats.GetStat("respawnCooldownBase") * charStats.GetStat("respawnCooldownMultiplier");
            if (!respawnCountdownText.isActiveAndEnabled)
            {
                respawnCountdownText.gameObject.SetActive(true);
            }

            respawnCountdownText.text = ((int)(respawnTime - (Network.time - timeOfDeath))).ToString();
        }
        else
        {
            respawnCountdownText.gameObject.SetActive(false);
        }
    }
}

public class Talent
{
    public string talentName;
    public int talentLevel;

    public Talent(string newTalent, int newLevel)
    {
        talentName = newTalent;
        talentLevel = newLevel;
    }
}

public class Effect
{
    public string name;
    public float strength;
    public float duration;
    public double startTime;

    public Effect(string newName, float newStrength, float newDuration, double newStartTime)
    {
        name = newName;
        strength = newStrength;
        duration = newDuration;
        startTime = newStartTime;
    }
}
