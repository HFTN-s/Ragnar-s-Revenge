using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleTrigger : MonoBehaviour
{
    [SerializeField] private BoxCollider puzzleTrigger;
    [SerializeField] private TutorialLevelManager levelManager;
    public string variableName;  // The name of the variable to set in levelManager
    public UnityEvent onTrigger; // The event to trigger when the player enters the trigger

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Use reflection to find and set the boolean variable by name , delete object once triggered and code successfully executed
            FieldInfo fieldInfo = levelManager.GetType().GetField(variableName, BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo propertyInfo = levelManager.GetType().GetProperty(variableName, BindingFlags.Public | BindingFlags.Instance);

            if (fieldInfo != null && fieldInfo.FieldType == typeof(bool)) // Check if the field is a boolean
            {
                fieldInfo.SetValue(levelManager, true); // Set the boolean field to true
            }
            else if (propertyInfo != null && propertyInfo.PropertyType == typeof(bool)) // Check if the property is a boolean
            {
                propertyInfo.SetValue(levelManager, true, null); // Set the boolean property to true
            }
            else // Log an error if the variable is not found or is not a boolean
            {
                Debug.LogError("Boolean variable '" + variableName + "' not found in TutorialLevelManager.");
            }
            GetComponent<BoxCollider>().enabled = false; // Disable the trigger
            onTrigger.Invoke(); // Trigger the event
        }
        
        TriggerWait();
    }

    void TriggerWait()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(45);
        GetComponent<BoxCollider>().enabled = true;
    }
}
