using System.ComponentModel;

public enum MoveDirections
{
    [Description("Вверх-влево")]
    TopLeft = 0,
    [Description("Вверх-вправо")]
    TopRight = 1,
    [Description("Вправо-вверх")]
    RightTop = 2,
    [Description("Вправо-вниз")]
    RightBottom = 3,
    [Description("Вниз-право")]
    BottomRight = 4,
    [Description("Вниз-влево")]
    BottomLeft = 5,
    [Description("Влево-вниз")]
    LeftBottom = 6,
    [Description("Влево-вверх")]
    LeftTop = 7
}