namespace SharedObjects
{
    public interface IAuthInfo
    {
        string Name { get; }
        string Token { get; }
    }

    public interface IResource
    { 
        public string Key { get; }
        public string Name { get; }
        public string Culture { get; }
        public string DisplayName { get; }
        public string ShortDisplayName { get; }
        public string Description { get; }
    }
}