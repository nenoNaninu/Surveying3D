using Utf8Json;

namespace Surveying3D
{
    public readonly struct SurveyResults
    {
        public readonly float Height;
        public readonly float Width;
        public readonly float Depth;
        public readonly float Volume;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="height">y axis(up)</param>
        /// <param name="width">x axis(horizontal)</param>
        /// <param name="depth">z axis(forward)</param>
        /// <param name="volume"></param>
        [SerializationConstructor]
        public SurveyResults(float height, float width,float depth, float volume)
        {
            Height = height;
            Width = width;
            Depth = depth;
            Volume = volume;
        }
    }
}