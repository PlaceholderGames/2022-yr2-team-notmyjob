using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endGame : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] bool Wbishop;
    [SerializeField] bool Wrook;
    [SerializeField] bool Bknight;
    [SerializeField] bool Bpawn;

    void Start()
    {
        Wbishop = false;
        Wrook = false;
        Bknight = false;
        Bpawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        print($"bishop  {Wbishop}");
        print($"rook  {Wrook}");
        print($"knight  {Bknight}");
        print($"pawn  {Bpawn}");

        if (Wbishop && Wrook && Bknight && Bpawn)
        {
            Application.Quit();
        }
    }

    public void setWbishop()
    {
        Wbishop = true;
    }

    public void setWrook()
    {
        Wrook = true;
    }

    public void setBknight()
    {
        Bknight = true;
    }

    public void setBpawn()
    {
        Bpawn = true;
    }

}
