using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TabCycleThroughInputs_HG : MonoBehaviour {

    public EventSystem system;
    public Button submit;

    void Start()
    {
        system = EventSystem.current;
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(system.currentSelectedGameObject.GetComponent<InputField>() != null)
            {
                submit.onClick.Invoke();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.RightShift))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();

            if (next != null && next.isActiveAndEnabled)
            {
                InputField inputField = next.GetComponent<InputField>();
                if (inputField != null)
                {
                    inputField.OnPointerClick(new PointerEventData(system));
                }

                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }
            else if (next != null && !next.isActiveAndEnabled)
            {
                next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
                InputField inputField = next.GetComponent<InputField>();
                if (inputField != null)
                {
                    inputField.OnPointerClick(new PointerEventData(system));
                }

                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }
            else
            {
                next = system.firstSelectedGameObject.GetComponent<Selectable>();
                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }

        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            if (next != null && next.isActiveAndEnabled)
            {
                InputField inputField = next.GetComponent<InputField>();
                if (inputField != null)
                {
                    inputField.OnPointerClick(new PointerEventData(system));
                }

                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }
            else if (next != null && !next.isActiveAndEnabled)
            {
                next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
                InputField inputField = next.GetComponent<InputField>();
                if (inputField != null)
                {
                    inputField.OnPointerClick(new PointerEventData(system));
                }

                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }
            else
            {
                next = system.firstSelectedGameObject.GetComponent<Selectable>();
                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }

        }
    }

}
