
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

                Trace.WriteLine($"{request.HttpMethod} {request.Url}");

                ParseRequest(request, context);

                Receive();
            }
        }

        private void ParseRequest(HttpListenerRequest request, HttpListenerContext context)
        {
            HttpListenerResponse response = context.Response;

            if (request.HasEntityBody)
            {
                Stream body = request.InputStream;
                Encoding contentEncoding = request.ContentEncoding;
                StreamReader bodyReader = new(body, contentEncoding);
                string bodyContent = bodyReader.ReadToEnd();
                bodyReader.Close();
                body.Close();

                Debug.WriteLine($"Data: {bodyContent}\n");

                // POST request - only answer content that's allowed
                if (request.HttpMethod == HttpMethod.Post.ToString())
                {
                    HandlePost(request, response, bodyContent);
                }
            }

            // GET request - serve the 1 page we allow
            if (request.HttpMethod == HttpMethod.Get.ToString())
            {
                HandleGet(request, response);
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
            if (request.RawUrl == null) return;

            response.ContentType = "text/plain; charset=utf-8";

            if (request.RawUrl.Equals("/tap"))
            {
                if (bodyContent.Equals("right"))
                {
                    WinAPI.GetCursorPos(out WinAPI.POINT p);
                    WinAPI.RightMouseClick(p.X, p.Y);
                }
                else if (bodyContent.Equals("middle"))
                {
                    WinAPI.GetCursorPos(out WinAPI.POINT p);
                    WinAPI.MiddleMouseClick(p.X, p.Y);
                }
                else
                {
                    WinAPI.GetCursorPos(out WinAPI.POINT p);
                    WinAPI.LeftMouseClick(p.X, p.Y);
                }
            }
            else if (request.RawUrl.Equals("/swipe"))
            {
                int x = (int)Convert.ToDouble(bodyContent.Split(' ')[0]);
                int y = (int)Convert.ToDouble(bodyContent.Split(' ')[1]);
                WinAPI.GetCursorPos(out WinAPI.POINT p);
                WinAPI.SetCursorPos(p.X + x, p.Y + y);
                //Mouse.MoveTo(x, y);
            }
            else if (request.RawUrl.Equals("/keys"))
            {
                if (bodyContent.Equals("backspace"))
                {
                    SendKeys.SendWait("{BACKSPACE}");
                }
                else
                {
                    SendKeys.SendWait(bodyContent);
                }
            }
            else if (request.RawUrl.Equals("/media"))
            {
                if (bodyContent.Equals("up"))
                {
                    WinAPI.SendKey(WinAPI.VK_VOLUME_UP);
                }
                else if (bodyContent.Equals("down"))
                {
                    WinAPI.SendKey(WinAPI.VK_VOLUME_DOWN);
                }
                else if (bodyContent.Equals("mute"))
                {
                    WinAPI.SendKey(WinAPI.VK_VOLUME_MUTE);
                }
                else if (bodyContent.Equals("play"))
                {
                    WinAPI.SendKey(WinAPI.VK_MEDIA_PLAY_PAUSE);
                }
                else if (bodyContent.Equals("next"))
                {
                    WinAPI.SendKey(WinAPI.VK_MEDIA_NEXT_TRACK);
                }
                else if (bodyContent.Equals("back"))
                {
                    WinAPI.SendKey(WinAPI.VK_MEDIA_PREV_TRACK);
                }
                else if (bodyContent.Equals("stop"))
                {
                    WinAPI.SendKey(WinAPI.VK_MEDIA_STOP);
                }
            }
        }
    }
}
