using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastChessEngine
{
    partial class ThinkerPro
    {
        //now situations
        const  int totalsituationfeatures = 6;
        int[] situations;
        public void setsituations()
        {
            //assign pieces
            situations = new int[totalnumberassign * 1 * totalsituationfeatures];
            //boardindex,parentindex,eval
        }
    }
}
