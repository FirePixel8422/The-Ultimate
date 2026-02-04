



    [System.Serializable]
    public struct StatusEffectInstance
    {
        public StatusEffectType Type;
        public int Duration;

        public StatusEffectInstance(StatusEffectType type, int duration)
        {
            Type = type;
            Duration = duration;
        }
    }