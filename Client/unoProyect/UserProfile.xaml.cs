using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using unoProyect.Proxy;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using unoProyect.Security;
using unoProyect.Logic;

namespace unoProyect
{
    /// <summary>
    /// Lógica de interacción para UserProfile.xaml
    /// </summary>
    public partial class UserProfile : Page
    {
        public string Username { get; set; }
        private DTOPlayer PlayerData { get; set; }
        private Logic.CallDataService logic = new Logic.CallDataService();
        public int NumImage { get; set; }
        public bool IsChangeBtnPress { get; set; }
        private const int SUCCESFUL = 1;

        public UserProfile()
        {
            InitializeComponent();
        }
        public UserProfile(string username): this()
        {
            this.Username = username;
            List<string> friends = logic.GetFriends(username);
            if (friends != null)
            {
                foreach (string friend in friends)
                {
                    LbFriendList.Items.Add(friend);
                }
            }
            PlayerData = logic.GetPlayer(username);
            ImPlayer.Source = new BitmapImage(new Uri(PlayerData.Image, UriKind.Relative));
            BtnSave.IsEnabled = false;
            NumImage = 3;
            MaterialDesignThemes.Wpf.HintAssist.SetHint(TbUsername, username);
            if (PlayerData != null)
            {
                MaterialDesignThemes.Wpf.HintAssist.SetHint(TbEmail, PlayerData.Credentials.Email);
            }
            IsChangeBtnPress = false;
            BtnDeleteFriend.IsEnabled = false;

        }

        private void BtnAddFriend_Click(object sender, RoutedEventArgs e)
        {
            if(TbAddFriend.Text.Trim() == string.Empty)
            {
                MessageBox.Show(Properties.Resources.notEmptyFields);
            }
            else
            {
                if (logic.SearchUser(TbAddFriend.Text.Trim()) == SUCCESFUL)
                {
                    if(logic.AddFriend(Username, TbAddFriend.Text.Trim()))
                    {
                        MessageBox.Show(Properties.Resources.ok);
                        LbFriendList.Items.Clear();
                        List<string> friends = logic.GetFriends(Username);
                        foreach (string friend in friends)
                        {
                            LbFriendList.Items.Add(friend);
                        }
                    }
                    else
                    {
                        MessageBox.Show(Properties.Resources.no);
                    }
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            int result = 0;
            bool resultInputUserValid = true;
            if (IsChangeBtnPress)
            {
                PlayerData.Image = "GraphicResources/playerImages/Recurso " + NumImage + ".png";
            }
            if(!(TbUsername.Text.Trim() == String.Empty))
            {
                PlayerData.Credentials.Username = TbUsername.Text;
            }
            if (!(TbEmail.Text.Trim() == String.Empty))
            {
                if (Utilities.ValidateEmail(TbEmail.Text))
                {
                    PlayerData.Credentials.Email = TbEmail.Text;
                }
                else
                {
                    resultInputUserValid = false;
                }
            }
            if(!(TbPassword.Text.Trim() == String.Empty))
            {
                if (Utilities.ValidatePassword(TbPassword.Text))
                {
                    PlayerData.Credentials.Password = Utilities.ComputeSHA256Hash(TbPassword.Text);
                }
                else
                {
                    resultInputUserValid = false;
                }
            }
            if (resultInputUserValid)
            {
                result = logic.SetPlayer(PlayerData, Username);
                switch (result)
                {
                    case 1:
                        MessageBox.Show(Properties.Resources.informationSuccesfullSignUp, "");
                        PlayerData = logic.GetPlayer(PlayerData.Credentials.Username);
                        if(PlayerData != null)
                        {
                            Username = PlayerData.Credentials.Username;
                        }
                        break;
                    case 2:
                        MessageBox.Show(Properties.Resources.informationUsernameDuplicate, "");
                        break;
                }
            }
            else
            {
                MessageBox.Show(Properties.Resources.invalidPasswordOrEmail, "");
            }


        }

        private void BtnChange_Click(object sender, RoutedEventArgs e)
        {
            BtnSave.IsEnabled = true;
            IsChangeBtnPress = true;

            if(NumImage == 44)
            {
                NumImage = 3;
            }
            else
            {
                NumImage++;
                ImPlayer.Source = new BitmapImage(new Uri(@"GraphicResources/playerImages/Recurso " + NumImage + ".png", UriKind.Relative));
            }

        }

        

        private void TbPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            BtnSave.IsEnabled = true;
        }

        private void TbEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            BtnSave.IsEnabled = true;
            

        }

        private void TbUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            BtnSave.IsEnabled = true;
        }

        private void LbFriendList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnDeleteFriend.IsEnabled = true;
        }

        private void BtnDeleteFriend_Click(object sender, RoutedEventArgs e)
        {
            if (logic.DeleteFriend(Username, LbFriendList.SelectedItem.ToString()))
            {
                LbFriendList.Items.Clear();
                List<string> friends = logic.GetFriends(Username);
                foreach (string friend in friends)
                {
                    LbFriendList.Items.Add(friend);
                }
            }
        }
    }
}
