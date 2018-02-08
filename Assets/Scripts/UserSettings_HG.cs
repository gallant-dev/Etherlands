using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[CreateAssetMenu(fileName = "UserSettings", menuName = "UserSettings", order = 1)]
public class UserSettings_HG : ScriptableObject {

    public string lastEmailUsed;
    public bool rememberUserEmailBool = false;

    //Mouse Related References.
    [Range(0f, 1f)]
    public float mouseSensitivity = 0.5f;

    public bool invertYAxis = true;

    //Chat Related Refences.
    public float chatPanelAlpha = 0.45f;

    [Range(1f, 14f)]
    public int fontSize = 14;

    public Font localUserChatFont;
    public FontStyle localUserChatFontStyle;
    public Color localUserChatColour = new Color(1f, 1f, 0f, 1f);

    public Font nonLocalUserChatFont;
    public FontStyle nonLocalUserChatFontStyle;
    public Color nonLocalUserChatColour = new Color(1f, 1f, 1f, 1f);
}


