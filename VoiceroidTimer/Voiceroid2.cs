using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codeer.Friendly.Windows;
using Codeer.Friendly.Windows.Grasp;
using RM.Friendly.WPFStandardControls;

namespace VoiceroidTimer
{
    class Voiceroid2
    {
        WPFTabControl VoicePresetTab = null;
        WPFTabControl TuneTab = null;
        WPFListView AvatorListView_std = null;
        WPFTextBox talk_text_box = null;
        WPFButtonBase play_button = null;
        public bool init()
        {
            //初期設定 ソフトの定義
            Process[] voiceroid_process = Process.GetProcessesByName("VoiceroidEditor");
            if (voiceroid_process.Length == 0)
            {
                Console.WriteLine("起動していない");
                return false;
            }
            Process process = voiceroid_process[0];

            WindowsAppFriend app = new WindowsAppFriend(process);

            WindowControl ui_tree_top = WindowControl.FromZTop(app);
            var text_edit_view = ui_tree_top.GetFromTypeFullName("AI.Talk.Editor.TextEditView")[0].LogicalTree();
            //判明しているGUI要素特定
            var tabs = ui_tree_top.GetFromTypeFullName("AI.Framework.Wpf.Controls.TitledTabControl");
            VoicePresetTab = new WPFTabControl(tabs[0]);  // ボイス（プリセット）のタブコントロール
            TuneTab = new WPFTabControl(tabs[1]);  // チューニングのタブコントロール
            talk_text_box = new WPFTextBox(text_edit_view[4]);
            play_button = new WPFButtonBase(text_edit_view[6]);
            TuneTab.EmulateChangeSelectedIndex(1);// ボイスタブ
            VoicePresetTab.EmulateChangeSelectedIndex(0);
            AvatorListView_std = new WPFListView(ui_tree_top.GetFromTypeFullName("System.Windows.Controls.ListView")[0]);
            return true;

        }
        public void say(string word)
        {
            //発言を行うメソッド
            talk_text_box.EmulateChangeText(word);
            play_button.EmulateClick();
        }
        public void setCharacter(int avatorIdx)
        {
            AvatorListView_std.EmulateChangeSelectedIndex(avatorIdx); //キャラ変更
        }

        public string[] getCharacter()
        {
            var list = new List<string>();
            int selectIdx = AvatorListView_std.SelectedIndex;
            for(int avatorIdx = 0;avatorIdx < AvatorListView_std.ItemCount; avatorIdx++)
            {
                AvatorListView_std.EmulateChangeSelectedIndex(avatorIdx); //キャラ変更
                //プリセット名取得（話者名）
                var params1 = TuneTab.VisualTree(TreeRunDirection.Descendants).ByType("AI.Framework.Wpf.Controls.TextBoxEx")[0];
                WPFTextBox nameTextBox = new WPFTextBox(params1);
                list.Add(nameTextBox.Text);
            }
            AvatorListView_std.EmulateChangeSelectedIndex(selectIdx); //キャラ変更
            string[] array = list.ToArray();
            return array;
        }
    }
}
