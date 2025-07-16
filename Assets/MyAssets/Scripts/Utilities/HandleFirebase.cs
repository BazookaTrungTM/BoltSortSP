//using Firebase;
//using Firebase.Analytics;
//using Firebase.Extensions;
//using UnityEngine;

//namespace Plugins
//{
//    public class HandleFirebase : MonoBehaviour
//    {
//        #region Start_Game Variables
//        public const string Current_level_start_game = "Current_level_start_game";
//        public const string Internet_status_start_game = "Internet_status_start_game";
//        public const string Iap_count_start_game = "Iap_count_start_game";
//        public const string Win_streak_start_game = "Win_streak_start_game";
//        public const string Balance_Heart_start_game = "Balance_Heart_start_game";
//        public const string Balance_Coin_start_game = "Balance_Coin_start_game";
//        public const string Balance_Undo_start_game = "Balance_Undo_start_game";
//        public const string Balance_SpecialNuts_start_game = "Balance_Fill_start_game";
//        public const string Balance_ExtraHolder_start_game = "Balance_Extra_Holder_start_game";

//        #endregion

//        #region Level_Start Variables
//        public const string Current_level_start_level = "Current_level_start_level";
//        public const string Play_Type_start_level = "Play_Type_start_level";
//        public const string Play_Index_start_level = "Play_Index_start_level";
//        public const string Lose_Index_start_level = "Lose_Index_start_level";

//        #endregion

//        #region Level_End Variables
//        public const string Current_level_end_level = "Current_level_end_level";
//        public const string Total_Keep_Playing_end_level = "Total_Keep_Playing_end_level";
//        public const string Total_Give_Up_end_level = "Total_Give_Up_end_level";
//        public const string Play_Type_end_level = "Play_Type_end_level";
//        public const string Time_duration_end_level = "Time_duration_end_level";
//        public const string Play_Index_end_level = "Play_Index_end_level";
//        public const string Total_Nut_end_level = "Total_Nut_end_level";
//        public const string Cleared_Nut_end_level = "Cleared_Nut_end_level";
//        public const string Total_Bolt_end_level = "Total_Bolt_end_level";
//        public const string Max_Bolt_end_level = "Max_Bolt_end_level";
//        public const string Total_Undo_end_level = "Total_Undo_end_level";
//        public const string Total_SpecialNuts_end_level = "Total_SpecialNuts_end_level";
//        public const string Total_ExtraHolder_end_level = "Total_ExtraHolder_end_level";
//        public const string Action_Sequence_end_level = "Action_Sequence_end_level";
//        public const string Level_Result_end_level = "Level_Result_end_level";
//        public const string Lose_By_end_level = "Lose_By_end_level";

//        #endregion

//        #region Earn_Resources Variables
//        public const string Earn_Resources_Type = "Earn_Resources_Type";
//        public const string Earn_Resources_Name = "Earn_Resources_Name";
//        public const string Earn_Resources_Amount = "Earn_Resources_Amount";
//        public const string Earn_Resources_Reason = "Earn_Resources_Reason";
//        public const string Earn_Resources_Position = "Earn_Resources_Position";

//        #endregion

//        #region Spend_Resources Variables
//        public const string Spend_Resources_Type = "Spend_Resources_Type";
//        public const string Spend_Resources_Name = "Spend_Resources_Name";
//        public const string Spend_Resources_Amount = "Spend_Resources_Amount";
//        public const string Spend_Resources_Reason = "Spend_Resources_Reason";
//        public const string Spend_Resources_Position = "Spend_Resources_Position";

//        #endregion

//        #region IAP Variables
//        public const string IAP_Show_Placement = "IAP_Show_Placement";
//        public const string IAP_Show_Type = "IAP_Show_Type";
//        public const string IAP_Show_Pack_Name = "IAP_Show_Pack_Name";
//        public const string IAP_Click_Placement = "IAP_Click_Placement";
//        public const string IAP_Click_Type = "IAP_Click_Type";
//        public const string IAP_Click_Pack_Name = "IAP_Click_Pack_Name";
//        public const string IAP_Purchase_Placement = "IAP_Purchase_Placement";
//        public const string IAP_Purchase_Type = "IAP_Purchase_Type";
//        public const string IAP_Purchase_Pack_Name = "IAP_Purchase_Pack_Name";
//        public const string IAP_Purchase_Price = "IAP_Purchase_Price";
//        public const string IAP_Purchase_Currency = "IAP_Purchase_Currency";


//        #endregion

//        #region Ads Variables
//        public const string Ads_Request_Format = "Ads_Request_Format";
//        public const string Ads_Request_Platform = "Ads_Request_Platform";
//        public const string Ads_Request_Network = "Ads_Request_Network";
//        public const string Ads_Request_Placement = "Ads_Request_Placement";
//        public const string Ads_Request_Is_Load = "Ads_Request_Is_Load";
//        public const string Ads_Request_Load_Time = "Ads_Request_Load_Time";
//        public const string Ads_Show_Format = "Ads_Show_Format";
//        public const string Ads_Show_Platform = "Ads_Show_Platform";
//        public const string Ads_Show_Network = "Ads_Show_Network";
//        public const string Ads_Show_Placement = "Ads_Show_Placement";
//        public const string Ads_Show_Is_Show = "Ads_Show_Is_Show";
//        public const string Ads_Show_Value = "Ads_Show_Value";
//        public const string Ads_Show_Currency = "Ads_Show_Currency";
//        public const string Ads_Complete_Format = "Ads_Complete_Format";
//        public const string Ads_Complete_Platform = "Ads_Complete_Platform";
//        public const string Ads_Complete_Network = "Ads_Complete_Network";
//        public const string Ads_Complete_End_Type = "Ads_Complete_End_Type";
//        public const string Ads_Complete_Ad_Duration = "Ads_Complete_Ad_Duration";
//        public const string Ads_Complete_Placement = "Ads_Complete_Placement";

//        #endregion

//        #region Others Variables
//        public const string Timeplay_Minute = "Timeplay_Minute";
//        public const string Session_Start_Count = "Session_Start_Count";
//        public const string Session_Start_Time_Per_Session = "Session_Start_Time_Per_Session";
//        public const string Sreen_Reach = "Sreen_Reach";


//        #endregion

//        public const string RESTORE_REMOVE_ADS = "RESTORE_REMOVE_ADS";

//        Coroutine _coroutine;
//        DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
//        public static HandleFirebase Instance =null;

//        void Awake()
//        {
//            if (Instance == null)
//            {
//                DontDestroyOnLoad(gameObject);
//                Instance = this;
//            }
//            else if (Instance != this)
//            {
//                Destroy(gameObject);
//            }
//        }
//        private void Start()
//        {
//            InitFirebase();
//        }
//        private void  InitFirebase()
//        {

//            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
//            {
//                dependencyStatus = task.Result;
//                Debug.Log("FIREBASE STATUS: " + dependencyStatus.ToString());
//                if (dependencyStatus == DependencyStatus.Available)
//                {
//                    FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
//                    // Set the user's sign up method.
//                    FirebaseAnalytics.SetUserProperty(FirebaseAnalytics.UserPropertySignUpMethod, "Google");
//                    //Set the user ID.
//                }
//                else
//                {
//                    // Debug.LogError(
//                        "Could not resolve all Firebase dependencies: " + dependencyStatus);
//                }
//            });
//        }
//        public void LogEventWithString(string eventName, string child = "")
//        {
//            // var testAb = AdsManager.Instance.AB_PlayOn_Value == 1 ? "A" : "B";
//            // var tmp = GameData.NewInstall ? $"V{RootManager.Instance.version}_{testAb}" : string.Empty;
//            var tmp = $"V{RootManager.Instance.version}";
//            //eventName += $"{tmp}";
//            if (child != "") eventName += $"_{child}";
//            tmp += $"_{eventName.ToUpper()}";
//            FirebaseAnalytics.LogEvent(tmp);
//            //// Debug.LogError("FIRE BASE: " + tmp);
//        }
//        public void LogEventADjust(string eventName, string child = "")
//        {
//            // var testAb = AdsManager.Instance.AB_PlayOn_Value == 1 ? "A" : "B";
//            // var tmp = GameData.NewInstall ? $"V{RootManager.Instance.version}_{testAb}" : string.Empty;
//            var tmp = $"V{RootManager.Instance.version}";
//            //eventName += $"{tmp}";
//            if (child != "") eventName += $"_{child}";
//            tmp += $"_{eventName.ToUpper()}";
//            FirebaseAnalytics.LogEvent(tmp);
//            // Debug.LogError("FIRE BASE: " + tmp);
//        }

//        public void LogEventWithoutVersion(string eventName)
//        {
//            FirebaseAnalytics.LogEvent(eventName);
//        }

//        private void OnApplicationQuit()
//        {
//            if (FirebaseApp.DefaultInstance != null)
//            {
//                FirebaseApp.DefaultInstance.Dispose();
//            }
//        }





//    }
//}

