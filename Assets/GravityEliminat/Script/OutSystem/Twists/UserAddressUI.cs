using UnityEngine.UI;

public class UserAddressUI : UIBase
{
    public Button btnClose, btnSave;
    public InputField userName, phone, address;

    private void Start()
    {
        btnClose.onClick.AddListener(OnClose);
        btnSave.onClick.AddListener(OnSave);
        UpdateUI();
    }

    private void UpdateUI()
    {
        var adrs = TwistedData.Instance.data.address;
        if (adrs!=null)
        {
            userName.text = adrs.name;
            phone.text = adrs.phoneNum;
            address.text = adrs.address;
        }
    }

    private void OnSave()
    {

        if (userName.text==null||phone.text==null||address.text==null)
        {
            //ShowText("请填写完整信息。");
            return;
        }
        TwistedData.ConsigneeAddress userAddress = new TwistedData.ConsigneeAddress(userName.text, phone.text, address.text);
        TwistedData.Instance.data.address = userAddress;
        TwistedData.Instance.SaveData();
        OnClose();
    }

    private void OnClose()
    {
        UIManager.Instance.Hide<UserAddressUI>();
    }
}
