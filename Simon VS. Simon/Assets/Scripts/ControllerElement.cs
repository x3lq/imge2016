using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ControllerElement
{
    public string Name;

    private string Typ;
    public int ID;
    private int Pos = -1;

    public ControllerElement(string Typ)
    {
        Name = "Button";
        this.Typ = Typ;

        this.Typ = "Button";
    }

    public ControllerElement(string type, int Position)
    {
        this.Typ = "Button";
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

        GameHandler.log("Other: " + cur.Typ + " " + cur.Pos + " " + cur.ID);
        GameHandler.log("This: " + Typ + " " + Pos + " " + ID);

        return Typ.Equals(cur.Typ) && Pos == cur.Pos && ID == cur.ID;
    }

    public override string ToString()
    {
        return Name;
    }
}
