using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AnimalDragonPackCharacterButton : MonoBehaviour
{

    public GameObject Animal;
    public GameObject ShootPoint;
    public SkinnedMeshRenderer bodyMesh;
    public SkinnedMeshRenderer faceMesh;

    public Texture[] faceTextureArray = new Texture[9];
    public Texture[] bodyTextureArray = new Texture[4];
    public GameObject[] effPrefabArray = new GameObject[9];

    // Use this for initialization
    void Start()
    {

    }

    void EffectClear()
    {
        GameObject tFindObj = GameObject.FindGameObjectWithTag("Effect");
        if (tFindObj != null)
        {
            DestroyImmediate(tFindObj);
        }
    }


    void OnGUI()
    {
        if (GUI.Button(new Rect(20, 20, 70, 40), "Idle"))
        {
            EffectClear();
            Animal.GetComponent<Animation>().wrapMode = WrapMode.Loop;
            Animal.GetComponent<Animation>().CrossFade("Idle");
            faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[0]);
        }
        if (GUI.Button(new Rect(90, 20, 70, 40), "Stand"))
        {
            EffectClear();
            Animal.GetComponent<Animation>().wrapMode = WrapMode.Loop;
            Animal.GetComponent<Animation>().CrossFade("Stand");
            faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[5]);
            if (GameObject.FindGameObjectWithTag("Effect") == null) GameObject.Instantiate(effPrefabArray[6]);
        }
        if (GUI.Button(new Rect(160, 20, 70, 40), "Walk"))
        {
            EffectClear();
            Animal.GetComponent<Animation>().wrapMode = WrapMode.Loop;
            Animal.GetComponent<Animation>().CrossFade("Walk");
            faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[0]);
        }
        if (GUI.Button(new Rect(230, 20, 70, 40), "Run"))
        {
            EffectClear();
            Animal.GetComponent<Animation>().wrapMode = WrapMode.Loop;
            Animal.GetComponent<Animation>().CrossFade("Run");
            faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[6]);
            if (GameObject.FindGameObjectWithTag("Effect") == null) GameObject.Instantiate(effPrefabArray[5]);

        }
        if (GUI.Button(new Rect(300, 20, 70, 40), "Attack"))
        {
            EffectClear();
            Animal.GetComponent<Animation>().wrapMode = WrapMode.Once;
            Animal.GetComponent<Animation>().CrossFade("Attack");
            faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[3]);

            Vector3 playerV = new Vector3(ShootPoint.transform.position.x, ShootPoint.transform.position.y, ShootPoint.transform.position.z);
            Instantiate(effPrefabArray[0], new Vector3(playerV.x, playerV.y, playerV.z), Animal.transform.rotation);
        }
        if (GUI.Button(new Rect(370, 20, 90, 40), "AttackStand"))
        {
            EffectClear();
            Animal.GetComponent<Animation>().wrapMode = WrapMode.Loop;
            Animal.GetComponent<Animation>().CrossFade("AttackStand");
            faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[7]);
            if (GameObject.FindGameObjectWithTag("Effect") == null) GameObject.Instantiate(effPrefabArray[2]);
        }

        if (GUI.Button(new Rect(460, 20, 70, 40), "Damage"))
        {
            EffectClear();
            Animal.GetComponent<Animation>().wrapMode = WrapMode.Once;
            Animal.GetComponent<Animation>().CrossFade("Damage");
            faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[8]);
            if (GameObject.FindGameObjectWithTag("Effect") == null) GameObject.Instantiate(effPrefabArray[3]);
        }

        //if (GUI.Button(new Rect(530, 20, 70, 40), "Eat"))
        //{
        //    EffectClear();
        //    Animal.GetComponent<Animation>().wrapMode = WrapMode.Loop;
        //    Animal.GetComponent<Animation>().CrossFade("Eat");
        //    faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[5]);
        //    if (GameObject.FindGameObjectWithTag("Effect") == null) GameObject.Instantiate(effPrefabArray[7]);
        //}
        //if (GUI.Button(new Rect(600, 20, 70, 40), "Sleep"))
        //{
        //    EffectClear();
        //    Animal.GetComponent<Animation>().wrapMode = WrapMode.Loop;
        //    Animal.GetComponent<Animation>().CrossFade("Sleep");
        //    faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[4]);
        //    if (GameObject.FindGameObjectWithTag("Effect") == null) GameObject.Instantiate(effPrefabArray[1]);
        //}
        if (GUI.Button(new Rect(530, 20, 70, 40), "Breath"))
        {
            EffectClear();
            Animal.GetComponent<Animation>().wrapMode = WrapMode.Once;
            Animal.GetComponent<Animation>().CrossFade("Breath");
            faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[1]);
            //if (GameObject.FindGameObjectWithTag("Effect") == null) GameObject.Instantiate(effPrefabArray[8]);

            Vector3 playerV = new Vector3(ShootPoint.transform.position.x, ShootPoint.transform.position.y, ShootPoint.transform.position.z);
            Instantiate(effPrefabArray[8], new Vector3(playerV.x, playerV.y, playerV.z), Animal.transform.rotation);
            
        }
        if (GUI.Button(new Rect(600, 20, 70, 40), "Die"))
        {
            EffectClear();
            Animal.GetComponent<Animation>().wrapMode = WrapMode.Once;
            Animal.GetComponent<Animation>().CrossFade("Die");
            faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[2]);
            if (GameObject.FindGameObjectWithTag("Effect") == null) GameObject.Instantiate(effPrefabArray[4]);
        }
        /////////////////////////////////////////////////////////////////////



        /////////////////////////////////////////////////////////////////////
        if (GUI.Button(new Rect(20, 700, 120, 40), "RandomFace"))
        {
            faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[Random.Range(0, faceTextureArray.Length)]);
        }
        if (GUI.Button(new Rect(150, 700, 70, 40), "Face01"))
        {
            faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[0]);
        }
        if (GUI.Button(new Rect(220, 700, 70, 40), "Face02"))
        {
            faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[1]);
        }
        if (GUI.Button(new Rect(290, 700, 70, 40), "Face03"))
        {
            faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[2]);
        }
        if (GUI.Button(new Rect(360, 700, 70, 40), "Face04"))
        {
            faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[3]);
        }
        if (GUI.Button(new Rect(430, 700, 70, 40), "Face05"))
        {
            faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[4]);
        }
        if (GUI.Button(new Rect(500, 700, 70, 40), "Face06"))
        {
            faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[5]);
        }
        if (GUI.Button(new Rect(570, 700, 70, 40), "Face07"))
        {
            faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[6]);
        }
        if (GUI.Button(new Rect(640, 700, 70, 40), "Face08"))
        {
            faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[7]);
        }
        if (GUI.Button(new Rect(710, 700, 70, 40), "Face09"))
        {
            faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[8]);
        }
        //if (GUI.Button(new Rect(780, 700, 70, 40), "Face10"))
        //{
        //    faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[9]);
        //}
        /////////////////////////////////////////////////////////////////////////////////

        if (GUI.Button(new Rect(20, 740, 120, 40), "RandomBody"))
        {
            bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[Random.Range(0, bodyTextureArray.Length)]);
        }
        if (GUI.Button(new Rect(150, 740, 70, 40), "Body_01"))
        {
            bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[0]);
        }
        if (GUI.Button(new Rect(220, 740, 70, 40), "Body_02"))
        {
            bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[1]);
        }
        if (GUI.Button(new Rect(290, 740, 70, 40), "Body_03"))
        {
            bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[2]);
        }
        if (GUI.Button(new Rect(360, 740, 70, 40), "Body_04"))
        {
            bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[3]);
        }
        if (GUI.Button(new Rect(430, 740, 70, 40), "Body_05"))
        {
            bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[4]);
        }
        if (GUI.Button(new Rect(500, 740, 70, 40), "Body_06"))
        {
            bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[5]);
        }
        if (GUI.Button(new Rect(570, 740, 70, 40), "Body_07"))
        {
            bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[6]);
        }
        if (GUI.Button(new Rect(640, 740, 70, 40), "Body_08"))
        {
            bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[7]);
        }
        ////////////////////////////////////////////////////////////////////
        if (GUI.Button(new Rect(720, 280, 120, 40), "Fire Dragon 1"))
        {
            SceneManager.LoadScene("Dragon_11");
        }

        if (GUI.Button(new Rect(720, 320, 120, 40), "Fire Dragon 2"))
        {
            SceneManager.LoadScene("Dragon_12");
        }

        if (GUI.Button(new Rect(720, 360, 120, 40), "Fire Dragon 3"))
        {
            SceneManager.LoadScene("Dragon_13");
        }
        if (GUI.Button(new Rect(720, 400, 120, 40), "Ice Dragon 1"))
        {
            SceneManager.LoadScene("Dragon_21");
        }

        if (GUI.Button(new Rect(720, 440, 120, 40), "Ice Dragon 2"))
        {
            SceneManager.LoadScene("Dragon_22");
        }

        if (GUI.Button(new Rect(720, 480, 120, 40), "Ice Dragon 3"))
        {
            SceneManager.LoadScene("Dragon_23");
        }
        if (GUI.Button(new Rect(720, 520, 120, 40), "Thunder Dragon 1"))
        {
            SceneManager.LoadScene("Dragon_31");
        }

        if (GUI.Button(new Rect(720, 560, 120, 40), "Thunder Dragon 2"))
        {
            SceneManager.LoadScene("Dragon_32");
        }

        if (GUI.Button(new Rect(720, 600, 120, 40), "Thunder Dragon 3"))
        {
            SceneManager.LoadScene("Dragon_33");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

}
