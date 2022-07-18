using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public static class CollectionUtils
    {
        public static List<GameObject> GetChildren(this GameObject fatherObject)
        {
            return fatherObject.transform
                .Cast<Transform>()
                .Select(childTransform => childTransform.gameObject)
                .ToList();
        }
    }
}