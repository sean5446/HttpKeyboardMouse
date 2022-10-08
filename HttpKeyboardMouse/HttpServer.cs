
using System.Diagnostics;
using System.Net;
using System.Text;

namespace WinFormsApp1
{
    internal class HttpServer
    {
        private int _port = 8080;
        private readonly HttpListener _httpListener = new();
        private byte[] _pageContent = { 0x00 };
        private byte[] _favIcon = { 0x00 };

        public void Start(int? port = null)
        {
            if (port != null) _port = port.Value;

            // for debug, use relative to project. for normal use, assume www is in the current dir
            string currentDir = Environment.CurrentDirectory;
            if (currentDir.ToLower().Contains("debug"))
            {
                _pageContent = File.ReadAllBytes("../../../../www/km.htm");
                _favIcon = File.ReadAllBytes("../../../../www/favicon.ico");
            } else
            {
                _pageContent = File.ReadAllBytes(Path.Combine(currentDir, "www", "km.htm"));
                _favIcon = File.ReadAllBytes(Path.Combine(currentDir, "www", "favicon.ico"));
            }

            /*
             * netsh http add urlacl url=http://*:8080/ user=Everyone listen=yes
             * 
             * netsh http delete urlacl  url=http://*:8080/
             * 
             * netsh advfirewall firewall add rule name="TCP Port 8080" dir=in localport=8080 protocol=TCP action=allow
             * 
             * netsh advfirewall firewall delete rule name="TCP Port 8080"
             * 
             */

            _httpListener.Prefixes.Add($"http://*:{_port}/");
            _httpListener.Start();
            Receive();
        }

        public void Stop()
        {
            _httpListener.Stop();
        }

        private void Receive()
        {
            _httpListener.BeginGetContext(new AsyncCallback(HttpListenerCallback), _httpListener);
        }

        private void HttpListenerCallback(IAsyncResult result)
        {
            if (_httpListener.IsListening)
            {
                HttpListenerContext context = _httpListener.EndGetContext(result);
                HttpListenerRequest request = context.Request;

                ParseRequest(request, context);

                Receive();
            }
        }

        private void ParseRequest(HttpListenerRequest request, HttpListenerContext context)
        {
            HttpListenerResponse response = context.Response;

            // GET request
            if (request.HttpMethod == HttpMethod.Get.ToString())
            {
                HandleGet(request, response);
            }

            if (request.HasEntityBody)
            {
                Stream body = request.InputStream;
                Encoding contentEncoding = request.ContentEncoding;
                StreamReader bodyReader = new(body, contentEncoding);
                string bodyContent = bodyReader.ReadToEnd();
                bodyReader.Close();
                body.Close();

                // POST request
                if (request.HttpMethod == HttpMethod.Post.ToString())
                {
                    HandlePost(request, response, bodyContent);
                }
            }

            response.StatusCode = (int)HttpStatusCode.OK;
            response.OutputStream.Close();
        }

        private void HandleGet(HttpListenerRequest request, HttpListenerResponse response)
        {
            if (request.RawUrl != null && request.RawUrl.Equals("/favicon.ico"))
            {
                response.ContentType = "image/x-icon";
                response.OutputStream.Write(_favIcon, 0, _favIcon.Length);
            }
            else
            {
                response.ContentType = "text/html; charset=utf-8";
                response.OutputStream.Write(_pageContent, 0, _pageContent.Length);
            }
        }

        private static void HandlePost(HttpListenerRequest request, HttpListenerResponse response, string bodyContent)
        {
            response.ContentType = "text/plain; charset=utf-8";

            switch (request.RawUrl)
            {
                case "/mouse":
                    HandleMouse(bodyContent);
                    // don't spam swipe messages
                    if (bodyContent.StartsWith("left") || bodyContent.StartsWith("right") || bodyContent.StartsWith("middle"))
                        Trace.WriteLine($"{request.RemoteEndPoint} used mouse: {bodyContent}");
                    break;
                case "/keys":
                    HandleKeys(bodyContent);
                    Trace.WriteLine($"{request.RemoteEndPoint} typed message: {bodyContent}");
                    break;
                case "/media":
                    HandleMedia(bodyContent);
                    Trace.WriteLine($"{request.RemoteEndPoint} used media operation: {bodyContent}");
                    break;
                case "/screens":
                    HandleScreens(bodyContent);
                    Trace.WriteLine($"{request.RemoteEndPoint} moved to screen: {bodyContent}");
                    break;
                default:
                    Trace.WriteLine($"Unknown endpoint: {bodyContent}");
                    break;
            }
        }

        private static void HandleMouse(string bodyContent)
        {
            try
            {
                string data0 = bodyContent.Split(' ')[0];
                string data1 = bodyContent.Split(' ')[1];

                if (data0.Equals("right"))
                {
                    if (data1.Equals("up"))
                    {
                        WinAPI.GetCursorPos(out WinAPI.POINT p);
                        WinAPI.MouseRightClick(p.X, p.Y);
                    }
                }
                else if (data0.Equals("middle"))
                {
                    if (data1.Equals("up"))
                    {
                        WinAPI.GetCursorPos(out WinAPI.POINT p);
                        WinAPI.MouseMiddleClick(p.X, p.Y);
                    }
                }
                else if (data0.Equals("left"))
                {
                    if (data1.Equals("up")) {
                        WinAPI.GetCursorPos(out WinAPI.POINT p);
                        WinAPI.MouseLeftUp(p.X, p.Y);
                    }
                    else if (data1.Equals("down"))
                    {
                        WinAPI.GetCursorPos(out WinAPI.POINT p);
                        WinAPI.MouseLeftDown(p.X, p.Y);
                    }
                    else
                    {
                        WinAPI.GetCursorPos(out WinAPI.POINT p);
                        WinAPI.MouseLeftClick(p.X, p.Y);
                    }
                }
                else
                {
                    int x = (int)Convert.ToDouble(data0);
                    int y = (int)Convert.ToDouble(data1);
                    WinAPI.GetCursorPos(out WinAPI.POINT p);
                    WinAPI.SetCursorPos(p.X + x, p.Y + y);
                }
            }
            catch (Exception)
            {
                Trace.WriteLine($"Unknown mouse content: {bodyContent}");
            }
        }

        private static void HandleKeys(string bodyContent)
        {
            SendKeys.SendWait(bodyContent);
        }

        private static void HandleMedia(string bodyContent)
        {
            switch (bodyContent)
            {
                case "up": 
                    WinAPI.SendKey(WinAPI.VK_VOLUME_UP); 
                    break;
                case "down":
                    WinAPI.SendKey(WinAPI.VK_VOLUME_DOWN); 
                    break;
                case "mute":
                    WinAPI.SendKey(WinAPI.VK_VOLUME_MUTE); 
                    break;
                case "play":
                    WinAPI.SendKey(WinAPI.VK_MEDIA_PLAY_PAUSE); 
                    break;
                case "next":
                    WinAPI.SendKey(WinAPI.VK_MEDIA_NEXT_TRACK); 
                    break;
                case "back":
                    WinAPI.SendKey(WinAPI.VK_MEDIA_PREV_TRACK); 
                    break;
                case "stop":
                    WinAPI.SendKey(WinAPI.VK_MEDIA_STOP); 
                    break;
                default:
                    Trace.WriteLine($"Unknown media content: {bodyContent}");
                    break;
            }
        }

        private static void HandleScreens(string bodyContent)
        {
            int n = (int)Convert.ToDouble(bodyContent) - 1;
            WinAPI.SetCursorInMiddleOfScreen(n);
        }

    }
}
