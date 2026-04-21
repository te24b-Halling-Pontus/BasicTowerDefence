using Raylib_cs;

namespace CellInfo;

class CellInfoClass
{
    public int X;
    public int Y;
    public int CellNumber;
    public Color CellColor;
    public CellInfoClass(int x, int y, int cellNumber, Color cellColor)
    {
        this.X = x;
        this.Y = y;
        this.CellNumber = cellNumber;
        this.CellColor = cellColor;
    }
}