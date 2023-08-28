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
        
        public static Quaternion ToUnityQuaternion(string quaternionString)
        {
            if (quaternionString == null)
            {
                return new Quaternion(0, 0, 0, 0);
            }
            
            var quaternions = quaternionString.Split(',');
            
            return new Quaternion(
                float.Parse(quaternions[0]),
                float.Parse(quaternions[1]),
                float.Parse(quaternions[2]),
                float.Parse(quaternions[3]));
        }
    }
}
