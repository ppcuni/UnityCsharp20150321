namespace Login.Progress
{
    /// <summary>
    /// ログイン完了。
    /// </summary>
    public class LoggedIn : LoginProgressBase
    {
        /// <summary>
        /// ユーザー情報。
        /// </summary>
        public GetUserDataResponse UserData { get; private set; }

        internal LoggedIn(GetUserDataResponse userData)
        {
            UserData = userData;
        }
    }
}
