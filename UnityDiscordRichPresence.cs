#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;

namespace UnityDiscordRichPresence
{
    /// <summary>
    /// Persistent user preferences for Discord Rich Presence, saved via EditorPrefs.
    /// </summary>
    public static class DiscordPresenceSettings
    {
        private const string PREF_PREFIX = "UnityDiscordRPC_";

        public static bool Enabled
        {
            get => EditorPrefs.GetBool(PREF_PREFIX + "Enabled", true);
            set => EditorPrefs.SetBool(PREF_PREFIX + "Enabled", value);
        }

        public static string ApplicationId
        {
            get => EditorPrefs.GetString(PREF_PREFIX + "AppId", "1555806398204608562");
            set => EditorPrefs.SetString(PREF_PREFIX + "AppId", string.IsNullOrWhiteSpace(value) ? "1555806398204608562" : value.Trim());
        }

        public static bool ShowProjectName
        {
            get => EditorPrefs.GetBool(PREF_PREFIX + "ShowProjectName", true);
            set => EditorPrefs.SetBool(PREF_PREFIX + "ShowProjectName", value);
        }

        public static bool ShowProjectVersion
        {
            get => EditorPrefs.GetBool(PREF_PREFIX + "ShowProjectVersion", true);
            set => EditorPrefs.SetBool(PREF_PREFIX + "ShowProjectVersion", value);
        }

        public static bool ShowSceneName
        {
            get => EditorPrefs.GetBool(PREF_PREFIX + "ShowSceneName", true);
            set => EditorPrefs.SetBool(PREF_PREFIX + "ShowSceneName", value);
        }

        public static bool ShowDirtyIndicator
        {
            get => EditorPrefs.GetBool(PREF_PREFIX + "ShowDirtyIndicator", true);
            set => EditorPrefs.SetBool(PREF_PREFIX + "ShowDirtyIndicator", value);
        }

        public static bool ShowPrefabName
        {
            get => EditorPrefs.GetBool(PREF_PREFIX + "ShowPrefabName", true);
            set => EditorPrefs.SetBool(PREF_PREFIX + "ShowPrefabName", value);
        }

        public static bool ShowPlatform
        {
            get => EditorPrefs.GetBool(PREF_PREFIX + "ShowPlatform", true);
            set => EditorPrefs.SetBool(PREF_PREFIX + "ShowPlatform", value);
        }

        public static bool ShowUnityVersion
        {
            get => EditorPrefs.GetBool(PREF_PREFIX + "ShowUnityVersion", true);
            set => EditorPrefs.SetBool(PREF_PREFIX + "ShowUnityVersion", value);
        }

        public static bool ShowRenderPipeline
        {
            get => EditorPrefs.GetBool(PREF_PREFIX + "ShowRenderPipeline", false);
            set => EditorPrefs.SetBool(PREF_PREFIX + "ShowRenderPipeline", value);
        }

        public static bool ShowWorkingTime
        {
            get => EditorPrefs.GetBool(PREF_PREFIX + "ShowWorkingTime", true);
            set => EditorPrefs.SetBool(PREF_PREFIX + "ShowWorkingTime", value);
        }

        public static bool ResetTimerOnPlayMode
        {
            get => EditorPrefs.GetBool(PREF_PREFIX + "ResetTimerOnPlayMode", true);
            set => EditorPrefs.SetBool(PREF_PREFIX + "ResetTimerOnPlayMode", value);
        }

        public static bool AutoDetectVersionIcon
        {
            get => EditorPrefs.GetBool(PREF_PREFIX + "AutoDetectVersionIcon", true);
            set => EditorPrefs.SetBool(PREF_PREFIX + "AutoDetectVersionIcon", value);
        }

        public static string LargeImageKey
        {
            get => EditorPrefs.GetString(PREF_PREFIX + "LargeImageKey", "unity_6");
            set => EditorPrefs.SetString(PREF_PREFIX + "LargeImageKey", string.IsNullOrWhiteSpace(value) ? "unity_6" : value.Trim());
        }

        public static string PlayModeImageKey
        {
            get => EditorPrefs.GetString(PREF_PREFIX + "PlayModeImageKey", "play");
            set => EditorPrefs.SetString(PREF_PREFIX + "PlayModeImageKey", string.IsNullOrWhiteSpace(value) ? "play" : value.Trim());
        }

        public static string PauseModeImageKey
        {
            get => EditorPrefs.GetString(PREF_PREFIX + "PauseModeImageKey", "pause");
            set => EditorPrefs.SetString(PREF_PREFIX + "PauseModeImageKey", string.IsNullOrWhiteSpace(value) ? "pause" : value.Trim());
        }

        public static string EditModeImageKey
        {
            get => EditorPrefs.GetString(PREF_PREFIX + "EditModeImageKey", "edit");
            set => EditorPrefs.SetString(PREF_PREFIX + "EditModeImageKey", string.IsNullOrWhiteSpace(value) ? "edit" : value.Trim());
        }

        public static string CompileModeImageKey
        {
            get => EditorPrefs.GetString(PREF_PREFIX + "CompileModeImageKey", "compile");
            set => EditorPrefs.SetString(PREF_PREFIX + "CompileModeImageKey", string.IsNullOrWhiteSpace(value) ? "compile" : value.Trim());
        }

        public static string Button1Label
        {
            get => EditorPrefs.GetString(PREF_PREFIX + "Btn1Label", "");
            set => EditorPrefs.SetString(PREF_PREFIX + "Btn1Label", value.Trim());
        }

        public static string Button1Url
        {
            get => EditorPrefs.GetString(PREF_PREFIX + "Btn1Url", "");
            set => EditorPrefs.SetString(PREF_PREFIX + "Btn1Url", value.Trim());
        }

        public static string Button2Label
        {
            get => EditorPrefs.GetString(PREF_PREFIX + "Btn2Label", "");
            set => EditorPrefs.SetString(PREF_PREFIX + "Btn2Label", value.Trim());
        }

        public static string Button2Url
        {
            get => EditorPrefs.GetString(PREF_PREFIX + "Btn2Url", "");
            set => EditorPrefs.SetString(PREF_PREFIX + "Btn2Url", value.Trim());
        }

        public static void ResetToDefaults()
        {
            Enabled = true;
            ApplicationId = "1555806398204608562";
            ShowProjectName = true;
            ShowProjectVersion = true;
            ShowSceneName = true;
            ShowDirtyIndicator = true;
            ShowPrefabName = true;
            ShowPlatform = true;
            ShowUnityVersion = true;
            ShowRenderPipeline = false;
            ShowWorkingTime = true;
            ResetTimerOnPlayMode = true;
            AutoDetectVersionIcon = true;
            LargeImageKey = "unity_6";
            PlayModeImageKey = "play";
            PauseModeImageKey = "pause";
            EditModeImageKey = "edit";
            CompileModeImageKey = "compile";
            Button1Label = "";
            Button1Url = "";
            Button2Label = "";
            Button2Url = "";
        }
    }

    /// <summary>
    /// Pure C# lightweight Discord IPC Named Pipe client.
    /// Operates without external DLLs or Game SDK dependencies across Windows, macOS, and Linux.
    /// </summary>
    public sealed class DiscordIpcClient : IDisposable
    {
        private Stream _stream;
        private readonly object _lock = new object();

        public bool IsConnected { get; private set; }
        public string ConnectedPipeName { get; private set; } = string.Empty;

        public bool TryConnect(string applicationId, int timeoutMs = 200)
        {
            Close();

            lock (_lock)
            {
                if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        try
                        {
                            var pipe = new NamedPipeClientStream(".", $"discord-ipc-{i}", PipeDirection.InOut, PipeOptions.Asynchronous);
                            pipe.Connect(timeoutMs);
                            _stream = pipe;
                            ConnectedPipeName = $"discord-ipc-{i}";
                            break;
                        }
                        catch
                        {
                            // Try next pipe index
                        }
                    }
                }
                else
                {
                    // Unix Domain Sockets for macOS and Linux
                    string[] tempDirs = {
                        Environment.GetEnvironmentVariable("XDG_RUNTIME_DIR"),
                        Environment.GetEnvironmentVariable("TMPDIR"),
                        Environment.GetEnvironmentVariable("TMP"),
                        Environment.GetEnvironmentVariable("TEMP"),
                        "/tmp"
                    };

                    for (int i = 0; i < 10; i++)
                    {
                        foreach (var dir in tempDirs)
                        {
                            if (string.IsNullOrEmpty(dir)) continue;
                            string socketPath = Path.Combine(dir, $"discord-ipc-{i}");
                            if (!File.Exists(socketPath)) continue;

                            try
                            {
                                var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
                                socket.Connect(new UnixDomainSocketEndPoint(socketPath));
                                _stream = new NetworkStream(socket, true);
                                ConnectedPipeName = socketPath;
                                break;
                            }
                            catch
                            {
                                // Try next
                            }
                        }
                        if (_stream != null) break;
                    }
                }

                if (_stream == null) return false;

                // Send Handshake (Opcode 0)
                try
                {
                    string handshakeJson = $"{{\"v\":1,\"client_id\":\"{applicationId}\"}}";
                    WriteFrame(0, handshakeJson);

                    var (opcode, _) = ReadFrame();
                    if (opcode == 1) // DISPATCH / READY
                    {
                        IsConnected = true;
                        return true;
                    }
                }
                catch
                {
                    Close();
                    return false;
                }

                return false;
            }
        }

        public bool SendActivity(string activityJson)
        {
            lock (_lock)
            {
                if (!IsConnected || _stream == null) return false;
                try
                {
                    WriteFrame(1, activityJson);

                    // Drain response frame so OS pipe buffers stay clear
                    if (_stream.CanRead)
                    {
                        ReadFrame();
                    }
                    return true;
                }
                catch
                {
                    Close();
                    return false;
                }
            }
        }

        public void ClearActivity(int pid)
        {
            string clearJson = $"{{\"cmd\":\"SET_ACTIVITY\",\"args\":{{\"pid\":{pid}}},\"nonce\":\"{Guid.NewGuid()}\"}}";
            SendActivity(clearJson);
        }

        private void WriteFrame(int opcode, string json)
        {
            byte[] body = Encoding.UTF8.GetBytes(json);
            byte[] header = new byte[8];
            BitConverter.GetBytes(opcode).CopyTo(header, 0);
            BitConverter.GetBytes(body.Length).CopyTo(header, 4);

            _stream.Write(header, 0, 8);
            _stream.Write(body, 0, body.Length);
            _stream.Flush();
        }

        private (int opcode, string json) ReadFrame()
        {
            byte[] header = new byte[8];
            int read = 0;
            while (read < 8)
            {
                int r = _stream.Read(header, read, 8 - read);
                if (r <= 0) throw new IOException("Discord IPC closed unexpectedly");
                read += r;
            }

            int opcode = BitConverter.ToInt32(header, 0);
            int len = BitConverter.ToInt32(header, 4);

            if (len < 0 || len > 65536)
            {
                throw new IOException($"Invalid Discord IPC frame length: {len}");
            }

            byte[] body = new byte[len];
            int bodyRead = 0;
            while (bodyRead < len)
            {
                int r = _stream.Read(body, bodyRead, len - bodyRead);
                if (r <= 0) break;
                bodyRead += r;
            }

            string json = Encoding.UTF8.GetString(body, 0, bodyRead);
            return (opcode, json);
        }

        public void Close()
        {
            lock (_lock)
            {
                IsConnected = false;
                ConnectedPipeName = string.Empty;
                if (_stream != null)
                {
                    try { _stream.Dispose(); } catch { }
                    _stream = null;
                }
            }
        }

        public void Dispose()
        {
            Close();
        }
    }

    /// <summary>
    /// Core manager for Unity Editor Discord Rich Presence.
    /// Tracks scene changes, play/pause state, script compiling, and project metadata.
    /// </summary>
    [InitializeOnLoad]
    public static class UnityDiscordRichPresence
    {
        private static readonly DiscordIpcClient _ipc = new DiscordIpcClient();
        private static bool _isConnecting = false;
        private static double _lastConnectAttempt = -999.0;
        private static double _lastSendTime = -999.0;
        private static bool _needsUpdate = true;

        private static long _sessionStartTime = 0;
        private static long _playModeStartTime = 0;
        private static bool _wasCompiling = false;
        private static string _lastStateHash = string.Empty;

        public static bool IsConnected => _ipc.IsConnected;
        public static string ConnectedPipeName => _ipc.ConnectedPipeName;

        static UnityDiscordRichPresence()
        {
            InitializeSessionTimes();

            EditorApplication.update += OnEditorUpdate;
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
            EditorApplication.pauseStateChanged += OnPauseStateChanged;

            EditorSceneManager.activeSceneChangedInEditMode += OnSceneChanged;
            EditorSceneManager.sceneOpened += OnSceneOpened;
            EditorSceneManager.sceneSaved += OnSceneSaved;

            AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
            EditorApplication.quitting += OnEditorQuitting;

            HookPrefabStageEvents();

            RequestUpdate(immediate: true);
        }

        private static void InitializeSessionTimes()
        {
            try
            {
                long elapsedMs = EditorAnalyticsSessionInfo.elapsedTime;
                if (elapsedMs > 0)
                {
                    _sessionStartTime = DateTimeOffset.UtcNow.AddMilliseconds(-elapsedMs).ToUnixTimeSeconds();
                }
                else
                {
                    _sessionStartTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                }
            }
            catch
            {
                _sessionStartTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            }
        }

        private static void HookPrefabStageEvents()
        {
            try
            {
#pragma warning disable 0618
                PrefabStage.prefabStageOpened += _ => RequestUpdate(immediate: true);
                PrefabStage.prefabStageClosing += _ => RequestUpdate(immediate: true);
#pragma warning restore 0618
            }
            catch
            {
                // Unsupported in legacy Unity versions
            }
        }

        public static void RequestUpdate(bool immediate = false)
        {
            _needsUpdate = true;
            if (immediate)
            {
                _lastSendTime = -999.0;
            }
        }

        public static void Reconnect()
        {
            _ipc.Close();
            _lastConnectAttempt = -999.0;
            RequestUpdate(immediate: true);
        }

        private static void OnEditorUpdate()
        {
            if (!DiscordPresenceSettings.Enabled)
            {
                if (_ipc.IsConnected)
                {
                    _ipc.ClearActivity(Process.GetCurrentProcess().Id);
                    _ipc.Close();
                }
                return;
            }

            // Detect script compilation transition
            bool compiling = EditorApplication.isCompiling;
            if (compiling != _wasCompiling)
            {
                _wasCompiling = compiling;
                RequestUpdate(immediate: true);
            }

            double now = EditorApplication.timeSinceStartup;

            // Handle connection lifecycle asynchronously
            if (!_ipc.IsConnected)
            {
                if (!_isConnecting && now - _lastConnectAttempt >= 8.0)
                {
                    _lastConnectAttempt = now;
                    _isConnecting = true;
                    string appId = DiscordPresenceSettings.ApplicationId;

                    Task.Run(() =>
                    {
                        try
                        {
                            _ipc.TryConnect(appId);
                        }
                        catch
                        {
                            // Ignore connection failure
                        }
                        finally
                        {
                            _isConnecting = false;
                            if (_ipc.IsConnected)
                            {
                                RequestUpdate(immediate: true);
                            }
                        }
                    });
                }
                return;
            }

            // Periodic heartbeat / activity update (every 15s or immediate on state change)
            if (_needsUpdate || (now - _lastSendTime >= 15.0))
            {
                UpdateActivityNow();
            }
        }

        private static void UpdateActivityNow()
        {
            if (!_ipc.IsConnected) return;

            string activityJson = BuildActivityJson();
            if (string.IsNullOrEmpty(activityJson)) return;

            // Avoid redundant packet transmissions if state string is identical
            if (!_needsUpdate && activityJson == _lastStateHash)
            {
                _lastSendTime = EditorApplication.timeSinceStartup;
                return;
            }

            if (_ipc.SendActivity(activityJson))
            {
                _lastStateHash = activityJson;
                _lastSendTime = EditorApplication.timeSinceStartup;
                _needsUpdate = false;
            }
        }

        private static string BuildActivityJson()
        {
            int pid = Process.GetCurrentProcess().Id;

            // 1. Details (Project info)
            string details = "";
            if (DiscordPresenceSettings.ShowProjectName)
            {
                string projectName = !string.IsNullOrWhiteSpace(PlayerSettings.productName)
                    ? PlayerSettings.productName
                    : Application.productName;

                if (DiscordPresenceSettings.ShowProjectVersion && !string.IsNullOrWhiteSpace(PlayerSettings.bundleVersion))
                {
                    details = $"{projectName} (v{PlayerSettings.bundleVersion})";
                }
                else
                {
                    details = projectName;
                }
            }

            // 2. State (Editor activity context)
            string state = "";
            string smallImage = DiscordPresenceSettings.EditModeImageKey;
            string smallText = "Edit Mode";

            if (EditorApplication.isCompiling)
            {
                smallImage = DiscordPresenceSettings.CompileModeImageKey;
                state = "Compiling scripts...";
                smallText = "Compiling...";
            }
            else if (BuildPipeline.isBuildingPlayer)
            {
                smallImage = DiscordPresenceSettings.CompileModeImageKey;
                string platform = FormatBuildTarget(EditorUserBuildSettings.activeBuildTarget);
                state = $"Building {platform} Player...";
                smallText = "Building...";
            }
            else if (EditorApplication.isPlaying)
            {
                smallImage = EditorApplication.isPaused
                    ? DiscordPresenceSettings.PauseModeImageKey
                    : DiscordPresenceSettings.PlayModeImageKey;
                smallText = EditorApplication.isPaused ? "Paused" : "Play Mode";

                string activeSceneName = EditorSceneManager.GetActiveScene().name;
                if (string.IsNullOrEmpty(activeSceneName)) activeSceneName = "Untitled Scene";

                state = EditorApplication.isPaused
                    ? $"Paused: {activeSceneName}"
                    : $"Playtesting: {activeSceneName}";
            }
            else
            {
                // Check Prefab Isolation Stage
                string prefabName = GetCurrentPrefabStageName();
                if (DiscordPresenceSettings.ShowPrefabName && !string.IsNullOrEmpty(prefabName))
                {
                    state = $"Editing Prefab: {prefabName}";
                    smallText = "Prefab Mode";
                }
                else if (DiscordPresenceSettings.ShowSceneName)
                {
                    var scene = EditorSceneManager.GetActiveScene();
                    string sceneName = !string.IsNullOrEmpty(scene.name) ? scene.name : "Untitled Scene";
                    string dirtyIndicator = (DiscordPresenceSettings.ShowDirtyIndicator && scene.isDirty) ? " ●" : "";
                    state = $"Editing: {sceneName}{dirtyIndicator}";
                    smallText = "Edit Mode";
                }
                else
                {
                    state = "Editing project";
                }
            }

            // 3. Timestamps
            long startTimestamp = 0;
            if (DiscordPresenceSettings.ShowWorkingTime)
            {
                if (DiscordPresenceSettings.ResetTimerOnPlayMode && EditorApplication.isPlaying && _playModeStartTime > 0)
                {
                    startTimestamp = _playModeStartTime;
                }
                else
                {
                    startTimestamp = _sessionStartTime;
                }
            }

            // 4. Large Image & Text (Unity Engine & Environment)
            string largeImage = GetResolvedLargeImageKey();
            string largeText = "";

            if (DiscordPresenceSettings.ShowUnityVersion)
            {
                largeText = $"Unity {Application.unityVersion}";
                if (DiscordPresenceSettings.ShowPlatform)
                {
                    largeText += $" | {FormatBuildTarget(EditorUserBuildSettings.activeBuildTarget)}";
                }
                if (DiscordPresenceSettings.ShowRenderPipeline)
                {
                    largeText += $" ({GetRenderPipelineShortName()})";
                }
            }
            else if (DiscordPresenceSettings.ShowPlatform)
            {
                largeText = FormatBuildTarget(EditorUserBuildSettings.activeBuildTarget);
            }

            // Discord string safety (128 char limit, null if empty)
            details = TruncateAndClean(details, 128);
            state = TruncateAndClean(state, 128);
            largeText = TruncateAndClean(largeText, 128);
            smallText = TruncateAndClean(smallText, 128);

            // Assemble Activity JSON
            var sb = new StringBuilder(512);
            sb.Append("{\"cmd\":\"SET_ACTIVITY\",\"args\":{\"pid\":").Append(pid).Append(",\"activity\":{");

            bool hasPrev = false;

            if (!string.IsNullOrEmpty(state))
            {
                sb.Append("\"state\":\"").Append(EscapeJson(state)).Append("\"");
                hasPrev = true;
            }

            if (!string.IsNullOrEmpty(details))
            {
                if (hasPrev) sb.Append(",");
                sb.Append("\"details\":\"").Append(EscapeJson(details)).Append("\"");
                hasPrev = true;
            }

            if (startTimestamp > 0)
            {
                if (hasPrev) sb.Append(",");
                sb.Append("\"timestamps\":{\"start\":").Append(startTimestamp).Append("}");
                hasPrev = true;
            }

            // Assets block
            if (!string.IsNullOrEmpty(largeImage) || !string.IsNullOrEmpty(smallImage))
            {
                if (hasPrev) sb.Append(",");
                sb.Append("\"assets\":{");
                bool hasAsset = false;
                if (!string.IsNullOrEmpty(largeImage))
                {
                    sb.Append("\"large_image\":\"").Append(EscapeJson(largeImage)).Append("\"");
                    if (!string.IsNullOrEmpty(largeText))
                    {
                        sb.Append(",\"large_text\":\"").Append(EscapeJson(largeText)).Append("\"");
                    }
                    hasAsset = true;
                }
                if (!string.IsNullOrEmpty(smallImage))
                {
                    if (hasAsset) sb.Append(",");
                    sb.Append("\"small_image\":\"").Append(EscapeJson(smallImage)).Append("\"");
                    if (!string.IsNullOrEmpty(smallText))
                    {
                        sb.Append(",\"small_text\":\"").Append(EscapeJson(smallText)).Append("\"");
                    }
                }
                sb.Append("}");
                hasPrev = true;
            }

            // Buttons block (up to 2 buttons)
            var buttons = new List<(string label, string url)>();
            if (!string.IsNullOrEmpty(DiscordPresenceSettings.Button1Label) &&
                !string.IsNullOrEmpty(DiscordPresenceSettings.Button1Url) &&
                DiscordPresenceSettings.Button1Url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                buttons.Add((DiscordPresenceSettings.Button1Label, DiscordPresenceSettings.Button1Url));
            }
            if (!string.IsNullOrEmpty(DiscordPresenceSettings.Button2Label) &&
                !string.IsNullOrEmpty(DiscordPresenceSettings.Button2Url) &&
                DiscordPresenceSettings.Button2Url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                buttons.Add((DiscordPresenceSettings.Button2Label, DiscordPresenceSettings.Button2Url));
            }

            if (buttons.Count > 0)
            {
                if (hasPrev) sb.Append(",");
                sb.Append("\"buttons\":[");
                for (int b = 0; b < buttons.Count; b++)
                {
                    if (b > 0) sb.Append(",");
                    sb.Append("{\"label\":\"").Append(EscapeJson(buttons[b].label))
                      .Append("\",\"url\":\"").Append(EscapeJson(buttons[b].url)).Append("\"}");
                }
                sb.Append("]");
            }

            sb.Append("}},\"nonce\":\"").Append(Guid.NewGuid()).Append("\"}");
            return sb.ToString();
        }

        public static string GetResolvedLargeImageKey()
        {
            if (DiscordPresenceSettings.AutoDetectVersionIcon)
            {
                return GetVersionIconKey();
            }
            return DiscordPresenceSettings.LargeImageKey;
        }

        public static string GetVersionIconKey()
        {
            string version = Application.unityVersion ?? string.Empty;
            if (version.StartsWith("6000") || version.StartsWith("6."))
                return "unity_6";
            if (version.StartsWith("2023"))
                return "unity_2023";
            if (version.StartsWith("2022"))
                return "unity_2022";
            if (version.StartsWith("2021"))
                return "unity_2021";
            if (version.StartsWith("2020"))
                return "unity_2020";
            if (version.StartsWith("2019"))
                return "unity_2019";
            if (version.StartsWith("2018"))
                return "unity_2018";
            if (version.StartsWith("5."))
                return "unity_5";

            return DiscordPresenceSettings.LargeImageKey;
        }

        private static string GetCurrentPrefabStageName()
        {
            try
            {
#pragma warning disable 0618
                var stage = PrefabStageUtility.GetCurrentPrefabStage();
#pragma warning restore 0618
                if (stage != null && stage.prefabContentsRoot != null)
                {
                    return stage.prefabContentsRoot.name;
                }
            }
            catch
            {
                // Ignore
            }
            return null;
        }

        private static string GetRenderPipelineShortName()
        {
            var rp = GraphicsSettings.currentRenderPipeline;
            if (rp == null) return "Built-in RP";
            string name = rp.GetType().Name;
            if (name.Contains("Universal")) return "URP";
            if (name.Contains("HighDefinition")) return "HDRP";
            return name;
        }

        private static string FormatBuildTarget(BuildTarget target)
        {
            string str = target.ToString();
            switch (str)
            {
                case "StandaloneWindows": return "Windows (32-bit)";
                case "StandaloneWindows64": return "Windows (64-bit)";
                case "StandaloneOSX": return "macOS";
                case "StandaloneLinux64": return "Linux (64-bit)";
                case "Android": return "Android";
                case "iOS": return "iOS";
                case "WebGL": return "WebGL";
                case "WSAPlayer": return "UWP";
                case "Switch": return "Nintendo Switch";
                case "PS4": return "PlayStation 4";
                case "PS5": return "PlayStation 5";
                default:
                    if (str.Contains("XboxSeries")) return "Xbox Series X/S";
                    if (str.Contains("Xbox")) return "Xbox";
                    return str;
            }
        }

        private static string TruncateAndClean(string s, int maxLen = 128)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            string trimmed = s.Trim();
            if (trimmed.Length < 2) trimmed += " "; // Discord requires >= 2 characters if provided
            return trimmed.Length <= maxLen ? trimmed : trimmed.Substring(0, maxLen - 3) + "...";
        }

        private static string EscapeJson(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.Replace("\\", "\\\\")
                    .Replace("\"", "\\\"")
                    .Replace("\r", "")
                    .Replace("\n", "\\n");
        }

        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                _playModeStartTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            }
            RequestUpdate(immediate: true);
        }

        private static void OnPauseStateChanged(PauseState state)
        {
            RequestUpdate(immediate: true);
        }

        private static void OnSceneChanged(UnityEngine.SceneManagement.Scene prev, UnityEngine.SceneManagement.Scene current)
        {
            RequestUpdate(immediate: true);
        }

        private static void OnSceneOpened(UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode)
        {
            RequestUpdate(immediate: true);
        }

        private static void OnSceneSaved(UnityEngine.SceneManagement.Scene scene)
        {
            RequestUpdate(immediate: true);
        }

        private static void OnBeforeAssemblyReload()
        {
            try
            {
                if (_ipc.IsConnected)
                {
                    _ipc.ClearActivity(Process.GetCurrentProcess().Id);
                    _ipc.Close();
                }
            }
            catch
            {
                // Suppress on reload
            }
        }

        private static void OnEditorQuitting()
        {
            try
            {
                if (_ipc.IsConnected)
                {
                    _ipc.ClearActivity(Process.GetCurrentProcess().Id);
                    _ipc.Close();
                }
            }
            catch
            {
                // Suppress on exit
            }
        }

        // ──────────────────────────────────────────────────────────────────────────
        // Menu Commands
        // ──────────────────────────────────────────────────────────────────────────
        [MenuItem("Tools/Discord Rich Presence/Toggle Rich Presence", false, 1)]
        private static void MenuTogglePresence()
        {
            DiscordPresenceSettings.Enabled = !DiscordPresenceSettings.Enabled;
            RequestUpdate(immediate: true);
        }

        [MenuItem("Tools/Discord Rich Presence/Toggle Rich Presence", true)]
        private static bool MenuTogglePresenceValidate()
        {
            Menu.SetChecked("Tools/Discord Rich Presence/Toggle Rich Presence", DiscordPresenceSettings.Enabled);
            return true;
        }

        [MenuItem("Tools/Discord Rich Presence/Reconnect", false, 2)]
        private static void MenuReconnect()
        {
            Reconnect();
        }

        [MenuItem("Tools/Discord Rich Presence/Preferences...", false, 3)]
        private static void MenuOpenPreferences()
        {
            SettingsService.OpenUserPreferences("Preferences/Discord Rich Presence");
        }
    }

    /// <summary>
    /// Registers a settings GUI in Unity Preferences (Edit > Preferences > Discord Rich Presence).
    /// </summary>
    public static class DiscordPresencePreferences
    {
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new SettingsProvider("Preferences/Discord Rich Presence", SettingsScope.User)
            {
                label = "Discord Rich Presence",
                guiHandler = _ => DrawSettingsGUI(),
                keywords = new HashSet<string>(new[] { "Discord", "RPC", "Presence", "Rich", "Unity", "Status" })
            };
        }

        private static void DrawSettingsGUI()
        {
            EditorGUILayout.LabelField("Discord Rich Presence for Unity Editor", EditorStyles.boldLabel);
            EditorGUILayout.Space(4);

            string status = UnityDiscordRichPresence.IsConnected
                ? $"● Connected ({UnityDiscordRichPresence.ConnectedPipeName})"
                : "○ Disconnected (Discord desktop client not detected or disabled)";

            var statusStyle = new GUIStyle(EditorStyles.label);
            statusStyle.normal.textColor = UnityDiscordRichPresence.IsConnected ? new Color(0.2f, 0.85f, 0.35f) : Color.gray;
            EditorGUILayout.LabelField("Connection Status:", status, statusStyle);
            EditorGUILayout.Space(8);

            EditorGUI.BeginChangeCheck();

            bool enabled = EditorGUILayout.Toggle("Enable Rich Presence", DiscordPresenceSettings.Enabled);
            string appId = EditorGUILayout.TextField("Discord Application ID", DiscordPresenceSettings.ApplicationId);

            EditorGUILayout.Space(8);
            EditorGUILayout.LabelField("Display Options", EditorStyles.boldLabel);
            bool showProject = EditorGUILayout.Toggle("Show Project Name", DiscordPresenceSettings.ShowProjectName);
            bool showVersion = EditorGUILayout.Toggle("Show Project Version", DiscordPresenceSettings.ShowProjectVersion);
            bool showScene = EditorGUILayout.Toggle("Show Scene Name", DiscordPresenceSettings.ShowSceneName);
            bool showDirty = EditorGUILayout.Toggle("Show Unsaved Scene Indicator (●)", DiscordPresenceSettings.ShowDirtyIndicator);
            bool showPrefab = EditorGUILayout.Toggle("Show Prefab Stage Name", DiscordPresenceSettings.ShowPrefabName);
            bool showPlatform = EditorGUILayout.Toggle("Show Build Target / Platform", DiscordPresenceSettings.ShowPlatform);
            bool showUnity = EditorGUILayout.Toggle("Show Unity Engine Version", DiscordPresenceSettings.ShowUnityVersion);
            bool showRP = EditorGUILayout.Toggle("Show Render Pipeline (URP/HDRP)", DiscordPresenceSettings.ShowRenderPipeline);
            bool showTime = EditorGUILayout.Toggle("Show Working Time Elapsed", DiscordPresenceSettings.ShowWorkingTime);
            bool resetPlayTime = EditorGUILayout.Toggle("Reset Timer In Play Mode", DiscordPresenceSettings.ResetTimerOnPlayMode);

            EditorGUILayout.Space(8);
            EditorGUILayout.LabelField("Asset & Icon Keys", EditorStyles.boldLabel);
            bool autoVersion = EditorGUILayout.Toggle(new GUIContent("Auto Version Large Icon", "Auto-resolve large image to unity_6, unity_2023, unity_2022, etc. based on Application.unityVersion"), DiscordPresenceSettings.AutoDetectVersionIcon);
            string largeKey;
            if (autoVersion)
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUILayout.TextField("Large Image Key", $"{UnityDiscordRichPresence.GetVersionIconKey()} (Auto)");
                }
                largeKey = DiscordPresenceSettings.LargeImageKey;
            }
            else
            {
                largeKey = EditorGUILayout.TextField("Large Image Key", DiscordPresenceSettings.LargeImageKey);
            }

            string playKey = EditorGUILayout.TextField("Play Mode Icon Key", DiscordPresenceSettings.PlayModeImageKey);
            string pauseKey = EditorGUILayout.TextField("Pause Mode Icon Key", DiscordPresenceSettings.PauseModeImageKey);
            string editKey = EditorGUILayout.TextField("Edit Mode Icon Key", DiscordPresenceSettings.EditModeImageKey);
            string compileKey = EditorGUILayout.TextField("Compile Mode Icon Key", DiscordPresenceSettings.CompileModeImageKey);

            EditorGUILayout.Space(8);
            EditorGUILayout.LabelField("Custom Buttons (Optional)", EditorStyles.boldLabel);
            string btn1Label = EditorGUILayout.TextField("Button 1 Label", DiscordPresenceSettings.Button1Label);
            string btn1Url = EditorGUILayout.TextField("Button 1 URL", DiscordPresenceSettings.Button1Url);
            string btn2Label = EditorGUILayout.TextField("Button 2 Label", DiscordPresenceSettings.Button2Label);
            string btn2Url = EditorGUILayout.TextField("Button 2 URL", DiscordPresenceSettings.Button2Url);

            if (EditorGUI.EndChangeCheck())
            {
                DiscordPresenceSettings.Enabled = enabled;
                DiscordPresenceSettings.ApplicationId = appId;
                DiscordPresenceSettings.ShowProjectName = showProject;
                DiscordPresenceSettings.ShowProjectVersion = showVersion;
                DiscordPresenceSettings.ShowSceneName = showScene;
                DiscordPresenceSettings.ShowDirtyIndicator = showDirty;
                DiscordPresenceSettings.ShowPrefabName = showPrefab;
                DiscordPresenceSettings.ShowPlatform = showPlatform;
                DiscordPresenceSettings.ShowUnityVersion = showUnity;
                DiscordPresenceSettings.ShowRenderPipeline = showRP;
                DiscordPresenceSettings.ShowWorkingTime = showTime;
                DiscordPresenceSettings.ResetTimerOnPlayMode = resetPlayTime;
                DiscordPresenceSettings.AutoDetectVersionIcon = autoVersion;
                DiscordPresenceSettings.LargeImageKey = largeKey;
                DiscordPresenceSettings.PlayModeImageKey = playKey;
                DiscordPresenceSettings.PauseModeImageKey = pauseKey;
                DiscordPresenceSettings.EditModeImageKey = editKey;
                DiscordPresenceSettings.CompileModeImageKey = compileKey;
                DiscordPresenceSettings.Button1Label = btn1Label;
                DiscordPresenceSettings.Button1Url = btn1Url;
                DiscordPresenceSettings.Button2Label = btn2Label;
                DiscordPresenceSettings.Button2Url = btn2Url;

                UnityDiscordRichPresence.RequestUpdate(immediate: true);
            }

            EditorGUILayout.Space(12);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Reconnect Now", GUILayout.Width(130)))
            {
                UnityDiscordRichPresence.Reconnect();
            }
            if (GUILayout.Button("Reset Defaults", GUILayout.Width(110)))
            {
                if (EditorUtility.DisplayDialog("Reset Discord RPC Settings", "Reset all Discord Rich Presence settings to defaults?", "Reset", "Cancel"))
                {
                    DiscordPresenceSettings.ResetToDefaults();
                    UnityDiscordRichPresence.RequestUpdate(immediate: true);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif
