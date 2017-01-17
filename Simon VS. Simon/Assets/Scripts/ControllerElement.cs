using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ControllerElement
{
    public string Name;

    private string Typ;
    private int ID;
    private int Pos = -1;

    public ControllerElement(string Typ)
    {
        Name = "TEST";
        this.Typ = Typ;
    }

    public ControllerElement(string type, int Position)
    {
        this.Typ = type;
        this.Pos = Position;
    }


    public ControllerElement(string type, int Position, int id)
    {
        this.Typ = type;
        this.Pos = Position;
        this.ID = id;
    }

    public string Type
    {
        get { return Typ; }
    }

    public int Position
    {
        get { return Pos; }
    }

    public int id
    {
        get { return ID; }
    }

    public override bool Equals(System.Object other)
    {
        if (other.GetType() != this.GetType()) return false;
        ControllerElement cur = (ControllerElement) other;

        return Typ.Equals(cur.Typ) && Pos == cur.Pos;
    }

    public override string ToString()
    {
        return Name;
    }
}
