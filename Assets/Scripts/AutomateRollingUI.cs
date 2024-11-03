using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutomateRollingUI : MonoBehaviour
{
    public static AutomateRollingUI Instance { get; private set; }
    public event EventHandler<OnAutomateRollsEventArgs> OnAutomateRolls;

    public class OnAutomateRollsEventArgs : EventArgs {
        public float rolls;
    }


	[SerializeField] private Slider rollSlider;
    [SerializeField] private TextMeshProUGUI rollAmountText;
    [SerializeField] private Button rollButton;

    private float rollAmount;

	private void Awake() {
        Instance= this;

        rollButton.onClick.AddListener(() => {
            rollAmount= rollSlider.value;
            OnAutomateRolls?.Invoke(this, new OnAutomateRollsEventArgs {
                rolls = rollAmount
            });

            rollButton.gameObject.SetActive(false);
        });
	}

	private void Update() {
		rollAmountText.text = rollSlider.value.ToString() + " dobás";
	}


}
