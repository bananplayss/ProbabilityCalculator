using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BarChart : MonoBehaviour
{
    private RectTransform graphContainer;
	private List<GameObject> gameObjectList = new List<GameObject>();

	[SerializeField] private GameObject UI;
	[SerializeField] private RectTransform labelTemplateY;
	[SerializeField] private TextMeshProUGUI[] texts;
	[SerializeField] private Button restartButton;

	private void Awake() {
		graphContainer = UI.transform.Find("GraphContainer").GetComponent<RectTransform>();
		labelTemplateY = UI.transform.Find("labelTemplateY").GetComponent<RectTransform>();

		restartButton.onClick.AddListener(() => {
			SceneManager.LoadScene(0);
		});

	}

	private void Start() {
		GameManager.Instance.OnAutoDone += Instance_OnAutoDone;

		UI.SetActive(false);
	}

	private void Instance_OnAutoDone(object sender, System.EventArgs e) {
		ShowGraph(Math.Instance.intList);
		UI.SetActive(true);

		for (int i = 0; i < texts.Length; i++) {
			texts[i].GetComponent<RectTransform>().anchoredPosition = 
				new Vector2 (gameObjectList[i].GetComponent<RectTransform>().anchoredPosition.x,gameObjectList[i].GetComponent<RectTransform>().anchoredPosition.y+50) ;
			texts[i].text = Decimal.Round((decimal)Math.Instance.intList[i] / (decimal)Math.Instance.rolls,2).ToString(); 
		}
	}

	private GameObject CreateBar(Vector2 graphPos, float barWidth) {
		GameObject gameobject = new GameObject("bar", typeof(Image));
		Outline outline = gameobject.AddComponent<Outline>();
		outline.effectDistance = new Vector2(5, 5);
		outline.effectColor = new Color(0, 0, 0, 255);
		gameobject.GetComponent<Image>().color = Color.green;
		gameobject.transform.SetParent(graphContainer, false);
		RectTransform rectTransform = gameobject.GetComponent<RectTransform>();
		rectTransform.anchoredPosition = new Vector2(graphPos.x, 0f); 
		rectTransform.sizeDelta = new Vector2(barWidth, graphPos.y);
		rectTransform.anchorMin = Vector2.zero;
		rectTransform.anchorMax = Vector2.zero;
		rectTransform.pivot = new Vector2(.5f, 0f);

		return gameobject;
	}

	private void ShowGraph(List<int> valueList) {
		float graphHeight = graphContainer.sizeDelta.y*8;

		int maxVisibleValueAmount = 12;
		float yMax = valueList[0];
		float yMin = valueList[0];
		

		foreach(int value in valueList) {
			if(value > yMax) {
				yMax = value;
			}
			if(value < yMin) {
				yMin= value;
			}
		}
		yMax = yMax + (( yMax- yMin ) * .2f);
		yMin = yMin - (( yMax- yMin ) * .2f);

		float xSize = 100f;

		for (int i = valueList.Count-maxVisibleValueAmount; i < valueList.Count; i++) {

			float xPos = xSize + i * xSize;
			
			float yPos = (valueList[i]-yMin) / (yMax-yMin) * graphHeight;
			if (valueList[i] == 0) {
				yPos = 0;
			}

			GameObject barGameObject = CreateBar(new Vector2(xPos, yPos), xSize *.7f);
			gameObjectList.Add(barGameObject);

			
		}

		int separatorCount = 10;
		for (int i = 0; i < separatorCount; i++) {
			RectTransform labelY = Instantiate(labelTemplateY);
			labelY.SetParent(graphContainer, false);
			labelY.gameObject.SetActive(true);
			float normalizedValue = i * 1f /separatorCount;
			labelY.anchoredPosition = new Vector2(-15.5f, normalizedValue*graphHeight);
			labelY.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(normalizedValue*yMax).ToString();
			}
	}
}
