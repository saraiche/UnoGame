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


        public UserProfile()
        {
            InitializeComponent();
        }
        public UserProfile(string username): this()
        {
            this.Username = username;
            List<string> friends = logic.GetFriends(username);
            foreach(string friend in friends)
            {
                LbFriendList.Items.Add(friend);
            }
            PlayerData = logic.GetPlayer(username);
            ImPlayer.Source = new BitmapImage(new Uri(PlayerData.Image, UriKind.Relative));
            BtnSaveImage.IsEnabled = false;
            NumImage = 3;
            LblUsername.Content = username;
        }

        private void BtnAddFriend_Click(object sender, RoutedEventArgs e)
        {
            if(TbAddFriend.Text.Trim() == string.Empty)
            {
                MessageBox.Show(Properties.Resources.notEmptyFields);
            }
            else
            {
                if (logic.SearchUser(TbAddFriend.Text.Trim()))
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

        private void BtnSaveImage_Click(object sender, RoutedEventArgs e)
        {
            PlayerData.Image = "GraphicResources/playerImages/Recurso " + NumImage + ".png";
            logic.SetPlayer(PlayerData,Username);

        }

        private void BtnChange_Click(object sender, RoutedEventArgs e)
        {
            BtnSaveImage.IsEnabled = true;
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

       
    }
}
