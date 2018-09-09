namespace JacobGames.SuperInvoke.Implementation.Pool {
    internal class PoolNode {
        public object Value { get; set; }
        public PoolNode Next { get; set; }
    }
}