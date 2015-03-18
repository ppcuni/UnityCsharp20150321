using IteratorTasks;
using TaskInteraction;

namespace Login
{
    /// <summary>
    /// ログイン結果。
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// 登録済みかどうか。
        /// </summary>
        public bool IsRegistered;
        /// <summary>
        /// 利用規約文。nullなら利用規約同意済み扱い。
        /// </summary>
        public string UserPolicy;
    }

    /// <summary>
    /// ユーザー登録結果
    /// </summary>
    public class RegisterResponse
    {
        /// <summary>
        /// ユーザー名がNGワードに引っかかった。
        /// </summary>
        public bool IsInvalidUserName;
    }

    /// <summary>
    /// ログイン後にゲームで使うユーザー情報の取得結果
    /// </summary>
    public class GetUserDataResponse
    {
        /// <summary>
        /// ユーザー名。
        /// </summary>
        public string UserName;

        /// <summary>
        /// 所持金。
        /// </summary>
        public int Gold;

        // ...その他いろいろ
    }
}
