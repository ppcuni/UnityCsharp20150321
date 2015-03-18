using IteratorTasks;
using System;
using System.Collections;
using TaskInteraction;

namespace Login
{
    /// <summary>
    /// ログインAPIのインターフェイス。
    /// </summary>
    public interface ILoginApi
    {
        /// <summary>
        /// ログインする。
        /// 登録済みかどうか、利用規約同意済みかどうかを返す。
        /// </summary>
        /// <param name="deviceId">端末識別子。</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<LoginResponse> LoginAsync(string deviceId, CancellationToken ct);

        /// <summary>
        /// 利用規約に同意する。
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task AcceptUserPolicyAsync(CancellationToken ct);

        /// <summary>
        /// ユーザー登録する。
        /// 不正なユーザー名かどうかもチェック。
        /// </summary>
        /// <param name="userName">ユーザーが入力したユーザー名。</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<RegisterResponse> RegisterAsync(string userName, CancellationToken ct);

        /// <summary>
        /// ユーザーデータを取得。登録済みユーザーしか呼べない想定。
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<GetUserDataResponse> GetUserDataAsync(CancellationToken ct);
    }

    /// <summary>
    /// ログイン処理のAPI(ダミー)
    /// 本当のアプリ作る場合はここで通信する。
    /// </summary>
    public class LoginApiDummy : ILoginApi
    {
        /// <summary>
        /// 登録済みかどうか。
        /// </summary>
        private bool IsRegistered { get { return _userName != null; } }

        /// <summary>
        /// 利用規約文。
        /// </summary>
        private const string UserPolicy = "とてもすごくかわいい規約に同意しますか？";

        /// <summary>
        /// 登録したユーザー名。
        /// </summary>
        private string _userName = null;

        /// <summary>
        /// 所持ゴールド。
        /// </summary>
        private const int _gold = 9999;

        public Task<LoginResponse> LoginAsync(string deviceId, CancellationToken ct)
        {
            return Task.Run<LoginResponse>(c => LoginIterator(c, deviceId, ct));
        }

        private IEnumerator LoginIterator(Action<LoginResponse> callback, string deviceId, CancellationToken ct)
        {
            // 通信遅延エミュレート。
            yield return Task.Delay(1000, ct);

            if (IsRegistered)
                callback(new LoginResponse { IsRegistered = true });
            else
                callback(new LoginResponse { UserPolicy = UserPolicy });
        }

        public Task AcceptUserPolicyAsync(CancellationToken ct)
        {
            // 通信遅延エミュレート。
            return Task.Delay(1000, ct);
        }

        public Task<RegisterResponse> RegisterAsync(string userName, CancellationToken ct)
        {
            return Task.Run<RegisterResponse>(c => RegisterIterator(c, userName, ct));
        }

        private IEnumerator RegisterIterator(Action<RegisterResponse> callback, string userName, CancellationToken ct)
        {
            // 通信遅延エミュレート。
            yield return Task.Delay(1000, ct);

            if (userName == "NG")
                callback(new RegisterResponse { IsInvalidUserName = true });
            else
            {
                // 登録済みにする。
                _userName = userName;
                callback(new RegisterResponse());
            }
        }

        public Task<GetUserDataResponse> GetUserDataAsync(CancellationToken ct)
        {
            return Task.Delay(1000, ct).OnSuccess(() => new GetUserDataResponse { UserName = _userName, Gold = _gold });
        }
    }
}
