using UnityEngine;
using System;
using System.Collections;

public class Disease : MonoBehaviour
{
    public Rigidbody m_Body;
    public event Action<GameObject> OnOtherBodyTouch;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Crowder")
        {
            DiseaseCrowder(other.gameObject);

//            if (OnOtherBodyTouch != null)
//            {
//                OnOtherBodyTouch(other.gameObject);
//            }
        }
    }

    void DiseaseCrowder(GameObject crowderBody)
    {
        PanicManager panicManager = PanicManager.Instance;

        // get real behaviour carrier
        GameObject crowderBehaviourGO = crowderBody.transform.parent.gameObject;

        // cash crowder position
        Vector3 crowderPos = crowderBehaviourGO.transform.localPosition;

        // remove from victims' list and destroy
        panicManager.m_Spawner.m_Crowders.Remove(crowderBehaviourGO);
        Destroy(crowderBehaviourGO);

        GameObject zombi = Instantiate(panicManager.m_Diseased, transform.parent.parent) as GameObject;
        zombi.transform.localScale = Vector3.one;
        zombi.transform.localPosition = crowderPos;

        ChaserMovement chaserBehaviour = zombi.GetComponent<ChaserMovement>();

        PanicManager.Instance.m_Chasers.Add(chaserBehaviour);
        PanicManager.Instance.m_PanicPoints.Add(chaserBehaviour.GetComponent<PanicPoint>());
        chaserBehaviour.Init();
    }

    void OldDiseaseCrowder(GameObject crowderBody)
    {
        // get real behaviour carrier
        GameObject crowderBehaviourGO = crowderBody.transform.parent.gameObject;

        // rename to diseased
        crowderBody.tag = "Zombi";
        crowderBehaviourGO.tag = "Zombi";

        // disable crowder component
        crowderBehaviourGO.GetComponent<PanicMovement>().enabled = false;

        // introduce disease component
        Disease newZombi = crowderBody.AddComponent<Disease>();
        newZombi.m_Body = newZombi.gameObject.GetComponent<Rigidbody>();

        // introduce panic component
        PanicPoint panicSource = crowderBehaviourGO.AddComponent<PanicPoint>();
        panicSource.m_FearStrength = 1;

        // introduce chaser component
        ChaserMovement chaserBehaviour = crowderBehaviourGO.AddComponent<ChaserMovement>();
        chaserBehaviour.m_Disease = newZombi;

        // change material
        Material newZombiMaterial = newZombi.m_Body.gameObject.GetComponent<Material>();
        newZombiMaterial = PanicManager.Instance.m_DiseaseMaterial;

        PanicManager.Instance.m_Chasers.Add(chaserBehaviour);
        PanicManager.Instance.m_PanicPoints.Add(panicSource);
        chaserBehaviour.Init();
    }
}
