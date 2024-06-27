using Services;

namespace GameCore
{
    public class MusicSystem : AudioEmitter
    {
        public void Init()
        {
            if (hasSource && audioCue != null)
            {
                source.loop = true;
                PlayAudio(false);
            }
        }
    }
}