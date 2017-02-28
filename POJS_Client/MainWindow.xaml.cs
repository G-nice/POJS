using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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

namespace POJS_Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        string serverIP = "";
        string port = "";
        string sorcecode = "";
        string input = "";
        string output = "";
        string result = "";

        private void button_submit_Click(object sender, RoutedEventArgs e)
        {
            new Thread(submit).Start();
        }

        private void button_check_Click(object sender, RoutedEventArgs e)
        {
            new Thread(check_connect).Start();
        }


        void check_connect()
        {
            this.Dispatcher.Invoke((Action)delegate () { button_check.IsEnabled = false; });
            this.Dispatcher.Invoke((Action) delegate() { get_setting(); });

            string result = "";
            string url = "http://" + serverIP + ":" + port + "/check";
            HttpWebRequest req = null;
            HttpWebResponse resp = null;
            Stream stream = null;
            try
            {
                req = (HttpWebRequest)WebRequest.Create(url);
                req.Timeout = 10000;  // 设置连接超时5秒
                resp  = (HttpWebResponse)req.GetResponse();
                stream = resp.GetResponseStream();
                //获取内容  
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
                if (result.Equals("OK\r\n"))
                    this.Dispatcher.Invoke((Action)delegate () { set_check_ressult("1"); });
            }
            catch(WebException e)
            {
                MessageBox.Show("连接超时，请检查服务器配置", "配  置");
                this.Dispatcher.Invoke((Action)delegate () { set_check_ressult("0"); });
            }
            catch(UriFormatException e)
            {
                MessageBox.Show("服务器地址或端口填写错误，请检查服务器配置", "配  置");
                this.Dispatcher.Invoke((Action)delegate () { set_check_ressult("2");  });
            }
            finally
            {
                if(stream != null)
                    stream.Close();
                if(req != null)
                    req.Abort();
                this.Dispatcher.Invoke((Action)delegate () { button_check.IsEnabled = true; });
            }

            //return check_result;
        }


        void submit()
        {
            this.Dispatcher.Invoke((Action)delegate () 
            {
                textBox_result.Text = string.Empty;
                get_sorcecode_input_output();
            });

            string result = "";
            string url = "http://" + serverIP + ":" + port + "/submit";
            HttpWebRequest req = null;
            try
            {
                req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.Timeout = 10000;
                req.KeepAlive = false;
                req.ReadWriteTimeout = 10000;
                req.ContentType = "application/x-www-form-urlencoded";

                #region 添加Post 参数  
                byte[] data = Encoding.UTF8.GetBytes(sorcecode + "#&" + input + "#&" + output);
                req.ContentLength = data.Length;
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Flush();
                    reqStream.Close();
                }
                #endregion

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream stream = resp.GetResponseStream();
                //获取响应内容  
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
                this.Dispatcher.Invoke((Action)delegate ()
                {
                    textBox_result.Text = result;
                });

            }
            catch(WebException e)
            {
                MessageBox.Show("连接超时，请检查服务器配置", "配  置");
                this.Dispatcher.Invoke((Action)delegate () 
                {
                    set_check_ressult("0");
                    button_submit.IsEnabled = false;
                });
            }
            finally
            {
                if (req != null)
                    req.Abort();

            }
        }


        void get_setting()
        {
            serverIP = textBox_serverIP.Text;
            port = textBox_port.Text;
        }

        void get_sorcecode_input_output()
        {
            sorcecode = textBox_code.Text;
            input = textBox_input.Text;
            output = textBox_output.Text;
        }

        void set_check_ressult(string hint)
        {
            switch(hint)
            {
                case "1":
                    label_check_result.Content = "配置成功";
                    label_check_result.Foreground = Brushes.Green;
                    //button_check.IsEnabled = false;
                    button_submit.IsEnabled = true;
                    break;
                case "2":
                    label_check_result.Content = "服务器地址或端口错误";
                    label_check_result.Foreground = Brushes.Red;
                    break;
                case "0":
                default:
                    label_check_result.Content = "连接超时";
                    label_check_result.Foreground = Brushes.Red;
                    break;
            }
            label_check_result.Visibility = Visibility.Visible;

        }

        void set_result(string result)
        {
            textBox_result.Text = result;
        }

        private void textBox_serverIP_TextChanged(object sender, TextChangedEventArgs e)
        {
            button_submit.IsEnabled = false;
            label_check_result.Visibility = Visibility.Collapsed;
        }

        private void textBox_port_TextChanged(object sender, TextChangedEventArgs e)
        {
            button_submit.IsEnabled = false;
            label_check_result.Visibility = Visibility.Collapsed;
        }

        private void textBox_code_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBox_result.Text = string.Empty;
        }
    }
}
