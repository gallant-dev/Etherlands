using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Character_Stats : NetworkBehaviour {

	//Talent Names assigned to UI through Player_Stats
	public string firstDamageTalentName;
	public string secondDamageTalentName;
	public string thirdDamageTalentName;
	public string firstDefenseTalentName;
	public string secondDefenseTalentName;
	public string thirdDefenseTalentName;
	public string firstSupportTalentName;
	public string secondSupportTalentName;
	public string thirdSupportTalentName;
	public string firstMobilityTalentName;
	public string secondMobilityTalentName;
	public string thirdMobilityTalentName;

	//Talent Types for UI Keys  - 0 is for passives - 1 is for left mouse button skills - 2 is for right mouse button skills - 3 is for E key skills
	public int firstDamageTalentType;
	public int secondDamageTalentType;
	public int thirdDamageTalentType;
	public int firstDefenseTalentType;
	public int secondDefenseTalentType;
	public int thirdDefenseTalentType;
	public int firstSupportTalentType;
	public int secondSupportTalentType;
	public int thirdSupportTalentType;
	public int firstMobilityTalentType;
	public int secondMobilityTalentType;
	public int thirdMobilityTalentType;



	//Talent Descriptions assigned to UI through Player_Stats
	public string firstDamageTalentDescription;
	public string secondDamageTalentDescription;
	public string thirdDamageTalentDescription;
	public string firstDefenseTalentDescription;
	public string secondDefenseTalentDescription;
	public string thirdDefenseTalentDescription;
	public string firstMobilityTalentDescription;
	public string secondMobilityTalentDescription;
	public string thirdMobilityTalentDescription;
	public string firstSupportTalentDescription;
	public string secondSupportTalentDescription;
	public string thirdSupportTalentDescription;

    private bool hasBaseStats = false;

	Player_Stats playerStats;
    Player_Controller controller;
    NetworkLobbyManager_HG networkManager;
    Animator anim;

    public List<Stat> stats = new List<Stat>();

    // Use this for initialization
    void Awake () {
		playerStats = GetComponent<Player_Stats> ();
		CheckCharacter();
	}

    void Start()
    {
        controller = GetComponent<Player_Controller>();
        anim = GetComponent<Animator>();
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkLobbyManager_HG>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CheckCharacter(){
		if(transform.name.Contains("Gideon")){
			GideonStats ();
		}
	}

    public float GetStat(string statName)
    { 
        return stats.Find(stat => stat.name == statName).value;
    }

    public void SetStat(string statName, float value)
    {
        stats.Find(stat => stat.name == statName).value = value;
    }

    //GideonStats
    void GideonStats(){
        if (hasBaseStats == false) {

            stats.Add(new Stat("primaryAbilityDamageBase", 25f));
            stats.Add(new Stat("primaryAbilityDamageMultiplier", 1f));
            stats.Add(new Stat("primaryAbilityInnerRadiusBase", 1f));
            stats.Add(new Stat("primaryAbilityInnerRadiusMultiplier", 1f));
            stats.Add(new Stat("primaryAbilityOutterRadiusBase", 1.3f));
            stats.Add(new Stat("primaryAbilityOutterRadiusMultiplier", 1f));
            stats.Add(new Stat("primaryAbilityRangeBase", 50f));
            stats.Add(new Stat("primaryAbilityRangeMultiplier", 1f));
            stats.Add(new Stat("primaryAbilityRateBase", 0.5f));
            stats.Add(new Stat("primaryAbilityRateMultiplier", 1f));
            stats.Add(new Stat("primaryAbilityDamageMultiplier", 1f));
            stats.Add(new Stat("primaryAbilityEnergyCostBase", 2.5f));
            stats.Add(new Stat("primaryAbilityEnergyCostMultiplier", 1f));
            stats.Add(new Stat("damageAuraMultiplier", 1f));
            stats.Add(new Stat("primaryAbilityExplosiveForce", 10f));
            stats.Add(new Stat("primaryZoom", 0f));

            stats.Add(new Stat("secondaryAbilityDamageBase", 100f));
            stats.Add(new Stat("secondaryAbilityDamageMultiplier", 1f));
            stats.Add(new Stat("secondaryAbilityInnerRadiusBase", 1f));
            stats.Add(new Stat("secondaryAbilityInnerRadiusMultiplier", 1f));
            stats.Add(new Stat("secondaryAbilityOutterRadiusBase", 2f));
            stats.Add(new Stat("secondaryAbilityOutterRadiusMultiplier", 1f));
            stats.Add(new Stat("secondaryAbilityRangeBase", 100f));
            stats.Add(new Stat("secondaryAbilityRangeMultiplier", 1f));
            stats.Add(new Stat("secondaryAbilityRateBase", 1f));
            stats.Add(new Stat("secondaryAbilityRateMultiplier", 1f));
            stats.Add(new Stat("secondaryAbilityEnergyCostBase", 15f));
            stats.Add(new Stat("secondaryAbilityEnergyCostMultiplier", 1f));
            stats.Add(new Stat("secondaryAbilityKnockbackBase", 1f));
            stats.Add(new Stat("secondaryAbilityKnockbackMultiplier", 1f));
            stats.Add(new Stat("secondaryAbilityExplosiveForce", 10f));
            stats.Add(new Stat("secondaryBeamDuration", 0.5f));
            stats.Add(new Stat("secondaryZoom", 1f));

            stats.Add(new Stat("tertiaryAbilityDamageBase", 100f));
            stats.Add(new Stat("tertiaryAbilityDamageMultiplier", 1f));
            stats.Add(new Stat("tertiaryAbilityInnerRadiusBase", 1f));
            stats.Add(new Stat("tertiaryAbilityInnerRadiusMultiplier", 1f));
            stats.Add(new Stat("tertiaryAbilityOutterRadiusBase", 2f));
            stats.Add(new Stat("tertiaryAbilityOutterRadiusMultiplier", 1f));
            stats.Add(new Stat("tertiaryAbilityRangeBase", 100f));
            stats.Add(new Stat("tertiaryAbilityRangeMultiplier", 1f));
            stats.Add(new Stat("tertiaryAbilityRateBase", 1f));
            stats.Add(new Stat("tertiaryAbilityRateMultiplier", 1f));
            stats.Add(new Stat("tertiaryAbilityEnergyCostBase", 5f));
            stats.Add(new Stat("tertiaryAbilityEnergyCostMultiplier", 1f));
            stats.Add(new Stat("tertiaryAbilityExplosiveForce", 12f));
            stats.Add(new Stat("tertiaryAbilityStunDurationBase", 0.5f));
            stats.Add(new Stat("tertiaryAbilityStunDurationMultiplier", 1f));
            stats.Add(new Stat("tertiaryZoom", 0f));

            stats.Add(new Stat("armourBase", 1f));
            stats.Add(new Stat("armourMultiplier", 1f));
            stats.Add(new Stat("shieldBase", 1f));
            stats.Add(new Stat("shieldMultiplier", 1f));
            stats.Add(new Stat("shieldEnergyCostBase", 2f));
            stats.Add(new Stat("shieldEnergyCostMultiplier", 1f));
            stats.Add(new Stat("healthRegenBase", 1f));
            stats.Add(new Stat("healthRegenMultiplier", 1f));
            stats.Add(new Stat("maxHealthBase", 100f));
            stats.Add(new Stat("maxHealthMultiplier", 1f));
            stats.Add(new Stat("defenseAuraMultiplier", 1f));
            stats.Add(new Stat("respawnCooldownBase", 15f));
            stats.Add(new Stat("respawnCooldownMultiplier", 1f));

            stats.Add(new Stat("healBase", 25f));
            stats.Add(new Stat("healMultiplier", 1f));
            stats.Add(new Stat("healEnergyCostBase", 10f));
            stats.Add(new Stat("healEnergyCostMultiplier", 1f));
            stats.Add(new Stat("energyRegenBase", 2f));
            stats.Add(new Stat("energyRegenMultiplier", 1f));
            stats.Add(new Stat("maxEnergyBase", 100f));
            stats.Add(new Stat("maxEnergyMultiplier", 1f));
            stats.Add(new Stat("energyRegenAuraMultiplier", 1f));
            stats.Add(new Stat("energyRegenAuraRadius", 4f));
            stats.Add(new Stat("repairBase", 1f));
            stats.Add(new Stat("repairMultiplier", 1f));

            stats.Add(new Stat("runSpeedBase", 6.5f));
            stats.Add(new Stat("runSpeedMultiplier", 1f));
            stats.Add(new Stat("jumpHeightBase", 10f));
            stats.Add(new Stat("jumpHeightMultiplier", 1f));
            stats.Add(new Stat("flightSpeedBase", 15f));
            stats.Add(new Stat("flightSpeedMultiplier", 1f));
            stats.Add(new Stat("flightEnergyCostBase", 2f));
            stats.Add(new Stat("flightEnergyCostMultiplier", 1f));
            stats.Add(new Stat("runSpeedAuraMultiplier", 1f));
            stats.Add(new Stat("runSpeedAuraRadius", 4f));
            stats.Add(new Stat("teleportDistanceBase", 3f));
            stats.Add(new Stat("teleportDistanceMultiplier", 1f));

            //Talent Names
            firstDamageTalentName = "Primary Ability";
            secondDamageTalentName = "Secondary Ability";
            thirdDamageTalentName = "Ability [E]";
            firstDefenseTalentName = "Health";
            secondDefenseTalentName = "Armour";
            thirdDefenseTalentName = "Phase";
            firstMobilityTalentName = "Run Speed";
            secondMobilityTalentName = "Jump";
            thirdMobilityTalentName = "Speed Aura";
            firstSupportTalentName = "Energy Regeneration";
            secondSupportTalentName = "Total Energy";
            thirdSupportTalentName = "Energy Regeneration Aura";

            //Talent Types for UI Display
            firstDamageTalentType = 1;
            secondDamageTalentType = 2;
            thirdDamageTalentType = 3;
            firstDefenseTalentType = 0;
            secondDefenseTalentType = 0;
            thirdDefenseTalentType = 0;
            firstMobilityTalentType = 0;
            secondMobilityTalentType = 0;
            thirdMobilityTalentType = 0;
            firstSupportTalentType = 0;
            secondSupportTalentType = 0;
            thirdSupportTalentType = 0;

            firstDamageTalentDescription = "Next Point:" + System.Environment.NewLine + "+50% Damage" + System.Environment.NewLine + "+20%  Base Energy Cost";
            secondDamageTalentDescription = "Next Point:" + System.Environment.NewLine + "+100% Damage" + System.Environment.NewLine + "+20% Splash Radius" + System.Environment.NewLine + "+40% Base Energy Cost";
            thirdDamageTalentDescription = "Next Point: " + System.Environment.NewLine + "+20% Damage" + System.Environment.NewLine + "+20% Range" + System.Environment.NewLine + "+ Stun your enemies for 0.5 seconds" + System.Environment.NewLine + "+20%  Base Energy Cost";
            firstDefenseTalentDescription = "Next Point: " + System.Environment.NewLine + "+20% Max Health" + System.Environment.NewLine + "+50% Health Regeneration";
            secondDefenseTalentDescription = "Next Point: " + System.Environment.NewLine + "+50% Armour";
            thirdDefenseTalentDescription = "Next Point: " + System.Environment.NewLine + "Once every 3 minutes Gideon can phase out, becoming ignoring enemy attacks for 5 seconds.";
			firstMobilityTalentDescription = "Next Point: " + System.Environment.NewLine + "+25% Run Speed";
			secondMobilityTalentDescription = "Next Point: " + System.Environment.NewLine + "+10% Jump Height" + System.Environment.NewLine + "+1 Additional Jump";
            thirdMobilityTalentDescription = "Next Point: " + System.Environment.NewLine +  "+5% Run Speed to Nearby Allies";
			firstSupportTalentDescription = "Next Point: " + System.Environment.NewLine + "+25% Energy Regeneration" + System.Environment.NewLine + "+ 40 % Max Energy";
            secondSupportTalentDescription = "Next Point: " + System.Environment.NewLine + "Secondary attack creates an expanding ether mine when it hits friendly blocks. This can be used to defend, or give allies a pad to launch off.";
			thirdSupportTalentDescription = "Next Point:" + System.Environment.NewLine + "+20%  Energy Regen to Nearby Allies";

            hasBaseStats = true;
		}

		if(playerStats.syncFirstDamageTalentLevel >= 1){
            SetStat("primaryAbilityEnergyCostMultiplier", 1.2f);
            SetStat("primaryAbilityDamageMultiplier", 1.5f);
			firstDamageTalentDescription = "Current Bonus:" + System.Environment.NewLine  + "+50% Damage" + System.Environment.NewLine + "+20%  Base Energy Cost" + System.Environment.NewLine + System.Environment.NewLine + "Next Point:" + System.Environment.NewLine + "+100% Damage" + System.Environment.NewLine + "+20% Splash Radius" + System.Environment.NewLine + "+40% Base Energy Cost";
		}
		if(playerStats.syncFirstDamageTalentLevel >= 2 ){
            SetStat("primaryAbilityEnergyCostMultiplier", 1.4f);
            SetStat("primaryAbilityDamageMultiplier", 1.5f);
            SetStat("primaryAbilityInnerRadiusMultiplier", 1.1f);
            SetStat("primaryAbilityOutterRadiusBase", 1.2f);
			firstDamageTalentDescription = "Current Bonus:"+ System.Environment.NewLine +"+100% Damage" + System.Environment.NewLine + "+20% Splash Radius" + System.Environment.NewLine + "+40% Base Energy Cost" + System.Environment.NewLine + System.Environment.NewLine + "Next Point:"+ System.Environment.NewLine +"+200% Damage" + System.Environment.NewLine + "+20% Splash Radius" + System.Environment.NewLine + "+60% Base Energy Cost";
		}
		if(playerStats.syncFirstDamageTalentLevel == 3){
            SetStat("primaryAbilityEnergyCostMultiplier", 1.6f);
            SetStat("primaryAbilityDamageMultiplier", 2.0f);
            SetStat("primaryAbilityInnerRadiusMultiplier", 1.15f);
            SetStat("primaryAbilityOutterRadiusBase", 1.3f);
			firstDamageTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+200% Damage" + System.Environment.NewLine + "+20% Splash Radius" + System.Environment.NewLine + "+60% Base Energy Cost";
		}
		if (playerStats.syncSecondDamageTalentLevel >= 1) {
            SetStat("secondaryAbilityEnergyCostMultiplier", 1.2f);
            SetStat("secondaryAbilityDamageMultiplier", 1.2f);
            SetStat("secondaryAbilityRangeMultiplier", 1.2f);
            secondDamageTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+20% Damage"+ System.Environment.NewLine + "+20% Range" + System.Environment.NewLine + "+20%  Base Energy Cost" + System.Environment.NewLine + System.Environment.NewLine + "Next Point:"+  System.Environment.NewLine + "+50% Damage" + System.Environment.NewLine + "+40%  Base Energy Cost";
		}
		if (playerStats.syncSecondDamageTalentLevel >= 2) {
            SetStat("secondaryAbilityEnergyCostMultiplier", 1.4f);
            SetStat("secondaryAbilityDamageMultiplier", 1.5f);
            SetStat("secondaryAbilityRangeMultiplier", 1.4f);
			secondDamageTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+50% Damage" + System.Environment.NewLine + "+40% Range" + System.Environment.NewLine + "+40%  Base Energy Cost" + System.Environment.NewLine + System.Environment.NewLine + "Next Point:" + System.Environment.NewLine + "+70% Damage" + System.Environment.NewLine + "+60% Range" + System.Environment.NewLine + "+60%  Base Energy Cost";
		}
		if (playerStats.syncSecondDamageTalentLevel == 3) {
            SetStat("secondaryAbilityEnergyCostMultiplier", 1.6f);
            SetStat("secondaryAbilityDamageMultiplier", 1.7f);
            SetStat("secondaryAbilityRangeMultiplier", 1.6f);
			secondDamageTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+70% Damage" + System.Environment.NewLine + "+60% Range" + System.Environment.NewLine + "+60%  Base Energy Cost";
		}
		if (playerStats.syncThirdDamageTalentLevel >= 1) {
            SetStat("tertiaryAbilityDamageMultiplier", 1.5f);
            SetStat("tertiaryAbilityInnerRadiusMultiplier", 1.2f);
            SetStat("tertiaryAbilityOutterRadiusMultiplier", 1.2f);
            SetStat("tertiaryAbilityEnergyCostMultiplier", 1.2f);
            thirdDamageTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+50% Damage" + System.Environment.NewLine + "+20% Splash Radius" + System.Environment.NewLine + "+20%  Base Energy Cost" + System.Environment.NewLine + "+ Stun your enemies for 0.5 seconds" + System.Environment.NewLine + System.Environment.NewLine + "Next Point:" + System.Environment.NewLine + "+70% Damage" + System.Environment.NewLine + "+30% Splash Radius" + System.Environment.NewLine + "+ Stun your enemies for 0.75 seconds" + System.Environment.NewLine + "+40%  Base Energy Cost";
		}
		if (playerStats.syncThirdDamageTalentLevel >= 2) {
            SetStat("tertiaryAbilityDamageMultiplier", 1.7f);
            SetStat("tertiaryAbilityInnerRadiusMultiplier", 1.3f);
            SetStat("tertiaryAbilityOutterRadiusMultiplier", 1.3f);
            SetStat("tertiaryAbilityEnergyCostMultiplier", 1.4f);
            SetStat("tertiaryAbilityStunDurationMultiplier", 1.5f);
            thirdDamageTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+70% Damage" + System.Environment.NewLine + "+30% Splash Radius" + System.Environment.NewLine + "+40%  Base Energy Cost" + System.Environment.NewLine + "+ Stun your enemies for 0.75 seconds" + System.Environment.NewLine + System.Environment.NewLine +	"Next Point:" + System.Environment.NewLine + "+90% Damage" + System.Environment.NewLine + "+40% Splash Radius" + System.Environment.NewLine + "+ Stun your enemies for 0.95 seconds" + System.Environment.NewLine + "+90%  Base Energy Cost";		
		}
		if (playerStats.syncThirdDamageTalentLevel == 3) {
            SetStat("tertiaryAbilityDamageMultiplier", 1.9f);
            SetStat("tertiaryAbilityInnerRadiusMultiplier", 1.4f);
            SetStat("tertiaryAbilityOutterRadiusMultiplier", 1.4f);
            SetStat("tertiaryAbilityEnergyCostMultiplier", 1.8f);
            SetStat("tertiaryAbilityStunDurationMultiplier", 1.9f);
            thirdDamageTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+90% Damage" + System.Environment.NewLine + "+40% Splash Radius" + System.Environment.NewLine + "+ Stun your enemies for 0.95 seconds" + System.Environment.NewLine + "+90%  Base Energy Cost";		
				
		}
		if (playerStats.syncFirstDefenseTalentLevel >= 1) {
            SetStat("maxHealthMultiplier", 1.2f);
            SetStat("healthRegenMultiplier", 1.5f);
            firstDefenseTalentDescription = ("Current Bonus:" + System.Environment.NewLine + "+20% Max Health" + System.Environment.NewLine + "+50% Health Regeneration" + System.Environment.NewLine + System.Environment.NewLine + "Next Point:" + System.Environment.NewLine + "+40% Max Health" + System.Environment.NewLine + "+80% Health Regeneration");
		}
		if (playerStats.syncFirstDefenseTalentLevel >= 2) {
            SetStat("maxHealthMultiplier", 1.4f);
            SetStat("healthRegenMultiplier", 1.8f);
			firstDefenseTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+40% Max Health" + System.Environment.NewLine + "+80% Health Regeneration" + System.Environment.NewLine + System.Environment.NewLine + "Next Point:" + System.Environment.NewLine + "+60% Max Health" + System.Environment.NewLine + "+120% Health Regeneration";
		}
		if (playerStats.syncFirstDefenseTalentLevel == 3) {
            SetStat("maxHealthMultiplier", 1.6f);
            SetStat("healthRegenMultiplier", 2.2f);
			firstDefenseTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+60% Max Health" + System.Environment.NewLine + "+120% Health Regeneration";
		}
		if (playerStats.syncSecondDefenseTalentLevel >= 1) {
            SetStat("armourMultiplier", 1.5f);
			secondDefenseTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+50% Armour" + System.Environment.NewLine + System.Environment.NewLine + "Next Point:" + System.Environment.NewLine + "+75% Amour";
		}
		if (playerStats.syncSecondDefenseTalentLevel >= 2) {
            SetStat("armourMultiplier", 1.75f);
			secondDefenseTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+75% Armour" + System.Environment.NewLine + System.Environment.NewLine + "Next Point:" + System.Environment.NewLine + "+100% Amour";
		}
		if (playerStats.syncSecondDefenseTalentLevel == 3) {
            SetStat("armourMultiplier", 2f);
			secondDefenseTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+100% Armour";
		}
		if (playerStats.syncThirdDefenseTalentLevel >= 1) {
			thirdDefenseTalentDescription = "Current Bonus:" + System.Environment.NewLine + "Once every 3 minutes Gideon can phase out, becoming ignoring enemy attacks for 5 seconds." + System.Environment.NewLine + System.Environment.NewLine + "Next Point:" + System.Environment.NewLine + "Once every 2.5 minutes Gideon can phase out, becoming ignoring enemy attacks for 5 seconds.";
		}
		if (playerStats.syncThirdDefenseTalentLevel >= 2) {
			thirdDefenseTalentDescription = "Current Bonus:" + System.Environment.NewLine + "Once every 2.5 minutes Gideon can phase out, becoming ignoring enemy attacks for 5 seconds."+ System.Environment.NewLine + System.Environment.NewLine + "Next Point:" + System.Environment.NewLine + "Once every 2 minutes Gideon can phase out, becoming ignoring enemy attacks and ether damage for 5 seconds.";
		}
		if (playerStats.syncThirdDefenseTalentLevel == 3) {
			thirdDefenseTalentDescription = "Current Bonus:" + System.Environment.NewLine + "Once every 2 minutes Gideon can phase out, becoming ignoring enemy attacks and ether damage for 5 seconds.";
		}
		if (playerStats.syncFirstMobilityTalentLevel >= 1) {
            SetStat("runSpeedMultiplier", 1.25f);
			firstMobilityTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+25% Run Speed" + System.Environment.NewLine + System.Environment.NewLine + "Next Point:" + System.Environment.NewLine + "+50% Run Speed";
		}
		if (playerStats.syncFirstMobilityTalentLevel >= 2) {
            SetStat("runSpeedMultiplier", 1.5f);
			firstMobilityTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+50% Run Speed" + System.Environment.NewLine + System.Environment.NewLine + "Next Point:" + System.Environment.NewLine + "+75% Run Speed";
		}
		if (playerStats.syncFirstMobilityTalentLevel == 3) {
            SetStat("runSpeedMultiplier", 1.75f);
			firstMobilityTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+75% Run Speed";
		}
		if (playerStats.syncSecondMobilityTalentLevel >= 1) {
            SetStat("jumpHeightMultiplier", 1.10f);
			secondMobilityTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+10% Jump Height" + System.Environment.NewLine + "+1 Jump" + System.Environment.NewLine + System.Environment.NewLine + "Next Point:" + System.Environment.NewLine + "+25% Jump Height" + System.Environment.NewLine + "+2 Jump";
        }
		if (playerStats.syncSecondMobilityTalentLevel >= 2) {
            SetStat("jumpHeightMultiplier", 1.25f);
			secondMobilityTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+25% Jump Height" + System.Environment.NewLine + "+2 Jumps" + System.Environment.NewLine + System.Environment.NewLine + "Next Point:" + System.Environment.NewLine + "40% Jump Height";
		}
		if (playerStats.syncSecondMobilityTalentLevel == 3) {
            SetStat("jumpHeightMultiplier", 1.40f);
            secondMobilityTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+40% Jump Height" + System.Environment.NewLine + "+Flying";
		}
		if (playerStats.syncThirdMobilityTalentLevel >= 1) {
            SetStat("runSpeedAuraMultiplier", 1.05f);
			thirdMobilityTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+5% Run Speed to Nearby Allies" + System.Environment.NewLine + System.Environment.NewLine + "Next Point:" + System.Environment.NewLine + "+10% Run Speed to Nearby Allies";
		}
		if (playerStats.syncThirdMobilityTalentLevel >= 2) {
            SetStat("runSpeedAuraMultiplier", 1.10f);
			thirdMobilityTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+10% Run Speed to Nearby Allies" + System.Environment.NewLine + System.Environment.NewLine + "Next Point:" + System.Environment.NewLine + "+15% Run Speed to Nearby Allies";
		}
		if (playerStats.syncThirdMobilityTalentLevel == 3) {
            SetStat("runSpeedAuraMultiplier", 1.15f);
			thirdMobilityTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+15% Run Speed to Nearby Allies";

		}
		if (playerStats.syncFirstSupportTalentLevel >= 1) {
            SetStat("energyRegenMultiplier", 1.25f);
			firstSupportTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+40 Max Energy" + System.Environment.NewLine + "+25% Energy Regeneration" + System.Environment.NewLine + System.Environment.NewLine + "Next Point:" + System.Environment.NewLine + "+80% Max Energy" + System.Environment.NewLine + "+50% Energy Regeneration";
		}
		if (playerStats.syncFirstSupportTalentLevel >= 2) {
            SetStat("energyRegenMultiplier", 1.5f);
			firstSupportTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+80%  Max Energy" + System.Environment.NewLine + "+50% Energy Regeneration" + System.Environment.NewLine + System.Environment.NewLine + "Next Point:" + System.Environment.NewLine + "+120% Max Energy" + System.Environment.NewLine + "+100% Energy Regeneration";
		}
		if (playerStats.syncFirstSupportTalentLevel == 3) {
            SetStat("energyRegenMultiplier", 2f);
			firstSupportTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+120%  Max Energy" + System.Environment.NewLine + "+100% Energy Regeneration";
		}
		if (playerStats.syncSecondSupportTalentLevel >= 1) {
            SetStat("maxEnergyMultiplier", 1.4f);
			secondSupportTalentDescription = "Current Bonus:" + System.Environment.NewLine + "Secondary attack creates an expanding ether mine on friendly blocks. By knocking the player in the opposite direction of impact, this can be used to defend, or give allies a pad to launch off." + System.Environment.NewLine + System.Environment.NewLine + "120% Knockback" + System.Environment.NewLine + System.Environment.NewLine + "Next Point:" + System.Environment.NewLine + "140% Knockback";
		}
		if (playerStats.syncSecondSupportTalentLevel >= 2) {
			secondSupportTalentDescription = "Current Bonus:" + System.Environment.NewLine + "Secondary attack creates an expanding ether mine on friendly blocks. By knocking the player in the opposite direction of impact, this can be used to defend, or give allies a pad to launch off." + System.Environment.NewLine + System.Environment.NewLine + "140% Knockback" + System.Environment.NewLine + "Next Point:" + System.Environment.NewLine + "160% Knockback";
		}
		if (playerStats.syncSecondSupportTalentLevel == 3) {
			secondSupportTalentDescription = "Current Bonus:" + System.Environment.NewLine + "Secondary attack creates an expanding ether mine on friendly blocks. By knocking the player in the opposite direction of impact, this can be used to defend, or give allies a pad to launch off." + System.Environment.NewLine + System.Environment.NewLine + "160% Knockback";
        }
		if (playerStats.syncThirdSupportTalentLevel >= 1) {
			thirdSupportTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+5%  Energy Regen to Nearby Allies" + System.Environment.NewLine + System.Environment.NewLine + "Next Point:" + System.Environment.NewLine + "+10% Energy Regen to Nearby Allies";
		}
		if (playerStats.syncThirdSupportTalentLevel >= 2) {
            SetStat("energyRegenAuraMultiplier", 1.1f);
            thirdSupportTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+10%  Energy Regen to Nearby Allies" + System.Environment.NewLine + System.Environment.NewLine + "Next Point:" + System.Environment.NewLine + "+20% Energy Regen to Nearby Allies";
		}
		if (playerStats.syncThirdSupportTalentLevel == 3) {
            SetStat("energyRegenAuraMultiplier", 1.2f);
            thirdSupportTalentDescription = "Current Bonus:" + System.Environment.NewLine + "+20%  Energy Regen to Nearby Allies";
		} 
	}

    //Gideon's 'Left Click'
    public void GideonPrimaryAbility()
    {
        GameObject clone = (GameObject)Instantiate(controller.projectileArray[0], controller.projectileOriginsArray[0].position, controller.projectileOriginsArray[0].rotation);
        clone.GetComponent<GideonPrimaryProjectile>().player = controller.gameObject;
        clone.SetActive(true);
        clone.GetComponent<Rigidbody>().velocity = clone.transform.forward * GetStat("primaryAbilityRangeBase") * GetStat("primaryAbilityRangeMultiplier");
        NetworkServer.Spawn(clone);
        playerStats.TakeEnergy(GetStat("primaryAbilityEnergyCostBase") * GetStat("primaryAbilityEnergyCostMultiplier"));
    }

    //Gideon's 'Right Click'
    public void GideonSecondaryAbility()
    {
       if (playerStats.energy >= GetStat("secondaryAbilityEnergyCostBase") * GetStat("secondaryAbilityEnergyCostMultiplier"))
        {
            controller.beamOrigin = controller.projectileOriginsArray[0];

            StopCoroutine(controller.Beam());
            StartCoroutine(controller.Beam());

            controller.beamStart = Network.time;


            RaycastHit hit;
            if (Physics.Raycast(controller.shooter.TransformPoint(0f, 0f, 0.5f), controller.shooter.forward, out hit, GetStat("secondaryAbilityRangeBase") * GetStat("secondaryAbilityRangeMultiplier")))
            {
                Debug.Log(hit.transform.position);

                if(networkManager.bluePlayerList.Find(player => player.playerName == controller.localGamePlayer.playerName) != null && hit.transform.IsChildOf(controller.blueSide))
                {
                    if (hit.transform.tag == "destructible" && playerStats.syncSecondSupportTalentLevel >= 1)
                    {
                        GameObject clone = (GameObject)Instantiate(controller.projectileArray[1], hit.transform.position, hit.transform.rotation);
                        clone.GetComponent<GideonSecondaryProjectile>().controller = controller;
                        NetworkServer.Spawn(clone);
                    }
                    else
                    {
                        controller.ApplyDamage(hit.transform.position, 1, true);
                    }
                }
                else if (networkManager.redPlayerList.Find(player => player.playerName == controller.localGamePlayer.playerName) != null && hit.transform.IsChildOf(controller.redSide))
                {
                    if (hit.transform.tag == "destructible" && playerStats.syncSecondSupportTalentLevel >= 1)
                    {
                        GameObject clone = (GameObject)Instantiate(controller.projectileArray[1], hit.transform.position, hit.transform.rotation);
                        GideonSecondaryProjectile projectileScript = clone.GetComponent<GideonSecondaryProjectile>();
                        projectileScript.controller = controller;
                        projectileScript.oppositeForceApplied = GetStat("secondaryAbilityKnockbackBase") * GetStat("secondaryAbilityKnockbackBase");
                        NetworkServer.Spawn(clone);
                    }
                    else
                    {
                        controller.ApplyDamage(hit.transform.position, 1, true);
                    }
                }
                else
                {
                    controller.ApplyDamage(hit.transform.position, 1, true);
                }



                playerStats.TakeEnergy(GetStat("secondaryAbilityEnergyCostBase") * GetStat("secondaryAbilityEnergyCostMultiplier"));
            }

            controller.secondaryAbilityDelay = Network.time;
        }
    }

    //Gideon's 'E'
    public void GideonTertiaryAbility()
    {
        if (anim.GetBool("IsJumping"))
        {
            GameObject clone = (GameObject)Instantiate(controller.projectileArray[2], controller.projectileOriginsArray[1].position, transform.rotation);
            clone.GetComponent<GideonTertiaryProjectile>().controller = controller;
            NetworkServer.Spawn(clone);
            controller.RpcSetProjectileParent(1, clone);
            playerStats.TakeEnergy(GetStat("tertiaryAbilityEnergyCostBase") * GetStat("tertiaryAbilityEnergyCostMultiplier"));
        }
        else
        {
            //need to fill else.
        }

    }

    public void GideonSpace()
    {
        //Prevent jumping too fast after last jump.
        if (controller.lastJumpTime + controller.jumpRepeatTime > Time.time)
            return;
        int count = controller.jumpCount;
        if (count <= playerStats.syncSecondMobilityTalentLevel)
        {
            if (count <= 2 /*|| (count >= 3 && playerStats.energy <= (GetStat("flightEnergyCostBase") * GetStat("flightEnergyCostMultiplier")))*/)
            {
                controller.verticalSpeed = controller.CalculateJumpVerticalSpeed(GetStat("jumpHeightBase") * GetStat("jumpHeightMultiplier"));
                controller.anim.SetBool("IsJumping", true);
                //controller.inAirControlAcceleration = 5f;
                controller.DidJump();
                controller.jumpCount++;
            }
            else if(count >= 3 && playerStats.energy >= (GetStat("flightEnergyCostBase") * GetStat("flightEnergyCostMultiplier")))
            {
                controller.verticalSpeed = controller.CalculateJumpVerticalSpeed(GetStat("jumpHeightBase") * GetStat("jumpHeightMultiplier"));
                controller.anim.SetBool("IsFlying", true);
                controller.inAirControlAcceleration = GetStat("flightSpeedBase") * GetStat("flightSpeedMultiplier");
                playerStats.TakeEffect("moveSpeed", GetStat("jumpHeightBase") * GetStat("jumpHeightMultiplier") * 0.1f, 1f) ;
                playerStats.TakeEnergy(GetStat("flightEnergyCostBase") * GetStat("flightEnergyCostMultiplier"));

            }
        }
    }

    public void GideonAuras(GameObject target)
    {
        if (playerStats.syncThirdMobilityTalentLevel >= 1)
        {
            controller.GetComponent<Player_Controller>().ApplyEffect(target, "moveSpeed", GetStat("runSpeedAuraMultiplier"), 2f);

        }
        if (playerStats.syncThirdSupportTalentLevel >= 1)
        {
            controller.ApplyEffect(target, "energyRegen", GetStat("energyRegenAuraMultiplier"), 2f);
        }
    }
}

public class Stat
{
    public string name;
    public float value;

    public Stat (string newName, float newValue)
    {
        name = newName;
        value = newValue;
    }
}
