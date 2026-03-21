using Raylib_cs;

namespace CellInfo;

class CellInfoClass
{
    public int X;
    public int Y;
    public int cellNumber;
    public Color CellColor;
    public CellInfoClass(int X, int Y, int cellNumber, Color CellColor)
    {
        this.X = X;
        this.Y = Y;
        this.cellNumber = cellNumber;
        this.CellColor = CellColor;
    }
}