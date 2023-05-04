using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Configuration;

namespace COMOSQR_CodeScanner.ViewModel
{
    internal class LoginViewModel : HTTPBaseViewModel
    {
        public Page Page { get; set; }

        // Konstruktor
        public LoginViewModel()
        {
            LoginModel = new Model.LoginModel()
            {
                Webserver = ConfigurationManager.AppSettings["webserver"],
                Username = ConfigurationManager.AppSettings["username"],
                DoLogin = new Commands.DelegateCommandGen<PasswordBox>(canExecute: CheckLogin, execute: Login)
            };
        }


        private string _feedback;

        public string Feedback
        {
            get
            {
                return _feedback;
            }
            set
            {
                if (_feedback != value)
                {
                    _feedback = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private Model.LoginModel _loginModel;
        public Model.LoginModel LoginModel
        {
            get
            {
                return _loginModel;
            }
            set
            {
                if (_loginModel != value)
                {
                    _loginModel = value;

                    NotifyPropertyChanged();
                }
            }
        }
        
        // Login-Methode
        private async void Login(PasswordBox pwb)
        {
            Feedback = "Login läuft...";

            BaseHttpClient = new HttpClient();
            if (BaseHttpClient != null)
            {
                var creds = Convert.ToBase64String(Encoding.UTF8.GetBytes(LoginModel.Username + ":" + pwb.Password));
                BaseHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", creds);

                try
                {
                    BaseHttpClient.BaseAddress = new Uri(LoginModel.Webserver);
                }

                catch
                {
                    Feedback = "Bitte überprüfen Sie das Format des Webservers.";
                    return;
                }

                BaseHttpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                var loginresponse = await SessionService.Login(BaseHttpClient);

                if (loginresponse.IsSuccessStatusCode)
                {
                    string json = loginresponse.Content.ReadAsStringAsync().Result;
                    var session = JsonConvert.DeserializeObject<Model.SessionModel>(json);

                    BaseHttpClient.DefaultRequestHeaders.Add("X-Comos-Session", session.Id);
                    Feedback = "Login erfolgreich!";

                    InitalizeHeartBeat(session.HeartBeat);

                    Uri uri = new Uri("View/Projectpage.xaml", UriKind.Relative);
                    Page.NavigationService.Navigate(uri);
                }

                else
                {
                    Feedback = "Login fehlgeschlagen.";
                }
            }
        }


        // Überprüfung, ob Login-Methode ausgeführt werden kann/darf
        private bool CheckLogin()
        {
            if (!String.IsNullOrEmpty(LoginModel.Webserver) && !String.IsNullOrEmpty(LoginModel.Username))
            {
                return true;
            }

            else
            {
                Feedback = "Bitte Webserver & Username ausfüllen";
                return false;
            }
        }
    }
}
