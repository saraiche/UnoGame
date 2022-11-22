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

namespace unoProyect
{
    /// <summary>
    /// Lógica de interacción para UserProfile.xaml
    /// </summary>
    public partial class UserProfile : Page
    {
        public string Username { get; set; }
        private Logic.CallDataService logic = new Logic.CallDataService();


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
    }
}
