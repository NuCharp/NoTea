using UnityEngine;
using UnityEngine.AI;

public class PedestrianCrossingTrigger : MonoBehaviour
{
    public TrafficLightCone pedestrianLight;  // Ссылка на светофор для пешеходов

    void OnTriggerEnter(Collider other)
    {
        // Проверяем, что объект, вошедший в зону, это куб
        if (other.CompareTag("Cube"))
        {
            // Проверяем состояние светофора
            if (pedestrianLight != null && !pedestrianLight.IsGreenLight())
            {
                // Останавливаем куб, если светофор не зеленый
                NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.isStopped = true;
                    Debug.Log("Куб остановился перед переходом, ждет зеленого света.");
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        // Проверяем, что объект, оставшийся в зоне, это куб
        if (other.CompareTag("Cube"))
        {
            // Если светофор становится зеленым, продолжаем движение
            if (pedestrianLight != null && pedestrianLight.IsGreenLight())
            {
                NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.isStopped = false;
                    Debug.Log("Куб продолжил движение на зеленый свет.");
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Когда куб покидает зону
        if (other.CompareTag("Cube"))
        {
            Debug.Log("Куб покинул зону пешеходного перехода.");
        }
    }
}
