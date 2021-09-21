using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum JumpMethod
{
    Teleport, Walk, Parabola, Curve
}
[RequireComponent(typeof(NavMeshAgent))]
public class AgentLinkMover : MonoBehaviour
{
    public JumpMethod jump = JumpMethod.Curve;
    public AnimationCurve curve = new AnimationCurve();
    float timer = 0;
    bool isRotating = false;
    IEnumerator Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false;
        while(true)
        {
            if (agent.isOnOffMeshLink)
            {
                switch (jump)
                {
                    case JumpMethod.Walk:
                        yield return StartCoroutine(Walk(agent));
                        break;
                    case JumpMethod.Parabola:
                        yield return StartCoroutine(Parabola(agent, 2.0f, .5f));
                        break;
                    case JumpMethod.Curve:
                        yield return StartCoroutine(Curve(agent, .7f));
                        break;
                }
                agent.CompleteOffMeshLink();
            }
            yield return null;
        }
    }

    void Update()
    {
        if (!isRotating)
            return;
        timer += Time.deltaTime;
    }

    IEnumerator Walk(NavMeshAgent agent)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        yield return new WaitForSeconds(1.0f);
        while (agent.transform.position != endPos)
        {
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        float normalizedTime = 0.0f;
        yield return new WaitForSeconds(1.0f);
        while (normalizedTime < 1.0f)
        {
            float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }

    IEnumerator Curve(NavMeshAgent agent, float duration)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        Vector3 lookDir = transform.TransformDirection(new Vector3(endPos.x, 0, endPos.z));
        isRotating = true;
        var rotation = Quaternion.LookRotation(lookDir - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, timer * 10);
        float normalizedTime = 0.0f;
        yield return new WaitForSeconds(1.0f);
        isRotating = false;
        timer = 0;
        while (normalizedTime < 1.0f)
        {
            float yOffset = curve.Evaluate(normalizedTime);
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null; 
        }
    }
}
