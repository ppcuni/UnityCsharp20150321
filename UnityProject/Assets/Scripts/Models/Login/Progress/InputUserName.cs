using TaskInteraction;

namespace Login.Progress
{
    /// <summary>
    /// ユーザー名入力待ち
    /// </summary>
    public class InputUserName : LoginProgressBase, IResponsiveMessage<string>
    {
        /// <summary>
        /// ユーザー名。
        /// View側の返答を入れておく。
        /// </summary>
        public string Response { get; set; }
        object IResponsiveMessage.Response { get { return Response; } set { Response = (string)value; } }
    }
}
