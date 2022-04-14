using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Data
{
    [CreateAssetMenu(fileName = "Bool", menuName = "Data/Bool")]
    public class BoolContainer : DataContainer<bool, BoolContainer> { }
}