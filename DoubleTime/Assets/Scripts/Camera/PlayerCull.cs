using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCull : MonoBehaviour {

    public GameObject target;
    public string checktag;

    public Shader transparentShader;
    [Range(0, 1)] public float chosenAlpha = 0.2f;

    // Materials
    //private Material mainOldMat;
    private Color mainOldColor;
    private Shader mainOldShader;
    private List<Color> mainChildrenColor = new List<Color>();
    private List<Shader> mainChildrenShader = new List<Shader>();

    private Color rightOldColor;
    private Shader rightOldShader;
    private List<Color> rightChildrenColor = new List<Color>();
    private List<Shader> rightChildrenShader = new List<Shader>();

    private Color leftOldColor;
    private Shader leftOldShader;
    private List<Color> leftChildrenColor = new List<Color>();
    private List<Shader> leftChildrenShader = new List<Shader>();

    // Game Objects
    private GameObject mainObj;
    private List<GameObject> mainChildren = new List<GameObject>();
    private GameObject rightObj;
    private List<GameObject> rightChildren = new List<GameObject>();
    private GameObject leftObj;
    private List<GameObject> leftChildren = new List<GameObject>();

    // Renderers
    private Renderer mainRend;
    private Renderer rightRend;
    private Renderer leftRend;

    // Update is called once per frame
    void LateUpdate () {
        Cull();
    }

    // Culls objects blocking the player //
    private void Cull()
    {
        RaycastHit mainHit; // mid

        // Main Ray
        Debug.DrawLine(transform.position, target.transform.position, Color.red);

        // If the ray hits a object
        if (Physics.Linecast(transform.position, target.transform.position, out mainHit))
        {
            // Right and Left Ray
            Debug.DrawRay(mainHit.collider.gameObject.transform.position, Vector3.right, Color.blue);
            Debug.DrawRay(mainHit.collider.gameObject.transform.position, Vector3.left, Color.green);

            if (mainHit.collider.gameObject.tag == checktag)
            {
                //Debug.Log("Main Object: " + mainHit.collider.gameObject.name);

                // Reset previous target material 
                if (mainObj != null)
                {
                    //Debug.Log("Main " + mainObj.name + " is not null");
                    mainRend.material.shader = mainOldShader;
                    mainRend.material.color = mainOldColor;

                    // Reset material for children
                    ClearMaterial(mainChildren, mainChildrenColor, mainChildrenShader);
                }

                // Store reference of target
                mainObj = mainHit.collider.gameObject;

                // Store reference of target Renderer and Color
                mainRend = mainObj.GetComponent<Renderer>();
                mainOldShader = mainRend.material.shader;
                mainOldColor = mainRend.material.color;

                // Set new Alpha
                Color newColor = mainOldColor;
                newColor.a = chosenAlpha;

                // Set target to new Material
                mainRend.material.shader = transparentShader;
                mainRend.material.color = newColor;

                // Set child objects of targetted object as well
                SetChildrenTransparent(mainHit, mainChildren, mainChildrenColor, mainChildrenShader);

                // Side rays if main ray hits
                RightRay(mainHit.collider.gameObject);
                LeftRay(mainHit.collider.gameObject);
            }
            // If the object hid is not part of check tag
            else
            {
                //Debug.Log("Ray not hitting target");
                ResetMaterial();
                ResetSideMaterial();
            }
        }
        // If the ray doesnt hit anything
        else
        {
            //Debug.Log("Ray not hitting anything");
            ResetMaterial();
            ResetSideMaterial();
        }
    }

    // Right Ray
    private void RightRay(GameObject hitObject)
    {
        //Debug.Log("Right Ray Running");

        RaycastHit hit;

        if (Physics.Raycast(hitObject.transform.position + Vector3.up, Vector3.right, out hit))
        {
            if (hit.collider.gameObject.tag == checktag)
            {
                //Debug.Log("Right Object: " + hit.collider.gameObject.name);

                // Reset previous target material 
                if (rightObj != null)
                {
                    //Debug.Log("Right " + rightObj.name + " is not null");
                    rightRend.material.shader = rightOldShader;
                    rightRend.material.color = rightOldColor;

                    // Reset material for children
                    ClearMaterial(rightChildren, rightChildrenColor, rightChildrenShader);
                }

                // Store reference of target
                rightObj = hit.collider.gameObject;

                // Store reference of target Renderer 
                rightRend = rightObj.GetComponent<Renderer>();
                rightOldShader = rightRend.material.shader;
                rightOldColor = rightRend.material.color;

                // Set new Alpha
                Color newColor = rightOldColor;
                newColor.a = chosenAlpha;

                // Set target to new Material
                rightRend.material.shader = transparentShader;
                rightRend.material.color = newColor;

                // Set child objects of targetted object as well
                SetChildrenTransparent(hit, rightChildren, rightChildrenColor, rightChildrenShader);
            }
            else
            {
                ResetSideMaterial();
            }
        }
        else
        {
            ResetSideMaterial();
        }
    }

    // Left Ray
    private void LeftRay(GameObject hitObject)
    {
        //Debug.Log("Left Ray Running");

        RaycastHit hit;

        if (Physics.Raycast(hitObject.transform.position + Vector3.up, Vector3.left, out hit))
        {
            if (hit.collider.gameObject.tag == checktag)
            {
                //Debug.Log("Left Object: " + hit.collider.gameObject.name);

                // Reset previous target material 
                if (leftObj != null)
                {
                    //Debug.Log("Right " + rightObj.name + " is not null");
                    leftRend.material.shader = leftOldShader;
                    leftRend.material.color = leftOldColor;

                    ClearMaterial(leftChildren, leftChildrenColor, leftChildrenShader);
                }

                // Store reference of target
                leftObj = hit.collider.gameObject;

                // Store reference of target Renderer 
                leftRend = leftObj.GetComponent<Renderer>();
                leftOldShader = leftRend.material.shader;
                leftOldColor = leftRend.material.color;

                // Set new Alpha
                Color newColor = leftOldColor;
                newColor.a = chosenAlpha;

                // Set target to new Material
                leftRend.material.shader = transparentShader;
                leftRend.material.color = newColor;

                // Set child objects of targetted object as well
                SetChildrenTransparent(hit, leftChildren, leftChildrenColor, leftChildrenShader);
            }
            else
            {
                ResetSideMaterial();
            }
        }
        else
        {
            ResetSideMaterial();
        }
    }

    // Resets material
    private void ResetMaterial()
    {
        if (mainObj != null)
        {
            //Debug.Log("Main: " + mainObj.name);
            //Debug.Log("Main Material: " + mainOldMat.name);

            mainRend.material.shader = mainOldShader;
            mainRend.material.color = mainOldColor;

            //mainRend.material = mainOldMat; // reset target material
            mainObj = null; // clear reference

            ClearMaterial(mainChildren, mainChildrenColor, mainChildrenShader);
        }
    }

    // Resets material if ray is not hitting anything but variable is still present
    private void ResetSideMaterial()
    {
        if(rightObj != null)
        {
            rightRend.material.shader = rightOldShader;
            rightRend.material.color = rightOldColor;

            rightObj = null;

            ClearMaterial(rightChildren, rightChildrenColor, rightChildrenShader);
        }

        if (leftObj != null)
        {
            leftRend.material.shader = leftOldShader;
            leftRend.material.color = rightOldColor;

            leftObj = null;

            ClearMaterial(leftChildren, leftChildrenColor, leftChildrenShader);
        }
    }

    // Sets Children of hit object to transparent shader and store data on previous material
    private void SetChildrenTransparent(RaycastHit hit, List<GameObject> childList, List<Color> childrenOldColor, List<Shader> childrenMaterial)
    {
        foreach (Transform child in hit.transform)
        {
            childList.Add(child.gameObject);

            Renderer childRend = child.GetComponent<Renderer>();

            // Store old material
            childrenMaterial.Add(childRend.material.shader);
            childrenOldColor.Add(childRend.material.color);

            Color newColor = childRend.material.color;
            newColor.a = chosenAlpha;

            // set current material to transparent material
            childRend.material.shader = transparentShader;
            childRend.material.color = newColor;
        }
    }

    // Resets the material of the object list and clears all list
    private void ClearMaterial(List<GameObject> childList, List<Color> oldColors, List<Shader> oldMaterials)
    {
        //Debug.Log("Child List: " + childList.Count);
        //Debug.Log("Color List: " + oldColors.Count);
        //Debug.Log("Materials List: " + oldMaterials.Count);

        if (childList.Count > 0)
        {
            for (int i = 0; i < childList.Count; i++)
            {
                childList[i].GetComponent<Renderer>().material.shader = oldMaterials[i];
                childList[i].GetComponent<Renderer>().material.color = oldColors[i];
            }

            oldMaterials.Clear();
            childList.Clear();
            oldColors.Clear();
        }
    }
}
