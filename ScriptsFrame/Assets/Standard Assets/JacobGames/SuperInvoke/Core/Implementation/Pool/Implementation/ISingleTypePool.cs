namespace JacobGames.SuperInvoke.Implementation.Pool {
    internal interface ISingleTypePool {
        object GetPooledInstance();
        void ReturnPooledInstance(object instance);
        void Clear();
    }
}