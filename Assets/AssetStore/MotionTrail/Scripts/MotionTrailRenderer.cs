using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionTrailRenderer : MonoBehaviour 
{

    [HideInInspector]public SkinnedMeshRenderer SkinMesh; //출력하고 싶은 타겟 스킨 렌더 (캐릭터)
    [HideInInspector]public string ValueName;
    [HideInInspector]public float ValueDetail;
    [HideInInspector]public float ValueTimeDelay;

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine("MaterialColorAnimation");
    }


    Mesh BakedMeshResult; //베이크 및 출력용 메쉬 데이터
    IEnumerator MaterialColorAnimation()
    {

        if (BakedMeshResult == null) //메쉬 데이터가 없을 경우 새로운 메쉬 데이터 생성
        {
            BakedMeshResult = new Mesh();
        }

        SkinMesh.BakeMesh(BakedMeshResult); //타겟 스킨메쉬 -> BakeMesh에 구움
        this.GetComponent<MeshFilter>().mesh = BakedMeshResult; //해당 메쉬필터에 구운 메쉬를 렌더링한다.
        
        for (float e = 0; e < 1.1; e += ValueDetail)
        {
            this.GetComponent<MeshRenderer>().material.SetFloat(ValueName, e);
            yield return new WaitForSeconds(ValueTimeDelay);
        }

        this.gameObject.SetActive(false);
    }
}
