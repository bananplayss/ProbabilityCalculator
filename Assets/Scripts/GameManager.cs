using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
	public event EventHandler OnAutoDone;
	public event EventHandler<OnNewCombinationEventArgs> OnNewCombination;

	public class OnNewCombinationEventArgs : EventArgs {
		public int num1;
		public int num2;
	}
	public static GameManager Instance { get; private set; }

	private bool redDiceBool = false;
	private bool blueDiceBool = false;

	private float fullThrows = 0;

	[SerializeField] private GameObject blueDice;
	[SerializeField] private GameObject redDice;
	[SerializeField] private Transform spawnPos;
	[SerializeField] private TextMeshProUGUI fullThrowsText;

	Dice redDiceGO;
	Dice blueDiceGO;

	Rigidbody currentDice;

	private void Awake() {
		Instance = this;
	}

	private void Update() {
		fullThrowsText.text = "Dobások: " + fullThrows.ToString();
		ChangeDiceNumber();
		if (EventSystem.current.IsPointerOverGameObject()) return;
		if (Math.Instance.automating) return;
				
			
		//if (redDiceBool && blueDiceBool) {
		//	if (Input.GetMouseButtonDown(0)) {
		//		redDiceBool = false;
		//		blueDiceBool = false;
		//		Destroy(redDiceGO.gameObject);
		//		Destroy(blueDiceGO.gameObject);
		//	}
		//}

		//if (Input.GetMouseButtonDown(0)) {
		//	if (!redDiceBool) {
		//		redDiceBool = true;
		//		Rigidbody redRig = Instantiate(redDice, spawnPos.position, Quaternion.identity).GetComponent<Rigidbody>();
		//		redDiceGO = redRig.GetComponent<Dice>();
		//		currentDice= redRig;
		//		redRig.AddForce(spawnPos.forward * 10, ForceMode.Impulse);
		//		redRig.AddTorque(new Vector3(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f)) * UnityEngine.Random.Range(1, 5));
		//	} 
		//	else if(!blueDiceBool) {
		//		blueDiceBool = true;
		//		Rigidbody blueRig = Instantiate(blueDice, spawnPos.position, Quaternion.identity).GetComponent<Rigidbody>();
		//		blueDiceGO= blueRig.GetComponent<Dice>();
		//		currentDice= blueRig;
		//		blueRig.AddForce(spawnPos.forward * 10, ForceMode.Impulse);
		//		blueRig.AddTorque(new Vector3(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f)) * UnityEngine.Random.Range(1, 5));
		//	}
		//}

		
	}

	public void IncreaseThrows() {
		fullThrows++;
		
		if(fullThrows % 2 == 0) { 
			fullThrows = fullThrows/ 2;
			if(blueDiceBool && redDiceBool) {
				OnNewCombination.Invoke(this, new OnNewCombinationEventArgs {
					num1 = blueDiceGO.number,
					num2 = redDiceGO.number
				});
			}
			
		}
		
	}

	public void ThrowDices(int forInt) {
		StartCoroutine(ThrowDicesCoroutine(forInt));
	}

	private IEnumerator ThrowDicesCoroutine(int forInt) {
		for (int i = 0; i < forInt; i++) {
			redDiceBool = true;
			Rigidbody redRig = Instantiate(redDice, spawnPos.position, Quaternion.identity).GetComponent<Rigidbody>();
			redDiceGO = redRig.GetComponent<Dice>();
			currentDice = redRig;
			redRig.AddForce(spawnPos.forward * 10, ForceMode.Impulse);
			redRig.AddTorque(new Vector3(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f)) * UnityEngine.Random.Range(1, 5));
			yield return new WaitForSeconds(6);
			blueDiceBool = true;
			Rigidbody blueRig = Instantiate(blueDice, spawnPos.position, Quaternion.identity).GetComponent<Rigidbody>();
			blueDiceGO = blueRig.GetComponent<Dice>();
			currentDice = blueRig;
			blueRig.AddForce(spawnPos.forward * 10, ForceMode.Impulse);
			blueRig.AddTorque(new Vector3(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f)) * UnityEngine.Random.Range(1, 5));

			yield return new WaitForSeconds(6f);
			redDiceBool = false;
			blueDiceBool = false;
			Destroy(redDiceGO.gameObject);
			Destroy(blueDiceGO.gameObject);
			Destroy(blueDiceGO.gameObject);
		}
		Math.Instance.automating = false;
		Time.timeScale = 1;
		OnAutoDone?.Invoke(this, EventArgs.Empty);

	}

	private void ChangeDiceNumber() {
		if (currentDice && currentDice.GetComponent<Dice>().onCollided) {
			if (currentDice.velocity.magnitude < 0.055f) {
				Transform highestFace = currentDice.transform.GetChild(0);

				foreach (Transform face in currentDice.transform) {
					if (face.position.y > highestFace.position.y) {
						highestFace = face;
					}
				}

				currentDice.GetComponent<Dice>().number = int.Parse(highestFace.name);

			}
		}
	}
}
