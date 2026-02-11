using System.Collections;
using System.Collections.Generic;


namespace FloorGame
{
    public class Tile 
    {

        public string NoteName { get; set; }
        public bool Correct { get; set; }

        public Tile(string name, bool safe) //constructor
        {
            NoteName = name;
            Correct = safe;
        }



    }
}
