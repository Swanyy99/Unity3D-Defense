using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class MotionTrail : MonoBehaviour {
    
    [Header("Target")]
    public GameObject TargetSkinMesh;

    [Header("EffectSpeed")]
    [Range(0, 1)]
    public float ExportSpeedDelay = 0.1f;

    [Header("EffectTime")]
    public bool UseLifeTime = false; 
    public float EffectLifeTime = 3;

    [Header("ShaderName")]
    public string ValueName;

    [Header("TimeDelay")]
    [Range(0, 1)]
    public float ValueTimeDelay = 0.1f;

    [Header("Adder")]
    [Range(0, 1)]
    public float ValueDetail = 0.1f;

    public PlayerController player;


    private bool NeedObject;
    private void OnEnable()
    {

        if (TargetSkinMesh != null && ValueName != "")
        {
            StopAllCoroutines();
            StartCoroutine("GhostStart");

            if (UseLifeTime == true)
            {
                StartCoroutine("TimerStart");
            }

        }
        
    }
    IEnumerator GhostStart()
    {
        while (true)
        {
            NeedObject = false;

            if (player.isDash == true)
            {
                for (int i = 1; i < transform.childCount + 1; i++)
                {

                    NeedObject = CreateTrailMotion(i); // 새로운 모션 생성
                    if (NeedObject == true) // 모션을 생성하고 성공 했는지의 여부
                    {
                        // Debug.Log("모션 생성 성공");
                        break;
                    }
                }

            }
            yield return new WaitForSeconds(ExportSpeedDelay);
        }
    }
    IEnumerator TimerStart() // 타이머
    {
        yield return new WaitForSeconds(EffectLifeTime);
        StopAllCoroutines();
        yield return null;
    }

    public bool CreateTrailMotion(int ArrayNum)
    {
        
            if (ArrayNum < transform.childCount)
            {
                if (transform.GetChild(ArrayNum).gameObject.activeSelf == false) // 오브젝트가 비활성화 되어 있음 (사용가능)
                {
                    transform.GetChild(ArrayNum).gameObject.transform.position = TargetSkinMesh.transform.position;
                    transform.GetChild(ArrayNum).gameObject.transform.rotation = TargetSkinMesh.transform.rotation;
                    transform.GetChild(ArrayNum).gameObject.GetComponent<MotionTrailRenderer>().SkinMesh = TargetSkinMesh.GetComponent<SkinnedMeshRenderer>();

                    transform.GetChild(ArrayNum).gameObject.GetComponent<MotionTrailRenderer>().ValueName = ValueName;
                    transform.GetChild(ArrayNum).gameObject.GetComponent<MotionTrailRenderer>().ValueTimeDelay = ValueTimeDelay;
                    transform.GetChild(ArrayNum).gameObject.GetComponent<MotionTrailRenderer>().ValueDetail = ValueDetail;
                    transform.GetChild(ArrayNum).gameObject.SetActive(true);
                    return true;
                }
                else
                {
                    
                    if (transform.childCount == ArrayNum + 1)
                    {
                        Instantiate(transform.GetChild(0), this.transform);
                    }
                    
                    return false;
                }
            }
            else // 갯수 부족
            {
                Instantiate(transform.GetChild(0), this.transform); //새로운 모션 생성
                return false;
            }
       

    }

}
