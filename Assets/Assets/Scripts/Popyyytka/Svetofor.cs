using System.Collections;
using UnityEngine;

public class TrafficLightCone : MonoBehaviour
{
    public Renderer coneRenderer;  // Рендерер для изменения цвета конуса
    public Color redColor = Color.red;    // Цвет для красного света
    public Color yellowColor = Color.yellow;  // Цвет для желтого света
    public Color greenColor = Color.green;    // Цвет для зеленого света

    private float greenDuration = 5f;   // Время, когда горит зеленый
    private float yellowDuration = 2f;  // Время, когда горит желтый
    private float redDuration = 5f;     // Время, когда горит красный

    private enum TrafficState { Red, Yellow, Green }
    private TrafficState currentState;

    void Start()
    {
        if (coneRenderer == null)
        {
            coneRenderer = GetComponent<Renderer>();  // Автоматически получаем рендерер конуса
        }

        // Запускаем светофорный цикл
        StartCoroutine(TrafficLightCycle());
    }

    // Корутин для цикла светофора
    IEnumerator TrafficLightCycle()
    {
        while (true)
        {
            // Зеленый свет
            currentState = TrafficState.Green;
            SetLightColor(greenColor);
            yield return new WaitForSeconds(greenDuration);

            // Желтый свет
            currentState = TrafficState.Yellow;
            SetLightColor(yellowColor);
            yield return new WaitForSeconds(yellowDuration);

            // Красный свет
            currentState = TrafficState.Red;
            SetLightColor(redColor);
            yield return new WaitForSeconds(redDuration);
        }
    }

    // Метод для установки цвета конуса
    void SetLightColor(Color color)
    {
        coneRenderer.material.color = color;
    }

    // Метод для проверки текущего состояния светофора (зеленый свет)
    public bool IsGreenLight()
    {
        return currentState == TrafficState.Green;
    }
}
