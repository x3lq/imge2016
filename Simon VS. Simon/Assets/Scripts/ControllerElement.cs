using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ControllerElement
{
    public string Name;

    private string type;
    private int position = -1;

    public ControllerElement(string type)
    {
        Name = "TEST";
        this.type = type;
    }

    public ControllerElement(string type, int position)
    {
        this.type = type;
        this.position = position;
    }

    public string Type
    {
        get { return type; }
    }

    public int Position
    {
        get { return position; }
    }

    public override bool Equals(System.Object other)
    {
        if (other.GetType() != this.GetType()) return false;
        ControllerElement cur = (ControllerElement) other;

        return type.Equals(cur.type) && position == cur.position;
    }

    public override string ToString()
    {
        return Name;
    }
}
