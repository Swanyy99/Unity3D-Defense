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
                    #region
                    //transform.GetChild(i).gameObject.transform.position = TargetSkinMesh.transform.position;
                    //transform.GetChild(i).gameObject.transform.rotation = TargetSkinMesh.transform.rotation;
                    //transform.GetChild(i).gameObject.GetComponent<MotionTrailRenderer>().SkinMesh = TargetSkinMesh.GetComponent<SkinnedMeshRenderer>();

                    //transform.GetChild(i).gameObject.GetComponent<MotionTrailRenderer>().ValueName = ValueName;
                    //transform.GetChild(i).gameObject.GetComponent<MotionTrailRenderer>().ValueTimeDelay = ValueTimeDelay;
                    //transform.GetChild(i).gameObject.GetComponent<MotionTrailRenderer>().ValueDetail = ValueDetail;
                    //transform.GetChild(i).gameObject.SetActive(true);
                    #endregion

                    NeedObject = CreateTrailMotion(i); //새로운 모션 생성
                    if (NeedObject == true) //모션을 생성하고 성공 했는지의 여부를 받아옵니다. true일 경우 생성이 된 것입니다.
                    {
                        //Debug.Log("모션 생성 성공");
                        break;
                    }
                }
                //if(NeedObject == false)
                //{
                //    Instantiate(transform.GetChild(0), this.transform);
                //    #if UNITY_EDITOR
                //    Debug.Log("<color=red>" + "Ghost 오브젝트가 부족합니다." + "</color>" + "\n 해결방법1 : Export Speed Delay를 올려주세요. \n 해결방법2 : Value Time Delay를 내려주세요. \n 해결방법3 : Value Detail을 올려주세요. \n 해결방법4 : Ghost를 더 복제 해주세요.");
                //    #endif
                //}
            }
            yield return new WaitForSeconds(ExportSpeedDelay);
        }
    }
    IEnumerator TimerStart() //타이머
    {
        yield return new WaitForSeconds(EffectLifeTime);
        StopAllCoroutines();
        yield return null;
    }

    public bool CreateTrailMotion(int ArrayNum)
    {
        
            if (ArrayNum < transform.childCount)
            {
                if (transform.GetChild(ArrayNum).gameObject.activeSelf == false) //오브젝트가 비활성화 되어 있음 (사용가능)
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
            else //갯수 부족
            {
                //Debug.Log("<color=red>" + "모션생성(갯수부족2)" + "</color>");
                Instantiate(transform.GetChild(0), this.transform); //새로운 모션을 생성합니다.
                return false;
            }
       

    }

}
