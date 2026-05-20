using Raylib_cs;

namespace CellInfo;

class CellInfoClass // har bara alla clers information
{
    public int X;
    public int Y;
    public int CellNumber;
    public Color CellColor;
    public CellInfoClass(int x, int y, int cellNumber, Color cellColor)// ändar variablerna till de som säts in när den instansieras
    {
        this.X = x;
        this.Y = y;
        this.CellNumber = cellNumber;
        this.CellColor = cellColor;
    }
}