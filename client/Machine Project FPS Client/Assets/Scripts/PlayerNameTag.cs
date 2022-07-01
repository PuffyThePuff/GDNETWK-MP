using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameTag : MonoBehaviour
{
    private GameObject namePlate;
    [SerializeField] private Player player;

    private Camera cameraMain;
    private Vector3 cameraDir;

    private void Start()
    {
        // Add namePlate here after instaniation.
        namePlate = new GameObject("NamePlate");
        namePlate.AddComponent<TextMesh>();            // To display name.

        //player = this.gameObject.GetComponent<Player>();

        // Adjust position and settings.
        TextMesh textMesh = namePlate.GetComponent<TextMesh>();
        if (textMesh != null)
        {
            Debug.Log("Adding Nameplate");
            textMesh.transform.position = player.transform.position + new Vector3(0, 1.3f, 0);  // elevate y
            textMesh.transform.rotation = player.transform.rotation * new Quaternion(0, 180.0f, 0, 0);  // turn it around
            textMesh.characterSize = 0.2f;
            textMesh.fontSize = 10;
            textMesh.alignment = TextAlignment.Center;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.color = Color.white;
            textMesh.text = player.GetUsername();
        }

        // Now set the new Player object as its parent.
        namePlate.transform.parent = player.transform;

        cameraMain = Camera.main;
    }

    private void FixedUpdate()
    {
        cameraDir = cameraMain.transform.forward;
        cameraDir.y = 0;

        namePlate.transform.rotation = Quaternion.LookRotation(cameraDir);
    }
}
