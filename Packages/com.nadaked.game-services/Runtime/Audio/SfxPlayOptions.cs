namespace GameServices.GameServices.Runtime.Audio
{
    public readonly struct SfxPlayOptions
    {
        public static SfxPlayOptions Default => new(1f, 1f);

        public SfxPlayOptions(float pitch, float volumeScale = 1f)
        {
            Pitch = pitch;
            VolumeScale = volumeScale;
        }

        public float Pitch { get; }
        public float VolumeScale { get; }
    }
}
