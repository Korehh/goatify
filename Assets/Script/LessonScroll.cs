using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LessonScroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Transform content;
    public GameObject[] lessonObjects; // An array to store references to all lesson GameObjects.
    public float titleOffset = 0.0f; // Adjust this value to control the vertical position of the title when scrolling.

    private void Start()
    {
        Button[] lessonButtons = GetComponentsInChildren<Button>();

        foreach (Button button in lessonButtons)
        {
            // Attach a click event listener to each lesson button.
            button.onClick.AddListener(() => ScrollToLessonTitle(button.gameObject.name));
        }
    }

    private void ScrollToLessonTitle(string lessonName)
    {
        // Find the GameObject by its name.
        Transform lessonToJumpTransform = content.Find(lessonName);

        if (lessonToJumpTransform != null)
        {
            // Calculate the desired scroll position to make the selected lesson's title appear at the top.
            float targetNormalizedPosition = 1.0f - ((lessonToJumpTransform.GetSiblingIndex() - 0.3f) / (float)(content.childCount - 1));

            // Scroll to the desired position.
            scrollRect.verticalNormalizedPosition = targetNormalizedPosition;
        }
        else
        {
            Debug.LogWarning("Lesson GameObject not found: " + lessonName);
        }
    }
}