using UnityEngine;
using UnityEngine.AI;

public class CarAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform[] waypoints;  // Точки маршрута для машины
    private int currentWaypointIndex = 0;

    public TrafficLightCone trafficLight;  // Ссылка на светофор
    public float stopDistance = 10f;  // Дистанция, на которой машина должна остановиться перед светофором

    private bool isStoppedAtLight = false;  // Флаг, показывающий, остановлена ли машина на светофоре

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Устанавливаем первую точку для движения
        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void Update()
    {
        if (!agent.isOnNavMesh)
        {
            Debug.LogError("Агент не находится на NavMesh!");
            return;
        }

        // Проверяем, находится ли машина рядом со светофором и нужно ли остановиться
        if (IsNearTrafficLight() && !trafficLight.IsGreenLight() && !isStoppedAtLight)
        {
            agent.isStopped = true;  // Останавливаем машину на красный или желтый свет
            isStoppedAtLight = true;  // Устанавливаем флаг, что машина остановлена
            Debug.Log("Машина остановилась на светофоре.");
        }

        // Если свет стал зеленым, машина может продолжить движение
        if (isStoppedAtLight && trafficLight.IsGreenLight())
        {
            agent.isStopped = false;  // Продолжаем движение
            isStoppedAtLight = false;  // Сбрасываем флаг остановки
            Debug.Log("Машина продолжила движение на зеленый свет.");
        }

        // Проверяем, достигла ли машина текущей точки маршрута
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !isStoppedAtLight)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    // Метод для проверки, находится ли машина рядом со светофором
    bool IsNearTrafficLight()
    {
        if (trafficLight == null) return false;

        // Рассчитываем дистанцию до светофора
        float distanceToLight = Vector3.Distance(transform.position, trafficLight.transform.position);

        // Проверяем, находится ли машина в пределах остановки перед светофором
        return distanceToLight < stopDistance;
    }
}
