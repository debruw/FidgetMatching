using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class GirlVRTKPrefabMaker : MonoBehaviour
{
    public bool allOptions;
    int hair;
    int chest;
    int skintone;
    public bool glassesactive;
    public bool hatactive;
    GameObject GOhead;
    GameObject GOhands;
    GameObject[] GOhair;
    GameObject[] GOchest;
    GameObject GOglasses;
    Object[] MATskins;
    Object[] MAThairA;
    Object[] MAThairB;
    Object[] MAThairC;
    Object[] MAThairD;
    Object[] MAThairE;
    Object[] MAThairF;
    Object[] MATeyes;
    Object[] MATglasses;
    Object[] MATshirt;
    Object[] MATtshirt;
    Object[] MATsweater;   
    Object[] MAThatA;
    Object[] MAThatB;
    Object[] MATteeth;
    int eyeindex;
    int skinindex;
    int teethindex;
    string model;

    void Start()
    {
        allOptions = false;
    }

    public void Getready()
    {
        GOhead = (GetComponent<Transform>().GetChild(2).gameObject);
        Identifymodel();
        GOhands = (GetComponent<Transform>().GetChild(10).gameObject);
        GOhair = new GameObject[8];
        GOchest = new GameObject[4];

        //load models
        for (int forAUX = 0; forAUX < 2; forAUX++) GOhair[forAUX + 6] = (GetComponent<Transform>().GetChild(forAUX).gameObject);
        for (int forAUX = 0; forAUX < 6; forAUX++) GOhair[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 4).gameObject);
        for (int forAUX = 0; forAUX < 4; forAUX++) GOchest[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 11).gameObject);
        GOglasses = transform.Find("ROOT/TK/TK Pelvis/TK Spine/TK Spine1/TK Spine2/TK Neck/TK Head/Glasses").gameObject as GameObject;

        //load  materials
        MATskins = Resources.LoadAll("Materials/GIRL/Skins/" + model, typeof(Material));
        MATglasses = Resources.LoadAll("Materials/COMMON/Glasses", typeof(Material));
        MATeyes = Resources.LoadAll("Materials/COMMON/Eyes", typeof(Material));
        MATtshirt = Resources.LoadAll("Materials/COMMON/Tshirt", typeof(Material));
        MATshirt = Resources.LoadAll("Materials/COMMON/Shirt", typeof(Material));
        MATsweater = Resources.LoadAll("Materials/COMMON/Sweater", typeof(Material));        
        MAThatA = Resources.LoadAll("Materials/COMMON/HatA", typeof(Material));
        MAThatB = Resources.LoadAll("Materials/GIRL/HatB", typeof(Material));
        MAThairA = Resources.LoadAll("Materials/GIRL/Hairs/HairA", typeof(Material));
        MAThairB = Resources.LoadAll("Materials/GIRL/Hairs/HairB", typeof(Material));
        MAThairC = Resources.LoadAll("Materials/GIRL/Hairs/HairC", typeof(Material));
        MAThairD = Resources.LoadAll("Materials/GIRL/Hairs/HairD", typeof(Material));
        MAThairE = Resources.LoadAll("Materials/GIRL/Hairs/HairE", typeof(Material));
        MAThairF = Resources.LoadAll("Materials/COMMON/Hair", typeof(Material));
        MATteeth = Resources.LoadAll("Materials/COMMON/Teeth", typeof(Material));

        if (GOhair[0].activeSelf && GOhair[1].activeSelf && GOhair[2].activeSelf)
        {
            Resetskin(MATskins[0] as Material);
            Randomize();
        }
        else
        {
            for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) { if (GOhair[forAUX].activeSelf) hair = forAUX; }
            while (!GOchest[chest].activeSelf) chest++;
            
            if (hair > 6) hatactive = true;

            Material[] AUXmaterials; int MATindex = 0;
            AUXmaterials = GOhead.GetComponent<Renderer>().sharedMaterials;
            while (AUXmaterials[skinindex].name != MATskins[MATindex].name) MATindex++;
            Resetskin(MATskins[MATindex] as Material);
        }
    }
    public void Identifymodel()
    {
        Object[] tempMATA = Resources.LoadAll("Materials/GIRL/skins/TKGirlA", typeof(Material));
        Object[] tempMATB = Resources.LoadAll("Materials/GIRL/skins/TKGirlB", typeof(Material));
        Object[] tempMATC = Resources.LoadAll("Materials/GIRL/skins/TKGirlC", typeof(Material));
        string theskin = GOhead.GetComponent<Renderer>().sharedMaterials[0].name;
        for (int forAUX = 0; forAUX < tempMATA.Length; forAUX++)
        {
            if (theskin == tempMATA[forAUX].name) model = "TKGirlA";
        }
        for (int forAUX = 0; forAUX < tempMATB.Length; forAUX++)
        {
            if (theskin == tempMATB[forAUX].name) model = "TKGirlB";
        }
        for (int forAUX = 0; forAUX < tempMATC.Length; forAUX++)
        {
            if (theskin == tempMATC[forAUX].name) model = "TKGirlC";
        }
        eyeindex = 1; skinindex = 0; teethindex = 2;
    }
    public void Deactivateall()
    {
        for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) GOhair[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) GOchest[forAUX].SetActive(false);        
        GOglasses.SetActive(false);
        glassesactive = false;
    }
    public void Activateall()
    {
        for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) GOhair[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) GOchest[forAUX].SetActive(true);        
        GOglasses.SetActive(true);
        glassesactive = true;
    }
    public void Menu()
    {
        allOptions = !allOptions;
    }
    public void Glasseson()
    {
        glassesactive = !glassesactive;
        GOglasses.SetActive(glassesactive);
    }
    

    //models
    public void NextHat()
    {
        if (!hatactive)
        {
            GOhair[hair].SetActive(false);
            hair = 6;
            GOhair[hair].SetActive(true);
            hatactive = true;
        }
        else
        {
            GOhair[hair].SetActive(false);
            hair++; if (hair > GOhair.Length - 1) hair = 6;
            GOhair[hair].SetActive(true);
            hatactive = true;
        }
    }
    public void PrevHat()
    {
        if (!hatactive)
        {
            GOhair[hair].SetActive(false);
            hair = 7;
            GOhair[hair].SetActive(true);
            hatactive = true;
        }
        else
        {
            GOhair[hair].SetActive(false);
            hair--; if (hair < GOhair.Length - 2) hair = 7;
            GOhair[hair].SetActive(true);
            hatactive = true;
        }
    }
    public void Nexthair()
    {
        GOhair[hair].SetActive(false);
        if (hatactive) hair = 0;
        hatactive = false;
        if (hair < GOhair.Length - 3) hair++;
        else hair = 0;
        GOhair[hair].SetActive(true);
    }
    public void Prevhair()
    {
        GOhair[hair].SetActive(false);
        if (hatactive) hair = GOhair.Length - 2;
        hatactive = false;
        if (hair > 0) hair--;
        else hair = GOhair.Length - 2;
        GOhair[hair].SetActive(true);
    }
    public void Nextchest()
    {
        GOchest[chest].SetActive(false);
        if (chest < GOchest.Length - 1) chest++;
        else chest = 0;
        GOchest[chest].SetActive(true);
    }
    public void Prevchest()
    {
        GOchest[chest].SetActive(false);
        chest--;
        if (chest < 0) chest = GOchest.Length - 1;
        GOchest[chest].SetActive(true);
    }
    

    //materials
    public void Nextskincolor(int todo)
    {
        //head
        SetGOmaterials(GOhead, MATskins, skinindex, todo);
        SetGOmaterials(GOhands, MATskins, skinindex, todo);

        //chest
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++)
        {
            SetGOmaterials(GOchest[forAUX], MATskins, 1, todo);
        } 
    }
    public void Nextglasses(int todo)
    {
        SetGOmaterial(GOglasses, MATglasses, todo);
    }
    public void Nexteyescolor(int todo)
    {
        SetGOmaterials(GOhead, MATeyes, eyeindex, todo);
    }
    public void Nextteethcolor(int todo)
    {
        SetGOmaterials(GOhead, MATteeth, teethindex, todo);
    }
    public void Nexthaircolor(int todo)
    {
        if (hair == 0) SetGOmaterial(GOhair[0], MAThairA, todo);
        if (hair == 1) SetGOmaterial(GOhair[1], MAThairB, todo);
        if (hair == 2) SetGOmaterial(GOhair[2], MAThairC, todo);
        if (hair == 3) SetGOmaterial(GOhair[3], MAThairD, todo);
        if (hair == 4) SetGOmaterial(GOhair[4], MAThairE, todo);
        if (hair == 5) SetGOmaterial(GOhair[5], MAThairF, todo);
        if (hair > 5) Setmaterials(GOhair, MAThairB, 1, todo);
    }
    public void Nexthatcolor(int todo)
    {
        SetGOmaterials(GOhair[6], MAThatA, 0, todo);
        SetGOmaterials(GOhair[7], MAThatB, 0, todo);
    }
    public void Nextchestcolor(int todo)
    {
        if (chest == 0) SetGOmaterial(GOchest[0], MATshirt, todo);
        if (chest == 1) Setmaterial(GOchest, MATsweater, todo);
        if (chest > 1) Setmaterial(GOchest, MATtshirt, todo);
    }
    

    public void Resetmodel()
    {
        Resetskin(MATskins[0] as Material);
        Activateall();
        Menu();
    }
    public void Resetskin(Material skinbase)
    {
        //head
        Material[] AUXmaterials;
        AUXmaterials = GOhead.GetComponent<Renderer>().sharedMaterials;
        AUXmaterials[skinindex] = skinbase;
        GOhead.GetComponent<Renderer>().sharedMaterials = AUXmaterials;
        GOhands.GetComponent<Renderer>().sharedMaterial = skinbase;

        //chest          
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++)
        {
            AUXmaterials = GOchest[forAUX].GetComponent<Renderer>().sharedMaterials;
            AUXmaterials[1] = skinbase;
            GOchest[forAUX].GetComponent<Renderer>().sharedMaterials = AUXmaterials;
        } 
    }
    public void Randomize()
    {
        Deactivateall();
        //models
        hair = Random.Range(0, GOhair.Length);
        GOhair[hair].SetActive(true);
        if (hair > 4) hatactive = true; else hatactive = false;
        chest = Random.Range(0, GOchest.Length); GOchest[chest].SetActive(true);
       
        if (Random.Range(0, 4) > 2)
        {
            glassesactive = true;
            GOglasses.SetActive(true);
            SetGOmaterial(GOglasses, MATglasses, 2);
        }
        else glassesactive = false;

        //materials
        SetGOmaterials(GOhead, MATeyes, eyeindex, 2);
        SetGOmaterials(GOhead, MATteeth, teethindex, 2);
        for (int forAUX = 0; forAUX < (Random.Range(0, 4)); forAUX++) Nexthaircolor(0);        
        for (int forAUX = 0; forAUX < (Random.Range(0, 12)); forAUX++) Nexthatcolor(0);
        for (int forAUX = 0; forAUX < (Random.Range(0, 17)); forAUX++) Nextchestcolor(0);
        for (int forAUX = 0; forAUX < (Random.Range(0, 4)); forAUX++) Nextskincolor(0);

    }
    public void CreateCopy()
    {
        GameObject newcharacter = Instantiate(gameObject, transform.position, transform.rotation);
        for (int forAUX = 28; forAUX > 0; forAUX--)
        {
            if (!newcharacter.transform.GetChild(forAUX).gameObject.activeSelf) DestroyImmediate(newcharacter.transform.GetChild(forAUX).gameObject);
        }
        if (!GOglasses.activeSelf) DestroyImmediate(newcharacter.transform.Find("ROOT/TK/TK Pelvis/TK Spine/TK Spine1/TK Spine2/TK Neck/TK Head/Glasses").gameObject as GameObject);
        DestroyImmediate(newcharacter.GetComponent<GirlVRTKPrefabMaker>());
    }
    public void FIX()
    {
        GameObject newcharacter = Instantiate(gameObject, transform.position, transform.rotation);
        for (int forAUX = 28; forAUX > 0; forAUX--)
        {
            if (!newcharacter.transform.GetChild(forAUX).gameObject.activeSelf) DestroyImmediate(newcharacter.transform.GetChild(forAUX).gameObject);
        }
        if (!GOglasses.activeSelf) DestroyImmediate(newcharacter.transform.Find("ROOT/TK/TK Pelvis/TK Spine/TK Spine1/TK Spine2/TK Neck/TK Head/Glasses").gameObject as GameObject);
        DestroyImmediate(newcharacter.GetComponent<GirlVRTKPrefabMaker>());
        DestroyImmediate(gameObject);
    }


    public void Setmaterial(GameObject[] GO, Object[] MAT, int todo)
    {
        int GOindex = 0;
        int MATindex = 0;
        Material AUXmaterial;
        for (int forAUX = 0; forAUX < GO.Length; forAUX++)
        {
            if (GO[forAUX].activeSelf) GOindex = forAUX;
        }
        AUXmaterial = GO[GOindex].GetComponent<Renderer>().sharedMaterial;
        while (AUXmaterial.name != MAT[MATindex].name) MATindex++;
        if (todo == 0) //increase
        {
            MATindex++;
            if (MATindex > MAT.Length - 1) MATindex = 0;
        }
        if (todo == 1) //decrease
        {
            MATindex--;
            if (MATindex < 0) MATindex = MAT.Length - 1;
        }
        if (todo == 2) //random value
        {
            MATindex = Random.Range(0, MAT.Length);
        }
        AUXmaterial = MAT[MATindex] as Material;
        GO[GOindex].GetComponent<Renderer>().sharedMaterial = AUXmaterial;
    }
    public void Setmaterials(GameObject[] GO, Object[] MAT, int matchannel, int todo)
    {
        int GOindex = 0;
        int MATindex = 0;
        Material[] AUXmaterials;
        for (int forAUX = 0; forAUX < GO.Length; forAUX++)
        {
            if (GO[forAUX].activeSelf) GOindex = forAUX;
        }
        AUXmaterials = GO[GOindex].GetComponent<Renderer>().sharedMaterials;
        while (AUXmaterials[matchannel].name != MAT[MATindex].name)
        {
            MATindex++;
        }
        
        if (todo == 0) //increase
        {
            MATindex++;
            if (MATindex > MAT.Length - 1) MATindex = 0;
        }
        if (todo == 1) //decrease
        {
            MATindex--;
            if (MATindex < 0) MATindex = MAT.Length - 1;
        }
        if (todo == 2) //random value
        {
            MATindex = Random.Range(0, MAT.Length);
        }
        AUXmaterials[matchannel] = MAT[MATindex] as Material;
        GO[GOindex].GetComponent<Renderer>().sharedMaterials = AUXmaterials;
    }
    public void SetGOmaterial(GameObject GO, Object[] MAT, int todo)
    {
        int MATindex = 0;
        Material AUXmaterial;
        AUXmaterial = GO.GetComponent<Renderer>().sharedMaterial;
        while (AUXmaterial.name != MAT[MATindex].name) MATindex++;
        if (todo == 0) //increase
        {
            MATindex++;
            if (MATindex > MAT.Length - 1) MATindex = 0;
        }
        if (todo == 1) //decrease
        {
            MATindex--;
            if (MATindex < 0) MATindex = MAT.Length - 1;
        }
        if (todo == 2) //random value
        {
            MATindex = Random.Range(0, MAT.Length);
        }
        AUXmaterial = MAT[MATindex] as Material;
        GO.GetComponent<Renderer>().sharedMaterial = AUXmaterial;
    }
    public void SetGOmaterials(GameObject GO, Object[] MAT, int matchannel, int todo)
    {
        int MATindex = 0;
        Material[] AUXmaterials;
        AUXmaterials = GO.GetComponent<Renderer>().sharedMaterials;
        while (AUXmaterials[matchannel].name != MAT[MATindex].name) MATindex++;
        if (todo == 0) //increase
        {
            MATindex++;
            if (MATindex > MAT.Length - 1) MATindex = 0;
        }
        if (todo == 1) //decrease
        {
            MATindex--;
            if (MATindex < 0) MATindex = MAT.Length - 1;
        }
        if (todo == 2) //random value
        {
            MATindex = Random.Range(0, MAT.Length);
        }
        AUXmaterials[matchannel] = MAT[MATindex] as Material;
        GO.GetComponent<Renderer>().sharedMaterials = AUXmaterials;
    }
}