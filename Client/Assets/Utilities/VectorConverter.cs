namespace Utilities
{
    public static class VectorConverter
    {
        public static UnityEngine.Vector3 ToUnityVector3(string vectorString)
        {
            var vectors = vectorString.Split(',');
            
            return new UnityEngine.Vector3(
                float.Parse(vectors[0]),
                float.Parse(vectors[1]),
                float.Parse(vectors[2]));
        }
        
        public static UnityEngine.Vector2 ToUnityVector2(string vectorString)
        {
            var vectors = vectorString.Split(',');
            
            return new UnityEngine.Vector2(
                float.Parse(vectors[0]),
                float.Parse(vectors[1]));
        }

        public static string ToString(UnityEngine.Vector3 unityVector)
        {
            return $"{unityVector.x},{unityVector.y},{unityVector.z}";
        }
    }
}
