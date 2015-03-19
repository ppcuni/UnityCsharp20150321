using TaskInteraction;

namespace Login.Progress
{
    /// <summary>
    /// ユーザーポリシー同意待ち
    /// </summary>
    public class UserPolicyConfirm : LoginProgressBase, IResponsiveMessage<bool>
    {
        /// <summary>
        /// 利用規約文。
        /// </summary>
        public string Policy { get; private set; }

        internal UserPolicyConfirm(string policy)
        {
            Policy = policy;
        }

        /// <summary>
        /// 同意したかどうか。
        /// View側の返答を入れておく。
        /// </summary>
        public bool Response { get; set; }
        object IResponsiveMessage.Response { get { return Response; } set { Response = (bool)value; } }
    }
}
