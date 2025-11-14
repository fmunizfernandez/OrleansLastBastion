using System.Linq;
using UnityEngine;

#if UNITY_Editor
using UnityEditor;
#endif

public class Path : MonoBehaviour
{
    public GameObject[] Waypoints;

    public Vector3 GetPosition(int index)
    {
        return Waypoints[index].transform.position;
    }

#if UNITY_Editor
    
    private void OnDrawGizmos()
    {
        if (!Waypoints.Any())
            return;

        for (var i = 0; i < Waypoints.Length; i++)
        {
            var style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.alignment = TextAnchor.MiddleCenter;
            Handles.Label(Waypoints[i].transform.position + Vector3.up * 0.7f, Waypoints[i].name, style);

            if (i < Waypoints.Length - 1)
            {
                Gizmos.color = Color.darkRed;
                Gizmos.DrawLine(Waypoints[i].transform.position, Waypoints[i + 1].transform.position);
            }
        }
    }

#endif

}
