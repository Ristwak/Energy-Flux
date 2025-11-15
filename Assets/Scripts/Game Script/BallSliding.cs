using UnityEngine;

public class BallSliding : MonoBehaviour
{
    [Header("Movement / Path")]
    public float minSlideSpeed = 1f;       // Minimum random speed
    public float maxSlideSpeed = 4f;       // Maximum random speed
    public Transform[] pathPoints;         // Points defining the path (in order, left → right)

    private float slideSpeed;              // Actual current speed
    private float[] segmentLengths;        // Length of each segment
    private float[] pointDistances;        // Distance from start to each path point
    private float totalPathLength;         // Sum of all segment lengths
    private float distanceTravelled = 0f;  // How far we've moved along the path
    private float previousDistanceTravelled = 0f;

    [Header("Question UI")]
    public GameObject questionPanel;       // The UI panel that shows the question

    // Stop logic
    private int[] stopPointIndices;        // 4 random indices of path points
    private int currentStopIndexIdx = 0;   // Which stop index we are heading to (0..3)
    private bool isSliding = true;         // Are we currently moving?
    private bool isStopped = false;        // Are we currently at a question?

    private Vector3 startPosition;         // Start position (first path point)

    void Start()
    {
        InitPath();             // Build path data (segment lengths, distances)
        SetupRandomStops();     // Pick 4 random path points
        SetRandomSpeed();       // Pick a random speed

        // Place ball at start of path
        transform.position = startPosition;

        if (questionPanel != null)
            questionPanel.SetActive(false);
    }

    void Update()
    {
        if (!isSliding) return;

        previousDistanceTravelled = distanceTravelled;

        SlideAlongPath();       // Move along the path
        CheckForStopTrigger();  // Check if we hit a question point
    }

    /// <summary>
    /// Precompute segment lengths, total path length, and cumulative distances.
    /// </summary>
    void InitPath()
    {
        if (pathPoints == null || pathPoints.Length < 2)
        {
            Debug.LogError("BallSliding: You must assign at least 2 path points.");
            startPosition = transform.position;
            return;
        }

        int count = pathPoints.Length;
        segmentLengths = new float[count - 1];
        pointDistances = new float[count];
        totalPathLength = 0f;

        pointDistances[0] = 0f;
        for (int i = 0; i < count - 1; i++)
        {
            float segLen = Vector3.Distance(pathPoints[i].position, pathPoints[i + 1].position);
            segmentLengths[i] = segLen;
            totalPathLength += segLen;
            pointDistances[i + 1] = totalPathLength;
        }

        startPosition = pathPoints[0].position;
        distanceTravelled = 0f;
        previousDistanceTravelled = 0f;
    }

    /// <summary>
    /// Randomly choose 4 unique path points (not including the first and last) as question stops.
    /// </summary>
    void SetupRandomStops()
    {
        int count = pathPoints.Length;

        if (count < 6)
        {
            Debug.LogWarning("BallSliding: Not enough points to safely pick 4 internal stops. Using whatever is available.");
        }

        // We avoid index 0 (start) and last index (end), so range is [1, count-2]
        // but code will still work even if count is small.
        System.Collections.Generic.HashSet<int> chosen = new System.Collections.Generic.HashSet<int>();

        int minIndex = 1;
        int maxIndex = count - 2; // inclusive

        int stopsNeeded = Mathf.Min(4, Mathf.Max(0, maxIndex - minIndex + 1));

        while (chosen.Count < stopsNeeded)
        {
            int idx = Random.Range(minIndex, maxIndex + 1);
            chosen.Add(idx);
        }

        // Copy to array and sort in ascending path order
        stopPointIndices = new int[stopsNeeded];
        chosen.CopyTo(stopPointIndices);
        System.Array.Sort(stopPointIndices);

        currentStopIndexIdx = 0;
    }

    /// <summary>
    /// Pick a random speed between minSlideSpeed and maxSlideSpeed.
    /// </summary>
    void SetRandomSpeed()
    {
        slideSpeed = Random.Range(minSlideSpeed, maxSlideSpeed);
    }

    /// <summary>
    /// Move the ball along the predefined path.
    /// </summary>
    void SlideAlongPath()
    {
        if (totalPathLength <= 0f) return;

        // Advance along the path
        distanceTravelled += slideSpeed * Time.deltaTime;
        distanceTravelled = Mathf.Clamp(distanceTravelled, 0f, totalPathLength);

        // Convert distanceTravelled to a position on the path
        float dist = distanceTravelled;
        int segmentIndex = 0;

        while (segmentIndex < segmentLengths.Length && dist > segmentLengths[segmentIndex])
        {
            dist -= segmentLengths[segmentIndex];
            segmentIndex++;
        }

        if (segmentIndex >= segmentLengths.Length)
        {
            // End of path
            transform.position = pathPoints[pathPoints.Length - 1].position;
            return;
        }

        Transform p0 = pathPoints[segmentIndex];
        Transform p1 = pathPoints[segmentIndex + 1];

        float t = segmentLengths[segmentIndex] > 0f ? dist / segmentLengths[segmentIndex] : 0f;
        transform.position = Vector3.Lerp(p0.position, p1.position, t);
    }

    /// <summary>
    /// Check if we have crossed the next stop point along the path.
    /// </summary>
    void CheckForStopTrigger()
    {
        if (isStopped) return;
        if (stopPointIndices == null || stopPointIndices.Length == 0) return;
        if (currentStopIndexIdx >= stopPointIndices.Length) return;

        int pathPointIndex = stopPointIndices[currentStopIndexIdx];
        float stopDist = pointDistances[pathPointIndex];

        // If we just crossed that distance this frame, stop.
        if (previousDistanceTravelled < stopDist && distanceTravelled >= stopDist)
        {
            // Snap ball exactly to that point
            transform.position = pathPoints[pathPointIndex].position;

            isStopped = true;
            isSliding = false;
            currentStopIndexIdx++;

            ShowQuestionPanel();
        }
    }

    void ShowQuestionPanel()
    {
        if (questionPanel != null)
            questionPanel.SetActive(true);
    }

    /// <summary>
    /// Called by your UI buttons with true/false.
    /// </summary>
    public void AnswerQuestion(bool isCorrect)
    {
        if (isCorrect)
        {
            // Correct answer, hide panel and continue sliding with a new random speed
            if (questionPanel != null)
                questionPanel.SetActive(false);

            isStopped = false;
            isSliding = true;
            SetRandomSpeed();        // New random speed for next segment
        }
        else
        {
            // Incorrect answer, reset the run
            ResetBall();
        }
    }

    void ResetBall()
    {
        distanceTravelled = 0f;
        previousDistanceTravelled = 0f;
        transform.position = startPosition;

        isStopped = false;
        isSliding = true;

        if (questionPanel != null)
            questionPanel.SetActive(false);

        SetupRandomStops();   // New random 4 stop points
        SetRandomSpeed();     // New random speed
    }
}
