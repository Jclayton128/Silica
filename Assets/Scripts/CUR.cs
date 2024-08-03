using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CUR : object
{

    #region Random Point Generators
    static public Vector3 FindRandomPointNearInputPoint(Vector3 center, float offsetDistance, float offsetRandomFactor)
    {
        float ang = UnityEngine.Random.value * 360;
        float random = offsetDistance + UnityEngine.Random.Range(-offsetRandomFactor, offsetRandomFactor);
        Vector3 pos;
        pos.x = center.x + random * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + random * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        //Debug.DrawLine(center, pos, Color.red, 1.0f);
        return pos;
    }
    static public Vector3 FindRandomPointWithinDistance(Vector3 center, float maxDistance)
    {
        float ang = Random.value * 360;
        float dist = Random.Range(0, maxDistance);
        Vector3 pos;
        pos.x = center.x + dist * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + dist * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        //Debug.DrawLine(center, pos, Color.red, 1.0f);
        return pos;
    }
    static public Vector3 FindRandomPointWithinDistance(Vector3 center, float maxDistance, float minDistance)
    {
        float ang = Random.value * 360;
        float dist = Random.Range(minDistance, maxDistance);
        Vector3 pos;
        pos.x = center.x + dist * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + dist * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        //Debug.DrawLine(center, pos, Color.red, 1.0f);
        return pos;
    }

    static public Vector3 FindRandomPointWithinRectangularArena(
        float xCentroid, float xSpan,
        float yCentroid, float ySpan)
    {
        float x = Random.Range(xCentroid - xSpan, xCentroid + xSpan);
        float y = Random.Range(yCentroid - ySpan, yCentroid + ySpan);

        return new Vector3(x, y, 0);
    }

    static public Vector3 FindRandomPositionWithinRangeBandAndWithinArena(Vector3 localCenter,
        float minLocalDistance, float maxLocalDistance, Vector3 globalCenter, float maxGlobalDistance)
    {
        Vector3 point;
        int breaker = 0;
        do
        {
            breaker++;
            float ang = Random.value * 360f;
            float dist = Random.Range(minLocalDistance, maxLocalDistance);
            point.x = localCenter.x + dist * Mathf.Sin(ang * Mathf.Deg2Rad);
            point.y = localCenter.y + dist * Mathf.Cos(ang * Mathf.Deg2Rad);
            point.z = localCenter.z;
            if (breaker > 10) break;
        }
        while ((globalCenter - point).magnitude > maxGlobalDistance);

        return point;
    }

    public static Vector3 GetRandomPosWithinRectangularArenaAwayFromOtherPoints(
        float xCentroid, float xSpan,
        float yCentroid, float ySpan,
        List<Vector3> otherPoints,
        float minSeparation)
    {
        Vector3 pos = Vector3.zero;

        int breaker = 0;
        do
        {
            pos = FindRandomPointWithinRectangularArena(xCentroid, xSpan, yCentroid, ySpan);
            breaker++;
            if (breaker > 10) 
            {
                Debug.Log("Break!");
                break;
            }

        }
        while (CheckIfPointsViolateMinSeparation(pos, otherPoints, minSeparation));


        return pos;
    }

    public static Vector3 GetRandomPosWithinArenaAwayFromOtherPoints(Vector3 arenaCenter, float arenaSize,
        List<Vector3> otherPoints, float minSeparation)
    {
        Vector3 pos = Vector3.zero;

        int breaker = 0;
        do
        {
            pos = FindRandomPointWithinDistance(arenaCenter, arenaSize);
            breaker++;
            if (breaker > 10) break;
        }
        while (CheckIfPointsViolateMinSeparation(pos, otherPoints, minSeparation));

        return pos;
    }

    private static bool CheckIfPointsViolateMinSeparation(Vector3 testPoint, List<Vector3> otherPoints, float minSeparation)
    {
        bool hasInsufficientSeparation = false;

        for (int i = 0; i < otherPoints.Count; i++)
        {
            if ((testPoint - otherPoints[i]).magnitude < minSeparation)
            {
                hasInsufficientSeparation = true;
                break;
            }
        }

        return hasInsufficientSeparation;
    }

    #endregion

    #region Proximity Searches by Tag
    static public GameObject FindNearestGameObjectWithTag(Transform posToSearchFrom, string tagName)
    {
        GameObject closestTarget = null;
        GameObject[] possibleTargets;
        possibleTargets = GameObject.FindGameObjectsWithTag(tagName);
        float distance = Mathf.Infinity;
        Vector3 position = posToSearchFrom.position;
        foreach (GameObject currentTargetBeingEvaluated in possibleTargets)
        {
            Vector3 diff = currentTargetBeingEvaluated.transform.position - position;
            float curDistance = diff.magnitude;
            if (curDistance < distance)
            {
                closestTarget = currentTargetBeingEvaluated;
                distance = curDistance;
            }
        }
        return closestTarget;
    }

    static public GameObject FindNearestGameObjectWithTag(Transform posToSearchFrom, string tagName, string componentToExcludeFromSearch)
    {

        GameObject closestTarget = null;
        GameObject[] possibleTargets;
        possibleTargets = GameObject.FindGameObjectsWithTag(tagName);
        float distance = Mathf.Infinity;
        Vector3 position = posToSearchFrom.position;
        foreach (GameObject currentTargetBeingEvaluated in possibleTargets)
        {
            if (IterateThroughChildrenLookingForComponent(currentTargetBeingEvaluated, componentToExcludeFromSearch) == true)
            {
                continue; //if the examined GO has that component, then skip it.
            }

            Vector3 diff = currentTargetBeingEvaluated.transform.position - position;
            float curDistance = diff.magnitude;
            if (curDistance < distance)
            {
                closestTarget = currentTargetBeingEvaluated;
                distance = curDistance;
            }
        }
        return closestTarget;
    }

    static private bool IterateThroughChildrenLookingForComponent(GameObject go, string component)
    {
        int childrenCount = go.transform.childCount;
        if (childrenCount == 0) { return false; }
        for (int i = 0; i < childrenCount; i++)
        {
            if (go.transform.GetChild(i).GetComponent(component) == true)
            {
                return true;
            }
            else
            {
                continue;
            }
        }
        return false;
    }


    static public GameObject FindNearestGameObjectWithTag(Transform posToSearchFrom, string tagName, float maxSearchDistance)
    {
        GameObject closestTarget = null;
        GameObject[] possibleTargets;
        possibleTargets = GameObject.FindGameObjectsWithTag(tagName);
        float distance = maxSearchDistance;
        Vector3 position = posToSearchFrom.position;
        foreach (GameObject currentTargetBeingEvaluated in possibleTargets)
        {
            Vector3 diff = currentTargetBeingEvaluated.transform.position - position;
            float curDistance = diff.magnitude;
            if (curDistance < distance)
            {
                closestTarget = currentTargetBeingEvaluated;
                distance = curDistance;
            }
        }
        return closestTarget;
    }

    static public GameObject FindNearestGameObjectWithTag(Transform posToSearchFrom, string tagName,
        float maxSearchDistance, string componentToExcludeFromSearch)
    {
        GameObject closestTarget = null;
        GameObject[] possibleTargets;
        possibleTargets = GameObject.FindGameObjectsWithTag(tagName);
        float distance = maxSearchDistance;
        Vector3 position = posToSearchFrom.position;
        foreach (GameObject currentTargetBeingEvaluated in possibleTargets)
        {
            if (IterateThroughChildrenLookingForComponent(currentTargetBeingEvaluated, componentToExcludeFromSearch) == true)
            {
                continue; //if the examined GO has that component, then skip it.
            }
            Vector3 diff = currentTargetBeingEvaluated.transform.position - position;
            float curDistance = diff.magnitude;
            if (curDistance < distance)
            {
                closestTarget = currentTargetBeingEvaluated;
                distance = curDistance;
            }
        }
        return closestTarget;
    }

    #endregion

    #region Proximity Search by Layers
    static public GameObject FindNearestGameObjectOnLayer(Transform posToSearchFrom, string layerName)
    {
        int layerMask = LayerMask.NameToLayer(layerName);
        float distance = Mathf.Infinity;
        GameObject closestTarget = null;
        RaycastHit2D[] hitColliders = Physics2D.CircleCastAll(posToSearchFrom.position, distance, Vector2.up, 0f, layerMask, -1f, 1f);
        foreach (RaycastHit2D contact in hitColliders)
        {
            GameObject go = contact.transform.gameObject;
            float evaluatedObjectDistance = (go.transform.position - posToSearchFrom.position).magnitude;
            if (evaluatedObjectDistance <= distance)
            {
                distance = evaluatedObjectDistance;
                closestTarget = go;
            }
        }
        return closestTarget;
    }

    static public GameObject FindNearestGameObjectOnLayer(Transform posToSearchFrom, string layerName, float maxSearchDistance)
    {
        int layerMask = LayerMask.NameToLayer(layerName);
        float distance = maxSearchDistance;
        GameObject closestTarget = null;
        RaycastHit2D[] hitColliders = Physics2D.CircleCastAll(posToSearchFrom.position, maxSearchDistance, Vector2.up, 0f, layerMask, -1f, 1f);
        foreach (RaycastHit2D contact in hitColliders)
        {
            GameObject go = contact.transform.gameObject;
            float evaluatedObjectDistance = (go.transform.position - posToSearchFrom.position).magnitude;
            if (evaluatedObjectDistance <= distance)
            {
                distance = evaluatedObjectDistance;
                closestTarget = go;
            }
        }
        return closestTarget;
    }
    static public GameObject FindNearestGameObjectOnLayer(Transform posToSearchFrom, int layerMask)
    {
        float distance = Mathf.Infinity;
        GameObject closestTarget = null;
        RaycastHit2D[] hitColliders = Physics2D.CircleCastAll(posToSearchFrom.position, distance, Vector2.up, 0f, layerMask, -1f, 1f);
        foreach (RaycastHit2D contact in hitColliders)
        {
            GameObject go = contact.transform.gameObject;
            float evaluatedObjectDistance = (go.transform.position - posToSearchFrom.position).magnitude;
            if (evaluatedObjectDistance <= distance)
            {
                distance = evaluatedObjectDistance;
                closestTarget = go;
            }
        }
        return closestTarget; ;
    }
    static public GameObject FindNearestGameObjectOnLayer(Transform posToSearchFrom, int layerMask, float maxSearchDistance)
    {
        //Debug.Log(LayerMask.LayerToName(layerIndex));
        float distance = maxSearchDistance;
        GameObject closestTarget = null;
        //RaycastHit2D[] hitColliders = Physics2D.CircleCastAll(posToSearchFrom.position, maxSearchDistance, Vector2.up, 0f, layerMask, -1f, 1f);
        Collider2D[] hits = Physics2D.OverlapCircleAll(posToSearchFrom.position, maxSearchDistance, layerMask);
        foreach (Collider2D hit in hits)
        {
            GameObject go = hit.transform.gameObject;
            float evaluatedObjectDistance = (go.transform.position - posToSearchFrom.position).magnitude;
            if (evaluatedObjectDistance <= distance)
            {
                distance = evaluatedObjectDistance;
                closestTarget = go;
            }
        }
        return closestTarget;
    }
    static public GameObject FindNearestGameObjectOnLayer(Vector3 posToSearchFrom, int layerMask, float maxSearchDistance)
    {
        //Debug.Log(LayerMask.LayerToName(layerIndex));
        float distance = maxSearchDistance;
        GameObject closestTarget = null;
        //RaycastHit2D[] hitColliders = Physics2D.CircleCastAll(posToSearchFrom.position, maxSearchDistance, Vector2.up, 0f, layerMask, -1f, 1f);
        Collider2D[] hits = Physics2D.OverlapCircleAll(posToSearchFrom, maxSearchDistance, layerMask);
        foreach (Collider2D hit in hits)
        {
            GameObject go = hit.transform.gameObject;
            float evaluatedObjectDistance = (go.transform.position - posToSearchFrom).magnitude;
            if (evaluatedObjectDistance <= distance)
            {
                distance = evaluatedObjectDistance;
                closestTarget = go;
            }
        }
        return closestTarget;
    }


    #endregion

    #region Unit Circle Points

    static public Vector2 FindPointOnUnitCircleCircumference()
    {
        float randomAngle = Random.Range(0f, Mathf.PI * 2f);
        return new Vector2(Mathf.Sin(randomAngle), Mathf.Cos(randomAngle)).normalized;
    }

    static public Vector2 FindPointOnUnitCircleCircumference(float sectorStartDeg, float sectorEndDeg)
    {
        float randAngleDeg = Random.Range(sectorStartDeg, sectorEndDeg);
        float randomAngle = randAngleDeg * Mathf.Deg2Rad;
        return new Vector2(Mathf.Sin(randomAngle), Mathf.Cos(randomAngle)).normalized;
    }

    #endregion

    #region Random Draw From Collection

    public static object GetRandomFromCollection(object[] source)
    {
        if (source.Length == 0)
        {
            Debug.LogError("Source is empty!");
            return null;
        }

        int rand = UnityEngine.Random.Range(0, source.Length);
        return (source[rand]);
    }

    public static object GetRandomFromCollection(List<object> source)
    {
        if (source.Count == 0)
        {
            Debug.LogError("Source is empty!");
            return null;
        }

        int rand = UnityEngine.Random.Range(0, source.Count);
        return (source[rand]);
    }

    #endregion

}
