using UnityEngine;

namespace Utilities
{
    public static class VectorConverter
    {
        public static Vector3 ToUnityVector3(string vectorString)
        {
            if (vectorString == null)
            {
                return new Vector3(0, 0, 0);
            }
            
            var vectors = vectorString.Split(',');
            
            return new Vector3(
                float.Parse(vectors[0]),
                float.Parse(vectors[1]),
                float.Parse(vectors[2]));
        }
        
        public static Vector2 ToUnityVector2(string vectorString)
        {
            if (vectorString == null)
            {
                return new Vector2(0, 0);
            }
            
            var vectors = vectorString.Split(',');
            
            return new Vector2(
                float.Parse(vectors[0]),
                float.Parse(vectors[1]));
        }
    }
}
