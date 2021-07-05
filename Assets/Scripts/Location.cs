using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Location : MonoBehaviour
{
    [SerializeField] internal List<Transform> markers = new List<Transform>();
    [SerializeField] internal Transform cameraMarker;

    private Transform FindMarkerTransform(string name) {
        foreach (var marker in markers) {
            if (marker.name.Equals(name, System.StringComparison.InvariantCultureIgnoreCase)) {
                return marker;
            }
        }
        Debug.LogWarning($"Location {this.name} has no marker named {name}. Available markers: {string.Join(", ", markers)}");
        return transform;
    }

    internal Vector3 GetPositionForMarker(string markerName) {
        return FindMarkerTransform(markerName).position;
    }

    // TODO COMMENT THESE METHODS
    internal Quaternion GetRotationForMarker(string markerName) {
        return FindMarkerTransform(markerName).rotation;        
    }
}

#if UNITY_EDITOR
#if false

[CustomEditor(typeof(Location))]
public class LocationEditor : Editor {
    static Vector3 characterPreviewSize = new Vector3(1f,2.5f,1f);

    static readonly string[] DefaultMarkerNames = new string[] {
        "Left",
        "Right",
        "Center",
        "Up"
    };

    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
    public static void DrawLocationGizmos(Location location, GizmoType type) {
        if (location == null) {
            return;
        }


        var markerColor = Color.green;
        
        // We are selected if we are in the selection hierarchy.
        var isSelected = (type & GizmoType.InSelectionHierarchy)  != 0;

        // We are selected if we are a PARENT of the currently selected object.
        isSelected |= Selection.activeGameObject != null && Selection.activeGameObject.transform.GetComponentInParent<Location>() == location;

        if (isSelected) {
            markerColor.a  = 1f;
        } else {
            markerColor.a = 0.5f;
        }
        Gizmos.color = markerColor;

        foreach (var marker in location.markers) {
            Gizmos.matrix = marker.localToWorldMatrix;
            Gizmos.DrawCube(Vector3.up * characterPreviewSize.y / 2, characterPreviewSize);
        }

        if (location.cameraMarker != null && Camera.main != null) {
            Gizmos.color = Color.white;
            Gizmos.matrix = location.cameraMarker.localToWorldMatrix;

            float distance = isSelected ? Camera.main.farClipPlane : 1f;

            Gizmos.DrawFrustum(Vector3.zero, Camera.main.fieldOfView, distance, Camera.main.nearClipPlane, Camera.main.aspect);
        }

        
    }

    public override void OnInspectorGUI() {
        base.DrawDefaultInspector();
        
        var location = target as Location;

        if (GUILayout.Button("Set Scene Camera to Here")) {
            // SceneView.lastActiveSceneView.camera.transform.position = location.transform.position;
            // SceneView.lastActiveSceneView.camera.transform.rotation = location.transform.rotation;
            SceneView.lastActiveSceneView.AlignViewToObject(location.cameraMarker.transform);
        }

        if (GUILayout.Button("Set Up Location")) {
            if (location.cameraMarker == null) {
                var newMarker = new GameObject("Camera");
                newMarker.transform.SetParent(location.transform, false);
                location.cameraMarker = newMarker.transform;
                EditorUtility.SetDirty(location);
            }
        }

        if (GUILayout.Button("Add Marker")) {
            string newMarkerName = GetNameForNewMarker(location);
            var newMarker = new GameObject(newMarkerName);
            newMarker.transform.SetParent(location.transform, false);
            location.markers.Add(newMarker.transform);
            EditorUtility.SetDirty(location);
        }
    }

    public static string GetNameForNewMarker(Location location) {
        foreach (var name in DefaultMarkerNames) {
            if (MarkerExistsWithName(location.markers, name) == false) {
                return name;
            }
        }

        return "New Marker";

        static bool MarkerExistsWithName(IEnumerable<Transform> markers, string name) {
            foreach (var marker in markers) {
                if (marker.name == name) {
                    return true;
                }
            }
            return false;
        }
    }
}
#endif
#endif