//using UnityEngine;
//using UnityEngine.AddressableAssets;
//using Base;

//namespace Do.Scripts.Tools
//{
//    public class DialogManager : SingletonLate<DialogManager>
//    {
//        [SerializeField] private Canvas canvas;
//        [SerializeField] private AssetReference winDialog, loseDialog,reviveDialog;

//        [SerializeField]
//        private SettingPopup settingDialog;
//        private WinPopup _winDialog;
//        private LosePopup _loseDialog;
//        private RevivePopup _revivePopup;
//        public void ShowSetting(SettingPopup.SettingType type, Callback close = null, Callback backHome = null, Callback<bool> audioCallBack = null)
//        {
         
//            canvas.gameObject.SetActive(true);


        
//            settingDialog.Enable(type,delegate
//            {
//                canvas.gameObject.SetActive(false);
//                close?.Invoke();
//            }, backHome, audioCallBack);
//        }

//        public void ShowWin(Callback next = null, Callback backHome = null)
//        {
//            canvas.gameObject.SetActive(true);
//            canvas.renderMode = RenderMode.ScreenSpaceCamera;
//            canvas.worldCamera = Camera.main;
//            if (_winDialog == null)
//            {
//                BundledObjectLoader.Instance.LoadFromFileWithLoading(winDialog, dialog =>
//                {
//                    _winDialog = Instantiate(dialog, canvas.transform).GetComponent<WinPopup>();
//                    _winDialog.Enable(delegate
//                    {
//                        canvas.gameObject.SetActive(false);
//                    }, next, backHome);
//                });
//                return;
//            }
//            _winDialog.Enable(delegate
//            {
//                canvas.gameObject.SetActive(false);
//            }, next, backHome);
//        }
//        public void ShowLose(Callback replay = null, Callback backHome = null)
//        {
          
//            canvas.gameObject.SetActive(true);
//            if (_loseDialog == null)
//            {
//                BundledObjectLoader.Instance.LoadFromFileWithLoading(loseDialog, dialog =>
//                {
//                    _loseDialog = Instantiate(dialog, canvas.transform).GetComponent<LosePopup>();
//                    _loseDialog.Enable(delegate
//                    {
//                        canvas.gameObject.SetActive(false);
//                    }, replay, backHome);
//                });
//                return;
//            }
//            _loseDialog.Enable(delegate
//            {
//                canvas.gameObject.SetActive(false);
//            }, replay, backHome);
//        }

//        public void ShowRevive(Callback replay = null, Callback backHome = null)
//        {
//            canvas.gameObject.SetActive(true);
//            if (_revivePopup == null)
//            {
//                BundledObjectLoader.Instance.LoadFromFileWithLoading(reviveDialog, dialog =>
//                {
//                    _revivePopup = Instantiate(dialog, canvas.transform).GetComponent<RevivePopup>();
//                    _revivePopup.Enable(delegate
//                    {
//                        canvas.gameObject.SetActive(false);
//                    }, replay, backHome);
//                });
//                return;
//            }
//            _revivePopup.Enable(delegate
//            {
//                canvas.gameObject.SetActive(false);
//            }, replay, backHome);
//        }


//    }
//}
