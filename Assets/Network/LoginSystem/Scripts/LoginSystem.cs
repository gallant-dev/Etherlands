using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.UI;
using System.Collections;

public class LoginSystem : MonoBehaviour {
	//Public Variables
	public string email;
	public string password;
    bool shouldRememberEmail;

	public int CurrentMenu = 1;

	//Private Variables
	//private string domainURL = "http://huntingtongameworks.com/";
	private string createAccountUrl = "http://huntingtongameworks.com/CreateAccount.php";
	private string loginUrl = "http://huntingtongameworks.com/AccountLogin.php";
	private string confirmEmail;
	private string confirmPassword;

	public string playerDisplayImagePath; 

	Image loginBox;
	Text accountLoginText;
	Text displaynameText;

	//InputFields
	InputField displayNameField;
	InputField emailField;
	InputField confirmEmailField;
	InputField passwordField;
	InputField confirmPasswordField;
	public Dropdown matchTypeDropdown;
    Toggle rememberUserEmailToggle;

	//Buttons
	Button loginButton;
	Button createAccountButton;
	Button cancelButton;
	Button playButton;

    NetworkLobbyManager_HG networkManager;
    public UserSettings_HG userSettings;

	// Use this for initialization
	void Start () {
		loginBox = GameObject.Find ("LoginBox").GetComponent<Image>();
		accountLoginText = GameObject.Find ("AccountLoginText").GetComponent<Text>();
		displaynameText = GameObject.Find ("DisplayNameText").GetComponent<Text> ();
		//Input Fields
		displayNameField = GameObject.Find("DisplayNameField").GetComponent<InputField>();
		emailField = GameObject.Find("EmailField").GetComponent<InputField>();
		confirmEmailField = GameObject.Find ("EmailFieldConfirm").GetComponent<InputField>();
		passwordField = GameObject.Find ("PasswordField").GetComponent<InputField>();
		confirmPasswordField = GameObject.Find ("PasswordFieldConfirm").GetComponent<InputField>();
		matchTypeDropdown = GameObject.Find ("MatchTypeDropdown").GetComponent<Dropdown> ();
        rememberUserEmailToggle = GetComponentInChildren<Toggle>();
        rememberUserEmailToggle.isOn = userSettings.rememberUserEmailBool;

        //Buttons
        loginButton = GameObject.Find ("LoginButton").GetComponent<Button>();
		createAccountButton = GameObject.Find ("CreateAccountButton").GetComponent<Button>();
		cancelButton = GameObject.Find ("CancelButton").GetComponent<Button>();
		playButton = GameObject.Find ("PlayButton").GetComponent<Button> ();

		//Network Manager
		networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkLobbyManager_HG>();

        //Get UserSettings_HG
        emailField.text = userSettings.lastEmailUsed;


        displaynameText.text = "";
		displayNameField.gameObject.SetActive (false);
		confirmEmailField.gameObject.SetActive (false);
		confirmPasswordField.gameObject.SetActive(false);
		matchTypeDropdown.gameObject.SetActive (false);
		playButton.gameObject.SetActive (false);
	}

    void Update()
    {
        if(CurrentMenu == 0 && Input.anyKey)
        {
            LoginMenu();
            Debug.Log("Key Pressed");
        }
    }

	void ResetMenuFields(){
		displayNameField.GetComponent<InputField>().text ="";
        if (!rememberUserEmailToggle.isOn)
        {
            emailField.GetComponent<InputField>().text = "";
        }
		confirmEmailField.GetComponent<InputField>().text ="";
		passwordField.GetComponent<InputField>().text ="";
		confirmPasswordField.GetComponent<InputField>().text ="";
		matchTypeDropdown.value = 0;
	}

	void LoginMenu(){
        
        //Open login box.
        if (!loginBox.isActiveAndEnabled)
        {
            loginBox.gameObject.SetActive(true);
        }

        CurrentMenu = 1;

		accountLoginText.text = "Log Into Your Account";
		displaynameText.text = "";

		//ResetFieldValues
		ResetMenuFields();

        //ToggleFields
        emailField.gameObject.SetActive(true);
        passwordField.gameObject.SetActive(true);
		displayNameField.gameObject.SetActive (false);
		confirmEmailField.gameObject.SetActive(false);
		confirmPasswordField.gameObject.SetActive(false);
		matchTypeDropdown.gameObject.SetActive (false);
        rememberUserEmailToggle.gameObject.SetActive(true);

        //ToggleButtons
        playButton.gameObject.SetActive (false);
		createAccountButton.gameObject.SetActive (true);
		loginButton.gameObject.SetActive(true);

        //Resize Login Box
        loginBox.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200f);

        //Get Create Button Rect Transform
        RectTransform buttonTransform = createAccountButton.GetComponent<RectTransform> ();
		Vector3 newPosition = new Vector3 (65f, -35f, 0f);
		buttonTransform.localScale = Vector3.one;
		buttonTransform.localPosition = newPosition;


        accountLoginText.GetComponent<RectTransform>().localPosition = new Vector3(0f, 76f, 0f);
        cancelButton.GetComponent<RectTransform>().localPosition = new Vector3(0f, -70f, 0f);
        emailField.GetComponent<RectTransform>().localPosition = new Vector3(0f, 35f, 0f);
        passwordField.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);

        //Set explicit navigation references for tab targeting.
        Navigation emailFieldNavigation = emailField.navigation;
        Navigation passwordFieldNavigation = passwordField.navigation;
        Navigation createAccountButtonNavigation = createAccountButton.navigation;
        Navigation cancelButtonNavigation = cancelButton.navigation;

        emailFieldNavigation.selectOnUp = cancelButton;
        emailFieldNavigation.selectOnDown = passwordField;
        passwordFieldNavigation.selectOnUp = emailField;
        passwordFieldNavigation.selectOnDown = loginButton;
        createAccountButtonNavigation.selectOnUp = loginButton;
        createAccountButtonNavigation.selectOnDown = cancelButton;
        cancelButtonNavigation.selectOnDown = emailField;

        emailField.navigation = emailFieldNavigation;
        passwordField.navigation = passwordFieldNavigation;
        createAccountButton.navigation = createAccountButtonNavigation;
        cancelButton.navigation = cancelButtonNavigation;

        //Set selected UI objects.
        GetComponent<TabCycleThroughInputs_HG>().system.SetSelectedGameObject(emailField.gameObject);
        GetComponent<TabCycleThroughInputs_HG>().submit = loginButton;
    }

	void CreateAccountMenu(){
		accountLoginText.text = "Create An Account";

		displayNameField.gameObject.SetActive (true);
		loginButton.gameObject.SetActive (false);
		confirmEmailField.gameObject.SetActive(true);
		confirmPasswordField.gameObject.SetActive(true);
        rememberUserEmailToggle.gameObject.SetActive(false);

        //Resize Login Box
        loginBox.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 300f);

		//Reposition buttons and fields
		RectTransform buttonTransform = createAccountButton.GetComponent<RectTransform> ();
		buttonTransform.localScale = Vector3.one;
		buttonTransform.localPosition = new Vector3(0f, -80f, 0f); ;
        
        accountLoginText.GetComponent<RectTransform>().localPosition = new Vector3(0f, 125f, 0f);
        cancelButton.GetComponent<RectTransform>().localPosition = new Vector3 (0f,-120f, 0f);
        emailField.GetComponent<RectTransform>().localPosition = new Vector3(0f, 60f, 0f);
        passwordField.GetComponent<RectTransform>().localPosition = new Vector3(0f, -5.5f, 0f);

        //Set explicit navigation references for tab targeting.
        Navigation emailFieldNavigation = emailField.navigation;
        Navigation passwordFieldNavigation = passwordField.navigation;
        Navigation createAccountButtonNavigation = createAccountButton.navigation;
        Navigation cancelButtonNavigation = cancelButton.navigation;

        emailFieldNavigation.selectOnUp = displayNameField;
        emailFieldNavigation.selectOnDown = confirmEmailField;
        passwordFieldNavigation.selectOnUp = confirmEmailField;
        passwordFieldNavigation.selectOnDown = confirmPasswordField;
        createAccountButtonNavigation.selectOnUp = confirmPasswordField;
        createAccountButtonNavigation.selectOnDown = cancelButton;
        cancelButtonNavigation.selectOnDown = displayNameField;

        emailField.navigation = emailFieldNavigation;
        passwordField.navigation = passwordFieldNavigation;
        createAccountButton.navigation = createAccountButtonNavigation;
        cancelButton.navigation = cancelButtonNavigation;

        //Set selected UI objects.
        GetComponent<TabCycleThroughInputs_HG>().system.SetSelectedGameObject(displayNameField.gameObject);
        GetComponent<TabCycleThroughInputs_HG>().submit = createAccountButton;

    }

	public void MenuChange(Button button){
		// 0 = No Menu; 1= Login Menu; 2 = CreateAccountMenu; 3 = MatchSelectMenu

		if (button.name == "LoginButton") {
			CurrentMenu = 1;
			StartCoroutine ("AccountLogin");
        }
        else if (button.name == "CreateAccountButton")
        {
			if (CurrentMenu == 1) {
				CurrentMenu = 2;

				//ResetFieldValues
				ResetMenuFields();
			
				CreateAccountMenu ();

			} else if (CurrentMenu == 2) {
				//Start Coroutine to actually create the account.
				StartCoroutine ("CreateAccount");
			}
		}
        else if(button.name == "CancelButton")
        {
			if (CurrentMenu == 1) {
				CurrentMenu = 0;
				loginBox.gameObject.SetActive (false);

				//ResetFieldValues
				ResetMenuFields ();

			}

				if (CurrentMenu == 2) {
				//Go Back To Log In Menu
				CurrentMenu = 1;
				LoginMenu ();
				ResetMenuFields ();
			}

			if (CurrentMenu == 3) {
				CurrentMenu = 1;
				LoginMenu ();
				ResetMenuFields ();

                //Open remember email toggle.
                rememberUserEmailToggle.gameObject.SetActive(true);
            }
		}else if (button.name == "PlayButton") {
			if (matchTypeDropdown.value == 0) {
				//Start 5vs5 Etherlands Arena
				//For Unity Multiplayer
				networkManager.MatchMaker(10);

			}
			if (matchTypeDropdown.value == 1) {
                //Start 4vs4 Etherlands Arena
                networkManager.MatchMaker(8);
            }
			if (matchTypeDropdown.value == 2) {
                //Start Training
                networkManager.MatchMaker(2);
            }
		}
	}

//	Coroutines
	IEnumerator CreateAccount(){
		WWWForm Form = new WWWForm ();
		Form.AddField ("DisplayName", displayNameField.GetComponent<InputField>().text);
		Form.AddField ("Email", emailField.GetComponent<InputField>().text);
		Form.AddField ("Password", passwordField.GetComponent<InputField>().text);

		WWW createAccountWWW = new WWW (createAccountUrl, Form);
		//wait for php script to send something back to Unity
		yield return createAccountWWW;

		if (createAccountWWW.error != null) {
			Debug.LogError ("Cannot Connect to Account Creation");
		} else {
			string createAccountReturn = createAccountWWW.text;
			if (createAccountReturn.Contains("Success")) {
				Debug.Log ("Success Account Created");
				LoginMenu ();
			}
		}
	}

	IEnumerator AccountLogin(){
		WWWForm Form = new WWWForm ();
		Form.AddField ("Email", emailField.GetComponent<InputField>().text);
		Form.AddField ("Password", passwordField.GetComponent<InputField>().text);

		WWW accountLoginWWW = new WWW (loginUrl, Form);
		//wait for php script to send something back to Unity
		yield return accountLoginWWW;   

		if (/*accountLoginWWW.error != null ||*/ !accountLoginWWW.text.Contains("Success")) {
            Debug.Log(accountLoginWWW.text);
			Debug.LogError ("Cannot Connect to Account Login");
		} else {
			string accountLoginReturn = accountLoginWWW.text;
			if (accountLoginReturn.Contains("Success")) {
				CurrentMenu = 3;

				accountLoginReturn = accountLoginReturn.Replace ("Success", "");
		
				string[] accountInfo  = accountLoginReturn.Split (':');
				string displayName = accountInfo[0];
				accountLoginText.text = "Welcome";
				displaynameText.text = displayName + "!";
				networkManager.localPlayerDisplayName = displayName;

                //Check to remember email.
                if (shouldRememberEmail)
                {
                    userSettings.rememberUserEmailBool = true;
                    userSettings.lastEmailUsed = emailField.text;
                }
                else
                {
                    userSettings.rememberUserEmailBool = false;
                    userSettings.lastEmailUsed = "";
                }

                //Close remember email toggle.
                rememberUserEmailToggle.gameObject.SetActive(false);

				//CloseForms
				emailField.gameObject.SetActive(false);
				passwordField.gameObject.SetActive (false);
				loginButton.gameObject.SetActive (false);
				createAccountButton.gameObject.SetActive (false);

				//OpenMatchTypeMenus
				matchTypeDropdown.gameObject.SetActive(true);
				playButton.gameObject.SetActive (true);

                //Set current selected gameObject to the play button.
                GetComponent<TabCycleThroughInputs_HG>().system.SetSelectedGameObject(playButton.gameObject);

                Debug.Log ("Login Successful!");
			}
		}
	}

    public void RememberUserEmailToggle()
    {
        if(shouldRememberEmail == false)
        {
            shouldRememberEmail = true;
        }
        else
        {
            shouldRememberEmail = false;
        }

    }
}
