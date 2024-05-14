using System;
using System.Threading;
using System.Threading.Tasks;

public abstract class BackgroundDataProvider:IDisposable
{
    private BackgroundData m_frameBackgroundData = new BackgroundData();
    private bool m_latest = false;
    object m_lockObj = new object();
    public bool IsRunning { get; set; } = false;
    private CancellationTokenSource kinectConnectionTokenSource;
    private CancellationToken _token;
    private int providerId;

    public BackgroundDataProvider(int id)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.quitting += OnEditorClose;
#endif
        providerId = id;
        kinectConnectionTokenSource = new CancellationTokenSource();
        ConnectDevice();
    }

    protected void ReConnectDevice()
    {
        kinectConnectionTokenSource.Cancel();
        ConnectDevice();
    }

    private void ConnectDevice()
    {
        _token = kinectConnectionTokenSource.Token;
        Task.Run(() => RunBackgroundThreadAsync(providerId, _token));
    }

    private void OnEditorClose()
    {
        Dispose();
    }

    protected abstract void RunBackgroundThreadAsync(int id, CancellationToken token);

    public void SetCurrentFrameData(ref BackgroundData currentFrameData)
    {
        lock (m_lockObj)
        {
            var temp = currentFrameData;
            currentFrameData = m_frameBackgroundData;
            m_frameBackgroundData = temp;
            m_latest = true;
        }
    }

    public bool GetCurrentFrameData(ref BackgroundData dataBuffer)
    {
        lock (m_lockObj)
        {
            var temp = dataBuffer;
            dataBuffer = m_frameBackgroundData;
            m_frameBackgroundData = temp;
            bool result = m_latest;
            m_latest = false;
            return result;
        }
    }

    public void Dispose()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.quitting -= OnEditorClose;
#endif
        kinectConnectionTokenSource?.Cancel();
        kinectConnectionTokenSource?.Dispose();
        kinectConnectionTokenSource = null;
    }
}
