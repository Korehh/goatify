using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
	/// Test DateScroller.
	/// </summary>
public class datescroll : MonoBehaviour
{
		/// <summary>
		/// DateScroller.
		/// </summary>
		[SerializeField]
		protected UIWidgets.DateBase DateScroller;

		/// <summary>
		/// Start this instance.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0603:Delegate allocation from a method group", Justification = "Required")]
		public InputField dateTextbox;
		protected void Start()
		{
			DateScroller.OnDateChanged.AddListener(ProcessDate);

			// change culture
			DateScroller.Culture = new System.Globalization.CultureInfo("en-US");

			// change calendar
			//DateScroller.Culture = new System.Globalization.CultureInfo("ja-JP");
			//DateScroller.Culture.DateTimeFormat.Calendar = new System.Globalization.JapaneseCalendar();
		}

		/// <summary>
		/// Process the destroy event.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0603:Delegate allocation from a method group", Justification = "Required")]
		protected void OnDestroy()
		{
			DateScroller.OnDateChanged.RemoveListener(ProcessDate);
		}

		void ProcessDate(System.DateTime dt)
		{
			Debug.Log(dt);
			 dateTextbox.text = dt.ToString("yyyy-MM-dd");
		}
		public void OnShowButtonClicked()
      	{
         // Manually trigger the DateScroller to update the date textbox
         ProcessDate(DateScroller.Date);
      	}
	}