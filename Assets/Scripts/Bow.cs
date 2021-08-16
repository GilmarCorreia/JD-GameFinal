using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{

    [Header("Arrow Settings")]
    public float arrowCount;
    public Rigidbody arrowPrefab;
    public Transform arrowPos;
    public Transform arrowEquipParent;
    Rigidbody currArrow;
    public float arrowForce = 3f;

    [Header("Bow Equip and Unequip Settings")]
    public Transform EquipPos;
    public Transform UnequipPos;
    public Transform EquipParent;
    public Transform UnequipParent;
    public GameObject bowObj;

    [Header("Bow String Settings")]
    public Transform bowString;
    public Transform stringInitialPos;
    public Transform stringHandPullPos;
    public Transform stringInitialParent;
    //public GameObject BowMesh;

    [Header("Crosshair Settings")]
    public GameObject crosshairPrefab;
    GameObject crosshair;

    public bool canPullString = false;
    bool canFireArrow = false;

    Animator anim;

    void Start()
    {
        anim = GetComponentInParent<Animator>();
        //bowObj.SetActive(false);
        DisableArrow();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EquipBow()
    {
        bowObj.transform.position = EquipPos.position;
        bowObj.transform.rotation = EquipPos.rotation;
        bowObj.transform.parent = EquipParent;
        //ShowBow(true);
    }
    public void DisarmBow()
    {
        bowObj.transform.position = UnequipPos.position;
        bowObj.transform.rotation = UnequipPos.rotation;
        bowObj.transform.parent = UnequipParent;
        //ShowBow(false);
    }
    public void ShowBow(bool show)
    {
        bowObj.SetActive(show);
    }

    public void ShowCrosshair(Vector3 crosshairPos)
    {
        print("CROSSHAIR");
        if (!crosshair)
        {
            crosshair = Instantiate(crosshairPrefab);
        }
        crosshair.transform.position = crosshairPos;
        crosshair.transform.LookAt(Camera.main.transform);
    }

    public void RemoveCrosshair()
    {
        if (crosshair)
            Destroy(crosshair);
    }

    public void PickArrow()
    {
        //currArrow = Instantiate(arrowPrefab,arrowPos.position,arrowPos.rotation);
        arrowPos.gameObject.SetActive(true);
    }

    public void DisableArrow()
    {
        arrowPos.gameObject.SetActive(false);
    }

    public void PullString()
    {
        canPullString = true;
        bowString.position = stringHandPullPos.position;
        bowString.parent = stringHandPullPos;
    }

    public void ReleaseString()
    {
        canPullString = false;
        bowString.position = stringInitialPos.position;
        bowString.parent = stringInitialParent;
    }

    public void Fire(Vector3 hitpoint)
    {
        print("fire arrow");
        Vector3 dir = hitpoint - arrowPos.position;
        currArrow = Instantiate(arrowPrefab,arrowPos.position,arrowPos.rotation);
        //currArrow.AddForce(arrowForce * dir,ForceMode.VelocityChange);
        currArrow.AddForce(arrowForce * dir,ForceMode.VelocityChange);
        currArrow.gameObject.AddComponent<Arrow>();
    }

    public void DrawArrow()
    {
        //if (arrowPrefab != null && firePoint != null)
        //{
        //    arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(6.47f, 94.16f, 0f));
        //    arrowMesh = arrow.transform.Find("arrowMesh").gameObject;
        //    arrow.transform.parent = hand.transform;
        //}
    }

    public void Shoot()
    {
        //arrow.transform.parent = null;
        //arrow.transform.position += 2f * (arrowMesh.transform.Find("head").gameObject.transform.position - arrowMesh.transform.Find("tail").gameObject.transform.position);
        //Rigidbody arrow_rb = arrow.AddComponent<Rigidbody>();
        //BoxCollider arrow_bc = arrow.AddComponent<BoxCollider>();
        //arrow_rb.drag = 1f;
        ////arrow_bc.isTrigger = true;
        //arrow.GetComponent<Rigidbody>().AddForce(3000f * arrow.transform.forward);
        //arrow = null;
        //arrowMesh = null;
    }
}
