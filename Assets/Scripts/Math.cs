using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Math : MonoBehaviour
{
	public static Math Instance { get; private set; }

	public float rolls = 0;
	public bool automating = false;

	public List<int> intList = new List<int>();

	private void Awake() {
		Instance = this;
	}

	private void Start() {
		GameManager.Instance.OnNewCombination += GameManager_OnNewCombination;
		AutomateRollingUI.Instance.OnAutomateRolls += Instance_OnAutomateRolls;
	}

	private void Instance_OnAutomateRolls(object sender, AutomateRollingUI.OnAutomateRollsEventArgs e) {
		if (automating) return;
		automating= true;
		rolls = e.rolls;
		int rollInt = Mathf.RoundToInt(rolls);

		GameManager.Instance.ThrowDices(rollInt);
		float time = (rollInt /2) * 1.25f;
		if(time > 100) {
			time = 100;
		}
		Time.timeScale = time;
		//Time.timeScale = 100;
	}

	private void GameManager_OnNewCombination(object sender, GameManager.OnNewCombinationEventArgs e) {
		int num = e.num1 + e.num2;
		intList[num-1] += 1;
		Debug.Log(num);
	}


}
