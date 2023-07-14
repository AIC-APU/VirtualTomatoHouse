namespace Plusplus.VirtualTomatoHouse.Scripts.Model
{
    public class AnnotationPair
    {
        private readonly ColorTexture _color;
        private readonly TagTexture _tag;
        private readonly int _id;

        public ColorTexture Color => _color;
        public TagTexture Tag => _tag;
        public int ID => _id;

        public AnnotationPair(ColorTexture color, TagTexture tag, int id)
        {
            _color = color;
            _tag = tag;
            _id = id;
        }
    }
}