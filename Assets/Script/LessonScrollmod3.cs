using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LessonScrollmod3 : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Transform content;
    public GameObject[] lessonObjects; // An array to store references to all lesson GameObjects.

    private void Start()
    {
        Button[] lessonButtons = GetComponentsInChildren<Button>();

        foreach (Button button in lessonButtons)
        {
            // Attach a click event listener to each lesson button.
            button.onClick.AddListener(() => JumpToLesson(button.gameObject.name));
        }
    }

    private void JumpToLesson(string lessonName)
    {
        // Find the GameObject by its name.
        Transform lessonToJumpTransform = content.Find(lessonName);

        if (lessonToJumpTransform != null)
        {
            // Calculate the desired scroll position to make the selected lesson appear at the top.
            float targetNormalizedPosition = 1.0f - (lessonToJumpTransform.GetSiblingIndex() / (float)(content.childCount - 1));

            // Scroll to the desired position.
            scrollRect.verticalNormalizedPosition = targetNormalizedPosition;
        }
        else
        {
            Debug.LogWarning("Lesson GameObject not found: " + lessonName);
        }
    }
} 