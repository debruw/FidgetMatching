using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class BoyTKPrefabMaker : MonoBehaviour
{
    public bool allOptions;
    int hair;
    int chest;
    int legs;
    int feet;
    int skintone;
    public bool glassesactive;
    public bool hatactive;
    GameObject GOhead;
    GameObject[] GOfeet;
    GameObject[] GOhair;
    GameObject[] GOchest;
    GameObject[] GOlegs;
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
    Object[] MATlegs;
    Object[] MATfeetA;
    Object[] MATfeetB;
    Object[] MATfeetC;
    Object[] MAThat;
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
        GOhead = (GetComponent<Transform>().GetChild(1).gameObject);
        Identifymodel();

        GetComponent<Transform>().GetChild(2).gameObject.SetActive(false);
        GOfeet = new GameObject[3];
        GOhair = new GameObject[7];
        GOchest = new GameObject[6];
        GOlegs = new GameObject[5];

        //load models
        for (int forAUX = 0; forAUX < 3; forAUX++) GOfeet[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 10).gameObject);
        for (int forAUX = 0; forAUX < 6; forAUX++) GOhair[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 4).gameObject);
        GOhair[6] = (GetComponent<Transform>().GetChild(0).gameObject);
        for (int forAUX = 0; forAUX < 6; forAUX++) GOchest[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 18).gameObject);
        for (int forAUX = 0; forAUX < 5; forAUX++) GOlegs[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 13).gameObject);
        GOglasses = transform.Find("ROOT/TK/TK Pelvis/TK Spine/TK Spine1/TK Spine2/TK Neck/TK Head/Glasses").gameObject as GameObject;

        //load  materials
        MATskins = Resources.LoadAll  ("Materials/BOY/skins/" + model, typeof(Material));
        MATglasses = Resources.LoadAll("Materials/COMMON/Glasses", typeof(Material));
        MATeyes = Resources.LoadAll   ("Materials/COMMON/Eyes", typeof(Material));
        MATtshirt = Resources.LoadAll ("Materials/COMMON/Tshirt", typeof(Material));
        MATshirt = Resources.LoadAll("Materials/COMMON/Shirt", typeof(Material));
        MATsweater = Resources.LoadAll ("Materials/COMMON/Sweater", typeof(Material));
        MATlegs = Resources.LoadAll   ("Materials/COMMON/Legs", typeof(Material));
        MATfeetA = Resources.LoadAll  ("Materials/COMMON/FeetA", typeof(Material));
        MATfeetB = Resources.LoadAll  ("Materials/COMMON/FeetB", typeof(Material));
        MAThat = Resources.LoadAll    ("Materials/COMMON/HatA", typeof(Material));                
        MAThairA = Resources.LoadAll("Materials/BOY/hairs/HairA", typeof(Material));
        MAThairB = Resources.LoadAll("Materials/BOY/hairs/HairB", typeof(Material));
        MAThairC = Resources.LoadAll("Materials/BOY/hairs/HairC", typeof(Material));
        MAThairD = Resources.LoadAll("Materials/BOY/hairs/HairD", typeof(Material));
        MAThairE = Resources.LoadAll("Materials/BOY/hairs/HairE", typeof(Material));
        MAThairF = Resources.LoadAll("Materials/COMMON/Hair", typeof(Material));
        MATteeth = Resources.LoadAll("Materials/COMMON/Teeth", typeof(Material));

        if (GOfeet[0].activeSelf && GOfeet[1].activeSelf && GOfeet[2].activeSelf)
        {
            Resetskin(MATskins[0] as Material);
            Randomize();
        }
        else
        {
            for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) { if (GOhair[forAUX].activeSelf) hair = forAUX; }
            while (!GOchest[chest].activeSelf) chest++;
            if (chest == 0 || chest > 3) while (!GOlegs[legs].activeSelf) legs++;
            while (!GOfeet[feet].activeSelf) feet++;
            if (hair > 6) hatactive = true;
            Material[] AUXmaterials; int MATindex = 0;
            AUXmaterials = GOhead.GetComponent<Renderer>().sharedMaterials;
            while (AUXmaterials[skinindex].name != MATskins[MATindex].name) MATindex++;
            Resetskin(MATskins[MATindex] as Material);
        }
    }
    public void Identifymodel()
    {
        Object[] tempMATA = Resources.LoadAll("Materials/BOY/skins/TKBoyA", typeof(Material));
        Object[] tempMATB = Resources.LoadAll("Materials/BOY/skins/TKBoyB", typeof(Material));
        Object[] tempMATC = Resources.LoadAll("Materials/BOY/skins/TKBoyC", typeof(Material));
        string theskin = GOhead.GetComponent<Renderer>().sharedMaterials[0].name;
        for (int forAUX = 0; forAUX < tempMATA.Length; forAUX++)
        {
            if (theskin == tempMATA[forAUX].name)  model = "TKBoyA"; 
        }
        
        for (int forAUX = 0; forAUX < tempMATB.Length; forAUX++)
        {
            if (theskin == tempMATB[forAUX].name)  model = "TKBoyB";  
        }
        for (int forAUX = 0; forAUX < tempMATC.Length; forAUX++)
        {
            if (theskin == tempMATC[forAUX].name)  model = "TKBoyC";  
        }
        eyeindex = 1; skinindex = 0; teethindex = 2; 
    }
    public void Deactivateall()
    {
        for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) GOhair[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) GOchest[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++) GOlegs[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOfeet.Length; forAUX++) GOfeet[forAUX].SetActive(false);
        GOglasses.SetActive(false);
        glassesactive = false;
    }
    public void Activateall()
    {
        for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) GOhair[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) GOchest[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++) GOlegs[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOfeet.Length; forAUX++) GOfeet[forAUX].SetActive(true);        
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
    public void HatOn()
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
            hair = 0;
            GOhair[hair].SetActive(true);
            hatactive = false;
        }
    }

    //models
    public void Nexthair()
    {
        GOhair[hair].SetActive(false);
        if (hatactive) hair = 0;
        hatactive = false;
        if (hair < GOhair.Length - 2) hair++;
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
    public void Nextlegs()
    {
        GOlegs[legs].SetActive(false);
        if (legs < GOlegs.Length - 1) legs++;
        else legs = 0;
        GOlegs[legs].SetActive(true);
    }
    public void Prevlegs()
    {
        GOlegs[legs].SetActive(false);
        if (legs > 0) legs--;
        else legs = GOlegs.Length - 1;
        GOlegs[legs].SetActive(true);
    }
    public void Nextfeet()
    {
        GOfeet[feet].SetActive(false);
        if (feet < GOfeet.Length - 1) feet++;
        else feet = 0;
        GOfeet[feet].SetActive(true);
    }
    public void Prevfeet()
    {
        GOfeet[feet].SetActive(false);
        if (feet > 0) feet--;
        else feet = GOfeet.Length - 1;
        GOfeet[feet].SetActive(true);
    }

    //materials
    public void Nextskincolor(int todo)
    {
        //head
        SetGOmaterials(GOhead, MATskins, skinindex, todo);
        //chest
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++)
        {
            SetGOmaterials(GOchest[forAUX], MATskins, 0, todo);
        }
        //legs
        for (int forAUX = 3; forAUX < GOlegs.Length; forAUX++)
        {
            SetGOmaterials(GOlegs[forAUX], MATskins, 0, todo);
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
        if (hair == 6) SetGOmaterials(GOhair[6], MAThairA, 1, 0);
    }
    public void Nexthatcolor(int todo)
    {
        SetGOmaterials(GOhair[6], MAThat, 0, todo);        
    }
    public void Nextchestcolor(int todo)
    {
        if (chest < 2) Setmaterials(GOchest, MATshirt, 1, todo);   
        if (chest == 2) SetGOmaterials(GOchest[2], MATsweater, 1, todo);
        if (chest > 2) Setmaterials(GOchest, MATtshirt, 1, todo);
    }
    public void Nextlegscolor(int todo)
    {        
        if (legs < 3) Setmaterial(GOlegs, MATlegs, todo);
        if (legs > 2) Setmaterials(GOlegs, MATlegs, 1, todo);
    }
    public void Nextfeetcolor(int todo)
    {
        if (feet < 2) Setmaterial(GOfeet, MATfeetA,  todo);
        if (feet == 2) Setmaterial(GOfeet, MATfeetB,  todo);
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
        //chest          
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++)
        {
            AUXmaterials = GOchest[forAUX].GetComponent<Renderer>().sharedMaterials;
            AUXmaterials[0] = skinbase;
            GOchest[forAUX].GetComponent<Renderer>().sharedMaterials = AUXmaterials;
        }
        //legs
        for (int forAUX = 3; forAUX < GOlegs.Length; forAUX++)
        {
            AUXmaterials = GOlegs[forAUX].GetComponent<Renderer>().sharedMaterials;
            AUXmaterials[0] = skinbase;
            GOlegs[forAUX].GetComponent<Renderer>().sharedMaterials = AUXmaterials;
        }        
    }
    public void Randomize()
    {
        Deactivateall();
        //models
        hair = Random.Range(0, 7);
        GOhair[hair].SetActive(true);
        if (hair > 4) hatactive = true;else hatactive = false;
        chest = Random.Range(1, GOchest.Length); GOchest[chest].SetActive(true);
        legs = Random.Range(1, GOlegs.Length); GOlegs[legs].SetActive(true);
        feet = Random.Range(1, GOfeet.Length); GOfeet[feet].SetActive(true);
                
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
        for (int forAUX = 0; forAUX < (Random.Range(0, 13)); forAUX++) Nextfeetcolor(0);
        for (int forAUX = 0; forAUX < (Random.Range(0, 25)); forAUX++) Nextlegscolor(0);
        for (int forAUX = 0; forAUX < (Random.Range(0, 12)); forAUX++) Nexthatcolor(0);
        for (int forAUX = 0; forAUX < (Random.Range(0, 17)); forAUX++) Nextchestcolor(0);        
        for (int forAUX = 0; forAUX < (Random.Range(0, 4)); forAUX++) Nextskincolor(0);  
    }
    public void CreateCopy()
    {
        GameObject newcharacter = Instantiate(gameObject, transform.position, transform.rotation);
        for (int forAUX = 23; forAUX > 0; forAUX--)
        {
            if (!newcharacter.transform.GetChild(forAUX).gameObject.activeSelf) DestroyImmediate(newcharacter.transform.GetChild(forAUX).gameObject);
        }
        if (!GOglasses.activeSelf) DestroyImmediate(newcharacter.transform.Find("ROOT/TK/TK Pelvis/TK Spine/TK Spine1/TK Spine2/TK Neck/TK Head/Glasses").gameObject as GameObject);
        DestroyImmediate(newcharacter.GetComponent<BoyTKPrefabMaker>());
    }
    public void FIX()
    {
        GameObject newcharacter = Instantiate(gameObject, transform.position, transform.rotation);
        for (int forAUX = 23; forAUX > 0; forAUX--)
        {
            if (!newcharacter.transform.GetChild(forAUX).gameObject.activeSelf) DestroyImmediate(newcharacter.transform.GetChild(forAUX).gameObject);
        }
        if (!GOglasses.activeSelf) DestroyImmediate(newcharacter.transform.Find("ROOT/TK/TK Pelvis/TK Spine/TK Spine1/TK Spine2/TK Neck/TK Head/Glasses").gameObject as GameObject);
        DestroyImmediate(newcharacter.GetComponent<BoyTKPrefabMaker>());
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