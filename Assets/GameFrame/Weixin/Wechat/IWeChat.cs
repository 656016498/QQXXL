

public interface IWeChat
{
    /// <summary>
    /// 登陆
    /// </summary>
    /// <param name="complete"></param>
    /// <param name="onfaild"></param>
    void Login(WeChat_AndroidHelps.Callback complete,WeChat_AndroidHelps.Callback onfaild);

    /// <summary>
    /// 是否登陆
    /// </summary>
    /// <param name="complete"></param>
    /// <param name="onfaild"></param>
    void IsLogin(WeChat_AndroidHelps.Callback complete,WeChat_AndroidHelps.Callback onfaild);

    /// <summary>
    /// 提交金币同步后台
    /// </summary>
    /// <param name="coin"></param>
    /// <param name="maxLv"></param>
    /// <param name="complete"></param>
    /// <param name="onfaild"></param>
    void PushCoins(int coin, int maxLv,int vedioN, WeChat_AndroidHelps.Callback complete, WeChat_AndroidHelps.Callback onfaild);
    /// <summary>
    /// 提现
    /// </summary>
    /// <param name="coin"></param>
    /// <param name="complete"></param>
    /// <param name="onfaild"></param>
    void Withdraw(string key, WeChat_AndroidHelps.Callback complete, WeChat_AndroidHelps.Callback onfaild);

    /// <summary>
    /// 提现历史
    /// </summary>
    /// <param name="complete"></param>
    /// <param name="onfaild"></param>
    void WithdrawHistory(WeChat_AndroidHelps.Callback complete, WeChat_AndroidHelps.Callback onfaild);

    /// <summary>
    /// 如果当前已登陆,那么可以直接获取用户信息
    /// </summary>
    /// <param name="complete"></param>
    /// <param name="onfaild"></param>
    void GetUserData(WeChat_AndroidHelps.Callback complete, WeChat_AndroidHelps.Callback onfaild);


    void CleanServerCoinData(WeChat_AndroidHelps.Callback complete, WeChat_AndroidHelps.Callback onfaild);

    void Share(WeChat_AndroidHelps.Callback complete, WeChat_AndroidHelps.Callback onfaild);
}
