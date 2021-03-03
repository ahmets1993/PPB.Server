using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using PPB.BL.UnitOfWork;
using PPB.Client.Models;
using PPB.DAL.Concrete;
using Newtonsoft.Json;
using NAudio.Wave;
using System.Diagnostics;
using System.Reflection;

namespace PPB.Client
{
    public class Client
    {
        static UnitOfWork _unitOfWork = new UnitOfWork();


        static void Main(string[] args)
        {
            Random rnd = new Random();
            Console.WriteLine("WELCOME TO LOL CARD GAME \n1-LOGIN\n2-REGISTER\n3-UPDATE ACCAUNT\n4-DELETE ACCUNT\n5-SHOW CHAMPION POINTS\n6-EXIT");
            string selected = Console.ReadLine().Split("-")[0];

            switch (selected)
            {
                case "1":
                    Console.WriteLine("Username:");
                    string userName = Console.ReadLine();
                    Console.WriteLine("Password:");
                    string password = Console.ReadLine();
                    string url;
                    UserAuthentication authentication;
                    LoginUser(rnd, userName, password, out url, out authentication);
                    if(_unitOfWork.Users.Any(x=>x.UserName==userName && x.isAdmin))  partyroom(userName);
                    else { Console.WriteLine("Enjoy your party.");
                        Console.ReadLine();
                    }
                    break;
                case "2":
                    try
                    {
                        Console.WriteLine("NameSurname:");
                        string NameSurname = Console.ReadLine();
                        Console.WriteLine("Username:");
                        userName = Console.ReadLine();
                        Console.WriteLine("Password:");
                        password = Console.ReadLine();
                        url = string.Format("http://localhost:5000/create-user?username=" + userName + "&password=" + password + "&NameSurname=" + NameSurname);
                        string resp = PPB.Helper.RestAPI.CallRestMethod(url, "POST");
                        Console.WriteLine(resp);
                        while (resp != "Registration Successful")
                        {
                            Console.WriteLine("Please try again");
                            Console.WriteLine("Re Username:");
                            userName = Console.ReadLine();
                            Console.WriteLine("Re Password:");
                            password = Console.ReadLine();
                            url = string.Format("http://localhost:5000/create-user?username=" + userName + "&password=" + password + "&NameSurname=" + NameSurname);
                            if (PPB.Helper.RestAPI.CallRestMethod(url, "POST") == "Registration Successful")
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Registration Successful");

                                Console.ForegroundColor = ConsoleColor.White;
                                Console.BackgroundColor = ConsoleColor.Black;
                                LoginUser(rnd, userName, password, out url, out authentication);

                            }
                        }
                        Console.WriteLine(resp);
                        Console.WriteLine("Game loading..");
                        Thread.Sleep(2000);
                        LoginUser(rnd, userName, password, out url, out authentication);
                     



                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine("Exeption Code: " + exp);
                    }
                    break;
                case "3":
                    Console.WriteLine("Username:");
                    userName = Console.ReadLine();
                    Console.WriteLine("Password:");
                    password = Console.ReadLine();
                    Console.WriteLine("New Username:");
                    string NewUserName = Console.ReadLine();
                    Console.WriteLine("New Password:");
                    string Newpassword = Console.ReadLine();
                    url = string.Format("http://localhost:5000/update-user?username=" + userName + "&password=" + password + "&newusername=" + NewUserName + "&newpassword=" + Newpassword + "");
                    authentication = JsonConvert.DeserializeObject<UserAuthentication>(PPB.Helper.RestAPI.CallRestMethod(url, "PUT"));
                    break;
                case "4":
                    try
                    {
                        Console.WriteLine("Username:");
                        userName = Console.ReadLine();
                        Console.WriteLine("Password:");
                        password = Console.ReadLine();
                        url = string.Format("http://localhost:5000/delete-user?username=" + userName + "&password=" + password + "");
                        PPB.Helper.RestAPI.CallRestMethod(url, "POST");
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine("Exeption Code: " + exp);
                    }
                    break;
                case "5":
                    var champList = _unitOfWork.Users.GetList().OrderByDescending(x => x.Coin);
                    int i = 1;
                    foreach (var item in champList)
                    {
                        Console.WriteLine(i + "." + item.UserName + "  ==>  " + item.Coin);
                        i++;
                    }
                    break;
                case "6":



                    break;
            }
        }

        private static void LoginUser(Random rnd, string userName, string password, out string url, out UserAuthentication authentication)
        {
            url = string.Format("http://localhost:5000/login-user?username=" + userName + "&password=" + password + "");
            authentication = JsonConvert.DeserializeObject<UserAuthentication>(PPB.Helper.RestAPI.CallRestMethod(url, "POST"));
            Console.WriteLine(authentication.Response);
            if (authentication.Auth)
            {

                int coinLimit = _unitOfWork.Users.FirstOrDefault(x => x.UserName == userName).Coin;
                Console.WriteLine("---------------------------**********-----------------------");
                Console.WriteLine("------------------------WELCOME " + userName + "----------------");
                Console.WriteLine("Your Coin:" + coinLimit);
                Console.WriteLine("Game Menu");
                Console.WriteLine("1-Create Game Room\n2-Login Room");
                string RoomConnectType = Console.ReadLine();
                if (RoomConnectType == "1")
                {
                    _unitOfWork.PlayerLobby.RemoveRange(_unitOfWork.PlayerLobby.Where(x => x.UserName == userName));
                    _unitOfWork.BattleLogs.RemoveRange(_unitOfWork.BattleLogs.Where(x => x.UserName == userName));

                    _unitOfWork.Save();
                    var room = userName + "-" + rnd.Next(0, 1000).ToString();


                    _unitOfWork.PlayerLobby.Insert(new PlayerLobby
                    {
                        LobbyNo = room,
                        Score = 100,
                        UserName = userName

                    });

                    _unitOfWork.Save();

                    Console.WriteLine("Game room is ready.Please wait other player.");
                    int i = 15;
                    while (i != 0)
                    {

                        Console.Clear();
                        Console.WriteLine("Your game room is ready.Room Code: " + userName + "-" + room);
                        Console.WriteLine("Game will be started in " + (i - 1) + "second.");
                        Thread.Sleep(1000);
                        i--;
                        if (i == 0)
                        {
                            var lobby = _unitOfWork.PlayerLobby.FirstOrDefault(x => x.LobbyNo == room);
                            lobby.Status = true;
                            _unitOfWork.PlayerLobby.Update(lobby);
                            _unitOfWork.Save();
                            break;
                        }

                    }



                    string[] Player1Moves = { "", "", "", "", "" };
                    Console.WriteLine("1.Rock = R\n2.Paper = P\n3.Scissors = S\n4.Lizard = L\n5.Spock = V");
                    for (int j = 1; j < 6; j++)
                    {
                        Console.WriteLine("Enter your " + j + ". move:");
                        string playermoves = Console.ReadLine();
                        _unitOfWork.BattleLogs.Insert(new BattleLogs
                        {
                            RoundNo = j,
                            LobbyNo = room,
                            Score = 100,
                            UserName = userName,
                            roundScore = 10,
                            PlayerMove = playermoves

                        });

                        _unitOfWork.Save();


                    }

                    GetPoints(userName, room);

                    Console.ReadLine();


                }
                else if (RoomConnectType == "2")
                {
                    _unitOfWork.PlayerLobby.RemoveRange(_unitOfWork.PlayerLobby.Where(x => x.UserName == userName));
                    _unitOfWork.BattleLogs.RemoveRange(_unitOfWork.BattleLogs.Where(x => x.UserName == userName));

                    _unitOfWork.Save();
                    Console.WriteLine("Room Code:");
                    string room = Console.ReadLine();

                    var lobby = _unitOfWork.PlayerLobby.FirstOrDefault(x => x.LobbyNo == room);
                    if (lobby.Status == false)
                    {
                        var user = _unitOfWork.Users.FirstOrDefault(x => x.UserName == userName);
                        while (true)
                        {
                            if (_unitOfWork.PlayerLobby.Any(x => x.LobbyNo == room))
                            {
                                Console.WriteLine(_unitOfWork.PlayerLobby.FirstOrDefault(x => x.LobbyNo == room).LobbyNo + " game room connected.");
                                _unitOfWork.PlayerLobby.Insert(new PlayerLobby
                                {
                                    LobbyNo = room,
                                    Score = 100,
                                    UserName = userName
                                });
                                _unitOfWork.Save();


                                string[] Player1Moves = { "", "", "", "", "" };
                                Console.WriteLine("1.Rock = R\n2.Paper = P\n3.Scissors = S\n4.Lizard = L\n5.Spock = V");
                                for (int j = 1; j < 6; j++)
                                {
                                    Console.WriteLine("Enter your " + j + ". move:");
                                    string playermoves = Console.ReadLine();
                                    _unitOfWork.BattleLogs.Insert(new BattleLogs
                                    {
                                        RoundNo = j,
                                        LobbyNo = room,
                                        Score = 100,
                                        UserName = userName,
                                        roundScore = 10,
                                        PlayerMove = playermoves

                                    });

                                    _unitOfWork.Save();


                                }

                                GetPoints(userName, room);

                                Console.ReadLine();
                                break;


                            }
                            else
                            {
                                Console.WriteLine("Your room code is not found.");
                                Console.WriteLine("Room Code:");
                                room = Console.ReadLine();
                        
                            }
                         
                        }
                    }
                    else
                    {
                        Console.WriteLine("Try a another room.This game is already started.");
                    }
                }
            }
        }

        private static void GetPoints(string userName, string room)
        {


            while (true)
            {
                if (_unitOfWork.PlayerLobby.Count(x => x.UserName != userName && x.LobbyNo == room) > 0)
                {
                   
                    if (_unitOfWork.BattleLogs.Where(x => x.LobbyNo == room).Count() > 9)
                    {

                        var mycardsmoves = _unitOfWork.BattleLogs.Where(x => x.UserName != userName && x.LobbyNo == room);
                        var otherplayermoves = _unitOfWork.BattleLogs.Where(x => x.UserName == userName && x.LobbyNo == room);


                        var gamer1Score = _unitOfWork.BattleLogs.FirstOrDefault(x => x.UserName != userName && x.LobbyNo == room);
                        int Player1Score = gamer1Score.roundScore;
                        var gamer2Score = _unitOfWork.BattleLogs.FirstOrDefault(x => x.UserName == userName && x.LobbyNo == room);
                        int Player2Score = gamer2Score.roundScore;
                        var Player1Values = _unitOfWork.BattleLogs.FirstOrDefault(x => x.UserName != userName && x.LobbyNo == room);
                        var Player1Name = Player1Values.UserName;
                        var Player2Values = _unitOfWork.BattleLogs.FirstOrDefault(x => x.UserName == userName && x.LobbyNo == room);
                        var Player2Name = Player2Values.UserName;
                        var isAdminStatus1 = _unitOfWork.Users.FirstOrDefault(x => x.UserName == userName);
                        bool isAdminStatusPlayer1 = isAdminStatus1.isAdmin;
                        var isAdminStatus2 = _unitOfWork.Users.FirstOrDefault(x => x.UserName != userName);
                        bool isAdminStatusPlayer2 = isAdminStatus1.isAdmin;
                        for (int c = 1; c < 6; c++)
                        {
                            var Player1Move = mycardsmoves[c - 1].PlayerMove;
                            var Player2Move = otherplayermoves[c - 1].PlayerMove;


                            if (Player1Move == "R" && Player2Move == "P") { Player1Score++; }
                            if (Player1Move == "R" && Player2Move == "S") { Player1Score++; }
                            if (Player1Move == "R" && Player2Move == "L") { Player2Score++; }
                            if (Player1Move == "R" && Player2Move == "V") { Player2Score++; }
                            if (Player1Move == "P" && Player2Move == "R") { Player1Score++; }
                            if (Player1Move == "P" && Player2Move == "S") { Player1Score++; }
                            if (Player1Move == "P" && Player2Move == "L") { Player2Score++; }
                            if (Player1Move == "P" && Player2Move == "V") { Player2Score++; }
                            if (Player1Move == "S" && Player2Move == "R") { Player2Score++; }
                            if (Player1Move == "S" && Player2Move == "P") { Player1Score++; }
                            if (Player1Move == "S" && Player2Move == "L") { Player1Score++; }
                            if (Player1Move == "S" && Player2Move == "V") { Player2Score++; }
                            if (Player1Move == "V" && Player2Move == "R") { Player1Score++; }
                            if (Player1Move == "V" && Player2Move == "P") { Player2Score++; }
                            if (Player1Move == "V" && Player2Move == "S") { Player1Score++; }
                            if (Player1Move == "V" && Player2Move == "L") { Player2Score++; }
                            if (Player1Move == "L" && Player2Move == "R") { Player2Score++; }
                            if (Player1Move == "L" && Player2Move == "P") { Player1Score++; }
                            if (Player1Move == "L" && Player2Move == "S") { Player1Score++; }
                            if (Player1Move == "L" && Player2Move == "V") { Player2Score++; }
                        }

                        Console.WriteLine(Player1Name + " Score = " + Player1Score);
                        Console.WriteLine(Player2Name + " Score = " + Player2Score);

                        if (Player1Score > Player2Score)
                        {


                            statusFalse();
                            isAdminStatusPlayer1 = true;
                            isAdminStatus1.isAdmin = isAdminStatusPlayer1;
                            _unitOfWork.Users.Update(isAdminStatus1);
                            _unitOfWork.Save();
                     

                            Console.WriteLine(Player1Name + " is Winner..\n" + Player1Name + " is Admin now..");
                            Console.WriteLine(Player1Name + " Admin status = " + isAdminStatusPlayer1 + "\n" + Player2Name + " Admin status = " + isAdminStatusPlayer2);

                        }
                        else
                        {

                          

                            statusFalse();
                            isAdminStatusPlayer2 = true;
                            isAdminStatus2.isAdmin = isAdminStatusPlayer2;
                            _unitOfWork.Users.Update(isAdminStatus2);
                            _unitOfWork.Save();
                         
                            Console.WriteLine(Player2Name + " is Winner..\n" + Player2Name + " is Admin now..");
                            Console.WriteLine(Player1Name + " Admin status = " + isAdminStatusPlayer1 + "\n" + Player2Name + " Admin status = " + isAdminStatusPlayer2);
                        }



                    }
                    else
                    {
                        Console.WriteLine("Waiting for the other player completing his rounds.");
                        while (true)
                        {
                            if (_unitOfWork.BattleLogs.Where(x => x.LobbyNo == room).Count() > 9)
                            {

                                var mycardsmoves = _unitOfWork.BattleLogs.Where(x => x.UserName == userName && x.LobbyNo == room);
                                var otherplayermoves = _unitOfWork.BattleLogs.Where(x => x.UserName != userName && x.LobbyNo == room);

                                var gamer1Score = _unitOfWork.BattleLogs.FirstOrDefault(x => x.UserName == userName && x.LobbyNo == room);
                                var gamer2Score = _unitOfWork.BattleLogs.FirstOrDefault(x => x.UserName != userName && x.LobbyNo == room);
                                
                                
                               
                                var gamer1Point = _unitOfWork.PlayerLobby.FirstOrDefault(x => x.UserName == userName && x.LobbyNo == room);
                                var gamer2Point = _unitOfWork.PlayerLobby.FirstOrDefault(x => x.UserName != userName && x.LobbyNo == room);
                                var Player2Score = gamer2Score.roundScore;
                                var Player1Score = gamer1Score.roundScore;
                                var Player1Values = _unitOfWork.BattleLogs.FirstOrDefault(x => x.UserName == userName && x.LobbyNo == room);
                                var Player1Name = Player1Values.UserName;
                                var Player2Values = _unitOfWork.BattleLogs.FirstOrDefault(x => x.UserName != userName && x.LobbyNo == room);
                                var Player2Name = Player2Values.UserName;
                               
                                
                                var isAdminStatus1 = _unitOfWork.Users.FirstOrDefault(x => x.UserName == userName);
                                bool isAdminStatusPlayer1 = isAdminStatus1.isAdmin;
                                var isAdminStatus2 = _unitOfWork.Users.FirstOrDefault(x => x.UserName != Player2Name);
                                bool isAdminStatusPlayer2 = isAdminStatus1.isAdmin;
                                var leaugeScore1 = isAdminStatus1.Coin;
                                var leaugeScore2 = isAdminStatus2.Coin;


                                for (int c = 1; c < 6; c++)
                                {
                                    var Player1Move = mycardsmoves[c - 1].PlayerMove;
                                    var Player2Move = otherplayermoves[c - 1].PlayerMove;

                                    if (Player1Move == "R" && Player2Move == "P") { Player1Score++; }
                                    if (Player1Move == "R" && Player2Move == "S") { Player1Score++; }
                                    if (Player1Move == "R" && Player2Move == "L") { Player2Score++; }
                                    if (Player1Move == "R" && Player2Move == "V") { Player2Score++; }
                                    if (Player1Move == "P" && Player2Move == "R") { Player1Score++; }
                                    if (Player1Move == "P" && Player2Move == "S") { Player1Score++; }
                                    if (Player1Move == "P" && Player2Move == "L") { Player2Score++; }
                                    if (Player1Move == "P" && Player2Move == "V") { Player2Score++; }
                                    if (Player1Move == "S" && Player2Move == "R") { Player2Score++; }
                                    if (Player1Move == "S" && Player2Move == "P") { Player1Score++; }
                                    if (Player1Move == "S" && Player2Move == "L") { Player1Score++; }
                                    if (Player1Move == "S" && Player2Move == "V") { Player2Score++; }
                                    if (Player1Move == "V" && Player2Move == "R") { Player1Score++; }
                                    if (Player1Move == "V" && Player2Move == "P") { Player2Score++; }
                                    if (Player1Move == "V" && Player2Move == "S") { Player1Score++; }
                                    if (Player1Move == "V" && Player2Move == "L") { Player2Score++; }
                                    if (Player1Move == "L" && Player2Move == "R") { Player2Score++; }
                                    if (Player1Move == "L" && Player2Move == "P") { Player1Score++; }
                                    if (Player1Move == "L" && Player2Move == "S") { Player1Score++; }
                                    if (Player1Move == "L" && Player2Move == "V") { Player2Score++; }
                                }
                                Console.WriteLine(Player1Name + " Score = " + Player1Score);
                                Console.WriteLine(Player2Name + " Score = " + Player2Score);

                                if (Player1Score > Player2Score)
                                {
                                   
                                    gamer1Point.Score++;
                                    gamer2Point.Score--;
                                    leaugeScore1++;
                                    leaugeScore2--;

                                    statusFalse();
                                    isAdminStatusPlayer1 = true;
                                    isAdminStatus1.isAdmin = isAdminStatusPlayer1;
                                    _unitOfWork.Users.Update(isAdminStatus1);
                                    _unitOfWork.Save();
                                    
                                    Console.WriteLine(Player1Name + " is Winner..\n" + Player1Name + " is Admin now..");
                                    Console.WriteLine(Player1Name + " Admin status = " + isAdminStatusPlayer1 + "\n" + Player2Name + " Admin status = " + isAdminStatusPlayer2);


                                }
                                else
                                {
                                   
                                    gamer1Point.Score--;
                                    gamer2Point.Score++;
                                    leaugeScore1--;
                                    leaugeScore2++;

                                    statusFalse();
                                    isAdminStatusPlayer2 = true;
                                    isAdminStatus2.isAdmin = isAdminStatusPlayer2;
                                    _unitOfWork.Users.Update(isAdminStatus2);
                                    _unitOfWork.Save();
                                   
                                    Console.WriteLine(Player2Name + " is Winner..\n" + Player2Name + " is Admin now..");
                                    Console.WriteLine(Player1Name + " Admin status = " + isAdminStatusPlayer1 + "\n" + Player2Name + " Admin status = " + isAdminStatusPlayer2);
                                }

                                gamer1Score.roundScore = Player1Score;
                                gamer2Score.roundScore = Player2Score;
                                isAdminStatus1.Coin = leaugeScore1;
                                isAdminStatus2.Coin = leaugeScore2;

                                _unitOfWork.Users.Update(isAdminStatus1);
                                _unitOfWork.Users.Update(isAdminStatus2);
                                _unitOfWork.PlayerLobby.Update(gamer1Point);
                                _unitOfWork.PlayerLobby.Update(gamer2Point);
                                _unitOfWork.BattleLogs.Update(gamer1Score);
                                _unitOfWork.BattleLogs.Update(gamer2Score);
                             
                               
                                _unitOfWork.Save();

                                break;

                            }
                        }
                    }



                    Console.ReadLine();
                }
                break;
            }
        }

        public class UserMusics
        {
            public int UserID { get; set; }
            public string Music { get; set; }

           
        }
        public static void statusFalse()
        {


            var statusFalser = _unitOfWork.Users.GetList();

            foreach (var item in statusFalser)
            {
                item.isAdmin = false;
                _unitOfWork.Save();
            }

        }

        public static void partyroom(string userName1)
        {
     
            Console.Clear();
            var userID = _unitOfWork.Users.FirstOrDefault(x => x.UserName == userName1).UserID;
            List<UserMusics> userMusics = (from user in _unitOfWork.UserMusics.Where(x => x.UserID == userID)
                                           join music in _unitOfWork.Musics.GetList() on user.MusicID equals music._id
                                           select new UserMusics
                                           {
                                               Music = music.FilePath,
                                               UserID = user.UserID
                                           }).ToList();
            for (int i = 0; i < userMusics.Count(); i++)
            {
                var music = userMusics[i];
                Console.WriteLine((i + 1) + "-Music Name:" + music.Music.Replace("_", " ").Replace("\\", "").Replace("Musics", "") + "\n");
            }
            var ekliolmayanmuzikler = _unitOfWork.Musics.GetList();
            foreach (var item in _unitOfWork.Musics.GetList())
            {
                if (!userMusics.Any(x => x.Music == item.FilePath))
                {
                    Console.WriteLine("This song not in yout list  " + "Music Name:" + item.FilePath.Replace("_", " ").Replace("\\", "").Replace("Musics", ""));
                    
                }
                else
                {
                    ekliolmayanmuzikler.Remove(item);
                }
            }
            

            Console.WriteLine("Selected your party music NO : ");
            int musicno = Convert.ToInt32(Console.ReadLine());
            var file = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + userMusics[musicno].Music;
            Console.WriteLine("Your music is playing..");
            Process.Start(@"powershell", $@"-c (New-Object Media.SoundPlayer '{file}').PlaySync();");



        }

    }
}





