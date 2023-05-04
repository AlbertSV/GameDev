using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Checks
{
    public static class Extensions 
    {
        public static (int, int) ToCoordinate(this string value)
        {
            var x = value[0];
            var y = value[2];

            return (x, y);
        }

        public static (int, int) ToCoordinate(this (int x, int y) value)
        {
            return new (value.x, value.y);
        }

        public static string ToSerializable(this string value, ColorType checkColor, RecordType command, string destination = "")
        {

            var playerSide = checkColor == ColorType.Black ? "1" : "2";

            switch (command)
            {
                case RecordType.Click:
                    return $"Player {playerSide} {command} to {value}";

                case RecordType.Move:
                    return $"Player {playerSide} {command} from {value} to {destination}";

                case RecordType.Remove:
                    return $"Player {playerSide} {command} chip at {value}";

                default:
                    return "break";
                    
            }
        }
    }
}
