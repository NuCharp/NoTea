using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CubeAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform home;
    private Transform[] tasks;  // Массив с возможными задачами
    private Transform[] assignedTasks; // Уникальные задачи для каждого куба
    private int currentTaskIndex = 0;
    private int numberOfTasksToComplete; // Количество задач для выполнения
    private bool allTasksComplete = false; // Проверка завершения всех задач
    private float workPauseTime = 5f;   // Пауза между задачами (время выполнения работы)
    private float restTime = 10f;       // Время отдыха
    public string[] disappearTaskNames; // Массив с именами задач, где кубы должны исчезать

    private Vector3 initialPosition;  // Начальная позиция, откуда агент начал движение
    private bool isOnLongPath = false;  // Флаг, указывающий, что агент идёт по длинному пути
    private float maxDistanceBeforeCommit = 5f;  // Максимальное расстояние, при котором агент может вернуться на короткий путь

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        initialPosition = transform.position;  // Сохраняем начальную позицию куба
        StartCoroutine(PerformTaskRoutine());
    }

    // Инициализация задач и дома
    public void InitializeTasks(Transform[] taskPoints, Transform homePoint)
    {
        home = homePoint;
        tasks = taskPoints;

        // Рандомно выбираем количество задач от 2 до 6
        numberOfTasksToComplete = Random.Range(2, 7);

        // Назначаем уникальные задачи для куба
        assignedTasks = new Transform[numberOfTasksToComplete];
        for (int i = 0; i < numberOfTasksToComplete; i++)
        {
            assignedTasks[i] = tasks[Random.Range(0, tasks.Length)];
        }

        Debug.Log("Кубу назначено " + numberOfTasksToComplete + " задач.");
    }

    // Основной корутин для выполнения задач, отдыха и возвращения домой
    IEnumerator PerformTaskRoutine()
    {
        if (assignedTasks == null || assignedTasks.Length == 0)
        {
            Debug.LogError("Задачи не назначены кубу! Куб не будет работать.");
            yield break;  // Прерываем выполнение корутины, если нет задач
        }

        // Куб идёт к каждой из задач по очереди
        for (int i = 0; i < assignedTasks.Length; i++)
        {
            currentTaskIndex = i;
            MoveToTask(assignedTasks[currentTaskIndex]);

            // Ждём, пока куб дойдет до точки задачи или столкнётся с препятствием
            yield return StartCoroutine(HandlePath());

            if (agent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                Debug.Log("Куб выполнил задачу " + (i + 1) + " из " + assignedTasks.Length + ".");
                yield return new WaitForSeconds(workPauseTime); // Куб работает на месте 5 секунд

                // Решаем, хочет ли куб отдохнуть (случайно после каждой задачи)
                if (ShouldRest())
                {
                    Debug.Log("Куб решил отдохнуть.");
                    yield return StartCoroutine(RestRoutine());
                }
            }
            else
            {
                Debug.Log("Путь заблокирован, куб ищет другую задачу.");
                yield return new WaitForSeconds(3f);  // Ждём немного перед поиском другой задачи
            }
        }

        // Все задачи выполнены
        allTasksComplete = true;
        Debug.Log("Куб выполнил все задачи и возвращается домой.");

        // Куб возвращается домой
        MoveToHome();

        // Ждём возвращения домой
        yield return new WaitUntil(() => agent.remainingDistance < 0.5f && !agent.pathPending);
        Debug.Log("Куб вернулся домой.");

        // Исчезаем после возврата домой
        Disappear();
    }

    // Перемещение к задаче
    void MoveToTask(Transform task)
    {
        if (task != null)
        {
            Debug.Log("Куб направляется к задаче: " + task.name);
            agent.SetDestination(task.position);
        }
        else
        {
            Debug.LogError("Задача не назначена!");
        }
    }

    // Метод для определения, когда куб решает отдохнуть (например, 50% шанс)
    bool ShouldRest()
    {
        return Random.Range(0f, 1f) < 0.2f;  // 50% шанс на отдых после каждой задачи
    }

    // Корутин для выполнения отдыха
    IEnumerator RestRoutine()
    {
        Debug.Log("Куб отдыхает в течение " + restTime + " секунд.");
        yield return new WaitForSeconds(restTime);  // Куб отдыхает заданное время
    }

    // Перемещение домой
    void MoveToHome()
    {
        if (home != null)
        {
            Debug.Log("Куб возвращается домой.");
            agent.SetDestination(home.position);
        }
        else
        {
            Debug.LogError("Дом для куба не назначен!");
        }
    }

    // Исчезновение куба после выполнения задач
    void Disappear()
    {
        if (allTasksComplete)
        {
            Debug.Log("Куб завершил все задачи и исчезает.");
            Destroy(gameObject);  // Удаляем объект
        }
    }

    // Метод для обработки маршрута и проверки на короткий путь
    IEnumerator HandlePath()
    {
        while (agent.pathPending)
        {
            yield return null; // Ждём, пока путь будет рассчитан
        }

        // Если путь заблокирован
        if (agent.pathStatus == NavMeshPathStatus.PathPartial || agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            Debug.Log("Путь заблокирован. Куб ждёт.");
            agent.ResetPath();  // Останавливаем агента
            yield return new WaitForSeconds(3f);  // Куб ждёт на месте

            // Проверяем, появился ли более короткий путь
            if (ShouldRecalculatePath())
            {
                Debug.Log("Появился более короткий путь! Пересчёт пути.");
                agent.SetDestination(assignedTasks[currentTaskIndex].position);
            }
        }
    }

    // Проверка, нужно ли пересчитывать путь
    bool ShouldRecalculatePath()
    {
        // Если агент уже ушёл слишком далеко от исходной позиции, он не возвращается
        if (Vector3.Distance(transform.position, initialPosition) > maxDistanceBeforeCommit)
        {
            Debug.Log("Агент слишком далеко, чтобы вернуться на короткий путь.");
            return false;
        }

        // Рассчитываем текущий путь
        NavMeshPath newPath = new NavMeshPath();
        agent.CalculatePath(assignedTasks[currentTaskIndex].position, newPath);

        // Проверяем, открылся ли более короткий путь (если путь валидный и его длина короче текущего)
        if (newPath.status == NavMeshPathStatus.PathComplete && CalculatePathLength(newPath) < CalculatePathLength(agent.path))
        {
            return true;
        }

        return false;
    }

    // Метод для расчёта длины пути
    float CalculatePathLength(NavMeshPath path)
    {
        float length = 0f;
        if (path.corners.Length < 2)
            return length;

        for (int i = 1; i < path.corners.Length; i++)
        {
            length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }

        return length;
    }
}
